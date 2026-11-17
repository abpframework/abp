using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Reflection;

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
        var method = invocation.Method;
        if (!ChangeTrackingHelper.IsEntityChangeTrackingMethod(method, out var changeTrackingAttribute))
        {
            await invocation.ProceedAsync();
            return;
        }

        if (TypeHelper.IsAsyncEnumerable(method.ReturnType))
        {
            await InterceptAsyncEnumerableAsync(invocation, changeTrackingAttribute);
            return;
        }

        using (_entityChangeTrackingProvider.Change(changeTrackingAttribute?.IsEnabled))
        {
            await invocation.ProceedAsync();
        }
    }

    private Task InterceptAsyncEnumerableAsync(IAbpMethodInvocation invocation, EntityChangeTrackingAttribute? changeTrackingAttribute)
    {
        return invocation.ProceedInScopeAsync(() => new AbpInterceptionScope(_entityChangeTrackingProvider.Change(changeTrackingAttribute?.IsEnabled)));
    }
}
