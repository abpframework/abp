using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace Volo.Abp.SettingManagement
{
    public class UpdateEmailSettingsDto
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

