using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Emailing.Templates;
using Xunit;

namespace Volo.Abp.Emailing
{
    public class EmailTemplateStore_Tests : AbpIntegratedTest<AbpEmailingTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        private readonly IEmailTemplateProvider _emailTemplateProvider;

        public EmailTemplateStore_Tests()
        {
            _emailTemplateProvider = GetRequiredService<IEmailTemplateProvider>();
        }

        [Fact]
        public async Task Should_Get_Registered_Template()
        {
            var template = await _emailTemplateProvider.GetAsync("template1", "tr");
            template.Content.ShouldContain("Lütfen aşağıdaki bağlantıya tıklayarak e-posta adresinizi onaylayın.");
        }

        [Fact]
        public async Task Should_Get_Default_Culture_Template()
        {
            var template = await _emailTemplateProvider.GetAsync("template1", "zh-Hans");
            template.Content.ShouldContain("Please confirm your email address by clicking the link below.");
        }

        [Fact]
        public async Task Should_Get_Registered_Template_With_Layout()
        {
            var template = await _emailTemplateProvider.GetAsync("template2", "en");

            template.Content.ShouldContain($"<body>{Environment.NewLine}    " + "Please confirm your email address by clicking the link below.");
        }


        [Fact]
        public async Task Should_Get_Registered_Template_With_Localize()
        {
            var template = await _emailTemplateProvider.GetAsync("template3", "tr");
            template.Content.ShouldContain("Merhaba Abp");
        }
    }
}
