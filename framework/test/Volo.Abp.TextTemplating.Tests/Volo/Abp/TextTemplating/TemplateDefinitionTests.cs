using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.TextTemplating;

public abstract class TemplateDefinitionTests<TStartupModule> : AbpTextTemplatingTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected readonly ITemplateDefinitionManager TemplateDefinitionManager;

    protected TemplateDefinitionTests()
    {
        TemplateDefinitionManager = GetRequiredService<ITemplateDefinitionManager>();
    }

    [Fact]
    public void Should_Retrieve_Template_Definition_By_Name()
    {
        var welcomeEmailTemplate = TemplateDefinitionManager.Get(TestTemplates.WelcomeEmail);
        welcomeEmailTemplate.Name.ShouldBe(TestTemplates.WelcomeEmail);
        welcomeEmailTemplate.IsInlineLocalized.ShouldBeFalse();

        var forgotPasswordEmailTemplate = TemplateDefinitionManager.Get(TestTemplates.ForgotPasswordEmail);
        forgotPasswordEmailTemplate.Name.ShouldBe(TestTemplates.ForgotPasswordEmail);
        forgotPasswordEmailTemplate.IsInlineLocalized.ShouldBeTrue();
    }

    [Fact]
    public void Should_Get_Null_If_Template_Not_Found()
    {
        var definition = TemplateDefinitionManager.GetOrNull("undefined-template");
        definition.ShouldBeNull();
    }

    [Fact]
    public void Should_Retrieve_All_Template_Definitions()
    {
        var definitions = TemplateDefinitionManager.GetAll();
        definitions.Count.ShouldBeGreaterThan(1);
    }
}
