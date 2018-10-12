using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Volo.AbpWebSite.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public PersonModel PersonInput { get; set; }

        public void OnGet()
        {
            PersonInput = new PersonModel
            {
                Id = Guid.NewGuid()
            };
        }

        public void OnPostDynamicForm()
        {
            
        }

        public class PersonModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [EmailAddress]
            [StringLength(255)]
            public string Email { get; set; }

            [Required]
            [StringLength(32)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [StringLength(255)]
            public string Address { get; set; }

            public Gender Gender { get; set; }
        }

        public enum Gender
        {
            Unspecified,
            Male,
            Female
        }
    }
}