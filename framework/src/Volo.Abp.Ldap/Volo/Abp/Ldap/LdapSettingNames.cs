namespace Volo.Abp.Ldap
{
    public static class LdapSettingNames
    {
        public const string ServerHost = "Abp.Ldap.ServerHost";

        public const string ServerPort = "Abp.Ldap.ServerPort";

        public const string UseSsl = "Abp.Ldap.UseSsl";

        public const string SearchBase = "Abp.Ldap.SearchBase";

        public const string DomainName = "Abp.Ldap.DomainName";

        public const string DomainDistinguishedName = "Abp.Ldap.DomainDistinguishedName";

        public static class Credentials
        {
            public const string DomainUserName = "Abp.Ldap.Credentials.DomainUserName";

            public const string Password = "Abp.Ldap.Credentials.Password";
        }
    }
}
