using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public interface IAbpInterceptedAsyncEnumerable
{
    Task WaitForInterceptorsAsync();
}
