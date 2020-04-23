using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.TextTemplating
{
    public class TemplateRenderer : ITemplateRenderer, ITransientDependency
    {
        private readonly ITemplateContentProvider _templateContentProvider;

        public TemplateRenderer(
            ITemplateContentProvider templateContentProvider
            )
        {
            _templateContentProvider = templateContentProvider;
        }

        public virtual async Task<string> RenderAsync(
            [NotNull] string templateName,
            [CanBeNull] string cultureName = null)
        {
            Check.NotNullOrWhiteSpace(templateName, nameof(templateName));

            return await _templateContentProvider.GetContentOrNullAsync(
                templateName,
                cultureName
            );
        }
    }
}