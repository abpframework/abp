# Sorting Navigation Elements
This documentation is describing how navigation elements sorted and how you can change this default behaviour.

When you want to add `Navigation Element` you can use `RoutesService`. For more info [click here](https://docs.abp.io/en/abp/latest/UI/Angular/Modifying-the-Menu#how-to-add-a-navigation-element).
However, in this documentation, we will talk more about how to sort navigation elements via the `Routes Service` `order` attribute.


### Order Property
- This parameter is optional and used for sorting purposes.
- If you define this property it will be sorted by default sorting function.
- You can edit this function.

**Default Compare Function;**
``compare-func.token.ts``
```ts
export const SORT_COMPARE_FUNC = new InjectionToken<0 | 1 | -1>('SORT_COMPARE_FUNC');

export function compareFuncFactory() {
  const localizationService = inject(LocalizationService);
  const fn = (a,b) => {
    const aName = localizationService.instant(a.name);
    const bName = localizationService.instant(b.name);
    
    const aNumber = a.order;
    const bNumber = b.order;
    
    if (!Number.isInteger(aNumber)) return 1;
    if (!Number.isInteger(bNumber)) return -1;
    
    if (aNumber > bNumber) return 1
    if (aNumber < bNumber) return -1
    
    if (aName > bName ) return 1;
    if (aName < bName ) return -1;
    
    return  0
  }
  return  fn;
}
```
**What this function do?**
- if order property defined, then it will be sorted by order value.
- if both navigation element has same order value then it will be sorted by name.
- If order property is not defined, it will be last element and sort by name between unordered navs.

You can edit this sorting function behaviour as you wish. 