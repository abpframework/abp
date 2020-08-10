namespace Volo.Abp.Ldap
{
    public interface ILdapManager
    {
        /// <summary>
        /// Authenticate with default username/password
        /// </summary>
        /// <returns></returns>
        bool Authenticate();

        /// <summary>
        /// Authenticate with specified username/password
        /// </summary>
        /// <returns></returns>
        bool Authenticate(string username, string password);
    }
}
