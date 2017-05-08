using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy
{
	public abstract class AbpInterceptor : IAbpInterceptor
	{
		public abstract void Intercept(IAbpMethodInvocation invocation);

		public virtual Task InterceptAsync(IAbpMethodInvocation invocation)
		{
			Intercept(invocation);
			return Task.CompletedTask;
		}
	}
}