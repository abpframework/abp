using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Domain.Repositories;

public class RepositoryInterceptor : AbpInterceptor, ITransientDependency
{
    private readonly IEntityChangeTrackingProvider _entityChangeTrackingProvider;

    public RepositoryInterceptor(IEntityChangeTrackingProvider entityChangeTrackingProvider)
    {
        _entityChangeTrackingProvider = entityChangeTrackingProvider;
    }

    public async override Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        if (!RepositoryHelper.IsEntityChangeTrackingMethod(invocation.Method, out var changeTrackingAttribute))
        {
            await invocation.ProceedAsync();
            return;
        }

        using (_entityChangeTrackingProvider.Change(changeTrackingAttribute?.Enabled))
        {
            await invocation.ProceedAsync();
        }
    }
}
