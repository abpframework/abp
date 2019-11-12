/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { throwError } from 'rxjs';
import { catchError, take, tap } from 'rxjs/operators';
import { RestOccurError } from '../actions/rest.actions';
import { ConfigState } from '../states/config.state';
import * as i0 from '@angular/core';
import * as i1 from '@angular/common/http';
import * as i2 from '@ngxs/store';
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
    config = config || /** @type {?} */ ({});
    const { observe = 'body' /* Body */, skipHandleError } = config;
    /** @type {?} */
    const url = (api || this.store.selectSnapshot(ConfigState.getApiUrl())) + request.url;
    const { method } = request,
      options = tslib_1.__rest(request, ['method']);
    return this.http.request(method, url, /** @type {?} */ (Object.assign({ observe }, options))).pipe(
      observe === 'body' /* Body */ ? take(1) : tap(),
      catchError(
        /**
         * @param {?} err
         * @return {?}
         */
        err => {
          if (skipHandleError) {
            return throwError(err);
          }
          return this.handleError(err);
        },
      ),
    );
  }
}
RestService.decorators = [
  {
    type: Injectable,
    args: [
      {
        providedIn: 'root',
      },
    ],
  },
];
/** @nocollapse */
RestService.ctorParameters = () => [{ type: HttpClient }, { type: Store }];
/** @nocollapse */ RestService.ngInjectableDef = i0.ɵɵdefineInjectable({
  factory: function RestService_Factory() {
    return new RestService(i0.ɵɵinject(i1.HttpClient), i0.ɵɵinject(i2.Store));
  },
  token: RestService,
  providedIn: 'root',
});
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmVzdC5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3Jlc3Quc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQWUsTUFBTSxzQkFBc0IsQ0FBQztBQUMvRCxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFjLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUM5QyxPQUFPLEVBQUUsVUFBVSxFQUFFLElBQUksRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUN2RCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0seUJBQXlCLENBQUM7QUFFekQsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLHdCQUF3QixDQUFDOzs7O0FBS3JELE1BQU0sT0FBTyxXQUFXOzs7OztJQUN0QixZQUFvQixJQUFnQixFQUFVLEtBQVk7UUFBdEMsU0FBSSxHQUFKLElBQUksQ0FBWTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7OztJQUU5RCxXQUFXLENBQUMsR0FBUTtRQUNsQixJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLGNBQWMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO1FBQzdDLE9BQU8sQ0FBQyxLQUFLLENBQUMsR0FBRyxDQUFDLENBQUM7UUFDbkIsT0FBTyxVQUFVLENBQUMsR0FBRyxDQUFDLENBQUM7SUFDekIsQ0FBQzs7Ozs7Ozs7SUFFRCxPQUFPLENBQU8sT0FBeUMsRUFBRSxNQUFvQixFQUFFLEdBQVk7UUFDekYsTUFBTSxHQUFHLE1BQU0sSUFBSSxDQUFDLG1CQUFBLEVBQUUsRUFBZSxDQUFDLENBQUM7Y0FDakMsRUFBRSxPQUFPLG9CQUFvQixFQUFFLGVBQWUsRUFBRSxHQUFHLE1BQU07O2NBQ3pELEdBQUcsR0FBRyxDQUFDLEdBQUcsSUFBSSxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsU0FBUyxFQUFFLENBQUMsQ0FBQyxHQUFHLE9BQU8sQ0FBQyxHQUFHO2NBQy9FLEVBQUUsTUFBTSxLQUFpQixPQUFPLEVBQXRCLDZDQUFVO1FBRTFCLE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPLENBQUksTUFBTSxFQUFFLEdBQUcsRUFBRSxtQ0FBRSxPQUFPLElBQUssT0FBTyxHQUFTLENBQUMsQ0FBQyxJQUFJLENBQzNFLE9BQU8sc0JBQXNCLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsR0FBRyxFQUFFLEVBQy9DLFVBQVU7Ozs7UUFBQyxHQUFHLENBQUMsRUFBRTtZQUNmLElBQUksZUFBZSxFQUFFO2dCQUNuQixPQUFPLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQzthQUN4QjtZQUVELE9BQU8sSUFBSSxDQUFDLFdBQVcsQ0FBQyxHQUFHLENBQUMsQ0FBQztRQUMvQixDQUFDLEVBQUMsQ0FDSCxDQUFDO0lBQ0osQ0FBQzs7O1lBNUJGLFVBQVUsU0FBQztnQkFDVixVQUFVLEVBQUUsTUFBTTthQUNuQjs7OztZQVhRLFVBQVU7WUFFVixLQUFLOzs7Ozs7OztJQVdBLDJCQUF3Qjs7Ozs7SUFBRSw0QkFBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBIdHRwQ2xpZW50LCBIdHRwUmVxdWVzdCB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbi9odHRwJztcbmltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSwgdGhyb3dFcnJvciB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgY2F0Y2hFcnJvciwgdGFrZSwgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHsgUmVzdE9jY3VyRXJyb3IgfSBmcm9tICcuLi9hY3Rpb25zL3Jlc3QuYWN0aW9ucyc7XG5pbXBvcnQgeyBSZXN0IH0gZnJvbSAnLi4vbW9kZWxzL3Jlc3QnO1xuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMvY29uZmlnLnN0YXRlJztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIFJlc3RTZXJ2aWNlIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBodHRwOiBIdHRwQ2xpZW50LCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cblxuICBoYW5kbGVFcnJvcihlcnI6IGFueSk6IE9ic2VydmFibGU8YW55PiB7XG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgUmVzdE9jY3VyRXJyb3IoZXJyKSk7XG4gICAgY29uc29sZS5lcnJvcihlcnIpO1xuICAgIHJldHVybiB0aHJvd0Vycm9yKGVycik7XG4gIH1cblxuICByZXF1ZXN0PFQsIFI+KHJlcXVlc3Q6IEh0dHBSZXF1ZXN0PFQ+IHwgUmVzdC5SZXF1ZXN0PFQ+LCBjb25maWc/OiBSZXN0LkNvbmZpZywgYXBpPzogc3RyaW5nKTogT2JzZXJ2YWJsZTxSPiB7XG4gICAgY29uZmlnID0gY29uZmlnIHx8ICh7fSBhcyBSZXN0LkNvbmZpZyk7XG4gICAgY29uc3QgeyBvYnNlcnZlID0gUmVzdC5PYnNlcnZlLkJvZHksIHNraXBIYW5kbGVFcnJvciB9ID0gY29uZmlnO1xuICAgIGNvbnN0IHVybCA9IChhcGkgfHwgdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBcGlVcmwoKSkpICsgcmVxdWVzdC51cmw7XG4gICAgY29uc3QgeyBtZXRob2QsIC4uLm9wdGlvbnMgfSA9IHJlcXVlc3Q7XG5cbiAgICByZXR1cm4gdGhpcy5odHRwLnJlcXVlc3Q8VD4obWV0aG9kLCB1cmwsIHsgb2JzZXJ2ZSwgLi4ub3B0aW9ucyB9IGFzIGFueSkucGlwZShcbiAgICAgIG9ic2VydmUgPT09IFJlc3QuT2JzZXJ2ZS5Cb2R5ID8gdGFrZSgxKSA6IHRhcCgpLFxuICAgICAgY2F0Y2hFcnJvcihlcnIgPT4ge1xuICAgICAgICBpZiAoc2tpcEhhbmRsZUVycm9yKSB7XG4gICAgICAgICAgcmV0dXJuIHRocm93RXJyb3IoZXJyKTtcbiAgICAgICAgfVxuXG4gICAgICAgIHJldHVybiB0aGlzLmhhbmRsZUVycm9yKGVycik7XG4gICAgICB9KSxcbiAgICApO1xuICB9XG59XG4iXX0=
