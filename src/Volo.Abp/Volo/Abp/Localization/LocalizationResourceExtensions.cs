using System;
using JetBrains.Annotations;

namespace Volo.Abp.Localization
{
    public static class LocalizationResourceExtensions
    {
        public static LocalizationResource InheritFrom([NotNull] this LocalizationResource resource, [NotNull] params Type[] baseResourceTypes)
        {
            Check.NotNull(resource, nameof(resource));
            Check.NotNull(baseResourceTypes, nameof(baseResourceTypes));

            resource.BaseResourceTypes.AddRange(baseResourceTypes);

            return resource;
        }
    }
}