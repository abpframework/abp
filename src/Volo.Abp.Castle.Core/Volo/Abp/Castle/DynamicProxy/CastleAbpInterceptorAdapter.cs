using Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Threading;
using Volo.ExtensionMethods;

namespace Volo.Abp.Castle.DynamicProxy
{
    public class CastleAbpInterceptorAdapter<TInterceptor> : IInterceptor
        where TInterceptor : IAbpInterceptor
    {
        private readonly TInterceptor _abpInterceptor;

        public CastleAbpInterceptorAdapter(TInterceptor abpInterceptor)
        {
            _abpInterceptor = abpInterceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.MethodInvocationTarget.IsAsync())
            {
                InterceptAsyncMethod(invocation);
            }
            else
            {
                InterceptSyncMethod(invocation);
            }
        }

        private void InterceptAsyncMethod(IInvocation invocation)
        {
            if (_abpInterceptor is IAbpAsyncInterceptor)
            {
                _abpInterceptor.As<IAbpAsyncInterceptor>().InterceptAsync(new CastleAbpAsyncMethodInvocationAdapter(invocation));
            }
            else
            {
                _abpInterceptor.Intercept(new CastleAbpMethodInvocationAdapter(invocation));
            }
        }

        private void InterceptSyncMethod(IInvocation invocation)
        {
            _abpInterceptor.Intercept(new CastleAbpMethodInvocationAdapter(invocation));
        }
    }
}
