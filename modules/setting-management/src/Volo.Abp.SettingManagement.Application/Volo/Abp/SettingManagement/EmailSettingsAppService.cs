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
                SmtpHost = await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.Host),
                SmtpPort = Convert.ToInt32(await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.Port)),
                SmtpUserName = await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.UserName),
                SmtpPassword = await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.Password),
                SmtpDomain = await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.Domain),
                SmtpEnableSsl = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.EnableSsl)),
                SmtpUseDefaultCredentials = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.UseDefaultCredentials)),
                DefaultFromAddress = await SettingProvider.GetOrNullAsync(EmailSettingNames.DefaultFromAddress),
                DefaultFromDisplayName = await SettingProvider.GetOrNullAsync(EmailSettingNames.DefaultFromDisplayName),
            };
        }

        public virtual async Task UpdateAsync(UpdateEmailSettingsDto input)
        {
            if (CurrentTenant.IsAvailable)
            {
                await SettingManager.SetForCurrentTenantAsync(EmailSettingNames.Smtp.Host, input.SmtpHost);
                await SettingManager.SetForCurrentTenantAsync(EmailSettingNames.Smtp.Port, input.SmtpPort.ToString());
                await SettingManager.SetForCurrentTenantAsync(EmailSettingNames.Smtp.UserName, input.SmtpUserName);
                await SettingManager.SetForCurrentTenantAsync(EmailSettingNames.Smtp.Password, input.SmtpPassword);
                await SettingManager.SetForCurrentTenantAsync(EmailSettingNames.Smtp.Domain, input.SmtpDomain);
                await SettingManager.SetForCurrentTenantAsync(EmailSettingNames.Smtp.EnableSsl, input.SmtpEnableSsl.ToString());
                await SettingManager.SetForCurrentTenantAsync(EmailSettingNames.Smtp.UseDefaultCredentials, input.SmtpUseDefaultCredentials.ToString().ToLowerInvariant());
                await SettingManager.SetForCurrentTenantAsync(EmailSettingNames.DefaultFromAddress, input.DefaultFromAddress);
                await SettingManager.SetForCurrentTenantAsync(EmailSettingNames.DefaultFromDisplayName, input.DefaultFromDisplayName);
            }
            else
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
}
