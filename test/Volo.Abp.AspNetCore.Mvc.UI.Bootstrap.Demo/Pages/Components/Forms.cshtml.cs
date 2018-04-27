using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo.Pages.Components
{
    public class FormsModel : PageModel
    {
        [Required]
        [DisplayName("Name")]
        public string Name = "MyName";

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string EmailAddress = "info@volosoft.com";

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password = "MyPass";

        [Phone]
        [DisplayName("Phone Number")]
        public string PhoneNumber = "05069231382";

        [DisplayName("Count")]
        public int Count = 42;

        [DataType(DataType.Date)]
        [DisplayName("Day")]
        public DateTime Day = DateTime.Today;
        
        [DisplayName("Is Active")]
        public bool IsActive = true;

        public void OnGet()
        {

        }
    }
}