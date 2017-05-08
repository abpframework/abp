using System.Threading.Tasks;
using Volo.Abp.DynamicProxy;
using Volo.Abp.TestBase.Logging;
using Volo.DependencyInjection;

namespace Volo.Abp.Castle.DynamicProxy
{
    public class SimpleAsyncInterceptor : AbpInterceptor, ITransientDependency
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