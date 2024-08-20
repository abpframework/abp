# Breadcrumb Component

ABP provides a component that listens to the angular router's `NavigationEnd`
event and creates inputs for `BreadcrumbItemsComponent`. This component is used in
ABP components with [`PageComponent`](./page-component.md).

## Breadcrumb Items Component

`BreadcrumbItemsComponent` is used to display breadcrumb items. It can be useful
when you want to display breadcrumb items in a different way than the default.

### Usage

Example of overriding the default template of `PageComponent`:

```html
<abp-page title="Title">
  <abp-page-breadcrumb-container>
    <abp-breadcrumb-items [items]="breadCrumbItems"></abp-breadcrumb-items>
  </abp-page-breadcrumb-container>
</abp-page>
```

```js
import { Component } from "@angular/core";
import { ABP } from "@abp/ng.core";

@Component({
  /* component metadata */
})
export class YourComponent {
  breadCrumbItems: ABP.Route[] = [
    {
      name: "Item 1",
    },
    {
      name: "Item 2",
      path: "/path",
    },
  ];
}
```

### Inputs

- items: Partial<ABP.Route>[] : Array of ABP.Route objects. The source code of ABP.Route can be found in [github](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/common.ts#L69).

## See Also

- [Page Component](./page-component.md)
