using System;
using System.Threading.Tasks;
using LdapForNet;
using LdapForNet.Native;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Ldap;

public class LdapManager : ILdapManager, ITransientDependency
{
    public ILogger<LdapManager> Logger { get; set; }
    protected ILdapSettingProvider LdapSettingProvider { get; }

    public LdapManager(ILdapSettingProvider ldapSettingProvider)
    {
        LdapSettingProvider = ldapSettingProvider;
        Logger = NullLogger<LdapManager>.Instance;
    }

    public virtual async Task<bool> AuthenticateAsync(string username, string password)
    {
        try
        {
            using (var conn = await CreateLdapConnectionAsync())
            {
                await AuthenticateLdapConnectionAsync(conn, username, password);
                return true;
            }
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            return false;
        }
    }

    protected virtual async Task<ILdapConnection> CreateLdapConnectionAsync()
    {
        var ldapConnection = new LdapConnection();
        await ConfigureLdapConnectionAsync(ldapConnection);
        await ConnectAsync(ldapConnection);
        return ldapConnection;
    }

    protected virtual Task ConfigureLdapConnectionAsync(ILdapConnection ldapConnection)
    {
        return Task.CompletedTask;
    }

    protected virtual async Task ConnectAsync(ILdapConnection ldapConnection)
    {
        ldapConnection.Connect(await LdapSettingProvider.GetServerHostAsync(), await LdapSettingProvider.GetServerPortAsync());
    }

    protected virtual async Task AuthenticateLdapConnectionAsync(ILdapConnection connection, string username, string password)
    {
        await connection.BindAsync(Native.LdapAuthType.Simple, new LdapCredential()
        {
            UserName = username,
            Password = password
        });
    }
}
