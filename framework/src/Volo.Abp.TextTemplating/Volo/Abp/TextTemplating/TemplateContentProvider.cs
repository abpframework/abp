using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.TextTemplating
{
    public class TemplateContentProvider : ITemplateContentProvider, ITransientDependency
    {
        private readonly ITemplateDefinitionManager _templateDefinitionManager;

        public TemplateContentProvider(
            ITemplateDefinitionManager templateDefinitionManager
        )
        {
            _templateDefinitionManager = templateDefinitionManager;
        }

        public async Task<string> GetContentOrNullAsync(
            [NotNull] string templateName, 
            [CanBeNull] string cultureName = null)
        {
            var template = _templateDefinitionManager.Get(templateName);

            foreach (var contributor in template.Contributors)
            {
                var templateString = contributor.GetOrNull(cultureName);
                if (templateString != null)
                {
                    return templateString;
                }
            }

            throw new AbpException(
                $"None of the template contributors could get the content for the template '{templateName}'"
            );
        }
    }
}