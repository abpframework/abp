# 使用ListService轻松查询列表

`ListService` 是一种实用程序服务,提供简单的分页,排序和搜索实现.

## 入门

`ListService` **没有在根提供**. 原因是通过这种方式它会清除组件上的所有订阅. 你可以使用可选的 `LIST_QUERY_DEBOUNCE_TIME` 令牌调整debounce行为.

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

> 注意 `list` 是 `public` 并且 `readonly`. 因为我们将直接在组件的模板中使用 `ListService` 属性. 可以视为反模式,但是实现起来要快得多. 你可以改为使用公共组件属性.

像这样绑定 `ListService` 到 ngx-datatable:

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

## 与Observables一起使用

你可以将Observables与Angular的[AsyncPipe](https://angular.io/guide/observables-in-angular#async-pipe)结合使用:

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

> 我们不建议将NGXS存储用于CRUD页面,除非你的应用程序需要在组件之间共享列表信息或稍后在另一页面中使用它.

## 如何在创建/更新/删除时刷新表

`ListService` 公开了一个 `get` 方法来触发当前查询的请求. 因此基本上每当创建,更新或删除操作解析时,你可以调用 `this.list.get();` 它会调用钩子流创建者.

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

## 如何在表中实现服务器端搜索

`ListService` 公开一个 `filter` 属性,该属性将使用当前查询和给定的搜索字符串触发一个请求. 你需要做的就是通过双向绑定将其绑定到输入元素.

```html
<!-- simplified representation -->

<input type="text" name="search" [(ngModel)]="list.filter">
```

## ABP v3.0的重大更改

我们必须修改 `ListService` 使其与 `ngx-datatable` 一起使用. 之前 `page` 属性的最小值为 `1`, 你可以像这样使用它:

```html
<!-- other bindings are hidden in favor of brevity -->
<abp-table
  [(page)]="list.page"
></abp-table>
```

从v3.0开始, 对于`ngx-datatable`, 初始页面的 `page`属性必须设置为 `0`. 因此如果你以前在表上使用过 `ListService` 并打算保留 `abp-table`,则需要进行以下更改:

```html
<!-- other bindings are hidden in favor of brevity -->
<abp-table
  [page]="list.page + 1"
  (pageChange)="list.page = $event - 1"
></abp-table>
```

**重要提示:** `abp-table` 没有被删除,但是会被弃用,并在将来的版本中移除,请考虑切换到 ngx-datatable.