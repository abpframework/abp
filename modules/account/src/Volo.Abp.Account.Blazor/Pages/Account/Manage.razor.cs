using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Account.Blazor.Pages.Account
{
    public abstract class AccountManageBase : OwningComponentBase
    {
        [Inject] protected IAccountAppService AccountAppService { get; set; }
        [Inject] protected IProfileAppService ProfileAppService { get; set; }
        
        [Inject] protected IObjectMapper<AbpAccountBlazorModule> ObjectMapper { get; set; }
        
        [Inject] protected IUiMessageService UiMessageService { get; set; }

        [Inject] protected IStringLocalizer<AbpUiResource> UiLocalizer { get; set; }
        
        [Inject] protected IStringLocalizer<AccountResource> L { get; set; }

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
                await UiMessageService.WarnAsync(L["NewPasswordConfirmFailed"]);
                return;
            }
            
            await ProfileAppService.ChangePasswordAsync(new ChangePasswordInput
            {
                CurrentPassword = ChangePasswordModel.CurrentPassword,
                NewPassword = ChangePasswordModel.NewPassword
            });
            
            await UiMessageService.SuccessAsync(L["PasswordChanged"]);
        }

        protected async Task UpdatePersonalInfoAsync()
        {            
            await ProfileAppService.UpdateAsync(
                ObjectMapper.Map<PersonalInfoModel, UpdateProfileDto>(PersonalInfoModel)
                );
            
            await UiMessageService.SuccessAsync(L["PersonalSettingsSaved"]);
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