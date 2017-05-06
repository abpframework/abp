using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy
{
    public interface IAbpAsyncInterceptor
    {
        Task InterceptAsync(IAbpAsyncMethodInvocation invocation);
    }
}