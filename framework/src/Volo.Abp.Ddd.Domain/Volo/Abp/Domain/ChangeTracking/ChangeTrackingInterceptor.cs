using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Domain.ChangeTracking;

public class ChangeTrackingInterceptor : AbpInterceptor, ITransientDependency
{
    private readonly IEntityChangeTrackingProvider _entityChangeTrackingProvider;

    public ChangeTrackingInterceptor(IEntityChangeTrackingProvider entityChangeTrackingProvider)
    {
        _entityChangeTrackingProvider = entityChangeTrackingProvider;
    }

    public async override Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        if (!ChangeTrackingHelper.IsEntityChangeTrackingMethod(invocation.Method, out var changeTrackingAttribute))
        {
            await invocation.ProceedAsync();
            return;
        }

        using (_entityChangeTrackingProvider.Change(changeTrackingAttribute?.IsEnabled))
        {
            await invocation.ProceedAsync();
        }
    }
}
