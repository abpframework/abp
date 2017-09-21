using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Validation
{
    public class ValidationInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override void Intercept(IAbpMethodInvocation invocation)
        {
            if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpCrossCuttingConcerns.Validation))
            {
                invocation.Proceed();
                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var validator = scope.ServiceProvider.GetRequiredService<MethodInvocationValidator>();
                validator.Initialize(invocation.Method, invocation.Arguments);
                validator.Validate();
            }

            invocation.Proceed();
        }
    }
}
