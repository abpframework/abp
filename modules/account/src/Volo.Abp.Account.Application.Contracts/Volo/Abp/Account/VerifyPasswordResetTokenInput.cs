using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Account;

public class VerifyPasswordResetTokenInput
{
    public Guid UserId { get; set; }

    [Required]
    public string ResetToken { get; set; }
}
