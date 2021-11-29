namespace Volo.Abp.Ldap
{
    public class AbpLdapOptions
    {
        public string ServerHost { get; set; }

        public int ServerPort { get; set; }

        public string BaseDc { get; set; }

        /// <summary>
        /// BindDN
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///BindPassword
        /// </summary>
        public string Password { get; set; }
    }
}
