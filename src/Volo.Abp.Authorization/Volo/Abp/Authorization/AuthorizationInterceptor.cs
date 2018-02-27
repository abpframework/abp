using System.Threading.Tasks;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Threading;

namespace Volo.Abp.Authorization
{
    public class AuthorizationInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly IMethodInvocationAuthorizationService _methodInvocationAuthorizationService;

        public AuthorizationInterceptor(IMethodInvocationAuthorizationService methodInvocationAuthorizationService)
        {
            _methodInvocationAuthorizationService = methodInvocationAuthorizationService;
        }

        public override void Intercept(IAbpMethodInvocation invocation)
        {
            AsyncHelper.RunSync(() => InterceptAsync(invocation));
        }

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpCrossCuttingConcerns.Authorization))
            {
                await invocation.ProceedAsync();
                return;
            }

            AsyncHelper.RunSync(() => AuthorizeAsync(invocation));
            await invocation.ProceedAsync();
        }

        protected virtual Task AuthorizeAsync(IAbpMethodInvocation invocation)
        {
            return _methodInvocationAuthorizationService.CheckAsync(
                new MethodInvocationAuthorizationContext(
                    invocation.Method
                )
            );
        }
    }
}
