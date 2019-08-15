## Angular Tutorial - Part II

### About this Tutorial

This is the second part of the Angular tutorial series. See all parts:

- [Part I: Create the project and a book list page](Part-I.md)
- **Part II: Create, Update and Delete books (this tutorial)**

You can access to the **source code** of the application from the [GitHub repository](https://github.com/abpframework/abp/tree/dev/samples/BookStore-Angular-MongoDb).

### Creating a New Book

In this section, you will learn how to create a new modal dialog form to create a new book.

#### Type Definition

Create an interface, named `CreateUpdateBookInput` in the `books.ts` as shown below:

```typescript
export namespace Books {
  //...
  export interface CreateUpdateBookInput {
    name: string;
    type: BookType;
    publishDate: string;
    price: number;
  }
}
```

`CreateUpdateBookInput` interface matches the `CreateUpdateBookDto` in the backend.

#### Service Method

Open the `books.service.ts` and add a new method, named `create` to perform an HTTP POST request to the server:

```typescript
create(body: Books.CreateUpdateBookInput): Observable<Books.Book> {
  const request: Rest.Request<Books.CreateUpdateBookInput> = {
    method: 'POST',
    url: '/api/app/book',
    body,
  };

  return this.rest.request<Books.CreateUpdateBookInput, Books.Book>(request);
}
```

#### State Definitions

Add the `CreateUpdateBook` action to `books.actions.ts` as shown below:

```typescript
import { Books } from '../models';

export class CreateUpdateBook {
  static readonly type = '[Books] Create Update Book';
  constructor(public payload: Books.CreateUpdateBookInput) {}
}
```

Open `books.state.ts` and define the `save` method that will listen to a `CreateUpdateBook` action to create a book:

```typescript
import { GetBooks, CreateUpdateBook } from '../actions/books.actions';
import { tap, switchMap } from 'rxjs/operators';
//...
@Action(CreateUpdateBook)
save({ dispatch }: StateContext<Books.State>, { payload }: CreateUpdateBook) {
  return this.booksService
      .create(payload)
      .pipe(switchMap(() => dispatch(new GetBooks())));
}
```

When the `SaveBook` action dispatched, the save method is executed. It call `create` method of the `BooksService` defined before. After the service call, `BooksState` dispatches the `GetBooks` action to get books again from the server to refresh the page.

#### Add a Modal to BookListComponent

Open the `book-list.component.html` and add the `abp-modal` to show/hide the book form.

```html
<abp-modal [(visible)]="isModalOpen">
  <ng-template #abpHeader>
    <h3>New Book</h3>
  </ng-template>

  <ng-template #abpBody> </ng-template>

  <ng-template #abpFooter>
    <button type="button" class="btn btn-secondary" #abpClose>
      Cancel
    </button>
  </ng-template>
</abp-modal>
```

`abp-modal` is a pre-built component to show modals. While you could use another approach to show a modal, `abp-modal` provides additional benefits.

Add a button, labeled `New book` to show the modal.

```html
<div class="row">
  <div class="col col-md-6">
    <h5 class="card-title">
      Books
    </h5>
  </div>
  <div class="text-right col col-md-6">
    <button id="create-role" class="btn btn-primary" type="button" (click)="onAdd()">
      <i class="fa fa-plus mr-1"></i> <span>New book</span>
    </button>
  </div>
</div>
```

Open the `book-list.component.ts` and add `isModalOpen` variable and `onAdd` method to show/hide the modal.

```typescript
isModalOpen = false;

//...

onAdd() {
  this.isModalOpen = true;
}
```

![empty-modal](images/bookstore-empty-new-book-modal.png)

#### Create a Reactive Form

> [Reactive forms](https://angular.io/guide/reactive-forms) provide a model-driven approach to handling form inputs whose values change over time.

Add a `form` variable and inject a `FormBuilder` service to the `book-list.component.ts` as shown below (remember add the import statement).

```typescript
import { FormGroup, FormBuilder } from '@angular/forms';

form: FormGroup;

constructor(
  //...
  private fb: FormBuilder
) {}
```

> The [FormBuilder](https://angular.io/api/forms/FormBuilder) service provides convenient methods for generating controls. It reduces the amount of boilerplate needed to build complex forms.

Add the `buildForm` method to create book form.

```typescript
buildForm() {
  this.form = this.fb.group({
    name: ['', Validators.required],
    type: [null, Validators.required],
    publishDate: [null, Validators.required],
    price: [null, Validators.required],
  });
}
```

- The `group` method of `FormBuilder` creates a `FormGroup`.
- Added `Validators.required` static method that validation of form element.

Modify the `onAdd` method as shown below:

```typescript
onAdd() {
  this.buildForm();
  this.isModalOpen = true;
}
```

### Create the DOM Elements of the Form

Open `book-list.component.html` and add the form in the body template of the modal.

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
        <option [ngValue]="booksType[type]" *ngFor="let type of bookTypeArr"> {{ type }}</option>
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

TODO: Add a short description.

> We've used [NgBootstrap datepicker](https://ng-bootstrap.github.io/#/components/datepicker/overview) in this component.

Open the `book-list.component.ts` and then add the `bookTypes`.

```typescript
//...
form: FormGroup;

bookTypeArr = Object.keys(Books.BookType).filter(bookType => typeof this.booksType[bookType] === 'number');
```

The `bookTypes` variable added to generate array from `BookType` enum. The `bookTypes` equals like this:

```js
['Adventure', 'Biography', 'Dystopia', 'Fantastic' ...]
```

### Add the Datepicker Requirements

Import `NgbDatepickerModule` to the `books.module.ts`.

```typescript
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  imports: [
    // ...
    NgbDatepickerModule,
  ],
})
export class BooksModule {}
```

Open the `book-list.component.html` and then add `providers` as shown below:

```typescript
@Component({
  // ...
  styleUrls: ['./book-list.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }],
})
export class BookListComponent implements OnInit {
// ...
```

> The `NgbDateAdapter` convert Datepicker value type to `Date` type. See the [datepicker adapters](https://ng-bootstrap.github.io/#/components/datepicker/overview) for more details.

![new-book-form](images/bookstore-new-book-form.png)

### Create a New Book

Add the `save` method to `BookListComponent`

```typescript
save() {
  if (this.form.invalid) {
    return;
  }

  this.store.dispatch(new CreateUpdateBook(this.form.value)).subscribe(() => {
    this.isModalOpen = false;
    this.form.reset();
  });
}
```

TODO: description ??

Then, open the `book-list.component.html` and add the `abp-button` for the run `save` method.

```html
<ng-template #abpFooter>
  <button type="button" class="btn btn-secondary" #abpClose>
    Cancel
  </button>
  <abp-button iconClass="fa fa-check" (click)="save()">Save</abp-button>
</ng-template>
```

Now, You can add a new book.

![bookstore-new-book-form-v2](images/bookstore-new-book-form-v2.png)

### Add HTTP GET and PUT Methods

TODO: Description

Open the `books.service.ts` and then add the `getById` and `update` methods.

```typescript
getById(id: string): Observable<Books.Book> {
  const request: Rest.Request<null> = {
    method: 'GET',
    url: `/api/app/book/${id}`,
  };

  return this.rest.request<null, Books.Book>(request);
}

update(body: Books.CreateUpdateBookInput, id: string): Observable<Books.Book> {
  const request: Rest.Request<Books.CreateUpdateBookInput> = {
    method: 'PUT',
    url: `/api/app/book/${id}`,
    body,
  };

  return this.rest.request<Books.CreateUpdateBookInput, Books.Book>(request);
}
```

- Added the `getById` method to get the editing book by performing an HTTP request to the related endpoint.
- Added the `update` method to update a book with the `id` by performing an HTTP request to the related endpoint.

TODO: header ??

Open the `books.actins.ts` and add `id` parameter.

```typescript
export class CreateUpdateBook {
  static readonly type = '[Books] Create Update Book';
  constructor(public payload: Books.CreateUpdateBookInput, public id?: string) {}
}
```

Added `id` parameter to reuse the `BooksSave` while updating and creating a book.

Open `books.state.ts` and then modify the `save` method as show below:

```typescript
@Action(CreateUpdateBook)
save({ dispatch }: StateContext<Books.State>, { payload, id }: CreateUpdateBook) {
  let request;

  if (id) {
    request = this.booksService.update(payload, id);
  } else {
    request = this.booksService.create(payload);
  }

  return request.pipe(switchMap(() => dispatch(new GetBooks())));
}
```

### Update a Book

Inject `BooksService` dependency by adding it to the `book-list.component.ts` constructor and add variable named `selectedBook`.

```typescript
import { BooksService } from '../shared/books.service';
//...
selectedBook = {} as Books.Book;

constructor(
  //...
  private booksService: BooksService
)
```

- Added `booksService` to get the detail of selected book by `id` before creating the form
- Added `selectedBook` variable to reuse detail of selected book.

Modify the `buildForm` method to reuse the same form while editing a book.

```typescript
buildForm() {
  this.form = this.fb.group({
    name: [this.selectedBook.name || '', Validators.required],
    type: this.selectedBook.type || null,
    publishDate: this.selectedBook.publishDate ? new Date(this.selectedBook.publishDate) : null,
    price: this.selectedBook.price || null,
  });
}
```

Add the `onEdit` method as shown below:

```typescript
  onEdit(id: string) {
    this.booksService.getById(id).subscribe(book => {
      this.selectedBook = book;
      this.buildForm();
      this.isModalOpen = true;
    });
  }
```

- Added `onEdit` method to get selected book detail, build form and then show the modal.

Add the `selectedBook` definition to `onAdd` method for reuse same form while adding a new book.

```typescript
  onAdd() {
    this.selectedBook = {} as Books.Book;
    //...
  }
```

Modify the `save` method as shown below:

```typescript
save() {
  if (this.form.invalid) {
    return;
  }

  this.store.dispatch(new CreateUpdateBook(this.form.value, this.selectedBook.id))
    .subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
    });
}
```

Added the `this.selectedBook.id` property to reuse the `CreateUpdateBook` action.

#### Add "Actions" Dropdown to the Table

Open the `book-list.component.html` and add modify the `p-table` as shown below:

```html
<p-table [value]="books$ | async" [loading]="loading" [paginator]="true" [rows]="10">
  <ng-template pTemplate="header">
    <tr>
      <th>Actions</th>
      <th>Book name</th>
      <th>Book type</th>
      <th>Publish date</th>
      <th>Price</th>
    </tr>
  </ng-template>
  <ng-template pTemplate="body" let-data>
    <tr>
      <td>
        <div ngbDropdown class="d-inline-block">
          <button
            class="btn btn-primary btn-sm dropdown-toggle"
            data-toggle="dropdown"
            aria-haspopup="true"
            ngbDropdownToggle
          >
            <i class="fa fa-cog mr-1"></i>Actions
          </button>
          <div ngbDropdownMenu>
            <button ngbDropdownItem (click)="onEdit(data.id)">Edit</button>
          </div>
        </div>
      </td>
      <td>{{ data.name }}</td>
      <td>{{ booksType[data.type] }}</td>
      <td>{{ data.publishDate | date }}</td>
      <td>{{ data.price }}</td>
    </tr>
  </ng-template>
</p-table>
```

Actions button added to each row of the table.

> We've used to [NgbDropdown](https://ng-bootstrap.github.io/#/components/dropdown/examples) for the dropdown menu of actions.

The Actions buttons looks like this:

![actions-buttons](images/bookstore-actions-buttons.png)

Update the modal header for reuse the same modal.

```html
<ng-template #abpHeader>
  <h3>{{ selectedBook.id ? 'Edit' : 'New Book' }}</h3>
</ng-template>
```

![actions-buttons](images/bookstore-edit-modal.png)

TODO: description & screenshot ??

### Delete a Book

Open `books.service.ts` and the the `delete` method for delete a book with the `id` by performing an HTTP request to the related endpoint.

```typescript
delete(id: string): Observable<null> {
  const request: Rest.Request<null> = {
    method: 'DELETE',
    url: `/api/app/book/${id}`,
  };

  return this.rest.request<null, null>(request);
}
```

### State Definitions

Add an action named `BooksDelete` to `books.actions.ts`

```typescript
export class DeleteBook {
  static readonly type = '[Books] Delete';
  constructor(public id: string) {}
}
```

Then, open the `books.state.ts` and add the `delete` method that will listen to a `CreateUpdateBook` action to create a book

```typescript
import { ... , DeleteBook } from '../actions/books.actions';
//...
@Action(DeleteBook)
delete({ dispatch }: StateContext<Books.State>, { id }: DeleteBook) {
  return this.booksService.delete(id).pipe(switchMap(() => dispatch(new GetBooks())));
}
```

TODO: description??

### Add a Delete Button

Open `book-list.component.html` and modify the `ngbDropdownMenu` for add the delete button as shown below:

```html
<div ngbDropdownMenu>
  ...
  <button ngbDropdownItem (click)="delete(data.id, data.name)">
    Delete
  </button>
</div>
```

The final actions dropdown UI looks like this:

![bookstore-final-actions-dropdown](images/bookstore-final-actions-dropdown.png)

### Open Confirmation Popup

Open `book-list.component.ts` and inject the `ConfirmationService` for show confirmation popup.

```typescript
import { ConfirmationService } from '@abp/ng.theme.shared';
//...
constructor(
	//...
  private confirmationService: ConfirmationService
)
```

Add the following method to `BookListComponent`.

```typescript
import { ... , DeleteBook } from '../../store/actions';
import { ... , Toaster } from '@abp/ng.theme.shared';
//...
delete(id: string, name: string) {
  this.confirmationService
    .error(`${name} will be deleted. Do you confirm that?`, 'Are you sure?')
    .subscribe(status => {
      if (status === Toaster.Status.confirm) {
        this.store.dispatch(new DeleteBook(id));
      }
    });
}
```

The `delete` method shows confirmation popup and listens to them. When close the popup, the subscribe block runs. If confirmed this popup, it will dispatch the `DeleteBook` action.

The confirmation popup looks like this:

![bookstore-confirmation-popup](images/bookstore-confirmation-popup.png)

TODO: End
