using System.Net.Mail;

namespace Volo.Abp.Emailing.Smtp
{
    /// <summary>
    /// Used to send emails over SMTP.
    /// </summary>
    public interface ISmtpEmailSender : IEmailSender
    {
        /// <summary>
        /// Creates and configures new <see cref="SmtpClient"/> object to send emails.
        /// </summary>
        /// <returns>
        /// An <see cref="SmtpClient"/> object that is ready to send emails.
        /// </returns>
        SmtpClient BuildClient();
    }
}