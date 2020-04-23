using Shouldly;
using Xunit;

namespace Volo.Abp.TextTemplating.VirtualFiles
{
    public class VirtualFileTemplateContributor_Tests : AbpTextTemplatingTestBase
    {
        [Fact]
        public void Should_Get_Localized_Content_By_Culture()
        {
            var contributor = new VirtualFileTemplateContributor(
                "/SampleTemplates/WelcomeEmail"
            );

            contributor.Initialize(
                new TemplateContributorInitializationContext(
                    new TemplateDefinition("Test"),
                    ServiceProvider
                )
            );

            contributor
                .GetOrNull("en")
                .ShouldBe("Welcome {{model.name}} to the abp.io!");

            contributor
                .GetOrNull("tr")
                .ShouldBe("Merhaba {{model.name}}, abp.io'ya hoşgeldiniz!");
        }

        [Fact]
        public void Should_Get_Non_Localized_Template_Content()
        {
            var contributor = new VirtualFileTemplateContributor(
                "/SampleTemplates/ForgotPasswordEmail.tpl"
            );

            contributor.Initialize(
                new TemplateContributorInitializationContext(
                    new TemplateDefinition("Test"),
                    ServiceProvider
                )
            );

            contributor
                .GetOrNull()
                .ShouldBe("{{l \"HelloText\"}}. Please click to the following link to get an email to reset your password!");
        }
    }
}
