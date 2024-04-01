using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.SettingManagement.Localization;

namespace Volo.Abp.SettingManagement.Blazor.Pages.SettingManagement.EmailSettingGroup;

public partial class EmailSettingGroupViewComponent
{
    [Inject]
    protected IEmailSettingsAppService EmailSettingsAppService { get; set; }
    
    [Inject]
    protected IPermissionChecker PermissionChecker { get; set; }

    [Inject]
    private ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; }

    [Inject]
    protected IUiMessageService UiMessageService { get; set; }

    protected UpdateEmailSettingsViewModel EmailSettings;

    protected SendTestEmailViewModel SendTestEmailInput;

    protected Validations EmailSettingValidation;
    
    protected Validations EmailSettingTestValidation;
    
    protected Modal SendTestEmailModal;
    
    protected bool HasSendTestEmailPermission { get; set; }
    
    

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
            HasSendTestEmailPermission = await PermissionChecker.IsGrantedAsync(SettingManagementPermissions.EmailingTest);
            SendTestEmailInput = new SendTestEmailViewModel();
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

            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
    
    protected virtual async Task OpenSendTestEmailModalAsync()
    {
        try
        {
            await EmailSettingTestValidation.ClearAll();
            var emailSettings = await EmailSettingsAppService.GetAsync();
            SendTestEmailInput = new SendTestEmailViewModel 
            {
                SenderEmailAddress = emailSettings.DefaultFromAddress,
                TargetEmailAddress = CurrentUser.Email,
                Subject = L["TestEmailSubject", new Random().Next(1000, 9999)],
                Body = L["TestEmailBody"]
            };
            
            await SendTestEmailModal.Show();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual Task CloseSendTestEmailModalAsync()
    {
        return InvokeAsync(SendTestEmailModal.Hide);
    }

    protected virtual async Task SendTestEmailAsync()
    {
        try
        {
            if (!await EmailSettingTestValidation.ValidateAll())
            {
                return;
            }
            
            await EmailSettingsAppService.SendTestEmailAsync(ObjectMapper.Map<SendTestEmailViewModel, SendTestEmailInput>(SendTestEmailInput));

            await Notify.Success(L["SuccessfullySent"]);
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
    
    public class SendTestEmailViewModel
    {
        [Required]
        public string SenderEmailAddress { get; set; }

        [Required]
        public string TargetEmailAddress { get; set; }

        [Required]
        public string Subject { get; set; }
    
        public string Body { get; set; }
    }
}
