using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.TextTemplating.VirtualFiles
{
    //TODO: Make tests running again!
    //public class VirtualFileTemplateContributor_Tests : AbpTextTemplatingTestBase
    //{
    //    [Fact]
    //    public async Task Should_Get_Localized_Content_By_Culture()
    //    {
    //        var contributor = new VirtualFileTemplateContentContributor(
    //            "/SampleTemplates/WelcomeEmail"
    //        );

    //        contributor.Initialize(
    //            new TemplateContentContributorInitializationContext(
    //                new TemplateDefinition("Test"),
    //                ServiceProvider
    //            )
    //        );

    //        (await contributor
    //                .GetOrNullAsync("en")).ShouldBe("Welcome {{model.name}} to the abp.io!");

    //        (await contributor
    //                .GetOrNullAsync("tr")).ShouldBe("Merhaba {{model.name}}, abp.io'ya hoşgeldiniz!");
    //    }

    //    [Fact]
    //    public async Task Should_Get_Non_Localized_Template_Content()
    //    {
    //        var contributor = new VirtualFileTemplateContentContributor(
    //            "/SampleTemplates/ForgotPasswordEmail.tpl"
    //        );

    //        contributor.Initialize(
    //            new TemplateContentContributorInitializationContext(
    //                new TemplateDefinition("Test"),
    //                ServiceProvider
    //            )
    //        );

    //        (await contributor
    //                .GetOrNullAsync()).ShouldBe("{{l \"HelloText\"}}. Please click to the following link to get an email to reset your password!");
    //    }
    //}
}
