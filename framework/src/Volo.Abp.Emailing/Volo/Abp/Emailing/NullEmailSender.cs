using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Emailing;

/// <summary>
/// This class is an implementation of <see cref="IEmailSender"/> as similar to null pattern.
/// It does not send emails but logs them.
/// </summary>
public class NullEmailSender : EmailSenderBase
{
    /// <summary>
    /// Creates a new <see cref="NullEmailSender"/> object.
    /// </summary>
    public NullEmailSender(ICurrentTenant currentTenant, IEmailSenderConfiguration configuration, IBackgroundJobManager backgroundJobManager)
        : base(currentTenant, configuration, backgroundJobManager)
    {

    }

    protected override Task SendEmailAsync(MailMessage mail)
    {
        Logger.LogWarning("USING NullEmailSender!");
        Logger.LogDebug("SendEmailAsync:");
        LogEmail(mail);
        return Task.FromResult(0);
    }

    private void LogEmail(MailMessage mail)
    {
        Logger.LogDebug(mail.To.ToString());
        Logger.LogDebug(mail.CC.ToString());
        Logger.LogDebug(mail.Subject);
        Logger.LogDebug(mail.Body);
    }
}
