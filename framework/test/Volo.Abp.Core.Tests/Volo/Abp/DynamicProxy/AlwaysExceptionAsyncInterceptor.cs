using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class AlwaysExceptionAsyncInterceptor : AbpInterceptor
{
    public override Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        throw new AbpException("This interceptor should not be executed!");
    }
}
