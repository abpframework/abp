using Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Castle.DynamicProxy
{
    public class CastleAbpInterceptorAdapter : IInterceptor
    {
        private readonly IAbpInterceptor _abpInterceptor;

        public CastleAbpInterceptorAdapter(IAbpInterceptor abpInterceptor)
        {
            _abpInterceptor = abpInterceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            _abpInterceptor.Intercept(new CastleAbpMethodInvocationAdapter(invocation));
        }
    }
}
