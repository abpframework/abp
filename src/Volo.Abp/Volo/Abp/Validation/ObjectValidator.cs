using System.Collections;
using System.ComponentModel;
using System.Linq;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Reflection;

namespace Volo.Abp.Validation
{
    public class ObjectValidator : ITransientDependency, IObjectValidator
    {
        private const int MaxRecursiveParameterValidationDepth = 8;

        private readonly AbpValidationOptions _options;
        private readonly IDataAnnotationValidator _dataAnnotationValidator;

        public ObjectValidator(IOptions<AbpValidationOptions> options, IDataAnnotationValidator dataAnnotationValidator)
        {
            _dataAnnotationValidator = dataAnnotationValidator;
            _options = options.Value;
        }

        public virtual void Validate(IAbpValidationResult validationResult, object validatingObject)
        {
            AddValidatationErrors(validationResult, validatingObject);

            if (validationResult.Errors.Any())
            {
                throw new AbpValidationException(
                    "Method arguments are not valid! See ValidationErrors for details.",
                    validationResult.Errors
                );
            }
        }

        public virtual void AddValidatationErrors(IAbpValidationResult validationResult, object validatingObject)
        {
            ValidateObjectRecursively(validationResult, validatingObject, 1);
        }

        protected virtual void ValidateObjectRecursively(IAbpValidationResult context, object validatingObject, int currentDepth)
        {
            if (currentDepth > MaxRecursiveParameterValidationDepth)
            {
                return;
            }

            if (validatingObject == null)
            {
                return;
            }

            _dataAnnotationValidator.AddDataAnnotationAttributeErrors(context, validatingObject);

            //Validate items of enumerable
            if (validatingObject is IEnumerable && !(validatingObject is IQueryable))
            {
                foreach (var item in (validatingObject as IEnumerable))
                {
                    ValidateObjectRecursively(context, item, currentDepth + 1);
                }
            }

            //Add list to be normalized later
            if (validatingObject is IShouldNormalize)
            {
                context.ObjectsToBeNormalized.Add(validatingObject as IShouldNormalize);
            }

            //Do not recursively validate for enumerable objects
            if (validatingObject is IEnumerable)
            {
                return;
            }

            var validatingObjectType = validatingObject.GetType();

            //Do not recursively validate for primitive objects
            if (TypeHelper.IsPrimitiveExtendedIncludingNullable(validatingObjectType))
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

                ValidateObjectRecursively(context, property.GetValue(validatingObject), currentDepth + 1);
            }
        }
    }
}