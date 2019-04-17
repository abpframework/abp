using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Reflection;

namespace Volo.Abp.Validation
{
    public class DataAnnotationValidator : IObjectValidator, ITransientDependency
    {
        private readonly AbpValidationOptions _options;

        public DataAnnotationValidator(IOptions<AbpValidationOptions> options)
        {
            _options = options.Value;
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
            var errors = new List<ValidationResult>();

            ValidateObjectRecursively(errors, validatingObject, 1);

            return errors;
        }

        protected virtual void ValidateObjectRecursively(List<ValidationResult> errors, object validatingObject, int currentDepth)
        {
            const int maxRecursiveParameterValidationDepth = 8;

            if (currentDepth > maxRecursiveParameterValidationDepth)
            {
                return;
            }

            if (validatingObject == null)
            {
                return;
            }

            errors.AddRange(GetDataAnnotationErrors(validatingObject));

            //Validate items of enumerable
            if (validatingObject is IEnumerable)
            {
                if (!(validatingObject is IQueryable))
                {
                    foreach (var item in (validatingObject as IEnumerable))
                    {
                        ValidateObjectRecursively(errors, item, currentDepth + 1);
                    }
                }

                return;
            }

            var validatingObjectType = validatingObject.GetType();

            //Do not recursively validate for primitive objects
            if (TypeHelper.IsPrimitiveExtended(validatingObjectType))
            {
                return;
            }

            if (_options.IgnoredTypes.Any(t => t.IsInstanceOfType(validatingObject)))
            {
                return;
            }

            var properties = TypeDescriptor.GetProperties(validatingObject).Cast<PropertyDescriptor>();
            foreach (var property in properties)
            {
                if (property.Attributes.OfType<DisableValidationAttribute>().Any())
                {
                    continue;
                }

                ValidateObjectRecursively(errors, property.GetValue(validatingObject), currentDepth + 1);
            }
        }

        /// <summary>
        /// Gets all errors from properties for DataAnnotations attributes and IValidatableObject interface.
        /// </summary>
        public virtual List<ValidationResult> GetDataAnnotationErrors(object validatingObject)
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
