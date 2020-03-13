## Tutorial do ASP.NET Core MVC - Parte II

### Sobre este tutorial

Esta é a segunda parte da série de tutoriais do ASP.NET Core MVC. Veja todas as peças:

- [Parte I: Crie o projeto e uma página da lista de livros](https://docs.abp.io/en/abp/latest/Tutorials/AspNetCore-Mvc/Part-I)
- **Parte II: Criar, atualizar e excluir livros (este tutorial)**
- [Parte III: Testes de Integração](https://docs.abp.io/en/abp/latest/Tutorials/AspNetCore-Mvc/Part-III)

Você pode acessar o **código fonte** do aplicativo [no repositório GitHub](https://github.com/volosoft/abp/tree/master/samples/BookStore) .

> Você também pode assistir a [este curso em vídeo](https://amazingsolutions.teachable.com/p/lets-build-the-bookstore-application) preparado por um membro da comunidade ABP, com base neste tutorial.

### Criando um novo livro

Nesta seção, você aprenderá como criar um novo formulário de diálogo modal para criar um novo livro. A caixa de diálogo do resultado será assim:

![livraria-criar-diálogo](https://raw.githubusercontent.com/abpframework/abp/master/docs/en/Tutorials/AspNetCore-Mvc/images/bookstore-create-dialog-2.png)

#### Crie o formulário modal

Crie uma nova página de navalha, nomeada `CreateModal.cshtml`sob a `Pages/Books`pasta do `Acme.BookStore.Web`projeto:

![livraria-adicionar-criar-diálogo](https://raw.githubusercontent.com/abpframework/abp/master/docs/en/Tutorials/AspNetCore-Mvc/images/bookstore-add-create-dialog-v2.png)

##### CreateModal.cshtml.cs

Abra o `CreateModal.cshtml.cs`arquivo ( `CreateModalModel`classe) e substitua pelo seguinte código:

```csharp
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Acme.BookStore.Web.Pages.Books
{
    public class CreateModalModel : BookStorePageModel
    {
        [BindProperty]
        public CreateUpdateBookDto Book { get; set; }

        private readonly IBookAppService _bookAppService;

        public CreateModalModel(IBookAppService bookAppService)
        {
            _bookAppService = bookAppService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _bookAppService.CreateAsync(Book);
            return NoContent();
        }
    }
}
```



- Esta classe é derivada do em `BookStorePageModel`vez do padrão `PageModel`. `BookStorePageModel`herda o `PageModel`e adiciona algumas propriedades / métodos comuns que podem ser usados pelas classes de modelo de página.
- `[BindProperty]`O atributo na `Book`propriedade vincula os dados de solicitação posterior a essa propriedade.
- Essa classe simplesmente injeta o `IBookAppService`em seu construtor e chama o `CreateAsync`método no `OnPostAsync`manipulador.

##### CreateModal.cshtml

Abra o `CreateModal.cshtml`arquivo e cole o código abaixo:

```html
@page
@inherits Acme.BookStore.Web.Pages.BookStorePage
@using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
@model Acme.BookStore.Web.Pages.Books.CreateModalModel
@{
    Layout = null;
}
<abp-dynamic-form abp-model="Book" data-ajaxForm="true" asp-page="/Books/CreateModal">
    <abp-modal>
        <abp-modal-header title="@L["NewBook"].Value"></abp-modal-header>
        <abp-modal-body>
            <abp-form-content />
        </abp-modal-body>
        <abp-modal-footer buttons="@(AbpModalButtons.Cancel|AbpModalButtons.Save)"></abp-modal-footer>
    </abp-modal>
</abp-dynamic-form>
```



- Este modal usa o 

  ```
  abp-dynamic-form
  ```

  auxiliar de marca para criar automaticamente o formulário a partir da 

  ```
  CreateBookViewModel
  ```

  classe.

  - `abp-model`O atributo indica o objeto do modelo, a `Book`propriedade neste caso.
  - `data-ajaxForm` O atributo faz com que o formulário seja enviado via AJAX, em vez de uma postagem de página clássica.
  - `abp-form-content`O auxiliar de marca é um espaço reservado para renderizar os controles do formulário (isso é opcional e necessário apenas se você tiver adicionado outro conteúdo à `abp-dynamic-form`marca, como nesta página).

#### Adicione o botão "Novo livro"

Abra `Pages/Books/Index.cshtml`e altere a `abp-card-header`tag, como mostrado abaixo:

```html
<abp-card-header>
    <abp-row>
        <abp-column size-md="_6">
            <h2>@L["Books"]</h2>
        </abp-column>
        <abp-column size-md="_6" class="text-right">
            <abp-button id="NewBookButton"
                        text="@L["NewBook"].Value"
                        icon="plus"
                        button-type="Primary" />
        </abp-column>
    </abp-row>
</abp-card-header>
```



Acabei de adicionar um botão **Novo livro** no canto **superior direito** da tabela:

![livraria-novo-livro-botão](https://raw.githubusercontent.com/abpframework/abp/master/docs/en/Tutorials/AspNetCore-Mvc/images/bookstore-new-book-button.png)

Abra o `pages/books/index.js`e adicione o seguinte código logo após a configuração da tabela de dados:

```js
var createModal = new abp.ModalManager(abp.appPath + 'Books/CreateModal');

createModal.onResult(function () {
    dataTable.ajax.reload();
});

$('#NewBookButton').click(function (e) {
    e.preventDefault();
    createModal.open();
});
```



- `abp.ModalManager`é uma classe auxiliar para abrir e gerenciar modais no lado do cliente. Ele usa internamente o modal padrão do Twitter Bootstrap, mas abstrai muitos detalhes, fornecendo uma API simples.

Agora, você pode **executar o aplicativo** e adicionar novos livros usando o novo formulário modal.

### Atualizando um livro existente

Crie uma nova página de navalha, nomeada `EditModal.cshtml`sob a `Pages/Books`pasta do `Acme.BookStore.Web`projeto:

![livraria-adicionar-editar-diálogo](https://raw.githubusercontent.com/abpframework/abp/master/docs/en/Tutorials/AspNetCore-Mvc/images/bookstore-add-edit-dialog.png)

#### EditModal.cshtml.cs

Abra o `EditModal.cshtml.cs`arquivo ( `EditModalModel`classe) e substitua pelo seguinte código:

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Acme.BookStore.Web.Pages.Books
{
    public class EditModalModel : BookStorePageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public CreateUpdateBookDto Book { get; set; }

        private readonly IBookAppService _bookAppService;

        public EditModalModel(IBookAppService bookAppService)
        {
            _bookAppService = bookAppService;
        }

        public async Task OnGetAsync()
        {
            var bookDto = await _bookAppService.GetAsync(Id);
            Book = ObjectMapper.Map<BookDto, CreateUpdateBookDto>(bookDto);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _bookAppService.UpdateAsync(Id, Book);
            return NoContent();
        }
    }
}
```



- `[HiddenInput]`e `[BindProperty]`são atributos padrão do ASP.NET Core MVC. Utilizado `SupportsGet`para obter o valor do ID a partir do parâmetro da string de consulta da solicitação.
- Mapeado `BookDto`(recebido de `BookAppService.GetAsync`) para `CreateUpdateBookDto`no `GetAsync`método
- O `OnPostAsync`simplesmente usa `BookAppService.UpdateAsync`para atualizar a entidade.

#### Mapeamento de BookDto para CreateUpdateBookDto

A fim de executar `BookDto`a `CreateUpdateBookDto`opor mapeamento, abrir o `BookStoreWebAutoMapperProfile.cs`no `Acme.BookStore.Web`projecto e alterá-lo como se mostra abaixo:

```csharp
using AutoMapper;

namespace Acme.BookStore.Web
{
    public class BookStoreWebAutoMapperProfile : Profile
    {
        public BookStoreWebAutoMapperProfile()
        {
            CreateMap<BookDto, CreateUpdateBookDto>();
        }
    }
}
```



- Apenas adicionado `CreateMap<BookDto, CreateUpdateBookDto>();`como a definição de mapeamento.

#### EditModal.cshtml

Substitua o `EditModal.cshtml`conteúdo pelo seguinte:

```html
@page
@inherits Acme.BookStore.Web.Pages.BookStorePage
@using Acme.BookStore.Web.Pages.Books
@using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
@model EditModalModel
@{
    Layout = null;
}
<abp-dynamic-form abp-model="Book" data-ajaxForm="true" asp-page="/Books/EditModal">
    <abp-modal>
        <abp-modal-header title="@L["Update"].Value"></abp-modal-header>
        <abp-modal-body>
            <abp-input asp-for="Id" />
            <abp-form-content />
        </abp-modal-body>
        <abp-modal-footer buttons="@(AbpModalButtons.Cancel|AbpModalButtons.Save)"></abp-modal-footer>
    </abp-modal>
</abp-dynamic-form>
```



Esta página é muito semelhante à `CreateModal.cshtml`exceção;

- Ele inclui um `abp-input`para a `Id`propriedade armazenar o ID do livro de edição (que é uma entrada oculta).
- Ele usa `Books/EditModal`como URL de postagem e texto de *atualização* como cabeçalho modal.

#### Adicione o menu suspenso "Ações" à tabela

Adicionaremos um botão suspenso ("Ações") para cada linha da tabela. A interface do usuário final é assim:

![livraria-livros-mesa-ações](https://raw.githubusercontent.com/abpframework/abp/master/docs/en/Tutorials/AspNetCore-Mvc/images/bookstore-books-table-actions.png)

Abra a `Pages/Books/Index.cshtml`página e altere a seção da tabela como mostrado abaixo:

```html
<abp-table striped-rows="true" id="BooksTable">
    <thead>
        <tr>
            <th>@L["Actions"]</th>
            <th>@L["Name"]</th>
            <th>@L["Type"]</th>
            <th>@L["PublishDate"]</th>
            <th>@L["Price"]</th>
            <th>@L["CreationTime"]</th>
        </tr>
    </thead>
</abp-table>
```



- Acabei de adicionar uma nova `th`tag para as "Ações".

Abra `pages/books/index.js`e substitua o conteúdo como abaixo:

```js
$(function () {

    var l = abp.localization.getResource('BookStore');

    var createModal = new abp.ModalManager(abp.appPath + 'Books/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Books/EditModal');

    var dataTable = $('#BooksTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(acme.bookStore.book.getList),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l('Edit'),
                                action: function (data) {
                                    editModal.open({ id: data.record.id });
                                }
                            }
                        ]
                }
            },
            { data: "name" },
            { data: "type" },
            { data: "publishDate" },
            { data: "price" },
            { data: "creationTime" }
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewBookButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
```



- Utilizado `abp.localization.getResource('BookStore')`para poder usar os mesmos textos de localização definidos no lado do servidor.
- Adicionado um novo `ModalManager`nome `createModal`para abrir a caixa de diálogo criar modal.
- Adicionado um novo `ModalManager`nome `editModal`para abrir a caixa de diálogo modal de edição.
- Adicionada uma nova coluna no início da `columnDefs`seção. Esta coluna é usada para o botão suspenso "Ações".
- A ação "Novo livro" simplesmente chama `createModal.open`para abrir a caixa de diálogo Criar.
- A ação "Editar" simplesmente chama `editModal.open`para abrir a caixa de diálogo de edição. `Você pode executar o aplicativo e editar qualquer livro selecionando a ação de edição.

### Exclusão de um livro existente

Abra o `pages/books/index.js`e adicione um novo item ao `rowAction` `items`:

```js
{
    text: l('Delete'),
    confirmMessage: function (data) {
        return l('BookDeletionConfirmationMessage', data.record.name);
    },
    action: function (data) {
        acme.bookStore.book
            .delete(data.record.id)
            .then(function() {
                abp.notify.info(l('SuccessfullyDeleted'));
                dataTable.ajax.reload();
            });
    }
}
```



- `confirmMessage`A opção é usada para fazer uma pergunta de confirmação antes de executar o `action`.
- Utilizou a `acme.bookStore.book.delete`função de proxy javascript para executar uma solicitação AJAX para excluir um livro.
- `abp.notify.info` é usado para mostrar uma notificação toastr logo após a exclusão.

O `index.js`conteúdo final é mostrado abaixo:

```js
$(function () {

    var l = abp.localization.getResource('BookStore');

    var createModal = new abp.ModalManager(abp.appPath + 'Books/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Books/EditModal');

    var dataTable = $('#BooksTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(acme.bookStore.book.getList),
        columnDefs: [
            {
                rowAction: {
                    items:
                    [
                        {
                            text: l('Edit'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            confirmMessage: function (data) {
                                return l('BookDeletionConfirmationMessage', data.record.name);
                            },
                            action: function (data) {
                                acme.bookStore.book
                                    .delete(data.record.id)
                                    .then(function() {
                                        abp.notify.info(l('SuccessfullyDeleted'));
                                        dataTable.ajax.reload();
                                    });
                            }
                        }
                    ]
                }
            },
            { data: "name" },
            { data: "type" },
            { data: "publishDate" },
            { data: "price" },
            { data: "creationTime" }
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewBookButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
```



Abra o `en.json`no `Acme.BookStore.Domain.Shared`projeto e adicione a seguinte linha:

```json
"BookDeletionConfirmationMessage": "Are you sure to delete the book {0}?",
"SuccessfullyDeleted": "Successfully deleted"
```



Execute o aplicativo e tente excluir um livro.

### Próxima parte

Veja a [próxima parte](https://docs.abp.io/en/abp/latest/Tutorials/AspNetCore-Mvc/Part-III) deste tutorial.