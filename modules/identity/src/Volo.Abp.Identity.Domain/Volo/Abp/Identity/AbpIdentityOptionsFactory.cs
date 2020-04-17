using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Options;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity
{
    public class AbpIdentityOptionsFactory : AbpOptionsFactory<IdentityOptions>
    {
        protected ISettingProvider SettingProvider { get; }

        public AbpIdentityOptionsFactory(
            IEnumerable<IConfigureOptions<IdentityOptions>> setups,
            IEnumerable<IPostConfigureOptions<IdentityOptions>> postConfigures,
            ISettingProvider settingProvider)
            : base(setups, postConfigures)
        {
            SettingProvider = settingProvider;
        }

        public override IdentityOptions Create(string name)
        {
            var options = base.Create(name);

            OverrideOptions(options);

            return options;
        }

        protected virtual void OverrideOptions(IdentityOptions options)
        {
            AsyncHelper.RunSync(()=>OverrideOptionsAsync(options));
        }

        protected virtual async Task OverrideOptionsAsync(IdentityOptions options)
        {
            options.Password.RequiredLength = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequiredLength, options.Password.RequiredLength);
            options.Password.RequiredUniqueChars = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequiredUniqueChars, options.Password.RequiredUniqueChars);
            options.Password.RequireNonAlphanumeric = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequireNonAlphanumeric, options.Password.RequireNonAlphanumeric);
            options.Password.RequireLowercase = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequireLowercase, options.Password.RequireLowercase);
            options.Password.RequireUppercase = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequireUppercase, options.Password.RequireUppercase);
            options.Password.RequireDigit = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequireDigit, options.Password.RequireDigit);

            options.Lockout.AllowedForNewUsers = await SettingProvider.GetAsync(IdentitySettingNames.Lockout.AllowedForNewUsers, options.Lockout.AllowedForNewUsers);
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(await SettingProvider.GetAsync(IdentitySettingNames.Lockout.LockoutDuration, options.Lockout.DefaultLockoutTimeSpan.TotalSeconds.To<int>()));
            options.Lockout.MaxFailedAccessAttempts = await SettingProvider.GetAsync(IdentitySettingNames.Lockout.MaxFailedAccessAttempts, options.Lockout.MaxFailedAccessAttempts);

            options.SignIn.RequireConfirmedEmail = await SettingProvider.GetAsync(IdentitySettingNames.SignIn.RequireConfirmedEmail, options.SignIn.RequireConfirmedEmail);
            options.SignIn.RequireConfirmedPhoneNumber = await SettingProvider.GetAsync(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber, options.SignIn.RequireConfirmedPhoneNumber);

        }
    }
}