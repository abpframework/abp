using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Emailing;

namespace Volo.Abp.SettingManagement
{
    [Authorize(SettingManagementPermissions.Emailing)]
    public class EmailSettingsAppService : SettingManagementAppServiceBase, IEmailSettingsAppService
    {
        protected ISettingManager SettingManager { get; }

        public EmailSettingsAppService(ISettingManager settingManager)
        {
            SettingManager = settingManager;
        }

        public virtual async Task<EmailSettingsDto> GetAsync()
        {
            return new EmailSettingsDto {
                SmtpHost = await SettingManager.GetOrNullGlobalAsync(EmailSettingNames.Smtp.Host),
                SmtpPort = Convert.ToInt32(await SettingManager.GetOrNullGlobalAsync(EmailSettingNames.Smtp.Port)),
                SmtpUserName = await SettingManager.GetOrNullGlobalAsync(EmailSettingNames.Smtp.UserName),
                SmtpPassword = await SettingManager.GetOrNullGlobalAsync(EmailSettingNames.Smtp.Password),
                SmtpDomain = await SettingManager.GetOrNullGlobalAsync(EmailSettingNames.Smtp.Domain),
                SmtpEnableSsl = Convert.ToBoolean(await SettingManager.GetOrNullGlobalAsync(EmailSettingNames.Smtp.EnableSsl)),
                SmtpUseDefaultCredentials = Convert.ToBoolean(await SettingManager.GetOrNullGlobalAsync(EmailSettingNames.Smtp.UseDefaultCredentials)),
                DefaultFromAddress = await SettingManager.GetOrNullGlobalAsync(EmailSettingNames.DefaultFromAddress),
                DefaultFromDisplayName = await SettingManager.GetOrNullGlobalAsync(EmailSettingNames.DefaultFromDisplayName),
            };
        }

        public virtual async Task UpdateAsync(UpdateEmailSettingsDto input)
        {
            await SettingManager.SetGlobalAsync(EmailSettingNames.Smtp.Host, input.SmtpHost);
            await SettingManager.SetGlobalAsync(EmailSettingNames.Smtp.Port, input.SmtpPort.ToString());
            await SettingManager.SetGlobalAsync(EmailSettingNames.Smtp.UserName, input.SmtpUserName);
            await SettingManager.SetGlobalAsync(EmailSettingNames.Smtp.Password, input.SmtpPassword);
            await SettingManager.SetGlobalAsync(EmailSettingNames.Smtp.Domain, input.SmtpDomain);
            await SettingManager.SetGlobalAsync(EmailSettingNames.Smtp.EnableSsl, input.SmtpEnableSsl.ToString());
            await SettingManager.SetGlobalAsync(EmailSettingNames.Smtp.UseDefaultCredentials, input.SmtpUseDefaultCredentials.ToString().ToLowerInvariant());
            await SettingManager.SetGlobalAsync(EmailSettingNames.DefaultFromAddress, input.DefaultFromAddress);
            await SettingManager.SetGlobalAsync(EmailSettingNames.DefaultFromDisplayName, input.DefaultFromDisplayName);
        }
    }
}
