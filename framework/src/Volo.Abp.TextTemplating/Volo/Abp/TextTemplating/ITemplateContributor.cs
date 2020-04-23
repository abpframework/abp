using JetBrains.Annotations;

namespace Volo.Abp.TextTemplating
{
    public interface ITemplateContributor
    {
        void Initialize(TemplateContributorInitializationContext context);

        string GetOrNull([CanBeNull] string cultureName);
    }
}