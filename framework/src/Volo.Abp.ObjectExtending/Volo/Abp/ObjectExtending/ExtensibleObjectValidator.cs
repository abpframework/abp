using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            [CanBeNull] IServiceProvider serviceProvider = null)
        {
            var validationErrors = new List<ValidationResult>();
            
            AddValidationErrors(
                extensibleObject,
                new ValidationContext(
                    extensibleObject,
                    serviceProvider,
                    new Dictionary<object, object>()
                ),
                validationErrors
            );

            return validationErrors;
        }

        public static void AddValidationErrors(
            [NotNull] IHasExtraProperties extensibleObject,
            [NotNull] ValidationContext validationContext,
            [NotNull] List<ValidationResult> validationErrors)
        {
            Check.NotNull(extensibleObject, nameof(extensibleObject));
            Check.NotNull(validationContext, nameof(validationContext));
            Check.NotNull(validationErrors, nameof(validationErrors));

            var objectType = ProxyHelper.UnProxy(extensibleObject).GetType();

            var objectExtensionInfo = ObjectExtensionManager.Instance
                .GetOrNull(objectType);

            if (objectExtensionInfo == null)
            {
                return;
            }

            foreach (var propertyInfo in objectExtensionInfo.GetProperties())
            {
                if (propertyInfo.ValidationAttributes.IsNullOrEmpty())
                {
                    continue;
                }

                var validationContext2 = new ValidationContext(extensibleObject, validationContext, null)
                {
                    DisplayName = propertyInfo.Name,
                    MemberName = propertyInfo.Name
                };

                foreach (var attribute in propertyInfo.ValidationAttributes)
                {
                    var result = attribute.GetValidationResult(extensibleObject.GetProperty(propertyInfo.Name), validationContext2);
                    if (result != null)
                    {
                        validationErrors.Add(result);
                    }
                }
            }
        }
    }
}