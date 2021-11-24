using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public abstract class AbpInterceptor : IAbpInterceptor
{
    public abstract Task InterceptAsync(IAbpMethodInvocation invocation);
}
