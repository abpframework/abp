using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Demo.Views.Components.Themes.Shared.Demos.FormElementsDemo;

public class FormElementsDemoModel
{
    public enum CarType
    {
        Sedan,
        Hatchback,
        StationWagon,
        Coupe
    }

    public List<SelectListItem> CityList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "NY", Text = "New York"},
            new SelectListItem { Value = "LDN", Text = "London"},
            new SelectListItem { Value = "IST", Text = "Istanbul"},
            new SelectListItem { Value = "MOS", Text = "Moscow"}
        };

    public class InformMeModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool CheckMeOut { get; set; }
    }

    public class DetailsModel
    {
        [Required]
        public string EmailAddress { get; set; }

        public string City { get; set; }

        public List<string> Cities { get; set; }

        [TextArea]
        public string Description { get; set; }
    }

    public class CheckboxModel
    {
        public bool DefaultCheckbox { get; set; }

        public bool DisabledCheckbox { get; set; }
    }

    public class CityRadioModel
    {
        [Display(Name = "City")]
        public string CityRadio { get; set; }
    }

    public class EnumModel
    {
        public CarType CarType { get; set; }
    }

    public InformMeModel MyInformMeModel { get; set; }
    public DetailsModel MyDetailsModel { get; set; }
    public CheckboxModel MyCheckboxModel { get; set; }
    public CityRadioModel MyCityRadioModel { get; set; }
    public EnumModel MyEnumModel { get; set; }


    public FormElementsDemoModel()
    {
        MyInformMeModel = new InformMeModel();
        MyDetailsModel = new DetailsModel();
        MyCheckboxModel = new CheckboxModel();
        MyCityRadioModel = new CityRadioModel() { CityRadio = "IST" };
        MyEnumModel = new EnumModel();
    }
}
