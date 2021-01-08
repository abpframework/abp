using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.CmsKit.Domain.Volo.CmsKit;

namespace Volo.CmsKit.Tags
{
    public class TagDefiniton : PolicySpecifiedDefinition
    {
        public string EntityType { get; }

        public TagDefiniton()
        {
        }

        public TagDefiniton(
            [NotNull] string entityType,
            [CanBeNull] ILocalizableString displayName = null,
            [CanBeNull] string createPolicy = null,
            [CanBeNull] string updatePolicy = null,
            [CanBeNull] string deletePolicy = null) : base(displayName, createPolicy, updatePolicy, deletePolicy)
        {
            EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

        }
    }
}
