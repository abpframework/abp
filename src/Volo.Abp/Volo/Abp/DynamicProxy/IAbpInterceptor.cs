using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy
{
	//TODO: Create also Intercept() for sync method interception! Create AbpInterceptor for simpler usage!
	public interface IAbpInterceptor
    {
        void Intercept(IAbpMethodInvocation invocation);

        Task InterceptAsync(IAbpMethodInvocation invocation);
	}
}
