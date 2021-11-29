using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Volo.Abp.ObjectExtending
{
    public class ObjectExtensionInfoEfCoreMappingOptions
    {
        [NotNull]
        public ObjectExtensionInfo ObjectExtension { get; }

        [CanBeNull]
        public Action<EntityTypeBuilder> EntityTypeBuildAction { get; set; }

        [CanBeNull]
        public Action<ModelBuilder> ModelBuildAction { get; set; }

        public ObjectExtensionInfoEfCoreMappingOptions(
            [NotNull] ObjectExtensionInfo objectExtension,
            [NotNull] Action<EntityTypeBuilder> entityTypeBuildAction)
        {
            ObjectExtension = Check.NotNull(objectExtension, nameof(objectExtension));
            EntityTypeBuildAction = Check.NotNull(entityTypeBuildAction, nameof(entityTypeBuildAction));

            EntityTypeBuildAction = entityTypeBuildAction;
        }

        public ObjectExtensionInfoEfCoreMappingOptions(
            [NotNull] ObjectExtensionInfo objectExtension,
            [NotNull] Action<ModelBuilder> modelBuildAction)
        {
            ObjectExtension = Check.NotNull(objectExtension, nameof(objectExtension));
            ModelBuildAction = Check.NotNull(modelBuildAction, nameof(modelBuildAction));

            ModelBuildAction = modelBuildAction;
        }
    }
}
