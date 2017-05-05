using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy
{
    public interface IAbpAsyncMethodInvocation: IAbpMethodInvocationCore
    {
        Task ProceedAsync();
    }
}