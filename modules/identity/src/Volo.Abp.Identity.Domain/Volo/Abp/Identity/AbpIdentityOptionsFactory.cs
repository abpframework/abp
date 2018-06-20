using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Json;
using Volo.Abp.Options;
using Volo.Abp.Settings;

namespace Volo.Abp.Identity
{
    public class AbpIdentityOptionsFactory : AbpOptionsFactory<IdentityOptions>
    {
        private readonly ISettingManager _settingManager;
        private readonly IJsonSerializer _jsonSerializer;

        public AbpIdentityOptionsFactory(
            IEnumerable<IConfigureOptions<IdentityOptions>> setups,
            IEnumerable<IPostConfigureOptions<IdentityOptions>> postConfigures,
            ISettingManager settingManager,
            IJsonSerializer jsonSerializer)
            : base(setups, postConfigures)
        {
            _settingManager = settingManager;
            _jsonSerializer = jsonSerializer;
        }

        public override IdentityOptions Create(string name)
        {
            var options = base.Create(name);

            SetPasswordOptions(options);

            return options;
        }

        protected virtual void SetPasswordOptions(IdentityOptions options)
        {
            options.Password.RequiredLength = _settingManager.Get(IdentitySettingNames.Password.RequiredLength, options.Password.RequiredLength);
            options.Password.RequiredUniqueChars = _settingManager.Get(IdentitySettingNames.Password.RequiredUniqueChars, options.Password.RequiredUniqueChars);
            options.Password.RequireNonAlphanumeric = _settingManager.Get(IdentitySettingNames.Password.RequireNonAlphanumeric, options.Password.RequireNonAlphanumeric);
            options.Password.RequireLowercase = _settingManager.Get(IdentitySettingNames.Password.RequireLowercase, options.Password.RequireLowercase);
            options.Password.RequireUppercase = _settingManager.Get(IdentitySettingNames.Password.RequireUppercase, options.Password.RequireUppercase);
            options.Password.RequireDigit = _settingManager.Get(IdentitySettingNames.Password.RequireDigit, options.Password.RequireDigit);

            options.Lockout.AllowedForNewUsers = _settingManager.Get(IdentitySettingNames.Lockout.AllowedForNewUsers, options.Lockout.AllowedForNewUsers);
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(_settingManager.Get(IdentitySettingNames.Lockout.LockoutDuration, options.Lockout.DefaultLockoutTimeSpan.TotalSeconds.To<int>()));
            options.Lockout.MaxFailedAccessAttempts = _settingManager.Get(IdentitySettingNames.Lockout.MaxFailedAccessAttempts, options.Lockout.MaxFailedAccessAttempts);

            options.SignIn.RequireConfirmedEmail = _settingManager.Get(IdentitySettingNames.SignIn.RequireConfirmedEmail, options.SignIn.RequireConfirmedEmail);
            options.SignIn.RequireConfirmedPhoneNumber = _settingManager.Get(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber, options.SignIn.RequireConfirmedPhoneNumber);
        }
    }
}