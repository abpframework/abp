using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending
{
    public static class ObjectExtensionPropertyInfoValidationExtensions
    {
        public const string ValidationConfigurationName = "Validation";

        [NotNull]
        public static ObjectExtensionPropertyInfo ConfigureValidation(
            [NotNull] this ObjectExtensionPropertyInfo propertyExtension,
            [NotNull] Action<ObjectExtensionPropertyInfoValidationOptions> configureAction)
        {
            Check.NotNull(propertyExtension, nameof(propertyExtension));
            Check.NotNull(configureAction, nameof(configureAction));

            configureAction(propertyExtension.GetOrAddValidationConfiguration());

            return propertyExtension;
        }

        [NotNull]
        public static ObjectExtensionPropertyInfo AddValidationAttributes(
            [NotNull] this ObjectExtensionPropertyInfo propertyExtension,
            [NotNull] params ValidationAttribute[] validationAttributes)
        {
            Check.NotNull(propertyExtension, nameof(propertyExtension));
            Check.NotNullOrEmpty(validationAttributes, nameof(validationAttributes));

            propertyExtension
                .GetOrAddValidationConfiguration()
                .ValidationAttributes
                .AddRange(validationAttributes);

            return propertyExtension;
        }

        [CanBeNull]
        public static ObjectExtensionPropertyInfoValidationOptions GetValidationConfigurationOrNull(
            [NotNull] this ObjectExtensionPropertyInfo propertyExtension)
        {
            Check.NotNull(propertyExtension, nameof(propertyExtension));

            return propertyExtension.Configuration.GetOrDefault(ValidationConfigurationName) 
                as ObjectExtensionPropertyInfoValidationOptions;
        }

        [NotNull]
        public static ObjectExtensionPropertyInfoValidationOptions GetOrAddValidationConfiguration(
            [NotNull] this ObjectExtensionPropertyInfo propertyExtension)
        {
            Check.NotNull(propertyExtension, nameof(propertyExtension));

            var validationConfiguration = propertyExtension.GetValidationConfigurationOrNull();

            if (validationConfiguration == null)
            {
                validationConfiguration = new ObjectExtensionPropertyInfoValidationOptions(propertyExtension);
                propertyExtension.Configuration[ValidationConfigurationName] = validationConfiguration;
            }

            return validationConfiguration;
        }
    }
}
