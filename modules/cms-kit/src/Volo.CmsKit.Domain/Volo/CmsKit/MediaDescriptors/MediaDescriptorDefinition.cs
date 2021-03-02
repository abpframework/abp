using JetBrains.Annotations;

namespace Volo.CmsKit.MediaDescriptors
{
    public class MediaDescriptorDefinition : PolicySpecifiedDefinition
    {
        public MediaDescriptorDefinition(
            [NotNull] string entityType,
            [CanBeNull] string createPolicy = null,
            [CanBeNull] string updatePolicy = null, 
            [CanBeNull] string deletePolicy = null) : base(entityType,
                                                           createPolicy,
                                                           updatePolicy,
                                                           deletePolicy)
        {
        }
    }
}
