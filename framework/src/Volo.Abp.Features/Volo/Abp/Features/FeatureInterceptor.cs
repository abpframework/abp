using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Features
{
    public class FeatureInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public FeatureInterceptor(IServiceScopeFactory serviceScopeFactory, ILogger<AbpInterceptor> logger) : base(logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
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
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                await scope.ServiceProvider.GetRequiredService<IMethodInvocationFeatureCheckerService>().CheckAsync(
                    new MethodInvocationFeatureCheckerContext(
                        invocation.Method
                    )
                );
            }
        }
    }
}
