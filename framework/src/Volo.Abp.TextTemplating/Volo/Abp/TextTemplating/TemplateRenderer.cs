using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;
using Scriban;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.TextTemplating
{
    public class TemplateRenderer : ITemplateRenderer, ITransientDependency
    {
        private readonly ITemplateContentProvider _templateContentProvider;
        private readonly ITemplateDefinitionManager _templateDefinitionManager;
        private readonly IStringLocalizerFactory _stringLocalizerFactory;

        public TemplateRenderer(
            ITemplateContentProvider templateContentProvider, 
            ITemplateDefinitionManager templateDefinitionManager, 
            IStringLocalizerFactory stringLocalizerFactory)
        {
            _templateContentProvider = templateContentProvider;
            _templateDefinitionManager = templateDefinitionManager;
            _stringLocalizerFactory = stringLocalizerFactory;
        }

        public virtual async Task<string> RenderAsync(
            [NotNull] string templateName,
            [CanBeNull] object model = null,
            [CanBeNull] string cultureName = null)
        {
            Check.NotNullOrWhiteSpace(templateName, nameof(templateName));

            var templateDefinition = _templateDefinitionManager.Get(templateName);

            var content = await _templateContentProvider.GetContentOrNullAsync(
                templateDefinition,
                cultureName
            );

            if (templateDefinition.LocalizationResource != null)
            {
                var localizer = _stringLocalizerFactory.Create(templateDefinition.LocalizationResource);
                content = Localize(localizer, content);
            }

            var renderedContent = await Template.Parse(content).RenderAsync(model);

            if (templateDefinition.Layout != null)
            {
                renderedContent = await RenderAsync(
                    templateDefinition.Layout,
                    new
                    {
                        content = renderedContent
                    },
                    cultureName: cultureName
                );
            }

            return renderedContent;
        }

        public string Localize(IStringLocalizer localizer, string text)
        {
            return new Regex("\\{\\{#L:.+?\\}\\}")
                .Replace(
                    text,
                    match => localizer[match.Value.Substring(5, match.Length - 7)]
                );
        }
    }
}