using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Account.Web.Areas.Account.Models.Login
{
    public class LoginModel
    {
        [Required]
        [MaxLength(255)]
        public string UserNameOrEmailAddress { get; set; }

        [Required]
        [MaxLength(32)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}