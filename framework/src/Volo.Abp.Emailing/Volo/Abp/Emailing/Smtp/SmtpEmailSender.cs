using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

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
            var host = await SmtpConfiguration.GetHostAsync().ConfigureAwait(false);
            var port = await SmtpConfiguration.GetPortAsync().ConfigureAwait(false);

            var smtpClient = new SmtpClient(host, port);

            try
            {
                if (await SmtpConfiguration.GetEnableSslAsync().ConfigureAwait(false))
                {
                    smtpClient.EnableSsl = true;
                }

                if (await SmtpConfiguration.GetUseDefaultCredentialsAsync().ConfigureAwait(false))
                {
                    smtpClient.UseDefaultCredentials = true;
                }
                else
                {
                    smtpClient.UseDefaultCredentials = false;

                    var userName = await SmtpConfiguration.GetUserNameAsync().ConfigureAwait(false);
                    if (!userName.IsNullOrEmpty())
                    {
                        var password = await SmtpConfiguration.GetPasswordAsync().ConfigureAwait(false);
                        var domain = await SmtpConfiguration.GetDomainAsync().ConfigureAwait(false);
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
            using (var smtpClient = await BuildClientAsync().ConfigureAwait(false))
            {
                await smtpClient.SendMailAsync(mail).ConfigureAwait(false);
            }
        }
    }
}
