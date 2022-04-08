using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.Auditing;
using Volo.Abp.SettingManagement.Localization;

namespace Volo.Abp.SettingManagement.Blazor.Pages.SettingManagement.EmailSettingGroup;

public partial class EmailSettingGroupViewComponent
{
    [Inject]
    protected IEmailSettingsAppService EmailSettingsAppService { get; set; }

    [Inject]
    private ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; }

    [Inject]
    protected IUiMessageService UiMessageService { get; set; }

    protected UpdateEmailSettingsViewModel EmailSettings;

    protected Validations EmailSettingValidation;

    public EmailSettingGroupViewComponent()
    {
        ObjectMapperContext = typeof(AbpSettingManagementBlazorModule);
        LocalizationResource = typeof(AbpSettingManagementResource);
    }

    protected async override Task OnInitializedAsync()
    {
        try
        {
            EmailSettings = ObjectMapper.Map<EmailSettingsDto, UpdateEmailSettingsViewModel>(await EmailSettingsAppService.GetAsync());
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
            if (!await EmailSettingValidation.ValidateAll())
            {
                return;
            }
            
            await EmailSettingsAppService.UpdateAsync(ObjectMapper.Map<UpdateEmailSettingsViewModel, UpdateEmailSettingsDto>(EmailSettings));

            await CurrentApplicationConfigurationCacheResetService.ResetAsync();

            await UiMessageService.Success(L["SuccessfullySaved"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
    
    public class UpdateEmailSettingsViewModel
    {
        [MaxLength(256)]
        [Display(Name = "SmtpHost")]
        public string SmtpHost { get; set; }

        [Range(1, 65535)]
        [Display(Name = "SmtpPort")]
        public int SmtpPort { get; set; }

        [MaxLength(1024)]
        [Display(Name = "SmtpUserName")]
        public string SmtpUserName { get; set; }

        [MaxLength(1024)]
        [DataType(DataType.Password)]
        [DisableAuditing]
        [Display(Name = "SmtpPassword")]
        public string SmtpPassword { get; set; }

        [MaxLength(1024)]
        [Display(Name = "SmtpDomain")]
        public string SmtpDomain { get; set; }

        [Display(Name = "SmtpEnableSsl")]
        public bool SmtpEnableSsl { get; set; }

        [Display(Name = "SmtpUseDefaultCredentials")]
        public bool SmtpUseDefaultCredentials { get; set; }

        [MaxLength(1024)]
        [Required]
        [Display(Name = "DefaultFromAddress")]
        public string DefaultFromAddress { get; set; }

        [MaxLength(1024)]
        [Required]
        [Display(Name = "DefaultFromDisplayName")]
        public string DefaultFromDisplayName { get; set; }
    }
}
