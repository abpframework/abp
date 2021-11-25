using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.Options;
using Volo.Abp.Settings;

namespace Volo.Abp.Ldap;

public class AbpAbpLdapOptionsManager : AbpDynamicOptionsManager<AbpLdapOptions>
{
    protected ISettingProvider SettingProvider { get; }

    public AbpAbpLdapOptionsManager(IOptionsFactory<AbpLdapOptions> factory, ISettingProvider settingProvider)
        : base(factory)
    {
        SettingProvider = settingProvider;
    }

    protected override async Task OverrideOptionsAsync(string name, AbpLdapOptions options)
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
