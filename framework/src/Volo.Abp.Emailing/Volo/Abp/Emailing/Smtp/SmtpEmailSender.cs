using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Emailing.Smtp
{
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
            ISmtpEmailSenderConfiguration smtpConfiguration,
            IBackgroundJobManager backgroundJobManager)
            : base(smtpConfiguration, backgroundJobManager)
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

        protected override async Task SendEmailAsync(MailMessage mail)
        {
            using (var smtpClient = await BuildClientAsync())
            {
                await smtpClient.SendMailAsync(mail);
            }
        }
    }
}
