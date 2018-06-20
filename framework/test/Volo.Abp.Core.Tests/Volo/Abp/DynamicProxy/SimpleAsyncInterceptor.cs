using System.Threading.Tasks;
using Volo.Abp.TestBase.Logging;

namespace Volo.Abp.DynamicProxy
{
    public class SimpleAsyncInterceptor : AbpInterceptor
    {
	    public override void Intercept(IAbpMethodInvocation invocation)
	    {
			(invocation.TargetObject as ICanLogOnObject)?.Logs?.Add($"{GetType().Name}_Intercept_BeforeInvocation");
		    invocation.ProceedAsync();
		    (invocation.TargetObject as ICanLogOnObject)?.Logs?.Add($"{GetType().Name}_Intercept_AfterInvocation");
		}

	    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
			//await Task.Delay(5); CAN NOT USE await before method execution! This is a restriction of Castle DynamicProxy
			(invocation.TargetObject as ICanLogOnObject)?.Logs?.Add($"{GetType().Name}_InterceptAsync_BeforeInvocation");
            await invocation.ProceedAsync();
            (invocation.TargetObject as ICanLogOnObject)?.Logs?.Add($"{GetType().Name}_InterceptAsync_AfterInvocation");
            await Task.Delay(5);
        }
    }
}