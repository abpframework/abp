# Querying Lists Easily with ListService

`ListService` is a utility service to provide an easy pagination, sorting, and search implementation.



## Getting Started

`ListService` is **not provided in root**. The reason is, this way, it will clear any subscriptions on component destroy. You may use the optional `LIST_QUERY_DEBOUNCE_TIME` token to adjust the debounce behavior.

```js
import { ListService } from '@abp/ng.core';
import { BookDto } from '../models';
import { BookService } from '../services';

@Component({
  /* class metadata here */
  providers: [
    // [Required]
    ListService,

    // [Optional]
    // Provide this token if you want a different debounce time.
    // Default is 300. Cannot be 0. Any value below 100 is not recommended.
    { provide: LIST_QUERY_DEBOUNCE_TIME, useValue: 500 },
  ],
  template: `
    
  `,
})
class BookComponent {
  items: BookDto[] = [];
  count = 0;

  constructor(
    public readonly list: ListService,
    private bookService: BookService,
  ) {}

  ngOnInit() {
    // A function that gets query and returns an observable
    const bookStreamCreator = query => this.bookService.getList(query);

    this.list.hookToQuery(bookStreamCreator).subscribe(
      response => {
        this.items = response.items;
        this.count = response.count;
        // If you use OnPush change detection strategy,
        // call detectChanges method of ChangeDetectorRef here.
      }
    ); // Subscription is auto-cleared on destroy.
  }
}
```

> Noticed `list` is `public` and `readonly`? That is because we will use `ListService` properties directly in the component's template. That may be considered as an anti-pattern, but it is much quicker to implement. You can always use public component properties instead.

Place `ListService` properties into the template like this:

```html
<abp-table
  [value]="book.items"
  [(page)]="list.page"
  [rows]="list.maxResultCount"
  [totalRecords]="book.totalCount"
  [headerTemplate]="tableHeader"
  [bodyTemplate]="tableBody"
  [abpLoading]="list.isLoading$ | async"
>
</abp-table>

<ng-template #tableHeader>
  <tr>
    <th (click)="nameSort.sort('name')">
      {%{{{ '::Name' | abpLocalization }}}%}
      <abp-sort-order-icon
        #nameSort
        sortKey="name"
        [(selectedSortKey)]="list.sortKey"
        [(order)]="list.sortOrder"
      ></abp-sort-order-icon>
    </th>
  </tr>
</ng-template>

<ng-template #tableBody let-data>
  <tr>
    <td>{%{{{ data.name }}}%}</td>
  </tr>
</ng-template>
```

## Usage with Observables

You may use observables in combination with [AsyncPipe](https://angular.io/guide/observables-in-angular#async-pipe) of Angular instead. Here are some possibilities:

```ts
  book$ = this.list.hookToQuery(query => this.bookService.getListByInput(query));
```

```html
<!-- simplified representation of the template -->

<abp-table
  [value]="(book$ | async)?.items || []"
  [totalRecords]="(book$ | async)?.totalCount"
>
</abp-table>

<!-- DO NOT WORRY, ONLY ONE REQUEST WILL BE MADE -->
```

...or...


```ts
  @Select(BookState.getBooks)
  books$: Observable<BookDto[]>;

  @Select(BookState.getBookCount)
  bookCount$: Observable<number>;

  ngOnInit() {
    this.list.hookToQuery((query) => this.store.dispatch(new GetBooks(query))).subscribe();
  }
```

```html
<!-- simplified representation of the template -->

<abp-table
  [value]="books$ | async"
  [totalRecords]="bookCount$ | async"
>
</abp-table>
```

## How to Refresh Table on Create/Update/Delete

`ListService` exposes a `get` method to trigger a request with the current query. So, basically, whenever a create, update, or delete action resolves, you can call `this.list.get();` and it will call hooked stream creator again.

```ts
this.store.dispatch(new DeleteBook(id)).subscribe(this.list.get);
```

...or...

```ts
this.bookService.createByInput(form.value)
  .subscribe(() => {
    this.list.get();

    // Other subscription logic here
  })
```

## How to Implement Server-Side Search in a Table

`ListService` exposes a `filter` property that will trigger a request with the current query and the given search string. All you need to do is to bind it to an input element with two-way binding.

```html
<!-- simplified representation -->

<input type="text" name="search" [(ngModel)]="list.filter">
```
