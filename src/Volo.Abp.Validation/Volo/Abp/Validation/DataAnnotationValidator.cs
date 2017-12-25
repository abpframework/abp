using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Validation
{
    public class DataAnnotationValidator : IDataAnnotationValidator, ITransientDependency
    {
        public void Validate(object validatingObject)
        {
            var validationResult = new AbpValidationResult();

            AddErrors(validationResult, validatingObject);

            if (validationResult.Errors.Any())
            {
                throw new AbpValidationException(
                    "Object state is not valid! See ValidationErrors for details.",
                    validationResult.Errors
                );
            }
        }

        /// <summary>
        /// Checks all properties for DataAnnotations attributes.
        /// </summary>
        public virtual void AddErrors(IAbpValidationResult validationResult, object validatingObject)
        {
            var properties = TypeDescriptor.GetProperties(validatingObject).Cast<PropertyDescriptor>();
            foreach (var property in properties)
            {
                var validationAttributes = property.Attributes.OfType<ValidationAttribute>().ToArray();
                if (validationAttributes.IsNullOrEmpty())
                {
                    continue;
                }

                var validationContext = new ValidationContext(validatingObject)
                {
                    DisplayName = property.DisplayName,
                    MemberName = property.Name
                };

                foreach (var attribute in validationAttributes)
                {
                    var result = attribute.GetValidationResult(property.GetValue(validatingObject), validationContext);
                    if (result != null)
                    {
                        validationResult.Errors.Add(result);
                    }
                }
            }

            if (validatingObject is IValidatableObject)
            {
                var results = (validatingObject as IValidatableObject).Validate(new ValidationContext(validatingObject));
                validationResult.Errors.AddRange(results);
            }
        }
    }
}