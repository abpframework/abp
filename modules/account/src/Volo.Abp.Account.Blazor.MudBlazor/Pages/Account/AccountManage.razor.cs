using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.Identity;
using Volo.Abp.MudBlazorUI;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Account.Blazor.MudBlazor.Pages.Account;

public partial class AccountManage
{
    [Parameter]
    public string? Culture { get; set; }

    [Inject] protected IProfileAppService ProfileAppService { get; set; } = default!;

    [Inject] protected IUiMessageService UiMessageService { get; set; } = default!;

    [Inject] protected ISnackbar Snackbar { get; set; } = default!;

    [Inject] protected IStringLocalizer<AccountResource> Localizer { get; set; } = default!;

    protected string SelectedTab = "Password";

    protected ChangePasswordModel? ChangePasswordModel;

    protected PersonalInfoModel? PersonalInfoModel;

    private int _activeTabIndex = 0;
    private MudForm? _changePasswordForm;
    private MudForm? _personalInfoForm;
    private AbpMudBlazorMessageLocalizerHelper<AccountResource>? _localizerHelper;

    protected override async Task OnInitializedAsync()
    {
        _localizerHelper = new AbpMudBlazorMessageLocalizerHelper<AccountResource>(Localizer);
        await GetUserInformations();
    }

    protected async Task GetUserInformations()
    {
        var user = await ProfileAppService.GetAsync();

        ChangePasswordModel = new ChangePasswordModel
        {
            HideOldPasswordInput = !user.HasPassword
        };

        PersonalInfoModel = ObjectMapper.Map<ProfileDto, PersonalInfoModel>(user);
    }

    protected async Task ChangePasswordAsync()
    {
        if (_changePasswordForm == null)
        {
            return;
        }

        await _changePasswordForm.ValidateAsync();
        if (!_changePasswordForm.IsValid)
        {
            return;
        }

        if (ChangePasswordModel == null || string.IsNullOrWhiteSpace(ChangePasswordModel.CurrentPassword))
        {
            return;
        }

        if (ChangePasswordModel.NewPassword != ChangePasswordModel.NewPasswordConfirm)
        {
            await UiMessageService.Warn(L["NewPasswordConfirmFailed"]);
            return;
        }

        if (ChangePasswordModel.CurrentPassword == ChangePasswordModel.NewPassword) 
        {
            await UiMessageService.Warn(L["NewPasswordSameAsOld"]);
            return;
        }

        await ProfileAppService.ChangePasswordAsync(new ChangePasswordInput
        {
            CurrentPassword = ChangePasswordModel.CurrentPassword,
            NewPassword = ChangePasswordModel.NewPassword
        });

        ChangePasswordModel.Clear();

        await UiMessageService.Success(L["PasswordChanged"]);
    }

    protected virtual async Task UpdatePersonalInfoAsync()
    {
        if (_personalInfoForm == null)
        {
            return;
        }

        await _personalInfoForm.ValidateAsync();
        
        if (!_personalInfoForm.IsValid)
        {
            return;
        }
        
        if (PersonalInfoModel == null)
        {
            return;
        }

        await ProfileAppService.UpdateAsync(
            ObjectMapper.Map<PersonalInfoModel, UpdateProfileDto>(PersonalInfoModel)
            );

        await UiMessageService.Success(L["PersonalSettingsSaved"]);
    }

}

public class ChangePasswordModel
{
    public string CurrentPassword { get; set; } = string.Empty;

    public string NewPassword { get; set; } = string.Empty;

    public string NewPasswordConfirm { get; set; } = string.Empty;

    public bool HideOldPasswordInput { get; set; }

    public void Clear()
    {
        CurrentPassword = string.Empty;
        NewPassword = string.Empty;
        NewPasswordConfirm = string.Empty;
    }
}

public class PersonalInfoModel : ExtensibleObject
{
    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public bool PhoneNumberConfirmed { get; set; }

    public bool EmailConfirmed { get; set; }

    public string ConcurrencyStamp { get; set; } = string.Empty;
}
