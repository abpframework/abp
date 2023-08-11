# Sorting Navigation Elements
This documentation describes how the navigation elements are sorted and how to change this default behaviour.

When you want to add the `Navigation Element` you can use the `RoutesService`. For more details, see the [document](https://docs.abp.io/en/abp/latest/UI/Angular/Modifying-the-Menu#how-to-add-a-navigation-element).
However, in this documentation, we will talk more about how to sort the navigation elements with the `order` attribute from the `Routes Service`.


### Order Property
- This parameter is optional and is used for sorting purposes.
- If you define this property it will be sorted by the default sorting function.
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
**What does this function do?**
- if the order property is defined, then it will be sorted by the order value.
- if both of the navigation elements have the same order value then it will be sorted by the name.
- If the order property is not defined, it will be the last element and the unordered navs will be sorted by name.

You can edit this sorting function behaviour as you wish. 
