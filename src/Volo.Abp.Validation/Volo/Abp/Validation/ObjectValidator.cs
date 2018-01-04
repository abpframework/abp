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
    public class ObjectValidator : IObjectValidator, ITransientDependency
    {
        private const int MaxRecursiveParameterValidationDepth = 8;

        private readonly AbpValidationOptions _options;
        private readonly IDataAnnotationValidator _dataAnnotationValidator;

        public ObjectValidator(IOptions<AbpValidationOptions> options, IDataAnnotationValidator dataAnnotationValidator)
        {
            _dataAnnotationValidator = dataAnnotationValidator;
            _options = options.Value;
        }

        public virtual void Validate(object validatingObject, string name = null, bool allowNull = false)
        {
            var errors = GetErrors(validatingObject, name, allowNull);

            if (errors.Any())
            {
                throw new AbpValidationException(
                    "Object state is not valid! See ValidationErrors for details.",
                    errors
                );
            }
        }

        public virtual List<ValidationResult> GetErrors(object validatingObject, string name = null, bool allowNull = false)
        {
            var errors = new List<ValidationResult>();

            if (validatingObject == null && !allowNull)
            {
                errors.Add(
                    name == null
                        ? new ValidationResult("Given object is null!")
                        : new ValidationResult(name + " is null!", new[] { name })
                );

                return errors;
            }

            ValidateObjectRecursively(errors, validatingObject, currentDepth: 1);

            return errors;
        }

        protected virtual void ValidateObjectRecursively(List<ValidationResult> errors, object validatingObject, int currentDepth)
        {
            if (currentDepth > MaxRecursiveParameterValidationDepth)
            {
                return;
            }

            if (validatingObject == null)
            {
                return;
            }

            errors.AddRange(_dataAnnotationValidator.GetErrors(validatingObject));

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
    }
}