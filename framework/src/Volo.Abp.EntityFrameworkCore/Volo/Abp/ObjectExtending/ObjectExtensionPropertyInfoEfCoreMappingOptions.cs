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

        [CanBeNull]
        public Action<PropertyBuilder> PropertyBuildAction { get; set; }

        [CanBeNull]
        public Action<EntityTypeBuilder, PropertyBuilder> EntityTypeAndPropertyBuildAction { get; set; }

        public ObjectExtensionPropertyInfoEfCoreMappingOptions(
            [NotNull] ObjectExtensionPropertyInfo extensionProperty,
            [CanBeNull] Action<PropertyBuilder> propertyBuildAction = null,
            [CanBeNull] Action<EntityTypeBuilder, PropertyBuilder> entityTypeAndPropertyBuildAction = null)
        {
            ExtensionProperty = Check.NotNull(extensionProperty, nameof(extensionProperty));

            PropertyBuildAction = propertyBuildAction;
            EntityTypeAndPropertyBuildAction = entityTypeAndPropertyBuildAction;
        }
    }
}
