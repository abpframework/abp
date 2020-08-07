using Volo.Abp.Modularity;

namespace Volo.Abp.Ldap
{
    [DependsOn(typeof(AbpLdapModule))]
    public class AbpLdapTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // not use ssl
            // "LDAP": {
            //     "ServerHost": "192.168.101.54",
            //     "ServerPort": 389,
            //     "UseSSL": false,
            //     "Credentials": {
            //         "DomainUserName": "administrator@yourdomain.com.cn",
            //         "Password": "yH.20190528"
            //     },
            //     "SearchBase": "CN=Users,DC=yourdomain,DC=com,DC=cn",
            //     "DomainName": "yourdomain.com.cn",
            //     "DomainDistinguishedName": "DC=yourdomain,DC=com,DC=cn"
            // }

            // use ssl
            // "LDAP": {
            //     "ServerHost": "192.168.101.54",
            //     "ServerPort": 636,
            //     "UseSSL": true,
            //     "Credentials": {
            //         "DomainUserName": "administrator@yourdomain.com.cn",
            //         "Password": "yH.20190528"
            //     },
            //     "SearchBase": "CN=Users,DC=yourdomain,DC=com,DC=cn",
            //     "DomainName": "yourdomain.com.cn",
            //     "DomainDistinguishedName": "DC=yourdomain,DC=com,DC=cn"
            // }

            Configure<AbpLdapOptions>(settings =>
            {
                settings.ServerHost = "192.168.101.54";
                settings.ServerPort = 636;
                settings.UseSsl = true;
                settings.Credentials.DomainUserName = "administrator@yourdomain.com.cn";
                settings.Credentials.Password = "yH.20190528";
                settings.SearchBase = "DC=yourdomain,DC=com,DC=cn";
                settings.DomainName = "yourdomain.com.cn";
                settings.DomainDistinguishedName = "DC=yourdomain,DC=com,DC=cn";
            });
        }
    }
}
