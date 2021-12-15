using System.Threading.Tasks;

namespace Volo.Abp.Cli.Auth;

public interface IAuthService
{
    Task<LoginInfo> GetLoginInfoAsync();

    Task LoginAsync(string userName, string password, string organizationName = null);

    Task LogoutAsync();
}
