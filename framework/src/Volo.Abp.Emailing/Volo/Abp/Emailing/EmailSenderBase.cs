using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
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
    /// <summary>
    /// Constructor.
    /// </summary>
    protected EmailSenderBase(ICurrentTenant currentTenant,
                              IEmailSenderConfiguration configuration,
                              IBackgroundJobManager backgroundJobManager)
    {
        Logger = NullLogger<EmailSenderBase>.Instance;

        CurrentTenant = currentTenant;
        Configuration = configuration;
        BackgroundJobManager = backgroundJobManager;
    }

    public ILogger<EmailSenderBase> Logger { get; set; }

    protected IBackgroundJobManager BackgroundJobManager { get; }
    protected IEmailSenderConfiguration Configuration { get; }
    protected ICurrentTenant CurrentTenant { get; }

    public virtual async Task QueueAsync(string to, string subject, string body, bool isBodyHtml = true, AdditionalEmailSendingArgs? additionalEmailSendingArgs = null)
    {
        ValidateEmailAddress(to);

        if (!BackgroundJobManager.IsAvailable())
        {
            await SendAsync(to, subject, body, isBodyHtml, additionalEmailSendingArgs);
            return;
        }

        _ = await BackgroundJobManager.EnqueueAsync(
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
        ValidateEmailAddress(to);

        if (!BackgroundJobManager.IsAvailable())
        {
            await SendAsync(from, to, subject, body, isBodyHtml, additionalEmailSendingArgs);
            return;
        }

        _ = await BackgroundJobManager.EnqueueAsync(
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

    public virtual async Task SendAsync(string to, string? subject, string? body, bool isBodyHtml = true, AdditionalEmailSendingArgs? additionalEmailSendingArgs = null)
    {
        await SendAsync(BuildMailMessage(null, to, subject, body, isBodyHtml, additionalEmailSendingArgs));
    }

    public virtual async Task SendAsync(string from, string to, string? subject, string? body, bool isBodyHtml = true, AdditionalEmailSendingArgs? additionalEmailSendingArgs = null)
    {
        await SendAsync(BuildMailMessage(from, to, subject, body, isBodyHtml, additionalEmailSendingArgs));
    }

    public virtual async Task SendAsync(MailMessage mail, bool normalize = true)
    {
        NormalizeMailForBase64Data(mail);
        if (normalize)
        {
            await NormalizeMailAsync(mail);
        }

        await SendEmailAsync(mail);
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
                    _ = fileStream.Seek(0, SeekOrigin.Begin);
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

    /// <summary>
    /// Normalizes given email. Fills <see cref="MailMessage.From"/> if it's not filled before. Sets
    /// encodings to UTF8 if they are not set before.
    /// </summary>
    /// <param name="mail">Mail to be normalized</param>
    protected virtual async Task NormalizeMailAsync(MailMessage mail)
    {
        if (mail.From == null || mail.From.Address.IsNullOrEmpty())
        {
            mail.From = new MailAddress(await Configuration.GetDefaultFromAddressAsync(),
                                        await Configuration.GetDefaultFromDisplayNameAsync(),
                                        Encoding.UTF8);
        }

        mail.HeadersEncoding ??= Encoding.UTF8;

        mail.SubjectEncoding ??= Encoding.UTF8;

        mail.BodyEncoding ??= Encoding.UTF8;
    }

    /// <summary>
    /// Should implement this method to send email in derived classes.
    /// </summary>
    /// <param name="mail">Mail to be sent</param>
    protected abstract Task SendEmailAsync(MailMessage mail);

    private static void NormalizeMailForBase64Data(MailMessage mail)
    {
        var htmlBody = mail.Body;

        // Find all base64 tags in the html message.
        var base64Tags = Regex.Matches(htmlBody, @"src\s*=\s*(""|')\s*data\s*:\s*(?<mediaType>[\w/\-\.]+);(?<encoding>\w+),(?<data>.*?)(""|')", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        foreach (Match base64Tag in base64Tags)
        {
            // Extract the base64-encoded data from the tag.
            var tag = base64Tag.Groups[0].Value;
            var mediaType = base64Tag.Groups["mediaType"].Value;
            var encoding = base64Tag.Groups["encoding"].Value;
            var data = base64Tag.Groups["data"].Value;

            // Convert the base64 data to binary.
            var dataBytes = Convert.FromBase64String(data);

            // Create a unique name and a MemoryStream containing the data.
            var name = Guid.NewGuid().ToString();
            MemoryStream dataStream = new(dataBytes);

            // Replace the base64 tag with cid: image tag.
            var newTag = base64Tag.Value.Replace(tag, $"src=\"cid:{name}\"");
            // Create the HTML view
            htmlBody = htmlBody.Replace(base64Tag.Value, newTag);
            var htmlView = AlternateView.CreateAlternateViewFromString(
                                                         htmlBody,
                                                         Encoding.UTF8,
                                                         MediaTypeNames.Text.Html);
            // Create a plain text message for client that don't support HTML
            var plainView = AlternateView.CreateAlternateViewFromString(htmlBody,
                                                        Encoding.UTF8,
                                                        MediaTypeNames.Text.Plain);

            LinkedResource img = new(dataStream, mediaType)
            {
                ContentId = name
            };
            img.ContentType.MediaType = mediaType;
            img.TransferEncoding = TransferEncoding.Base64;
            img.ContentType.Name = img.ContentId;
            img.ContentLink = new Uri("cid:" + img.ContentId);
            htmlView.LinkedResources.Add(img);
            mail.AlternateViews.Add(plainView);
            mail.AlternateViews.Add(htmlView);
        }
        mail.Body = htmlBody;
    }

    private static void ValidateEmailAddress(string emailAddress)
    {
        if (ValidationHelper.IsValidEmailAddress(emailAddress))
        {
            return;
        }

        throw new ArgumentException($"Email address '{emailAddress}' is not valid!");
    }
}