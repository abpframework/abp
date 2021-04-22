using Volo.Abp.Collections;

namespace Volo.Abp.TextTemplating
{
    public class AbpTextTemplatingOptions
    {
        public ITypeList<ITemplateDefinitionProvider> DefinitionProviders { get; }
        public ITypeList<ITemplateContentContributor> ContentContributors { get; }

        public AbpTextTemplatingOptions()
        {
            DefinitionProviders = new TypeList<ITemplateDefinitionProvider>();
            ContentContributors = new TypeList<ITemplateContentContributor>();
        }
    }
}