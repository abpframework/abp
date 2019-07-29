/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Pipe } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import { takeUntilDestroy } from '../utils';
import { distinctUntilChanged } from 'rxjs/operators';
var LocalizationPipe = /** @class */ (function () {
    function LocalizationPipe(store) {
        this.store = store;
        this.initialized = false;
    }
    /**
     * @param {?} value
     * @param {...?} interpolateParams
     * @return {?}
     */
    LocalizationPipe.prototype.transform = /**
     * @param {?} value
     * @param {...?} interpolateParams
     * @return {?}
     */
    function (value) {
        var _this = this;
        var interpolateParams = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            interpolateParams[_i - 1] = arguments[_i];
        }
        if (!this.initialized) {
            this.initialized = true;
            this.store
                .select(ConfigState.getCopy.apply(ConfigState, tslib_1.__spread([value], interpolateParams.reduce((/**
             * @param {?} acc
             * @param {?} val
             * @return {?}
             */
            function (acc, val) { return (Array.isArray(val) ? tslib_1.__spread(acc, val) : tslib_1.__spread(acc, [val])); }), []))))
                .pipe(takeUntilDestroy(this), distinctUntilChanged())
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
    LocalizationPipe.prototype.initialized;
    /** @type {?} */
    LocalizationPipe.prototype.value;
    /**
     * @type {?}
     * @private
     */
    LocalizationPipe.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnBpcGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvcGlwZXMvbG9jYWxpemF0aW9uLnBpcGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsSUFBSSxFQUE0QixNQUFNLGVBQWUsQ0FBQztBQUMvRCxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxXQUFXLENBQUM7QUFDeEMsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sVUFBVSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBRXREO0lBU0UsMEJBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO1FBSmhDLGdCQUFXLEdBQVksS0FBSyxDQUFDO0lBSU0sQ0FBQzs7Ozs7O0lBRXBDLG9DQUFTOzs7OztJQUFULFVBQVUsS0FBYTtRQUF2QixpQkFtQkM7UUFuQndCLDJCQUE4QjthQUE5QixVQUE4QixFQUE5QixxQkFBOEIsRUFBOUIsSUFBOEI7WUFBOUIsMENBQThCOztRQUNyRCxJQUFJLENBQUMsSUFBSSxDQUFDLFdBQVcsRUFBRTtZQUNyQixJQUFJLENBQUMsV0FBVyxHQUFHLElBQUksQ0FBQztZQUV4QixJQUFJLENBQUMsS0FBSztpQkFDUCxNQUFNLENBQ0wsV0FBVyxDQUFDLE9BQU8sT0FBbkIsV0FBVyxvQkFDVCxLQUFLLEdBQ0YsaUJBQWlCLENBQUMsTUFBTTs7Ozs7WUFBQyxVQUFDLEdBQUcsRUFBRSxHQUFHLElBQUssT0FBQSxDQUFDLEtBQUssQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQyxrQkFBSyxHQUFHLEVBQUssR0FBRyxFQUFFLENBQUMsa0JBQUssR0FBRyxHQUFFLEdBQUcsRUFBQyxDQUFDLEVBQXZELENBQXVELEdBQUUsRUFBRSxDQUFDLEdBRXpHO2lCQUNBLElBQUksQ0FDSCxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsRUFDdEIsb0JBQW9CLEVBQUUsQ0FDdkI7aUJBQ0EsU0FBUzs7OztZQUFDLFVBQUEsSUFBSSxJQUFJLE9BQUEsQ0FBQyxLQUFJLENBQUMsS0FBSyxHQUFHLElBQUksQ0FBQyxFQUFuQixDQUFtQixFQUFDLENBQUM7U0FDM0M7UUFFRCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUM7SUFDcEIsQ0FBQzs7OztJQUVELHNDQUFXOzs7SUFBWCxjQUFlLENBQUM7O2dCQWhDakIsSUFBSSxTQUFDO29CQUNKLElBQUksRUFBRSxpQkFBaUI7b0JBQ3ZCLElBQUksRUFBRSxLQUFLO2lCQUNaOzs7O2dCQVJRLEtBQUs7O0lBc0NkLHVCQUFDO0NBQUEsQUFqQ0QsSUFpQ0M7U0E3QlksZ0JBQWdCOzs7SUFDM0IsdUNBQTZCOztJQUU3QixpQ0FBYzs7Ozs7SUFFRixpQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBQaXBlLCBQaXBlVHJhbnNmb3JtLCBPbkRlc3Ryb3kgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMnO1xuaW1wb3J0IHsgdGFrZVVudGlsRGVzdHJveSB9IGZyb20gJy4uL3V0aWxzJztcbmltcG9ydCB7IGRpc3RpbmN0VW50aWxDaGFuZ2VkIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuXG5AUGlwZSh7XG4gIG5hbWU6ICdhYnBMb2NhbGl6YXRpb24nLFxuICBwdXJlOiBmYWxzZSwgLy8gcmVxdWlyZWQgdG8gdXBkYXRlIHRoZSB2YWx1ZVxufSlcbmV4cG9ydCBjbGFzcyBMb2NhbGl6YXRpb25QaXBlIGltcGxlbWVudHMgUGlwZVRyYW5zZm9ybSwgT25EZXN0cm95IHtcbiAgaW5pdGlhbGl6ZWQ6IGJvb2xlYW4gPSBmYWxzZTtcblxuICB2YWx1ZTogc3RyaW5nO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIHRyYW5zZm9ybSh2YWx1ZTogc3RyaW5nLCAuLi5pbnRlcnBvbGF0ZVBhcmFtczogc3RyaW5nW10pOiBzdHJpbmcge1xuICAgIGlmICghdGhpcy5pbml0aWFsaXplZCkge1xuICAgICAgdGhpcy5pbml0aWFsaXplZCA9IHRydWU7XG5cbiAgICAgIHRoaXMuc3RvcmVcbiAgICAgICAgLnNlbGVjdChcbiAgICAgICAgICBDb25maWdTdGF0ZS5nZXRDb3B5KFxuICAgICAgICAgICAgdmFsdWUsXG4gICAgICAgICAgICAuLi5pbnRlcnBvbGF0ZVBhcmFtcy5yZWR1Y2UoKGFjYywgdmFsKSA9PiAoQXJyYXkuaXNBcnJheSh2YWwpID8gWy4uLmFjYywgLi4udmFsXSA6IFsuLi5hY2MsIHZhbF0pLCBbXSksXG4gICAgICAgICAgKSxcbiAgICAgICAgKVxuICAgICAgICAucGlwZShcbiAgICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxuICAgICAgICAgIGRpc3RpbmN0VW50aWxDaGFuZ2VkKCksXG4gICAgICAgIClcbiAgICAgICAgLnN1YnNjcmliZShjb3B5ID0+ICh0aGlzLnZhbHVlID0gY29weSkpO1xuICAgIH1cblxuICAgIHJldHVybiB0aGlzLnZhbHVlO1xuICB9XG5cbiAgbmdPbkRlc3Ryb3koKSB7fVxufVxuIl19