using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Volo.Abp.ObjectExtending
{
    public class ObjectExtensionPropertyInfoEfCoreMappingOptions
    {
        [NotNull]
        public ObjectExtensionPropertyInfo ExtensionProperty { get; }

        [NotNull]
        public ObjectExtensionInfo ObjectExtension => ExtensionProperty.ObjectExtension;

        [NotNull]
        public Type FieldType { get; }

        [CanBeNull]
        public Action<PropertyBuilder> PropertyBuildAction { get; set; }

        public ObjectExtensionPropertyInfoEfCoreMappingOptions(
            [NotNull] Type fieldType,
            [NotNull] ObjectExtensionPropertyInfo extensionProperty,
            [CanBeNull] Action<PropertyBuilder> propertyBuildAction = null)
        {
            FieldType = Check.NotNull(fieldType, nameof(fieldType));
            ExtensionProperty = Check.NotNull(extensionProperty, nameof(extensionProperty));
            PropertyBuildAction = propertyBuildAction;
        }
    }
}