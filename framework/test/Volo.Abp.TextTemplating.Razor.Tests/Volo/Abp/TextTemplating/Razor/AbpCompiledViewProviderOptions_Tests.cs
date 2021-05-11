using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.TextTemplating.Razor.SampleTemplates;
using Xunit;

namespace Volo.Abp.TextTemplating.Razor
{
    public class AbpCompiledViewProviderOptions_Tests : TemplateDefinitionTests<RazorTextTemplatingTestModule>
    {
        private readonly IAbpCompiledViewProvider _compiledViewProvider;
        private readonly ITemplateDefinitionManager _templateDefinitionManager;

        public AbpCompiledViewProviderOptions_Tests()
        {
            _templateDefinitionManager = GetRequiredService<ITemplateDefinitionManager>();
            _compiledViewProvider = GetRequiredService<IAbpCompiledViewProvider>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.Configure<AbpCompiledViewProviderOptions>(options =>
            {
                options.TemplateReferences.Add(RazorTestTemplates.TestTemplate, new List<Assembly>()
                    {
                        Assembly.Load("Microsoft.Extensions.Logging.Abstractions")
                    }
                    .Select(x => MetadataReference.CreateFromFile(x.Location))
                    .ToList());
            });
            base.AfterAddApplication(services);
        }

        [Fact]
        public async Task Custom_TemplateReferences_Test()
        {
            var templateDefinition = _templateDefinitionManager.GetOrNull(RazorTestTemplates.TestTemplate);

            var assembly = await _compiledViewProvider.GetAssemblyAsync(templateDefinition);

            assembly.GetReferencedAssemblies().ShouldContain(x => x.Name == "Microsoft.Extensions.Logging.Abstractions");
        }
    }
}
