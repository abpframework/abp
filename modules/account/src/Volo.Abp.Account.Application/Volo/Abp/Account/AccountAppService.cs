﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Account.Emailing;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Settings;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.Settings;

namespace Volo.Abp.Account
{
    public class AccountAppService : ApplicationService, IAccountAppService
    {
        protected IIdentityRoleRepository RoleRepository { get; }
        protected IdentityUserManager UserManager { get; }
        protected IAccountEmailer AccountEmailer { get; }
        protected IdentitySecurityLogManager IdentitySecurityLogManager { get; }

        public AccountAppService(
            IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository,
            IAccountEmailer accountEmailer,
            IdentitySecurityLogManager identitySecurityLogManager)
        {
            RoleRepository = roleRepository;
            AccountEmailer = accountEmailer;
            IdentitySecurityLogManager = identitySecurityLogManager;
            UserManager = userManager;
            LocalizationResource = typeof(AccountResource);
        }

        public virtual async Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            await CheckSelfRegistrationAsync();

            var user = new IdentityUser(GuidGenerator.Create(), input.UserName, input.EmailAddress, CurrentTenant.Id);

            (await UserManager.CreateAsync(user, input.Password)).CheckErrors();

            await UserManager.SetEmailAsync(user,input.EmailAddress);
            await UserManager.AddDefaultRolesAsync(user);

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        public virtual async Task SendPasswordResetCodeAsync(SendPasswordResetCodeDto input)
        {
            var user = await GetUserByEmail(input.Email);
            var resetToken = await UserManager.GeneratePasswordResetTokenAsync(user);
            await AccountEmailer.SendPasswordResetLinkAsync(user, resetToken, input.AppName, input.ReturnUrl, input.ReturnUrlHash);
        }

        public virtual async Task ResetPasswordAsync(ResetPasswordDto input)
        {
            var user = await UserManager.GetByIdAsync(input.UserId);
            (await UserManager.ResetPasswordAsync(user, input.ResetToken, input.Password)).CheckErrors();

            await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext
            {
                Identity = IdentitySecurityLogIdentityConsts.Identity,
                Action = IdentitySecurityLogActionConsts.ChangePassword
            });
        }

        protected virtual async Task<IdentityUser> GetUserByEmail(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserFriendlyException(L["Volo.Account:InvalidEmailAddress", email]);
            }

            return user;
        }

        protected virtual async Task CheckSelfRegistrationAsync()
        {
            if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled))
            {
                throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
            }
        }
    }
}
