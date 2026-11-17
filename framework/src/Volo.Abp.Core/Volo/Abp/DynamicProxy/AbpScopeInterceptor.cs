using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public abstract class AbpScopeInterceptor : AbpInterceptor
{
    public override Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        return ShouldIntercept(invocation)
            ? invocation.ProceedInScopeAsync(() => BeginScope(invocation))
            : invocation.ProceedAsync();
    }

    protected virtual bool ShouldIntercept(IAbpMethodInvocation invocation)
    {
        return true;
    }

    /// <summary>
    /// Must set its async local values synchronously.
    /// </summary>
    protected abstract IAbpInterceptionScope BeginScope(IAbpMethodInvocation invocation);
}
