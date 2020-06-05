using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace Volo.Abp.Emailing.Smtp
{
    /// <summary>
    /// Implementation of <see cref="ISmtpEmailSenderConfiguration"/> that reads settings
    /// from <see cref="ISettingProvider"/>.
    /// </summary>
    public class SmtpEmailSenderConfiguration : EmailSenderConfiguration, ISmtpEmailSenderConfiguration, ITransientDependency
    {
        public SmtpEmailSenderConfiguration(ISettingProvider settingProvider)
            : base(settingProvider)
        {

        }

        public Task<string> GetHostAsync()
        {
            return GetNotEmptySettingValueAsync(EmailSettingNames.Smtp.Host);
        }

        public async Task<int> GetPortAsync()
        {
            return (await GetNotEmptySettingValueAsync(EmailSettingNames.Smtp.Port)).To<int>();
        }

        public Task<string> GetUserNameAsync()
        {
            return GetNotEmptySettingValueAsync(EmailSettingNames.Smtp.UserName);
        }

        public Task<string> GetPasswordAsync()
        {
            return GetNotEmptySettingValueAsync(EmailSettingNames.Smtp.Password);
        }

        public Task<string> GetDomainAsync()
        {
            return SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.Domain);
        }

        public async Task<bool> GetEnableSslAsync()
        {
            return (await GetNotEmptySettingValueAsync(EmailSettingNames.Smtp.EnableSsl)).To<bool>();
        }

        public async Task<bool> GetUseDefaultCredentialsAsync()
        {
            return (await GetNotEmptySettingValueAsync(EmailSettingNames.Smtp.UseDefaultCredentials)).To<bool>();
        }
    }
}