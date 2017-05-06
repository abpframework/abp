using System.Threading.Tasks;
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
            if (invocation.Method.ReturnType == typeof(Task))
            {
                invocation.ReturnValue = _abpInterceptor.InterceptAsync(new CastleAbpMethodInvocationAdapter(invocation));
            }
            else
            {
	            var interceptResult = _abpInterceptor.InterceptAsync(new CastleAbpMethodInvocationAdapter(invocation));

				invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
		            invocation.Method.ReturnType.GenericTypeArguments[0],
		            invocation.ReturnValue, //TODO: Can not change return value in that case !!! Create an interceptor that changes return value, for test purposes!
		            () => interceptResult,
		            exception => { }
	            );
            }
        }

        private void InterceptSyncMethod(IInvocation invocation)
        {
            AsyncHelper.RunSync(() => _abpInterceptor.InterceptAsync(new CastleAbpMethodInvocationAdapter(invocation)));
        }
    }
}
