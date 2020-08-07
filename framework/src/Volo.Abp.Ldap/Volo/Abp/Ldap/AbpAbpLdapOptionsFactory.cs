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
            options.ServerHost = await SettingProvider.GetOrNullAsync(LdapSettingNames.ServerHost) ?? options.ServerHost;
            options.ServerPort = await SettingProvider.GetAsync(LdapSettingNames.ServerPort, options.ServerPort);
            options.UseSsl = await SettingProvider.GetAsync(LdapSettingNames.UseSsl, options.UseSsl);
            options.SearchBase = await SettingProvider.GetOrNullAsync(LdapSettingNames.SearchBase) ?? options.SearchBase;
            options.DomainName = await SettingProvider.GetOrNullAsync(LdapSettingNames.DomainName) ?? options.DomainName;
            options.DomainDistinguishedName = await SettingProvider.GetOrNullAsync(LdapSettingNames.DomainDistinguishedName) ?? options.DomainDistinguishedName;
            options.Credentials.DomainUserName = await SettingProvider.GetOrNullAsync(LdapSettingNames.Credentials.DomainUserName) ?? options.Credentials.DomainUserName;
            options.Credentials.Password = await SettingProvider.GetOrNullAsync(LdapSettingNames.Credentials.Password) ?? options.Credentials.Password;
        }
    }
}
