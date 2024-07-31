# Easy *ngFor trackBy

`TrackByService` is a utility service to provide an easy implementation for one of the most frequent needs in Angular templates: `TrackByFunction`. Please see [this page in Angular docs](https://angular.io/guide/template-syntax#ngfor-with-trackby) for its purpose.



## Getting Started

You do not have to provide the `TrackByService` at module or component level, because it is already **provided in root**. You can inject and start using it immediately in your components. For better type support, you may pass in the type of the iterated item to it.

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



> Noticed `track` is `public` and `readonly`? That is because we will see some examples where methods of `TrackByService` instance are directly called in the component's template. That may be considered as an anti-pattern, but it has its own advantage, especially when component inheritance is leveraged. You can always use public component properties instead.



**The members are also exported as separate functions.** If you do not want to inject `TrackByService`, you can always import and use those functions directly in your classes.



## Usage

There are two approaches available.

1. You may inject `TrackByService` to your component and use its members.
2. You may use exported higher-order functions directly on component properties.



### How to Track Items by a Key

You can use `by` to get a `TrackByFunction` that tracks the iterated object based on one of its keys. For type support, you may pass in the type of the iterated item to it.

```html
<!-- template of DemoComponent -->

<div *ngFor="let item of list; trackBy: track.by('id')">{%{{{ item.name }}}%}</div>
```



`by` is exported as a stand-alone function and is named `trackBy`.

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



### How to Track by a Deeply Nested Key

You can use `byDeep` to get a `TrackByFunction` that tracks the iterated object based on a deeply nested key. For type support, you may pass in the type of the iterated item to it.

```html
<!-- template of DemoComponent -->

<div
  *ngFor="let item of list; trackBy: track.byDeep('tenant', 'account', 'id')"
>
  {%{{{ item.tenant.name }}}%}
</div>
```



`byDeep` is exported as a stand-alone function and is named `trackByDeep`.

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
