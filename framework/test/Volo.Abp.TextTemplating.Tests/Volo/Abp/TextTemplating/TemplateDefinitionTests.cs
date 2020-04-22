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
        public void Should_Retrieve_Template_Definition_By_Name()
        {
            var definition = _templateDefinitionManager.Get(TestTemplates.TestTemplate1);
            definition.Name.ShouldBe(TestTemplates.TestTemplate1);
        }

        [Fact]
        public void Should_Get_Null_If_Template_Not_Found()
        {
            var definition = _templateDefinitionManager.GetOrNull("undefined-template");
            definition.ShouldBeNull();
        }

        [Fact]
        public void Should_Retrieve_All_Template_Definitions()
        {
            var definitions = _templateDefinitionManager.GetAll();
            definitions.Count.ShouldBeGreaterThan(1);
        }
    }
}