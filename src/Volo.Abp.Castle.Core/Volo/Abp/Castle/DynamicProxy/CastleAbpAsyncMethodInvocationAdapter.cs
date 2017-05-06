using System.Threading.Tasks;
using Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Castle.DynamicProxy
{
    public class CastleAbpAsyncMethodInvocationAdapter : CastleAbpMethodInvocationAdapterBase, IAbpAsyncMethodInvocation
    {
        public CastleAbpAsyncMethodInvocationAdapter(IInvocation invocation)
            : base(invocation)
        {

        }

        public Task ProceedAsync()
        {
            Invocation.Proceed();
            return (Task)Invocation.ReturnValue;
        }
    }
}