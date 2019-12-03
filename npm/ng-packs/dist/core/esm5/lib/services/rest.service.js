/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { throwError } from 'rxjs';
import { catchError, take, tap } from 'rxjs/operators';
import { RestOccurError } from '../actions/rest.actions';
import { ConfigState } from '../states/config.state';
import * as i0 from "@angular/core";
import * as i1 from "@angular/common/http";
import * as i2 from "@ngxs/store";
var RestService = /** @class */ (function () {
    function RestService(http, store) {
        this.http = http;
        this.store = store;
    }
    /**
     * @param {?} err
     * @return {?}
     */
    RestService.prototype.handleError = /**
     * @param {?} err
     * @return {?}
     */
    function (err) {
        this.store.dispatch(new RestOccurError(err));
        console.error(err);
        return throwError(err);
    };
    /**
     * @template T, R
     * @param {?} request
     * @param {?=} config
     * @param {?=} api
     * @return {?}
     */
    RestService.prototype.request = /**
     * @template T, R
     * @param {?} request
     * @param {?=} config
     * @param {?=} api
     * @return {?}
     */
    function (request, config, api) {
        var _this = this;
        config = config || ((/** @type {?} */ ({})));
        var _a = config.observe, observe = _a === void 0 ? "body" /* Body */ : _a, skipHandleError = config.skipHandleError;
        /** @type {?} */
        var url = (api || this.store.selectSnapshot(ConfigState.getApiUrl())) + request.url;
        var method = request.method, params = request.params, options = tslib_1.__rest(request, ["method", "params"]);
        return this.http
            .request(method, url, (/** @type {?} */ (tslib_1.__assign({ observe: observe }, (params && {
            params: Object.keys(params).reduce((/**
             * @param {?} acc
             * @param {?} key
             * @return {?}
             */
            function (acc, key) {
                var _a;
                return (tslib_1.__assign({}, acc, (typeof params[key] !== 'undefined' && params[key] !== '' && (_a = {}, _a[key] = params[key], _a))));
            }), {}),
        }), options))))
            .pipe(observe === "body" /* Body */ ? take(1) : tap(), catchError((/**
         * @param {?} err
         * @return {?}
         */
        function (err) {
            if (skipHandleError) {
                return throwError(err);
            }
            return _this.handleError(err);
        })));
    };
    RestService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    RestService.ctorParameters = function () { return [
        { type: HttpClient },
        { type: Store }
    ]; };
    /** @nocollapse */ RestService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function RestService_Factory() { return new RestService(i0.ɵɵinject(i1.HttpClient), i0.ɵɵinject(i2.Store)); }, token: RestService, providedIn: "root" });
    return RestService;
}());
export { RestService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    RestService.prototype.http;
    /**
     * @type {?}
     * @private
     */
    RestService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmVzdC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3Jlc3Quc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQWUsTUFBTSxzQkFBc0IsQ0FBQztBQUMvRCxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFjLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUM5QyxPQUFPLEVBQUUsVUFBVSxFQUFFLElBQUksRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUN2RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0seUJBQXlCLENBQUM7QUFFekQsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLHdCQUF3QixDQUFDOzs7O0FBRXJEO0lBSUUscUJBQW9CLElBQWdCLEVBQVUsS0FBWTtRQUF0QyxTQUFJLEdBQUosSUFBSSxDQUFZO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7O0lBRTlELGlDQUFXOzs7O0lBQVgsVUFBWSxHQUFRO1FBQ2xCLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksY0FBYyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7UUFDN0MsT0FBTyxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQztRQUNuQixPQUFPLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQztJQUN6QixDQUFDOzs7Ozs7OztJQUVELDZCQUFPOzs7Ozs7O0lBQVAsVUFBYyxPQUF5QyxFQUFFLE1BQW9CLEVBQUUsR0FBWTtRQUEzRixpQkE4QkM7UUE3QkMsTUFBTSxHQUFHLE1BQU0sSUFBSSxDQUFDLG1CQUFBLEVBQUUsRUFBZSxDQUFDLENBQUM7UUFDL0IsSUFBQSxtQkFBMkIsRUFBM0IsZ0RBQTJCLEVBQUUsd0NBQWU7O1lBQzlDLEdBQUcsR0FBRyxDQUFDLEdBQUcsSUFBSSxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsU0FBUyxFQUFFLENBQUMsQ0FBQyxHQUFHLE9BQU8sQ0FBQyxHQUFHO1FBQzdFLElBQUEsdUJBQU0sRUFBRSx1QkFBTSxFQUFFLHVEQUFVO1FBRWxDLE9BQU8sSUFBSSxDQUFDLElBQUk7YUFDYixPQUFPLENBQUksTUFBTSxFQUFFLEdBQUcsRUFBRSxzQ0FDdkIsT0FBTyxTQUFBLElBQ0osQ0FBQyxNQUFNLElBQUk7WUFDWixNQUFNLEVBQUUsTUFBTSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsQ0FBQyxNQUFNOzs7OztZQUNoQyxVQUFDLEdBQUcsRUFBRSxHQUFHOztnQkFBSyxPQUFBLHNCQUNULEdBQUcsRUFDSCxDQUFDLE9BQU8sTUFBTSxDQUFDLEdBQUcsQ0FBQyxLQUFLLFdBQVcsSUFBSSxNQUFNLENBQUMsR0FBRyxDQUFDLEtBQUssRUFBRSxjQUFNLEdBQUMsR0FBRyxJQUFHLE1BQU0sQ0FBQyxHQUFHLENBQUMsS0FBRSxDQUFDLEVBQ3ZGO1lBSFksQ0FHWixHQUNGLEVBQUUsQ0FDSDtTQUNGLENBQUMsRUFDQyxPQUFPLEdBQ0osQ0FBQzthQUNSLElBQUksQ0FDSCxPQUFPLHNCQUFzQixDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUcsRUFBRSxFQUMvQyxVQUFVOzs7O1FBQUMsVUFBQSxHQUFHO1lBQ1osSUFBSSxlQUFlLEVBQUU7Z0JBQ25CLE9BQU8sVUFBVSxDQUFDLEdBQUcsQ0FBQyxDQUFDO2FBQ3hCO1lBRUQsT0FBTyxLQUFJLENBQUMsV0FBVyxDQUFDLEdBQUcsQ0FBQyxDQUFDO1FBQy9CLENBQUMsRUFBQyxDQUNILENBQUM7SUFDTixDQUFDOztnQkExQ0YsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFYUSxVQUFVO2dCQUVWLEtBQUs7OztzQkFGZDtDQW9EQyxBQTNDRCxJQTJDQztTQXhDWSxXQUFXOzs7Ozs7SUFDViwyQkFBd0I7Ozs7O0lBQUUsNEJBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSHR0cENsaWVudCwgSHR0cFJlcXVlc3QgfSBmcm9tICdAYW5ndWxhci9jb21tb24vaHR0cCc7XHJcbmltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IE9ic2VydmFibGUsIHRocm93RXJyb3IgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgY2F0Y2hFcnJvciwgdGFrZSwgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5pbXBvcnQgeyBSZXN0T2NjdXJFcnJvciB9IGZyb20gJy4uL2FjdGlvbnMvcmVzdC5hY3Rpb25zJztcclxuaW1wb3J0IHsgUmVzdCB9IGZyb20gJy4uL21vZGVscy9yZXN0JztcclxuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMvY29uZmlnLnN0YXRlJztcclxuXHJcbkBJbmplY3RhYmxlKHtcclxuICBwcm92aWRlZEluOiAncm9vdCcsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBSZXN0U2VydmljZSB7XHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBodHRwOiBIdHRwQ2xpZW50LCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cclxuXHJcbiAgaGFuZGxlRXJyb3IoZXJyOiBhbnkpOiBPYnNlcnZhYmxlPGFueT4ge1xyXG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgUmVzdE9jY3VyRXJyb3IoZXJyKSk7XHJcbiAgICBjb25zb2xlLmVycm9yKGVycik7XHJcbiAgICByZXR1cm4gdGhyb3dFcnJvcihlcnIpO1xyXG4gIH1cclxuXHJcbiAgcmVxdWVzdDxULCBSPihyZXF1ZXN0OiBIdHRwUmVxdWVzdDxUPiB8IFJlc3QuUmVxdWVzdDxUPiwgY29uZmlnPzogUmVzdC5Db25maWcsIGFwaT86IHN0cmluZyk6IE9ic2VydmFibGU8Uj4ge1xyXG4gICAgY29uZmlnID0gY29uZmlnIHx8ICh7fSBhcyBSZXN0LkNvbmZpZyk7XHJcbiAgICBjb25zdCB7IG9ic2VydmUgPSBSZXN0Lk9ic2VydmUuQm9keSwgc2tpcEhhbmRsZUVycm9yIH0gPSBjb25maWc7XHJcbiAgICBjb25zdCB1cmwgPSAoYXBpIHx8IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0QXBpVXJsKCkpKSArIHJlcXVlc3QudXJsO1xyXG4gICAgY29uc3QgeyBtZXRob2QsIHBhcmFtcywgLi4ub3B0aW9ucyB9ID0gcmVxdWVzdDtcclxuXHJcbiAgICByZXR1cm4gdGhpcy5odHRwXHJcbiAgICAgIC5yZXF1ZXN0PFQ+KG1ldGhvZCwgdXJsLCB7XHJcbiAgICAgICAgb2JzZXJ2ZSxcclxuICAgICAgICAuLi4ocGFyYW1zICYmIHtcclxuICAgICAgICAgIHBhcmFtczogT2JqZWN0LmtleXMocGFyYW1zKS5yZWR1Y2UoXHJcbiAgICAgICAgICAgIChhY2MsIGtleSkgPT4gKHtcclxuICAgICAgICAgICAgICAuLi5hY2MsXHJcbiAgICAgICAgICAgICAgLi4uKHR5cGVvZiBwYXJhbXNba2V5XSAhPT0gJ3VuZGVmaW5lZCcgJiYgcGFyYW1zW2tleV0gIT09ICcnICYmIHsgW2tleV06IHBhcmFtc1trZXldIH0pLFxyXG4gICAgICAgICAgICB9KSxcclxuICAgICAgICAgICAge30sXHJcbiAgICAgICAgICApLFxyXG4gICAgICAgIH0pLFxyXG4gICAgICAgIC4uLm9wdGlvbnMsXHJcbiAgICAgIH0gYXMgYW55KVxyXG4gICAgICAucGlwZShcclxuICAgICAgICBvYnNlcnZlID09PSBSZXN0Lk9ic2VydmUuQm9keSA/IHRha2UoMSkgOiB0YXAoKSxcclxuICAgICAgICBjYXRjaEVycm9yKGVyciA9PiB7XHJcbiAgICAgICAgICBpZiAoc2tpcEhhbmRsZUVycm9yKSB7XHJcbiAgICAgICAgICAgIHJldHVybiB0aHJvd0Vycm9yKGVycik7XHJcbiAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgcmV0dXJuIHRoaXMuaGFuZGxlRXJyb3IoZXJyKTtcclxuICAgICAgICB9KSxcclxuICAgICAgKTtcclxuICB9XHJcbn1cclxuIl19