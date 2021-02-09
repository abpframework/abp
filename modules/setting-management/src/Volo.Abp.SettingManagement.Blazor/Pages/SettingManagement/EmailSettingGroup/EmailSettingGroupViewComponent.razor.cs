using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.ObjectMapping;
using Volo.Abp.SettingManagement.Localization;

namespace Volo.Abp.SettingManagement.Blazor.Pages.SettingManagement.EmailSettingGroup
{
    public partial class EmailSettingGroupViewComponent
    {
        [Inject]
        protected IEmailSettingsAppService EmailSettingsAppService { get; set; }

        [Inject]
        protected IUiMessageService UiMessageService { get; set; }

        [Inject]
        protected IStringLocalizer<AbpSettingManagementResource> L { get; set; }

        [Inject]
        protected  IObjectMapper ObjectMapper { get; set; }

        protected EmailSettingsDto EmailSettings;

        protected Validations IdentitySettingValidation;

        protected override async Task OnInitializedAsync()
        {
            EmailSettings = await EmailSettingsAppService.GetAsync();
        }

        protected virtual async Task UpdateSettingsAsync()
        {
            await EmailSettingsAppService.UpdateAsync(ObjectMapper.Map<EmailSettingsDto, UpdateEmailSettingsDto>(EmailSettings));

            await UiMessageService.Success(L["SavedSuccessfully"]);
        }
    }
}
