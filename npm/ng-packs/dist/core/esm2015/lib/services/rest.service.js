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
export class RestService {
    /**
     * @param {?} http
     * @param {?} store
     */
    constructor(http, store) {
        this.http = http;
        this.store = store;
    }
    /**
     * @param {?} err
     * @return {?}
     */
    handleError(err) {
        this.store.dispatch(new RestOccurError(err));
        console.error(err);
        return NEVER;
    }
    /**
     * @template T, R
     * @param {?} request
     * @param {?=} config
     * @param {?=} api
     * @return {?}
     */
    request(request, config = {}, api) {
        const { observe = "body" /* Body */, throwErr } = config;
        /** @type {?} */
        const url = api || this.store.selectSnapshot(ConfigState.getApiUrl()) + request.url;
        const { method } = request, options = tslib_1.__rest(request, ["method"]);
        return this.http.request(method, url, (/** @type {?} */ (Object.assign({ observe }, options)))).pipe(observe === "body" /* Body */ ? take(1) : null, catchError((/**
         * @param {?} err
         * @return {?}
         */
        err => {
            if (throwErr) {
                return throwError(err);
            }
            return this.handleError(err);
        })));
    }
}
RestService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
RestService.ctorParameters = () => [
    { type: HttpClient },
    { type: Store }
];
/** @nocollapse */ RestService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function RestService_Factory() { return new RestService(i0.ɵɵinject(i1.HttpClient), i0.ɵɵinject(i2.Store)); }, token: RestService, providedIn: "root" });
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmVzdC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3Jlc3Quc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQWUsTUFBTSxzQkFBc0IsQ0FBQztBQUMvRCxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLEtBQUssRUFBYyxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDckQsT0FBTyxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUVsRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sV0FBVyxDQUFDO0FBQ3hDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxZQUFZLENBQUM7Ozs7QUFLNUMsTUFBTSxPQUFPLFdBQVc7Ozs7O0lBQ3RCLFlBQW9CLElBQWdCLEVBQVUsS0FBWTtRQUF0QyxTQUFJLEdBQUosSUFBSSxDQUFZO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7O0lBRTlELFdBQVcsQ0FBQyxHQUFRO1FBQ2xCLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksY0FBYyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7UUFDN0MsT0FBTyxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQztRQUNuQixPQUFPLEtBQUssQ0FBQztJQUNmLENBQUM7Ozs7Ozs7O0lBRUQsT0FBTyxDQUFPLE9BQXlDLEVBQUUsU0FBc0IsRUFBRSxFQUFFLEdBQVk7Y0FDdkYsRUFBRSxPQUFPLG9CQUFvQixFQUFFLFFBQVEsRUFBRSxHQUFHLE1BQU07O2NBQ2xELEdBQUcsR0FBRyxHQUFHLElBQUksSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLFNBQVMsRUFBRSxDQUFDLEdBQUcsT0FBTyxDQUFDLEdBQUc7Y0FDN0UsRUFBRSxNQUFNLEtBQWlCLE9BQU8sRUFBdEIsNkNBQVU7UUFDMUIsT0FBTyxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBSSxNQUFNLEVBQUUsR0FBRyxFQUFFLG1DQUFFLE9BQU8sSUFBSyxPQUFPLEdBQVMsQ0FBQyxDQUFDLElBQUksQ0FDM0UsT0FBTyxzQkFBc0IsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLEVBQzlDLFVBQVU7Ozs7UUFBQyxHQUFHLENBQUMsRUFBRTtZQUNmLElBQUksUUFBUSxFQUFFO2dCQUNaLE9BQU8sVUFBVSxDQUFDLEdBQUcsQ0FBQyxDQUFDO2FBQ3hCO1lBRUQsT0FBTyxJQUFJLENBQUMsV0FBVyxDQUFDLEdBQUcsQ0FBQyxDQUFDO1FBQy9CLENBQUMsRUFBQyxDQUNILENBQUM7SUFDSixDQUFDOzs7WUExQkYsVUFBVSxTQUFDO2dCQUNWLFVBQVUsRUFBRSxNQUFNO2FBQ25COzs7O1lBWFEsVUFBVTtZQUVWLEtBQUs7Ozs7Ozs7O0lBV0EsMkJBQXdCOzs7OztJQUFFLDRCQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEh0dHBDbGllbnQsIEh0dHBSZXF1ZXN0IH0gZnJvbSAnQGFuZ3VsYXIvY29tbW9uL2h0dHAnO1xuaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBORVZFUiwgT2JzZXJ2YWJsZSwgdGhyb3dFcnJvciB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgY2F0Y2hFcnJvciwgdGFrZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7IFJlc3QgfSBmcm9tICcuLi9tb2RlbHMvcmVzdCc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcyc7XG5pbXBvcnQgeyBSZXN0T2NjdXJFcnJvciB9IGZyb20gJy4uL2FjdGlvbnMnO1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290Jyxcbn0pXG5leHBvcnQgY2xhc3MgUmVzdFNlcnZpY2Uge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGh0dHA6IEh0dHBDbGllbnQsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIGhhbmRsZUVycm9yKGVycjogYW55KTogT2JzZXJ2YWJsZTxhbnk+IHtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBSZXN0T2NjdXJFcnJvcihlcnIpKTtcbiAgICBjb25zb2xlLmVycm9yKGVycik7XG4gICAgcmV0dXJuIE5FVkVSO1xuICB9XG5cbiAgcmVxdWVzdDxULCBSPihyZXF1ZXN0OiBIdHRwUmVxdWVzdDxUPiB8IFJlc3QuUmVxdWVzdDxUPiwgY29uZmlnOiBSZXN0LkNvbmZpZyA9IHt9LCBhcGk/OiBzdHJpbmcpOiBPYnNlcnZhYmxlPFI+IHtcbiAgICBjb25zdCB7IG9ic2VydmUgPSBSZXN0Lk9ic2VydmUuQm9keSwgdGhyb3dFcnIgfSA9IGNvbmZpZztcbiAgICBjb25zdCB1cmwgPSBhcGkgfHwgdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBcGlVcmwoKSkgKyByZXF1ZXN0LnVybDtcbiAgICBjb25zdCB7IG1ldGhvZCwgLi4ub3B0aW9ucyB9ID0gcmVxdWVzdDtcbiAgICByZXR1cm4gdGhpcy5odHRwLnJlcXVlc3Q8VD4obWV0aG9kLCB1cmwsIHsgb2JzZXJ2ZSwgLi4ub3B0aW9ucyB9IGFzIGFueSkucGlwZShcbiAgICAgIG9ic2VydmUgPT09IFJlc3QuT2JzZXJ2ZS5Cb2R5ID8gdGFrZSgxKSA6IG51bGwsXG4gICAgICBjYXRjaEVycm9yKGVyciA9PiB7XG4gICAgICAgIGlmICh0aHJvd0Vycikge1xuICAgICAgICAgIHJldHVybiB0aHJvd0Vycm9yKGVycik7XG4gICAgICAgIH1cblxuICAgICAgICByZXR1cm4gdGhpcy5oYW5kbGVFcnJvcihlcnIpO1xuICAgICAgfSksXG4gICAgKTtcbiAgfVxufVxuIl19