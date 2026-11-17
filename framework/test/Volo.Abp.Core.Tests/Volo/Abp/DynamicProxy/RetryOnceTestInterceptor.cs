using System;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public class RetryOnceTestInterceptor : AbpInterceptor
{
    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        try
        {
            await invocation.ProceedAsync();
        }
        catch (InvalidOperationException e) when (e.Message == "First attempt failed")
        {
            await invocation.ProceedAsync();
        }
    }
}
