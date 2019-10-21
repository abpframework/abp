/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Pipe } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
export class LocalizationPipe {
  /**
   * @param {?} store
   */
  constructor(store) {
    this.store = store;
  }
  /**
   * @param {?=} value
   * @param {...?} interpolateParams
   * @return {?}
   */
  transform(value = '', ...interpolateParams) {
    return this.store.selectSnapshot(
      ConfigState.getLocalization(
        value,
        ...interpolateParams.reduce(
          /**
           * @param {?} acc
           * @param {?} val
           * @return {?}
           */
          (acc, val) => (Array.isArray(val) ? [...acc, ...val] : [...acc, val]),
          [],
        ),
      ),
    );
  }
}
LocalizationPipe.decorators = [
  {
    type: Pipe,
    args: [
      {
        name: 'abpLocalization',
      },
    ],
  },
];
/** @nocollapse */
LocalizationPipe.ctorParameters = () => [{ type: Store }];
if (false) {
  /**
   * @type {?}
   * @private
   */
  LocalizationPipe.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnBpcGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvcGlwZXMvbG9jYWxpemF0aW9uLnBpcGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxJQUFJLEVBQWlCLE1BQU0sZUFBZSxDQUFDO0FBQ3BELE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFFcEMsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLFdBQVcsQ0FBQztBQUt4QyxNQUFNLE9BQU8sZ0JBQWdCOzs7O0lBQzNCLFlBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQzs7Ozs7O0lBRXBDLFNBQVMsQ0FBQyxRQUFpRCxFQUFFLEVBQUUsR0FBRyxpQkFBMkI7UUFDM0YsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FDOUIsV0FBVyxDQUFDLGVBQWUsQ0FDekIsS0FBSyxFQUNMLEdBQUcsaUJBQWlCLENBQUMsTUFBTTs7Ozs7UUFBQyxDQUFDLEdBQUcsRUFBRSxHQUFHLEVBQUUsRUFBRSxDQUFDLENBQUMsS0FBSyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLEdBQUcsRUFBRSxHQUFHLEdBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUcsR0FBRyxFQUFFLEdBQUcsQ0FBQyxDQUFDLEdBQUUsRUFBRSxDQUFDLENBQ3ZHLENBQ0YsQ0FBQztJQUNKLENBQUM7OztZQWJGLElBQUksU0FBQztnQkFDSixJQUFJLEVBQUUsaUJBQWlCO2FBQ3hCOzs7O1lBTlEsS0FBSzs7Ozs7OztJQVFBLGlDQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFBpcGUsIFBpcGVUcmFuc2Zvcm0gfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgQ29uZmlnIH0gZnJvbSAnLi4vbW9kZWxzJztcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzJztcblxuQFBpcGUoe1xuICBuYW1lOiAnYWJwTG9jYWxpemF0aW9uJyxcbn0pXG5leHBvcnQgY2xhc3MgTG9jYWxpemF0aW9uUGlwZSBpbXBsZW1lbnRzIFBpcGVUcmFuc2Zvcm0ge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cblxuICB0cmFuc2Zvcm0odmFsdWU6IHN0cmluZyB8IENvbmZpZy5Mb2NhbGl6YXRpb25XaXRoRGVmYXVsdCA9ICcnLCAuLi5pbnRlcnBvbGF0ZVBhcmFtczogc3RyaW5nW10pOiBzdHJpbmcge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFxuICAgICAgQ29uZmlnU3RhdGUuZ2V0TG9jYWxpemF0aW9uKFxuICAgICAgICB2YWx1ZSxcbiAgICAgICAgLi4uaW50ZXJwb2xhdGVQYXJhbXMucmVkdWNlKChhY2MsIHZhbCkgPT4gKEFycmF5LmlzQXJyYXkodmFsKSA/IFsuLi5hY2MsIC4uLnZhbF0gOiBbLi4uYWNjLCB2YWxdKSwgW10pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG59XG4iXX0=
