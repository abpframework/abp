using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Volo.Abp.Emailing;

namespace Volo.Abp.MailKit;

public interface IMailKitSmtpEmailSender : IEmailSender
{
    Task<SmtpClient> BuildClientAsync();
}
