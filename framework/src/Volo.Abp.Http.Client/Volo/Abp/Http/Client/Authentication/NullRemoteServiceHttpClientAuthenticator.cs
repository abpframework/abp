using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Http.Client.Authentication;

[Dependency(TryRegister = true)]
public class NullRemoteServiceHttpClientAuthenticator : IRemoteServiceHttpClientAuthenticator, ISingletonDependency
{
    public Task Authenticate(RemoteServiceHttpClientAuthenticateContext context)
    {
        return Task.CompletedTask;
    }
}
