using System.Threading.Tasks;
using Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Threading;

namespace Volo.Abp.Castle.DynamicProxy
{
    public class CastleAbpMethodInvocationAdapter : CastleAbpMethodInvocationAdapterBase, IAbpMethodInvocation
    {
        public CastleAbpMethodInvocationAdapter(IInvocation invocation)
            : base(invocation)
        {

        }

        public void Proceed()
        {
            if (Invocation.Method.IsAsync())
            {
                Invocation.Proceed();
                AsyncHelper.RunSync(() => (Task) Invocation.ReturnValue);
            }
            else
            {
                Invocation.Proceed();
            }
        }
    }
}