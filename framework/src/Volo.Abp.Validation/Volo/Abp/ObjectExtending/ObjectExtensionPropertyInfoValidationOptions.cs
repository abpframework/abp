using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending
{
    public class ObjectExtensionPropertyInfoValidationOptions
    {
        [NotNull]
        public ObjectExtensionPropertyInfo ExtensionProperty { get; }

        [NotNull]
        public ObjectExtensionInfo ObjectExtension => ExtensionProperty.ObjectExtension;

        [NotNull]
        public List<ValidationAttribute> ValidationAttributes { get; }

        public ObjectExtensionPropertyInfoValidationOptions(
            [NotNull] ObjectExtensionPropertyInfo extensionProperty)
        {
            ExtensionProperty = Check.NotNull(extensionProperty, nameof(extensionProperty));
            ValidationAttributes = new List<ValidationAttribute>();
        }
    }
}