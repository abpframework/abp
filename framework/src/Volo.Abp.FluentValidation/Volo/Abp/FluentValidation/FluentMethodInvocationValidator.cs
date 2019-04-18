using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentValidation;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;

namespace Volo.Abp.FluentValidation
{
    public class FluentMethodInvocationValidator : IMethodInvocationValidator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public FluentMethodInvocationValidator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Validate(MethodInvocationValidationContext context)
        {
            var validationResult = new AbpValidationResult();

            foreach (var parameterValue in context.ParameterValues)
            {
                var serviceType = typeof(IValidator<>).MakeGenericType(parameterValue.GetType());
                var validator = _serviceProvider.GetService(serviceType) as IValidator;
                if (validator == null)
                {
                    continue;
                }

                var result = validator.Validate(parameterValue);
                if (!result.IsValid)
                {
                    validationResult.Errors.AddRange(
                        result.Errors.Select(
                            error =>
                                new ValidationResult(error.ErrorMessage)
                        )
                    );
                }
            }

            if (validationResult.Errors.Any())
            {
                //TODO: How to localize messages?
                throw new AbpValidationException(
                    "Method arguments are not valid! See ValidationErrors for details.",
                    context.Errors
                );
            }
        }
    }
}
