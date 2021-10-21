using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement.Components.EmailSettingGroup
{
    public class EmailSettingGroupViewComponent : AbpViewComponent
    {
        protected IEmailSettingsAppService EmailSettingsAppService { get; }

        public EmailSettingGroupViewComponent(IEmailSettingsAppService emailSettingsAppService)
        {
            ObjectMapperContext = typeof(AbpSettingManagementWebModule);
            EmailSettingsAppService = emailSettingsAppService;
        }

        public async virtual Task<IViewComponentResult> InvokeAsync()
        {
            var model = await EmailSettingsAppService.GetAsync();

            return View("~/Pages/SettingManagement/Components/EmailSettingGroup/Default.cshtml", model);
        }
    }
}
