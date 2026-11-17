using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class WrongReturnValueTestInterceptor : AbpInterceptor
{
    public override Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        invocation.ReturnValue = new List<int> { 1 };
        return Task.CompletedTask;
    }
}
