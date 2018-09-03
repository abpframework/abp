using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Emailing.Templates;
using Xunit;

namespace Volo.Abp.Emailing
{
    public class EmailTemplateStore_Tests : AbpIntegratedTest<AbpEmailingTestModule>
    {
        private readonly IEmailTemplateProvider _emailTemplateProvider;

        public EmailTemplateStore_Tests()
        {
            _emailTemplateProvider = GetRequiredService<IEmailTemplateProvider>();
        }

        [Fact]
        public async Task Should_Get_Registered_Template()
        {
            var template = await _emailTemplateProvider.GetAsync("template1");
            template.Content.ShouldContain("This is a test template!");
        }
    }
}
