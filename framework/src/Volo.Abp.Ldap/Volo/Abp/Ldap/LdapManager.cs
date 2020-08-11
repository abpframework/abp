using System;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Ldap
{
    public class LdapManager : ILdapManager, ITransientDependency
    {
        public ILogger<LdapManager> Logger { get; set; }
        protected AbpLdapOptions LdapOptions { get; }

        public LdapManager(IOptions<AbpLdapOptions> ldapSettingsOptions)
        {
            LdapOptions = ldapSettingsOptions.Value;

            Logger = NullLogger<LdapManager>.Instance;
        }

        public bool Authenticate(string username, string password)
        {
            try
            {
                using (var conn = CreateLdapConnection())
                {
                    AuthenticateLdapConnection(conn, username, password);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }

        protected virtual ILdapConnection CreateLdapConnection()
        {
            var ldapConnection = new LdapConnection();
            ConfigureLdapConnection(ldapConnection);
            ldapConnection.Connect(LdapOptions.ServerHost, LdapOptions.ServerPort);
            return ldapConnection;
        }

        protected virtual void ConfigureLdapConnection(ILdapConnection connection)
        {

        }

        protected virtual void AuthenticateLdapConnection(ILdapConnection connection, string username, string password)
        {
            connection.Bind(username, password);
        }
    }
}
