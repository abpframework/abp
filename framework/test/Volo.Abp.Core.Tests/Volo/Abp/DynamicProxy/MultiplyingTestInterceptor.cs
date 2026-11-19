using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class MultiplyingTestInterceptor : AbpInterceptor
{
    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        await invocation.ProceedAsync();
        invocation.ReturnValue = MultiplyAsync((IAsyncEnumerable<int>)invocation.ReturnValue);
    }

    private static async IAsyncEnumerable<int> MultiplyAsync(IAsyncEnumerable<int> source)
    {
        await foreach (var item in source)
        {
            yield return item * 10;
        }
    }
}
