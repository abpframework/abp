namespace Volo.Abp.Ldap
{
    public interface ILdapManager
    {
        bool Authenticate(string username, string password);
    }
}
