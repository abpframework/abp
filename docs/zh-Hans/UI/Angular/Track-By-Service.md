# 轻松实现TrackByFunction

`TrackByService` 是一个实用服务,为Angular模板中最常见的需求之一: `TrackByFunction` 提供简单的实现. 在继续下面的内容之前,请参先阅 [Angular 文档](https://angular.io/guide/template-syntax#ngfor-with-trackby).

## 入门

你不必在模块或组件级别提供 `TrackByService`,因为它已经在**根中提供了**. 你可以在组件中注入并开始使用它. 为了获得更好的类型支持,你可以将迭代项目的类型传递给它.

```js
import { TrackByService } from '@abp/ng.core';

@Component({
  /* class metadata here */
})
class DemoComponent {
  list: Item[];

  constructor(public readonly track: TrackByService<Item>) {}
}
```

> 注意到 `track` 是 `public` 并且 `readonly` 了吗? 因为我们将看到一些在组件模板中直接使用 `TrackByService` 实例的方法示例. 可以把它看做反模式,但它有自身的优势,尤其是在利用组件继承时. 你始终可以使用公共组件属性.

**成员也被导出做为独立的函数.** 如果你不想注入 `TrackByService`, 你可以直接在类中导入并使用这些函数.

## 用法

有两种方法可用.

1. 你可以直接注入 `TrackByService` 到你的组件并且使用它的成员.
2. 你可以在直接在组件属性上使用导出的函数.

### 如何通过一个键跟踪项

你可以使用 `by` 获取一个 `TrackByFunction` , 该函数根据它的一个键来跟踪迭代的对象. 你可以将迭代类型传递给它获得类型支持.

```html
<!-- template of DemoComponent -->

<div *ngFor="let item of list; trackBy: track.by('id')">{%{{{ item.name }}}%}</div>
```

`by` 作为一个独立函数导出,命名为 `trackBy`.

```js
import { trackBy } from "@abp/ng.core";

@Component({
  template: `
    <div
      *ngFor="let item of list; trackBy: trackById"
    >
      {%{{{ item.name }}}%}
    </div>
  `,
})
class DemoComponent {
  list: Item[];

  trackById = trackBy<Item>('id');
}
```

### 如何通过深度嵌套的键进行跟踪

你可以使用 `byDeep` 获取一个 `TrackByFunction` , 它根据深度嵌套的键跟踪迭代对象. 你可以将迭代类型传递给它获得类型支持.


```html
<!-- template of DemoComponent -->

<div
  *ngFor="let item of list; trackBy: track.byDeep('tenant', 'account', 'id')"
>
  {%{{{ item.tenant.name }}}%}
</div>
```

`byDeep` 作为一个独立函数导出,命名为 `trackByDeep`.

```js
import { trackByDeep } from "@abp/ng.core";

@Component({
  template: `
    <div
      *ngFor="let item of list; trackBy: trackByTenantAccountId"
    >
      {%{{{ item.name }}}%}
    </div>
  `,
})
class DemoComponent {
  list: Item[];

  trackByTenantAccountId = trackByDeep<Item>('tenant', 'account', 'id');
}
```

## 下一步是什么?

- [SubscriptionService](./Subscription-Service.md)