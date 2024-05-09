# ASP.NET Core MVC / Razor Pages UI: Modais

Embora você possa continuar usando o [método padrão do Bootstrap](https://getbootstrap.com/docs/4.5/components/modal/) para criar, abrir e gerenciar modais em suas aplicações, o ABP Framework fornece uma maneira **flexível** de gerenciar modais, automatizando tarefas comuns para você.

**Exemplo: Um diálogo modal para criar uma nova entidade de função**

![modal-manager-example-modal](../../images/modal-manager-example-modal.png)

O ABP Framework oferece os seguintes benefícios para um modal com um formulário dentro dele:

* **Carrega preguiçosamente** o HTML do modal na página e o **remove** do DOM assim que for fechado. Isso facilita o consumo de um diálogo modal reutilizável. Além disso, toda vez que você abrir o modal, ele será um novo modal, para que você não precise lidar com a redefinição do conteúdo do modal.
* **Dá foco automaticamente** ao primeiro campo de entrada do formulário assim que o modal for aberto. Você também pode especificar isso usando uma `function` ou um seletor `jquery`.
* Determina automaticamente o **formulário** dentro de um modal e envia o formulário via **AJAX** em vez de uma postagem normal da página.
* Verifica automaticamente se o formulário dentro do modal **foi alterado, mas não salvo**. Ele avisa o usuário nesse caso.
* Desabilita automaticamente os botões do modal (salvar e cancelar) até que a operação AJAX seja concluída.
* Facilita o registro de um **objeto JavaScript que é inicializado** assim que o modal é carregado.

Portanto, você escreve menos código ao lidar com os modais, especialmente os modais com um formulário dentro.

## Uso básico

### Criando um modal como uma página Razor

Para demonstrar o uso, estamos criando uma página Razor simples, chamada `ProductInfoModal.cshtml`, na pasta `/Pages/Products`:

![modal-page-on-rider](../../images/modal-page-on-rider.png)

**Conteúdo do ProductInfoModal.cshtml:**

````html
@page
@model MyProject.Web.Pages.Products.ProductInfoModalModel
@{
    Layout = null;
}
<abp-modal>
    <abp-modal-header title="Informações do Produto"></abp-modal-header>
    <abp-modal-body>
        <h3>@Model.ProductName</h3>
        <div>
            <img src="@Model.ProductImageUrl" />
        </div>
        <p>
            @Model.ProductDescription
        </p>
        <p>
            <small><i>Referência: https://acme.com/catalog/</i></small>
        </p>
    </abp-modal-body>
    <abp-modal-footer buttons="Fechar"></abp-modal-footer>
</abp-modal>
````

* Esta página define o `Layout` como `null`, pois mostraremos isso como um modal. Portanto, não é necessário envolvê-lo com um layout.
* Ele usa o [tag helper abp-modal](Tag-Helpers/Modals.md) para simplificar a criação do código HTML do modal. Você pode usar o código padrão do modal do Bootstrap se preferir.

**Conteúdo do ProductInfoModalModel.cshtml.cs:**

```csharp
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace MyProject.Web.Pages.Products
{
    public class ProductInfoModalModel : AbpPageModel
    {
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public string ProductImageUrl { get; set; }

        public void OnGet()
        {
            ProductName = "Bola de Aço Indestrutível Acme";
            ProductDescription = "A Bola de Aço Indestrutível Acme é completamente indestrutível, não há nada que possa destruí-la!";
            ProductImageUrl = "https://acme.com/catalog/acmeindestructo.jpg";
        }
    }
}
```

Você pode obter as informações do produto de um banco de dados ou API. Estamos definindo as propriedades codificadas para simplificar.

### Definindo o Modal Manager

Depois de ter um modal, você pode abri-lo em qualquer página usando algum código JavaScript simples.

Primeiro, crie um objeto `abp.ModalManager` definindo o `viewUrl`, no arquivo JavaScript da página que usará o modal:

````js
var productInfoModal = new abp.ModalManager({
    viewUrl: '/Products/ProductInfoModal'
});
````

> Se você só precisa especificar o `viewUrl`, você pode passá-lo diretamente para o construtor `ModalManager`, como um atalho. Exemplo: `new abp.ModalManager('/Products/ProductInfoModal');`

### Abrindo o Modal

Em seguida, abra o modal sempre que precisar:

````js
productInfoModal.open();
````

Normalmente, você deseja abrir o modal quando algo acontece; Por exemplo, quando o usuário clica em um botão:

````js
$('#OpenProductInfoModal').click(function(){
    productInfoModal.open();
});
````

O modal resultante será assim:

![modal-example-product-info](../../images/modal-example-product-info.png)

#### Abrindo o Modal com Argumentos

Ao chamar o método `open()`, o `ModalManager` carrega o HTML do modal solicitando-o do `viewUrl`. Você pode passar alguns **parâmetros de string de consulta** para esta URL ao abrir o modal.

**Exemplo: Passando o ID do produto ao abrir o modal**

````js
productInfoModal.open({
    productId: 42
});
````

Você pode adicionar um parâmetro `productId` ao método `get`:

````csharp
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace MyProject.Web.Pages.Products
{
    public class ProductInfoModalModel : AbpPageModel
    {
        //...

        public async Task OnGetAsync(int productId) //Adicione o parâmetro productId
        {
            //TODO: Obter o produto do banco de dados com o productId fornecido
            //...
        }
    }
}
````

Dessa forma, você pode usar o `productId` para consultar o produto em uma fonte de dados.

## Modais com Formulários

`abp.ModalManager` lida com várias tarefas comuns (descritas na introdução) quando você deseja usar um formulário dentro do modal.

### Exemplo de Modal com um Formulário

Esta seção mostra um exemplo de formulário para criar um novo produto.

#### Criando a Página Razor

Para este exemplo, crie uma nova página Razor, chamada `ProductCreateModal.cshtml`, na pasta `/Pages/Products`:

![product-create-modal-page-on-rider](../../images/product-create-modal-page-on-rider.png)

**Conteúdo do ProductCreateModal.cshtml:**

````html
@page
@using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
@model MyProject.Web.Pages.Products.ProductCreateModalModel
@{
    Layout = null;
}
<form method="post" action="@Url.Page("/Products/ProductCreateModal")">
    <abp-modal>
        <abp-modal-header title="Criar Novo Produto"></abp-modal-header>
        <abp-modal-body>
            <abp-input asp-for="Product.Name"/>
            <abp-input asp-for="Product.Description"/>
            <abp-input asp-for="Product.ReleaseDate"/>
        </abp-modal-body>
        <abp-modal-footer buttons="@AbpModalButtons.Save | @AbpModalButtons.Cancel"></abp-modal-footer>
    </abp-modal>
</form>
````

* O `abp-modal` foi envolvido pelo `form`. Isso é necessário para colocar os botões `Salvar` e `Cancelar` dentro do formulário. Dessa forma, o botão `Salvar` age como o botão `submit` para o `form`.
* Usamos os [tag helpers abp-input](Tag-Helpers/Form-elements.md) para simplificar a criação dos elementos do formulário. Caso contrário, você precisaria escrever mais HTML.

**Conteúdo do ProductCreateModalModel.cshtml.cs:**

```csharp
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace MyProject.Web.Pages.Products
{
    public class ProductCreateModalModel : AbpPageModel
    {
        [BindProperty]
        public PoductCreationDto Product { get; set; }

        public async Task OnGetAsync()
        {
            //TODO: Lógica de obtenção, se disponível
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //TODO: Salvar o Produto...

            return NoContent();
        }
    }
}
```

* Esta é uma classe `PageModal` simples. O `[BindProperty]` faz com que o formulário seja vinculado ao modelo quando você envia (submete) o formulário; É o sistema padrão do ASP.NET Core.
* `OnPostAsync` retorna `NoContent` (este método é definido pela classe base `AbpPageModel`). Porque não precisamos de um valor de retorno no lado do cliente, após a operação de envio do formulário.

**PoductCreationDto:**

`ProductCreateModalModel` usa uma classe `PoductCreationDto` definida da seguinte forma:

````csharp
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace MyProject.Web.Pages.Products
{
    public class PoductCreationDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }
        
        [TextArea(Rows = 4)]
        [StringLength(2000)]
        public string Description { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
    }
}
````

* O Tag Helper `abp-input` pode entender os atributos de anotação de dados e usá-los para moldar e validar os elementos do formulário. Consulte o documento [tag helpers abp-input](Tag-Helpers/Form-elements.md) para saber mais.

#### Definindo o Modal Manager

Novamente, crie um objeto `abp.ModalManager` definindo o `viewUrl`, no arquivo JavaScript da página que usará o modal:

````js
var productCreateModal = new abp.ModalManager({
    viewUrl: '/Products/ProductCreateModal'
});
````

#### Abrindo o Modal

Em seguida, abra o modal sempre que precisar:

````js
productCreateModal.open();
````

Normalmente, você deseja abrir o modal quando algo acontece; Por exemplo, quando o usuário clica em um botão:

````js
$('#OpenProductCreateModal').click(function(){
    productCreateModal.open();
});
````

Portanto, o código completo será algo assim (supondo que você tenha um `button` com `id` igual a `OpenProductCreateModal` no lado da visualização):

```js
$(function () {

    var productCreateModal = new abp.ModalManager({
        viewUrl: '/Products/ProductCreateModal'
    });

    $('#OpenProductCreateModal').click(function () {
        productCreateModal.open();
    });

});
```

O modal resultante será assim:

![modal-example-product-create](../../images/modal-example-product-create.png)

#### Salvando o Modal

Quando você clica no botão `Salvar`, o formulário é enviado para o servidor. Se o servidor retornar uma **resposta de sucesso**, o evento `onResult` é acionado com alguns argumentos, incluindo a resposta do servidor, e o modal é fechado automaticamente.

Um exemplo de retorno de chamada que registra os argumentos passados para o método `onResult`:

````js
productCreateModal.onResult(function(){
   console.log(arguments);
});
````

Se o servidor retornar uma resposta de falha, ele mostra a mensagem de erro retornada pelo servidor e mantém o modal aberto.

> Consulte a seção *Referência do Modal Manager* abaixo para outros eventos do modal.

#### Cancelando o Modal

Se você clicar no botão Cancelar com algumas alterações feitas, mas não salvas, você receberá uma mensagem de aviso como esta:

![modal-manager-cancel-warning](../../images/modal-manager-cancel-warning.png)

Se você não deseja essa verificação e mensagem, pode adicionar o atributo `data-check-form-on-close="false"` ao seu elemento `form`. Exemplo:

````html
<form method="post"
      action="@Url.Page("/Products/ProductCreateModal")"
      data-check-form-on-close="false">
````

### Validação do Formulário

`ModalManager` aciona automaticamente a validação do formulário quando você clica no botão `Salvar` ou pressiona a tecla `Enter` no formulário:

![modal-manager-validation](../../images/modal-manager-validation.png)

Consulte o documento [Forms & Validation](Forms-Validation.md) para saber mais sobre a validação.

## Modais com Arquivos de Script

Você pode precisar executar alguma lógica para o seu modal. Para fazer isso, crie um arquivo JavaScript como abaixo:

````js
abp.modals.ProductInfo = function () {

    function initModal(modalManager, args) {
        var $modal = modalManager.getModal();
        var $form = modalManager.getForm();

        $modal.find('h3').css('color', 'red');
        
        console.log('initialized the modal...');
    };

    return {
        initModal: initModal
    };
};
````

* Este código simplesmente adiciona uma classe `ProductInfo` ao namespace `abp.modals`. A classe `ProductInfo` expõe uma única função pública: `initModal`.
* O método `initModal` é chamado pelo `ModalManager` assim que o HTML do modal é inserido no DOM e está pronto para a lógica de inicialização.
* O parâmetro `modalManager` é o objeto `ModalManager` relacionado a essa instância do modal. Portanto, você pode usar qualquer função nele em seu código. Consulte a seção *Referência do ModalManager*.

Em seguida, inclua este arquivo na página que você usa o modal:

````html
<abp-script src="/Pages/Products/ProductInfoModal.js"/>
<abp-script src="/Pages/Products/Index.js"/>
````

* Usamos o `abp-script` Tag Helper aqui. Consulte o documento [Bundling & Minification](Bundling-Minification.md) se você quiser entender isso. Você pode usar a tag `script` padrão. Não importa para este caso.

Por fim, defina a opção `modalClass` ao criar a instância `ModalManager`:

````js
var productInfoModal = new abp.ModalManager({
    viewUrl: '/Products/ProductInfoModal',
    modalClass: 'ProductInfo' //Corresponde a abp.modals.ProductInfo
});
````

### Carregamento Preguiçoso do Arquivo de Script

Em vez de adicionar o `ProductInfoModal.js` à página em que você usa o modal, você pode configurá-lo para carregar preguiçosamente o arquivo de script quando o modal for aberto pela primeira vez.

Exemplo:

````js
var productInfoModal = new abp.ModalManager({
    viewUrl: '/Products/ProductInfoModal',
    scriptUrl: '/Pages/Products/ProductInfoModal.js', //URL de carregamento preguiçoso
    modalClass: 'ProductInfo'
});
````

* `scriptUrl` é usado para definir a URL para carregar o arquivo de script do modal.
* Nesse caso, você não precisa mais incluir o `ProductInfoModal.js` na página. Ele será carregado sob demanda.

#### Dica: Bundling & Minification

Embora o carregamento preguiçoso pareça legal no início, ele requer uma chamada adicional ao servidor quando você abre o modal pela primeira vez.

Em vez disso, você pode usar o sistema de [Bundling & Minification](Bundling-Minification.md) para criar um pacote (que é um único arquivo minificado na produção) para todos os arquivos de script usados em uma página:

````html
<abp-script-bundle>
    <abp-script src="/Pages/Products/ProductInfoModal.js"/>
    <abp-script src="/Pages/Products/Index.js"/>
</abp-script-bundle>
````

Isso é eficiente se o arquivo de script não for grande e for aberto com frequência enquanto os usuários usam a página.

Alternativamente, você pode definir a classe `abp.modals.ProductInfo` no arquivo JavaScript principal da página se o modal for usado apenas e sempre na mesma página. Nesse caso, você não precisa de outro arquivo de script externo.