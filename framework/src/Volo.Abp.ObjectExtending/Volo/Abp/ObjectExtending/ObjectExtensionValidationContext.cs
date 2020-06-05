using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Volo.Abp.Data;

namespace Volo.Abp.ObjectExtending
{
    public class ObjectExtensionValidationContext
    {
        /// <summary>
        /// Related object extension information.
        /// </summary>
        [NotNull]
        public ObjectExtensionInfo ObjectExtensionInfo { get; }

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
        /// Can be used to resolve services from the dependency injection container.
        /// </summary>
        [CanBeNull]
        public IServiceProvider ServiceProvider => ValidationContext;

        public ObjectExtensionValidationContext(
            [NotNull] ObjectExtensionInfo objectExtensionInfo,
            [NotNull] IHasExtraProperties validatingObject,
            [NotNull] List<ValidationResult> validationErrors,
            [NotNull] ValidationContext validationContext)
        {
            ObjectExtensionInfo = Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));
            ValidatingObject = Check.NotNull(validatingObject, nameof(validatingObject));
            ValidationErrors = Check.NotNull(validationErrors, nameof(validationErrors));
            ValidationContext = Check.NotNull(validationContext, nameof(validationContext));
        }
    }
}