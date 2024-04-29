# Form Elements

## Introduction

Abp provides form input tag helpers to make building forms easier.

## Demo

See the [form elements demo page](https://bootstrap-taghelpers.abp.io/Components/FormElements) to see it in action.

## abp-input

`abp-input` tag creates a Bootstrap form input for a given c# property. It uses [Asp.Net Core Input Tag Helper](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-7.0#the-input-tag-helper) in background, so every data annotation attribute of `input` tag helper of Asp.Net Core is also valid for `abp-input`.

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

You can set some of the attributes on your c# property, or directly on HTML tag. If you are going to use this property in a [abp-dynamic-form](dynamic-forms.md), then you can only set these properties via property attributes.

#### Property Attributes

- `[TextArea()]`: Converts the input into a text area.

* `[Placeholder()]`: Sets the description are of the input. You can use a localization key directly.
* `[InputInfoText()]`: Sets text for the input. You can directly use a localization key.
* `[FormControlSize()]`: Sets the size of the form-control wrapper element. Available values are
  -  `AbpFormControlSize.Default`
  -  `AbpFormControlSize.Small`
  -  `AbpFormControlSize.Medium`
  -  `AbpFormControlSize.Large`
* `[DisabledInput]` : Sets the input as disabled.
* `[ReadOnlyInput]`: Sets the input as read-only.

#### Tag Attributes

* `info`: Sets text for the input. You can directly use a localization key.
* `auto-focus`: It lets the browser set focus to the element when its value is true.
* `size`: Sets the size of the form-control wrapper element. Available values are
  - `AbpFormControlSize.Default`
  - `AbpFormControlSize.Small`
  - `AbpFormControlSize.Medium`
  - `AbpFormControlSize.Large`
* `disabled`: Sets the input as disabled.
* `readonly`: Sets the input as read-only.
* `label`: Sets the label of input.
* `required-symbol`: Adds the required symbol `(*)` to the label when the input is required. The default value is `True`.
* `floating-label`: Sets the label as floating label. The default value is `False`.

`asp-format`, `name` and `value` attributes of [Asp.Net Core Input Tag Helper](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-7.0#the-input-tag-helper) are also valid for `abp-input` tag helper.

### Label & Localization

You can set the label of the input in several ways:

- You can use the `Label` attribute to set the label directly. This property does not automatically localize the text. To localize the label, use `label="@L["{LocalizationKey}"].Value"`.
- You can set it using `[Display(name="{LocalizationKey}")]` attribute of ASP.NET Core.
- You can just let **abp** find the localization key for the property. It will try to find "DisplayName:{PropertyName}" or "{PropertyName}" localization keys, if `label` or `[DisplayName]` attributes are not set.

## abp-select

`abp-select` tag creates a Bootstrap form select for a given c# property. It uses [ASP.NET Core Select Tag Helper](https://docs.microsoft.com/tr-tr/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-3.1#the-select-tag-helper) in background, so every data annotation attribute of `select` tag helper of ASP.NET Core is also valid for `abp-select`.

`abp-select` tag needs a list of `Microsoft.AspNetCore.Mvc.Rendering.SelectListItem ` to work. It can be provided by `asp-items` attriube on the tag or `[SelectItems()]` attribute on c# property. (if you are using [abp-dynamic-form](dynamic-forms.md), c# attribute is the only way.)

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

You can set some of the attributes on your c# property, or directly on HTML tag. If you are going to use this property in a [abp-dynamic-form](dynamic-forms.md), then you can only set these properties via property attributes.

#### Property Attributes

* `[SelectItems()]`: Sets the select data. Parameter should be the name of the data list. (see example above)

- `[InputInfoText()]`: Sets text for the input. You can directly use a localization key.
- `[FormControlSize()]`: Sets the size of the form-control wrapper element. Available values are
  - `AbpFormControlSize.Default`
  - `AbpFormControlSize.Small`
  - `AbpFormControlSize.Medium`
  - `AbpFormControlSize.Large`

#### Tag Attributes

- `asp-items`: Sets the select data. This Should be a list of SelectListItem.
- `info`: Sets text for the input. You can directly use a localization key.
- `size`: Sets the size of the form-control wrapper element. Available values are
  - `AbpFormControlSize.Default`
  - `AbpFormControlSize.Small`
  - `AbpFormControlSize.Medium`
  - `AbpFormControlSize.Large`
- `label`: Sets the label of input.
- `required-symbol`: Adds the required symbol `(*)` to the label when the input is required. The default value is `True`.

### Label & Localization

You can set the label of the input in several ways:

- You can use `Label` attribute and directly set the label. But it doesn't auto localize your localization key. So use it as `label="@L["{LocalizationKey}"].Value".`
- You can set it using `[Display(name="{LocalizationKey}")]` attribute of ASP.NET Core.
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

You can set some of the attributes on your c# property, or directly on HTML tag. If you are going to use this property in a [abp-dynamic-form](dynamic-forms.md), then you can only set these properties via property attributes.

#### Property Attributes

- `[SelectItems()]`: Sets the select data. Parameter should be the name of the data list. (see example above)

#### Tag Attributes

- `asp-items`: Sets the select data. This Should be a list of SelectListItem.
- `Inline`: If true, radio buttons will be in single line, next to each other. If false, they will be under each other.

## abp-date-picker & abp-date-range-picker

`abp-date-picker` and `abp-date-range-picker` tags creates a Bootstrap form date picker for a given c# property. `abp-date-picker` is for single date selection, `abp-date-range-picker` is for date range selection. It uses [datepicker](https://www.daterangepicker.com/) jQuery plugin.

Usage:

````xml
<abp-date-picker asp-for="@Model.MyModel.MyDate" />
<abp-date-range-picker asp-for-start="@Model.MyModel.MyDateRangeStart" asp-for-end="@Model.MyModel.MyDateRangeEnd" />
<abp-dynamic-form abp-model="DynamicFormExample"></abp-dynamic-form>
````

Model:

````csharp
 public class FormElementsModel : PageModel
    {
        public SampleModel MyModel { get; set; }

        public DynamicForm DynamicFormExample { get; set; }

        public void OnGet()
        {
            MyModel = new SampleModel();

            DynamicFormExample = new DynamicForm();
        }

        public class SampleModel
        {
            public DateTime MyDate { get; set; }
            
            public DateTime MyDateRangeStart { get; set; }
            
            public DateTime MyDateRangeEnd { get; set; }
        }

        public class DynamicForm
        {
            [DateRangePicker("MyPicker",true)]
            public DateTime StartDate { get; set; }
            
            [DateRangePicker("MyPicker",false)]
            [DatePickerOptions(nameof(DatePickerOptions))]
            public DateTime EndDate { get; set; }
            
            public DateTime DateTime { get; set; }

            public DynamicForm()
            {
                StartDate = DateTime.Now;
                EndDate = DateTime.Now;
                DateTime = DateTime.Now;
            }
        }
    
        public AbpDatePickerOptions DatePickerOptions { get; set; }
    }
````

### Attributes

You can set some of the attributes on your c# property, or directly on HTML tag. If you are going to use this property in a [abp-dynamic-form](dynamic-forms.md), then you can only set these properties via property attributes.

#### Property Attributes

* `[Placeholder()]`: Sets the description are of the input. You can use a localization key directly.
* `[InputInfoText()]`: Sets text for the input. You can directly use a localization key.
* `[FormControlSize()]`: Sets the size of the form-control wrapper element. Available values are
  -  `AbpFormControlSize.Default`
  -  `AbpFormControlSize.Small`
  -  `AbpFormControlSize.Medium`
  -  `AbpFormControlSize.Large`
* `[DisabledInput]` : Sets the input as disabled.
* `[ReadOnlyInput]`: Sets the input as read-only.
- `[DatePickerOptions()]`: Sets the predefined options of the date picker. Parameter should be the name of the options property (see example above). See the available [datepicker options](https://www.daterangepicker.com/#options). You can use a localization key directly.

##### abp-date-picker
`[DatePicker]` : Sets the input as datepicker. Especially for string properties.

##### abp-date-range-picker
`[DateRangePicker()]` : Sets the picker id for the date range picker. You can set the property as a start date by setting IsStart=true or leave it as default/false to set it as an end date.

#### Tag Attributes

* `info`: Sets text for the input. You can directly use a localization key.
* `auto-focus`: It lets the browser set focus to the element when its value is true.
* `size`: Sets the size of the form-control wrapper element. Available values are
  - `AbpFormControlSize.Default`
  - `AbpFormControlSize.Small`
  - `AbpFormControlSize.Medium`
  - `AbpFormControlSize.Large`
* `disabled`: Sets the input as disabled.
* `readonly`: Sets the input as read-only.
* `label`: Sets the label of input.
* `required-symbol`: Adds the required symbol `(*)` to the label when the input is required. The default value is `True`.
* `open-button`: A button to open the datepicker will be added when its `True`. The default value is `True`.
* `clear-button`: A button to clear the datepicker will be added when its `True`. The default value is `True`.
* `single-open-and-clear-button`: Shows the open and clear buttons in a single button when it's `True`. The default value is `True`.
* `is-utc`: Converts the date to UTC when its `True`. The default value is `False`.
* `is-iso`: Converts the date to ISO format when its `True`. The default value is `False`.
* `visible-date-format`: Sets the date format of the input. The default format is the user's culture date format. You need to provide a JavaScript date format convention. Eg:  `YYYY-MM-DDTHH:MM:SSZ`.
* `input-date-format`: Sets the date format of the hidden input for backend compatibility. The default format is `YYYY-MM-DD`. You need to provide a JavaScript date format convention. Eg:  `YYYY-MM-DDTHH:MM:SSZ`.
* `date-separator`: Sets a character to separate start and end dates. The default value is `-`
* Other non-mapped attributes will be automatically added to the input element as is. See the available [datepicker options](https://www.daterangepicker.com/#options). Eg: `data-start-date="2020-01-01"`

##### abp-date-picker

* `asp-date`: Sets the date value. This should be a `DateTime`, `DateTime?`, `DateTimeOffset`, `DateTimeOffset?` or `string` value.

##### abp-date-range-picker

* `asp-for-start`: Sets the start date value. This should be a `DateTime`, `DateTime?`, `DateTimeOffset`, `DateTimeOffset?` or `string` value.
* `asp-for-end`: Sets the end date value. This should be a `DateTime`, `DateTime?`, `DateTimeOffset`, `DateTimeOffset?` or `string` value.

### Label & Localization

You can set the label of the input in several ways:

- You can use the `Label` attribute to set the label directly. This property does not automatically localize the text. To localize the label, use `label="@L["{LocalizationKey}"].Value"`.
- You can set it using `[Display(name="{LocalizationKey}")]` attribute ASP.NET Core.
- You can just let **abp** find the localization key for the property. If the `label` or `[DisplayName]` attributes are not set, it tries to find the localization by convention. For example `DisplayName:{YourPropertyName}` or `{YourPropertyName}`. If your property name is FullName, it will search for `DisplayName:FullName` or `{FullName}`.

### JavaScript Usage

````javascript
var newPicker = abp.libs.bootstrapDateRangePicker.createDateRangePicker(
    {
        label: "New Picker",
    }
);
newPicker.insertAfter($('body'));
````

````javascript
var newPicker = abp.libs.bootstrapDateRangePicker.createSinglePicker(
    {
        label: "New Picker",
    }
);
newPicker.insertAfter($('body'));
````

#### Options

* `label`: Sets the label of the input.
* `placeholder`: Sets the placeholder of the input.
* `value`: Sets the value of the input.
* `name`: Sets the name of the input.
* `id`: Sets the id of the input.
* `required`: Sets the input as required.
* `disabled`: Sets the input as disabled.
* `readonly`: Sets the input as read-only.
* `size`: Sets the size of the form-control wrapper element. Available values are
  - `AbpFormControlSize.Default`
  - `AbpFormControlSize.Small`
  - `AbpFormControlSize.Medium`
  - `AbpFormControlSize.Large`
* `openButton`: A button to open the datepicker will be added when its `True`. The default value is `True`.
* `clearButton`: A button to clear the datepicker will be added when its `True`. The default value is `True`.
* `singleOpenAndClearButton`: Shows the open and clear buttons in a single button when it's `True`. The default value is `True`.
* `isUtc`: Converts the date to UTC when its `True`. The default value is `False`.
* `isIso`: Converts the date to ISO format when its `True`. The default value is `False`.
* `visibleDateFormat`: Sets the date format of the input. The default format is the user's culture date format. You need to provide a JavaScript date format convention. Eg:  `YYYY-MM-DDTHH:MM:SSZ`.
* `inputDateFormat`: Sets the date format of the hidden input for backend compatibility. The default format is `YYYY-MM-DD`. You need to provide a JavaScript date format convention. Eg:  `YYYY-MM-DDTHH:MM:SSZ`.
* `dateSeparator`: Sets a character to separate start and end dates. The default value is `-`.
* `startDateName`: Sets the name of the hidden start date input.
* `endDateName`: Sets the name of the hidden end date input.
* `dateName`: Sets the name of the hidden date input.
* Other [datepicker options](https://www.daterangepicker.com/#options). Eg: `startDate: "2020-01-01"`.