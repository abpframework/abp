using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Account.Web.Areas.Account.Models.Register
{
    public class RegisterModel
    {
        [Required]
        [MaxLength(32)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string EmailAddress { get; set; }

        [Required]
        [MaxLength(32)]
        public string Password { get; set; }
    }
}