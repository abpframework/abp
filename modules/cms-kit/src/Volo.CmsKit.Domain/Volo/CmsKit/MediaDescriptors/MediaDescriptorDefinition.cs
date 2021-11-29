using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Volo.CmsKit.MediaDescriptors
{
    public class MediaDescriptorDefinition : PolicySpecifiedDefinition
    {
        public MediaDescriptorDefinition(
            [NotNull] string entityType,
            IEnumerable<string> createPolicies = null,
            IEnumerable<string> updatePolicies = null,
            IEnumerable<string> deletePolicies = null) : base(entityType,
                                                              createPolicies,
                                                              updatePolicies,
                                                              deletePolicies)
        {
        }
    }
}
