# Sorting Navigation Elements

This documentation describes how the navigation elements are sorted and how to change this default behaviour.

- When you want to add the `Navigation Element` you can use the `RoutesService`. For more details, see the [document](../angular/modifying-the-menu.md).
- However, in this documentation, we will talk more about how to sort the navigation elements.

## Order Property

- Normally, you are able to sort your routes with this property. But you can customize our default sorting algorithm.

## Default Sorting algorithm

- To see our default sorting algorithm [click](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/tokens/compare-func.token.ts) here.
  **What does this function do?**
  - if the order property is defined, then it will be sorted by the order value.
  - if both of the navigation elements have the same order value then it will be sorted by the name.
  - If the order property is not defined, it will be the last element and the unordered navs will be sorted by name.

# How to Customize

**`in app.module.ts`**

```ts
import { SORT_COMPARE_FUNC } from "@abp/ng.core";

@NgModule({
  providers: [
    ...{
      provide: SORT_COMPARE_FUNC,
      useFactory: yourCompareFuncFactory,
    },
  ],
  // imports, declarations, and bootstrap
})
export class AppModule {}
```
