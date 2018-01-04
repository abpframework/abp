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
            var errors = GetErrors(validatingObject);

            if (errors.Any())
            {
                throw new AbpValidationException(
                    "Object state is not valid! See ValidationErrors for details.",
                    errors
                );
            }
        }

        /// <summary>
        /// Gets all errors from properties for DataAnnotations attributes and IValidatableObject interface.
        /// </summary>
        public virtual List<ValidationResult> GetErrors(object validatingObject)
        {
            var errors = new List<ValidationResult>();
            var properties = TypeDescriptor.GetProperties(validatingObject).Cast<PropertyDescriptor>();

            foreach (var property in properties)
            {
                AddPropertyErrors(validatingObject, property, errors);
            }

            if (validatingObject is IValidatableObject validatableObject)
            {
                errors.AddRange(
                    validatableObject.Validate(new ValidationContext(validatableObject))
                );
            }

            return errors;
        }

        protected virtual void AddPropertyErrors(object validatingObject, PropertyDescriptor property, List<ValidationResult> errors)
        {
            var validationAttributes = property.Attributes.OfType<ValidationAttribute>().ToArray();
            if (validationAttributes.IsNullOrEmpty())
            {
                return;
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
                    errors.Add(result);
                }
            }
        }
    }
}