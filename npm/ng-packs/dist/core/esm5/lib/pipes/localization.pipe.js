/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Pipe } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import { takeUntilDestroy } from '../utils';
import { distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
var LocalizationPipe = /** @class */ (function () {
    function LocalizationPipe(store) {
        this.store = store;
        this.initialValue = '';
        this.destroy$ = new Subject();
    }
    /**
     * @param {?=} value
     * @param {...?} interpolateParams
     * @return {?}
     */
    LocalizationPipe.prototype.transform = /**
     * @param {?=} value
     * @param {...?} interpolateParams
     * @return {?}
     */
    function (value) {
        var _this = this;
        if (value === void 0) { value = ''; }
        var interpolateParams = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            interpolateParams[_i - 1] = arguments[_i];
        }
        if (this.initialValue !== value) {
            this.initialValue = value;
            this.destroy$.next();
            this.store
                .select(ConfigState.getCopy.apply(ConfigState, tslib_1.__spread([value], interpolateParams.reduce((/**
             * @param {?} acc
             * @param {?} val
             * @return {?}
             */
            function (acc, val) { return (Array.isArray(val) ? tslib_1.__spread(acc, val) : tslib_1.__spread(acc, [val])); }), []))))
                .pipe(takeUntil(this.destroy$), takeUntilDestroy(this), distinctUntilChanged())
                .subscribe((/**
             * @param {?} copy
             * @return {?}
             */
            function (copy) { return (_this.value = copy); }));
        }
        return this.value;
    };
    /**
     * @return {?}
     */
    LocalizationPipe.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () { };
    LocalizationPipe.decorators = [
        { type: Pipe, args: [{
                    name: 'abpLocalization',
                    pure: false,
                },] }
    ];
    /** @nocollapse */
    LocalizationPipe.ctorParameters = function () { return [
        { type: Store }
    ]; };
    return LocalizationPipe;
}());
export { LocalizationPipe };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnBpcGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvcGlwZXMvbG9jYWxpemF0aW9uLnBpcGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsSUFBSSxFQUE0QixNQUFNLGVBQWUsQ0FBQztBQUMvRCxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxXQUFXLENBQUM7QUFDeEMsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sVUFBVSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxTQUFTLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNqRSxPQUFPLEVBQUUsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBRS9CO0lBV0UsMEJBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO1FBTmhDLGlCQUFZLEdBQVcsRUFBRSxDQUFDO1FBSTFCLGFBQVEsR0FBRyxJQUFJLE9BQU8sRUFBRSxDQUFDO0lBRVUsQ0FBQzs7Ozs7O0lBRXBDLG9DQUFTOzs7OztJQUFULFVBQVUsS0FBa0I7UUFBNUIsaUJBcUJDO1FBckJTLHNCQUFBLEVBQUEsVUFBa0I7UUFBRSwyQkFBOEI7YUFBOUIsVUFBOEIsRUFBOUIscUJBQThCLEVBQTlCLElBQThCO1lBQTlCLDBDQUE4Qjs7UUFDMUQsSUFBSSxJQUFJLENBQUMsWUFBWSxLQUFLLEtBQUssRUFBRTtZQUMvQixJQUFJLENBQUMsWUFBWSxHQUFHLEtBQUssQ0FBQztZQUMxQixJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksRUFBRSxDQUFDO1lBRXJCLElBQUksQ0FBQyxLQUFLO2lCQUNQLE1BQU0sQ0FDTCxXQUFXLENBQUMsT0FBTyxPQUFuQixXQUFXLG9CQUNULEtBQUssR0FDRixpQkFBaUIsQ0FBQyxNQUFNOzs7OztZQUFDLFVBQUMsR0FBRyxFQUFFLEdBQUcsSUFBSyxPQUFBLENBQUMsS0FBSyxDQUFDLE9BQU8sQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLGtCQUFLLEdBQUcsRUFBSyxHQUFHLEVBQUUsQ0FBQyxrQkFBSyxHQUFHLEdBQUUsR0FBRyxFQUFDLENBQUMsRUFBdkQsQ0FBdUQsR0FBRSxFQUFFLENBQUMsR0FFekc7aUJBQ0EsSUFBSSxDQUNILFNBQVMsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLEVBQ3hCLGdCQUFnQixDQUFDLElBQUksQ0FBQyxFQUN0QixvQkFBb0IsRUFBRSxDQUN2QjtpQkFDQSxTQUFTOzs7O1lBQUMsVUFBQSxJQUFJLElBQUksT0FBQSxDQUFDLEtBQUksQ0FBQyxLQUFLLEdBQUcsSUFBSSxDQUFDLEVBQW5CLENBQW1CLEVBQUMsQ0FBQztTQUMzQztRQUVELE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQztJQUNwQixDQUFDOzs7O0lBRUQsc0NBQVc7OztJQUFYLGNBQWUsQ0FBQzs7Z0JBcENqQixJQUFJLFNBQUM7b0JBQ0osSUFBSSxFQUFFLGlCQUFpQjtvQkFDdkIsSUFBSSxFQUFFLEtBQUs7aUJBQ1o7Ozs7Z0JBVFEsS0FBSzs7SUEyQ2QsdUJBQUM7Q0FBQSxBQXJDRCxJQXFDQztTQWpDWSxnQkFBZ0I7OztJQUMzQix3Q0FBMEI7O0lBRTFCLGlDQUFjOztJQUVkLG9DQUF5Qjs7Ozs7SUFFYixpQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBQaXBlLCBQaXBlVHJhbnNmb3JtLCBPbkRlc3Ryb3kgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMnO1xuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJy4uL3V0aWxzJztcbmltcG9ydCB7IGRpc3RpbmN0VW50aWxDaGFuZ2VkLCB0YWtlVW50aWwgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgeyBTdWJqZWN0IH0gZnJvbSAncnhqcyc7XG5cbkBQaXBlKHtcbiAgbmFtZTogJ2FicExvY2FsaXphdGlvbicsXG4gIHB1cmU6IGZhbHNlLCAvLyByZXF1aXJlZCB0byB1cGRhdGUgdGhlIHZhbHVlXG59KVxuZXhwb3J0IGNsYXNzIExvY2FsaXphdGlvblBpcGUgaW1wbGVtZW50cyBQaXBlVHJhbnNmb3JtLCBPbkRlc3Ryb3kge1xuICBpbml0aWFsVmFsdWU6IHN0cmluZyA9ICcnO1xuXG4gIHZhbHVlOiBzdHJpbmc7XG5cbiAgZGVzdHJveSQgPSBuZXcgU3ViamVjdCgpO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIHRyYW5zZm9ybSh2YWx1ZTogc3RyaW5nID0gJycsIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXSk6IHN0cmluZyB7XG4gICAgaWYgKHRoaXMuaW5pdGlhbFZhbHVlICE9PSB2YWx1ZSkge1xuICAgICAgdGhpcy5pbml0aWFsVmFsdWUgPSB2YWx1ZTtcbiAgICAgIHRoaXMuZGVzdHJveSQubmV4dCgpO1xuXG4gICAgICB0aGlzLnN0b3JlXG4gICAgICAgIC5zZWxlY3QoXG4gICAgICAgICAgQ29uZmlnU3RhdGUuZ2V0Q29weShcbiAgICAgICAgICAgIHZhbHVlLFxuICAgICAgICAgICAgLi4uaW50ZXJwb2xhdGVQYXJhbXMucmVkdWNlKChhY2MsIHZhbCkgPT4gKEFycmF5LmlzQXJyYXkodmFsKSA/IFsuLi5hY2MsIC4uLnZhbF0gOiBbLi4uYWNjLCB2YWxdKSwgW10pLFxuICAgICAgICAgICksXG4gICAgICAgIClcbiAgICAgICAgLnBpcGUoXG4gICAgICAgICAgdGFrZVVudGlsKHRoaXMuZGVzdHJveSQpLFxuICAgICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcyksXG4gICAgICAgICAgZGlzdGluY3RVbnRpbENoYW5nZWQoKSxcbiAgICAgICAgKVxuICAgICAgICAuc3Vic2NyaWJlKGNvcHkgPT4gKHRoaXMudmFsdWUgPSBjb3B5KSk7XG4gICAgfVxuXG4gICAgcmV0dXJuIHRoaXMudmFsdWU7XG4gIH1cblxuICBuZ09uRGVzdHJveSgpIHt9XG59XG4iXX0=