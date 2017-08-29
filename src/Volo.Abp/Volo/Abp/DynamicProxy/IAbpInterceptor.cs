using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy
{
	public interface IAbpInterceptor
    {
        void Intercept(IAbpMethodInvocation invocation);

        Task InterceptAsync(IAbpMethodInvocation invocation);
	}
}
