using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Volo.Abp.Data;

namespace Volo.Abp.ObjectExtending;

public class ObjectExtensionPropertyValidationContext
{
    /// <summary>
    /// Related property extension information.
    /// </summary>
    [NotNull]
    public ObjectExtensionPropertyInfo ExtensionPropertyInfo { get; }

    /// <summary>
    /// Reference to the validating object.
    /// </summary>
    [NotNull]
    public IHasExtraProperties ValidatingObject { get; }

    /// <summary>
    /// Add validation errors to this list.
    /// </summary>
    [NotNull]
    public List<ValidationResult> ValidationErrors { get; }

    /// <summary>
    /// Validation context comes from the <see cref="IValidatableObject.Validate"/> method.
    /// </summary>
    [NotNull]
    public ValidationContext ValidationContext { get; }

    /// <summary>
    /// The value of the validating property of the <see cref="ValidatingObject"/>.
    /// </summary>
    [CanBeNull]
    public object Value { get; }

    /// <summary>
    /// Can be used to resolve services from the dependency injection container.
    /// This can be null when SetProperty method is used on the object.
    /// </summary>
    [CanBeNull]
    public IServiceProvider ServiceProvider => ValidationContext;

    public ObjectExtensionPropertyValidationContext(
        [NotNull] ObjectExtensionPropertyInfo objectExtensionPropertyInfo,
        [NotNull] IHasExtraProperties validatingObject,
        [NotNull] List<ValidationResult> validationErrors,
        [NotNull] ValidationContext validationContext,
        [CanBeNull] object value)
    {
        ExtensionPropertyInfo = Check.NotNull(objectExtensionPropertyInfo, nameof(objectExtensionPropertyInfo));
        ValidatingObject = Check.NotNull(validatingObject, nameof(validatingObject));
        ValidationErrors = Check.NotNull(validationErrors, nameof(validationErrors));
        ValidationContext = Check.NotNull(validationContext, nameof(validationContext));
        Value = value;
    }
}
