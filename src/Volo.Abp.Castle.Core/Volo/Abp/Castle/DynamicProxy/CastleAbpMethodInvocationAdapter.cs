using Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;

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
            Invocation.Proceed();
        }
    }
}