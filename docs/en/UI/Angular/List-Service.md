# Working with Lists

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

> Noticed `list` is `public` and `readonly`? That is because we will use `ListService` directly in the component's template. That may be considered as an anti-pattern, but it is much quicker to implement. You can always use public component members to expose the `ListService` instance instead.

Bind `ListService` to ngx-datatable like this:

```html
<ngx-datatable
  [rows]="items"
  [count]="count"
  [list]="list"
  default
>
  <!-- column templates here -->
</ngx-datatable>
```


## Usage with Observables

You may use observables in combination with [AsyncPipe](https://angular.io/guide/observables-in-angular#async-pipe) of Angular instead. Here are some possibilities:

```js
  book$ = this.list.hookToQuery(query => this.bookService.getListByInput(query));
```

```html
<!-- simplified representation of the template -->

<ngx-datatable
  [rows]="(book$ | async)?.items || []"
  [count]="(book$ | async)?.totalCount || 0"
  [list]="list"
  default
>
  <!-- column templates here -->
</ngx-datatable>

<!-- DO NOT WORRY, ONLY ONE REQUEST WILL BE MADE -->
```

...or...


```js
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

<ngx-datatable
  [rows]="(books$ | async) || []"
  [count]="(bookCount$ | async) || 0"
  [list]="list"
  default
>
  <!-- column templates here -->
</ngx-datatable>
```

> We do not recommend using NGXS store for CRUD pages, unless your application needs to share list information between components or use it later on in another page.


## How to Refresh Table on Create/Update/Delete

`ListService` exposes a `get` method to trigger a request with the current query. So, basically, whenever a create, update, or delete action resolves, you can call `this.list.get();` and it will call hooked stream creator again.

```js
  this.bookService.createByInput(form.value)
    .subscribe(() => {
      this.list.get();

      // Other subscription logic here
    });
```

...or...

```js
  this.store.dispatch(new DeleteBook(id)).subscribe(this.list.get);
```

> We donot recommend using NGXS store for CRUD pages, unless your application needs to share list information between components or use it later on in another page.


## How to Implement Server-Side Search in a Table

`ListService` exposes a `filter` property that will trigger a request with the current query and the given search string. All you need to do is to bind it to an input element with two-way binding.

```html
<!-- simplified representation -->

<input type="text" name="search" [(ngModel)]="list.filter">
```

## Breaking Change with ABP v3.0

We had to modify the `ListService` to make it work with `ngx-datatable`. Previously, the minimum value for `page` property was `1` and you could use it like this:

```html
<!-- other bindings are hidden in favor of brevity -->
<abp-table
  [(page)]="list.page"
></abp-table>
```

As of v3.0, with ngx-datatable, the `page` property has to be set as `0` for inital page. Therefore, if you used `ListService` on your tables before and are going to keep `abp-table`, you need to make the following change:

```html
<!-- other bindings are hidden in favor of brevity -->
<abp-table
  [page]="list.page + 1"
  (pageChange)="list.page = $event - 1"
></abp-table>
```

**Important Note:** The `abp-table` is not removed, but is deprecated and will be removed in the future. Please consider switching to ngx-datatable.
