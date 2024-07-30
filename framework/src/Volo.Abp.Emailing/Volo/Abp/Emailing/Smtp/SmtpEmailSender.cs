using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Emailing.Smtp;

/// <summary>
/// Used to send emails over SMTP.
/// </summary>
public class SmtpEmailSender : EmailSenderBase, ISmtpEmailSender, ITransientDependency
{
    protected ISmtpEmailSenderConfiguration SmtpConfiguration { get; }

    /// <summary>
    /// Creates a new <see cref="SmtpEmailSender"/>.
    /// </summary>
    public SmtpEmailSender(
        ICurrentTenant currentTenant,
        ISmtpEmailSenderConfiguration smtpConfiguration,
        IBackgroundJobManager backgroundJobManager)
        : base(currentTenant, smtpConfiguration, backgroundJobManager)
    {
        SmtpConfiguration = smtpConfiguration;
    }

    public async Task<SmtpClient> BuildClientAsync()
    {
        var host = await SmtpConfiguration.GetHostAsync();
        var port = await SmtpConfiguration.GetPortAsync();

        var smtpClient = new SmtpClient(host, port);

        try
        {
            if (await SmtpConfiguration.GetEnableSslAsync())
            {
                smtpClient.EnableSsl = true;
            }

            if (await SmtpConfiguration.GetUseDefaultCredentialsAsync())
            {
                smtpClient.UseDefaultCredentials = true;
            }
            else
            {
                smtpClient.UseDefaultCredentials = false;

                var userName = await SmtpConfiguration.GetUserNameAsync();
                if (!userName.IsNullOrEmpty())
                {
                    var password = await SmtpConfiguration.GetPasswordAsync();
                    var domain = await SmtpConfiguration.GetDomainAsync();
                    smtpClient.Credentials = !domain.IsNullOrEmpty()
                        ? new NetworkCredential(userName, password, domain)
                        : new NetworkCredential(userName, password);
                }
            }

            return smtpClient;
        }
        catch
        {
            smtpClient.Dispose();
            throw;
        }
    }

    protected async override Task SendEmailAsync(MailMessage mail)
    {
        using (var smtpClient = await BuildClientAsync())
        {
            Logger.LogWarning("We don't recommend that you use the SmtpClient class for new development because SmtpClient doesn't support many modern protocols. " +
                               "Use MailKit(https://abp.io/docs/latest/framework/infrastructure/mail-kit) or other libraries instead." +
                               "For more information, see https://github.com/dotnet/platform-compat/blob/master/docs/DE0005.md");

            await smtpClient.SendMailAsync(mail);
        }
    }
}
