using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Validation
{
    public class ValidationInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly MethodInvocationValidator _validator;

        public ValidationInterceptor(MethodInvocationValidator validator)
        {
            _validator = validator;
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

        protected virtual void Validate(IAbpMethodInvocation invocation)
        {
            _validator
                .Validate(
                    new MethodInvocationValidationContext(
                        invocation.Method,
                        invocation.Arguments
                    )
                );
        }
    }
}
