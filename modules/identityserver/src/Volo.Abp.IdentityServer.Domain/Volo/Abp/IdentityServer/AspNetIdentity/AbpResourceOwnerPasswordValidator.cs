using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;

namespace Volo.Abp.IdentityServer.AspNetIdentity
{
    public class AbpResourceOwnerPasswordValidator : ResourceOwnerPasswordValidator<IdentityUser>
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly ILogger<ResourceOwnerPasswordValidator<IdentityUser>> _logger;
        private readonly IdentityUserManager _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEventService _events;
        public AbpResourceOwnerPasswordValidator(
            IdentityUserManager userManager,
            SignInManager<IdentityUser> signInManager,
            IEventService events,
            ILogger<ResourceOwnerPasswordValidator<IdentityUser>> logger, ICurrentTenant currentTenant) : base(
                userManager,
                signInManager,
                events,
                logger)
        {
            _logger = logger;
            _currentTenant = currentTenant;
            _events = events;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // [UnitOfWork]
        //public override async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        //{
        //    await base.ValidateAsync(context);

        //}

        [UnitOfWork]
        public override async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            string tenantId = context.Request.Raw["__tenant"];
            if (!tenantId.IsNullOrEmpty())
            {
                _logger.LogInformation($"currentTenantId:{_currentTenant.Id}");
                _currentTenant.Change(Guid.Parse(tenantId), "tenant1");
            }
            var user = await _userManager.FindByNameAsync(context.UserName);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, context.Password, true);
                if (result.Succeeded)
                {
                    var sub = await _userManager.GetUserIdAsync(user);

                    _logger.LogInformation("Credentials validated for username: {username}", context.UserName);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, sub, context.UserName, interactive: false));

                    context.Result = new GrantValidationResult(sub, OidcConstants.AuthenticationMethods.Password, GetAdditionalClaimsOrNull(user));
                    return;
                }
                else if (result.IsLockedOut)
                {
                    _logger.LogInformation("Authentication failed for username: {username}, reason: locked out", context.UserName);
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "locked out", interactive: false));
                }
                else if (result.IsNotAllowed)
                {
                    _logger.LogInformation("Authentication failed for username: {username}, reason: not allowed", context.UserName);
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "not allowed", interactive: false));
                }
                else
                {
                    _logger.LogInformation("Authentication failed for username: {username}, reason: invalid credentials", context.UserName);
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid credentials", interactive: false));
                }
            }
            else
            {
                _logger.LogInformation("No user found matching username: {username}", context.UserName);
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username", interactive: false));
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
        }

        protected virtual IEnumerable<Claim> GetAdditionalClaimsOrNull(IdentityUser user)
        {
            if (!user.TenantId.HasValue)
            {
                return null;
            }

            return new[] { new Claim(AbpClaimTypes.TenantId, user.TenantId?.ToString()) };
        }
    }
}
