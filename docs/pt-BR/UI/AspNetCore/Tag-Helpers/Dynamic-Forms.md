# Formulários Dinâmicos

## Introdução

`abp-dynamic-form` cria um formulário bootstrap para um modelo C# fornecido.

Uso básico:

````xml
<abp-dynamic-form abp-model="@Model.MyDetailedModel"/>
````

Modelo:

````csharp
public class DynamicFormsModel : PageModel
{
    [BindProperty]
    public DetailedModel MyDetailedModel { get; set; }

    public List<SelectListItem> CountryList { get; set; } = new List<SelectListItem>
    {
        new SelectListItem { Value = "CA", Text = "Canadá"},
        new SelectListItem { Value = "US", Text = "EUA"},
        new SelectListItem { Value = "UK", Text = "Reino Unido"},
        new SelectListItem { Value = "RU", Text = "Rússia"}
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
        [Placeholder("Digite seu nome...")]
        [Display(Name = "Nome")]
        public string Name { get; set; }
        
        [TextArea(Rows = 4)]
        [Display(Name = "Descrição")]
        [InputInfoText("Descreva-se")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Ativo")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Idade")]
        public int Age { get; set; }

        [Required]
        [Display(Name = "Meu Tipo de Carro")]
        public CarType MyCarType { get; set; }

        [Required]
        [AbpRadioButton(Inline = true)]
        [Display(Name = "Seu Tipo de Carro")]
        public CarType YourCarType { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Dia")]
        public DateTime Day { get; set; }
        
        [SelectItems(nameof(CountryList))]
        [Display(Name = "País")]
        public string Country { get; set; }
        
        [SelectItems(nameof(CountryList))]
        [Display(Name = "Países Vizinhos")]
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

## Demonstração

Veja a [página de demonstração de formulários dinâmicos](https://bootstrap-taghelpers.abp.io/Components/DynamicForms) para vê-lo em ação.

## Atributos

### abp-model

Define o modelo C# para o formulário dinâmico. As propriedades deste modelo são convertidas em campos de entrada no formulário.

### column-size

Aqui, use 'col-sm' para definir o tamanho. Ao definir essa propriedade, 'col-12' será adicionado ao mesmo tempo.

### submit-button

Pode ser `True` ou `False`.

Se `True`, um botão de envio será gerado na parte inferior do formulário.

O valor padrão é `False`.

### required-symbols

Pode ser `True` ou `False`.

Se `True`, as entradas obrigatórias terão um símbolo (*) que indica que são obrigatórias.

O valor padrão é `True`.

## Posicionamento do Conteúdo do Formulário

Por padrão, `abp-dynamic-form` limpa o HTML interno e coloca os campos de entrada nele. Se você quiser adicionar conteúdo adicional ao formulário dinâmico ou colocar os campos de entrada em uma área específica, você pode usar a tag `<abp-form-content />`. Essa tag será substituída pelo conteúdo do formulário e o restante do HTML interno da tag `abp-dynamic-form` permanecerá inalterado.

Uso:

````xml
<abp-dynamic-form abp-model="@Model.MyExampleModel">
    <div>
        Algum conteúdo....
    </div>
    <div class="input-area">
        <abp-form-content />
    </div>
    <div>
        Mais algum conteúdo....
    </div>
</abp-dynamic-form>
````

## Ordem de Entrada

`abp-dynamic-form` ordena as propriedades pelo atributo `DisplayOrder` e, em seguida, pela ordem das propriedades na classe do modelo.

O número padrão do atributo `DisplayOrder` é 10000 para todas as propriedades.

Veja o exemplo abaixo:

````csharp
public class OrderExampleModel
{
    [DisplayOrder(10004)]
    public string Name{ get; set; }
    
    [DisplayOrder(10005)]
    public string Surname{ get; set; }

    //Padrão 10000
    public string EmailAddress { get; set; }

    [DisplayOrder(10003)]
    public string PhoneNumber { get; set; }

    [DisplayOrder(9999)]
    public string City { get; set; }
}
````

Neste exemplo, os campos de entrada serão exibidos com a seguinte ordem: `City` > `EmailAddress` > `PhoneNumber` > `Name` > `Surname`.

## Ignorando uma propriedade

Por padrão, `abp-dynamic-form` gera uma entrada para cada propriedade na classe do modelo. Se você quiser ignorar uma propriedade, use o atributo `DynamicFormIgnore`.

Veja o exemplo abaixo:

````csharp
        public class MyModel
        {
            public string Name { get; set; }

            [DynamicFormIgnore]
            public string Surname { get; set; }
        }
````

Neste exemplo, nenhuma entrada será gerada para a propriedade `Surname`.

## Indicando Caixa de Texto, Grupo de Rádio e Combobox

Se você leu o documento [Elementos de Formulário](Form-elements.md), você percebeu que as tags `abp-radio` e `abp-select` são muito semelhantes no modelo C#. Portanto, temos que usar o atributo `[AbpRadioButton()]` para informar ao `abp-dynamic-form` qual das suas propriedades será um grupo de rádio e qual será uma combobox. Veja o exemplo abaixo:

````xml
<abp-dynamic-form abp-model="@Model.MyDetailedModel"/>
````

Modelo:

````csharp
public class DynamicFormsModel : PageModel
{
    [BindProperty]
    public DetailedModel MyDetailedModel { get; set; }

    public List<SelectListItem> CountryList { get; set; } = new List<SelectListItem>
    {
        new SelectListItem { Value = "CA", Text = "Canadá"},
        new SelectListItem { Value = "US", Text = "EUA"},
        new SelectListItem { Value = "UK", Text = "Reino Unido"},
        new SelectListItem { Value = "RU", Text = "Rússia"}
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

Como você pode ver no exemplo acima:

* Se `[AbpRadioButton()]` for usado em uma propriedade **Enum**, será um grupo de rádio. Caso contrário, será uma combobox.
* Se `[SelectItems()]` e `[AbpRadioButton()]` forem usados em uma propriedade, será um grupo de rádio.
* Se apenas `[SelectItems()]` for usado em uma propriedade, será uma combobox.
* Se nenhum desses atributos for usado em uma propriedade, será uma caixa de texto.

## Localização

`abp-dynamic-form` também lida com localização.

Por padrão, ele tentará encontrar as chaves de localização "DisplayName:{PropertyName}" ou "{PropertyName}" e definir o valor de localização como rótulo de entrada.

Você pode definir isso você mesmo usando o atributo `[Display()]` do Asp.Net Core. Você pode usar uma chave de localização neste atributo. Veja o exemplo abaixo:

````csharp
[Display(Name = "Nome")]
public string Name { get; set; }
````

## Veja Também

* [Elementos de Formulário](Form-elements.md)