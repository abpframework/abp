using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Ldap
{
    public class LdapTestData : ISingletonDependency
    {
        public string AdministratorName { get; } = "Administrator";
        public string AdministratorPassword { get; } = "yH.20190528";
        public string AdministratorDistinguishedName { get; } = "CN=Administrator,CN=Users,DC=yourdomain,DC=com,DC=cn";
        public string AdministratorDomainName { get; } = "Administrator@yourdomain.com.cn";

        public string DomainControllersName = "Domain Controllers";
        public string DomainControllersDistinguishedName = "OU=Domain Controllers,DC=yourdomain,DC=com,DC=cn";

        public string RootDistinguishedName { get; } = "DC=yourdomain,DC=com,DC=cn";

        public string Organization001Name { get; } = "Test_A";

        public string Test001Name { get; } = "test001";
        public string Test001Password { get; } = "yH.20190528";
        public string Test001Email { get; } = "test001@yourdomain.com.cn";

        public string Test002Name { get; } = "test002";
        public string Test002Password { get; } = "yH.20190528";
        public string Test002WrongPassword { get; } = "yH.20190529";
    }
}