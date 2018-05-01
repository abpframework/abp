using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo.Pages.Components
{
    public class FormsModel : PageModel
    {
        public PersonViewModel Ahmet { get; set; } = new PersonViewModel();

        public void OnGet()
        {
            Ahmet = new PersonViewModel
            {
                Name = "ahmet",
                Age = 65,
                Phone = new Phone {Number = "326346231",Name = "MyPhone"}
            };
        }
    }

    public enum Cities
    {
        Istanbul,
        NewJersey,
        Moscow
    }

    public class PersonViewModel
    {
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; } = "MyName";

        [Required]
        [DisplayOrder(61)]
        [DisplayName("Age")]
        [Range(1, 100)]
        public int Age { get; set; }

        [Required]
        [DisplayName("City")]
        public Cities City { get; set; }

        public Phone Phone { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Day")]
        public DateTime Day { get; set; }

        [DisplayOrder(51)]
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [DisplayName("Country")]
        [SelectItems(ItemsListPropertyName = nameof(Countries))]
        public string Country { get; set; }

        public  List<SelectListItem> Countries { get; set; } =  new List<SelectListItem>
        {
            new SelectListItem { Value = "MX", Text = "Mexico" },
            new SelectListItem { Value = "CA", Text = "Canada" },
            new SelectListItem { Value = "US", Text = "USA"  },
        };
    }

    public class Phone
    {
        [Required]
        [DisplayName("Number")]
        public string Number { get; set; }

        [Required]
        [DisplayOrder(71)]
        [DisplayName("PhoneName")]
        public string Name { get; set; }
    }
}