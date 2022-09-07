using Dapr.Client;
using Microsoft.Extensions.Options;
using Volo.Abp.Dapr;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Http.Client.Dapr;

public class AbpInvocationHandler : InvocationHandler, ITransientDependency
{
    public AbpInvocationHandler(IOptions<AbpDaprOptions> daprOptions)
    {
        DaprEndpoint = daprOptions.Value.HttpEndpoint;
    }
}