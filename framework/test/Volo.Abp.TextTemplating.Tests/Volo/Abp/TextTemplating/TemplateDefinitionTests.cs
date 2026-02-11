using System.Threading.Tasks;
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
    public async Task Should_Retrieve_Template_Definition_By_Name()
    {
        var welcomeEmailTemplate = await TemplateDefinitionManager.GetAsync(TestTemplates.WelcomeEmail);
        welcomeEmailTemplate.Name.ShouldBe(TestTemplates.WelcomeEmail);
        welcomeEmailTemplate.IsInlineLocalized.ShouldBeFalse();

        var forgotPasswordEmailTemplate = await TemplateDefinitionManager.GetAsync(TestTemplates.ForgotPasswordEmail);
        forgotPasswordEmailTemplate.Name.ShouldBe(TestTemplates.ForgotPasswordEmail);
        forgotPasswordEmailTemplate.IsInlineLocalized.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Get_Null_If_Template_Not_Found()
    {
        var definition = await TemplateDefinitionManager.GetOrNullAsync("undefined-template");
        definition.ShouldBeNull();
    }

    [Fact]
    public async Task  Should_Retrieve_All_Template_Definitions()
    {
        var definitions = await TemplateDefinitionManager.GetAllAsync();
        definitions.Count.ShouldBeGreaterThan(1);
    }
}
