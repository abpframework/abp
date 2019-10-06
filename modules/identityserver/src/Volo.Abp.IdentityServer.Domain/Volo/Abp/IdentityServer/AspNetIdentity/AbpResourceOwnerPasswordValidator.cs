using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.IdentityServer.AspNetIdentity
{
    public class AbpResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEventService _events;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ResourceOwnerPasswordValidator<IdentityUser>> _logger;

        public AbpResourceOwnerPasswordValidator(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEventService events,
            ILogger<ResourceOwnerPasswordValidator<IdentityUser>> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _events = events;
            _logger = logger;
        }

        /// <summary>
        /// https://github.com/IdentityServer/IdentityServer4/blob/master/src/AspNetIdentity/src/ResourceOwnerPasswordValidator.cs#L53
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            await ReplaceEmailToUsernameOfInputIfNeeds(context);

            var user = await _userManager.FindByNameAsync(context.UserName);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, context.Password, true);
                if (result.Succeeded)
                {
                    var sub = await _userManager.GetUserIdAsync(user);

                    _logger.LogInformation("Credentials validated for username: {username}", context.UserName);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, sub, context.UserName, interactive: false));

                    var additionalClaims = new List<Claim>();

                    await AddCustomClaimsAsync(additionalClaims, user, context);

                    context.Result = new GrantValidationResult(
                        sub,
                        OidcConstants.AuthenticationMethods.Password,
                        additionalClaims.ToArray()
                    );

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

        protected virtual async Task ReplaceEmailToUsernameOfInputIfNeeds(ResourceOwnerPasswordValidationContext context)
        {
            if (!ValidationHandler.IsValidEmailAddress(context.UserName))
            {
                return;
            }

            var userByUsername = await _userManager.FindByNameAsync(context.UserName);
            if (userByUsername != null)
            {
                return;
            }

            var userByEmail = await _userManager.FindByEmailAsync(context.UserName);
            if (userByEmail == null)
            {
                return;
            }

            context.UserName = userByEmail.UserName;
        }

        protected virtual Task AddCustomClaimsAsync(List<Claim> customClaims, IdentityUser user, ResourceOwnerPasswordValidationContext context)
        {
            if (user.TenantId.HasValue)
            {
                customClaims.Add(new Claim(AbpClaimTypes.TenantId, user.TenantId?.ToString()));
            }

            return Task.CompletedTask;
        }
    }
}
