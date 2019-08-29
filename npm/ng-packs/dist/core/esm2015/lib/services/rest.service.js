/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { throwError } from 'rxjs';
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
        return throwError(err);
    }
    /**
     * @template T, R
     * @param {?} request
     * @param {?=} config
     * @param {?=} api
     * @return {?}
     */
    request(request, config = {}, api) {
        const { observe = "body" /* Body */, skipHandleError } = config;
        /** @type {?} */
        const url = api || this.store.selectSnapshot(ConfigState.getApiUrl()) + request.url;
        const { method } = request, options = tslib_1.__rest(request, ["method"]);
        return this.http.request(method, url, (/** @type {?} */ (Object.assign({ observe }, options)))).pipe(observe === "body" /* Body */ ? take(1) : null, catchError((/**
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmVzdC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3Jlc3Quc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQWUsTUFBTSxzQkFBc0IsQ0FBQztBQUMvRCxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFxQixVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDckQsT0FBTyxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUVsRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sV0FBVyxDQUFDO0FBQ3hDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxZQUFZLENBQUM7Ozs7QUFLNUMsTUFBTSxPQUFPLFdBQVc7Ozs7O0lBQ3RCLFlBQW9CLElBQWdCLEVBQVUsS0FBWTtRQUF0QyxTQUFJLEdBQUosSUFBSSxDQUFZO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7O0lBRTlELFdBQVcsQ0FBQyxHQUFRO1FBQ2xCLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksY0FBYyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7UUFDN0MsT0FBTyxDQUFDLEtBQUssQ0FBQyxHQUFHLENBQUMsQ0FBQztRQUNuQixPQUFPLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQztJQUN6QixDQUFDOzs7Ozs7OztJQUVELE9BQU8sQ0FBTyxPQUF5QyxFQUFFLFNBQXNCLEVBQUUsRUFBRSxHQUFZO2NBQ3ZGLEVBQUUsT0FBTyxvQkFBb0IsRUFBRSxlQUFlLEVBQUUsR0FBRyxNQUFNOztjQUN6RCxHQUFHLEdBQUcsR0FBRyxJQUFJLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxTQUFTLEVBQUUsQ0FBQyxHQUFHLE9BQU8sQ0FBQyxHQUFHO2NBQzdFLEVBQUUsTUFBTSxLQUFpQixPQUFPLEVBQXRCLDZDQUFVO1FBQzFCLE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQUksTUFBTSxFQUFFLEdBQUcsRUFBRSxtQ0FBRSxPQUFPLElBQUssT0FBTyxHQUFTLENBQUMsQ0FBQyxJQUFJLENBQzNFLE9BQU8sc0JBQXNCLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsSUFBSSxFQUM5QyxVQUFVOzs7O1FBQUMsR0FBRyxDQUFDLEVBQUU7WUFDZixJQUFJLGVBQWUsRUFBRTtnQkFDbkIsT0FBTyxVQUFVLENBQUMsR0FBRyxDQUFDLENBQUM7YUFDeEI7WUFFRCxPQUFPLElBQUksQ0FBQyxXQUFXLENBQUMsR0FBRyxDQUFDLENBQUM7UUFDL0IsQ0FBQyxFQUFDLENBQ0gsQ0FBQztJQUNKLENBQUM7OztZQTFCRixVQUFVLFNBQUM7Z0JBQ1YsVUFBVSxFQUFFLE1BQU07YUFDbkI7Ozs7WUFYUSxVQUFVO1lBRVYsS0FBSzs7Ozs7Ozs7SUFXQSwyQkFBd0I7Ozs7O0lBQUUsNEJBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSHR0cENsaWVudCwgSHR0cFJlcXVlc3QgfSBmcm9tICdAYW5ndWxhci9jb21tb24vaHR0cCc7XG5pbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE5FVkVSLCBPYnNlcnZhYmxlLCB0aHJvd0Vycm9yIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBjYXRjaEVycm9yLCB0YWtlIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHsgUmVzdCB9IGZyb20gJy4uL21vZGVscy9yZXN0JztcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzJztcbmltcG9ydCB7IFJlc3RPY2N1ckVycm9yIH0gZnJvbSAnLi4vYWN0aW9ucyc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBSZXN0U2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgaHR0cDogSHR0cENsaWVudCwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgaGFuZGxlRXJyb3IoZXJyOiBhbnkpOiBPYnNlcnZhYmxlPGFueT4ge1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFJlc3RPY2N1ckVycm9yKGVycikpO1xuICAgIGNvbnNvbGUuZXJyb3IoZXJyKTtcbiAgICByZXR1cm4gdGhyb3dFcnJvcihlcnIpO1xuICB9XG5cbiAgcmVxdWVzdDxULCBSPihyZXF1ZXN0OiBIdHRwUmVxdWVzdDxUPiB8IFJlc3QuUmVxdWVzdDxUPiwgY29uZmlnOiBSZXN0LkNvbmZpZyA9IHt9LCBhcGk/OiBzdHJpbmcpOiBPYnNlcnZhYmxlPFI+IHtcbiAgICBjb25zdCB7IG9ic2VydmUgPSBSZXN0Lk9ic2VydmUuQm9keSwgc2tpcEhhbmRsZUVycm9yIH0gPSBjb25maWc7XG4gICAgY29uc3QgdXJsID0gYXBpIHx8IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0QXBpVXJsKCkpICsgcmVxdWVzdC51cmw7XG4gICAgY29uc3QgeyBtZXRob2QsIC4uLm9wdGlvbnMgfSA9IHJlcXVlc3Q7XG4gICAgcmV0dXJuIHRoaXMuaHR0cC5yZXF1ZXN0PFQ+KG1ldGhvZCwgdXJsLCB7IG9ic2VydmUsIC4uLm9wdGlvbnMgfSBhcyBhbnkpLnBpcGUoXG4gICAgICBvYnNlcnZlID09PSBSZXN0Lk9ic2VydmUuQm9keSA/IHRha2UoMSkgOiBudWxsLFxuICAgICAgY2F0Y2hFcnJvcihlcnIgPT4ge1xuICAgICAgICBpZiAoc2tpcEhhbmRsZUVycm9yKSB7XG4gICAgICAgICAgcmV0dXJuIHRocm93RXJyb3IoZXJyKTtcbiAgICAgICAgfVxuXG4gICAgICAgIHJldHVybiB0aGlzLmhhbmRsZUVycm9yKGVycik7XG4gICAgICB9KSxcbiAgICApO1xuICB9XG59XG4iXX0=