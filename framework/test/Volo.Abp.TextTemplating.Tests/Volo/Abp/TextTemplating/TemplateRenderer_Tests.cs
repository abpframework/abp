using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.TextTemplating
{
    public class TemplateRenderer_Tests : AbpTextTemplatingTestBase
    {
        private readonly ITemplateRenderer _templateRenderer;

        public TemplateRenderer_Tests()
        {
            _templateRenderer = GetRequiredService<ITemplateRenderer>();
        }

        [Fact]
        public async Task Should_Get_Rendered_Non_Localized_Template_Content()
        {
            var content = await _templateRenderer.RenderAsync(TestTemplates.ForgotPasswordEmail);
            content.ShouldBe("Please click to the following link to get an email to reset your password!");
        }
    }
}
