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
        public DetailedModel MyDetailedModel { get; set; }

        public OrderExampleModel MyOrderExampleModel { get; set; }

        public AttributeExamplesModel MyAttributeExamplesModel { get; set; }

        public FormContentExampleModel MyFormContentExampleModel { get; set; }

        public List<SelectListItem> CountryList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "CA", Text = "Canada"},
            new SelectListItem { Value = "US", Text = "USA"},
            new SelectListItem { Value = "UK", Text = "United Kingdom"},
            new SelectListItem { Value = "RU", Text = "Russia"}
        };

        public void OnGet()
        {
                MyDetailedModel = new DetailedModel
                {
                    Name = "",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    IsActive = true,
                    Age = 65,
                    Day = DateTime.Now,
                    MyCarType = CarType.Coupe,
                    YourCarType = CarType.Sedan,
                    Country = "RU",
                    NeighborCountries = new List<string>() { "UK", "CA" }
                };

            MyFormContentExampleModel = new FormContentExampleModel();

            MyOrderExampleModel = new OrderExampleModel();

            MyAttributeExamplesModel = new AttributeExamplesModel
            {
                DisabledInput = "Disabled Input",
                ReadonlyInput = "Readonly Input",
                LargeInput = "Large Input",
                SmallInput = "Small Input"
            };

        }

        public class FormContentExampleModel
        {
            public string SampleInput { get; set; }
        }

        public class AttributeExamplesModel
        {
            [HiddenInput]
            public string HiddenInput { get; set; }

            [DisabledInput]
            public string DisabledInput{ get; set; }

            [ReadOnlyInput]
            public string ReadonlyInput { get; set; }

            [FormControlSize(AbpFormControlSize.Large)]
            public string LargeInput { get; set; }

            [FormControlSize(AbpFormControlSize.Small)]
            public string SmallInput { get; set; }
        }

        public class OrderExampleModel
        {
            [DisplayOrder(10005)]
            public string Surname{ get; set; }

            //Default 10000
            public string EmailAddress { get; set; }

            [DisplayOrder(10003)]
            public string Name { get; set; }

            [DisplayOrder(9999)]
            public string City { get; set; }
        }

        public class DetailedModel
        {
            [Required]
            [Placeholder("Enter your name...")]
            [Display(Name = "Name")]
            public string Name { get; set; }
            
            [TextArea(Rows = 4)]
            [Display(Name = "Description")]
            [InputInfoText("Describe Yourself")]
            public string Description { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Is Active")]
            public bool IsActive { get; set; }

            [Required]
            [Display(Name = "Age")]
            public int Age { get; set; }

            [Required]
            [Display(Name = "My Car Type")]
            public CarType MyCarType { get; set; }

            [Required]
            [Display(Name = "My Car Type Nullable")]
            public CarType? MyCarTypeNullable { get; set; }

            [Required]
            [AbpRadioButton(Inline = true)]
            [Display(Name = "Your Car Type")]
            public CarType YourCarType { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Day")]
            public DateTime Day { get; set; }
            
            [SelectItems(nameof(CountryList))]
            [Display(Name = "Country")]
            public string Country { get; set; }
            
            [SelectItems(nameof(CountryList))]
            [Display(Name = "Neighbor Countries")]
            public List<string> NeighborCountries { get; set; }

            [DynamicFormIgnore]
            public string IgnoredProperty { get; set; }
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