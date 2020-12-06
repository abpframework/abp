using System.Threading.Tasks;

namespace Volo.Abp.Ldap
{
    public interface ILdapManager
    {
        Task<bool> AuthenticateAsync(string username, string password);
    }
}
