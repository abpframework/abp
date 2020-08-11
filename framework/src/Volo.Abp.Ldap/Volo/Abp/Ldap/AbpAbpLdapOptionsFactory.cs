using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Options;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace Volo.Abp.Ldap
{
    public class AbpAbpLdapOptionsFactory : AbpOptionsFactory<AbpLdapOptions>
    {
        protected ISettingProvider SettingProvider { get; }

        public AbpAbpLdapOptionsFactory(
            IEnumerable<IConfigureOptions<AbpLdapOptions>> setups,
            IEnumerable<IPostConfigureOptions<AbpLdapOptions>> postConfigures,
            ISettingProvider settingProvider)
            : base(setups, postConfigures)
        {
            SettingProvider = settingProvider;
        }

        public override AbpLdapOptions Create(string name)
        {
            var options = base.Create(name);

            AsyncHelper.RunSync(() => OverrideOptionsAsync(options));

            return options;
        }

        protected virtual async Task OverrideOptionsAsync(AbpLdapOptions options)
        {
            options.ServerHost = await GetSettingOrDefaultValue(LdapSettingNames.ServerHost, options.ServerHost);
            options.ServerPort = await SettingProvider.GetAsync(LdapSettingNames.ServerPort, options.ServerPort);
            options.BaseDc = await GetSettingOrDefaultValue(LdapSettingNames.BaseDc, options.BaseDc);
            options.UserName = await GetSettingOrDefaultValue(LdapSettingNames.UserName, options.UserName);
            options.Password = await GetSettingOrDefaultValue(LdapSettingNames.Password, options.Password);
        }

        protected virtual async Task<string> GetSettingOrDefaultValue(string name, string defaultValue)
        {
            var value = await SettingProvider.GetOrNullAsync(name);
            return value.IsNullOrWhiteSpace() ? defaultValue : value;
        }
    }
}
