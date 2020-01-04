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
            AsyncHelper.RunSync(()=>OverrideOptionsAsync(options));
        }

        protected virtual async Task OverrideOptionsAsync(IdentityOptions options)
        {
            options.Password.RequiredLength = await _settingProvider.GetAsync(IdentitySettingNames.Password.RequiredLength, options.Password.RequiredLength).ConfigureAwait(false);
            options.Password.RequiredUniqueChars = await _settingProvider.GetAsync(IdentitySettingNames.Password.RequiredUniqueChars, options.Password.RequiredUniqueChars).ConfigureAwait(false);
            options.Password.RequireNonAlphanumeric = await _settingProvider.GetAsync(IdentitySettingNames.Password.RequireNonAlphanumeric, options.Password.RequireNonAlphanumeric).ConfigureAwait(false);
            options.Password.RequireLowercase = await _settingProvider.GetAsync(IdentitySettingNames.Password.RequireLowercase, options.Password.RequireLowercase).ConfigureAwait(false);
            options.Password.RequireUppercase = await _settingProvider.GetAsync(IdentitySettingNames.Password.RequireUppercase, options.Password.RequireUppercase).ConfigureAwait(false);
            options.Password.RequireDigit = await _settingProvider.GetAsync(IdentitySettingNames.Password.RequireDigit, options.Password.RequireDigit).ConfigureAwait(false);

            options.Lockout.AllowedForNewUsers = await _settingProvider.GetAsync(IdentitySettingNames.Lockout.AllowedForNewUsers, options.Lockout.AllowedForNewUsers).ConfigureAwait(false);
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(await _settingProvider.GetAsync(IdentitySettingNames.Lockout.LockoutDuration, options.Lockout.DefaultLockoutTimeSpan.TotalSeconds.To<int>()));
            options.Lockout.MaxFailedAccessAttempts = await _settingProvider.GetAsync(IdentitySettingNames.Lockout.MaxFailedAccessAttempts, options.Lockout.MaxFailedAccessAttempts).ConfigureAwait(false);

            options.SignIn.RequireConfirmedEmail = await _settingProvider.GetAsync(IdentitySettingNames.SignIn.RequireConfirmedEmail, options.SignIn.RequireConfirmedEmail).ConfigureAwait(false);
            options.SignIn.RequireConfirmedPhoneNumber = await _settingProvider.GetAsync(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber, options.SignIn.RequireConfirmedPhoneNumber).ConfigureAwait(false);

        }
    }
}