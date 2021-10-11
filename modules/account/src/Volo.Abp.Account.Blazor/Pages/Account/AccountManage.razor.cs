using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.Identity;

namespace Volo.Abp.Account.Blazor.Pages.Account
{
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
            
            await UiMessageService.Success(L["PasswordChanged"]);
        }

        protected async Task UpdatePersonalInfoAsync()
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
    }
    
    public class PersonalInfoModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool EmailConfirmed { get; set; }
    }
}