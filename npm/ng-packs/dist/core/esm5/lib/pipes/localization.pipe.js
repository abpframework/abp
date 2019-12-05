/**
 * @fileoverview added by tsickle
 * Generated from: lib/pipes/localization.pipe.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Pipe, Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
var LocalizationPipe = /** @class */ (function () {
    function LocalizationPipe(store) {
        this.store = store;
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
        if (value === void 0) { value = ''; }
        var interpolateParams = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            interpolateParams[_i - 1] = arguments[_i];
        }
        return this.store.selectSnapshot(ConfigState.getLocalization.apply(ConfigState, tslib_1.__spread([value], interpolateParams.reduce((/**
         * @param {?} acc
         * @param {?} val
         * @return {?}
         */
        function (acc, val) { return (Array.isArray(val) ? tslib_1.__spread(acc, val) : tslib_1.__spread(acc, [val])); }), []))));
    };
    LocalizationPipe.decorators = [
        { type: Injectable },
        { type: Pipe, args: [{
                    name: 'abpLocalization',
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
    /**
     * @type {?}
     * @private
     */
    LocalizationPipe.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnBpcGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvcGlwZXMvbG9jYWxpemF0aW9uLnBpcGUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLElBQUksRUFBaUIsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ2hFLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFFcEMsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLFdBQVcsQ0FBQztBQUV4QztJQUtFLDBCQUFvQixLQUFZO1FBQVosVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7OztJQUVwQyxvQ0FBUzs7Ozs7SUFBVCxVQUFVLEtBQW1EO1FBQW5ELHNCQUFBLEVBQUEsVUFBbUQ7UUFBRSwyQkFBOEI7YUFBOUIsVUFBOEIsRUFBOUIscUJBQThCLEVBQTlCLElBQThCO1lBQTlCLDBDQUE4Qjs7UUFDM0YsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FDOUIsV0FBVyxDQUFDLGVBQWUsT0FBM0IsV0FBVyxvQkFDVCxLQUFLLEdBQ0YsaUJBQWlCLENBQUMsTUFBTTs7Ozs7UUFBQyxVQUFDLEdBQUcsRUFBRSxHQUFHLElBQUssT0FBQSxDQUFDLEtBQUssQ0FBQyxPQUFPLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQyxrQkFBSyxHQUFHLEVBQUssR0FBRyxFQUFFLENBQUMsa0JBQUssR0FBRyxHQUFFLEdBQUcsRUFBQyxDQUFDLEVBQXZELENBQXVELEdBQUUsRUFBRSxDQUFDLEdBRXpHLENBQUM7SUFDSixDQUFDOztnQkFkRixVQUFVO2dCQUNWLElBQUksU0FBQztvQkFDSixJQUFJLEVBQUUsaUJBQWlCO2lCQUN4Qjs7OztnQkFQUSxLQUFLOztJQW1CZCx1QkFBQztDQUFBLEFBZkQsSUFlQztTQVhZLGdCQUFnQjs7Ozs7O0lBQ2YsaUNBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgUGlwZSwgUGlwZVRyYW5zZm9ybSwgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBDb25maWcgfSBmcm9tICcuLi9tb2RlbHMnO1xuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMnO1xuXG5ASW5qZWN0YWJsZSgpXG5AUGlwZSh7XG4gIG5hbWU6ICdhYnBMb2NhbGl6YXRpb24nLFxufSlcbmV4cG9ydCBjbGFzcyBMb2NhbGl6YXRpb25QaXBlIGltcGxlbWVudHMgUGlwZVRyYW5zZm9ybSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIHRyYW5zZm9ybSh2YWx1ZTogc3RyaW5nIHwgQ29uZmlnLkxvY2FsaXphdGlvbldpdGhEZWZhdWx0ID0gJycsIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXSk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoXG4gICAgICBDb25maWdTdGF0ZS5nZXRMb2NhbGl6YXRpb24oXG4gICAgICAgIHZhbHVlLFxuICAgICAgICAuLi5pbnRlcnBvbGF0ZVBhcmFtcy5yZWR1Y2UoKGFjYywgdmFsKSA9PiAoQXJyYXkuaXNBcnJheSh2YWwpID8gWy4uLmFjYywgLi4udmFsXSA6IFsuLi5hY2MsIHZhbF0pLCBbXSksXG4gICAgICApLFxuICAgICk7XG4gIH1cbn1cbiJdfQ==