using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class NullReturnValueTestInterceptor : AbpInterceptor
{
    public override Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        invocation.ReturnValue = null;
        return Task.CompletedTask;
    }
}
