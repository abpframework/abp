using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentValidation;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;

namespace Volo.Abp.FluentValidation
{
    public class FluentValidator : IFluentValidator, ITransientDependency
    {
        public void Validate(MethodInvocationValidationContext context)
        {
            var validationResult = new AbpValidationResult();

            foreach (var parameterValue in context.ParameterValues)
            {
                if (parameterValue is IValidator validator)
                {
                    var result = validator.Validate(parameterValue);
                    if (!result.IsValid)
                    {
                        validationResult.Errors.AddRange(result.Errors.Select(error =>
                            new ValidationResult(error.ErrorMessage)));
                    }
                }
            }

            if (validationResult.Errors.Any())
            {
                throw new AbpValidationException(
                    "Method arguments are not valid! See ValidationErrors for details.",
                    context.Errors
                );
            }
        }
    }
}
