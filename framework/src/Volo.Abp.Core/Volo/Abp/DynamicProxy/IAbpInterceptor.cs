using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy
{
	public interface IAbpInterceptor
    {
        Task InterceptAsync(IAbpMethodInvocation invocation);
	}
}
