/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Pipe } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import { takeUntilDestroy } from '../utils';
import { distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
export class LocalizationPipe {
    /**
     * @param {?} store
     */
    constructor(store) {
        this.store = store;
        this.initialValue = '';
        this.destroy$ = new Subject();
    }
    /**
     * @param {?=} value
     * @param {...?} interpolateParams
     * @return {?}
     */
    transform(value = '', ...interpolateParams) {
        if (this.initialValue !== value) {
            this.initialValue = value;
            this.destroy$.next();
            this.store
                .select(ConfigState.getCopy(value, ...interpolateParams.reduce((/**
             * @param {?} acc
             * @param {?} val
             * @return {?}
             */
            (acc, val) => (Array.isArray(val) ? [...acc, ...val] : [...acc, val])), [])))
                .pipe(takeUntil(this.destroy$), takeUntilDestroy(this), distinctUntilChanged())
                .subscribe((/**
             * @param {?} copy
             * @return {?}
             */
            copy => (this.value = copy)));
        }
        return this.value;
    }
    /**
     * @return {?}
     */
    ngOnDestroy() { }
}
LocalizationPipe.decorators = [
    { type: Pipe, args: [{
                name: 'abpLocalization',
                pure: false,
            },] }
];
/** @nocollapse */
LocalizationPipe.ctorParameters = () => [
    { type: Store }
];
if (false) {
    /** @type {?} */
    LocalizationPipe.prototype.initialValue;
    /** @type {?} */
    LocalizationPipe.prototype.value;
    /** @type {?} */
    LocalizationPipe.prototype.destroy$;
    /**
     * @type {?}
     * @private
     */
    LocalizationPipe.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnBpcGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvcGlwZXMvbG9jYWxpemF0aW9uLnBpcGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxJQUFJLEVBQTRCLE1BQU0sZUFBZSxDQUFDO0FBQy9ELE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLFdBQVcsQ0FBQztBQUN4QyxPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxVQUFVLENBQUM7QUFDNUMsT0FBTyxFQUFFLG9CQUFvQixFQUFFLFNBQVMsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ2pFLE9BQU8sRUFBRSxPQUFPLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFNL0IsTUFBTSxPQUFPLGdCQUFnQjs7OztJQU8zQixZQUFvQixLQUFZO1FBQVosVUFBSyxHQUFMLEtBQUssQ0FBTztRQU5oQyxpQkFBWSxHQUFXLEVBQUUsQ0FBQztRQUkxQixhQUFRLEdBQUcsSUFBSSxPQUFPLEVBQUUsQ0FBQztJQUVVLENBQUM7Ozs7OztJQUVwQyxTQUFTLENBQUMsUUFBZ0IsRUFBRSxFQUFFLEdBQUcsaUJBQTJCO1FBQzFELElBQUksSUFBSSxDQUFDLFlBQVksS0FBSyxLQUFLLEVBQUU7WUFDL0IsSUFBSSxDQUFDLFlBQVksR0FBRyxLQUFLLENBQUM7WUFDMUIsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLEVBQUUsQ0FBQztZQUVyQixJQUFJLENBQUMsS0FBSztpQkFDUCxNQUFNLENBQ0wsV0FBVyxDQUFDLE9BQU8sQ0FDakIsS0FBSyxFQUNMLEdBQUcsaUJBQWlCLENBQUMsTUFBTTs7Ozs7WUFBQyxDQUFDLEdBQUcsRUFBRSxHQUFHLEVBQUUsRUFBRSxDQUFDLENBQUMsS0FBSyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLEdBQUcsRUFBRSxHQUFHLEdBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUcsR0FBRyxFQUFFLEdBQUcsQ0FBQyxDQUFDLEdBQUUsRUFBRSxDQUFDLENBQ3ZHLENBQ0Y7aUJBQ0EsSUFBSSxDQUNILFNBQVMsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLEVBQ3hCLGdCQUFnQixDQUFDLElBQUksQ0FBQyxFQUN0QixvQkFBb0IsRUFBRSxDQUN2QjtpQkFDQSxTQUFTOzs7O1lBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxDQUFDLElBQUksQ0FBQyxLQUFLLEdBQUcsSUFBSSxDQUFDLEVBQUMsQ0FBQztTQUMzQztRQUVELE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQztJQUNwQixDQUFDOzs7O0lBRUQsV0FBVyxLQUFJLENBQUM7OztZQXBDakIsSUFBSSxTQUFDO2dCQUNKLElBQUksRUFBRSxpQkFBaUI7Z0JBQ3ZCLElBQUksRUFBRSxLQUFLO2FBQ1o7Ozs7WUFUUSxLQUFLOzs7O0lBV1osd0NBQTBCOztJQUUxQixpQ0FBYzs7SUFFZCxvQ0FBeUI7Ozs7O0lBRWIsaUNBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgUGlwZSwgUGlwZVRyYW5zZm9ybSwgT25EZXN0cm95IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzJztcbmltcG9ydCB7IHRha2VVbnRpbERlc3Ryb3kgfSBmcm9tICcuLi91dGlscyc7XG5pbXBvcnQgeyBkaXN0aW5jdFVudGlsQ2hhbmdlZCwgdGFrZVVudGlsIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHsgU3ViamVjdCB9IGZyb20gJ3J4anMnO1xuXG5AUGlwZSh7XG4gIG5hbWU6ICdhYnBMb2NhbGl6YXRpb24nLFxuICBwdXJlOiBmYWxzZSwgLy8gcmVxdWlyZWQgdG8gdXBkYXRlIHRoZSB2YWx1ZVxufSlcbmV4cG9ydCBjbGFzcyBMb2NhbGl6YXRpb25QaXBlIGltcGxlbWVudHMgUGlwZVRyYW5zZm9ybSwgT25EZXN0cm95IHtcbiAgaW5pdGlhbFZhbHVlOiBzdHJpbmcgPSAnJztcblxuICB2YWx1ZTogc3RyaW5nO1xuXG4gIGRlc3Ryb3kkID0gbmV3IFN1YmplY3QoKTtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cblxuICB0cmFuc2Zvcm0odmFsdWU6IHN0cmluZyA9ICcnLCAuLi5pbnRlcnBvbGF0ZVBhcmFtczogc3RyaW5nW10pOiBzdHJpbmcge1xuICAgIGlmICh0aGlzLmluaXRpYWxWYWx1ZSAhPT0gdmFsdWUpIHtcbiAgICAgIHRoaXMuaW5pdGlhbFZhbHVlID0gdmFsdWU7XG4gICAgICB0aGlzLmRlc3Ryb3kkLm5leHQoKTtcblxuICAgICAgdGhpcy5zdG9yZVxuICAgICAgICAuc2VsZWN0KFxuICAgICAgICAgIENvbmZpZ1N0YXRlLmdldENvcHkoXG4gICAgICAgICAgICB2YWx1ZSxcbiAgICAgICAgICAgIC4uLmludGVycG9sYXRlUGFyYW1zLnJlZHVjZSgoYWNjLCB2YWwpID0+IChBcnJheS5pc0FycmF5KHZhbCkgPyBbLi4uYWNjLCAuLi52YWxdIDogWy4uLmFjYywgdmFsXSksIFtdKSxcbiAgICAgICAgICApLFxuICAgICAgICApXG4gICAgICAgIC5waXBlKFxuICAgICAgICAgIHRha2VVbnRpbCh0aGlzLmRlc3Ryb3kkKSxcbiAgICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxuICAgICAgICAgIGRpc3RpbmN0VW50aWxDaGFuZ2VkKCksXG4gICAgICAgIClcbiAgICAgICAgLnN1YnNjcmliZShjb3B5ID0+ICh0aGlzLnZhbHVlID0gY29weSkpO1xuICAgIH1cblxuICAgIHJldHVybiB0aGlzLnZhbHVlO1xuICB9XG5cbiAgbmdPbkRlc3Ryb3koKSB7fVxufVxuIl19