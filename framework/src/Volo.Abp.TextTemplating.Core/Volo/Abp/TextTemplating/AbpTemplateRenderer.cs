using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.TextTemplating
{
    public class AbpTemplateRenderer : ITemplateRenderer, ITransientDependency
    {
        protected IServiceScopeFactory ServiceScopeFactory { get; }
        protected ITemplateDefinitionManager TemplateDefinitionManager { get; }
        protected AbpTextTemplatingOptions Options { get; }

        public AbpTemplateRenderer(
            IServiceScopeFactory serviceScopeFactory,
            ITemplateDefinitionManager templateDefinitionManager,
            IOptions<AbpTextTemplatingOptions> options)
        {
            ServiceScopeFactory = serviceScopeFactory;
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
                renderEngine = Options.DefaultRenderingEngine;
            }

            var providerType = Options.RenderingEngines.GetOrDefault(renderEngine);

            if (providerType != null && typeof(ITemplateRenderingEngine).IsAssignableFrom(providerType))
            {
                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    var templateRenderingEngine = (ITemplateRenderingEngine)scope.ServiceProvider.GetRequiredService(providerType);
                    return await templateRenderingEngine.RenderAsync(templateName, model, cultureName, globalContext);
                }
            }

            throw new AbpException("There is no rendering engine found with template name: " + templateName);
        }
    }
}
