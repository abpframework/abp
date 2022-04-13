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
        [Display(Name = "SmtpHost")]
        public string SmtpHost { get; set; }

        [Range(1, 65535)]
        [Display(Name = "SmtpPort")]
        public int SmtpPort { get; set; }

        [MaxLength(1024)]
        [Display(Name = "SmtpUserName")]
        public string SmtpUserName { get; set; }

        [MaxLength(1024)]
        [DataType(DataType.Password)]
        [DisableAuditing]
        [Display(Name = "SmtpPassword")]
        public string SmtpPassword { get; set; }

        [MaxLength(1024)]
        [Display(Name = "SmtpDomain")]
        public string SmtpDomain { get; set; }

        [Display(Name = "SmtpEnableSsl")]
        public bool SmtpEnableSsl { get; set; }

        [Display(Name = "SmtpUseDefaultCredentials")]
        public bool SmtpUseDefaultCredentials { get; set; }

        [MaxLength(1024)]
        [Required]
        [Display(Name = "DefaultFromAddress")]
        public string DefaultFromAddress { get; set; }

        [MaxLength(1024)]
        [Required]
        [Display(Name = "DefaultFromDisplayName")]
        public string DefaultFromDisplayName { get; set; }
    }
}
