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

将 `ListService` 属性放入模板中,如下所示:

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

## 与Observables一起使用

你可以将Observables与Angular的[AsyncPipe](https://angular.io/guide/observables-in-angular#async-pipe)结合使用:

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
