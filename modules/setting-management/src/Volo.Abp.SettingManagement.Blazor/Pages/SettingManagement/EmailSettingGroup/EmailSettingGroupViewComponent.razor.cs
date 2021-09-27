using System;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.SettingManagement.Localization;

namespace Volo.Abp.SettingManagement.Blazor.Pages.SettingManagement.EmailSettingGroup
{
    public partial class EmailSettingGroupViewComponent
    {
        [Inject]
        protected IEmailSettingsAppService EmailSettingsAppService { get; set; }
        
        [Inject]
        private ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; }

        [Inject]
        protected IUiMessageService UiMessageService { get; set; }

        protected EmailSettingsDto EmailSettings;

        protected Validations IdentitySettingValidation;

        public EmailSettingGroupViewComponent()
        {
            ObjectMapperContext = typeof(AbpSettingManagementBlazorModule);
            LocalizationResource = typeof(AbpSettingManagementResource);
        }
        
        protected override async Task OnInitializedAsync()
        {
            try
            {
                EmailSettings = await EmailSettingsAppService.GetAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        protected virtual async Task UpdateSettingsAsync()
        {
            try
            {
                await EmailSettingsAppService.UpdateAsync(ObjectMapper.Map<EmailSettingsDto, UpdateEmailSettingsDto>(EmailSettings));
                
                await CurrentApplicationConfigurationCacheResetService.ResetAsync();

                await UiMessageService.Success(L["SuccessfullySaved"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
    }
}
