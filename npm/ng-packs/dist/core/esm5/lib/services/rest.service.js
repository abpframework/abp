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
        var method = request.method, options = tslib_1.__rest(request, ["method"]);
        return this.http.request(method, url, (/** @type {?} */ (tslib_1.__assign({ observe: observe }, options)))).pipe(observe === "body" /* Body */ ? take(1) : tap(), catchError((/**
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmVzdC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3Jlc3Quc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQWUsTUFBTSxzQkFBc0IsQ0FBQztBQUMvRCxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFjLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUM5QyxPQUFPLEVBQUUsVUFBVSxFQUFFLElBQUksRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUN2RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0seUJBQXlCLENBQUM7QUFFekQsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLHdCQUF3QixDQUFDOzs7O0FBRXJEO0lBSUUscUJBQW9CLElBQWdCLEVBQVUsS0FBWTtRQUF0QyxTQUFJLEdBQUosSUFBSSxDQUFZO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7O0lBRTlELGlDQUFXOzs7O0lBQVgsVUFBWSxHQUFRO1FBQ2xCLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksY0FBYyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7UUFDN0MsT0FBTyxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQztRQUNuQixPQUFPLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQztJQUN6QixDQUFDOzs7Ozs7OztJQUVELDZCQUFPOzs7Ozs7O0lBQVAsVUFBYyxPQUF5QyxFQUFFLE1BQW9CLEVBQUUsR0FBWTtRQUEzRixpQkFnQkM7UUFmQyxNQUFNLEdBQUcsTUFBTSxJQUFJLENBQUMsbUJBQUEsRUFBRSxFQUFlLENBQUMsQ0FBQztRQUMvQixJQUFBLG1CQUEyQixFQUEzQixnREFBMkIsRUFBRSx3Q0FBZTs7WUFDOUMsR0FBRyxHQUFHLENBQUMsR0FBRyxJQUFJLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxTQUFTLEVBQUUsQ0FBQyxDQUFDLEdBQUcsT0FBTyxDQUFDLEdBQUc7UUFDN0UsSUFBQSx1QkFBTSxFQUFFLDZDQUFVO1FBRTFCLE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQUksTUFBTSxFQUFFLEdBQUcsRUFBRSxzQ0FBRSxPQUFPLFNBQUEsSUFBSyxPQUFPLEdBQVMsQ0FBQyxDQUFDLElBQUksQ0FDM0UsT0FBTyxzQkFBc0IsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLEVBQUUsRUFDL0MsVUFBVTs7OztRQUFDLFVBQUEsR0FBRztZQUNaLElBQUksZUFBZSxFQUFFO2dCQUNuQixPQUFPLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQzthQUN4QjtZQUVELE9BQU8sS0FBSSxDQUFDLFdBQVcsQ0FBQyxHQUFHLENBQUMsQ0FBQztRQUMvQixDQUFDLEVBQUMsQ0FDSCxDQUFDO0lBQ0osQ0FBQzs7Z0JBNUJGLFVBQVUsU0FBQztvQkFDVixVQUFVLEVBQUUsTUFBTTtpQkFDbkI7Ozs7Z0JBWFEsVUFBVTtnQkFFVixLQUFLOzs7c0JBRmQ7Q0FzQ0MsQUE3QkQsSUE2QkM7U0ExQlksV0FBVzs7Ozs7O0lBQ1YsMkJBQXdCOzs7OztJQUFFLDRCQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEh0dHBDbGllbnQsIEh0dHBSZXF1ZXN0IH0gZnJvbSAnQGFuZ3VsYXIvY29tbW9uL2h0dHAnO1xyXG5pbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBPYnNlcnZhYmxlLCB0aHJvd0Vycm9yIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IGNhdGNoRXJyb3IsIHRha2UsIHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcclxuaW1wb3J0IHsgUmVzdE9jY3VyRXJyb3IgfSBmcm9tICcuLi9hY3Rpb25zL3Jlc3QuYWN0aW9ucyc7XHJcbmltcG9ydCB7IFJlc3QgfSBmcm9tICcuLi9tb2RlbHMvcmVzdCc7XHJcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL2NvbmZpZy5zdGF0ZSc7XHJcblxyXG5ASW5qZWN0YWJsZSh7XHJcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgUmVzdFNlcnZpY2Uge1xyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgaHR0cDogSHR0cENsaWVudCwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XHJcblxyXG4gIGhhbmRsZUVycm9yKGVycjogYW55KTogT2JzZXJ2YWJsZTxhbnk+IHtcclxuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFJlc3RPY2N1ckVycm9yKGVycikpO1xyXG4gICAgY29uc29sZS5lcnJvcihlcnIpO1xyXG4gICAgcmV0dXJuIHRocm93RXJyb3IoZXJyKTtcclxuICB9XHJcblxyXG4gIHJlcXVlc3Q8VCwgUj4ocmVxdWVzdDogSHR0cFJlcXVlc3Q8VD4gfCBSZXN0LlJlcXVlc3Q8VD4sIGNvbmZpZz86IFJlc3QuQ29uZmlnLCBhcGk/OiBzdHJpbmcpOiBPYnNlcnZhYmxlPFI+IHtcclxuICAgIGNvbmZpZyA9IGNvbmZpZyB8fCAoe30gYXMgUmVzdC5Db25maWcpO1xyXG4gICAgY29uc3QgeyBvYnNlcnZlID0gUmVzdC5PYnNlcnZlLkJvZHksIHNraXBIYW5kbGVFcnJvciB9ID0gY29uZmlnO1xyXG4gICAgY29uc3QgdXJsID0gKGFwaSB8fCB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEFwaVVybCgpKSkgKyByZXF1ZXN0LnVybDtcclxuICAgIGNvbnN0IHsgbWV0aG9kLCAuLi5vcHRpb25zIH0gPSByZXF1ZXN0O1xyXG5cclxuICAgIHJldHVybiB0aGlzLmh0dHAucmVxdWVzdDxUPihtZXRob2QsIHVybCwgeyBvYnNlcnZlLCAuLi5vcHRpb25zIH0gYXMgYW55KS5waXBlKFxyXG4gICAgICBvYnNlcnZlID09PSBSZXN0Lk9ic2VydmUuQm9keSA/IHRha2UoMSkgOiB0YXAoKSxcclxuICAgICAgY2F0Y2hFcnJvcihlcnIgPT4ge1xyXG4gICAgICAgIGlmIChza2lwSGFuZGxlRXJyb3IpIHtcclxuICAgICAgICAgIHJldHVybiB0aHJvd0Vycm9yKGVycik7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICByZXR1cm4gdGhpcy5oYW5kbGVFcnJvcihlcnIpO1xyXG4gICAgICB9KSxcclxuICAgICk7XHJcbiAgfVxyXG59XHJcbiJdfQ==