using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement.Components.EmailSettingGroup;

public class EmailSettingGroupViewComponent : AbpViewComponent
{
    protected IEmailSettingsAppService EmailSettingsAppService { get; }

    public EmailSettingGroupViewComponent(IEmailSettingsAppService emailSettingsAppService)
    {
        ObjectMapperContext = typeof(AbpSettingManagementWebModule);
        EmailSettingsAppService = emailSettingsAppService;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var emailSettings = await EmailSettingsAppService.GetAsync();
        var model = ObjectMapper.Map<EmailSettingsDto, UpdateEmailSettingsViewModel>(emailSettings);
        return View("~/Pages/SettingManagement/Components/EmailSettingGroup/Default.cshtml", model);
    }

    public class UpdateEmailSettingsViewModel
    {
        [MaxLength(256)]
        public string SmtpHost { get; set; }

        [Range(1, 65535)]
        public int SmtpPort { get; set; }

        [MaxLength(1024)]
        public string SmtpUserName { get; set; }

        [MaxLength(1024)]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public string SmtpPassword { get; set; }

        [MaxLength(1024)]
        public string SmtpDomain { get; set; }

        public bool SmtpEnableSsl { get; set; }

        public bool SmtpUseDefaultCredentials { get; set; }

        [MaxLength(1024)]
        [Required]
        public string DefaultFromAddress { get; set; }

        [MaxLength(1024)]
        [Required]
        public string DefaultFromDisplayName { get; set; }
    }
}
