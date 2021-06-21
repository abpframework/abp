using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;

namespace Volo.Abp.Account
{
    public class ResetPasswordDto
    {
        public Guid UserId { get; set; }

        public Guid? TenantId { get; set; }

        [Required]
        public string ResetToken { get; set; }

        [Required]
        [DisableAuditing]
        public string Password { get; set; }
    }
}
