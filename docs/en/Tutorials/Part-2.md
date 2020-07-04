# ASP.NET Core {{UI_Value}} Tutorial - Part 2
````json
//[doc-params]
{
    "UI": ["MVC","NG"]
}
````

{{
if UI == "MVC"
  DB="ef"
  DB_Text="Entity Framework Core"
  UI_Text="mvc"
else if UI == "NG"
  DB="mongodb"
  DB_Text="MongoDB"
  UI_Text="angular"
else
  DB ="?"
  UI_Text="?"
end
}}

## About This Tutorial

In this tutorial series, you will build an ABP based web application named `Acme.BookStore`. This application is used to manage a list of books and their authors. It is developed using the following technologies:

* **{{DB_Text}}** as the ORM provider. 
* **{{UI_Value}}** as the UI Framework.

This tutorial is organized as the following parts;

- [Part I: Creating the project and book list page](part-1.md)
- **Part-2: Creating, updating and deleting books (this part)**
- [Part-3: Integration tests](part-3.md)

{{if UI == "MVC"}}

## Creating a New Book

In this section, you will learn how to create a new modal dialog form to create a new book. The modal dialog will look like in the image below:

![bookstore-create-dialog](./images/bookstore-create-dialog-2.png)

### Create the Modal Form

Create a new razor page, named `CreateModal.cshtml` under the `Pages/Books` folder of the `Acme.BookStore.Web` project.

![bookstore-add-create-dialog](./images/bookstore-add-create-dialog-v2.png)

#### CreateModal.cshtml.cs

Open the `CreateModal.cshtml.cs` file (`CreateModalModel` class) and replace with the following code:

````C#
using System.Threading.Tasks;
using Acme.BookStore.Books;
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

        public void OnGet()
        {
            Book = new CreateUpdateBookDto();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _bookAppService.CreateAsync(Book);
            return NoContent();
        }
    }
}
````

* This class is derived from the `BookStorePageModel` instead of standard `PageModel`. `BookStorePageModel` indirectly inherits the `PageModel` and adds some common properties & methods that can be shared in your page model classes.
* `[BindProperty]` attribute on the `Book` property binds post request data to this property.
* This class simply injects the `IBookAppService` in the constructor and calls the `CreateAsync` method in the `OnPostAsync` handler.
* It creates a new `CreateUpdateBookDto` object in the `OnGet` method. ASP.NET Core can work without creating a new instance like that. However, it doesn't create an instance for you and if your class has some default value assignments or code execution in the class constructor, they won't work. For this case, we set default values for some of the `CreateUpdateBookDto` properties.

#### CreateModal.cshtml

Open the `CreateModal.cshtml` file and paste the code below:

````html
@page
@using Acme.BookStore.Localization
@using Acme.BookStore.Web.Pages.Books
@using Microsoft.Extensions.Localization
@using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
@model CreateModalModel
@inject IStringLocalizer<BookStoreResource> L
@{
    Layout = null;
}
<abp-dynamic-form abp-model="Book" asp-page="/Books/CreateModal">
    <abp-modal>
        <abp-modal-header title="@L["NewBook"].Value"></abp-modal-header>
        <abp-modal-body>
            <abp-form-content />
        </abp-modal-body>
        <abp-modal-footer buttons="@(AbpModalButtons.Cancel|AbpModalButtons.Save)"></abp-modal-footer>
    </abp-modal>
</abp-dynamic-form>
````

* This modal uses `abp-dynamic-form` [tag helper](../UI/AspNetCore/Tag-Helpers/Dynamic-Forms.md) to automatically create the form from the  `CreateBookViewModel` model class.
* `abp-model` attribute indicates the model object where it's the `Book` property in this case.
* `abp-form-content` tag helper is a placeholder to render the form controls (it is optional and needed only if you have added some other content in the `abp-dynamic-form` tag, just like in this page).

> Tip: `Layout` should be `null` just as done in this example since we don't want to include all the layout for the modals when they are loaded via AJAX.

### Add the "New book" Button

Open the `Pages/Books/Index.cshtml` and set the content of `abp-card-header` tag as below:

````html
<abp-card-header>
    <abp-row>
        <abp-column size-md="_6">
            <abp-card-title>@L["Books"]</abp-card-title>
        </abp-column>
        <abp-column size-md="_6" class="text-right">
            <abp-button id="NewBookButton"
                        text="@L["NewBook"].Value"
                        icon="plus"
                        button-type="Primary"/>
        </abp-column>
    </abp-row>
</abp-card-header>
````

The final content of the `Index.cshtml` is shown below:

````html
@page
@using Acme.BookStore.Localization
@using Acme.BookStore.Web.Pages.Books
@using Microsoft.Extensions.Localization
@model IndexModel
@inject IStringLocalizer<BookStoreResource> L
@section scripts
{
    <abp-script src="/Pages/Books/Index.js"/>
}

<abp-card>
    <abp-card-header>
        <abp-row>
            <abp-column size-md="_6">
                <abp-card-title>@L["Books"]</abp-card-title>
            </abp-column>
            <abp-column size-md="_6" class="text-right">
                <abp-button id="NewBookButton"
                            text="@L["NewBook"].Value"
                            icon="plus"
                            button-type="Primary"/>
            </abp-column>
        </abp-row>
    </abp-card-header>
    <abp-card-body>
        <abp-table striped-rows="true" id="BooksTable"></abp-table>
    </abp-card-body>
</abp-card>
````

This adds a new button called **New book** to the **top-right** of the table:

![bookstore-new-book-button](./images/bookstore-new-book-button-2.png)

Open the `Pages/Books/Index.js` and add the following code just after the `Datatable` configuration:

````js
var createModal = new abp.ModalManager(abp.appPath + 'Books/CreateModal');

createModal.onResult(function () {
    dataTable.ajax.reload();
});

$('#NewBookButton').click(function (e) {
    e.preventDefault();
    createModal.open();
});
````

* `abp.ModalManager` is a helper class to manage modals in the client side. It internally uses Twitter Bootstrap's standard modal, but abstracts many details by providing a simple API.
* `createModal.onResult(...)` used to refresh the data table after creating a new book.
* `createModal.open();` is used to open the model to create a new book.

The final content of the `Index.js` should be like that:

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

    var createModal = new abp.ModalManager(abp.appPath + 'Books/CreateModal');

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewBookButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
````

Now, you can **run the application** and add some new books using the new modal form.

## Updating a Book

Create a new razor page, named `EditModal.cshtml` under the `Pages/Books` folder of the `Acme.BookStore.Web` project:

![bookstore-add-edit-dialog](./images/bookstore-add-edit-dialog.png)

### EditModal.cshtml.cs

Open the `EditModal.cshtml.cs` file (`EditModalModel` class) and replace with the following code:

````csharp
using System;
using System.Threading.Tasks;
using Acme.BookStore.Books;
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
````

* `[HiddenInput]` and `[BindProperty]` are standard ASP.NET Core MVC attributes. `SupportsGet` is used to be able to get `Id` value from query string parameter of the request.
* In the `OnGetAsync` method, we get `BookDto ` from the `BookAppService` and this is being mapped to the DTO object `CreateUpdateBookDto`.
* The `OnPostAsync` uses `BookAppService.UpdateAsync(...)` to update the entity.

### Mapping from BookDto to CreateUpdateBookDto

To be able to map the `BookDto` to `CreateUpdateBookDto`, configure a new mapping. To do this, open the `BookStoreWebAutoMapperProfile.cs` in the `Acme.BookStore.Web` project and change it as shown below:

````csharp
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
````

* We have just added `CreateMap<BookDto, CreateUpdateBookDto>();` to define this mapping.

> Notice that we do the mapping definition in the web layer as a best practice since it is only needed in this layer.

### EditModal.cshtml

Replace `EditModal.cshtml` content with the following content:

````html
@page
@using Acme.BookStore.Localization
@using Acme.BookStore.Web.Pages.Books
@using Microsoft.Extensions.Localization
@using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal
@model EditModalModel
@inject IStringLocalizer<BookStoreResource> L
@{
    Layout = null;
}
<abp-dynamic-form abp-model="Book" asp-page="/Books/EditModal">
    <abp-modal>
        <abp-modal-header title="@L["Update"].Value"></abp-modal-header>
        <abp-modal-body>
            <abp-input asp-for="Id" />
            <abp-form-content />
        </abp-modal-body>
        <abp-modal-footer buttons="@(AbpModalButtons.Cancel|AbpModalButtons.Save)"></abp-modal-footer>
    </abp-modal>
</abp-dynamic-form>
````

This page is very similar to the `CreateModal.cshtml`, except:

* It includes an `abp-input` for the `Id` property to store `Id` of the editing book (which is a hidden input).
* It uses `Books/EditModal` as the post URL.

### Add "Actions" Dropdown to the Table

We will add a dropdown button to the table named *Actions*.

Open the `Pages/Books/Index.js` and replace the content as below:

````js
$(function () {
    var l = abp.localization.getResource('BookStore');
    var createModal = new abp.ModalManager(abp.appPath + 'Books/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Books/EditModal');

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
                    title: l('Actions'),
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
````

* Added a new `ModalManager` named `editModal` to open the edit modal dialog.
* Added a new column at the beginning of the `columnDefs` section. This column is used for the "*Actions*" dropdown button.
* "*Edit*" action simply calls `editModal.open()` to open the edit dialog.
* `editModal.onResult(...)`  callback refreshes the data table when you close the edit modal.

You can run the application and edit any book by selecting the edit action on a book.

The final UI looks as below:

![bookstore-books-table-actions](./images/bookstore-edit-button-2.png)

## Deleting a Book

Open the `Pages/Books/Index.js` and add a new item to the `rowAction` `items`:

````js
{
    text: l('Delete'),
    confirmMessage: function (data) {
        return l('BookDeletionConfirmationMessage', data.record.name);
    },
    action: function (data) {
        acme.bookStore.books.book
            .delete(data.record.id)
            .then(function() {
                abp.notify.info(l('SuccessfullyDeleted'));
                dataTable.ajax.reload();
            });
    }
}
````

* `confirmMessage` option is used to ask a confirmation question before executing the `action`.
* `acme.bookStore.books.book.delete(...)` method makes an AJAX request to the server to delete a book.
* `abp.notify.info()` shows a notification after the delete operation.

Since we've used two new localization texts (`BookDeletionConfirmationMessage` and `SuccessfullyDeleted`) you need to add these to the localization file (`en.json` under the `Localization/BookStore` folder of the `Acme.BookStore.Domain.Shared` project):

````json
"BookDeletionConfirmationMessage": "Are you sure to delete the book '{0}'?",
"SuccessfullyDeleted": "Successfully deleted!"
````

The final `Index.js` content is shown below:

````js
$(function () {
    var l = abp.localization.getResource('BookStore');
    var createModal = new abp.ModalManager(abp.appPath + 'Books/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Books/EditModal');

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
                    title: l('Actions'),
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
                                        return l(
                                            'BookDeletionConfirmationMessage',
                                            data.record.name
                                        );
                                    },
                                    action: function (data) {
                                        acme.bookStore.books.book
                                            .delete(data.record.id)
                                            .then(function() {
                                                abp.notify.info(
                                                    l('SuccessfullyDeleted')
                                                );
                                                dataTable.ajax.reload();
                                            });
                                    }
                                }
                            ]
                    }
                },
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
````

You can run the application and try to delete a book.

{{end}}

{{if UI == "NG"}}

## Creating a new book

In this section, you will learn how to create a new modal dialog form to create a new book.

### Add a modal to BookListComponent

Open `book-list.component.ts` file in `app\book\book-list` folder and replace the content as below:

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

  isModalOpen = false; // <== added this line ==>

  constructor(public readonly list: ListService, private bookService: BookService) {}

  ngOnInit() {
    const bookStreamCreator = (query) => this.bookService.getListByInput(query);

    this.list.hookToQuery(bookStreamCreator).subscribe((response) => {
      this.book = response;
    });
  }

  // added createBook method
  createBook() {
    this.isModalOpen = true;
  }
}
```

* We defined a variable called `isModalOpen` and `createBook` method.
* We added the `createBook` method.


Open `book-list.component.html` file in `books\book-list` folder and replace the content as below:

```html
<div class="card">
  <div class="card-header">
    <div class="row">
      <div class="col col-md-6">
        <h5 class="card-title">{%{{{ '::Menu:Books' | abpLocalization }}}%}</h5>
      </div>
      <!--Added new book button -->
      <div class="text-right col col-md-6">
        <div class="text-lg-right pt-2">
          <button id="create" class="btn btn-primary" type="button" (click)="createBook()">
            <i class="fa fa-plus mr-1"></i>
            <span>{%{{{ "::NewBook" | abpLocalization }}}%}</span>
          </button>
        </div>
      </div>
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

<!--added modal-->
<abp-modal [(visible)]="isModalOpen">
  <ng-template #abpHeader>
    <h3>{%{{{ '::NewBook' | abpLocalization }}}%}</h3>
  </ng-template>

  <ng-template #abpBody> </ng-template>

  <ng-template #abpFooter>
    <button type="button" class="btn btn-secondary" #abpClose>
      {%{{{ 'AbpAccount::Close' | abpLocalization }}}%}
    </button>
  </ng-template>
</abp-modal>
```

* We added the `abp-modal` which renders a modal to allow user to create a new book.
* `abp-modal` is a pre-built component to show modals. While you could use another approach to show a modal, `abp-modal` provides additional benefits.
* We added `New book` button to the `AbpContentToolbar`.

You can open your browser and click **New book** button to see the new modal.

![Empty modal for new book](./images/bookstore-empty-new-book-modal.png)

### Create a reactive form

[Reactive forms](https://angular.io/guide/reactive-forms) provide a model-driven approach to handling form inputs whose values change over time.

Open `book-list.component.ts` file in `app\book\book-list` folder and replace the content as below:

```js
import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { BookDto, BookType } from '../models';
import { BookService } from '../services';
import { FormGroup, FormBuilder, Validators } from '@angular/forms'; // <== added this line ==>

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss'],
  providers: [ListService],
})
export class BookListComponent implements OnInit {
  book = { items: [], totalCount: 0 } as PagedResultDto<BookDto>;

  booksType = BookType;

  isModalOpen = false;

  form: FormGroup; // <== added this line ==>

  constructor(
    public readonly list: ListService,
    private bookService: BookService,
    private fb: FormBuilder // <== injected FormBuilder ==>
  ) {}

  ngOnInit() {
    const bookStreamCreator = (query) => this.bookService.getListByInput(query);

    this.list.hookToQuery(bookStreamCreator).subscribe((response) => {
      this.book = response;
    });
  }

  createBook() {
    this.buildForm(); // <== added this line ==>
    this.isModalOpen = true;
  }

  // added buildForm method
  buildForm() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      type: [null, Validators.required],
      publishDate: [null, Validators.required],
      price: [null, Validators.required],
    });
  }
}
```

* We imported `FormGroup, FormBuilder and Validators`.
* We added `form: FormGroup` variable.
* We injected `fb: FormBuilder` service to the constructor. The [FormBuilder](https://angular.io/api/forms/FormBuilder) service provides convenient methods for generating controls. It reduces the amount of boilerplate needed to build complex forms.
* We added `buildForm` method to the end of the file and executed  `buildForm()` in the `createBook` method. This method creates a reactive form to be able to create a new book.
  * The `group` method of `FormBuilder`, `fb` creates a `FormGroup`.
  * Added `Validators.required` static method which validates the relevant form element.

### Create the DOM elements of the form

Open `book-list.component.html` in `app\books\book-list` folder and replace `<ng-template #abpBody> </ng-template>`  with the following code part:

```html
<ng-template #abpBody>
  <form [formGroup]="form">
    <div class="form-group">
      <label for="book-name">Name</label><span> * </span>
      <input type="text" id="book-name" class="form-control" formControlName="name" autofocus />
    </div>

    <div class="form-group">
      <label for="book-price">Price</label><span> * </span>
      <input type="number" id="book-price" class="form-control" formControlName="price" />
    </div>

    <div class="form-group">
      <label for="book-type">Type</label><span> * </span>
      <select class="form-control" id="book-type" formControlName="type">
        <option [ngValue]="null">Select a book type</option>
        <option [ngValue]="booksType[type]" *ngFor="let type of bookTypeArr"> {%{{{ type }}}%}</option>
      </select>
    </div>

    <div class="form-group">
      <label>Publish date</label><span> * </span>
      <input
        #datepicker="ngbDatepicker"
        class="form-control"
        name="datepicker"
        formControlName="publishDate"
        ngbDatepicker
        (click)="datepicker.toggle()"
      />
    </div>
  </form>
</ng-template>
```

- This template creates a form with `Name`, `Price`, `Type` and `Publish` date fields.
- We've used [NgBootstrap datepicker](https://ng-bootstrap.github.io/#/components/datepicker/overview) in this component.

### Datepicker requirements

Open `book.module.ts` file in `app\book` folder and replace the content as below:

```js
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookRoutingModule } from './book-routing.module';
import { BookListComponent } from './book-list/book-list.component';
import { SharedModule } from '../shared/shared.module';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap'; //<== added this line ==>

@NgModule({
  declarations: [BookListComponent],
  imports: [
    CommonModule,
    BookRoutingModule,
    SharedModule,
    NgbDatepickerModule, //<== added this line ==>
  ],
})
export class BookModule {}
```

* We imported `NgbDatepickerModule`  to be able to use the date picker.

Open `book-list.component.ts` file in `app\book\book-list` folder and replace the content as below:

```js
import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { BookDto, BookType } from '../models';
import { BookService } from '../services';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap'; // <== added this line ==>

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss'],
  providers: [ListService, { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }], // <== added a provide ==>
})
export class BookListComponent implements OnInit {
  book = { items: [], totalCount: 0 } as PagedResultDto<BookDto>;

  booksType = BookType;

  // <== added bookTypeArr array ==>
  bookTypeArr = Object.keys(BookType).filter(
    (bookType) => typeof this.booksType[bookType] === 'number'
  );

  isModalOpen = false;

  form: FormGroup;

  constructor(
    public readonly list: ListService,
    private bookService: BookService,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    const bookStreamCreator = (query) => this.bookService.getListByInput(query);

    this.list.hookToQuery(bookStreamCreator).subscribe((response) => {
      this.book = response;
    });
  }

  createBook() {
    this.buildForm();
    this.isModalOpen = true;
  }

  buildForm() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      type: [null, Validators.required],
      publishDate: [null, Validators.required],
      price: [null, Validators.required],
    });
  }
}
```

* We imported ` NgbDateNativeAdapter, NgbDateAdapter`

* We added a new provider `NgbDateAdapter` that converts Datepicker value to `Date` type. See the [datepicker adapters](https://ng-bootstrap.github.io/#/components/datepicker/overview) for more details.

* We added `bookTypeArr` array to be able to use it in the combobox values. The `bookTypeArr` contains the fields of the `BookType` enum. Resulting array is shown below:

  ```js
  ['Adventure', 'Biography', 'Dystopia', 'Fantastic' ...]
  ```

  This array was used in the previous form template in the `ngFor` loop.

Now, you can open your browser to see the changes:


![New book modal](./images/bookstore-new-book-form.png)

### Saving the book

Open `book-list.component.ts` file in `app\book\book-list` folder and replace the content as below:

```js
import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { BookDto, BookType } from '../models';
import { BookService } from '../services';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss'],
  providers: [ListService, { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }],
})
export class BookListComponent implements OnInit {
  book = { items: [], totalCount: 0 } as PagedResultDto<BookDto>;

  booksType = BookType;

  bookTypeArr = Object.keys(BookType).filter(
    (bookType) => typeof this.booksType[bookType] === 'number'
  );

  isModalOpen = false;

  form: FormGroup;

  constructor(
    public readonly list: ListService,
    private bookService: BookService,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    const bookStreamCreator = (query) => this.bookService.getListByInput(query);

    this.list.hookToQuery(bookStreamCreator).subscribe((response) => {
      this.book = response;
    });
  }

  createBook() {
    this.buildForm();
    this.isModalOpen = true;
  }

  buildForm() {
    this.form = this.fb.group({
      name: ['', Validators.required],
      type: [null, Validators.required],
      publishDate: [null, Validators.required],
      price: [null, Validators.required],
    });
  }

  // <== added save ==>
  save() {
    if (this.form.invalid) {
      return;
    }

    this.bookService.createByInput(this.form.value).subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }
}
```

* We added `save` method

Open `book-list.component.html` in `app\book\book-list` folder, find the `<ng-template #abpFooter>` element and replace this element with the following to create a new book.

```html
<ng-template #abpFooter>
  <button type="button" class="btn btn-secondary" #abpClose>
      {%{{{ 'AbpAccount::Close' | abpLocalization }}}%}
  </button>

  <!--added save button-->
  <button class="btn btn-primary" (click)="save()" [disabled]="form.invalid">
        <i class="fa fa-check mr-1"></i>
        {%{{{ 'AbpAccount::Save' | abpLocalization }}}%}
  </button>
</ng-template>
```

Find the `<form [formGroup]="form">` tag and replace below content:

```html
<form [formGroup]="form" (ngSubmit)="save()"> <!-- added the ngSubmit -->
```


* We added the `(ngSubmit)="save()"` to `<form>` element to save a new book by pressing the enter.
* We added `abp-button` to the bottom area of the modal to save a new book.

The final modal UI looks like below:

![Save button to the modal](./images/bookstore-new-book-form-v2.png)

## Updating a book

Open `book-list.component.ts` in `app\book\book-list` folder and add a variable named `selectedBook`.

```js
import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { BookDto, BookType } from '../models';
import { BookService } from '../services';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss'],
  providers: [ListService, { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }],
})
export class BookListComponent implements OnInit {
  book = { items: [], totalCount: 0 } as PagedResultDto<BookDto>;

  booksType = BookType;

  bookTypeArr = Object.keys(BookType).filter(
    (bookType) => typeof this.booksType[bookType] === 'number'
  );

  isModalOpen = false;

  form: FormGroup;

  selectedBook = {} as BookDto; // <== declared selectedBook ==>

  constructor(
    public readonly list: ListService,
    private bookService: BookService,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    const bookStreamCreator = (query) => this.bookService.getListByInput(query);

    this.list.hookToQuery(bookStreamCreator).subscribe((response) => {
      this.book = response;
    });
  }

  // <== this method is replaced ==>
  createBook() {
    this.selectedBook = {} as BookDto; // <== added ==>
    this.buildForm();
    this.isModalOpen = true;
  }

  // <== added editBook method ==>
  editBook(id: string) {
    this.bookService.getById(id).subscribe((book) => {
      this.selectedBook = book;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  // <== this method is replaced ==>
  buildForm() {
    this.form = this.fb.group({
      name: [this.selectedBook.name || '', Validators.required],
      type: [this.selectedBook.type || null, Validators.required],
      publishDate: [
        this.selectedBook.publishDate ? new Date(this.selectedBook.publishDate) : null,
        Validators.required,
      ],
      price: [this.selectedBook.price || null, Validators.required],
    });
  }

  // <== this method is replaced ==>
  save() {
    if (this.form.invalid) {
      return;
    }

    // <== added request ==>
    const request = this.selectedBook.id
      ? this.bookService.updateByIdAndInput(this.form.value, this.selectedBook.id)
      : this.bookService.createByInput(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }
}
```

* We declared a variable named `selectedBook` as `BookDto`.
* We added `editBook`  method. This method fetches the book with the given `Id` and sets it to `selectedBook` object.
* We replaced the `buildForm` method so that it creates the form with the `selectedBook` data.
* We replaced the `createBook` method so it sets `selectedBook` to an empty object.
* We replaced the `save` method.

### Add "Actions" dropdown to the table

Open the `book-list.component.html` in `app\book\book-list` folder and replace the `<div class="card-body">` tag as below:

```html
<div class="card-body">
  <ngx-datatable [rows]="book.items" [count]="book.totalCount" [list]="list" default>
    <!-- added actions column -->
    <ngx-datatable-column
      [name]="'::Actions' | abpLocalization"
      [maxWidth]="150"
      [sortable]="false"
    >
      <ng-template let-row="row" ngx-datatable-cell-template>
        <div ngbDropdown container="body" class="d-inline-block">
          <button
            class="btn btn-primary btn-sm dropdown-toggle"
            data-toggle="dropdown"
            aria-haspopup="true"
            ngbDropdownToggle
          >
            <i class="fa fa-cog mr-1"></i>{%{{{ '::Actions' | abpLocalization }}}%}
          </button>
          <div ngbDropdownMenu>
            <button ngbDropdownItem (click)="editBook(row.id)">
              {%{{{ '::Edit' | abpLocalization }}}%}
            </button>
          </div>
        </div>
      </ng-template>
    </ngx-datatable-column>
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
```

- We added a `ngx-datatable-column` for the "Actions" column.
- We added `button` with `ngbDropdownToggle` to open actions when clicked the button.
- We have used to [NgbDropdown](https://ng-bootstrap.github.io/#/components/dropdown/examples) for the dropdown menu of actions.

The final UI looks like as below:

![Action buttons](./images/bookstore-actions-buttons.png)

Open `book-list.component.html` in `app\book\book-list` folder and find the `<ng-template #abpHeader>` tag and replace the content as below.

```html
<ng-template #abpHeader>
    <h3>{%{{{ (selectedBook.id ? '::Edit' : '::NewBook' ) | abpLocalization }}}%}</h3>
</ng-template>
```

* This template will show **Edit** text for edit record operation, **New Book** for new record operation in the title.

## Deleting a book

### Delete confirmation popup

Open `book-list.component.ts` in `app\book\book-list` folder and inject the `ConfirmationService`.

Replace the constructor as below:

```js
import { ConfirmationService } from '@abp/ng.theme.shared';
//...

constructor(
  public readonly list: ListService,
  private bookService: BookService,
  private fb: FormBuilder,
  private confirmation: ConfirmationService // <== added this line ==>
) {}
```

* We imported `ConfirmationService`.
* We injected `ConfirmationService` to the constructor.

See the [Confirmation Popup documentation](https://docs.abp.io/en/abp/latest/UI/Angular/Confirmation-Service)

In the `book-list.component.ts` add a delete method:

```js
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared'; //<== imported Confirmation namespace ==>

//...

delete(id: string) {
  this.confirmation.warn('::AreYouSureToDelete', 'AbpAccount::AreYouSure').subscribe((status) => {
    if (status === Confirmation.Status.confirm) {
      this.bookService.deleteById(id).subscribe(() => this.list.get());
    }
  });
}
```


The `delete` method shows a confirmation popup and subscribes for the user response. The `deleteById` method of `BookService` called only if user clicks to the `Yes` button. The confirmation popup looks like below:

![bookstore-confirmation-popup](./images/bookstore-confirmation-popup.png)


### Add a delete button


Open `book-list.component.html` in `app\book\book-list` folder and modify the `ngbDropdownMenu` to add the delete button as shown below:

```html
<div ngbDropdownMenu>
  <!-- added Delete button -->
    <button ngbDropdownItem (click)="delete(row.id)">
        {%{{{ 'AbpAccount::Delete' | abpLocalization }}}%}
    </button>
</div>
```

The final actions dropdown UI looks like below:

![bookstore-final-actions-dropdown](./images/bookstore-final-actions-dropdown.png)

{{end}}

## Next Part

See the [next part](part-3.md) of this tutorial.
