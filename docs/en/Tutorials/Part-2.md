# Web Application Development Tutorial - Part 2: The Book List Page
````json
//[doc-params]
{
    "UI": ["MVC","NG"],
    "DB": ["EF","Mongo"]
}
````
{{
if UI == "MVC"
  UI_Text="mvc"
else if UI == "NG"
  UI_Text="angular"
else
  UI_Text="?"
end
if DB == "EF"
  DB_Text="Entity Framework Core"
else if DB == "Mongo"
  DB_Text="MongoDB"
else
  DB_Text="?"
end
}}

## About This Tutorial

In this tutorial series, you will build an ABP based web application named `Acme.BookStore`. This application is used to manage a list of books and their authors. It is developed using the following technologies:

* **{{DB_Text}}** as the ORM provider. 
* **{{UI_Value}}** as the UI Framework.

This tutorial is organized as the following parts;

- [Part 1: Creating the server side](Part-1.md)
- **Part 2: The book list page (this part)**
- [Part 3: Creating, updating and deleting books](Part-3.md)
- [Part 4: Integration tests](Part-4.md)
- [Part 5: Authorization](Part-5.md)

### Download the Source Code

This tutorials has multiple versions based on your **UI** and **Database** preferences. We've prepared two combinations of the source code to be downloaded:

* [MVC (Razor Pages) UI with EF Core](https://github.com/abpframework/abp-samples/tree/master/BookStore-Mvc-EfCore)
* [Angular UI with MongoDB](https://github.com/abpframework/abp-samples/tree/master/BookStore-Angular-MongoDb)

{{if UI == "MVC"}}

## Dynamic JavaScript Proxies

It's common to call the HTTP API endpoints via AJAX from the **JavaScript** side. You can use `$.ajax` or another tool to call the endpoints. However, ABP offers a better way.

ABP **dynamically** creates **[JavaScript Proxies](../UI/AspNetCore/)** for all API endpoints. So, you can use any **endpoint** just like calling a **JavaScript function**.

### Testing in the Developer Console

You can easily test the JavaScript proxies using your favorite browser's **Developer Console**. Run the application, open your browser's **developer tools** (*shortcut is generally F12*), switch to the **Console** tab, type the following code and press enter:

````js
acme.bookStore.books.book.getList({}).done(function (result) { console.log(result); });
````

* `acme.bookStore.books` is the namespace of the `BookAppService` converted to [camelCase](https://en.wikipedia.org/wiki/Camel_case).
* `book` is the conventional name for the `BookAppService` (removed `AppService` postfix and converted to camelCase).
* `getList` is the conventional name for the `GetListAsync` method defined in the `CrudAppService` base class (removed `Async` postfix and converted to camelCase).
* `{}` argument is used to send an empty object to the `GetListAsync` method which normally expects an object of type `PagedAndSortedResultRequestDto` that is used to send paging and sorting options to the server (all properties are optional with default values, so you can send an empty object).
* `getList` function returns a `promise`. You can pass a callback to the `then` (or `done`) function to get the result returned from the server.

Running this code produces the following output:

![bookstore-javascript-proxy-console](images/bookstore-javascript-proxy-console.png)

You can see the **book list** returned from the server. You can also check the **network** tab of the developer tools to see the client to server communication:

![bookstore-getlist-result-network](images/bookstore-getlist-result-network.png)

Let's **create a new book** using the `create` function:

````js
acme.bookStore.books.book.create({ 
        name: 'Foundation', 
        type: 7, 
        publishDate: '1951-05-24', 
        price: 21.5 
    }).then(function (result) { 
        console.log('successfully created the book with id: ' + result.id); 
    });
````

You should see a message in the console something like that:

````text
successfully created the book with id: 439b0ea8-923e-8e1e-5d97-39f2c7ac4246
````

Check the `Books` table in the database to see the new book row. You can try `get`, `update` and `delete` functions yourself.

We will use these dynamic proxy functions in the next sections to communicate to the server.

{{end}}

## Localization

Before starting to the UI development, we first want to prepare the localization texts (you normally do when needed while developing your application).

Localization texts are located under the `Localization/BookStore` folder of the `Acme.BookStore.Domain.Shared` project:

![bookstore-localization-files](./images/bookstore-localization-files-v2.png)

Open the `en.json` (*the English translations*) file and change the content as below:

````json
{
  "Culture": "en",
  "Texts": {
    "Menu:Home": "Home",
    "Welcome": "Welcome",
    "LongWelcomeMessage": "Welcome to the application. This is a startup project based on the ABP framework. For more information, visit abp.io.",
    "Menu:BookStore": "Book Store",
    "Menu:Books": "Books",
    "Actions": "Actions",
    "Edit": "Edit",
    "PublishDate": "Publish date",
    "NewBook": "New book",
    "Name": "Name",
    "Type": "Type",
    "Price": "Price",
    "CreationTime": "Creation time",
    "AreYouSureToDelete": "Are you sure you want to delete this item?",
    "Enum:BookType:0": "Undefined",
    "Enum:BookType:1": "Adventure",
    "Enum:BookType:2": "Biography",
    "Enum:BookType:3": "Dystopia",
    "Enum:BookType:4": "Fantastic",
    "Enum:BookType:5": "Horror",
    "Enum:BookType:6": "Science",
    "Enum:BookType:7": "Science fiction",
    "Enum:BookType:8": "Poetry"
  }
}
````

* Localization key names are arbitrary. You can set any name. We prefer some conventions for specific text types;
  * Add `Menu:` prefix for menu items.
  * Use `Enum:<enum-type>:<enum-value>` naming convention to localize the enum members. When you do it like that, ABP can automatically localize the enums in some proper cases.

If a text is not defined in the localization file, it **fallbacks** to the localization key (as ASP.NET Core's standard behavior).

> ABP's localization system is built on [ASP.NET Core's standard localization](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization) system and extends it in many ways. See the [localization document](../Localization.md) for details.

{{if UI == "MVC"}}

## Create a Books Page

It's time to create something visible and usable! Instead of classic MVC, we will use the [Razor Pages UI](https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/razor-pages-start) approach which is recommended by Microsoft.

Create `Books` folder under the `Pages` folder of the `Acme.BookStore.Web` project. Add a new Razor Page by right clicking the Books folder then selecting **Add > Razor Page** menu item. Name it as `Index`:

![bookstore-add-index-page](./images/bookstore-add-index-page-v2.png)

Open the `Index.cshtml` and change the whole content as shown below:

````html
@page
@using Acme.BookStore.Web.Pages.Books
@model IndexModel

<h2>Books</h2>
````

`Index.cshtml.cs` content should be like that:

```csharp
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Acme.BookStore.Web.Pages.Books
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            
        }
    }
}
```

### Add Books Page to the Main Menu

Open the `BookStoreMenuContributor` class in the `Menus` folder and add the following code to the end of the `ConfigureMainMenuAsync` method:

````csharp
context.Menu.AddItem(
    new ApplicationMenuItem(
        "BooksStore",
        l["Menu:BookStore"],
        icon: "fa fa-book"
    ).AddItem(
        new ApplicationMenuItem(
            "BooksStore.Books",
            l["Menu:Books"],
            url: "/Books"
        )
    )
);
````

Run the project, login to the application with the username `admin` and the password `1q2w3E*` and see the new menu item has been added to the main menu:

![bookstore-menu-items](./images/bookstore-new-menu-item.png)

When you click to the Books menu item under the Book Store parent, you are being redirected to the new empty Books Page.

### Book List

We will use the [Datatables.net](https://datatables.net/) jQuery library to show the book list. Datatables library completely work via AJAX, it is fast, popular and provides a good user experience.

> Datatables library is configured in the startup template, so you can directly use it in any page without including any style or script file to your page.

#### Index.cshtml

Change the `Pages/Books/Index.cshtml` as following:

````html
@page
@using Acme.BookStore.Localization
@using Acme.BookStore.Web.Pages.Books
@using Microsoft.Extensions.Localization
@model IndexModel
@inject IStringLocalizer<BookStoreResource> L
@section scripts
{
    <abp-script src="/Pages/Books/Index.js" />
}
<abp-card>
    <abp-card-header>
        <h2>@L["Books"]</h2>
    </abp-card-header>
    <abp-card-body>
        <abp-table striped-rows="true" id="BooksTable"></abp-table>
    </abp-card-body>
</abp-card>
````

* `abp-script` [tag helper](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/intro) is used to add external **scripts** to the page. It has many additional features compared to standard `script` tag. It handles **minification** and **versioning**. See the [bundling & minification document](../UI/AspNetCore/Bundling-Minification.md) for details.
* `abp-card` is a tag helper for Twitter Bootstrap's [card component](https://getbootstrap.com/docs/4.5/components/card/). There are other useful tag helpers provided by the ABP Framework to easily use most of the [bootstrap](https://getbootstrap.com/) components. You could use the regular HTML tags instead of these tag helpers, but using tag helpers reduces HTML code and prevents errors by help the of IntelliSense and compile time type checking. Further information, see the [tag helpers](../UI/AspNetCore/Tag-Helpers/Index.md) document.

#### Index.js

Create an `Index.js` file under the `Pages/Books` folder:

![bookstore-index-js-file](./images/bookstore-index-js-file-v3.png)

The content of the file is shown below:

````js
$(function () {
    var l = abp.localization.getResource('BookStore');

    var dataTable = $('#BooksTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(acme.bookStore.books.book.getList),
            columnDefs: [
                {
                    title: l('Name'),
                    data: "name"
                },
                {
                    title: l('Type'),
                    data: "type",
                    render: function (data) {
                        return l('Enum:BookType:' + data);
                    }
                },
                {
                    title: l('PublishDate'),
                    data: "publishDate",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toLocaleString();
                    }
                },
                {
                    title: l('Price'),
                    data: "price"
                },
                {
                    title: l('CreationTime'), data: "creationTime",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toLocaleString(luxon.DateTime.DATETIME_SHORT);
                    }
                }
            ]
        })
    );
});
````

* `abp.localization.getResource` gets a function that is used to localize text using the same JSON file defined in the server side. In this way, you can share the localization values with the client side.
* `abp.libs.datatables.normalizeConfiguration` is a helper function defined by the ABP Framework. There's no requirement to use it, but it simplifies the [Datatables](https://datatables.net/) configuration by providing conventional default values for missing options.
* `abp.libs.datatables.createAjax` is another helper function to adapt ABP's dynamic JavaScript API proxies to [Datatable](https://datatables.net/)'s expected parameter format
* `acme.bookStore.books.book.getList` is the dynamic JavaScript proxy function introduced before.
* [luxon](https://moment.github.io/luxon/) library is also a standard library that is pre-configured in the solution, so you can use to perform date/time operations easily.

> See [Datatables documentation](https://datatables.net/manual/) for all configuration options.

## Run the Final Application

You can run the application! The final UI of this part is shown below:

![Book list](images/bookstore-book-list-3.png)

This is a fully working, server side paged, sorted and localized table of books.

{{end}}

{{if UI == "NG"}}

## Install NPM packages

If you haven't done it before, open a new command line interface (terminal window) and go to your `angular` folder and then run `yarn` command to install NPM packages:

```bash
yarn
```

## Create a Books Page

It's time to create something visible and usable! There are some tools that we will use when developing the Angular frontend application:

- [Ng Bootstrap](https://ng-bootstrap.github.io/#/home) will be used as the UI component library.
- [Ngx-Datatable](https://swimlane.gitbook.io/ngx-datatable/) will be used as the datatable library.

### BookModule

Run the following command line to create a new module, named `BookModule` in the root folder of the angular application:

```bash
yarn ng generate module books --module app --routing --route books
```

This command should produce the following output:

![Generating books module](./images/bookstore-creating-book-module-terminal-2.png)

### Routing

Generated code places the new route definition to the `src/app/app-routing.module.ts` file as shown below:

````js
const routes: Routes = [
  {
    path: '',
    component: DynamicLayoutComponent,
    children: [
      // ...
    ],
  },
  { path: 'books', loadChildren: () => import('./books/books.module').then(m => m.BooksModule) },
];
````

We need to **move this route** definition inside the `children` array, so it can use the application layout:

````js
const routes: Routes = [
  {
    path: '',
    component: DynamicLayoutComponent,
    children: [
      // ...
      { path: 'books', loadChildren: () => import('./books/books.module').then(m => m.BooksModule) },
    ],
  },
];
````

Finally, open the `src/app/route.provider.ts` file replace the `configureRoutes` function declaration as shown below:

```js
function configureRoutes(routes: RoutesService) {
  return () => {
    routes.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/book-store',
        name: '::Menu:BookStore',
        iconClass: 'fas fa-book',
        order: 2,
        layout: eLayoutType.application,
      },
      {
        path: '/books',
        name: '::Menu:Books',
        parentName: '::Menu:BookStore',
        layout: eLayoutType.application,
      },
    ]);
  };
}
```

`RoutesService` is a service provided by the ABP Framework to configure the main menu and the routes.

* `path` is the URL of the route.
* `name` is the localized menu item name (see the [localization document](../UI/Angular/Localization.md) for details).
* `iconClass` is the icon of the menu item (you can use [Font Awesome](https://fontawesome.com/) icons by default).
* `order` is the order of the menu item.
* `layout` is the layout of the BooksModule's routes. `eLayoutType.application`, `eLayoutType.account` or `eLayoutType.empty` can be used.

For more information, see the [RoutesService document](https://docs.abp.io/en/abp/latest/UI/Angular/Modifying-the-Menu.md#via-routesservice).

### Book List Component

Run the command below on the terminal in the root folder to generate a new component, named book-list:

```bash
yarn ng generate component book/book-list
```

![Creating books list](./images/bookstore-creating-book-list-terminal.png)

Open `book.module.ts` file in the `app\book` folder and replace the content as below:

```js
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookRoutingModule } from './book-routing.module';
import { BookListComponent } from './book-list/book-list.component';
import { SharedModule } from '../shared/shared.module'; //<== added this line ==>

@NgModule({
  declarations: [BookListComponent],
  imports: [
    CommonModule,
    BookRoutingModule,
    SharedModule, //<== added this line ==>
  ],
})
export class BookModule {}
```

* We imported `SharedModule` and added to `imports` array.

Open `book-routing.module.ts`  file in the `app\book` folder and replace the content as below:

```js
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BookListComponent } from './book-list/book-list.component'; // <== added this line ==>

// <== replaced routes ==>
const routes: Routes = [
  {
    path: '',
    component: BookListComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BookRoutingModule { }
```

* We imported `BookListComponent` and replaced `routes` const.

Run `yarn start` and wait for Angular to serve the application:

```bash
yarn start
```

Open the browser and navigate to http://localhost:4200/books. We'll see **book-list works!**  text on the books page:

![Initial book list page](./images/bookstore-initial-book-list-page.png)

### Generate Proxies

ABP CLI provides `generate-proxy` command that generates client proxies for your HTTP APIs to make easy to consume your services from the client side. Before running generate-proxy command, your host must be up and running. See the [CLI documentation](../CLI.md)

Run the following command in the `angular` folder:

```bash
abp generate-proxy --module app
```

![Generate proxy command](./images/generate-proxy-command.png)

The generated files looks like below:

![Generated files](./images/generated-proxies.png)

### BookListComponent

Open the `book-list.component.ts` file in `app\book\book-list` folder and replace the content as below:

```js
import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { BookDto, BookType } from '../models';
import { BookService } from '../services';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss'],
  providers: [ListService],
})
export class BookListComponent implements OnInit {
  book = { items: [], totalCount: 0 } as PagedResultDto<BookDto>;

  booksType = BookType;

  constructor(public readonly list: ListService, private bookService: BookService) {}

  ngOnInit() {
    const bookStreamCreator = (query) => this.bookService.getListByInput(query);

    this.list.hookToQuery(bookStreamCreator).subscribe((response) => {
      this.book = response;
    });
  }
}
```

* We imported and injected the generated `BookService`.
* We implemented the [ListService](https://docs.abp.io/en/abp/latest/UI/Angular/List-Service) that is a utility service to provide easy pagination, sorting, and search implementation.

Open the `book-list.component.html` file in `app\book\book-list` folder and replace the content as below:

```html
<div class="card">
  <div class="card-header">
    <div class="row">
      <div class="col col-md-6">
        <h5 class="card-title">
          {%{{{ '::Menu:Books' | abpLocalization }}}%}
        </h5>
      </div>
      <div class="text-right col col-md-6"></div>
    </div>
  </div>
  <div class="card-body">
    <ngx-datatable [rows]="book.items" [count]="book.totalCount" [list]="list" default>
      <ngx-datatable-column [name]="'::Name' | abpLocalization" prop="name"></ngx-datatable-column>
      <ngx-datatable-column [name]="'::Type' | abpLocalization" prop="type">
        <ng-template let-row="row" ngx-datatable-cell-template>
          {%{{{ booksType[row.type] }}}%}
        </ng-template>
      </ngx-datatable-column>
      <ngx-datatable-column [name]="'::PublishDate' | abpLocalization" prop="publishDate">
        <ng-template let-row="row" ngx-datatable-cell-template>
          {%{{{ row.publishDate | date }}}%}
        </ng-template>
      </ngx-datatable-column>
      <ngx-datatable-column [name]="'::Price' | abpLocalization" prop="price">
        <ng-template let-row="row" ngx-datatable-cell-template>
          {%{{{ row.price | currency }}}%}
        </ng-template>
      </ngx-datatable-column>
    </ngx-datatable>
  </div>
</div>
```

* We added HTML code of book list page.

Now you can see the final result on your browser:

![Book list final result](./images/bookstore-book-list.png)

The file system structure of the project:

![Book list final result](./images/bookstore-angular-file-tree.png)

In this tutorial we have applied the rules of official [Angular Style Guide](https://angular.io/guide/styleguide#file-tree).

{{end}}

## The Next Part

See the [next part](Part-3.md) of this tutorial.
