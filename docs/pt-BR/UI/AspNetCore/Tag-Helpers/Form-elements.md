# Elementos de Formulário

## Introdução

O Abp fornece ajudantes de tag de entrada de formulário para facilitar a construção de formulários.

## Demonstração

Veja a página de demonstração dos [elementos de formulário](https://bootstrap-taghelpers.abp.io/Components/FormElements) para vê-los em ação.

## abp-input

A tag `abp-input` cria um campo de entrada de formulário do Bootstrap para uma propriedade C# específica. Ela usa o [Asp.Net Core Input Tag Helper](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-7.0#the-input-tag-helper) em segundo plano, portanto, todos os atributos de anotação de dados da tag `input` do Asp.Net Core também são válidos para `abp-input`.

Uso:

````xml
<abp-input asp-for="@Model.MyModel.Name"/>
<abp-input asp-for="@Model.MyModel.Description"/>
<abp-input asp-for="@Model.MyModel.Password"/>
<abp-input asp-for="@Model.MyModel.IsActive"/>
````

Modelo:

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
            [Placeholder("Digite seu nome...")]
            [InputInfoText("Qual é o seu nome?")]
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

### Atributos

Você pode definir alguns atributos na propriedade C# ou diretamente na tag HTML. Se você for usar essa propriedade em um [abp-dynamic-form](Dynamic-Forms.md), só poderá definir essas propriedades por meio de atributos de propriedade.

#### Atributos de Propriedade

- `[TextArea()]`: Converte a entrada em uma área de texto.

* `[Placeholder()]`: Define a descrição da entrada. Você pode usar uma chave de localização diretamente.
* `[InputInfoText()]`: Define o texto para a entrada. Você pode usar uma chave de localização diretamente.
* `[FormControlSize()]`: Define o tamanho do elemento de wrapper form-control. Os valores disponíveis são:
  -  `AbpFormControlSize.Default`
  -  `AbpFormControlSize.Small`
  -  `AbpFormControlSize.Medium`
  -  `AbpFormControlSize.Large`
* `[DisabledInput]` : Define a entrada como desabilitada.
* `[ReadOnlyInput]`: Define a entrada como somente leitura.

#### Atributos de Tag

* `info`: Define o texto para a entrada. Você pode usar uma chave de localização diretamente.
* `auto-focus`: Permite que o navegador defina o foco no elemento quando o valor for verdadeiro.
* `size`: Define o tamanho do elemento de wrapper form-control. Os valores disponíveis são:
  - `AbpFormControlSize.Default`
  - `AbpFormControlSize.Small`
  - `AbpFormControlSize.Medium`
  - `AbpFormControlSize.Large`
* `disabled`: Define a entrada como desabilitada.
* `readonly`: Define a entrada como somente leitura.
* `label`: Define o rótulo da entrada.
* `required-symbol`: Adiciona o símbolo de obrigatório `(*)` ao rótulo quando a entrada é obrigatória. O valor padrão é `True`.
* `floating-label`: Define o rótulo como rótulo flutuante. O valor padrão é `False`.

Os atributos `asp-format`, `name` e `value` do [Asp.Net Core Input Tag Helper](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-7.0#the-input-tag-helper) também são válidos para o ajudante de tag `abp-input`.

### Rótulo e Localização

Você pode definir o rótulo da entrada de várias maneiras:

- Você pode usar o atributo `Label` para definir o rótulo diretamente. Essa propriedade não localiza automaticamente o texto. Para localizar o rótulo, use `label="@L["{LocalizationKey}"].Value"`.
- Você pode defini-lo usando o atributo `[Display(name="{LocalizationKey}")]` do ASP.NET Core.
- Você pode deixar o **abp** encontrar a chave de localização para a propriedade. Ele tentará encontrar as chaves de localização "DisplayName:{PropertyName}" ou "{PropertyName}", se os atributos `label` ou `[DisplayName]` não estiverem definidos.

## abp-select

A tag `abp-select` cria um seletor de formulário do Bootstrap para uma propriedade C# específica. Ela usa o [ASP.NET Core Select Tag Helper](https://docs.microsoft.com/tr-tr/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-3.1#the-select-tag-helper) em segundo plano, portanto, todos os atributos de anotação de dados da tag `select` do ASP.NET Core também são válidos para `abp-select`.

A tag `abp-select` precisa de uma lista de `Microsoft.AspNetCore.Mvc.Rendering.SelectListItem` para funcionar. Ela pode ser fornecida pelo atributo `asp-items` na tag ou pelo atributo `[SelectItems()]` na propriedade C# (se você estiver usando [abp-dynamic-form](Dynamic-Forms.md), o atributo C# é a única opção).

O `abp-select` suporta seleção múltipla.

O `abp-select` cria automaticamente uma lista de seleção para propriedades **Enum**. Nenhum dado extra é necessário. Se a propriedade for nula, uma chave e valor vazios serão adicionados no topo da lista gerada automaticamente.

Uso:

````xml
<abp-select asp-for="@Model.MyModel.City" asp-items="@Model.CityList"/>

<abp-select asp-for="@Model.MyModel.AnotherCity"/>

<abp-select asp-for="@Model.MyModel.MultipleCities" asp-items="@Model.CityList"/>

<abp-select asp-for="@Model.MyModel.MyCarType"/>

<abp-select asp-for="@Model.MyModel.MyNullableCarType"/>
````

Modelo:

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
                new SelectListItem { Value = "NY", Text = "Nova York"},
                new SelectListItem { Value = "LDN", Text = "Londres"},
                new SelectListItem { Value = "IST", Text = "Istambul"},
                new SelectListItem { Value = "MOS", Text = "Moscou"}
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

### Atributos

Você pode definir alguns atributos na propriedade C# ou diretamente na tag HTML. Se você for usar essa propriedade em um [abp-dynamic-form](Dynamic-Forms.md), só poderá definir essas propriedades por meio de atributos de propriedade.

#### Atributos de Propriedade

* `[SelectItems()]`: Define os dados do seletor. O parâmetro deve ser o nome da lista de dados. (veja o exemplo acima)

- `[InputInfoText()]`: Define o texto para a entrada. Você pode usar uma chave de localização diretamente.
- `[FormControlSize()]`: Define o tamanho do elemento de wrapper form-control. Os valores disponíveis são:
  - `AbpFormControlSize.Default`
  - `AbpFormControlSize.Small`
  - `AbpFormControlSize.Medium`
  - `AbpFormControlSize.Large`

#### Atributos de Tag

- `asp-items`: Define os dados do seletor. Isso deve ser uma lista de SelectListItem.
- `info`: Define o texto para a entrada. Você pode usar uma chave de localização diretamente.
- `size`: Define o tamanho do elemento de wrapper form-control. Os valores disponíveis são:
  - `AbpFormControlSize.Default`
  - `AbpFormControlSize.Small`
  - `AbpFormControlSize.Medium`
  - `AbpFormControlSize.Large`
- `label`: Define o rótulo da entrada.
- `required-symbol`: Adiciona o símbolo de obrigatório `(*)` ao rótulo quando a entrada é obrigatória. O valor padrão é `True`.

### Rótulo e Localização

Você pode definir o rótulo da entrada de várias maneiras:

- Você pode usar o atributo `Label` e definir diretamente o rótulo. Mas isso não localiza automaticamente sua chave de localização. Portanto, use-o como `label="@L["{LocalizationKey}"].Value".`
- Você pode defini-lo usando o atributo `[Display(name="{LocalizationKey}")]` do ASP.NET Core.
- Você pode deixar o **abp** encontrar a chave de localização para a propriedade. Ele tentará encontrar as chaves de localização "DisplayName:{PropertyName}" ou "{PropertyName}".

As localizações dos valores do combobox são definidas pelo `abp-select` para a propriedade **Enum**. Ele procura pelas chaves de localização "{EnumTypeName}.{EnumPropertyName}" ou "{EnumPropertyName}". Por exemplo, no exemplo acima, ele usará as chaves "CarType.StationWagon" ou "StationWagon" para localizar os valores do combobox.

## abp-radio

A tag `abp-radio` cria um grupo de rádio do Bootstrap para uma propriedade C# específica. O uso é muito semelhante à tag `abp-select`.

Uso:

````xml
<abp-radio asp-for="@Model.MyModel.CityRadio" asp-items="@Model.CityList" inline="true"/>

<abp-radio asp-for="@Model.MyModel.CityRadio2"/>
````

Modelo:

````csharp
 public class FormElementsModel : PageModel
    {
        public SampleModel MyModel { get; set; }

        public List<SelectListItem> CityList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "NY", Text = "Nova York"},
            new SelectListItem { Value = "LDN", Text = "Londres"},
            new SelectListItem { Value = "IST", Text = "Istambul"},
            new SelectListItem { Value = "MOS", Text = "Moscou"}
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

### Atributos

Você pode definir alguns atributos na propriedade C# ou diretamente na tag HTML. Se você for usar essa propriedade em um [abp-dynamic-form](Dynamic-Forms.md), só poderá definir essas propriedades por meio de atributos de propriedade.

#### Atributos de Propriedade

- `[SelectItems()]`: Define os dados do seletor. O parâmetro deve ser o nome da lista de dados. (veja o exemplo acima)

#### Atributos de Tag

- `asp-items`: Define os dados do seletor. Isso deve ser uma lista de SelectListItem.
- `Inline`: Se verdadeiro, os botões de rádio serão exibidos em uma única linha, um ao lado do outro. Se falso, eles serão exibidos um abaixo do outro.

## abp-date-picker & abp-date-range-picker

As tags `abp-date-picker` e `abp-date-range-picker` criam um seletor de data do Bootstrap para uma propriedade C# específica. O `abp-date-picker` é para seleção de uma única data, o `abp-date-range-picker` é para seleção de um intervalo de datas. Elas usam o plugin jQuery [datepicker](https://www.daterangepicker.com/).

Uso:

````xml
<abp-date-picker asp-for="@Model.MyModel.MyDate" />
<abp-date-range-picker asp-for-start="@Model.MyModel.MyDateRangeStart" asp-for-end="@Model.MyModel.MyDateRangeEnd" />
<abp-dynamic-form abp-model="DynamicFormExample"></abp-dynamic-form>
````

Modelo:

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

### Atributos

Você pode definir alguns atributos na propriedade C# ou diretamente na tag HTML. Se você for usar essa propriedade em um [abp-dynamic-form](Dynamic-Forms.md), só poderá definir essas propriedades por meio de atributos de propriedade.

#### Atributos de Propriedade

* `[Placeholder()]`: Define a descrição da entrada. Você pode usar uma chave de localização diretamente.
* `[InputInfoText()]`: Define o texto para a entrada. Você pode usar uma chave de localização diretamente.
* `[FormControlSize()]`: Define o tamanho do elemento de wrapper form-control. Os valores disponíveis são:
  -  `AbpFormControlSize.Default`
  -  `AbpFormControlSize.Small`
  -  `AbpFormControlSize.Medium`
  -  `AbpFormControlSize.Large`
* `[DisabledInput]` : Define a entrada como desabilitada.
* `[ReadOnlyInput]`: Define a entrada como somente leitura.
- `[DatePickerOptions()]`: Define as opções predefinidas do seletor de data. O parâmetro deve ser o nome da propriedade de opções (veja o exemplo acima). Veja as opções de [datepicker disponíveis](https://www.daterangepicker.com/#options). Você pode usar uma chave de localização diretamente.

##### abp-date-picker
`[DatePicker]` : Define a entrada como seletor de data. Especialmente para propriedades de string.

##### abp-date-range-picker
`[DateRangePicker()]` : Define o ID do seletor para o seletor de intervalo de datas. Você pode definir a propriedade como uma data de início definindo IsStart=true ou deixá-la como padrão/falso para definir como uma data de término.

#### Atributos de Tag

* `info`: Define o texto para a entrada. Você pode usar uma chave de localização diretamente.
* `auto-focus`: Permite que o navegador defina o foco no elemento quando o valor for verdadeiro.
* `size`: Define o tamanho do elemento de wrapper form-control. Os valores disponíveis são:
  - `AbpFormControlSize.Default`
  - `AbpFormControlSize.Small`
  - `AbpFormControlSize.Medium`
  - `AbpFormControlSize.Large`
* `disabled`: Define a entrada como desabilitada.
* `readonly`: Define a entrada como somente leitura.
* `label`: Define o rótulo da entrada.
* `required-symbol`: Adiciona o símbolo de obrigatório `(*)` ao rótulo quando a entrada é obrigatória. O valor padrão é `True`.
* `open-button`: Um botão para abrir o seletor de data será adicionado quando for `True`. O valor padrão é `True`.
* `clear-button`: Um botão para limpar o seletor de data será adicionado quando for `True`. O valor padrão é `True`.
* `single-open-and-clear-button`: Mostra os botões de abrir e limpar em um único botão quando for `True`. O valor padrão é `True`.
* `is-utc`: Converte a data para UTC quando for `True`. O valor padrão é `False`.
* `is-iso`: Converte a data para o formato ISO quando for `True`. O valor padrão é `False`.
* `visible-date-format`: Define o formato de data da entrada. O formato padrão é o formato de data da cultura do usuário. Você precisa fornecer uma convenção de formato de data JavaScript. Por exemplo:  `YYYY-MM-DDTHH:MM:SSZ`.
* `input-date-format`: Define o formato de data do input oculto para compatibilidade com o backend. O formato padrão é `YYYY-MM-DD`. Você precisa fornecer uma convenção de formato de data JavaScript. Por exemplo:  `YYYY-MM-DDTHH:MM:SSZ`.
* `date-separator`: Define um caractere para separar as datas de início e término. O valor padrão é `-`
* Outros atributos não mapeados serão adicionados automaticamente ao elemento de entrada como estão. Veja as opções de [datepicker disponíveis](https://www.daterangepicker.com/#options). Por exemplo: `data-start-date="2020-01-01"`

##### abp-date-picker

* `asp-date`: Define o valor da data. Isso deve ser um valor `DateTime`, `DateTime?`, `DateTimeOffset`, `DateTimeOffset?` ou `string`.

##### abp-date-range-picker

* `asp-for-start`: Define o valor da data de início. Isso deve ser um valor `DateTime`, `DateTime?`, `DateTimeOffset`, `DateTimeOffset?` ou `string`.
* `asp-for-end`: Define o valor da data de término. Isso deve ser um valor `DateTime`, `DateTime?`, `DateTimeOffset`, `DateTimeOffset?` ou `string`.

### Rótulo e Localização

Você pode definir o rótulo da entrada de várias maneiras:

- Você pode usar o atributo `Label` para definir o rótulo diretamente. Essa propriedade não localiza automaticamente o texto. Para localizar o rótulo, use `label="@L["{LocalizationKey}"].Value"`.
- Você pode defini-lo usando o atributo `[Display(name="{LocalizationKey}")]` do ASP.NET Core.
- Você pode deixar o **abp** encontrar a chave de localização para a propriedade. Ele tentará encontrar as chaves de localização "DisplayName:{PropertyName}" ou "{PropertyName}".

### Uso em JavaScript

````javascript
var newPicker = abp.libs.bootstrapDateRangePicker.createDateRangePicker(
    {
        label: "Novo Seletor",
    }
);
newPicker.insertAfter($('body'));
````

````javascript
var newPicker = abp.libs.bootstrapDateRangePicker.createSinglePicker(
    {
        label: "Novo Seletor",
    }
);
newPicker.insertAfter($('body'));
````

#### Opções

* `label`: Define o rótulo da entrada.
* `placeholder`: Define o espaço reservado da entrada.
* `value`: Define o valor da entrada.
* `name`: Define o nome da entrada.
* `id`: Define o ID da entrada.
* `required`: Define a entrada como obrigatória.
* `disabled`: Define a entrada como desabilitada.
* `readonly`: Define a entrada como somente leitura.
* `size`: Define o tamanho do elemento de wrapper form-control. Os valores disponíveis são:
  - `AbpFormControlSize.Default`
  - `AbpFormControlSize.Small`
  - `AbpFormControlSize.Medium`
  - `AbpFormControlSize.Large`
* `openButton`: Um botão para abrir o seletor de data será adicionado quando for `True`. O valor padrão é `True`.
* `clearButton`: Um botão para limpar o seletor de data será adicionado quando for `True`. O valor padrão é `True`.
* `singleOpenAndClearButton`: Mostra os botões de abrir e limpar em um único botão quando for `True`. O valor padrão é `True`.
* `isUtc`: Converte a data para UTC quando for `True`. O valor padrão é `False`.
* `isIso`: Converte a data para o formato ISO quando for `True`. O valor padrão é `False`.
* `visibleDateFormat`: Define o formato de data da entrada. O formato padrão é o formato de data da cultura do usuário. Você precisa fornecer uma convenção de formato de data JavaScript. Por exemplo:  `YYYY-MM-DDTHH:MM:SSZ`.
* `inputDateFormat`: Define o formato de data do input oculto para compatibilidade com o backend. O formato padrão é `YYYY-MM-DD`. Você precisa fornecer uma convenção de formato de data JavaScript. Por exemplo:  `YYYY-MM-DDTHH:MM:SSZ`.
* `dateSeparator`: Define um caractere para separar as datas de início e término. O valor padrão é `-`.
* `startDateName`: Define o nome do input de data de início oculto.
* `endDateName`: Define o nome do input de data de término oculto.
* `dateName`: Define o nome do input de data oculto.
* Outras [opções de datepicker](https://www.daterangepicker.com/#options). Por exemplo: `startDate: "2020-01-01"`.