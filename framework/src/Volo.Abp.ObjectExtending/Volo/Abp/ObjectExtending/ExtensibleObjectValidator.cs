using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Data;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Validation;

namespace Volo.Abp.ObjectExtending;

public static class ExtensibleObjectValidator
{
    public static void CheckValue(
        [NotNull] IHasExtraProperties extensibleObject,
        [NotNull] string propertyName,
        [CanBeNull] object value)
    {
        var validationErrors = GetValidationErrors(
            extensibleObject,
            propertyName,
            value
        );

        if (validationErrors.Any())
        {
            throw new AbpValidationException(validationErrors);
        }
    }

    public static bool IsValid(
        [NotNull] IHasExtraProperties extensibleObject,
        [CanBeNull] ValidationContext objectValidationContext = null)
    {
        return GetValidationErrors(
            extensibleObject,
            objectValidationContext
        ).Any();
    }

    public static bool IsValid(
        [NotNull] IHasExtraProperties extensibleObject,
        [NotNull] string propertyName,
        [CanBeNull] object value,
        [CanBeNull] ValidationContext objectValidationContext = null)
    {
        return GetValidationErrors(
            extensibleObject,
            propertyName,
            value,
            objectValidationContext
        ).Any();
    }

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

    [NotNull]
    public static List<ValidationResult> GetValidationErrors(
        [NotNull] IHasExtraProperties extensibleObject,
        [NotNull] string propertyName,
        [CanBeNull] object value,
        [CanBeNull] ValidationContext objectValidationContext = null)
    {
        var validationErrors = new List<ValidationResult>();

        AddValidationErrors(
            extensibleObject,
            validationErrors,
            propertyName,
            value,
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

        AddPropertyValidationErrors(
            extensibleObject,
            validationErrors,
            objectValidationContext,
            objectExtensionInfo
        );

        ExecuteCustomObjectValidationActions(
            extensibleObject,
            validationErrors,
            objectValidationContext,
            objectExtensionInfo
        );
    }

    public static void AddValidationErrors(
        [NotNull] IHasExtraProperties extensibleObject,
        [NotNull] List<ValidationResult> validationErrors,
        [NotNull] string propertyName,
        [CanBeNull] object value,
        [CanBeNull] ValidationContext objectValidationContext = null)
    {
        Check.NotNull(extensibleObject, nameof(extensibleObject));
        Check.NotNull(validationErrors, nameof(validationErrors));
        Check.NotNullOrWhiteSpace(propertyName, nameof(propertyName));

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

        var property = objectExtensionInfo.GetPropertyOrNull(propertyName);
        if (property == null)
        {
            return;
        }

        AddPropertyValidationErrors(
            extensibleObject,
            validationErrors,
            objectValidationContext,
            property,
            value
        );
    }

    private static void AddPropertyValidationErrors(
        IHasExtraProperties extensibleObject,
        List<ValidationResult> validationErrors,
        ValidationContext objectValidationContext,
        ObjectExtensionInfo objectExtensionInfo)
    {
        var properties = objectExtensionInfo.GetProperties();
        if (!properties.Any())
        {
            return;
        }

        foreach (var property in properties)
        {
            AddPropertyValidationErrors(
                extensibleObject,
                validationErrors,
                objectValidationContext,
                property,
                extensibleObject.GetProperty(property.Name)
            );
        }
    }

    private static void AddPropertyValidationErrors(
        IHasExtraProperties extensibleObject,
        List<ValidationResult> validationErrors,
        ValidationContext objectValidationContext,
        ObjectExtensionPropertyInfo property,
        object value)
    {
        AddPropertyValidationAttributeErrors(
            extensibleObject,
            validationErrors,
            objectValidationContext,
            property,
            value
        );

        ExecuteCustomPropertyValidationActions(
            extensibleObject,
            validationErrors,
            objectValidationContext,
            property,
            value
        );
    }

    private static void AddPropertyValidationAttributeErrors(
        IHasExtraProperties extensibleObject,
        List<ValidationResult> validationErrors,
        ValidationContext objectValidationContext,
        ObjectExtensionPropertyInfo property,
        object value)
    {
        var validationAttributes = property.GetValidationAttributes();

        if (!validationAttributes.Any())
        {
            return;
        }

        var propertyValidationContext = new ValidationContext(extensibleObject, objectValidationContext, null)
        {
            DisplayName = property.Name,
            MemberName = property.Name
        };

        foreach (var attribute in validationAttributes)
        {
            var result = attribute.GetValidationResult(
                value,
                propertyValidationContext
            );

            if (result != null)
            {
                validationErrors.Add(result);
            }
        }
    }

    private static void ExecuteCustomPropertyValidationActions(
        IHasExtraProperties extensibleObject,
        List<ValidationResult> validationErrors,
        ValidationContext objectValidationContext,
        ObjectExtensionPropertyInfo property,
        object value)
    {
        if (!property.Validators.Any())
        {
            return;
        }

        var context = new ObjectExtensionPropertyValidationContext(
            property,
            extensibleObject,
            validationErrors,
            objectValidationContext,
            value
        );

        foreach (var validator in property.Validators)
        {
            validator(context);
        }
    }

    private static void ExecuteCustomObjectValidationActions(
        IHasExtraProperties extensibleObject,
        List<ValidationResult> validationErrors,
        ValidationContext objectValidationContext,
        ObjectExtensionInfo objectExtensionInfo)
    {
        if (!objectExtensionInfo.Validators.Any())
        {
            return;
        }

        var context = new ObjectExtensionValidationContext(
            objectExtensionInfo,
            extensibleObject,
            validationErrors,
            objectValidationContext
        );

        foreach (var validator in objectExtensionInfo.Validators)
        {
            validator(context);
        }
    }
}
