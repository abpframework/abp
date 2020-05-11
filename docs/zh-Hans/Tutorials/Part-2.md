## ASP.NET Core {{UI_Value}} 教程 - 第二章
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

### 关于本教程

这是ASP.NET Core{{UI_Value}}系列教程的第二章. 共有三章:

- [Part-1: 创建项目和书籍列表页面](Part-1.md)
- **Part 2: 创建,编辑,删除书籍(本章)**
- [Part-3: 集成测试](Part-3.md)

> 你也可以观看由ABP社区成员为本教程录制的[视频课程](https://amazingsolutions.teachable.com/p/lets-build-the-booktore-application).

{{if UI == "MVC"}}

### 新增 Book 实体

通过本节, 你将会了解如何创建一个 modal form 来实现新增书籍的功能. 最终成果如下图所示:

![booktore-create-dialog](./images/booktore-create-dialog-2.png)

#### 新建 modal form

在 `Acme.BookStore.Web` 项目的 `Pages/Books` 目录下新建一个 `CreateModal.cshtml` Razor页面:

![booktore-add-create-dialog](./images/booktore-add-create-dialog-v2.png)

##### CreateModal.cshtml.cs

打开 `CreateModal.cshtml.cs` 代码文件,用如下代码替换 `CreateModalModel` 类的实现:

````C#
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
````

* 该类派生于 `BookStorePageModel` 而非默认的 `PageModel`. `BookStorePageModel` 继承了 `PageModel` 并且添加了一些可以被你的page model类使用的通用属性和方法.
*  `Book` 属性上的 `[BindProperty]` 特性将post请求提交上来的数据绑定到该属性上.
* 该类通过构造函数注入了 `IBookAppService` 应用服务,并且在 `OnPostAsync` 处理程序中调用了服务的 `CreateAsync` 方法.

##### CreateModal.cshtml

打开 `CreateModal.cshtml` 文件并粘贴如下代码:

````html
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
````

* 这个 modal 使用 `abp-dynamic-form` Tag Helper 根据 `CreateBookViewModel` 类自动构建了表单.
  * `abp-model` 指定了 `Book` 属性为模型对象.
  * `data-ajaxForm` 设置了表单通过AJAX提交,而不是经典的页面回发.
  * `abp-form-content` tag helper 作为表单控件渲染位置的占位符 (这是可选的,只有你在 `abp-dynamic-form` 中像本示例这样添加了其他内容才需要).

#### 添加 "New book" 按钮

打开 `Pages/Books/Index.cshtml` 并按如下代码修改 `abp-card-header` :

````html
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
````

如下图所示,只是在表格 **右上方** 添加了 **New book** 按钮:

![booktore-new-book-button](./images/booktore-new-book-button.png)

打开 `Pages/book/index.js` 在 `datatable` 配置代码后面添加如下代码:

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

* `abp.ModalManager` 是一个在客户端打开和管理modal的辅助类.它基于Twitter Bootstrap的标准modal组件通过简化的API抽象隐藏了许多细节.

现在,你可以 **运行程序** 通过新的 modal form 来创建书籍了.

### 编辑更新已存在的 Book 实体

在 `Acme.BookStore.Web` 项目的 `Pages/Books` 目录下新建一个名叫 `EditModal.cshtml` 的Razor页面:

![booktore-add-edit-dialog](./images/booktore-add-edit-dialog.png)

#### EditModal.cshtml.cs

展开 `EditModal.cshtml`,打开 `EditModal.cshtml.cs` 文件（ `EditModalModel` 类） 并替换成以下代码:

````csharp
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
````

* `[HiddenInput]` 和 `[BindProperty]` 是标准的 ASP.NET Core MVC 特性.这里启用 `SupportsGet` 从Http请求的查询字符串中获取Id的值.
* 在 `OnGetAsync` 方法中,将 `BookAppService.GetAsync` 方法返回的 `BookDto` 映射成 `CreateUpdateBookDto` 并赋值给Book属性.
* `OnPostAsync` 方法直接使用 `BookAppService.UpdateAsync` 来更新实体.

#### BookDto到CreateUpdateBookDto对象映射

为了执行`BookDto`到`CreateUpdateBookDto`对象映射,请打开`Acme.BookStore.Web`项目中的`BookStoreWebAutoMapperProfile.cs`并更改它,如下所示:

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

* 刚刚添加了`CreateMap<BookDto, CreateUpdateBookDto>();`作为映射定义.

#### EditModal.cshtml

将 `EditModal.cshtml` 页面内容替换成如下代码:

````html
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
````

这个页面内容和 `CreateModal.cshtml` 非常相似,除了以下几点:

* 它包含`id`属性的`abp-input`, 用于存储编辑书的 `id` (它是隐藏的Input)
* 此页面指定的post地址是`Books/EditModal`, 并用文本 *Update* 作为 modal 标题.

#### 为表格添加 "操作（Actions）" 下拉菜单

我们将为表格每行添加下拉按钮 ("Actions") . 最终效果如下:

![booktore-book-table-actions](images/booktore-book-table-actions.png)

打开 `Pages/Books/Index.cshtml` 页面,并按下方所示修改表格部分的代码:

````html
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
````

* 只是为"Actions"增加了一个 `th` 标签.

打开 `Pages/book/index.js` 并用以下内容进行替换:

````js
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
````

* 通过 `abp.localization.getResource('BookStore')` 可以在客户端使用服务器端定义的相同的本地化语言文本.
* 添加了一个名为 `createModal` 的新的 `ModalManager` 来打开创建用的 modal 对话框.
* 添加了一个名为 `editModal` 的新的 `ModalManager` 来打开编辑用的 modal 对话框.
* 在 `columnDefs` 起始处新增一列用于显示 "Actions" 下拉按钮.
* "New Book"动作只需调用`createModal.open`来打开创建对话框.
* "Edit" 操作只是简单调用 `editModal.open` 来打开编辑对话框.

现在,你可以运行程序,通过编辑操作来更新任一个book实体.

### 删除一个已有的Book实体

打开 `Pages/book/index.js` 文件,在 `rowAction` `items` 下新增一项:

````js
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
````

* `confirmMessage` 用来在实际执行 `action` 之前向用户进行确认.
* 通过javascript代理方法 `acme.bookStore.book.delete` 执行一个AJAX请求来删除一个book实体.
* `abp.notify.info` 用来在执行删除操作后显示一个toastr通知信息.

最终的 `index.js` 文件内容如下所示:

````js
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
````

打开`Acme.BookStore.Domain.Shared`项目中的`en.json`并添加以下行:

````json
"BookDeletionConfirmationMessage": "Are you sure to delete the book {0}?",
"SuccessfullyDeleted": "Successfully deleted"
````

运行程序并尝试删除一个book实体.

{{end}}

### 新增 Book 实体

下面的章节中,你将学习到如何创建一个新的模态对话框来新增Book实体.

#### 状态定义

在 `app\book\state` 文件夹下打开 `book.action.ts` 文件,使用以下内容替换它:

```js
import { CreateUpdateBookDto } from '../models'; //<== added this line ==>

export class GetBooks {
  static readonly type = '[Book] Get';
}

// added CreateUpdateBook class
export class CreateUpdateBook {
  static readonly type = '[Book] Create Update Book';
  constructor(public payload: CreateUpdateBookDto) { }
}
```

* 我们导入了 `CreateUpdateBookDto` 模型并且创建了 `CreateUpdateBook` 动作.

打开 `app\book\state` 文件夹下的 `book.state.ts` 文件,使用以下内容替换它:

```js
import { PagedResultDto } from '@abp/ng.core';
import { State, Action, StateContext, Selector } from '@ngxs/store';
import { GetBooks, CreateUpdateBook } from './book.actions'; // <== added CreateUpdateBook==>
import { BookService } from '../services';
import { tap } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { BookDto } from '../models';

export class BookStateModel {
  public book: PagedResultDto<BookDto>;
}

@State<BookStateModel>({
  name: 'BookState',
  defaults: { book: {} } as BookStateModel,
})
@Injectable()
export class BookState {
  @Selector()
  static getBooks(state: BookStateModel) {
    return state.book.items || [];
  }

  constructor(private bookService: BookService) {}

  @Action(GetBooks)
  get(ctx: StateContext<BookStateModel>) {
    return this.bookService.getListByInput().pipe(
      tap((bookResponse) => {
        ctx.patchState({
          book: bookResponse,
        });
      })
    );
  }

  // added CreateUpdateBook action listener
  @Action(CreateUpdateBook)
  save(ctx: StateContext<BookStateModel>, action: CreateUpdateBook) {
    return this.bookService.createByInput(action.payload);
  }
}
```

* 我们导入了 `CreateUpdateBook` 动作并且定义了 `save` 方法监听 `CreateUpdateBook` 动作去创建图书.

当 `SaveBook` 动作被分派时,save方法被执行. 它调用 `BookService` 的 `createByInput` 方法.

#### 添加模态到 BookListComponent

打开 `app\book\book-list` 文件夹内的 `book-list.component.html` 文件,使用以下内容替换它:

```html
<div class="card">
  <div class="card-header">
    <div class="row">
      <div class="col col-md-6">
        <h5 class="card-title">
          {%{{{ '::Menu:Books' | abpLocalization }}}%}
        </h5>
      </div>
       <!--Added new book button -->
      <div class="text-right col col-md-6">
        <div class="text-lg-right pt-2">
          <button
            id="create"
            class="btn btn-primary"
            type="button"
            (click)="createBook()"
          >
            <i class="fa fa-plus mr-1"></i>
            <span>{%{{{ "::NewBook" | abpLocalization }}}%}</span>
          </button>
        </div>
      </div>
    </div>
  </div>
  <div class="card-body">
    <abp-table
      [value]="book$ | async"
      [abpLoading]="loading"
      [headerTemplate]="tableHeader"
      [bodyTemplate]="tableBody"
      [rows]="10"
      [scrollable]="true"
    >
    </abp-table>
    <ng-template #tableHeader>
      <tr>
        <th>{%{{{ "::Name" | abpLocalization }}}%}</th>
        <th>{%{{{ "::Type" | abpLocalization }}}%}</th>
        <th>{%{{{ "::PublishDate" | abpLocalization }}}%}</th>
        <th>{%{{{ "::Price" | abpLocalization }}}%}</th>
      </tr>
    </ng-template>
    <ng-template #tableBody let-data>
      <tr>
        <td>{%{{{ data.name }}}%}</td>
        <td>{%{{{ bookType[data.type] }}}%}</td>
        <td>{%{{{ data.publishDate | date }}}%}</td>
        <td>{%{{{ data.price }}}%}</td>
      </tr>
    </ng-template>
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

* 我们添加了 `abp-modal` 渲染模态框,允许用户创建新书.
* `abp-modal` 是显示模态框的预构建组件. 你也可以使用其它方法显示模态框,但 `abp-modal` 提供了一些附加的好处.
* 我们添加了 `New book` 按钮到 `AbpContentToolbar`.

打开 `app\book\book-list` 文件夹下的 `book-list.component.ts` 文件,使用以下内容替换它:

```js
import { Component, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { BookDto, BookType } from '../models';
import { GetBooks } from '../state/book.actions';
import { BookState } from '../state/book.state';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss'],
})
export class BookListComponent implements OnInit {
  @Select(BookState.getBooks)
  books$: Observable<BookDto[]>;

  booksType = BookType;

  loading = false;

  isModalOpen = false; // <== added this line ==>

  constructor(private store: Store) {}

  ngOnInit() {
    this.get();
  }

  get() {
    this.loading = true;
    this.store
      .dispatch(new GetBooks())
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(() => {});
  }

  // added createBook method
  createBook() {
    this.isModalOpen = true;
  }
}
```

* 我们添加了 `isModalOpen = false` 和 `createBook` 方法.

你可以打开浏览器,点击**New book**按钮看到模态框.

![Empty modal for new book](./images/booktore-empty-new-book-modal.png)

#### 添加响应式表单

[响应式表单](https://angular.io/guide/reactive-forms) 提供一种模型驱动的方法来处理其值随时间变化的表单输入.

打开 `app\app\book\book-list` 文件夹下的 `book-list.component.ts` 文件,使用以下内容替换它:

```js
import { Component, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { BookDto, BookType } from '../models';
import { GetBooks } from '../state/book.actions';
import { BookState } from '../state/book.state';
import { FormGroup, FormBuilder, Validators } from '@angular/forms'; // <== added this line ==>

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss'],
})
export class BookListComponent implements OnInit {
  @Select(BookState.getBooks)
  books$: Observable<BookDto[]>;

  booksType = BookType;

  loading = false;

  isModalOpen = false;

  form: FormGroup; // <== added this line ==>

  constructor(private store: Store, private fb: FormBuilder) {} // <== added FormBuilder ==>

  ngOnInit() {
    this.get();
  }

  get() {
    this.loading = true;
    this.store
      .dispatch(new GetBooks())
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(() => {});
  }

  createBook() {
    this.buildForm(); //<== added this line ==>
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

* 我们导入了 `FormGroup, FormBuilder and Validators`.
* 我们添加了 `form: FormGroup` 变量.
* 我们注入了 `fb: FormBuilder` 服务到构造函数. [FormBuilder](https://angular.io/api/forms/FormBuilder) 服务为生成控件提供了方便的方法. 它减少了构建复杂表单所需的样板文件的数量.
* 我们添加了 `buildForm` 方法到文件末尾, 在 `createBook` 方法调用 `buildForm()` 方法. 该方法创建一个响应式表单去创建新书.
  * `FormBuilder` 内的 `fb.group` 方法创建一个 `FormGroup`.
  * 添加了 `Validators.required` 静态方法用于验证表单元素.

#### 创建表单的DOM元素

打开 `app\app\book\book-list` 文件夹下的 `book-list.component.html` 文件,使用以下内容替换 `<ng-template #abpBody> </ng-template>`:

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
        <option [ngValue]="bookType[type]" *ngFor="let type of bookTypeArr"> {%{{{ type }}}%}</option>
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

- 模板创建了带有 `Name`, `Price`, `Type` 和 `Publish` 时间字段的表单.
- 我们在组件中使用了 [NgBootstrap datepicker](https://ng-bootstrap.github.io/#/components/datepicker/overview).

#### Datepicker 要求

打开 `app\book` 文件夹下的  `book.module.ts` 文件,使用以下内容替换它:

```js
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BooksRoutingModule } from './book-routing.module';
import { BooksComponent } from './book.component';
import { BookListComponent } from './book-list/book-list.component';
import { SharedModule } from '../shared/shared.module';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap'; //<== added this line ==>

@NgModule({
  declarations: [BooksComponent, BookListComponent],
  imports: [
    CommonModule,
    BooksRoutingModule,
    SharedModule,
    NgbDatepickerModule //<== added this line ==>
  ]
})
export class BooksModule { }
```

* 我们导入了 `NgbDatepickerModule`  来使用日期选择器.

打开 `app\app\book\book-list` 文件夹下的 `book-list.component.ts` 文件,使用以下内容替换它:

```js
import { Component, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { BookDto, BookType } from '../models';
import { GetBooks } from '../state/book.actions';
import { BookState } from '../state/book.state';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap'; // <== added this line ==>

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }], // <== added this line ==>
})
export class BookListComponent implements OnInit {
  @Select(BookState.getBooks)
  books$: Observable<BookDto[]>;

  booksType = BookType;

  //added bookTypeArr array
  bookTypeArr = Object.keys(BookType).filter(
    (bookType) => typeof this.booksType[bookType] === 'number'
  );

  loading = false;

  isModalOpen = false;

  form: FormGroup;

  constructor(private store: Store, private fb: FormBuilder) {}

  ngOnInit() {
    this.get();
  }

  get() {
    this.loading = true;
    this.store
      .dispatch(new GetBooks())
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(() => {});
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

* 我们导入了 ` NgbDateNativeAdapter, NgbDateAdapter` 

* 我们添加了一个新的 `NgbDateAdapter` 提供程序,它将Datepicker值转换为Date类型. 有关更多详细信息,请参见[datepicker adapters](https://ng-bootstrap.github.io/#/components/datepicker/overview).

* 我们添加了 `bookTypeArr` 数组,以便能够在combobox值中使用它. `bookTypeArr` 包含 `BookType` 枚举的字段. 得到的数组如下所示:

  ```js
  ['Adventure', 'Biography', 'Dystopia', 'Fantastic' ...]
  ```

  在先前的表单模板中 用 `ngFor` 使用这个数组.

现在你可以打开浏览器看到以下变化:

![New book modal](./images/booktore-new-book-form.png)

#### 保存图书

打开 `app\app\book\book-list` 文件夹下的 `book-list.component.ts` 文件,使用以下内容替换它:

```js
import { Component, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { BookDto, BookType } from '../models';
import { GetBooks, CreateUpdateBook } from '../state/book.actions'; // <== added CreateUpdateBook ==>
import { BookState } from '../state/book.state';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }],
})
export class BookListComponent implements OnInit {
  @Select(BookState.getBooks)
  books$: Observable<BookDto[]>;

  booksType = BookType;

  bookTypeArr = Object.keys(BookType).filter(
    (bookType) => typeof this.booksType[bookType] === 'number'
  );

  loading = false;

  isModalOpen = false;

  form: FormGroup;

  constructor(private store: Store, private fb: FormBuilder) {}

  ngOnInit() {
    this.get();
  }

  get() {
    this.loading = true;
    this.store
      .dispatch(new GetBooks())
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(() => {});
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

    this.store.dispatch(new CreateUpdateBook(this.form.value)).subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.get();
    });
  }
}
```

* 我们导入了 `CreateUpdateBook`.
* 我们添加了 `save` 方法.

打开 `app\app\book\book-list` 文件夹下的 `app\app\book\book-list`文件, 添加 `abp-button` 保存图书.

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

使用以下内容替换 `<form [formGroup]="form">` 标签:

```html
<form [formGroup]="form" (ngSubmit)="save()"> <!-- added the ngSubmit -->
```

* 我们添加了 `(ngSubmit)="save()"` 到 `<form>` 元素,当按下enter时保存图书.
* 我们在模态框的底部添加了 `abp-button` 来保存图书.

模态框最终看起来像这样:

![Save button to the modal](./images/booktore-new-book-form-v2.png)

### 更新图书

#### CreateUpdateBook 动作

打开 `app\book\state` 文件夹下的 `book.actions.ts` 文件,使用以下内容替换它:

```js
import { CreateUpdateBookDto } from '../models';

export class GetBooks {
  static readonly type = '[Book] Get';
}

export class CreateUpdateBook {
  static readonly type = '[Book] Create Update Book';
  constructor(public payload: CreateUpdateBookDto, public id?: string) {} // <== added id parameter ==>
}
```

* 我们在 `CreateUpdateBook` 动作的构造函数添加了 `id` 参数.

打开 `app\book\state` 文件夹下的 `book.state.ts` 文件,使用以下内容替换 `save` 方法:

```js
@Action(CreateUpdateBook)
save(ctx: StateContext<BookStateModel>, action: CreateUpdateBook) {
  if (action.id) {
    return this.bookService.updateByIdAndInput(action.payload, action.id);
  } else {
    return this.bookService.createByInput(action.payload);
  }
}
```

#### BookListComponent

打开 `app\app\book\book-list` 文件夹下的 `book-list.component.ts` 文件,在构造函数注入 `BookService` 服务,并添加 名为 `selectedBook` 的变量.

```js
import { Component, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { BookDto, BookType } from '../models';
import { GetBooks, CreateUpdateBook } from '../state/book.actions';
import { BookState } from '../state/book.state';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { BookService } from '../services'; // <== imported BookService ==>

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }],
})
export class BookListComponent implements OnInit {
  @Select(BookState.getBooks)
  book$: Observable<BookDto[]>;

  bookType = BookType;

  bookTypeArr = Object.keys(BookType).filter(
    (bookType) => typeof this.bookType[bookType] === 'number'
  );

  loading = false;

  isModalOpen = false;

  form: FormGroup;

  selectedBook = {} as BookDto; // <== declared selectedBook ==>

  constructor(private store: Store, private fb: FormBuilder, private bookService: BookService) {} //<== injected BookService ==>

  ngOnInit() {
    this.get();
  }

  get() {
    this.loading = true;
    this.store
      .dispatch(new GetBooks())
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(() => {});
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

  save() {
    if (this.form.invalid) {
      return;
    }

    //<== added this.selectedBook.id ==>
    this.store
      .dispatch(new CreateUpdateBook(this.form.value, this.selectedBook.id))
      .subscribe(() => {
        this.isModalOpen = false;
        this.form.reset();
        this.get();
      });
  }
}
```

* 我们导入了 `BookService`.
* 我们声明了类型为 `BookDto` 的 `selectedBook` 变量.
* 我们在构造函数注入了  `BookService`, 它用于检索正在编辑的图书数据.
* 我们添加了 `editBook`  方法, 根据给定图书 `Id` 设置 `selectedBook` 对象.
* 我们替换了 `buildForm` 方法使用 `selectedBook` 数据创建表单.
* 我们替换了 `createBook` 方法,设置 `selectedBook` 为空对象.
* 我们在 `CreateUpdateBook` 构造函数添加了 `selectedBook.id`.

#### 添加 "Actions" 下拉框到表格

打开 `app\app\book\book-list` 文件夹下的 `book-list.component.html` 文件,使用以下内容替换 `<div class="card-body">` 标签:

```html
<div class="card-body">
  <abp-table
    [value]="book$ | async"
    [abpLoading]="loading"
    [headerTemplate]="tableHeader"
    [bodyTemplate]="tableBody"
    [rows]="10"
    [scrollable]="true"
  >
  </abp-table>
  <ng-template #tableHeader>
    <tr>
      <th>{%{{{ "::Actions" | abpLocalization }}}%}</th>
      <th>{%{{{ "::Name" | abpLocalization }}}%}</th>
      <th>{%{{{ "::Type" | abpLocalization }}}%}</th>
      <th>{%{{{ "::PublishDate" | abpLocalization }}}%}</th>
      <th>{%{{{ "::Price" | abpLocalization }}}%}</th>
    </tr>
  </ng-template>
  <ng-template #tableBody let-data>
    <tr>
      <td>
        <div ngbDropdown container="body" class="d-inline-block">
          <button
            class="btn btn-primary btn-sm dropdown-toggle"
            data-toggle="dropdown"
            aria-haspopup="true"
            ngbDropdownToggle
          >
            <i class="fa fa-cog mr-1"></i>{%{{{ "::Actions" | abpLocalization }}}%}
          </button>
          <div ngbDropdownMenu>
            <button ngbDropdownItem (click)="editBook(data.id)">
              {%{{{ "::Edit" | abpLocalization }}}%}
            </button>
          </div>
        </div>
      </td>
      <td>{%{{{ data.name }}}%}</td>
      <td>{%{{{ bookType[data.type] }}}%}</td>
      <td>{%{{{ data.publishDate | date }}}%}</td>
      <td>{%{{{ data.price }}}%}</td>
    </tr>
  </ng-template>
</div>
```

- 我们添加了 "Actions" 栏的 `th`.
- 我们添加了带有 `ngbDropdownToggle` 的 `button`,在点击按钮时打开操作.
- 我们习惯于将[NgbDropdown](https://ng-bootstrap.github.io/#/components/dropdown/examples)用于操作的下拉菜单.

UI最终看起来像这样:

![Action buttons](./images/booktore-actions-buttons.png)

打开 `app\app\book\book-list` 文件夹下的 `book-list.component.html` 文件,使用以下内容替换 `<ng-template #abpHeader>` 标签:

```html
<ng-template #abpHeader>
    <h3>{%{{{ (selectedBook.id ? 'AbpIdentity::Edit' : '::NewBook' ) | abpLocalization }}}%}</h3>
</ng-template>
```

* **Edit** 文本做为编辑记录操作的标题, **New Book** 做为添加记录操作的标题.

### 删除图书

#### DeleteBook 动作

打开 `app\book\state` 文件夹下的 `book.actions.ts` 文件添加名为 `DeleteBook` 的动作.

```js
export class DeleteBook {
  static readonly type = '[Book] Delete';
  constructor(public id: string) {}
}
```

打开 `app\book\state` 文件夹下的 `book.state.ts` 文件,使用以下内容替换它:

```js
import { PagedResultDto } from '@abp/ng.core';
import { State, Action, StateContext, Selector } from '@ngxs/store';
import { GetBooks, CreateUpdateBook, DeleteBook } from './book.actions'; // <== added DeleteBook==>
import { BookService } from '../services';
import { tap } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { BookDto } from '../models';

export class BookStateModel {
  public book: PagedResultDto<BookDto>;
}

@State<BookStateModel>({
  name: 'BookState',
  defaults: { book: {} } as BookStateModel,
})
@Injectable()
export class BookState {
  @Selector()
  static getBooks(state: BookStateModel) {
    return state.book.items || [];
  }

  constructor(private bookService: BookService) {}

  @Action(GetBooks)
  get(ctx: StateContext<BookStateModel>) {
    return this.bookService.getListByInput().pipe(
      tap((booksResponse) => {
        ctx.patchState({
          book: booksResponse,
        });
      })
    );
  }

  @Action(CreateUpdateBook)
  save(ctx: StateContext<BookStateModel>, action: CreateUpdateBook) {
    if (action.id) {
      return this.bookService.updateByIdAndInput(action.payload, action.id);
    } else {
      return this.bookService.createByInput(action.payload);
    }
  }

  // <== added DeleteBook action listener ==>
  @Action(DeleteBook)
  delete(ctx: StateContext<BookStateModel>, action: DeleteBook) {
    return this.bookService.deleteById(action.id);
  }
}
```

- 我们导入了 `DeleteBook` .

- 我们在文件末尾添加了 `DeleteBook` 动作监听器.

#### 删除确认弹层

打开 `app\app\book\book-list` 文件夹下的 `book-list.component.ts` 文件,注入 `ConfirmationService`.

替换构造函数:

```js
import { ConfirmationService } from '@abp/ng.theme.shared';
//...

constructor(
    private store: Store, 
    private fb: FormBuilder,
    private bookService: BookService,
    private confirmation: ConfirmationService // <== added this line ==>
) { }
```

* 我们导入了 `ConfirmationService`.
* 我们在构造函数注入了 `ConfirmationService` .

参阅[确认弹层文档](https://docs.abp.io/en/abp/latest/UI/Angular/Confirmation-Service)了解更多

在 `book-list.component.ts` 中添加删除方法:

```js
import { GetBooks, CreateUpdateBook, DeleteBook } from '../state/book.actions' ;// <== imported DeleteBook ==>

import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared'; //<== imported Confirmation ==>

//...

delete(id: string) {
    this.confirmation
        .warn('::AreYouSureToDelete', 'AbpAccount::AreYouSure')
        .subscribe(status => {
        if (status === Confirmation.Status.confirm) {
            this.store.dispatch(new DeleteBook(id)).subscribe(() => this.get());
        }
    });
}
```

`delete` 方法会显示一个确认弹层并订阅用户响应. 只在用户点击 `Yes` 按钮时分派动作. 确认弹层看起来如下:

![booktore-confirmation-popup](./images/booktore-confirmation-popup.png)

#### 添加删除按钮

打开 `app\app\book\book-list` 文件夹下的 `app\app\book\book-list` 文件,修改 `ngbDropdownMenu` 添加删除按钮:

```html
<div ngbDropdownMenu>
  <!-- added Delete button -->
    <button ngbDropdownItem (click)="delete(data.id)">
        {%{{{ 'AbpAccount::Delete' | abpLocalization }}}%}
    </button>
</div>
```

最终操作下拉框UI看起来如下:

![booktore-final-actions-dropdown](./images/booktore-final-actions-dropdown.png)

{{end}}

### 下一章

查看本教程的 [下一章](Part-3.md) .
