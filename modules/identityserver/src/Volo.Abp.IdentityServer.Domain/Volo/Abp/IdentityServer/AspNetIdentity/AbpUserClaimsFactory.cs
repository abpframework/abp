using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Volo.Abp.IdentityServer.AspNetIdentity
{
    public class AbpUserClaimsFactory<TUser> : IUserClaimsPrincipalFactory<TUser>
        where TUser : class
    {
        private readonly IObjectAccessor<IUserClaimsPrincipalFactory<TUser>> _inner;
        private readonly UserManager<TUser> _userManager;

        public AbpUserClaimsFactory(IObjectAccessor<IUserClaimsPrincipalFactory<TUser>> inner,
            UserManager<TUser> userManager)
        {
            _inner = inner;
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            var principal = await _inner.Value.CreateAsync(user);
            var identity = principal.Identities.First();

            if (!identity.HasClaim(x => x.Type == JwtClaimTypes.Subject))
            {
                var sub = await _userManager.GetUserIdAsync(user);
                identity.AddIfNotContains(new Claim(JwtClaimTypes.Subject, sub));
            }

            var username = await _userManager.GetUserNameAsync(user);
            var usernameClaim = identity.FindFirst(claim =>
                claim.Type == _userManager.Options.ClaimsIdentity.UserNameClaimType && claim.Value == username);
            if (usernameClaim != null)
            {
                identity.RemoveClaim(usernameClaim);
                identity.AddIfNotContains(new Claim(JwtClaimTypes.PreferredUserName, username));
            }

            if (!identity.HasClaim(x => x.Type == JwtClaimTypes.Name))
            {
                identity.AddIfNotContains(new Claim(JwtClaimTypes.Name, username));
            }

            if (_userManager.SupportsUserEmail)
            {
                var email = await _userManager.GetEmailAsync(user);
                if (!string.IsNullOrWhiteSpace(email))
                {
                    identity.AddIfNotContains(new Claim(JwtClaimTypes.Email, email));
                    identity.AddIfNotContains(new Claim(JwtClaimTypes.EmailVerified,
                        await _userManager.IsEmailConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean));
                }
            }

            if (_userManager.SupportsUserPhoneNumber)
            {
                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                if (!string.IsNullOrWhiteSpace(phoneNumber))
                {
                    identity.AddIfNotContains(new Claim(JwtClaimTypes.PhoneNumber, phoneNumber));
                    identity.AddIfNotContains(new Claim(JwtClaimTypes.PhoneNumberVerified,
                        await _userManager.IsPhoneNumberConfirmedAsync(user) ? "true" : "false",
                        ClaimValueTypes.Boolean));
                }
            }

            if (user is IdentityUser identityUser)
            {
                if (!identityUser.Name.IsNullOrEmpty())
                {
                    identity.AddIfNotContains(new Claim(JwtClaimTypes.GivenName, identityUser.Name));
                }

                if (!identityUser.Surname.IsNullOrEmpty())
                {
                    identity.AddIfNotContains(new Claim(JwtClaimTypes.FamilyName, identityUser.Surname));
                }
            }

            return principal;
        }
    }
}
