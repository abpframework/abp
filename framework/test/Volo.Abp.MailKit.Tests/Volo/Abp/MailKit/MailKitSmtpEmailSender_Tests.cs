using System.Net.Mail;
using System.Threading.Tasks;
using NSubstitute;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing.Smtp;
using Xunit;

namespace Volo.Abp.MailKit
{
    //Tests are commented because those tests can pass only when a true email configuration is set.
    public class MailKitSmtpEmailSender_Tests : AbpIntegratedTest<AbpMailKitTestModule>
    {
        //[Fact]
        public async Task ShouldSendMailMessageAsync()
        {
            var mailSender = CreateMailKitEmailSender();
            var mailMessage = new MailMessage("from_mail_address@asd.com", "to_mail_address@asd.com", "subject", "body")
                { IsBodyHtml = true };

            await mailSender.SendAsync(mailMessage);
        }

        //[Fact]
        public void ShouldSendMailMessage()
        {
            var mailSender = CreateMailKitEmailSender();
            var mailMessage = new MailMessage("from_mail_address@asd.com", "to_mail_address@asd.com", "subject", "body")
                { IsBodyHtml = true };

            mailSender.Send(mailMessage);
        }

        private static MailKitSmtpEmailSender CreateMailKitEmailSender()
        {
            var mailConfig = Substitute.For<ISmtpEmailSenderConfiguration>();
            var mailKitConfig = Substitute.For<IAbpMailKitConfiguration>();
            var bgJob = Substitute.For<IBackgroundJobManager>();

            mailConfig.GetHostAsync().Returns("stmp_server_name");
            mailConfig.GetUserNameAsync().Returns("mail_server_user_name");
            mailConfig.GetPasswordAsync().Returns("mail_server_password");
            mailConfig.GetPortAsync().Returns(587);
            mailConfig.GetEnableSslAsync().Returns(false);

            var mailSender = new MailKitSmtpEmailSender(mailConfig, bgJob, mailKitConfig);
            return mailSender;
        }
    }
}
