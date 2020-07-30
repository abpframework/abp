using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Localization;

namespace Volo.CmsKit.Reactions
{
    public class ReactionDefinitionDictionary : Dictionary<string, ReactionDefinition>
    {
        public void AddOrReplace([NotNull] string name, ILocalizableString displayName = null)
        {
            this[name] = new ReactionDefinition(name, displayName);
        }
    }
}
