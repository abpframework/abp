using Volo.Abp.TestBase.Logging;
using Volo.DependencyInjection;

namespace Volo.Abp.DynamicProxy
{
	public class SimpleSyncInterceptor : AbpInterceptor
	{
		public override void Intercept(IAbpMethodInvocation invocation)
		{
			(invocation.TargetObject as ICanLogOnObject)?.Logs?.Add($"{GetType().Name}_Intercept_BeforeInvocation");
			invocation.Proceed();
			(invocation.TargetObject as ICanLogOnObject)?.Logs?.Add($"{GetType().Name}_Intercept_AfterInvocation");
		}
	}
}