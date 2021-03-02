using JetBrains.Annotations;
using System;

namespace Volo.CmsKit.MediaDescriptors
{
    public class MediaDescriptorDefinition : PolicySpecifiedDefinition, IEquatable<MediaDescriptorDefinition>
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

        public bool Equals(MediaDescriptorDefinition other)
        {
            return base.Equals(other);
        }
    }
}
