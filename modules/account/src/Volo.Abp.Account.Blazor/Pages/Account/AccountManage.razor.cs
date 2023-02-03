using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Account.Blazor.Pages.Account;

public partial class AccountManage
{
    [Inject] protected IProfileAppService ProfileAppService { get; set; }

    [Inject] protected IUiMessageService UiMessageService { get; set; }

    protected string SelectedTab = "Password";

    protected ChangePasswordModel ChangePasswordModel;

    protected PersonalInfoModel PersonalInfoModel;

    protected override async Task OnInitializedAsync()
    {
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
        if (string.IsNullOrWhiteSpace(ChangePasswordModel.CurrentPassword))
        {
            return;
        }

        if (ChangePasswordModel.NewPassword != ChangePasswordModel.NewPasswordConfirm)
        {
            await UiMessageService.Warn(L["NewPasswordConfirmFailed"]);
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
        await ProfileAppService.UpdateAsync(
            ObjectMapper.Map<PersonalInfoModel, UpdateProfileDto>(PersonalInfoModel)
            );

        await UiMessageService.Success(L["PersonalSettingsSaved"]);
    }
}

public class ChangePasswordModel
{
    public string CurrentPassword { get; set; }

    public string NewPassword { get; set; }

    public string NewPasswordConfirm { get; set; }

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
    public string UserName { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool EmailConfirmed { get; set; }

    public string ConcurrencyStamp { get; set; }
}
