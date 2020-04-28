using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.TextTemplating
{
    public class TemplateContentProvider : ITemplateContentProvider, ITransientDependency
    {
        public IHybridServiceScopeFactory ServiceScopeFactory { get; }
        public AbpTextTemplatingOptions Options { get; }
        private readonly ITemplateDefinitionManager _templateDefinitionManager;

        public TemplateContentProvider(
            ITemplateDefinitionManager templateDefinitionManager,
            IHybridServiceScopeFactory serviceScopeFactory,
            IOptions<AbpTextTemplatingOptions> options)
        {
            ServiceScopeFactory = serviceScopeFactory;
            Options = options.Value;
            _templateDefinitionManager = templateDefinitionManager;
        }

        public Task<string> GetContentOrNullAsync(
            [NotNull] string templateName, 
            [CanBeNull] string cultureName = null)
        {
            var template = _templateDefinitionManager.Get(templateName);
            return GetContentOrNullAsync(template, cultureName);
        }

        public async Task<string> GetContentOrNullAsync(
            [NotNull] TemplateDefinition templateDefinition,
            [CanBeNull] string cultureName = null)
        {
            Check.NotNull(templateDefinition, nameof(templateDefinition));

            if (!Options.ContentContributors.Any())
            {
                throw new AbpException(
                    $"No template content contributor was registered. Use {nameof(AbpTextTemplatingOptions)} to register contributors!"
                );
            }

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var context = new TemplateContentContributorContext(
                    templateDefinition,
                    scope.ServiceProvider,
                    cultureName
                );

                foreach (var contentContributorType in Options.ContentContributors.Reverse())
                {
                    var contributor = (ITemplateContentContributor) scope.ServiceProvider
                            .GetRequiredService(contentContributorType);

                    var templateString = await contributor.GetOrNullAsync(context);
                    if (templateString != null)
                    {
                        return templateString;
                    }
                }
            }
            
            throw new AbpException(
                $"None of the template content contributors could get the content for the template '{templateDefinition.Name}'"
            );
        }
    }
}