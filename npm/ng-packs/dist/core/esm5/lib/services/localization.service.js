/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { Injectable, NgZone, Optional, SkipSelf } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { noop } from 'rxjs';
import { ConfigState } from '../states/config.state';
import { registerLocale } from '../utils/initial-utils';
import * as i0 from '@angular/core';
import * as i1 from '@ngxs/store';
import * as i2 from '@angular/router';
var LocalizationService = /** @class */ (function() {
  function LocalizationService(store, router, ngZone, otherInstance) {
    this.store = store;
    this.router = router;
    this.ngZone = ngZone;
    if (otherInstance) throw new Error('LocaleService should have only one instance.');
  }
  Object.defineProperty(LocalizationService.prototype, 'currentLang', {
    /**
     * @return {?}
     */
    get: function() {
      return this.store.selectSnapshot(
        /**
         * @param {?} state
         * @return {?}
         */
        function(state) {
          return state.SessionState.language;
        },
      );
    },
    enumerable: true,
    configurable: true,
  });
  /**
   * @param {?} reuse
   * @return {?}
   */
  LocalizationService.prototype.setRouteReuse
  /**
   * @param {?} reuse
   * @return {?}
   */ = function(reuse) {
    this.router.routeReuseStrategy.shouldReuseRoute = reuse;
  };
  /**
   * @param {?} locale
   * @return {?}
   */
  LocalizationService.prototype.registerLocale
  /**
   * @param {?} locale
   * @return {?}
   */ = function(locale) {
    var _this = this;
    var shouldReuseRoute = this.router.routeReuseStrategy.shouldReuseRoute;
    this.setRouteReuse(
      /**
       * @return {?}
       */
      function() {
        return false;
      },
    );
    this.router.navigated = false;
    return registerLocale(locale).then(
      /**
       * @return {?}
       */
      function() {
        _this.ngZone.run(
          /**
           * @return {?}
           */
          function() {
            return tslib_1.__awaiter(_this, void 0, void 0, function() {
              return tslib_1.__generator(this, function(_a) {
                switch (_a.label) {
                  case 0:
                    return [4 /*yield*/, this.router.navigateByUrl(this.router.url).catch(noop)];
                  case 1:
                    _a.sent();
                    this.setRouteReuse(shouldReuseRoute);
                    return [2 /*return*/];
                }
              });
            });
          },
        );
      },
    );
  };
  /**
   * @param {?} key
   * @param {...?} interpolateParams
   * @return {?}
   */
  LocalizationService.prototype.get
  /**
   * @param {?} key
   * @param {...?} interpolateParams
   * @return {?}
   */ = function(key) {
    var interpolateParams = [];
    for (var _i = 1; _i < arguments.length; _i++) {
      interpolateParams[_i - 1] = arguments[_i];
    }
    return this.store.select(
      ConfigState.getLocalization.apply(ConfigState, tslib_1.__spread([key], interpolateParams)),
    );
  };
  /**
   * @param {?} key
   * @param {...?} interpolateParams
   * @return {?}
   */
  LocalizationService.prototype.instant
  /**
   * @param {?} key
   * @param {...?} interpolateParams
   * @return {?}
   */ = function(key) {
    var interpolateParams = [];
    for (var _i = 1; _i < arguments.length; _i++) {
      interpolateParams[_i - 1] = arguments[_i];
    }
    return this.store.selectSnapshot(
      ConfigState.getLocalization.apply(ConfigState, tslib_1.__spread([key], interpolateParams)),
    );
  };
  LocalizationService.decorators = [{ type: Injectable, args: [{ providedIn: 'root' }] }];
  /** @nocollapse */
  LocalizationService.ctorParameters = function() {
    return [
      { type: Store },
      { type: Router },
      { type: NgZone },
      { type: LocalizationService, decorators: [{ type: Optional }, { type: SkipSelf }] },
    ];
  };
  /** @nocollapse */ LocalizationService.ngInjectableDef = i0.ɵɵdefineInjectable({
    factory: function LocalizationService_Factory() {
      return new LocalizationService(
        i0.ɵɵinject(i1.Store),
        i0.ɵɵinject(i2.Router),
        i0.ɵɵinject(i0.NgZone),
        i0.ɵɵinject(LocalizationService, 12),
      );
    },
    token: LocalizationService,
    providedIn: 'root',
  });
  return LocalizationService;
})();
export { LocalizationService };
if (false) {
  /**
   * @type {?}
   * @private
   */
  LocalizationService.prototype.store;
  /**
   * @type {?}
   * @private
   */
  LocalizationService.prototype.router;
  /**
   * @type {?}
   * @private
   */
  LocalizationService.prototype.ngZone;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvbG9jYWxpemF0aW9uLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRSxRQUFRLEVBQUUsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3ZFLE9BQU8sRUFBMEIsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDakUsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsSUFBSSxFQUFjLE1BQU0sTUFBTSxDQUFDO0FBQ3hDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSx3QkFBd0IsQ0FBQztBQUNyRCxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sd0JBQXdCLENBQUM7Ozs7QUFJeEQ7SUFNRSw2QkFDVSxLQUFZLEVBQ1osTUFBYyxFQUNkLE1BQWMsRUFHdEIsYUFBa0M7UUFMMUIsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUNaLFdBQU0sR0FBTixNQUFNLENBQVE7UUFDZCxXQUFNLEdBQU4sTUFBTSxDQUFRO1FBS3RCLElBQUksYUFBYTtZQUFFLE1BQU0sSUFBSSxLQUFLLENBQUMsOENBQThDLENBQUMsQ0FBQztJQUNyRixDQUFDO0lBYkQsc0JBQUksNENBQVc7Ozs7UUFBZjtZQUNFLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjOzs7O1lBQUMsVUFBQSxLQUFLLElBQUksT0FBQSxLQUFLLENBQUMsWUFBWSxDQUFDLFFBQVEsRUFBM0IsQ0FBMkIsRUFBQyxDQUFDO1FBQ3pFLENBQUM7OztPQUFBOzs7OztJQWFELDJDQUFhOzs7O0lBQWIsVUFBYyxLQUF1QjtRQUNuQyxJQUFJLENBQUMsTUFBTSxDQUFDLGtCQUFrQixDQUFDLGdCQUFnQixHQUFHLEtBQUssQ0FBQztJQUMxRCxDQUFDOzs7OztJQUVELDRDQUFjOzs7O0lBQWQsVUFBZSxNQUFjO1FBQTdCLGlCQVdDO1FBVlMsSUFBQSxrRUFBZ0I7UUFDeEIsSUFBSSxDQUFDLGFBQWE7OztRQUFDLGNBQU0sT0FBQSxLQUFLLEVBQUwsQ0FBSyxFQUFDLENBQUM7UUFDaEMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxTQUFTLEdBQUcsS0FBSyxDQUFDO1FBRTlCLE9BQU8sY0FBYyxDQUFDLE1BQU0sQ0FBQyxDQUFDLElBQUk7OztRQUFDO1lBQ2pDLEtBQUksQ0FBQyxNQUFNLENBQUMsR0FBRzs7O1lBQUM7OztnQ0FDZCxxQkFBTSxJQUFJLENBQUMsTUFBTSxDQUFDLGFBQWEsQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLEdBQUcsQ0FBQyxDQUFDLEtBQUssQ0FBQyxJQUFJLENBQUMsRUFBQTs7NEJBQTVELFNBQTRELENBQUM7NEJBQzdELElBQUksQ0FBQyxhQUFhLENBQUMsZ0JBQWdCLENBQUMsQ0FBQzs7OztpQkFDdEMsRUFBQyxDQUFDO1FBQ0wsQ0FBQyxFQUFDLENBQUM7SUFDTCxDQUFDOzs7Ozs7SUFFRCxpQ0FBRzs7Ozs7SUFBSCxVQUFJLEdBQVc7UUFBRSwyQkFBOEI7YUFBOUIsVUFBOEIsRUFBOUIscUJBQThCLEVBQTlCLElBQThCO1lBQTlCLDBDQUE4Qjs7UUFDN0MsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLE1BQU0sQ0FBQyxXQUFXLENBQUMsZUFBZSxPQUEzQixXQUFXLG9CQUFpQixHQUFHLEdBQUssaUJBQWlCLEdBQUUsQ0FBQztJQUNuRixDQUFDOzs7Ozs7SUFFRCxxQ0FBTzs7Ozs7SUFBUCxVQUFRLEdBQVc7UUFBRSwyQkFBOEI7YUFBOUIsVUFBOEIsRUFBOUIscUJBQThCLEVBQTlCLElBQThCO1lBQTlCLDBDQUE4Qjs7UUFDakQsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsZUFBZSxPQUEzQixXQUFXLG9CQUFpQixHQUFHLEdBQUssaUJBQWlCLEdBQUUsQ0FBQztJQUMzRixDQUFDOztnQkF4Q0YsVUFBVSxTQUFDLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRTs7OztnQkFQekIsS0FBSztnQkFEbUIsTUFBTTtnQkFEbEIsTUFBTTtnQkFxQlIsbUJBQW1CLHVCQUZqQyxRQUFRLFlBQ1IsUUFBUTs7OzhCQXBCYjtDQWtEQyxBQXpDRCxJQXlDQztTQXhDWSxtQkFBbUI7Ozs7OztJQU01QixvQ0FBb0I7Ozs7O0lBQ3BCLHFDQUFzQjs7Ozs7SUFDdEIscUNBQXNCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSwgTmdab25lLCBPcHRpb25hbCwgU2tpcFNlbGYgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEFjdGl2YXRlZFJvdXRlU25hcHNob3QsIFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IG5vb3AsIE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL2NvbmZpZy5zdGF0ZSc7XG5pbXBvcnQgeyByZWdpc3RlckxvY2FsZSB9IGZyb20gJy4uL3V0aWxzL2luaXRpYWwtdXRpbHMnO1xuXG50eXBlIFNob3VsZFJldXNlUm91dGUgPSAoZnV0dXJlOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90LCBjdXJyOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90KSA9PiBib29sZWFuO1xuXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxuZXhwb3J0IGNsYXNzIExvY2FsaXphdGlvblNlcnZpY2Uge1xuICBnZXQgY3VycmVudExhbmcoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChzdGF0ZSA9PiBzdGF0ZS5TZXNzaW9uU3RhdGUubGFuZ3VhZ2UpO1xuICB9XG5cbiAgY29uc3RydWN0b3IoXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXG4gICAgcHJpdmF0ZSByb3V0ZXI6IFJvdXRlcixcbiAgICBwcml2YXRlIG5nWm9uZTogTmdab25lLFxuICAgIEBPcHRpb25hbCgpXG4gICAgQFNraXBTZWxmKClcbiAgICBvdGhlckluc3RhbmNlOiBMb2NhbGl6YXRpb25TZXJ2aWNlLFxuICApIHtcbiAgICBpZiAob3RoZXJJbnN0YW5jZSkgdGhyb3cgbmV3IEVycm9yKCdMb2NhbGVTZXJ2aWNlIHNob3VsZCBoYXZlIG9ubHkgb25lIGluc3RhbmNlLicpO1xuICB9XG5cbiAgc2V0Um91dGVSZXVzZShyZXVzZTogU2hvdWxkUmV1c2VSb3V0ZSkge1xuICAgIHRoaXMucm91dGVyLnJvdXRlUmV1c2VTdHJhdGVneS5zaG91bGRSZXVzZVJvdXRlID0gcmV1c2U7XG4gIH1cblxuICByZWdpc3RlckxvY2FsZShsb2NhbGU6IHN0cmluZykge1xuICAgIGNvbnN0IHsgc2hvdWxkUmV1c2VSb3V0ZSB9ID0gdGhpcy5yb3V0ZXIucm91dGVSZXVzZVN0cmF0ZWd5O1xuICAgIHRoaXMuc2V0Um91dGVSZXVzZSgoKSA9PiBmYWxzZSk7XG4gICAgdGhpcy5yb3V0ZXIubmF2aWdhdGVkID0gZmFsc2U7XG5cbiAgICByZXR1cm4gcmVnaXN0ZXJMb2NhbGUobG9jYWxlKS50aGVuKCgpID0+IHtcbiAgICAgIHRoaXMubmdab25lLnJ1bihhc3luYyAoKSA9PiB7XG4gICAgICAgIGF3YWl0IHRoaXMucm91dGVyLm5hdmlnYXRlQnlVcmwodGhpcy5yb3V0ZXIudXJsKS5jYXRjaChub29wKTtcbiAgICAgICAgdGhpcy5zZXRSb3V0ZVJldXNlKHNob3VsZFJldXNlUm91dGUpO1xuICAgICAgfSk7XG4gICAgfSk7XG4gIH1cblxuICBnZXQoa2V5OiBzdHJpbmcsIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXSk6IE9ic2VydmFibGU8c3RyaW5nPiB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0KENvbmZpZ1N0YXRlLmdldExvY2FsaXphdGlvbihrZXksIC4uLmludGVycG9sYXRlUGFyYW1zKSk7XG4gIH1cblxuICBpbnN0YW50KGtleTogc3RyaW5nLCAuLi5pbnRlcnBvbGF0ZVBhcmFtczogc3RyaW5nW10pOiBzdHJpbmcge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldExvY2FsaXphdGlvbihrZXksIC4uLmludGVycG9sYXRlUGFyYW1zKSk7XG4gIH1cbn1cbiJdfQ==
