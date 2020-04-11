using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Data;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.ObjectExtending
{
    public static class ExtensibleObjectValidator
    {
        [NotNull]
        public static List<ValidationResult> GetValidationErrors(
            [NotNull] IHasExtraProperties extensibleObject,
            [CanBeNull] ValidationContext objectValidationContext = null)
        {
            var validationErrors = new List<ValidationResult>();
            
            AddValidationErrors(
                extensibleObject,
                validationErrors,
                objectValidationContext
            );

            return validationErrors;
        }

        public static void AddValidationErrors(
            [NotNull] IHasExtraProperties extensibleObject,
            [NotNull] List<ValidationResult> validationErrors,
            [CanBeNull] ValidationContext objectValidationContext = null)
        {
            Check.NotNull(extensibleObject, nameof(extensibleObject));
            Check.NotNull(validationErrors, nameof(validationErrors));

            if (objectValidationContext == null)
            {
                objectValidationContext = new ValidationContext(
                    extensibleObject,
                    null,
                    new Dictionary<object, object>()
                );
            }

            var objectType = ProxyHelper.UnProxy(extensibleObject).GetType();

            var objectExtensionInfo = ObjectExtensionManager.Instance
                .GetOrNull(objectType);

            if (objectExtensionInfo == null)
            {
                return;
            }

            foreach (var propertyInfo in objectExtensionInfo.GetProperties())
            {
                if (propertyInfo.ValidationAttributes.Any())
                {
                    var propertyValidationContext = new ValidationContext(extensibleObject, objectValidationContext, null)
                    {
                        DisplayName = propertyInfo.Name,
                        MemberName = propertyInfo.Name
                    };

                    foreach (var attribute in propertyInfo.ValidationAttributes)
                    {
                        var result = attribute.GetValidationResult(
                            extensibleObject.GetProperty(propertyInfo.Name),
                            propertyValidationContext
                        );
                        if (result != null)
                        {
                            validationErrors.Add(result);
                        }
                    }
                }
            }
        }
    }
}