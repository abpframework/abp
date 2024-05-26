using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
        NormalizeMailForBase64Image(mail);
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

    private static void NormalizeMailForBase64Image(MailMessage mail)
    {
        var htmlBody = mail.Body;
        Dictionary<string, byte[]> inlineImages = [];

        // Find all base64 image tags in the html message.
        var base64ImageTags = Regex.Matches(htmlBody, @"<img src=""data:image/(png|jpeg|gif);base64,([^""]+)""[^>]*>");

        foreach (Match base64ImageTag in base64ImageTags)
        {
            // Extract the base64-encoded image data from the tag.
            var format = base64ImageTag.Groups[1].Value;
            var base64Data = base64ImageTag.Groups[2].Value;

            // Convert the base64 data to binary.
            var imageByte = Convert.FromBase64String(base64Data);

            // Create a unique image name and a MemoryStream containing the image data.
            var imageName = Guid.NewGuid().ToString();
            var imageStream = new MemoryStream(imageByte);

            // Replace the base64 image tag with cid: image tag.
            var newImageTag = base64ImageTag.Value.Replace(@"src=""data:image/" + format + @";base64,[^""]+""", $"src=\"cid:{imageName}\"");
            htmlBody = htmlBody.Replace(base64ImageTag.Value, newImageTag);

            inlineImages[imageName] = imageByte;
        }

        // Attach the inline images
        foreach (var image in inlineImages)
        {
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(
                $"<img src=\"cid:{image.Key}\">", null, "text/html"));
            mail.Attachments.Add(new Attachment(new MemoryStream(image.Value), image.Key, "image"));
        }
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