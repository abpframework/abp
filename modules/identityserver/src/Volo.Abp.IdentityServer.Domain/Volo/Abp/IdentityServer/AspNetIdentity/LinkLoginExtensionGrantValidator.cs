using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.IdentityServer.AspNetIdentity
{
    public class LinkLoginExtensionGrantValidator : IExtensionGrantValidator
    {
        public const string ExtensionGrantType = "LinkLogin";

        public string GrantType => ExtensionGrantType;

        protected ITokenValidator TokenValidator  { get; }
        protected IdentityLinkUserManager IdentityLinkUserManager { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected ICurrentUser CurrentUser { get; }
        protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; }
        protected IdentityUserManager UserManager { get; }
        protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }
        protected ILogger<LinkLoginExtensionGrantValidator> Logger { get; }
        protected IStringLocalizer<AbpIdentityServerResource> Localizer { get; }

        public LinkLoginExtensionGrantValidator(
            ITokenValidator tokenValidator,
            IdentityLinkUserManager identityLinkUserManager,
            ICurrentTenant currentTenant,
            ICurrentUser currentUser,
            IdentityUserManager userManager,
            ICurrentPrincipalAccessor currentPrincipalAccessor,
            IdentitySecurityLogManager identitySecurityLogManager,
            ILogger<LinkLoginExtensionGrantValidator> logger,
            IStringLocalizer<AbpIdentityServerResource> localizer)
        {
            TokenValidator = tokenValidator;
            IdentityLinkUserManager = identityLinkUserManager;
            CurrentTenant = currentTenant;
            CurrentUser = currentUser;
            UserManager = userManager;
            CurrentPrincipalAccessor = currentPrincipalAccessor;
            IdentitySecurityLogManager = identitySecurityLogManager;
            Logger = logger;
            Localizer = localizer;
        }

        public virtual async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var accessToken = context.Request.Raw["access_token"];
            if (accessToken.IsNullOrWhiteSpace())
            {
                context.Result = new GrantValidationResult
                {
                    IsError = true,
                    Error = "invalid_access_token"
                };
                return;
            }

            var result = await TokenValidator.ValidateAccessTokenAsync(accessToken);
            if (result.IsError)
            {
                context.Result = new GrantValidationResult
                {
                    IsError = true,
                    Error = result.Error,
                    ErrorDescription = result.ErrorDescription
                };
                return;
            }

            using (CurrentPrincipalAccessor.Change(result.Claims))
            {
                if (!Guid.TryParse(context.Request.Raw["LinkUserId"], out var linkUserId))
                {
                    context.Result = new GrantValidationResult
                    {
                        IsError = true,
                        Error = "invalid_link_user_id"
                    };
                    return;
                }

                Guid? linkTenantId = null;
                if (!context.Request.Raw["LinkTenantId"].IsNullOrWhiteSpace())
                {
                    if (!Guid.TryParse(context.Request.Raw["LinkTenantId"], out var parsedGuid))
                    {
                        context.Result = new GrantValidationResult
                        {
                            IsError = true,
                            Error = "invalid_link_tenant_id"
                        };
                        return;
                    }

                    linkTenantId = parsedGuid;
                }

                var isLinked = await IdentityLinkUserManager.IsLinkedAsync(
                    new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentTenant.Id),
                    new IdentityLinkUserInfo(linkUserId, linkTenantId),
                    true);

                if (isLinked)
                {
                    using (CurrentTenant.Change(linkTenantId))
                    {
                        var user = await UserManager.GetByIdAsync(linkUserId);
                        var sub = await UserManager.GetUserIdAsync(user);

                        var additionalClaims = new List<Claim>();
                        await AddCustomClaimsAsync(additionalClaims, user, context);

                        context.Result = new GrantValidationResult(
                            sub,
                            GrantType,
                            additionalClaims.ToArray()
                        );
                    }
                }
                else
                {
                    context.Result = new GrantValidationResult
                    {
                        IsError = true,
                        Error = Localizer["TheTargetUserIsNotLinkedToYou"]
                    };
                }
            }
        }

        protected virtual Task AddCustomClaimsAsync(List<Claim> customClaims, IdentityUser user, ExtensionGrantValidationContext context)
        {
            if (user.TenantId.HasValue)
            {
                customClaims.Add(new Claim(AbpClaimTypes.TenantId, user.TenantId?.ToString()));
            }

            return Task.CompletedTask;
        }
    }
}
