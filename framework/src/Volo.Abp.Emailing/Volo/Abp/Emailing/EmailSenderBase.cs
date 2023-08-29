using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Data;

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

    public virtual async Task SendAsync(string to, string? subject, string? body, bool isBodyHtml = true, List<EmailAttachment>? attachments = null, ExtraPropertyDictionary? extraProperties = null)
    {
        await SendAsync(BuildMailMessage(null, to, subject, body, isBodyHtml, attachments, extraProperties));
    }

    public virtual async Task SendAsync(string from, string to, string? subject, string? body, bool isBodyHtml = true, List<EmailAttachment>? attachments = null, ExtraPropertyDictionary? extraProperties = null)
    {
        await SendAsync(BuildMailMessage(from, to, subject, body, isBodyHtml, attachments, extraProperties));
    }

    protected virtual MailMessage BuildMailMessage(string? from, string to, string? subject, string? body, bool isBodyHtml = true, List<EmailAttachment>? attachments = null, ExtraPropertyDictionary? extraProperties = null)
    {
        var message = from == null
            ? new MailMessage { To = { to }, Subject = subject, Body = body, IsBodyHtml = isBodyHtml }
            : new MailMessage(from, to, subject, body) { IsBodyHtml = isBodyHtml };

        if (attachments != null)
        {
            foreach (var attachment in attachments.Where(x => x.File != null))
            {
                var fileStream = new MemoryStream(attachment.File!);
                fileStream.Seek(0, SeekOrigin.Begin);
                message.Attachments.Add(new Attachment(fileStream, attachment.Name));
            }
        }

        return message;
    }

    public virtual async Task SendAsync(MailMessage mail, bool normalize = true)
    {
        if (normalize)
        {
            await NormalizeMailAsync(mail);
        }

        await SendEmailAsync(mail);
    }

    public virtual async Task QueueAsync(string to, string subject, string body, bool isBodyHtml = true, List<EmailAttachment>? attachments = null, ExtraPropertyDictionary? extraProperties = null)
    {
        if (!BackgroundJobManager.IsAvailable())
        {
            await SendAsync(to, subject, body, isBodyHtml, attachments, extraProperties);
            return;
        }

        await BackgroundJobManager.EnqueueAsync(
            new BackgroundEmailSendingJobArgs
            {
                To = to,
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml,
                Attachments = attachments,
                ExtraProperties = extraProperties
            }
        );
    }

    public virtual async Task QueueAsync(string from, string to, string subject, string body, bool isBodyHtml = true, List<EmailAttachment>? attachments = null, ExtraPropertyDictionary? extraProperties = null)
    {
        if (!BackgroundJobManager.IsAvailable())
        {
            await SendAsync(from, to, subject, body, isBodyHtml, attachments, extraProperties);
            return;
        }

        await BackgroundJobManager.EnqueueAsync(
            new BackgroundEmailSendingJobArgs
            {
                From = from,
                To = to,
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml,
                Attachments = attachments,
                ExtraProperties = extraProperties
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
