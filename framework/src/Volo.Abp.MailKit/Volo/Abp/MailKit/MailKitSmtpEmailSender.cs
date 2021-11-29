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
            using (var client = await BuildClientAsync())
            {
                var message = MimeMessage.CreateFromMailMessage(mail);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        public async Task<SmtpClient> BuildClientAsync()
        {
            var client = new SmtpClient();

            try
            {
                await ConfigureClient(client);
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
            await client.ConnectAsync(
                await SmtpConfiguration.GetHostAsync(),
                await SmtpConfiguration.GetPortAsync(),
                await GetSecureSocketOption()
            );

            if (await SmtpConfiguration.GetUseDefaultCredentialsAsync())
            {
                return;
            }

            await client.AuthenticateAsync(
                await SmtpConfiguration.GetUserNameAsync(),
                await SmtpConfiguration.GetPasswordAsync()
            );
        }

        protected virtual async Task<SecureSocketOptions> GetSecureSocketOption()
        {
            if (AbpMailKitOptions.SecureSocketOption.HasValue)
            {
                return AbpMailKitOptions.SecureSocketOption.Value;
            }

            return await SmtpConfiguration.GetEnableSslAsync()
                ? SecureSocketOptions.SslOnConnect
                : SecureSocketOptions.StartTlsWhenAvailable;
        }
    }
}
