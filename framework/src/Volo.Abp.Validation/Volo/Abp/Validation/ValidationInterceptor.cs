using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Validation
{
    public class ValidationInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly AbpValidationOptions _abpValidationOptions;
        private readonly IServiceProvider _serviceProvider;

        public ValidationInterceptor(IServiceProvider serviceProvider, IOptions<AbpValidationOptions> abpValidationOptions)
        {
            _serviceProvider = serviceProvider;
            _abpValidationOptions = abpValidationOptions.Value;
        }

        public override void Intercept(IAbpMethodInvocation invocation)
        {
            if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpCrossCuttingConcerns.Validation))
            {
                invocation.Proceed();
                return;
            }

            Validate(invocation);

            invocation.Proceed();
        }

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpCrossCuttingConcerns.Validation))
            {
                await invocation.ProceedAsync();
                return;
            }

            Validate(invocation);

            await invocation.ProceedAsync();
        }

        protected virtual void Validate(IAbpMethodInvocation invocation)
        {
            foreach (var validationContributor in _abpValidationOptions.MethodValidationContributors)
            {
                var validator = (IMethodInvocationValidator) _serviceProvider.GetRequiredService(validationContributor);

                validator.Validate(
                    new MethodInvocationValidationContext(
                        invocation.TargetObject,
                        invocation.Method,
                        invocation.Arguments
                    )
                );
            }
        }
    }
}
