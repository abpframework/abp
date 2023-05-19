using System;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;

namespace Volo.Abp.Emailing;

/// <summary>
/// This class can be used as base to implement <see cref="IEmailSender"/>.
/// </summary>
public abstract class EmailSenderBase : IEmailSender
{
    protected IEmailSenderConfiguration Configuration { get; }

    protected IBackgroundJobManager BackgroundJobManager { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    protected EmailSenderBase(IEmailSenderConfiguration configuration, IBackgroundJobManager backgroundJobManager)
    {
        Configuration = configuration;
        BackgroundJobManager = backgroundJobManager;
    }

    public virtual async Task SendAsync(string to, string subject, string body, bool isBodyHtml = true)
    {
        await SendAsync(new MailMessage
        {
            To = { to },
            Subject = subject,
            Body = body,
            IsBodyHtml = isBodyHtml
        });
    }

    public virtual async Task SendAsync(string from, string to, string subject, string body, bool isBodyHtml = true)
    {
        await SendAsync(new MailMessage(from, to, subject, body) { IsBodyHtml = isBodyHtml });
    }

    public virtual async Task SendAsync(MailMessage mail, bool normalize = true)
    {
        if (normalize)
        {
            await NormalizeMailAsync(mail);
        }

        await SendEmailAsync(mail);
    }

    public virtual async Task QueueAsync(string to, string subject, string body, bool isBodyHtml = true)
    {
        if (!BackgroundJobManager.IsAvailable())
        {
            await SendAsync(to, subject, body, isBodyHtml);
            return;
        }

        await BackgroundJobManager.EnqueueAsync(
            new BackgroundEmailSendingJobArgs
            {
                To = to,
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            }
        );
    }

    public virtual async Task QueueAsync(string from, string to, string subject, string body, bool isBodyHtml = true)
    {
        if (!BackgroundJobManager.IsAvailable())
        {
            await SendAsync(from, to, subject, body, isBodyHtml);
            return;
        }

        await BackgroundJobManager.EnqueueAsync(
            new BackgroundEmailSendingJobArgs
            {
                From = from,
                To = to,
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            }
        );
    }

    /// <summary>
    /// Should implement this method to send email in derived classes.
    /// </summary>
    /// <param name="mail">Mail to be sent</param>
    protected abstract Task SendEmailAsync(MailMessage mail);

    /// <summary>
    /// Normalizes given email.
    /// Fills <see cref="MailMessage.From"/> if it's not filled before.
    /// Sets encodings to UTF8 if they are not set before.
    /// </summary>
    /// <param name="mail">Mail to be normalized</param>
    protected virtual async Task NormalizeMailAsync(MailMessage mail)
    {
        if (mail.From == null || mail.From.Address.IsNullOrEmpty())
        {
            mail.From = new MailAddress(
                await Configuration.GetDefaultFromAddressAsync(),
                await Configuration.GetDefaultFromDisplayNameAsync(),
                Encoding.UTF8
                );
        }

        if (mail.HeadersEncoding == null)
        {
            mail.HeadersEncoding = Encoding.UTF8;
        }

        if (mail.SubjectEncoding == null)
        {
            mail.SubjectEncoding = Encoding.UTF8;
        }

        if (mail.BodyEncoding == null)
        {
            mail.BodyEncoding = Encoding.UTF8;
        }
    }
}
