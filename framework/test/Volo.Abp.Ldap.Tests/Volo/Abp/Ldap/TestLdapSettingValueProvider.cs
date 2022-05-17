using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace Volo.Abp.Ldap;

public class TestLdapSettingValueProvider : ISettingValueProvider, ITransientDependency
{
    public const string ProviderName = "Test";

    public string Name => ProviderName;

    public Task<string> GetOrNullAsync(SettingDefinition setting)
    {
        switch (setting.Name)
        {
            case LdapSettingNames.ServerHost:
                return Task.FromResult("localhost");

            case LdapSettingNames.ServerPort:
                return Task.FromResult("389");

            case LdapSettingNames.BaseDc:
                return Task.FromResult("dc=abp,dc=io");

            case LdapSettingNames.Domain:
                return Task.FromResult<string>(null);

            case LdapSettingNames.UserName:
                return Task.FromResult("admin");

            case LdapSettingNames.Password:
                return Task.FromResult("123qwe");

            default:
                return Task.FromResult<string>(null);
        }
    }

    public Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings)
    {
        throw new NotImplementedException();
    }
}
