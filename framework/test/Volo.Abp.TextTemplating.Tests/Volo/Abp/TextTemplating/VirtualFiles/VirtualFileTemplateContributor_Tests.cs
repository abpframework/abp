using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.TextTemplating.VirtualFiles
{
    public class VirtualFileTemplateContributor_Tests : AbpTextTemplatingTestBase
    {
        private readonly ITemplateDefinitionManager _templateDefinitionManager;
        private readonly VirtualFileTemplateContentContributor _virtualFileTemplateContentContributor;

        public VirtualFileTemplateContributor_Tests()
        {
            _templateDefinitionManager = GetRequiredService<ITemplateDefinitionManager>();
            _virtualFileTemplateContentContributor = GetRequiredService<VirtualFileTemplateContentContributor>();
        }

        [Fact]
        public async Task Should_Get_Localized_Content_By_Culture()
        {
            (await _virtualFileTemplateContentContributor.GetOrNullAsync(
                    new TemplateContentContributorContext(_templateDefinitionManager.Get(TestTemplates.WelcomeEmail),
                        ServiceProvider,
                        "en")))
                .ShouldBe("Welcome {{model.name}} to the abp.io!");

            (await _virtualFileTemplateContentContributor.GetOrNullAsync(
                    new TemplateContentContributorContext(_templateDefinitionManager.Get(TestTemplates.WelcomeEmail),
                        ServiceProvider,
                        "tr")))
                .ShouldBe("Merhaba {{model.name}}, abp.io'ya hoşgeldiniz!");
        }

        [Fact]
        public async Task Should_Get_Non_Localized_Template_Content()
        {
            (await _virtualFileTemplateContentContributor.GetOrNullAsync(
                    new TemplateContentContributorContext(
                        _templateDefinitionManager.Get(TestTemplates.ForgotPasswordEmail),
                        ServiceProvider,
                        null)))
                .ShouldBe("{{L \"HelloText\" model.name}}, {{L \"HowAreYou\" }}. Please click to the following link to get an email to reset your password!");
        }
    }
}
