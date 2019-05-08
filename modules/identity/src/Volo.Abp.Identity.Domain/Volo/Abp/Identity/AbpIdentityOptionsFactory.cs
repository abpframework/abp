using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Options;
using Volo.Abp.Settings;

namespace Volo.Abp.Identity
{
    public class AbpIdentityOptionsFactory : AbpOptionsFactory<IdentityOptions>
    {
        private readonly ISettingProvider _settingProvider;

        public AbpIdentityOptionsFactory(
            IEnumerable<IConfigureOptions<IdentityOptions>> setups,
            IEnumerable<IPostConfigureOptions<IdentityOptions>> postConfigures,
            ISettingProvider settingProvider)
            : base(setups, postConfigures)
        {
            _settingProvider = settingProvider;
        }

        public override IdentityOptions Create(string name)
        {
            var options = base.Create(name);

            OverrideOptions(options);

            return options;
        }

        protected virtual void OverrideOptions(IdentityOptions options)
        {
            
            options.Password.RequiredLength = _settingProvider.Get(IdentitySettingNames.Password.RequiredLength, options.Password.RequiredLength);
            options.Password.RequiredUniqueChars = _settingProvider.Get(IdentitySettingNames.Password.RequiredUniqueChars, options.Password.RequiredUniqueChars);
            options.Password.RequireNonAlphanumeric = _settingProvider.Get(IdentitySettingNames.Password.RequireNonAlphanumeric, options.Password.RequireNonAlphanumeric);
            options.Password.RequireLowercase = _settingProvider.Get(IdentitySettingNames.Password.RequireLowercase, options.Password.RequireLowercase);
            options.Password.RequireUppercase = _settingProvider.Get(IdentitySettingNames.Password.RequireUppercase, options.Password.RequireUppercase);
            options.Password.RequireDigit = _settingProvider.Get(IdentitySettingNames.Password.RequireDigit, options.Password.RequireDigit);

            options.Lockout.AllowedForNewUsers = _settingProvider.Get(IdentitySettingNames.Lockout.AllowedForNewUsers, options.Lockout.AllowedForNewUsers);
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(_settingProvider.Get(IdentitySettingNames.Lockout.LockoutDuration, options.Lockout.DefaultLockoutTimeSpan.TotalSeconds.To<int>()));
            options.Lockout.MaxFailedAccessAttempts = _settingProvider.Get(IdentitySettingNames.Lockout.MaxFailedAccessAttempts, options.Lockout.MaxFailedAccessAttempts);

            options.SignIn.RequireConfirmedEmail = _settingProvider.Get(IdentitySettingNames.SignIn.RequireConfirmedEmail, options.SignIn.RequireConfirmedEmail);
            options.SignIn.RequireConfirmedPhoneNumber = _settingProvider.Get(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber, options.SignIn.RequireConfirmedPhoneNumber);
        }
    }
}