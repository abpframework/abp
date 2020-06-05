using System.Threading.Tasks;
using Volo.Abp.TestBase.Logging;

namespace Volo.Abp.DynamicProxy
{
    public class SimpleAsyncInterceptor : AbpInterceptor
    {
        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            await Task.Delay(5);
			(invocation.TargetObject as ICanLogOnObject)?.Logs?.Add($"{GetType().Name}_InterceptAsync_BeforeInvocation");
            await invocation.ProceedAsync();
            (invocation.TargetObject as ICanLogOnObject)?.Logs?.Add($"{GetType().Name}_InterceptAsync_AfterInvocation");
            await Task.Delay(5);
        }
    }
}