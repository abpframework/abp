using System;
using System.Collections.Generic;
using Volo.Abp.Collections;

namespace Volo.Abp.TextTemplating
{
    public class AbpTextTemplatingOptions
    {
        public ITypeList<ITemplateDefinitionProvider> DefinitionProviders { get; }
        public ITypeList<ITemplateContentContributor> ContentContributors { get; }
        public IDictionary<string, Type> RenderingEngines { get; }

        public string DefaultRenderingEngine  { get; set; }

        public AbpTextTemplatingOptions()
        {
            DefinitionProviders = new TypeList<ITemplateDefinitionProvider>();
            ContentContributors = new TypeList<ITemplateContentContributor>();
            RenderingEngines = new Dictionary<string, Type>();
        }
    }
}
