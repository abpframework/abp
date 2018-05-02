using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo.Pages.Components
{
    public class DynamicFormsModel : PageModel
    {
        [BindProperty]
        public PersonModel PersonInput { get; set; }
        
        public List<SelectListItem> Countries { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "MX", Text = "Mexico"},
            new SelectListItem { Value = "CA", Text = "Canada"},
            new SelectListItem { Value = "US", Text = "USA"  },
        };

        public void OnGet()
        {
            if (PersonInput == null)
            {
                PersonInput = new PersonModel
                {
                    Name = "John",
                    Age = 65,
                    Country = "CA",
                    Day = DateTime.Now,
                    City = Cities.NewJersey,
                    Phone = new PhoneModel { Number = "326346231", Name = "MyPhone" }
                };
            }
        }

        public void OnPost()
        {
            
        }

        public class PersonModel
        {
            [Required]
            [DisplayName("Name")]
            public string Name { get; set; } = "MyName";

            [Required]
            [DisplayName("Age")]
            [Range(1, 100)]
            public int Age { get; set; }

            [Required]
            [DisplayName("City")]
            public Cities City { get; set; }

            public PhoneModel Phone { get; set; }

            [DataType(DataType.Date)]
            [DisplayName("Day")]
            [DisplayOrder(10003)]
            public DateTime Day { get; set; }
            
            [DisplayName("Is Active")]
            public bool IsActive { get; set; }

            [DisplayName("Country")]
            [SelectItems(ItemsListPropertyName = nameof(Countries))]
            public string Country { get; set; }
        }

        public class PhoneModel
        {
            [Required]
            [DisplayOrder(10002)]
            [DisplayName("Number")]
            public string Number { get; set; }

            [Required]
            [DisplayOrder(10001)]
            [DisplayName("PhoneName")]
            public string Name { get; set; }
        }

        public enum Cities
        {
            Istanbul,
            NewJersey,
            Moscow
        }
    }
}