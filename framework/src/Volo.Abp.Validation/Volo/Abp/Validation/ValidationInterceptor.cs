using System.Threading.Tasks;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Validation
{
    public class ValidationInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly IMethodInvocationValidator _methodInvocationValidator;

        public ValidationInterceptor(IMethodInvocationValidator methodInvocationValidator)
        {
            _methodInvocationValidator = methodInvocationValidator;
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
            _methodInvocationValidator.Validate(
                new MethodInvocationValidationContext(
                    invocation.TargetObject,
                    invocation.Method,
                    invocation.Arguments
                )
            );
        }
    }
}
