using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentValidation;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;

namespace Volo.Abp.FluentValidation
{
    public class FluentValidator : IObjectValidator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public FluentValidator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Validate(object validatingObject, string name = null, bool allowNull = false)
        {
            var errors = GetErrors(validatingObject, name, allowNull);

            if (errors.Any())
            {
                //TODO: How to localize messages?
                throw new AbpValidationException(
                    "Object state is not valid! See ValidationErrors for details.",
                    errors
                );
            }
        }

        public List<ValidationResult> GetErrors(object validatingObject, string name = null, bool allowNull = false)
        {
            var validationResult = new List<ValidationResult>();

            var serviceType = typeof(IValidator<>).MakeGenericType(validatingObject.GetType());
            if (!(_serviceProvider.GetService(serviceType) is IValidator validator))
            {
                return validationResult;
            }

            var result = validator.Validate(validatingObject);
            if (!result.IsValid)
            {
                //TODO: How to localize  Fluent Validation messages?
                validationResult.AddRange(
                    result.Errors.Select(
                        error =>
                            new ValidationResult(error.ErrorMessage)
                    )
                );
            }

            return validationResult;
        }
    }
}
