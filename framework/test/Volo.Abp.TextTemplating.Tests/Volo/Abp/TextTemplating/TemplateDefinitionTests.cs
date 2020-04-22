using Shouldly;
using Xunit;

namespace Volo.Abp.TextTemplating
{
    public class TemplateDefinitionTests : AbpTextTemplatingTestBase
    {
        private readonly ITemplateDefinitionManager _templateDefinitionManager;

        public TemplateDefinitionTests()
        {
            _templateDefinitionManager = GetRequiredService<ITemplateDefinitionManager>();
        }

        [Fact]
        public void Should_Retrieve_Template_Definition()
        {
            var definition = _templateDefinitionManager.Get(TestTemplates.TestTemplate1);
            definition.Name.ShouldBe(TestTemplates.TestTemplate1);
        }
    }
}