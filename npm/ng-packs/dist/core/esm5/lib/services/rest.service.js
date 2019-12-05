/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/rest.service.ts
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmVzdC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3Jlc3Quc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFlLE1BQU0sc0JBQXNCLENBQUM7QUFDL0QsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBYyxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDOUMsT0FBTyxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdkQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHlCQUF5QixDQUFDO0FBRXpELE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSx3QkFBd0IsQ0FBQzs7OztBQUVyRDtJQUlFLHFCQUFvQixJQUFnQixFQUFVLEtBQVk7UUFBdEMsU0FBSSxHQUFKLElBQUksQ0FBWTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7OztJQUU5RCxpQ0FBVzs7OztJQUFYLFVBQVksR0FBUTtRQUNsQixJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLGNBQWMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO1FBQzdDLE9BQU8sQ0FBQyxLQUFLLENBQUMsR0FBRyxDQUFDLENBQUM7UUFDbkIsT0FBTyxVQUFVLENBQUMsR0FBRyxDQUFDLENBQUM7SUFDekIsQ0FBQzs7Ozs7Ozs7SUFFRCw2QkFBTzs7Ozs7OztJQUFQLFVBQWMsT0FBeUMsRUFBRSxNQUFvQixFQUFFLEdBQVk7UUFBM0YsaUJBOEJDO1FBN0JDLE1BQU0sR0FBRyxNQUFNLElBQUksQ0FBQyxtQkFBQSxFQUFFLEVBQWUsQ0FBQyxDQUFDO1FBQy9CLElBQUEsbUJBQTJCLEVBQTNCLGdEQUEyQixFQUFFLHdDQUFlOztZQUM5QyxHQUFHLEdBQUcsQ0FBQyxHQUFHLElBQUksSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLFNBQVMsRUFBRSxDQUFDLENBQUMsR0FBRyxPQUFPLENBQUMsR0FBRztRQUM3RSxJQUFBLHVCQUFNLEVBQUUsdUJBQU0sRUFBRSx1REFBVTtRQUVsQyxPQUFPLElBQUksQ0FBQyxJQUFJO2FBQ2IsT0FBTyxDQUFJLE1BQU0sRUFBRSxHQUFHLEVBQUUsc0NBQ3ZCLE9BQU8sU0FBQSxJQUNKLENBQUMsTUFBTSxJQUFJO1lBQ1osTUFBTSxFQUFFLE1BQU0sQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLENBQUMsTUFBTTs7Ozs7WUFDaEMsVUFBQyxHQUFHLEVBQUUsR0FBRzs7Z0JBQUssT0FBQSxzQkFDVCxHQUFHLEVBQ0gsQ0FBQyxPQUFPLE1BQU0sQ0FBQyxHQUFHLENBQUMsS0FBSyxXQUFXLElBQUksTUFBTSxDQUFDLEdBQUcsQ0FBQyxLQUFLLEVBQUUsY0FBTSxHQUFDLEdBQUcsSUFBRyxNQUFNLENBQUMsR0FBRyxDQUFDLEtBQUUsQ0FBQyxFQUN2RjtZQUhZLENBR1osR0FDRixFQUFFLENBQ0g7U0FDRixDQUFDLEVBQ0MsT0FBTyxHQUNKLENBQUM7YUFDUixJQUFJLENBQ0gsT0FBTyxzQkFBc0IsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLEVBQUUsRUFDL0MsVUFBVTs7OztRQUFDLFVBQUEsR0FBRztZQUNaLElBQUksZUFBZSxFQUFFO2dCQUNuQixPQUFPLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQzthQUN4QjtZQUVELE9BQU8sS0FBSSxDQUFDLFdBQVcsQ0FBQyxHQUFHLENBQUMsQ0FBQztRQUMvQixDQUFDLEVBQUMsQ0FDSCxDQUFDO0lBQ04sQ0FBQzs7Z0JBMUNGLFVBQVUsU0FBQztvQkFDVixVQUFVLEVBQUUsTUFBTTtpQkFDbkI7Ozs7Z0JBWFEsVUFBVTtnQkFFVixLQUFLOzs7c0JBRmQ7Q0FvREMsQUEzQ0QsSUEyQ0M7U0F4Q1ksV0FBVzs7Ozs7O0lBQ1YsMkJBQXdCOzs7OztJQUFFLDRCQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEh0dHBDbGllbnQsIEh0dHBSZXF1ZXN0IH0gZnJvbSAnQGFuZ3VsYXIvY29tbW9uL2h0dHAnO1xuaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlLCB0aHJvd0Vycm9yIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBjYXRjaEVycm9yLCB0YWtlLCB0YXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgeyBSZXN0T2NjdXJFcnJvciB9IGZyb20gJy4uL2FjdGlvbnMvcmVzdC5hY3Rpb25zJztcbmltcG9ydCB7IFJlc3QgfSBmcm9tICcuLi9tb2RlbHMvcmVzdCc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcy9jb25maWcuc3RhdGUnO1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290Jyxcbn0pXG5leHBvcnQgY2xhc3MgUmVzdFNlcnZpY2Uge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGh0dHA6IEh0dHBDbGllbnQsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIGhhbmRsZUVycm9yKGVycjogYW55KTogT2JzZXJ2YWJsZTxhbnk+IHtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBSZXN0T2NjdXJFcnJvcihlcnIpKTtcbiAgICBjb25zb2xlLmVycm9yKGVycik7XG4gICAgcmV0dXJuIHRocm93RXJyb3IoZXJyKTtcbiAgfVxuXG4gIHJlcXVlc3Q8VCwgUj4ocmVxdWVzdDogSHR0cFJlcXVlc3Q8VD4gfCBSZXN0LlJlcXVlc3Q8VD4sIGNvbmZpZz86IFJlc3QuQ29uZmlnLCBhcGk/OiBzdHJpbmcpOiBPYnNlcnZhYmxlPFI+IHtcbiAgICBjb25maWcgPSBjb25maWcgfHwgKHt9IGFzIFJlc3QuQ29uZmlnKTtcbiAgICBjb25zdCB7IG9ic2VydmUgPSBSZXN0Lk9ic2VydmUuQm9keSwgc2tpcEhhbmRsZUVycm9yIH0gPSBjb25maWc7XG4gICAgY29uc3QgdXJsID0gKGFwaSB8fCB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEFwaVVybCgpKSkgKyByZXF1ZXN0LnVybDtcbiAgICBjb25zdCB7IG1ldGhvZCwgcGFyYW1zLCAuLi5vcHRpb25zIH0gPSByZXF1ZXN0O1xuXG4gICAgcmV0dXJuIHRoaXMuaHR0cFxuICAgICAgLnJlcXVlc3Q8VD4obWV0aG9kLCB1cmwsIHtcbiAgICAgICAgb2JzZXJ2ZSxcbiAgICAgICAgLi4uKHBhcmFtcyAmJiB7XG4gICAgICAgICAgcGFyYW1zOiBPYmplY3Qua2V5cyhwYXJhbXMpLnJlZHVjZShcbiAgICAgICAgICAgIChhY2MsIGtleSkgPT4gKHtcbiAgICAgICAgICAgICAgLi4uYWNjLFxuICAgICAgICAgICAgICAuLi4odHlwZW9mIHBhcmFtc1trZXldICE9PSAndW5kZWZpbmVkJyAmJiBwYXJhbXNba2V5XSAhPT0gJycgJiYgeyBba2V5XTogcGFyYW1zW2tleV0gfSksXG4gICAgICAgICAgICB9KSxcbiAgICAgICAgICAgIHt9LFxuICAgICAgICAgICksXG4gICAgICAgIH0pLFxuICAgICAgICAuLi5vcHRpb25zLFxuICAgICAgfSBhcyBhbnkpXG4gICAgICAucGlwZShcbiAgICAgICAgb2JzZXJ2ZSA9PT0gUmVzdC5PYnNlcnZlLkJvZHkgPyB0YWtlKDEpIDogdGFwKCksXG4gICAgICAgIGNhdGNoRXJyb3IoZXJyID0+IHtcbiAgICAgICAgICBpZiAoc2tpcEhhbmRsZUVycm9yKSB7XG4gICAgICAgICAgICByZXR1cm4gdGhyb3dFcnJvcihlcnIpO1xuICAgICAgICAgIH1cblxuICAgICAgICAgIHJldHVybiB0aGlzLmhhbmRsZUVycm9yKGVycik7XG4gICAgICAgIH0pLFxuICAgICAgKTtcbiAgfVxufVxuIl19