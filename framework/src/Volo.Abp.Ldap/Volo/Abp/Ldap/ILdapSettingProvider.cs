using System.Threading.Tasks;

namespace Volo.Abp.Ldap;

public interface ILdapSettingProvider
{
    public Task<string> GetServerHostAsync();

    public Task<int> GetServerPortAsync();

    public Task<string> GetBaseDcAsync();

    public Task<string> GetDomainDcAsync();

    public Task<string> GetUserNameAsync();

    public Task<string> GetPasswordAsync();
}
