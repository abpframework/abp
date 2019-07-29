/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { NEVER, throwError } from 'rxjs';
import { catchError, take } from 'rxjs/operators';
import { ConfigState } from '../states';
import { RestOccurError } from '../actions';
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
        return NEVER;
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
        if (config === void 0) { config = {}; }
        var _a = config.observe, observe = _a === void 0 ? "body" /* Body */ : _a, throwErr = config.throwErr;
        /** @type {?} */
        var url = api || this.store.selectSnapshot(ConfigState.getApiUrl()) + request.url;
        var method = request.method, options = tslib_1.__rest(request, ["method"]);
        return this.http.request(method, url, (/** @type {?} */ (tslib_1.__assign({ observe: observe }, options)))).pipe(observe === "body" /* Body */ ? take(1) : null, catchError((/**
         * @param {?} err
         * @return {?}
         */
        function (err) {
            if (throwErr) {
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmVzdC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3Jlc3Quc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQWUsTUFBTSxzQkFBc0IsQ0FBQztBQUMvRCxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLEtBQUssRUFBYyxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDckQsT0FBTyxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUVsRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sV0FBVyxDQUFDO0FBQ3hDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxZQUFZLENBQUM7Ozs7QUFFNUM7SUFJRSxxQkFBb0IsSUFBZ0IsRUFBVSxLQUFZO1FBQXRDLFNBQUksR0FBSixJQUFJLENBQVk7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQzs7Ozs7SUFFOUQsaUNBQVc7Ozs7SUFBWCxVQUFZLEdBQVE7UUFDbEIsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxjQUFjLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQztRQUM3QyxPQUFPLENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQyxDQUFDO1FBQ25CLE9BQU8sS0FBSyxDQUFDO0lBQ2YsQ0FBQzs7Ozs7Ozs7SUFFRCw2QkFBTzs7Ozs7OztJQUFQLFVBQWMsT0FBeUMsRUFBRSxNQUF3QixFQUFFLEdBQVk7UUFBL0YsaUJBY0M7UUFkd0QsdUJBQUEsRUFBQSxXQUF3QjtRQUN2RSxJQUFBLG1CQUEyQixFQUEzQixnREFBMkIsRUFBRSwwQkFBUTs7WUFDdkMsR0FBRyxHQUFHLEdBQUcsSUFBSSxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsU0FBUyxFQUFFLENBQUMsR0FBRyxPQUFPLENBQUMsR0FBRztRQUMzRSxJQUFBLHVCQUFNLEVBQUUsNkNBQVU7UUFDMUIsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBSSxNQUFNLEVBQUUsR0FBRyxFQUFFLHNDQUFFLE9BQU8sU0FBQSxJQUFLLE9BQU8sR0FBUyxDQUFDLENBQUMsSUFBSSxDQUMzRSxPQUFPLHNCQUFzQixDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLElBQUksRUFDOUMsVUFBVTs7OztRQUFDLFVBQUEsR0FBRztZQUNaLElBQUksUUFBUSxFQUFFO2dCQUNaLE9BQU8sVUFBVSxDQUFDLEdBQUcsQ0FBQyxDQUFDO2FBQ3hCO1lBRUQsT0FBTyxLQUFJLENBQUMsV0FBVyxDQUFDLEdBQUcsQ0FBQyxDQUFDO1FBQy9CLENBQUMsRUFBQyxDQUNILENBQUM7SUFDSixDQUFDOztnQkExQkYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFYUSxVQUFVO2dCQUVWLEtBQUs7OztzQkFGZDtDQW9DQyxBQTNCRCxJQTJCQztTQXhCWSxXQUFXOzs7Ozs7SUFDViwyQkFBd0I7Ozs7O0lBQUUsNEJBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSHR0cENsaWVudCwgSHR0cFJlcXVlc3QgfSBmcm9tICdAYW5ndWxhci9jb21tb24vaHR0cCc7XG5pbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE5FVkVSLCBPYnNlcnZhYmxlLCB0aHJvd0Vycm9yIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBjYXRjaEVycm9yLCB0YWtlIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHsgUmVzdCB9IGZyb20gJy4uL21vZGVscy9yZXN0JztcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzJztcbmltcG9ydCB7IFJlc3RPY2N1ckVycm9yIH0gZnJvbSAnLi4vYWN0aW9ucyc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBSZXN0U2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgaHR0cDogSHR0cENsaWVudCwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgaGFuZGxlRXJyb3IoZXJyOiBhbnkpOiBPYnNlcnZhYmxlPGFueT4ge1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFJlc3RPY2N1ckVycm9yKGVycikpO1xuICAgIGNvbnNvbGUuZXJyb3IoZXJyKTtcbiAgICByZXR1cm4gTkVWRVI7XG4gIH1cblxuICByZXF1ZXN0PFQsIFI+KHJlcXVlc3Q6IEh0dHBSZXF1ZXN0PFQ+IHwgUmVzdC5SZXF1ZXN0PFQ+LCBjb25maWc6IFJlc3QuQ29uZmlnID0ge30sIGFwaT86IHN0cmluZyk6IE9ic2VydmFibGU8Uj4ge1xuICAgIGNvbnN0IHsgb2JzZXJ2ZSA9IFJlc3QuT2JzZXJ2ZS5Cb2R5LCB0aHJvd0VyciB9ID0gY29uZmlnO1xuICAgIGNvbnN0IHVybCA9IGFwaSB8fCB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEFwaVVybCgpKSArIHJlcXVlc3QudXJsO1xuICAgIGNvbnN0IHsgbWV0aG9kLCAuLi5vcHRpb25zIH0gPSByZXF1ZXN0O1xuICAgIHJldHVybiB0aGlzLmh0dHAucmVxdWVzdDxUPihtZXRob2QsIHVybCwgeyBvYnNlcnZlLCAuLi5vcHRpb25zIH0gYXMgYW55KS5waXBlKFxuICAgICAgb2JzZXJ2ZSA9PT0gUmVzdC5PYnNlcnZlLkJvZHkgPyB0YWtlKDEpIDogbnVsbCxcbiAgICAgIGNhdGNoRXJyb3IoZXJyID0+IHtcbiAgICAgICAgaWYgKHRocm93RXJyKSB7XG4gICAgICAgICAgcmV0dXJuIHRocm93RXJyb3IoZXJyKTtcbiAgICAgICAgfVxuXG4gICAgICAgIHJldHVybiB0aGlzLmhhbmRsZUVycm9yKGVycik7XG4gICAgICB9KSxcbiAgICApO1xuICB9XG59XG4iXX0=