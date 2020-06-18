using System.Threading.Tasks;
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

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
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
