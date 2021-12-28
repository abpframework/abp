using System.Net.Mail;
using System.Threading.Tasks;
using NSubstitute;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing.Smtp;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.MailKit;

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
    public async Task ShouldSendMailMessage()
    {
        var mailSender = CreateMailKitEmailSender();
        var mailMessage = new MailMessage("from_mail_address@asd.com", "to_mail_address@asd.com", "subject", "body")
        {
            IsBodyHtml = true
        };

        await mailSender.SendAsync(mailMessage);
    }

    private static MailKitSmtpEmailSender CreateMailKitEmailSender()
    {
        var mailConfig = Substitute.For<ISmtpEmailSenderConfiguration>();
        var bgJob = Substitute.For<IBackgroundJobManager>();

        mailConfig.GetHostAsync().Returns(Task.FromResult("stmp_server_name"));
        mailConfig.GetUserNameAsync().Returns(Task.FromResult("mail_server_user_name"));
        mailConfig.GetPasswordAsync().Returns(Task.FromResult("mail_server_password"));
        mailConfig.GetPortAsync().Returns(Task.FromResult(587));
        mailConfig.GetEnableSslAsync().Returns(Task.FromResult(false));

        var mailSender = new MailKitSmtpEmailSender(mailConfig, bgJob, null);
        return mailSender;
    }
}
