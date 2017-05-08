using Volo.Abp.DynamicProxy;
using Volo.Abp.TestBase.Logging;
using Volo.DependencyInjection;

namespace Volo.Abp.Castle.DynamicProxy
{
	public class SimpleSyncInterceptor : AbpInterceptor, ITransientDependency
	{
		public override void Intercept(IAbpMethodInvocation invocation)
		{
			(invocation.TargetObject as ICanLogOnObject)?.Logs?.Add($"{GetType().Name}_Intercept_BeforeInvocation");
			invocation.Proceed();
			(invocation.TargetObject as ICanLogOnObject)?.Logs?.Add($"{GetType().Name}_Intercept_AfterInvocation");
		}
	}
}