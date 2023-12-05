using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Volo.Abp.ObjectExtending;

public class ObjectExtensionPropertyInfoEfCoreMappingOptions
{
    [NotNull]
    public ObjectExtensionPropertyInfo ExtensionProperty { get; }

    [NotNull]
    public ObjectExtensionInfo ObjectExtension => ExtensionProperty.ObjectExtension;

    [Obsolete("Use EntityTypeAndPropertyBuildAction property.")]
    public Action<PropertyBuilder>? PropertyBuildAction { get; set; }

    public Action<EntityTypeBuilder, PropertyBuilder>? EntityTypeAndPropertyBuildAction { get; set; }

    [Obsolete("Use other constructors.")]
    public ObjectExtensionPropertyInfoEfCoreMappingOptions(
        [NotNull] ObjectExtensionPropertyInfo extensionProperty,
        Action<PropertyBuilder>? propertyBuildAction = null,
        Action<EntityTypeBuilder, PropertyBuilder>? entityTypeAndPropertyBuildAction = null)
    {
        ExtensionProperty = Check.NotNull(extensionProperty, nameof(extensionProperty));

        PropertyBuildAction = propertyBuildAction;
        EntityTypeAndPropertyBuildAction = entityTypeAndPropertyBuildAction;
    }

    public ObjectExtensionPropertyInfoEfCoreMappingOptions(
        [NotNull] ObjectExtensionPropertyInfo extensionProperty)
    {
        ExtensionProperty = Check.NotNull(extensionProperty, nameof(extensionProperty));
    }

    public ObjectExtensionPropertyInfoEfCoreMappingOptions(
        [NotNull] ObjectExtensionPropertyInfo extensionProperty,
        Action<EntityTypeBuilder, PropertyBuilder>? entityTypeAndPropertyBuildAction)
    {
        ExtensionProperty = Check.NotNull(extensionProperty, nameof(extensionProperty));

        EntityTypeAndPropertyBuildAction = entityTypeAndPropertyBuildAction;
    }
}
