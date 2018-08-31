using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Emailing.Templates;
using Xunit;

namespace Volo.Abp.Emailing
{
    public class EmailTemplateStore_Tests : AbpIntegratedTest<AbpEmailingTestModule>
    {
        private readonly IEmailTemplateStore _emailTemplateStore;

        public EmailTemplateStore_Tests()
        {
            _emailTemplateStore = GetRequiredService<IEmailTemplateStore>();
        }

        [Fact]
        public async Task Should_Get_Registered_Template()
        {
            var template = await _emailTemplateStore.GetAsync("template1");
            template.Content.ShouldContain("This is a test template!");
        }
    }
}
