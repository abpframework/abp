using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.SettingManagement.Localization;

namespace Volo.Abp.SettingManagement.Blazor.MudBlazor.Pages.SettingManagement.EmailSettingGroup;

public partial class EmailSettingGroupViewComponent
{
    [Inject]
    protected IEmailSettingsAppService EmailSettingsAppService { get; set; } = default!;
    
    [Inject]
    protected IPermissionChecker PermissionChecker { get; set; } = default!;

    [Inject]
    private ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; } = default!;

    [Inject]
    protected IUiMessageService UiMessageService { get; set; } = default!;

    [Inject]
    protected ISnackbar Snackbar { get; set; } = default!;

    protected UpdateEmailSettingsViewModel? EmailSettings;

    protected SendTestEmailViewModel SendTestEmailInput = new();

    protected MudForm? _emailFormRef;
    protected MudForm? _testEmailFormRef;
    
    protected bool _sendTestEmailDialogVisible;
    
    protected bool HasSendTestEmailPermission { get; set; }
    
    public EmailSettingGroupViewComponent()
    {
        ObjectMapperContext = typeof(AbpSettingManagementBlazorMudBlazorModule);
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
            if (_emailFormRef == null)
            {
                return;
            }
            
            await _emailFormRef.ValidateAsync();
            if (!_emailFormRef.IsValid)
            {
                return;
            }

            if (EmailSettings == null)
            {
                return;
            }
            
            await EmailSettingsAppService.UpdateAsync(ObjectMapper.Map<UpdateEmailSettingsViewModel, UpdateEmailSettingsDto>(EmailSettings));

            await CurrentApplicationConfigurationCacheResetService.ResetAsync();

            Snackbar.Add(L["SavedSuccessfully"], Severity.Success);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
    
    protected virtual async Task OpenSendTestEmailDialogAsync()
    {
        try
        {
            if (_testEmailFormRef != null)
            {
                await _testEmailFormRef.ResetAsync();
            }
            var emailSettings = await EmailSettingsAppService.GetAsync();
            SendTestEmailInput = new SendTestEmailViewModel 
            {
                SenderEmailAddress = emailSettings.DefaultFromAddress,
                TargetEmailAddress = CurrentUser.Email,
                Subject = L["TestEmailSubject", new Random().Next(1000, 9999)],
                Body = L["TestEmailBody"]
            };
            
            _sendTestEmailDialogVisible = true;
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual Task CloseSendTestEmailDialogAsync()
    {
        _sendTestEmailDialogVisible = false;
        return InvokeAsync(StateHasChanged);
    }

    protected virtual async Task SendTestEmailAsync()
    {
        try
        {
            if (_testEmailFormRef == null)
            {
                return;
            }

            await _testEmailFormRef.ValidateAsync();
            if (!_testEmailFormRef.IsValid)
            {
                return;
            }
            
            await EmailSettingsAppService.SendTestEmailAsync(ObjectMapper.Map<SendTestEmailViewModel, SendTestEmailInput>(SendTestEmailInput));

            Snackbar.Add(L["SentSuccessfully"], Severity.Success);

            await CloseSendTestEmailDialogAsync();
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
        public string? SmtpHost { get; set; }

        [Range(1, 65535)]
        [Display(Name = "SmtpPort")]
        public int SmtpPort { get; set; }

        [MaxLength(1024)]
        [Display(Name = "SmtpUserName")]
        public string? SmtpUserName { get; set; }

        [MaxLength(1024)]
        [DataType(DataType.Password)]
        [DisableAuditing]
        [Display(Name = "SmtpPassword")]
        public string? SmtpPassword { get; set; }

        [MaxLength(1024)]
        [Display(Name = "SmtpDomain")]
        public string? SmtpDomain { get; set; }

        [Display(Name = "SmtpEnableSsl")]
        public bool SmtpEnableSsl { get; set; }

        [Display(Name = "SmtpUseDefaultCredentials")]
        public bool SmtpUseDefaultCredentials { get; set; }

        [MaxLength(1024)]
        [Required]
        [Display(Name = "DefaultFromAddress")]
        public string? DefaultFromAddress { get; set; }

        [MaxLength(1024)]
        [Required]
        [Display(Name = "DefaultFromDisplayName")]
        public string? DefaultFromDisplayName { get; set; }
    }
    
    public class SendTestEmailViewModel
    {
        [Required]
        public string? SenderEmailAddress { get; set; }

        [Required]
        public string? TargetEmailAddress { get; set; }

        [Required]
        public string? Subject { get; set; }
    
        public string? Body { get; set; }
    }
}
