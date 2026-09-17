using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Volo.Abp.TextTemplating;
using Xunit;

namespace Volo.Abp.Emailing.Templates;

public abstract class AbpEmailingTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}

public class StandardEmailTemplates_Tests : AbpEmailingTestBase<AbpEmailingTestModule>
{
    private readonly ITemplateRenderer _templateRenderer;

    public StandardEmailTemplates_Tests()
    {
        _templateRenderer = GetRequiredService<ITemplateRenderer>();
    }

    [Theory]
    [InlineData("en", "<html lang=\"en\" dir=\"ltr\"")]
    [InlineData("fr", "<html lang=\"fr\" dir=\"ltr\"")]
    [InlineData("ar", "<html lang=\"ar\" dir=\"rtl\"")]
    public async Task Should_Declare_The_Language_And_The_Direction_Of_The_Rendering_Culture(
        string cultureName, string expectedHtmlTag)
    {
        var result = await _templateRenderer.RenderAsync(
            StandardEmailTemplates.Message,
            model: new { message = "This is email body..." },
            cultureName: cultureName
        );

        result.ShouldContain(expectedHtmlTag);
        result.ShouldContain("This is email body...");
    }
}
