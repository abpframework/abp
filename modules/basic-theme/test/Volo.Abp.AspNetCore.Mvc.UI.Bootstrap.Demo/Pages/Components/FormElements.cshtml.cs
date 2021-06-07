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
    public class FormElementsModel : PageModel
    {
        [BindProperty]
        public SampleModel MyModel { get; set; }

        public List<SelectListItem> CityList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "NY", Text = "New York"},
            new SelectListItem { Value = "LDN", Text = "London"},
            new SelectListItem { Value = "IST", Text = "Istanbul"},
            new SelectListItem { Value = "MOS", Text = "Moscow"}
        };

        public void OnGet()
        {
            MyModel = new SampleModel();
            MyModel.SampleInput0 = "This is a disabled input.";
            MyModel.SampleInput1 = "This is a readonly input.";
            MyModel.CityRadio = "IST";
        }

        public class SampleModel
        {
            public string Name { get; set; }

            public string SampleInput0 { get; set; }

            public string SampleInput1 { get; set; }

            public string SampleInput2 { get; set; }

            public string LargeInput { get; set; }

            public string SmallInput { get; set; }

            [TextArea]
            public string Description { get; set; }

            public string EmailAddress { get; set; }
            
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool CheckMeOut { get; set; }

            public bool DefaultCheckbox { get; set; }

            public bool DisabledCheckbox { get; set; }

            public CarType CarType { get; set; }

            public string City { get; set; }

            [Display(Name="City")]
            public string CityRadio { get; set; }

            public List<string> Cities { get; set; }
        }

        public enum CarType
        {
            Sedan,
            Hatchback,
            StationWagon,
            Coupe
        }
    }
}