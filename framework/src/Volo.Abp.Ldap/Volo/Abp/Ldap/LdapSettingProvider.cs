using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace Volo.Abp.Ldap;

public class LdapSettingProvider : ILdapSettingProvider, ITransientDependency
{
    protected ISettingProvider SettingProvider { get; }

    public LdapSettingProvider(ISettingProvider settingProvider)
    {
        SettingProvider = settingProvider;
    }

    public virtual async Task<bool> GetLdapOverSsl()
    {
        return (await SettingProvider.GetOrNullAsync(LdapSettingNames.Ldaps))?.To<bool>() ?? default;
    }

    public virtual async Task<string?> GetServerHostAsync()
    {
        return await SettingProvider.GetOrNullAsync(LdapSettingNames.ServerHost);
    }

    public virtual async Task<int> GetServerPortAsync()
    {
        return (await SettingProvider.GetOrNullAsync(LdapSettingNames.ServerPort))?.To<int>() ?? default;
    }

    public virtual async Task<string?> GetBaseDcAsync()
    {
        return await SettingProvider.GetOrNullAsync(LdapSettingNames.BaseDc);
    }

    public virtual async Task<string?> GetDomainAsync()
    {
        return await SettingProvider.GetOrNullAsync(LdapSettingNames.Domain);
    }

    public virtual async Task<string?> GetUserNameAsync()
    {
        return await SettingProvider.GetOrNullAsync(LdapSettingNames.UserName);
    }

    public virtual async Task<string?> GetPasswordAsync()
    {
        return await SettingProvider.GetOrNullAsync(LdapSettingNames.Password);
    }
}
