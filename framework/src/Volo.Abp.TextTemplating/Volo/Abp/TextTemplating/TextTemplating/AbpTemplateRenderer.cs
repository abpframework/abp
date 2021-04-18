using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating.Scriban;

namespace Volo.Abp.TextTemplating
{
    public class AbpTemplateRenderer : ITemplateRenderer, ITransientDependency
    {
        protected IServiceProvider ServiceProvider { get; }
        protected ITemplateDefinitionManager TemplateDefinitionManager { get; }
        protected AbpTemplateRendererProviderOptions Options { get; }

        public AbpTemplateRenderer(
            IServiceProvider serviceProvider,
            ITemplateDefinitionManager templateDefinitionManager,
            IOptions<AbpTemplateRendererProviderOptions> options)
        {
            ServiceProvider = serviceProvider;
            TemplateDefinitionManager = templateDefinitionManager;
            Options = options.Value;
        }

        public virtual async Task<string> RenderAsync(
            string templateName,
            object model = null,
            string cultureName = null,
            Dictionary<string, object> globalContext = null)
        {
            var templateDefinition = TemplateDefinitionManager.Get(templateName);

            var renderEngine = templateDefinition.RenderEngine;

            if (renderEngine.IsNullOrWhiteSpace())
            {
                renderEngine = ScribanTemplateRendererProvider.ProviderName;
            }

            var providerType = Options.GetProviderTypeOrNull(renderEngine);

            if (providerType != null)
            {
                var templateRendererProvider = (ITemplateRendererProvider)ServiceProvider.GetRequiredService(providerType);
                return await templateRendererProvider.RenderAsync(templateName, model, cultureName, globalContext);
            }

            throw new AbpException("There is no render engine found with template name: " + templateName);
        }
    }
}
