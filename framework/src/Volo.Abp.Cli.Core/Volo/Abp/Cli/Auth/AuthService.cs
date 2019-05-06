using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Auth
{
    public class AuthService : ITransientDependency
    {
        public async Task LoginAsync(string userName, string password)
        {

        }

        public async Task LogoutAsync()
        {

        }
    }
}
