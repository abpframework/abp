using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Volo.Abp.Emailing
{
    /// <summary>
    /// This class is an implementation of <see cref="IEmailSender"/> as similar to null pattern.
    /// It does not send emails but logs them.
    /// </summary>
    public class NullEmailSender : EmailSenderBase
    {
        public ILogger<NullEmailSender> Logger { get; set; }

        /// <summary>
        /// Creates a new <see cref="NullEmailSender"/> object.
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public NullEmailSender(IEmailSenderConfiguration configuration)
            : base(configuration)
        {
            Logger = NullLogger<NullEmailSender>.Instance;
        }

        protected override Task SendEmailAsync(MailMessage mail)
        {
            Logger.LogWarning("USING NullEmailSender!");
            Logger.LogDebug("SendEmailAsync:");
            LogEmail(mail);
            return Task.FromResult(0);
        }

        protected override void SendEmail(MailMessage mail)
        {
            Logger.LogWarning("USING NullEmailSender!");
            Logger.LogWarning("SendEmail:");
            LogEmail(mail);
        }

        private void LogEmail(MailMessage mail)
        {
            Logger.LogDebug(mail.To.ToString());
            Logger.LogDebug(mail.CC.ToString());
            Logger.LogDebug(mail.Subject);
            Logger.LogDebug(mail.Body);
        }
    }
}