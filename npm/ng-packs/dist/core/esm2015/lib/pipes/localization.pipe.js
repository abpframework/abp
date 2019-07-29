/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Pipe } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import { takeUntilDestroy } from '../utils';
import { distinctUntilChanged } from 'rxjs/operators';
export class LocalizationPipe {
    /**
     * @param {?} store
     */
    constructor(store) {
        this.store = store;
        this.initialized = false;
    }
    /**
     * @param {?} value
     * @param {...?} interpolateParams
     * @return {?}
     */
    transform(value, ...interpolateParams) {
        if (!this.initialized) {
            this.initialized = true;
            this.store
                .select(ConfigState.getCopy(value, ...interpolateParams.reduce((/**
             * @param {?} acc
             * @param {?} val
             * @return {?}
             */
            (acc, val) => (Array.isArray(val) ? [...acc, ...val] : [...acc, val])), [])))
                .pipe(takeUntilDestroy(this), distinctUntilChanged())
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
    LocalizationPipe.prototype.initialized;
    /** @type {?} */
    LocalizationPipe.prototype.value;
    /**
     * @type {?}
     * @private
     */
    LocalizationPipe.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnBpcGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvcGlwZXMvbG9jYWxpemF0aW9uLnBpcGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxJQUFJLEVBQTRCLE1BQU0sZUFBZSxDQUFDO0FBQy9ELE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLFdBQVcsQ0FBQztBQUN4QyxPQUFPLEVBQUUsZ0JBQWdCLEVBQUUsTUFBTSxVQUFVLENBQUM7QUFDNUMsT0FBTyxFQUFFLG9CQUFvQixFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFNdEQsTUFBTSxPQUFPLGdCQUFnQjs7OztJQUszQixZQUFvQixLQUFZO1FBQVosVUFBSyxHQUFMLEtBQUssQ0FBTztRQUpoQyxnQkFBVyxHQUFZLEtBQUssQ0FBQztJQUlNLENBQUM7Ozs7OztJQUVwQyxTQUFTLENBQUMsS0FBYSxFQUFFLEdBQUcsaUJBQTJCO1FBQ3JELElBQUksQ0FBQyxJQUFJLENBQUMsV0FBVyxFQUFFO1lBQ3JCLElBQUksQ0FBQyxXQUFXLEdBQUcsSUFBSSxDQUFDO1lBRXhCLElBQUksQ0FBQyxLQUFLO2lCQUNQLE1BQU0sQ0FDTCxXQUFXLENBQUMsT0FBTyxDQUNqQixLQUFLLEVBQ0wsR0FBRyxpQkFBaUIsQ0FBQyxNQUFNOzs7OztZQUFDLENBQUMsR0FBRyxFQUFFLEdBQUcsRUFBRSxFQUFFLENBQUMsQ0FBQyxLQUFLLENBQUMsT0FBTyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUcsR0FBRyxFQUFFLEdBQUcsR0FBRyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBRyxHQUFHLEVBQUUsR0FBRyxDQUFDLENBQUMsR0FBRSxFQUFFLENBQUMsQ0FDdkcsQ0FDRjtpQkFDQSxJQUFJLENBQ0gsZ0JBQWdCLENBQUMsSUFBSSxDQUFDLEVBQ3RCLG9CQUFvQixFQUFFLENBQ3ZCO2lCQUNBLFNBQVM7Ozs7WUFBQyxJQUFJLENBQUMsRUFBRSxDQUFDLENBQUMsSUFBSSxDQUFDLEtBQUssR0FBRyxJQUFJLENBQUMsRUFBQyxDQUFDO1NBQzNDO1FBRUQsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDO0lBQ3BCLENBQUM7Ozs7SUFFRCxXQUFXLEtBQUksQ0FBQzs7O1lBaENqQixJQUFJLFNBQUM7Z0JBQ0osSUFBSSxFQUFFLGlCQUFpQjtnQkFDdkIsSUFBSSxFQUFFLEtBQUs7YUFDWjs7OztZQVJRLEtBQUs7Ozs7SUFVWix1Q0FBNkI7O0lBRTdCLGlDQUFjOzs7OztJQUVGLGlDQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFBpcGUsIFBpcGVUcmFuc2Zvcm0sIE9uRGVzdHJveSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcyc7XG5pbXBvcnQgeyB0YWtlVW50aWxEZXN0cm95IH0gZnJvbSAnLi4vdXRpbHMnO1xuaW1wb3J0IHsgZGlzdGluY3RVbnRpbENoYW5nZWQgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbkBQaXBlKHtcbiAgbmFtZTogJ2FicExvY2FsaXphdGlvbicsXG4gIHB1cmU6IGZhbHNlLCAvLyByZXF1aXJlZCB0byB1cGRhdGUgdGhlIHZhbHVlXG59KVxuZXhwb3J0IGNsYXNzIExvY2FsaXphdGlvblBpcGUgaW1wbGVtZW50cyBQaXBlVHJhbnNmb3JtLCBPbkRlc3Ryb3kge1xuICBpbml0aWFsaXplZDogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIHZhbHVlOiBzdHJpbmc7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgdHJhbnNmb3JtKHZhbHVlOiBzdHJpbmcsIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXSk6IHN0cmluZyB7XG4gICAgaWYgKCF0aGlzLmluaXRpYWxpemVkKSB7XG4gICAgICB0aGlzLmluaXRpYWxpemVkID0gdHJ1ZTtcblxuICAgICAgdGhpcy5zdG9yZVxuICAgICAgICAuc2VsZWN0KFxuICAgICAgICAgIENvbmZpZ1N0YXRlLmdldENvcHkoXG4gICAgICAgICAgICB2YWx1ZSxcbiAgICAgICAgICAgIC4uLmludGVycG9sYXRlUGFyYW1zLnJlZHVjZSgoYWNjLCB2YWwpID0+IChBcnJheS5pc0FycmF5KHZhbCkgPyBbLi4uYWNjLCAuLi52YWxdIDogWy4uLmFjYywgdmFsXSksIFtdKSxcbiAgICAgICAgICApLFxuICAgICAgICApXG4gICAgICAgIC5waXBlKFxuICAgICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcyksXG4gICAgICAgICAgZGlzdGluY3RVbnRpbENoYW5nZWQoKSxcbiAgICAgICAgKVxuICAgICAgICAuc3Vic2NyaWJlKGNvcHkgPT4gKHRoaXMudmFsdWUgPSBjb3B5KSk7XG4gICAgfVxuXG4gICAgcmV0dXJuIHRoaXMudmFsdWU7XG4gIH1cblxuICBuZ09uRGVzdHJveSgpIHt9XG59XG4iXX0=