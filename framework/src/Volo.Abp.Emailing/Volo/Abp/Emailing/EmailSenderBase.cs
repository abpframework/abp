using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Validation;

namespace Volo.Abp.Emailing;

/// <summary>
/// This class can be used as base to implement <see cref="IEmailSender"/>.
/// </summary>
public abstract class EmailSenderBase : IEmailSender
{
    public ILogger<EmailSenderBase> Logger { get; set; }

    protected ICurrentTenant CurrentTenant { get; }

    protected IEmailSenderConfiguration Configuration { get; }

    protected IBackgroundJobManager BackgroundJobManager { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    protected EmailSenderBase(
        ICurrentTenant currentTenant,
        IEmailSenderConfiguration configuration,
        IBackgroundJobManager backgroundJobManager)
    {
        Logger = NullLogger<EmailSenderBase>.Instance;

        CurrentTenant = currentTenant;
        Configuration = configuration;
        BackgroundJobManager = backgroundJobManager;
    }

    public virtual async Task SendAsync(string to, string? subject, string? body, bool isBodyHtml = true, AdditionalEmailSendingArgs? additionalEmailSendingArgs = null)
    {
        await SendAsync(BuildMailMessage(null, to, subject, body, isBodyHtml, additionalEmailSendingArgs));
    }

    public virtual async Task SendAsync(string from, string to, string? subject, string? body, bool isBodyHtml = true, AdditionalEmailSendingArgs? additionalEmailSendingArgs = null)
    {
        await SendAsync(BuildMailMessage(from, to, subject, body, isBodyHtml, additionalEmailSendingArgs));
    }

    protected virtual MailMessage BuildMailMessage(string? from, string to, string? subject, string? body, bool isBodyHtml = true, AdditionalEmailSendingArgs? additionalEmailSendingArgs = null)
    {
        var message = from == null
            ? new MailMessage { To = { to }, Subject = subject, Body = body, IsBodyHtml = isBodyHtml }
            : new MailMessage(from, to, subject, body) { IsBodyHtml = isBodyHtml };

        if (additionalEmailSendingArgs != null)
        {
            if (additionalEmailSendingArgs.Attachments != null)
            {
                foreach (var attachment in additionalEmailSendingArgs.Attachments.Where(x => x.File != null))
                {
                    var fileStream = new MemoryStream(attachment.File!);
                    fileStream.Seek(0, SeekOrigin.Begin);
                    message.Attachments.Add(new Attachment(fileStream, attachment.Name));
                }
            }

            if (additionalEmailSendingArgs.CC != null)
            {
                foreach (var cc in additionalEmailSendingArgs.CC)
                {
                    message.CC.Add(cc);
                }
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

    public virtual async Task QueueAsync(string to, string subject, string body, bool isBodyHtml = true, AdditionalEmailSendingArgs? additionalEmailSendingArgs = null)
    {
        await ValidateEmailAddressAsync(to);

        if (!BackgroundJobManager.IsAvailable())
        {
            await SendAsync(to, subject, body, isBodyHtml, additionalEmailSendingArgs);
            return;
        }

        await BackgroundJobManager.EnqueueAsync(
            new BackgroundEmailSendingJobArgs
            {
                TenantId = CurrentTenant.Id,
                To = to,
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml,
                AdditionalEmailSendingArgs = additionalEmailSendingArgs
            }
        );
    }

    public virtual async Task QueueAsync(string from, string to, string subject, string body, bool isBodyHtml = true, AdditionalEmailSendingArgs? additionalEmailSendingArgs = null)
    {
        await ValidateEmailAddressAsync(to);

        if (!BackgroundJobManager.IsAvailable())
        {
            await SendAsync(from, to, subject, body, isBodyHtml, additionalEmailSendingArgs);
            return;
        }

        await BackgroundJobManager.EnqueueAsync(
            new BackgroundEmailSendingJobArgs
            {
                TenantId = CurrentTenant.Id,
                From = from,
                To = to,
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml,
                AdditionalEmailSendingArgs = additionalEmailSendingArgs
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

    protected virtual Task ValidateEmailAddressAsync(string emailAddress)
    {
        try
        {
            _ = new MailAddressCollection { emailAddress };
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            throw new ArgumentException($"Email address '{emailAddress}' is not valid!");
        }
    }
}
