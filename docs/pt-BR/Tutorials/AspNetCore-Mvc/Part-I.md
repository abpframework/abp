## Tutorial do ASP.NET Core MVC - Parte I

### Sobre este tutorial

Nesta série de tutoriais, você criará um aplicativo usado para gerenciar uma lista de livros e seus autores. **O Entity Framework Core** (EF Core) será usado como o provedor ORM, pois é o provedor de banco de dados padrão.

Esta é a primeira parte da série de tutoriais do ASP.NET Core MVC. Veja todas as peças:

- **Parte I: Crie o projeto e uma página de lista de livros (este tutorial)**
- [Parte II: Criar, atualizar e excluir livros](Part-II.md)
- [Parte III: Testes de Integração](Part-III.md)

Você pode acessar o **código fonte** do aplicativo [no repositório GitHub](https://github.com/abpframework/abp-samples/tree/master/BookStore) .

> Você também pode assistir a [este curso em vídeo](https://amazingsolutions.teachable.com/p/lets-build-the-bookstore-application) preparado por um membro da comunidade ABP, com base neste tutorial.

### Criando o projeto

Crie um novo projeto chamado `Acme.BookStore`, crie o banco de dados e execute o aplicativo seguindo o [documento Introdução](Getting-Started-AspNetCore-MVC-Template.md).

### Estrutura da solução

É assim que a estrutura da solução em camadas cuida da criação:

![livraria-visual-studio-solução](images/bookstore-visual-studio-solution-v3.png)

> Você pode ver o [documento do modelo de aplicativo](https://docs.abp.io/en/abp/latest/Startup-Templates/Application) para entender a estrutura da solução em detalhes. No entanto, você entenderá o básico com este tutorial.

### Criar a entidade do livro

A camada de domínio no modelo de inicialização é separada em dois projetos:

- `Acme.BookStore.Domain`contém suas [entidades](https://docs.abp.io/en/abp/latest/Entities.md) , [serviços de domínio](https://docs.abp.io/en/abp/latest/Domain-Services) e outros objetos principais de domínio.
- `Acme.BookStore.Domain.Shared` contém constantes, enumerações ou outros objetos relacionados ao domínio que podem ser compartilhados com os clientes.

Defina [entidades](https://docs.abp.io/en/abp/latest/Entities) na **camada de domínio** ( `Acme.BookStore.Domain`projeto) da solução. A entidade principal do aplicativo é a `Book`. Crie uma classe, chamada `Book`, no `Acme.BookStore.Domain`projeto, como mostrado abaixo:

```csharp
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore
{
    public class Book : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }
    }
}
```

- O ABP possui duas classes base fundamentais para entidades: `AggregateRoot`e `Entity`. **A raiz agregada** é um dos conceitos de **DDD (Domain Driven Design)** . Consulte o [documento da entidade](https://docs.abp.io/en/abp/latest/Entities) para obter detalhes e melhores práticas.
- `Book`entidade herda `AuditedAggregateRoot`que adiciona algumas propriedades de auditoria ( `CreationTime`, `CreatorId`, `LastModificationTime`... etc.) no topo da `AggregateRoot`classe.
- `Guid`é o **tipo** de **chave primária** da `Book`entidade.

#### BookType Enum

Defina a `BookType`enumeração no `Acme.BookStore.Domain.Shared`projeto:

```csharp
namespace Acme.BookStore
{
    public enum BookType
    {
        Undefined,
        Adventure,
        Biography,
        Dystopia,
        Fantastic,
        Horror,
        Science,
        ScienceFiction,
        Poetry
    }
}
```

#### Adicionar entidade de livro ao seu DbContext

O EF Core exige que você relacione entidades com seu DbContext. A maneira mais fácil de fazer isso é adicionar uma `DbSet`propriedade à `BookStoreDbContext`classe no `Acme.BookStore.EntityFrameworkCore`projeto, conforme mostrado abaixo:

```csharp
    public class BookStoreDbContext : AbpDbContext<BookStoreDbContext>
    {
        public DbSet<Book> Books { get; set; }
		...
    }
```

#### Configure sua entidade do livro

Abra o `BookStoreDbContextModelCreatingExtensions.cs`arquivo no `Acme.BookStore.EntityFrameworkCore`projeto e adicione o seguinte código ao final do `ConfigureBookStore`método para configurar a entidade Livro:

```csharp
builder.Entity<Book>(b =>
{
    b.ToTable(BookStoreConsts.DbTablePrefix + "Books", BookStoreConsts.DbSchema);
    b.ConfigureByConvention(); //auto configure for the base class props
    b.Property(x => x.Name).IsRequired().HasMaxLength(128);
});
```

#### Adicionar nova migração e atualizar o banco de dados

O modelo de inicialização usa [as primeiras migrações do código principal EF](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/) para criar e manter o esquema do banco de dados. Abra o **Gerenciador de Console Package (PMC)** (sob as *Ferramentas / Gerente Nuget Package* menu), selecione o `Acme.BookStore.EntityFrameworkCore.DbMigrations`como o **projeto padrão** e execute o seguinte comando:

![livraria-pmc-add-book-migration](https://raw.githubusercontent.com/abpframework/abp/master/docs/en/Tutorials/AspNetCore-Mvc/images/bookstore-pmc-add-book-migration-v2.png)

Isso criará uma nova classe de migração dentro da `Migrations`pasta. Em seguida, execute o `Update-Database`comando para atualizar o esquema do banco de dados:

```
PM> Update-Database
```

#### Adicionar dados de amostra

`Update-Database`O comando criou a `AppBooks`tabela no banco de dados. Abra seu banco de dados e insira algumas linhas de amostra, para que você possa mostrá-las na página:

![livraria-livros-mesa](https://raw.githubusercontent.com/abpframework/abp/master/docs/en/Tutorials/AspNetCore-Mvc/images/bookstore-books-table.png)

### Crie o serviço de aplicativo

O próximo passo é criar um [serviço de aplicativo](https://docs.abp.io/en/abp/latest/Application-Services) para gerenciar (criar, listar, atualizar, excluir ...) os livros. A camada de aplicativo no modelo de inicialização é separada em dois projetos:

- `Acme.BookStore.Application.Contracts` contém principalmente seus DTOs e interfaces de serviço de aplicativo.
- `Acme.BookStore.Application` contém as implementações dos seus serviços de aplicativo.

#### BookDto

Crie uma classe DTO denominada `BookDto`no `Acme.BookStore.Application.Contracts`projeto:

```csharp
using System;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore
{
    public class BookDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }
    }
}
```

- **As** classes **DTO** são usadas para **transferir dados** entre a *camada de apresentação* e a *camada de aplicativo* . Consulte o [documento Objetos de transferência de dados](https://docs.abp.io/en/abp/latest/Data-Transfer-Objects) para obter mais detalhes.
- `BookDto` é usado para transferir dados do livro para a camada de apresentação para mostrar as informações do livro na interface do usuário.
- `BookDto`é derivado do `AuditedEntityDto<Guid>`que possui propriedades de auditoria exatamente como a `Book`classe definida acima.

Será necessário converter `Book`entidades em `BookDto`objetos enquanto retorna os livros para a camada de apresentação. [A](https://automapper.org/) biblioteca do [AutoMapper](https://automapper.org/) pode automatizar essa conversão quando você define o mapeamento adequado. O modelo de inicialização é fornecido com o AutoMapper configurado, para que você possa definir o mapeamento na `BookStoreApplicationAutoMapperProfile`classe no `Acme.BookStore.Application`projeto:

```csharp
using AutoMapper;

namespace Acme.BookStore
{
    public class BookStoreApplicationAutoMapperProfile : Profile
    {
        public BookStoreApplicationAutoMapperProfile()
        {
            CreateMap<Book, BookDto>();
        }
    }
}
```

#### CreateUpdateBookDto

Crie uma classe DTO denominada `CreateUpdateBookDto`no `Acme.BookStore.Application.Contracts`projeto:

```csharp
using System;
using System.ComponentModel.DataAnnotations;

namespace Acme.BookStore
{
    public class CreateUpdateBookDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        public BookType Type { get; set; } = BookType.Undefined;

        [Required]
        public DateTime PublishDate { get; set; }

        [Required]
        public float Price { get; set; }
    }
}
```

- Essa classe DTO é usada para obter informações do livro a partir da interface do usuário ao criar ou atualizar um livro.
- Ele define atributos de anotação de dados (como `[Required]`) para definir validações para as propriedades. Os DTOs são [validados automaticamente](https://docs.abp.io/en/abp/latest/Validation) pela estrutura ABP.

Em seguida, adicione um mapeamento `BookStoreApplicationAutoMapperProfile`do `CreateUpdateBookDto`objeto à `Book`entidade:

```csharp
CreateMap<CreateUpdateBookDto, Book>();
```

#### IBookAppService

Defina uma interface nomeada `IBookAppService`no `Acme.BookStore.Application.Contracts`projeto:

```csharp
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore
{
    public interface IBookAppService :
        ICrudAppService< //Defines CRUD methods
            BookDto, //Used to show books
            Guid, //Primary key of the book entity
            PagedAndSortedResultRequestDto, //Used for paging/sorting on getting a list of books
            CreateUpdateBookDto, //Used to create a new book
            CreateUpdateBookDto> //Used to update a book
    {

    }
}
```

- A definição de interfaces para serviços de aplicativos não é requerida pela estrutura. No entanto, é sugerido como uma prática recomendada.
- `ICrudAppService`define comuns **CRUD** métodos: `GetAsync`, `GetListAsync`, `CreateAsync`, `UpdateAsync`e `DeleteAsync`. Não é necessário estendê-lo. Em vez disso, você pode herdar da `IApplicationService`interface vazia e definir seus próprios métodos manualmente.
- Existem algumas variações de `ICrudAppService`onde você pode usar DTOs separados para cada método.

#### BookAppService

Implemente `IBookAppService`como nomeado `BookAppService`no `Acme.BookStore.Application`projeto:

```csharp
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore
{
    public class BookAppService :
        CrudAppService<Book, BookDto, Guid, PagedAndSortedResultRequestDto,
                       CreateUpdateBookDto, CreateUpdateBookDto>,
        IBookAppService
    {
        public BookAppService(IRepository<Book, Guid> repository)
            : base(repository)
        {

        }
    }
}
```

- `BookAppService`é derivado do `CrudAppService<...>`qual implementa todos os métodos CRUD definidos acima.
- `BookAppService`injeta `IRepository<Book, Guid>`qual é o repositório padrão da `Book`entidade. O ABP cria automaticamente repositórios padrão para cada raiz (ou entidade) agregada. Veja o [documento](https://docs.abp.io/en/abp/latest/Repositories) do [repositório](https://docs.abp.io/en/abp/latest/Repositories) .
- `BookAppService`usa `IObjectMapper`para converter `Book`objetos em `BookDto`objetos e `CreateUpdateBookDto`objetos em `Book`objetos. O modelo de inicialização usa a biblioteca [AutoMapper](http://automapper.org/) como o provedor de mapeamento de objetos. Você definiu os mapeamentos antes, para que funcionem conforme o esperado.

### Controladores de API automática

Você normalmente cria **controladores** para expor serviços de aplicativos como pontos de extremidade da **API HTTP** . Assim, permite que navegadores ou clientes de terceiros os chamem via AJAX. O ABP pode configurar [**automaticamente**](https://docs.abp.io/en/abp/latest/AspNetCore/Auto-API-Controllers) seus serviços de aplicativo como controladores de API MVC por convenção.

#### UI do Swagger

O modelo de inicialização está configurado para executar a [interface do usuário](https://swagger.io/tools/swagger-ui/) do [swagger](https://swagger.io/tools/swagger-ui/) usando a biblioteca [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) . Execute o aplicativo e insira `https://localhost:XXXX/swagger/`(substitua XXXX por sua própria porta) como URL no seu navegador.

Você verá alguns pontos de extremidade de serviço internos, bem como o `Book`serviço e seus pontos de extremidade no estilo REST:

![livraria-arrogância](https://raw.githubusercontent.com/abpframework/abp/master/docs/en/Tutorials/AspNetCore-Mvc/images/bookstore-swagger.png)

O Swagger tem uma ótima interface para testar APIs. Você pode tentar executar a `[GET] /api/app/book`API para obter uma lista de livros.

### Proxies dinâmicos de JavaScript

É comum chamar pontos de extremidade da API HTTP via AJAX do lado do **JavaScript** . Você pode usar `$.ajax`ou outra ferramenta para chamar os pontos de extremidade. No entanto, o ABP oferece uma maneira melhor.

O ABP cria **dinamicamente** **proxies** JavaScript para todos os pontos de extremidade da API. Portanto, você pode usar qualquer **terminal,** assim como chamar uma **função JavaScript** .

#### Testando no console do desenvolvedor do navegador

Você pode testar facilmente os proxies JavaScript usando o **Console** do **desenvolvedor** do seu navegador favorito agora. Execute o aplicativo, abra as **ferramentas de desenvolvedor** do navegador (atalho: F12), vá para a guia **Console** , digite o seguinte código e pressione enter:

```js
acme.bookStore.book.getList({}).done(function (result) { console.log(result); });
```

- `acme.bookStore`é o espaço para nome do `BookAppService`convertido em [camelCase](https://en.wikipedia.org/wiki/Camel_case) .
- `book`é o nome convencional para o `BookAppService`(postfix do AppService removido e convertido em camelCase).
- `getList`é o nome convencional para o `GetListAsync`método definido na `AsyncCrudAppService`classe base (postfix assíncrono removido e convertido em camelCase).
- `{}`O argumento é usado para enviar um objeto vazio ao `GetListAsync`método que normalmente espera um objeto do tipo `PagedAndSortedResultRequestDto`usado para enviar opções de paginação e classificação ao servidor (todas as propriedades são opcionais, para que você possa enviar um objeto vazio).
- `getList`A função retorna a `promise`. Portanto, você pode passar um retorno de chamada para a função `done`(ou `then`) para obter o resultado do servidor.

A execução desse código produz a seguinte saída:

![livraria-teste-js-proxy-getlist](https://raw.githubusercontent.com/abpframework/abp/master/docs/en/Tutorials/AspNetCore-Mvc/images/bookstore-test-js-proxy-getlist.png)

Você pode ver a **lista de livros** retornada do servidor. Você também pode verificar a guia de **rede** das ferramentas do desenvolvedor para ver a comunicação do cliente com o servidor:

![livraria-teste-js-proxy-getlist-rede](https://raw.githubusercontent.com/abpframework/abp/master/docs/en/Tutorials/AspNetCore-Mvc/images/bookstore-test-js-proxy-getlist-network.png)

Vamos **criar um novo livro** usando a `create`função:

```js
acme.bookStore.book.create({ name: 'Foundation', type: 7, publishDate: '1951-05-24', price: 21.5 }).done(function (result) { console.log('successfully created the book with id: ' + result.id); });
```

Você deve ver uma mensagem no console, algo assim:

```
successfully created the book with id: f3f03580-c1aa-d6a9-072d-39e75c69f5c7
```

Verifique a `Books`tabela no banco de dados para ver a nova linha do livro. Você pode tentar `get`, `update`e `delete`funciona mesmo.

### Crie a página de livros

É hora de criar algo visível e utilizável! Em vez do MVC clássico, usaremos a nova abordagem de [interface do usuário do Razor Pages,](https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/razor-pages-start) recomendada pela Microsoft.

Crie uma nova `Books`pasta na `Pages`pasta do `Acme.BookStore.Web`projeto e adicione uma nova página Razor denominada `Index.cshtml`:

![livraria-add-index-page](https://raw.githubusercontent.com/abpframework/abp/master/docs/en/Tutorials/AspNetCore-Mvc/images/bookstore-add-index-page-v2.png)

Abra `Index.cshtml`e altere o conteúdo, como mostrado abaixo:

```html
@page
@using Acme.BookStore.Web.Pages.Books
@model IndexModel

<h2>Books</h2>
```

- Verifique se o `IndexModel`( *Index.cshtml.cs)* possui o `Acme.BookStore.Pages.Books`espaço para nome ou atualize-o no `Index.cshtml`.

#### Adicionar página de livros ao menu principal

Abra a `BookStoreMenuContributor`classe na `Menus`pasta e adicione o seguinte código ao final do `ConfigureMainMenuAsync`método:

```csharp
context.Menu.AddItem(
    new ApplicationMenuItem("BooksStore", l["Menu:BookStore"])
        .AddItem(new ApplicationMenuItem("BooksStore.Books", l["Menu:Books"], url: "/Books"))
);
```

#### Localizando os itens de menu

Os textos de localização estão localizados na `Localization/BookStore`pasta do `Acme.BookStore.Domain.Shared`projeto:

![arquivos de localização de livraria](https://raw.githubusercontent.com/abpframework/abp/master/docs/en/Tutorials/AspNetCore-Mvc/images/bookstore-localization-files-v2.png)

Abra o `en.json`arquivo e adicione textos de localização `Menu:BookStore`e `Menu:Books`chaves ao final do arquivo:

```json
{
  "culture": "en",
  "texts": {
    "Menu:BookStore": "Book Store",
    "Menu:Books": "Books"
  }
}
```

- O sistema de localização da ABP é construído no sistema de [localização padrão do ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization) e o estende de várias maneiras. Consulte o [documento de localização](https://docs.abp.io/en/abp/latest/Localization) para obter detalhes.
- Os nomes das chaves de localização são arbitrários. Você pode definir qualquer nome. Preferimos adicionar `Menu:`prefixo aos itens de menu para distinguir de outros textos. Se um texto não estiver definido no arquivo de localização, ele **recuará** para a chave de localização (comportamento padrão do ASP.NET Core).

Execute o aplicativo e veja se o novo item de menu foi adicionado à barra superior:

![itens-menu-livraria](https://raw.githubusercontent.com/abpframework/abp/master/docs/en/Tutorials/AspNetCore-Mvc/images/bookstore-menu-items.png)

Quando você clica no item de menu Livros, você é redirecionado para a nova página Livros.

#### Lista de livros

Usaremos o plug-in [Datatables.net](https://datatables.net/) JQuery para mostrar a lista de tabelas na página. As tabelas de dados podem funcionar completamente via AJAX, são rápidas e oferecem uma boa experiência ao usuário. O plug-in Datatables está configurado no modelo de inicialização, para que você possa usá-lo diretamente em qualquer página sem incluir nenhum estilo ou arquivo de script em sua página.

##### Index.cshtml

Altere o `Pages/Books/Index.cshtml`seguinte:

```html
@page
@model Acme.BookStore.Web.Pages.Books.IndexModel
@section scripts
{
    <abp-script src="/Pages/Books/index.js" />
}
<abp-card>
    <abp-card-header>
        <h2>@L["Books"]</h2>
    </abp-card-header>
    <abp-card-body>
        <abp-table striped-rows="true" id="BooksTable">
            <thead>
                <tr>
                    <th>@L["Name"]</th>
                    <th>@L["Type"]</th>
                    <th>@L["PublishDate"]</th>
                    <th>@L["Price"]</th>
                    <th>@L["CreationTime"]</th>
                </tr>
            </thead>
        </abp-table>
    </abp-card-body>
</abp-card>
```

- `abp-script` [O auxiliar de marca](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/intro) é usado para adicionar **scripts** externos à página. Possui muitos recursos adicionais em comparação com a `script`tag padrão . Ele lida com **minificação** e **controle** de **versão,** por exemplo. Consulte o [documento de compactação e redução](https://docs.abp.io/en/abp/latest/AspNetCore/Bundling-Minification) para obter detalhes.
- `abp-card`e `abp-table`são **auxiliares de tags** para o [componente de cartão](http://getbootstrap.com/docs/4.1/components/card/) do Twitter Bootstrap . Existem muitos auxiliares de tag no ABP para usar facilmente a maioria dos componentes de [autoinicialização](https://getbootstrap.com/) . Você também pode usar tags HTML regulares em vez desses auxiliares de tag, mas o uso de tag reduz o código HTML e evita erros com a ajuda do intellisense e da verificação do tipo de tempo de compilação. Consulte o [documento auxiliares](https://docs.abp.io/en/abp/latest/AspNetCore/Tag-Helpers) da [tag](https://docs.abp.io/en/abp/latest/AspNetCore/Tag-Helpers) .
- Você pode **localizar** os nomes das colunas no arquivo de localização, como fez nos itens de menu acima.

##### Adicionar um arquivo de script

Crie um `index.js`arquivo JavaScript na `Pages/Books/`pasta:

![arquivo-index-js-bookstore](https://raw.githubusercontent.com/abpframework/abp/master/docs/en/Tutorials/AspNetCore-Mvc/images/bookstore-index-js-file-v2.png)

`index.js` o conteúdo é mostrado abaixo:

```js
$(function () {
    var dataTable = $('#BooksTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        ajax: abp.libs.datatables.createAjax(acme.bookStore.book.getList),
        columnDefs: [
            { data: "name" },
            { data: "type" },
            { data: "publishDate" },
            { data: "price" },
            { data: "creationTime" }
        ]
    }));
});
```

- `abp.libs.datatables.createAjax` é uma função auxiliar para adaptar os proxies dinâmicos da API JavaScript da ABP ao formato do Datatable.
- `abp.libs.datatables.normalizeConfiguration`é outra função auxiliar. Não há necessidade de usá-lo, mas simplifica a configuração das tabelas de dados, fornecendo valores convencionais para as opções ausentes.
- `acme.bookStore.book.getList` é a função para obter a lista de livros (você já viu isso antes).
- Consulte [a documentação do Datatable](https://datatables.net/manual/) para obter mais opções de configuração.

A interface do usuário final é mostrada abaixo:

![livraria-lista-de-livros](https://raw.githubusercontent.com/abpframework/abp/master/docs/en/Tutorials/AspNetCore-Mvc/images/bookstore-book-list-2.png)

### Próxima parte

Veja a [próxima parte](https://docs.abp.io/en/abp/latest/Tutorials/AspNetCore-Mvc/Part-II) deste tutorial.
