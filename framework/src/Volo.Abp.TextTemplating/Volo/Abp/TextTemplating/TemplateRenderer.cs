using System.Threading.Tasks;
using JetBrains.Annotations;
using Scriban;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.TextTemplating
{
    public class TemplateRenderer : ITemplateRenderer, ITransientDependency
    {
        private readonly ITemplateContentProvider _templateContentProvider;

        public TemplateRenderer(
            ITemplateContentProvider templateContentProvider)
        {
            _templateContentProvider = templateContentProvider;
        }

        public virtual async Task<string> RenderAsync(
            [NotNull] string templateName,
            [CanBeNull] object model = null,
            [CanBeNull] string cultureName = null)
        {
            Check.NotNullOrWhiteSpace(templateName, nameof(templateName));

            var content = await _templateContentProvider.GetContentOrNullAsync(
                templateName,
                cultureName
            );

            var parsedTemplate = Template.Parse(content);

            return await parsedTemplate.RenderAsync(model);
        }
    }
}