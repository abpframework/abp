using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo.Pages.Components
{
    public class FormsModel : PageModel
    {
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; } = "MyPass";

        [Phone]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Count")]
        public int Count { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Day")]
        public DateTime Day { get; set; }
        
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [DisplayName("Country")]
        public string Country { get; set; }

        [DisplayName("City")]
        public Cities City { get; set; }

        public Person Person { get; set; }

        public List<SelectListItem> Countries { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "MX", Text = "Mexico" },
            new SelectListItem { Value = "CA", Text = "Canada" },
            new SelectListItem { Value = "US", Text = "USA"  },
        };

        public static IEnumerable<SelectListItem> EnumCityList;

        public void OnGet()
        {
            Name = "MyName";
            Day = DateTime.Today;
            //var htmlHelper = new HtmlHelper();
            //EnumCityList = htmlHelper.GetEnumSelectList(typeof(Cities));
        }
    }

    public enum Cities
    {
        Istanbul,
        NewJersey,
        Moscow
    }

    public class Person
    {
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Age")]
        public int Age { get; set; }

        [Required]
        [DisplayName("City")]
        public Cities City { get; set; }

        public Phone Phone { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Day")]
        public DateTime Day { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [DisplayName("Country")]
        public string Country { get; set; }
    }

    public class Phone
    {
        [Required]
        [DisplayName("Number")]
        public string Number { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }
    }
}