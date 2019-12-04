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
        return throwError(err);
    }
    /**
     * @template T, R
     * @param {?} request
     * @param {?=} config
     * @param {?=} api
     * @return {?}
     */
    request(request, config, api) {
        config = config || ((/** @type {?} */ ({})));
        const { observe = "body" /* Body */, skipHandleError } = config;
        /** @type {?} */
        const url = (api || this.store.selectSnapshot(ConfigState.getApiUrl())) + request.url;
        const { method, params } = request, options = tslib_1.__rest(request, ["method", "params"]);
        return this.http
            .request(method, url, (/** @type {?} */ (Object.assign({ observe }, (params && {
            params: Object.keys(params).reduce((/**
             * @param {?} acc
             * @param {?} key
             * @return {?}
             */
            (acc, key) => (Object.assign({}, acc, (typeof params[key] !== 'undefined' && params[key] !== '' && { [key]: params[key] })))), {}),
        }), options))))
            .pipe(observe === "body" /* Body */ ? take(1) : tap(), catchError((/**
         * @param {?} err
         * @return {?}
         */
        err => {
            if (skipHandleError) {
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmVzdC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3Jlc3Quc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFlLE1BQU0sc0JBQXNCLENBQUM7QUFDL0QsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBYyxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDOUMsT0FBTyxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdkQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHlCQUF5QixDQUFDO0FBRXpELE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSx3QkFBd0IsQ0FBQzs7OztBQUtyRCxNQUFNLE9BQU8sV0FBVzs7Ozs7SUFDdEIsWUFBb0IsSUFBZ0IsRUFBVSxLQUFZO1FBQXRDLFNBQUksR0FBSixJQUFJLENBQVk7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQzs7Ozs7SUFFOUQsV0FBVyxDQUFDLEdBQVE7UUFDbEIsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxjQUFjLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQztRQUM3QyxPQUFPLENBQUMsS0FBSyxDQUFDLEdBQUcsQ0FBQyxDQUFDO1FBQ25CLE9BQU8sVUFBVSxDQUFDLEdBQUcsQ0FBQyxDQUFDO0lBQ3pCLENBQUM7Ozs7Ozs7O0lBRUQsT0FBTyxDQUFPLE9BQXlDLEVBQUUsTUFBb0IsRUFBRSxHQUFZO1FBQ3pGLE1BQU0sR0FBRyxNQUFNLElBQUksQ0FBQyxtQkFBQSxFQUFFLEVBQWUsQ0FBQyxDQUFDO2NBQ2pDLEVBQUUsT0FBTyxvQkFBb0IsRUFBRSxlQUFlLEVBQUUsR0FBRyxNQUFNOztjQUN6RCxHQUFHLEdBQUcsQ0FBQyxHQUFHLElBQUksSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLFNBQVMsRUFBRSxDQUFDLENBQUMsR0FBRyxPQUFPLENBQUMsR0FBRztjQUMvRSxFQUFFLE1BQU0sRUFBRSxNQUFNLEtBQWlCLE9BQU8sRUFBdEIsdURBQVU7UUFFbEMsT0FBTyxJQUFJLENBQUMsSUFBSTthQUNiLE9BQU8sQ0FBSSxNQUFNLEVBQUUsR0FBRyxFQUFFLG1DQUN2QixPQUFPLElBQ0osQ0FBQyxNQUFNLElBQUk7WUFDWixNQUFNLEVBQUUsTUFBTSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsQ0FBQyxNQUFNOzs7OztZQUNoQyxDQUFDLEdBQUcsRUFBRSxHQUFHLEVBQUUsRUFBRSxDQUFDLG1CQUNULEdBQUcsRUFDSCxDQUFDLE9BQU8sTUFBTSxDQUFDLEdBQUcsQ0FBQyxLQUFLLFdBQVcsSUFBSSxNQUFNLENBQUMsR0FBRyxDQUFDLEtBQUssRUFBRSxJQUFJLEVBQUUsQ0FBQyxHQUFHLENBQUMsRUFBRSxNQUFNLENBQUMsR0FBRyxDQUFDLEVBQUUsQ0FBQyxFQUN2RixHQUNGLEVBQUUsQ0FDSDtTQUNGLENBQUMsRUFDQyxPQUFPLEdBQ0osQ0FBQzthQUNSLElBQUksQ0FDSCxPQUFPLHNCQUFzQixDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUcsRUFBRSxFQUMvQyxVQUFVOzs7O1FBQUMsR0FBRyxDQUFDLEVBQUU7WUFDZixJQUFJLGVBQWUsRUFBRTtnQkFDbkIsT0FBTyxVQUFVLENBQUMsR0FBRyxDQUFDLENBQUM7YUFDeEI7WUFFRCxPQUFPLElBQUksQ0FBQyxXQUFXLENBQUMsR0FBRyxDQUFDLENBQUM7UUFDL0IsQ0FBQyxFQUFDLENBQ0gsQ0FBQztJQUNOLENBQUM7OztZQTFDRixVQUFVLFNBQUM7Z0JBQ1YsVUFBVSxFQUFFLE1BQU07YUFDbkI7Ozs7WUFYUSxVQUFVO1lBRVYsS0FBSzs7Ozs7Ozs7SUFXQSwyQkFBd0I7Ozs7O0lBQUUsNEJBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSHR0cENsaWVudCwgSHR0cFJlcXVlc3QgfSBmcm9tICdAYW5ndWxhci9jb21tb24vaHR0cCc7XG5pbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUsIHRocm93RXJyb3IgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGNhdGNoRXJyb3IsIHRha2UsIHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7IFJlc3RPY2N1ckVycm9yIH0gZnJvbSAnLi4vYWN0aW9ucy9yZXN0LmFjdGlvbnMnO1xuaW1wb3J0IHsgUmVzdCB9IGZyb20gJy4uL21vZGVscy9yZXN0JztcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL2NvbmZpZy5zdGF0ZSc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBSZXN0U2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgaHR0cDogSHR0cENsaWVudCwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgaGFuZGxlRXJyb3IoZXJyOiBhbnkpOiBPYnNlcnZhYmxlPGFueT4ge1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFJlc3RPY2N1ckVycm9yKGVycikpO1xuICAgIGNvbnNvbGUuZXJyb3IoZXJyKTtcbiAgICByZXR1cm4gdGhyb3dFcnJvcihlcnIpO1xuICB9XG5cbiAgcmVxdWVzdDxULCBSPihyZXF1ZXN0OiBIdHRwUmVxdWVzdDxUPiB8IFJlc3QuUmVxdWVzdDxUPiwgY29uZmlnPzogUmVzdC5Db25maWcsIGFwaT86IHN0cmluZyk6IE9ic2VydmFibGU8Uj4ge1xuICAgIGNvbmZpZyA9IGNvbmZpZyB8fCAoe30gYXMgUmVzdC5Db25maWcpO1xuICAgIGNvbnN0IHsgb2JzZXJ2ZSA9IFJlc3QuT2JzZXJ2ZS5Cb2R5LCBza2lwSGFuZGxlRXJyb3IgfSA9IGNvbmZpZztcbiAgICBjb25zdCB1cmwgPSAoYXBpIHx8IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0QXBpVXJsKCkpKSArIHJlcXVlc3QudXJsO1xuICAgIGNvbnN0IHsgbWV0aG9kLCBwYXJhbXMsIC4uLm9wdGlvbnMgfSA9IHJlcXVlc3Q7XG5cbiAgICByZXR1cm4gdGhpcy5odHRwXG4gICAgICAucmVxdWVzdDxUPihtZXRob2QsIHVybCwge1xuICAgICAgICBvYnNlcnZlLFxuICAgICAgICAuLi4ocGFyYW1zICYmIHtcbiAgICAgICAgICBwYXJhbXM6IE9iamVjdC5rZXlzKHBhcmFtcykucmVkdWNlKFxuICAgICAgICAgICAgKGFjYywga2V5KSA9PiAoe1xuICAgICAgICAgICAgICAuLi5hY2MsXG4gICAgICAgICAgICAgIC4uLih0eXBlb2YgcGFyYW1zW2tleV0gIT09ICd1bmRlZmluZWQnICYmIHBhcmFtc1trZXldICE9PSAnJyAmJiB7IFtrZXldOiBwYXJhbXNba2V5XSB9KSxcbiAgICAgICAgICAgIH0pLFxuICAgICAgICAgICAge30sXG4gICAgICAgICAgKSxcbiAgICAgICAgfSksXG4gICAgICAgIC4uLm9wdGlvbnMsXG4gICAgICB9IGFzIGFueSlcbiAgICAgIC5waXBlKFxuICAgICAgICBvYnNlcnZlID09PSBSZXN0Lk9ic2VydmUuQm9keSA/IHRha2UoMSkgOiB0YXAoKSxcbiAgICAgICAgY2F0Y2hFcnJvcihlcnIgPT4ge1xuICAgICAgICAgIGlmIChza2lwSGFuZGxlRXJyb3IpIHtcbiAgICAgICAgICAgIHJldHVybiB0aHJvd0Vycm9yKGVycik7XG4gICAgICAgICAgfVxuXG4gICAgICAgICAgcmV0dXJuIHRoaXMuaGFuZGxlRXJyb3IoZXJyKTtcbiAgICAgICAgfSksXG4gICAgICApO1xuICB9XG59XG4iXX0=