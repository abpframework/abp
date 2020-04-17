using JetBrains.Annotations;
using Volo.Abp.Localization;

namespace Volo.Abp.Features
{
    public interface IFeatureDefinitionContext
    {
        FeatureGroupDefinition AddGroup([NotNull] string name, ILocalizableString displayName = null);

        FeatureGroupDefinition GetGroupOrNull(string name);
        
        void RemoveGroup(string name);
    }
}