using System.Threading.Tasks;
using Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Threading;

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
            if (invocation.Method.ReturnType == typeof(Task))
            {
                invocation.ReturnValue = _abpInterceptor.InterceptAsync(new CastleAbpMethodInvocationAdapter(invocation));
            }
            else
            {
	            var interceptResult = _abpInterceptor.InterceptAsync(new CastleAbpMethodInvocationAdapter(invocation));
	            var actualReturnValue = invocation.ReturnValue;
				invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult(
		            invocation.Method.ReturnType.GenericTypeArguments[0],
		            () => actualReturnValue,
		            () => interceptResult
	            );
            }
        }

        private void InterceptSyncMethod(IInvocation invocation)
        {
	        _abpInterceptor.Intercept(new CastleAbpMethodInvocationAdapter(invocation));
        }
    }
}
