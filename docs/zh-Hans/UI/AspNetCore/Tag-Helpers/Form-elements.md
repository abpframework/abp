
# 表单元素

## 简介

Abp提供表单输入标签助手以便更轻松地构建表单。

## 演示

请查看[表单元素演示页面](https://bootstrap-taghelpers.abp.io/Components/FormElements)。

## abp-input

`abp-input`标签为给定的C#属性创建一个Bootstrap表单输入。它在后台使用了[Asp.Net Core Input Tag Helper](https://docs.microsoft.com/tr-tr/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-3.1#the-input-tag-helper)，所以Asp.Net Core的每个input标签助手的数据注释属性对于`abp-input`也有效。

用法：

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

### 特性(Attributes)

你可以在你的c#属性上设置一些特性，或者直接在html标签上设置。如果您要在[abp-dynamic-form](Dynamic-forms.md)中使用此属性，则只能通过属性特性设置这些属性。


#### 属性特性(Property Attributes)

- `[TextArea()]`: 将输入转换为文本区域。

* `[Placeholder()]`: 为输入设置占位符。您可以直接使用本地化键。
* `[InputInfoText()]`: 为输入设置小型信息文本。您可以直接使用本地化键。
* `[FormControlSize()]`: 设置表单控件包装器元素的大小。可用的值为
  -  `AbpFormControlSize.Default`
  -  `AbpFormControlSize.Small`
  -  `AbpFormControlSize.Medium`
  -  `AbpFormControlSize.Large`
* `[DisabledInput]` :  输入被禁用。
* `[ReadOnlyInput]`: 输入为只读。

#### 标签属性(Tag Attributes)

* `info`: 为输入设置小型信息文本。您可以直接使用本地化键。
* `auto-focus`: 如果为true，则浏览器会自动聚焦在该元素上。
* `size`: 设置表单控件包装器元素的大小。可用的值为
  - `AbpFormControlSize.Default`
  - `AbpFormControlSize.Small`
  - `AbpFormControlSize.Medium`
  - `AbpFormControlSize.Large`
* `disabled`: 输入被禁用。
* `readonly`: 输入为只读。
* `label`: 为输入设置标签。
* `display-required-symbol`: 如果输入为必填项，则向标签添加必需符号（*）。默认为`True`。

[Asp.Net Core Input Tag Helper](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-3.1#the-input-tag-helper)的`asp-format`、`name`和`value`属性也对`abp-input`标签助手有效。

### 标签和本地化

您可以通过不同的方式设置输入的标签：

- 您可以使用`Label`属性直接设置标签。 但是，它不会自动本地化您的本地化键。 因此，请使用`label = "@L [“ {LocalizationKey}”] .Value"`。
- 您可以使用Asp.Net Core的`[Display(name = "{LocalizationKey}")]`属性设置标签。
- 您可以让 **abp** 查找属性的本地化键。 如果未设置`label`或`[DisplayName]`属性，则会尝试查找“DisplayName：{PropertyName}”或“{PropertyName}”本地化键。

## abp-select

`abp-select` 标签为给定的 C# 属性创建了一个 Bootstrap 表单选择器。它在后台使用 [Asp.Net Core 选择标签助手](https://docs.microsoft.com/tr-tr/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-3.1#the-select-tag-helper)，因此 `Asp.Net Core` 的 `select` 标签助手的每个数据注释属性也适用于 `abp-select`。

`abp-select` 标签需要一个 `Microsoft.AspNetCore.Mvc.Rendering.SelectListItem` 的列表来工作。它可以通过标签上的 `asp-items` 属性或 C# 属性上的 `[SelectItems()]` 属性来提供（如果您使用的是 [abp-dynamic-form](Dynamic-forms.md)，则只能使用 C# 属性的方式。）

`abp-select` 支持多重选择。

`abp-select` 自动为 **枚举（Enum）** 属性创建选择列表。不需要额外的数据。如果属性是可空的，则会在自动生成的列表顶部添加一个空键和值。

使用：

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

### 属性

您可以在 C# 属性上或直接在 HTML 标签上设置一些属性。如果您将在 [abp-dynamic-form](Dynamic-forms.md) 中使用此属性，则只能通过属性属性设置这些属性。

#### 属性特性(Property Attributes)

* `[SelectItems()]`: 设置选择数据。参数应为数据列表的名称。（见上面的示例）

- `[InputInfoText()]`：设置输入的小信息文本。您可以直接使用本地化键。
- `[FormControlSize()]`：设置表单控件包装元素的大小。可用值为
  - `AbpFormControlSize.Default`
  - `AbpFormControlSize.Small`
  - `AbpFormControlSize.Medium`
  - `AbpFormControlSize.Large`

#### 标签属性(Tag Attributes)

- `asp-items`: 设置选择数据。这应该是一个 `SelectListItem` 列表。
- `info`: 为输入设置一个小的信息文本。您可以直接使用本地化键。
- `size`: 设置表单控件包装元素的大小。可用值为
  - `AbpFormControlSize.Default`
  - `AbpFormControlSize.Small`
  - `AbpFormControlSize.Medium`
  - `AbpFormControlSize.Large`
- `label`: 为输入设置标签。
- `display-required-symbol`: 如果输入是必需的，则向标签添加必需符号 (*)。默认为 `True`。
- `floating-label`: 设置输入的标签是否应该是浮动的。默认为 `False`。


### 标签和本地化

您可以以不同的方式设置输入的标签：

- 您可以使用 `Label` 属性并直接设置标签。但是它不会自动本地化您的本地化键。因此，请将其用作 `label="@L["{LocalizationKey}"].Value"`。
- 您可以使用 Asp.Net Core 的 `[Display(name="{LocalizationKey}")]` 属性进行设置。
- 您可以让 **abp** 查找属性的本地化键。它将尝试查找“DisplayName:{PropertyName}”或“{PropertyName}”本地化键。

对于**枚举**属性，`abp-select`会设置下拉框的本地化值。它会查找"{EnumTypeName}.{EnumPropertyName}"或"{EnumPropertyName}"的本地化键。例如，在上面的示例中，当它本地化下拉框的值时，它将使用"CarType.StationWagon"或"StationWagon"键。

## abp-radio

`abp-radio`标签为给定的C#属性创建一个Bootstrap表单单选框组。使用方式与`abp-select`标签非常相似。

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

### 特性(attributes)

您可以在C#属性或直接在HTML标签上设置一些属性。如果您将在[abp-dynamic-form](Dynamic-forms.md)中使用此属性，则只能通过属性属性设置这些属性。

####  属性特性(Property Attributes)

- `[SelectItems()]`:设置选择数据。参数应为数据列表的名称。 （见上面的示例）

#### 标签特性(Tag Attributes)

- `asp-items`: 设置选择数据。这应该是一个SelectListItem列表。
- `Inline`: 如果为true，则单选按钮将在单行中，相互紧挨着。如果为false，则它们将在彼此下面。
