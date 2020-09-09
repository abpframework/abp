# 动态表单

`提示:` 在开始阅读本文档之前,请确保你已经看过并理解了[abp表单元素](Form-elements.md)文档.

## 介绍

`abp-dynamic-form` 为给定c#模型创建bootstrap表单.

基本用法:

````xml
<abp-dynamic-form abp-model="@Model.MyDetailedModel"/>
````

Model:

````csharp
public class DynamicFormsModel : PageModel
{
    [BindProperty]
    public DetailedModel MyDetailedModel { get; set; }

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
                Description = "Lorem ipsum dolor sit amet.",
                IsActive = true,
                Age = 65,
                Day = DateTime.Now,
                MyCarType = CarType.Coupe,
                YourCarType = CarType.Sedan,
                Country = "RU",
                NeighborCountries = new List<string>() { "UK", "CA" }
            };
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

## Demo

参阅 [动态表单demo页面](https://bootstrap-taghelpers.abp.io/Components/Dropdowns)查看示例.

## Attributes

### abp-model

为动态表单设置c#模型,模型的属性以表单形式转化为输入.

### submit-button

可以为 `True` 或 `False`.

如果为 `True`,则会在表单底部生成一个提交按钮.

默认值是 `False`.

### required-symbols

可以为 `True` 或 `False`.  

如果为 `True`,则必需的输入将带有一个符号(*),表示它们是必需的.

默认值是 `True`.

## 表单内容布局

默认情况下,“`abp-dynamic-form` 会清除内部html并将inputs放入自身. 如果要向动态表单添加其他内容或将inputs放置到某些特定区域,可以使用`<abp-form-content />`标签. 这个标签将被表单内容替换, 而 `abp-dynamic-form` 标签的内部html的其余部分将保持不变.

用法:

````xml
<abp-dynamic-form abp-model="@Model.MyExampleModel">
    <div>
        Some content....
    </div>
    <div class="input-area">
        <abp-form-content />
    </div>
    <div>
        Some more content....
    </div>
</abp-dynamic-form>
````

## 输入排序

`abp-dynamic-form` 通过 `DisplayOrder` attribute对属性进行排序,然后按模型类中的属性顺序进行排序.

默认每个属性的 `DisplayOrder` attribute值是10000.

参见以下示例:

````csharp
public class OrderExampleModel
{
    [DisplayOrder(10004)]
    public string Name{ get; set; }
    
    [DisplayOrder(10005)]
    public string Surname{ get; set; }

    //Default 10000
    public string EmailAddress { get; set; }

    [DisplayOrder(10003)]
    public string PhoneNumber { get; set; }

    [DisplayOrder(9999)]
    public string City { get; set; }
}
````

在这个示例中,inputs字段顺序为: `City` > `EmailAddress` > `PhoneNumber` > `Name` > `Surname`. 

## 忽略属性

默认情况下, `abp-dynamic-form` 会为模型类中的每个属性生成输入. 如果要忽略属性请使用 `DynamicFormIgnore` attribute.

参见以下示例:

````csharp
public class MyModel
{
    public string Name { get; set; }

    [DynamicFormIgnore]
    public string Surname { get; set; }
}
````

在这个示例中,不会为 `Surname` 属性生成输入.

## 指示文本框,单选按钮组和组合框

如果你已经阅读了[表单元素文档](Form-elements.md),你会注意到在c#模型上 `abp-radio` 和 `abp-select` 标签非常相. 我们必须使用 `[AbpRadioButton()]` attribute来告诉 `abp-dynamic-form` 你的哪些属性是单选按钮组,哪些属性是组合框.

参见以下示例:

````xml
<abp-dynamic-form abp-model="@Model.MyDetailedModel"/>
````

Model:

````csharp
public class DynamicFormsModel : PageModel
{
    [BindProperty]
    public DetailedModel MyDetailedModel { get; set; }

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
                ComboCarType = CarType.Coupe,
                RadioCarType = CarType.Sedan,
                ComboCountry = "RU",
                RadioCountry = "UK"
            };
    }

    public class DetailedModel
    {
        public CarType ComboCarType { get; set; }

        [AbpRadioButton(Inline = true)]
        public CarType RadioCarType { get; set; }
        
        [SelectItems(nameof(CountryList))]
        public string ComboCountry { get; set; }
        
        [AbpRadioButton()]
        [SelectItems(nameof(CountryList))]
        public string RadioCountry { get; set; }
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

正如你上面的例子中看到:

* 如果在**Enum**属性上使用 `[AbpRadioButton()]`,它将是一个单选按钮组. 否则它是组合框.
* 如果在属性上使用 `[SelectItems()]` 和 `[AbpRadioButton()]`,那么它将是一个单选按钮组.
* 如果只在属性上使用 `[SelectItems()]`,它将是一个组合框.
* 如果一个属性没有使用这些属性,它将是一个文本框.

## 本地化

`abp-dynamic-form` 会处理本地化.

默认情况下, 它将尝试查找 "DisplayName:{PropertyName}" 或 "{PropertyName}" 定位本地化键,并将定位值设置为label.

你可以使用Asp.Net Core的 `[Display()]` attribute自行设置. 可以在此属性中使用本地化密钥. 请参阅以下示例:

````csharp
[Display(Name = "Name")]
public string Name { get; set; }
````