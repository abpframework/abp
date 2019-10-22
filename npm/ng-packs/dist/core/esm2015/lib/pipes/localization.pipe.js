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
        return this.store.selectSnapshot(ConfigState.getLocalization(value, ...interpolateParams.reduce((/**
         * @param {?} acc
         * @param {?} val
         * @return {?}
         */
        (acc, val) => (Array.isArray(val) ? [...acc, ...val] : [...acc, val])), [])));
    }
}
LocalizationPipe.decorators = [
    { type: Pipe, args: [{
                name: 'abpLocalization',
            },] }
];
/** @nocollapse */
LocalizationPipe.ctorParameters = () => [
    { type: Store }
];
if (false) {
    /**
     * @type {?}
     * @private
     */
    LocalizationPipe.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnBpcGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvcGlwZXMvbG9jYWxpemF0aW9uLnBpcGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxJQUFJLEVBQWlCLE1BQU0sZUFBZSxDQUFDO0FBQ3BELE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFFcEMsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLFdBQVcsQ0FBQztBQUt4QyxNQUFNLE9BQU8sZ0JBQWdCOzs7O0lBQzNCLFlBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQzs7Ozs7O0lBRXBDLFNBQVMsQ0FBQyxRQUFpRCxFQUFFLEVBQUUsR0FBRyxpQkFBMkI7UUFDM0YsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FDOUIsV0FBVyxDQUFDLGVBQWUsQ0FDekIsS0FBSyxFQUNMLEdBQUcsaUJBQWlCLENBQUMsTUFBTTs7Ozs7UUFBQyxDQUFDLEdBQUcsRUFBRSxHQUFHLEVBQUUsRUFBRSxDQUFDLENBQUMsS0FBSyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLEdBQUcsRUFBRSxHQUFHLEdBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUcsR0FBRyxFQUFFLEdBQUcsQ0FBQyxDQUFDLEdBQUUsRUFBRSxDQUFDLENBQ3ZHLENBQ0YsQ0FBQztJQUNKLENBQUM7OztZQWJGLElBQUksU0FBQztnQkFDSixJQUFJLEVBQUUsaUJBQWlCO2FBQ3hCOzs7O1lBTlEsS0FBSzs7Ozs7OztJQVFBLGlDQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFBpcGUsIFBpcGVUcmFuc2Zvcm0gfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IENvbmZpZyB9IGZyb20gJy4uL21vZGVscyc7XHJcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzJztcclxuXHJcbkBQaXBlKHtcclxuICBuYW1lOiAnYWJwTG9jYWxpemF0aW9uJyxcclxufSlcclxuZXhwb3J0IGNsYXNzIExvY2FsaXphdGlvblBpcGUgaW1wbGVtZW50cyBQaXBlVHJhbnNmb3JtIHtcclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cclxuXHJcbiAgdHJhbnNmb3JtKHZhbHVlOiBzdHJpbmcgfCBDb25maWcuTG9jYWxpemF0aW9uV2l0aERlZmF1bHQgPSAnJywgLi4uaW50ZXJwb2xhdGVQYXJhbXM6IHN0cmluZ1tdKTogc3RyaW5nIHtcclxuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFxyXG4gICAgICBDb25maWdTdGF0ZS5nZXRMb2NhbGl6YXRpb24oXHJcbiAgICAgICAgdmFsdWUsXHJcbiAgICAgICAgLi4uaW50ZXJwb2xhdGVQYXJhbXMucmVkdWNlKChhY2MsIHZhbCkgPT4gKEFycmF5LmlzQXJyYXkodmFsKSA/IFsuLi5hY2MsIC4uLnZhbF0gOiBbLi4uYWNjLCB2YWxdKSwgW10pLFxyXG4gICAgICApLFxyXG4gICAgKTtcclxuICB9XHJcbn1cclxuIl19