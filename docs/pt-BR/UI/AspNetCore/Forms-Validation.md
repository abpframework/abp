# ASP.NET Core MVC / Razor Pages: Formulários e Validação

O ABP Framework fornece infraestrutura e convenções para facilitar a criação de formulários, localizar nomes de exibição para os elementos do formulário e lidar com validação no lado do servidor e do cliente;

* O helper de tag [abp-dynamic-form](Tag-Helpers/Dynamic-Forms.md) automatiza a criação de um **formulário completo** a partir de uma classe de modelo C#: cria os elementos de entrada, lida com a localização e validação no lado do cliente.
* Os helpers de tag [ABP Form](Tag-Helpers/Form-elements.md) (`abp-input`, `abp-select`, `abp-radio`...) renderizam **um único elemento de formulário** com localização e validação no lado do cliente.
* O ABP Framework automaticamente **localiza o nome de exibição** de um elemento do formulário sem a necessidade de adicionar um atributo `[DisplayName]`.
* Os **erros de validação** são automaticamente localizados com base na cultura do usuário.

> Este documento é para a **validação no lado do cliente** e não cobre a validação no lado do servidor. Verifique o [documento de validação](../../Validation.md) para a infraestrutura de validação no lado do servidor.

## O Modo Clássico

Em uma típica interface de usuário ASP.NET Core MVC / Razor Pages baseada no Bootstrap, você [precisa escrever](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation#client-side-validation) um código boilerplate como este para criar um elemento de formulário simples:

````html
<div class="form-group">
    <label asp-for="Movie.ReleaseDate" class="control-label"></label>
    <input asp-for="Movie.ReleaseDate" class="form-control" />
    <span asp-validation-for="Movie.ReleaseDate" class="text-danger"></span>
</div>
````

Você pode continuar usando essa abordagem se precisar ou preferir. No entanto, os helpers de tag do ABP Form podem produzir a mesma saída com um código mínimo.

## ABP Dynamic Forms

O helper de tag [abp-dynamic-form](Tag-Helpers/Dynamic-Forms.md) automatiza completamente a criação do formulário. Considere esta classe de modelo como exemplo:

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace MyProject.Web.Pages
{
    public class MovieViewModel
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [TextArea]
        [StringLength(1000)]
        public string Description { get; set; }

        public Genre Genre { get; set; }

        public float? Price { get; set; }

        public bool PreOrder { get; set; }
    }
}
```

Ele usa os atributos de anotação de dados para definir regras de validação e estilos de UI para as propriedades. `Genre` é um `enum` neste exemplo:

````csharp
namespace MyProject.Web.Pages
{
    public enum Genre
    {
        Classic,
        Action,
        Fiction,
        Fantasy,
        Animation
    }
}
````

Para criar o formulário em uma página razor, crie uma propriedade na sua classe `PageModel`:

```csharp
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyProject.Web.Pages
{
    public class CreateMovieModel : PageModel
    {
        [BindProperty]
        public MovieViewModel Movie { get; set; }

        public void OnGet()
        {
            Movie = new MovieViewModel();
        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                //TODO: Salvar o Filme
            }
        }
    }
}
```

Em seguida, você pode renderizar o formulário no arquivo `.cshtml`:

```html
@page
@model MyProject.Web.Pages.CreateMovieModel

<h2>Criar um novo Filme</h2>

<abp-dynamic-form abp-model="Movie" submit-button="true" />
```

O resultado é mostrado abaixo:

![abp-dynamic-form-result](../../images/abp-dynamic-form-result.png)

Consulte a seção *Localização e Validação* abaixo para localizar os nomes de exibição dos campos e ver como a validação funciona.

> Consulte [seu próprio documento](Tag-Helpers/Dynamic-Forms.md) para todas as opções do helper de tag `abp-dynamic-form`.

## ABP Form Tag Helpers

O `abp-dynamic-form` cobre a maioria dos cenários e permite controlar e personalizar o formulário usando os atributos.

No entanto, se você quiser **renderizar o corpo do formulário você mesmo** (por exemplo, se você quiser ter controle total sobre o **layout do formulário**), você pode usar diretamente os [ABP Form Tag Helpers](Tag-Helpers/Form-elements.md). O mesmo formulário auto-gerado acima pode ser criado usando os ABP Form Tag Helpers, como mostrado abaixo:

```html
@page
@model MyProject.Web.Pages.CreateMovieModel

<h2>Criar um novo Filme</h2>

<form method="post">
    <abp-input asp-for="Movie.Name"/>
    <abp-input asp-for="Movie.ReleaseDate"/>
    <abp-input asp-for="Movie.Description"/>
    <abp-select asp-for="Movie.Genre"/>
    <abp-input asp-for="Movie.Price"/>
    <abp-input asp-for="Movie.PreOrder"/>
    <abp-button button-type="Primary" type="submit">Salvar</abp-button>
</form>
```

> Consulte o documento [ABP Form Tag Helpers](Tag-Helpers/Form-elements.md) para obter detalhes desses helpers de tag e suas opções.

## Validação e Localização

Tanto o Dynamic Form quanto os Form Tag Helpers **validam automaticamente** a entrada com base nos atributos de anotação de dados e exibem mensagens de erro de validação na interface do usuário. As mensagens de erro são **automaticamente localizadas** com base na cultura atual.

**Exemplo: O usuário deixa vazio uma propriedade de string obrigatória**

![abp-form-input-validation-error](../../images/abp-form-input-validation-error.png)

A mensagem de erro abaixo é mostrada se o idioma for francês:

![abp-form-input-validation-error](../../images/abp-form-input-validation-error-french.png)

Os erros de validação já estão [traduzidos](https://github.com/abpframework/abp/tree/dev/framework/src/Volo.Abp.Validation/Volo/Abp/Validation/Localization) em muitos idiomas. Você pode [contribuir](../../Contribution/Index.md) para a tradução do seu próprio idioma ou substituir os textos para sua própria aplicação seguindo a documentação de [localização](../../Localization.md).

## Localização do Nome de Exibição

O ABP Framework usa o nome da propriedade como o nome do campo na interface do usuário. Normalmente, você deseja [localizar](../../Localization.md) esse nome com base na cultura atual.

O ABP Framework pode localizar convencionalmente os campos na interface do usuário quando você adiciona as chaves de localização aos arquivos JSON de localização.

Exemplo: Localização em francês para a propriedade *Name* (adicionar ao `fr.json` na aplicação):

````js
"Name": "Nom"
````

Então, a interface do usuário usará o nome fornecido para o idioma francês:

![abp-form-input-validation-error](../../images/abp-form-input-validation-error-french-name.png)

### Usando o Prefixo `DisplayName:`

Usar diretamente o nome da propriedade como chave de localização pode ser um problema se você precisar usar o nome da propriedade para outro propósito, com um valor de tradução diferente. Nesse caso, use o prefixo `DisplayName:` para a chave de localização:

````js
"DisplayName:Name": "Nom"
````

O ABP prefere usar a chave `DisplayName:Name` em vez da chave `Name` se ela existir.

### Usando uma Chave de Localização Personalizada

Se necessário, você pode usar o atributo `[DisplayName]` para especificar a chave de localização para uma propriedade específica:

````csharp
[DisplayName("MyNameKey")]
public string Name { get; set; }
````

Nesse caso, você pode adicionar uma entrada ao arquivo de localização usando a chave `MyNameKey`.

> Se você usar o `[DisplayName]` mas não adicionar uma entrada correspondente ao arquivo de localização, o ABP Framework mostrará a chave fornecida como o nome do campo, `MyNameKey` para esse caso. Portanto, ele fornece uma maneira de especificar um nome de exibição codificado mesmo se você não precisar usar o sistema de localização.

### Localização de Enum

Os membros do Enum também são automaticamente localizados sempre que possível. Por exemplo, quando adicionamos `<abp-select asp-for="Movie.Genre"/>` ao formulário (como fizemos na seção *ABP Form Tag Helpers*), o ABP pode preencher automaticamente os nomes localizados dos membros do Enum. Para habilitar isso, você deve definir os valores localizados no arquivo JSON de localização. Exemplo de entradas para o Enum `Genre` definido na seção *ABP Form Tag Helpers*:

````json
"Enum:Genre.0": "Filme Clássico",
"Enum:Genre.1": "Filme de Ação",
"Enum:Genre.2": "Ficção",
"Enum:Genre.3": "Fantasia",
"Enum:Genre.4": "Animação/Desenho"
````

Você pode usar uma das seguintes sintaxes para as chaves de localização:

* `Enum:<nome-do-tipo-enum>.<valor-do-enum>`
* `<nome-do-tipo-enum>.<valor-do-enum>`

> Lembre-se de que se você não especificar valores para o seu Enum, os valores serão ordenados, começando em `0`.

> Os helpers de tag do MVC também suportam o uso de nomes de membros do Enum em vez de valores (então, você pode definir `"Enum:Genre.Action"` em vez de `"Enum:Genre.1"`, por exemplo), mas isso não é sugerido. Porque, quando você serializa propriedades Enum para JSON e envia para clientes, o serializador padrão usa os valores Enum em vez dos nomes do Enum. Portanto, o nome do Enum não estará disponível para os clientes, e isso será um problema se você quiser usar os mesmos valores de localização no lado do cliente.

## Veja também

* [Validação no Lado do Servidor](../../Validation.md)