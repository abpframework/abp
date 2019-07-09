using System.Threading.Tasks;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Threading;

namespace Volo.Abp.Features
{
    public class FeatureInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly IMethodInvocationFeatureCheckerService _methodInvocationFeatureCheckerService;

        public FeatureInterceptor(
            IMethodInvocationFeatureCheckerService methodInvocationFeatureCheckerService)
        {
            _methodInvocationFeatureCheckerService = methodInvocationFeatureCheckerService;
        }

        public override void Intercept(IAbpMethodInvocation invocation)
        {
            if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpCrossCuttingConcerns.FeatureChecking))
            {
                invocation.Proceed();
                return;
            }

            AsyncHelper.RunSync(() => CheckFeaturesAsync(invocation));
            invocation.Proceed();
        }

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpCrossCuttingConcerns.FeatureChecking))
            {
                await invocation.ProceedAsync();
                return;
            }

            await CheckFeaturesAsync(invocation);
            await invocation.ProceedAsync();
        }

        protected virtual async Task CheckFeaturesAsync(IAbpMethodInvocation invocation)
        {
            await _methodInvocationFeatureCheckerService.CheckAsync(
                new MethodInvocationFeatureCheckerContext(
                    invocation.Method
                )
            );
        }
    }
}
