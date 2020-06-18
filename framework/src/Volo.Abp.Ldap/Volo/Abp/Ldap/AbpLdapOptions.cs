namespace Volo.Abp.Ldap
{
    public class AbpLdapOptions
    {
        public string ServerHost { get; set; }

        public int ServerPort { get; set; }

        public bool UseSsl { get; set; }

        public string SearchBase { get; set; }

        public string DomainName { get; set; }

        public string DomainDistinguishedName { get; set; }

        public LdapCredentials Credentials { get; set; }

        public AbpLdapOptions()
        {
            Credentials = new LdapCredentials();
        }
    }
}