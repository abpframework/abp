using System.Threading.Tasks;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Validation;

namespace Volo.Abp.FluentValidation
{
    public class FluentValidationInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly IFluentValidator _fluentValidator;

        public FluentValidationInterceptor(IFluentValidator fluentValidator)
        {
            _fluentValidator = fluentValidator;
        }

        public override void Intercept(IAbpMethodInvocation invocation)
        {
            if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpFluentValidationCrossCuttingConcern.FluentValidation))
            {
                invocation.Proceed();
                return;
            }

            Validate(invocation);

            invocation.Proceed();
        }

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpFluentValidationCrossCuttingConcern.FluentValidation))
            {
                await invocation.ProceedAsync();
                return;
            }

            Validate(invocation);

            await invocation.ProceedAsync();
        }

        protected virtual void Validate(IAbpMethodInvocation invocation)
        {
            _fluentValidator.Validate(new MethodInvocationValidationContext(
                invocation.TargetObject,
                invocation.Method,
                invocation.Arguments
            ));
        }
    }
}
