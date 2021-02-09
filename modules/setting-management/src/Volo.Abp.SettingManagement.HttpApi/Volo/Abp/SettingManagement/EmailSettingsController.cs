using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.SettingManagement
{
    [RemoteService(Name = SettingManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("settingManagement")]
    [ControllerName("emailSettings")]
    [Route("api/settingManagement/emailSettings")]
    public class EmailSettingsController : AbpController, IEmailSettingsAppService
    {
        private readonly IEmailSettingsAppService _emailSettingsAppService;

        public EmailSettingsController(IEmailSettingsAppService emailSettingsAppService)
        {
            _emailSettingsAppService = emailSettingsAppService;
        }

        [HttpGet]
        public Task<EmailSettingsDto> GetAsync()
        {
            return _emailSettingsAppService.GetAsync();
        }

        [HttpPost]
        public Task UpdateAsync(UpdateEmailSettingsDto input)
        {
            return _emailSettingsAppService.UpdateAsync(input);
        }
    }
}
