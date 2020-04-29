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
            var welcomeEmailTemplate = _templateDefinitionManager.Get(TestTemplates.WelcomeEmail);
            welcomeEmailTemplate.Name.ShouldBe(TestTemplates.WelcomeEmail);
            welcomeEmailTemplate.IsInlineLocalized.ShouldBeFalse();

            var forgotPasswordEmailTemplate = _templateDefinitionManager.Get(TestTemplates.ForgotPasswordEmail);
            forgotPasswordEmailTemplate.Name.ShouldBe(TestTemplates.ForgotPasswordEmail);
            forgotPasswordEmailTemplate.IsInlineLocalized.ShouldBeTrue();
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