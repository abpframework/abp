using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Http.Client.Authentication
{
    [Dependency(TryRegister = true)]
    public class NullHttpClientAuthenticator : IHttpClientAuthenticator, ISingletonDependency
    {
        public Task Authenticate(HttpClientAuthenticateContext context)
        {
            return Task.CompletedTask;
        }
    }
}