using System.Net.Mail;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.Emailing.Smtp;
using MailKit.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Volo.Abp.MailKit
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    public class MailKitSmtpEmailSender : EmailSenderBase, IMailKitSmtpEmailSender
    {
        protected AbpMailKitOptions AbpMailKitOptions { get; }

        protected ISmtpEmailSenderConfiguration SmtpConfiguration { get; }

        public MailKitSmtpEmailSender(ISmtpEmailSenderConfiguration smtpConfiguration,
            IBackgroundJobManager backgroundJobManager,
            IOptions<AbpMailKitOptions> abpMailKitConfiguration)
            : base(smtpConfiguration, backgroundJobManager)
        {
            AbpMailKitOptions = abpMailKitConfiguration.Value;
            SmtpConfiguration = smtpConfiguration;
        }

        protected override async Task SendEmailAsync(MailMessage mail)
        {
            using (var client = await BuildClientAsync().ConfigureAwait(false))
            {
                var message = MimeMessage.CreateFromMailMessage(mail);
                await client.SendAsync(message).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }

        public async Task<SmtpClient> BuildClientAsync()
        {
            var client = new SmtpClient();

            try
            {
                await ConfigureClient(client).ConfigureAwait(false);
                return client;
            }
            catch
            {
                client.Dispose();
                throw;
            }
        }

        protected virtual async Task ConfigureClient(SmtpClient client)
        {
            client.Connect(
                await SmtpConfiguration.GetHostAsync().ConfigureAwait(false),
                await SmtpConfiguration.GetPortAsync().ConfigureAwait(false),
                await GetSecureSocketOption()
.ConfigureAwait(false));

            if (await SmtpConfiguration.GetUseDefaultCredentialsAsync().ConfigureAwait(false))
            {
                return;
            }

            client.Authenticate(
                await SmtpConfiguration.GetUserNameAsync().ConfigureAwait(false),
                await SmtpConfiguration.GetPasswordAsync()
.ConfigureAwait(false));
        }

        protected virtual async Task<SecureSocketOptions> GetSecureSocketOption()
        {
            if (AbpMailKitOptions.SecureSocketOption.HasValue)
            {
                return AbpMailKitOptions.SecureSocketOption.Value;
            }

            return await SmtpConfiguration.GetEnableSslAsync()
.ConfigureAwait(false) ? SecureSocketOptions.SslOnConnect
                : SecureSocketOptions.StartTlsWhenAvailable;
        }
    }
}
