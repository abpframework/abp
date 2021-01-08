using JetBrains.Annotations;
using System.Collections.Generic;
using Volo.Abp.Localization;

namespace Volo.CmsKit.Tags
{
    public class TagDefinitionDictionary : Dictionary<string, TagDefiniton>
    {
        public void AddOrReplace(
            [NotNull] string entityType,
            [CanBeNull] ILocalizableString displayName = null,
            [CanBeNull] string createPolicy = null,
            [CanBeNull] string updatePolicy = null,
            [CanBeNull] string deletePolicy = null
            )
        {
            this[entityType] = new TagDefiniton(entityType, displayName, createPolicy, updatePolicy, deletePolicy);
        }
    }
}
