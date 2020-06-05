# Form Elements

## Introduction

Abp provides form input tag helpers to make building forms easier.

## Demo

See the [form elements demo page](https://bootstrap-taghelpers.abp.io/Components/FormElements) to see it in action.

## abp-input

`abp-input` tag creates a Bootstrap form input for a given c# property. It uses [Asp.Net Core Input Tag Helper](https://docs.microsoft.com/tr-tr/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-3.1#the-input-tag-helper) in background, so every data annotation attribute of `input` tag helper of Asp.Net Core is also valid for `abp-input`.

Usage:

````xml
<abp-input asp-for="@Model.MyModel.Name"/>
<abp-input asp-for="@Model.MyModel.Description"/>
<abp-input asp-for="@Model.MyModel.Password"/>
<abp-input asp-for="@Model.MyModel.IsActive"/>
````

Model:

````csharp
    public class FormElementsModel : PageModel
    {
        public SampleModel MyModel { get; set; }

        public void OnGet()
        {
            MyModel = new SampleModel();
        }

        public class SampleModel
        {
            [Required]
            [Placeholder("Enter your name...")]
            [InputInfoText("What is your name?")]
            public string Name { get; set; }
            
            [Required]
            [FormControlSize(AbpFormControlSize.Large)]
            public string SurName { get; set; }

            [TextArea(Rows = 4)]
            public string Description { get; set; }
            
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool IsActive { get; set; }
        }
    }
````

### Attributes

You can set some of the attributes on your c# property, or directly on html tag. If you are going to use this property in a [abp-dynamic-form](Dynamic-forms.md), then you can only set these properties via property attributes.

#### Property Attributes

- `[TextArea()]`: Converts the input into a text area.

* `[Placeholder()]`: Sets placeholder for input. You can use a localization key directly.
* `[InputInfoText()]`: Sets a small info text for input. You can use a localization key directly.
* `[FormControlSize()]`: Sets size of form-control wrapper element. Available values are
  -  `AbpFormControlSize.Default`
  -  `AbpFormControlSize.Small`
  -  `AbpFormControlSize.Medium`
  -  `AbpFormControlSize.Large`
* `[DisabledInput]` :  Input is disabled.
* `[ReadOnlyInput]`: Input is read-only.

#### Tag Attributes

* `info`: Sets a small info text for input. You can use a localization key directly.
* `auto-focus`: If true, browser auto focuses on the element.
* `size`: Sets size of form-control wrapper element. Available values are
  - `AbpFormControlSize.Default`
  - `AbpFormControlSize.Small`
  - `AbpFormControlSize.Medium`
  - `AbpFormControlSize.Large`
* `disabled`: Input is disabled.
* `readonly`: Input is read-only.
* `label`: Sets the label for input.
* `display-required-symbol`: Adds the required symbol (*) to label if input is required. Default `True`.

`asp-format`, `name` and `value` attributes of [Asp.Net Core Input Tag Helper](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-3.1#the-input-tag-helper) are also valid for `abp-input` tag helper.

### Label & Localization

You can set label of your input in different ways:

- You can use `Label` attribute and directly set the label. But it doesn't auto localize your localization key. So use it as `label="@L["{LocalizationKey}"].Value"`.
- You can set it using `[Display(name="{LocalizationKey}")]` attribute of Asp.Net Core.
- You can just let **abp** find the localization key for the property. It will try to find "DisplayName:{PropertyName}" or "{PropertyName}" localization keys, if `label` or `[DisplayName]` attributes are not set.

## abp-select

`abp-select` tag creates a Bootstrap form select for a given c# property. It uses [Asp.Net Core Select Tag Helper](https://docs.microsoft.com/tr-tr/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-3.1#the-select-tag-helper) in background, so every data annotation attribute of `select` tag helper of Asp.Net Core is also valid for `abp-select`.

`abp-select` tag needs a list of `Microsoft.AspNetCore.Mvc.Rendering.SelectListItem ` to work. It can be provided by `asp-items` attriube on the tag or `[SelectItems()]` attribute on c# property. (if you are using [abp-dynamic-form](Dynamic-forms.md), c# attribute is the only way.)

`abp-select` supports multiple selection.

`abp-select` auto-creates a select list for **Enum** properties. No extra data is needed. If property is nullable, an empty key and value is added to top of the auto-generated list.

Usage:

````xml
<abp-select asp-for="@Model.MyModel.City" asp-items="@Model.CityList"/>

<abp-select asp-for="@Model.MyModel.AnotherCity"/>

<abp-select asp-for="@Model.MyModel.MultipleCities" asp-items="@Model.CityList"/>

<abp-select asp-for="@Model.MyModel.MyCarType"/>

<abp-select asp-for="@Model.MyModel.MyNullableCarType"/>
````

Model:

````csharp
 public class FormElementsModel : PageModel
    {
        public SampleModel MyModel { get; set; }
                    
        public List<SelectListItem> CityList { get; set; }

        public void OnGet()
        {
            MyModel = new SampleModel();
            
            CityList = new List<SelectListItem>
            {
                new SelectListItem { Value = "NY", Text = "New York"},
                new SelectListItem { Value = "LDN", Text = "London"},
                new SelectListItem { Value = "IST", Text = "Istanbul"},
                new SelectListItem { Value = "MOS", Text = "Moscow"}
            };
        }

        public class SampleModel
        {
            public string City { get; set; }
            
            [SelectItems(nameof(CityList))]
            public string AnotherCity { get; set; }

            public List<string> MultipleCities { get; set; }
            
            public CarType MyCarType { get; set; }

            public CarType? MyNullableCarType { get; set; }
        }
        
        public enum CarType
        {
            Sedan,
            Hatchback,
            StationWagon,
            Coupe
        }
    }
````

### Attributes

You can set some of the attributes on your c# property, or directly on html tag. If you are going to use this property in a [abp-dynamic-form](Dynamic-forms.md), then you can only set these properties via property attributes.

#### Property Attributes

* `[SelectItems()]`: Sets the select data. Parameter should be the name of the data list. (see example above)

- `[InputInfoText()]`: Sets a small info text for input. You can use a localization key directly.
- `[FormControlSize()]`: Sets size of form-control wrapper element. Available values are
  - `AbpFormControlSize.Default`
  - `AbpFormControlSize.Small`
  - `AbpFormControlSize.Medium`
  - `AbpFormControlSize.Large`

#### Tag Attributes

- `asp-items`: Sets the select data. This Should be a list of SelectListItem.
- `info`: Sets a small info text for input. You can use a localization key directly.
- `size`: Sets size of form-control wrapper element. Available values are
  - `AbpFormControlSize.Default`
  - `AbpFormControlSize.Small`
  - `AbpFormControlSize.Medium`
  - `AbpFormControlSize.Large`
- `label`: Sets the label for input.
- `display-required-symbol`: Adds the required symbol (*) to label if input is required. Default `True`.

### Label & Localization

You can set label of your input in different ways:

- You can use `Label` attribute and directly set the label. But it doesn't auto localize your localization key. So use it as `label="@L["{LocalizationKey}"].Value".`
- You can set it using `[Display(name="{LocalizationKey}")]` attribute of Asp.Net Core.
- You can just let **abp** find the localization key for the property. It will try to find "DisplayName:{PropertyName}" or "{PropertyName}" localization keys.

Localizations of combobox values are set by `abp-select` for **Enum** property. It searches for "{EnumTypeName}.{EnumPropertyName}" or "{EnumPropertyName}" localization keys. For instance, in the example above, it will use "CarType.StationWagon" or "StationWagon" keys for localization when it localizes combobox values.

## abp-radio

`abp-radio` tag creates a Bootstrap form radio group for a given c# property. Usage is very similar to `abp-select` tag.

Usage:

````xml
<abp-radio asp-for="@Model.MyModel.CityRadio" asp-items="@Model.CityList" inline="true"/>

<abp-radio asp-for="@Model.MyModel.CityRadio2"/>
````

Model:

````csharp
 public class FormElementsModel : PageModel
    {
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
            MyModel.CityRadio = "IST";
            MyModel.CityRadio2 = "MOS";
        }

        public class SampleModel
        {
            public string CityRadio { get; set; }
            
            [SelectItems(nameof(CityList))]
            public string CityRadio2 { get; set; }
        }
    }
````

### Attributes

You can set some of the attributes on your c# property, or directly on html tag. If you are going to use this property in a [abp-dynamic-form](Dynamic-forms.md), then you can only set these properties via property attributes.

#### Property Attributes

- `[SelectItems()]`: Sets the select data. Parameter should be the name of the data list. (see example above)

#### Tag Attributes

- `asp-items`: Sets the select data. This Should be a list of SelectListItem.
- `Inline`: If true, radio buttons will be in single line, next to each other. If false, they will be under each other.
