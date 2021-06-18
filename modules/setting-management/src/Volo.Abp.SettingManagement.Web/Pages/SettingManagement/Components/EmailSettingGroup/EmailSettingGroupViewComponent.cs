using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement.Components.EmailSettingGroup
{
    public class EmailSettingGroupViewComponent : AbpViewComponent
    {
        private readonly IEmailSettingsAppService _emailSettingsAppService;

        public EmailSettingGroupViewComponent(IEmailSettingsAppService emailSettingsAppService)
        {
            ObjectMapperContext = typeof(AbpSettingManagementWebModule);
            _emailSettingsAppService = emailSettingsAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _emailSettingsAppService.GetAsync();

            return View("~/Pages/SettingManagement/Components/EmailSettingGroup/Default.cshtml", model);
        }
    }
}
