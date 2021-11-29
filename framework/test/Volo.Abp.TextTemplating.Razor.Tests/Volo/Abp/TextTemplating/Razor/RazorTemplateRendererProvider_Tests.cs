using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.TextTemplating.Razor
{
    public class RazorTemplateRendererProvider_Tests : AbpTextTemplatingTestBase<RazorTextTemplatingTestModule>
    {
        private readonly ITemplateRenderer _templateRenderer;

        public RazorTemplateRendererProvider_Tests()
        {
            _templateRenderer = GetRequiredService<ITemplateRenderer>();
        }

        [Fact]
        public async Task Should_Get_Rendered_Localized_Template_Content_With_Different_Cultures()
        {
            (await _templateRenderer.RenderAsync(
                TestTemplates.WelcomeEmail,
                model: new WelcomeEmailModel()
                {
                    Name = "John"
                },
                cultureName: "en"
            )).ShouldBe("Welcome John to the abp.io!");

            (await _templateRenderer.RenderAsync(
                TestTemplates.WelcomeEmail,
                model: new WelcomeEmailModel()
                {
                    Name = "John"
                },
                cultureName: "tr"
            )).ShouldBe("Merhaba John, abp.io'ya hoşgeldiniz!");

            //"en-US" fallbacks to "en" since "en-US" doesn't exists and "en" is the fallback culture
            (await _templateRenderer.RenderAsync(
                TestTemplates.WelcomeEmail,
                model: new WelcomeEmailModel()
                {
                    Name = "John"
                },
                cultureName: "en-US"
            )).ShouldBe("Welcome John to the abp.io!");

            //"fr" fallbacks to "en" since "fr" doesn't exists and "en" is the default culture
            (await _templateRenderer.RenderAsync(
                TestTemplates.WelcomeEmail,
                model: new WelcomeEmailModel()
                {
                    Name = "John"
                },
                cultureName: "fr"
            )).ShouldBe("Welcome John to the abp.io!");
        }

        [Fact]
        public async Task Should_Get_Rendered_Localized_Template_Content_With_Stronly_Typed_Model()
        {
            (await _templateRenderer.RenderAsync(
                TestTemplates.WelcomeEmail,
                model: new WelcomeEmailModel("John"),
                cultureName: "en"
            )).ShouldBe("Welcome John to the abp.io!");
        }

        [Fact]
        public async Task Should_Get_Rendered_Inline_Localized_Template()
        {
            (await _templateRenderer.RenderAsync(
                TestTemplates.ForgotPasswordEmail,
                new ForgotPasswordEmailModel("John"),
                cultureName: "en"
            )).ShouldBe("*BEGIN*Hello John, how are you?. Please click to the following link to get an email to reset your password!<a target=\"_blank\" href=\"https://abp.io/Account/ResetPassword\">Reset your password</a>*END*");

            (await _templateRenderer.RenderAsync(
                TestTemplates.ForgotPasswordEmail,
                new ForgotPasswordEmailModel("John"),
                cultureName: "tr"
            )).ShouldBe("*BEGIN*Merhaba John, nasılsın?. Please click to the following link to get an email to reset your password!<a target=\"_blank\" href=\"https://abp.io/Account/ResetPassword\">Reset your password</a>*END*");
        }

        [Fact]
        public async Task Should_Get_Localized_Numbers()
        {
            (await _templateRenderer.RenderAsync(
                TestTemplates.ShowDecimalNumber,
                new ShowDecimalNumberModel(123.45M),
                cultureName: "en"
            )).ShouldBe("*BEGIN*123.45*END*");

            (await _templateRenderer.RenderAsync(
                TestTemplates.ShowDecimalNumber,
                new ShowDecimalNumberModel(123.45M),
                cultureName: "de"
            )).ShouldBe("*BEGIN*123,45*END*");
        }

        public class WelcomeEmailModel
        {
            public string Name { get; set; }

            public WelcomeEmailModel()
            {

            }

            public WelcomeEmailModel(string name)
            {
                Name = name;
            }
        }

        public class ForgotPasswordEmailModel
        {
            public string Name { get; set; }

            public ForgotPasswordEmailModel()
            {

            }

            public ForgotPasswordEmailModel(string name)
            {
                Name = name;
            }
        }

        public class ShowDecimalNumberModel
        {
            public decimal Amount { get; set; }

            public ShowDecimalNumberModel()
            {

            }

            public ShowDecimalNumberModel(decimal amount)
            {
                Amount = amount;
            }
        }
    }
}
