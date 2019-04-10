using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Threading;

namespace Volo.Abp.Castle.DynamicProxy
{
    public class CastleAbpInterceptorAdapter<TInterceptor> : IInterceptor
        where TInterceptor : IAbpInterceptor
    {
        private static readonly MethodInfo MethodExecuteWithoutReturnValueAsync =
            typeof(CastleAbpInterceptorAdapter<TInterceptor>)
                .GetMethod(
                    nameof(ExecuteWithoutReturnValueAsync),
                    BindingFlags.NonPublic | BindingFlags.Instance
                );

        private static readonly MethodInfo MethodExecuteWithReturnValueAsync =
            typeof(CastleAbpInterceptorAdapter<TInterceptor>)
                .GetMethod(
                    nameof(ExecuteWithReturnValueAsync),
                    BindingFlags.NonPublic | BindingFlags.Instance
                );

        private readonly TInterceptor _abpInterceptor;

        public CastleAbpInterceptorAdapter(TInterceptor abpInterceptor)
        {
            _abpInterceptor = abpInterceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            var proceedInfo = invocation.CaptureProceedInfo();

            var method = invocation.MethodInvocationTarget ?? invocation.Method;

            if (method.IsAsync())
            {
                InterceptAsyncMethod(invocation, proceedInfo);
            }
            else
            {
                InterceptSyncMethod(invocation, proceedInfo);
            }
        }

        private void InterceptSyncMethod(IInvocation invocation, IInvocationProceedInfo proceedInfo)
        {
            _abpInterceptor.Intercept(new CastleAbpMethodInvocationAdapter(invocation, proceedInfo));
        }

        private void InterceptAsyncMethod(IInvocation invocation, IInvocationProceedInfo proceedInfo)
        {
            if (invocation.Method.ReturnType == typeof(Task))
            {
                invocation.ReturnValue = MethodExecuteWithoutReturnValueAsync
                    .Invoke(this, new object[] { invocation, proceedInfo });
            }
            else
            {
                invocation.ReturnValue = MethodExecuteWithReturnValueAsync
                    .MakeGenericMethod(invocation.Method.ReturnType.GenericTypeArguments[0])
                    .Invoke(this, new object[] {invocation, proceedInfo});
            }
        }

        private async Task ExecuteWithoutReturnValueAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo)
        {
            await Task.Yield();

            await _abpInterceptor.InterceptAsync(
                new CastleAbpMethodInvocationAdapter(invocation, proceedInfo)
            );
        }

        private async Task<T> ExecuteWithReturnValueAsync<T>(IInvocation invocation, IInvocationProceedInfo proceedInfo)
        {
            await Task.Yield();

            await _abpInterceptor.InterceptAsync(
                new CastleAbpMethodInvocationAdapter(invocation, proceedInfo)
            );

            return await (Task<T>)invocation.ReturnValue;
        }
    }
}
