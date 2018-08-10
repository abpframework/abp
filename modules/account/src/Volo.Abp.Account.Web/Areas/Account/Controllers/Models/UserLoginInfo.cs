using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Account.Web.Areas.Account.Controllers.Models
{
    public class UserLoginInfo
    {
        [Required]
        [StringLength(255)]
        public string UserNameOrEmailAddress { get; set; }

        [Required]
        [StringLength(32)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}