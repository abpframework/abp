/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/localization.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Injectable, NgZone, Optional, SkipSelf } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { noop } from 'rxjs';
import { ConfigState } from '../states/config.state';
import { registerLocale } from '../utils/initial-utils';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
import * as i2 from "@angular/router";
var LocalizationService = /** @class */ (function () {
    function LocalizationService(store, router, ngZone, otherInstance) {
        this.store = store;
        this.router = router;
        this.ngZone = ngZone;
        if (otherInstance)
            throw new Error('LocalizationService should have only one instance.');
    }
    Object.defineProperty(LocalizationService.prototype, "currentLang", {
        get: /**
         * @return {?}
         */
        function () {
            return this.store.selectSnapshot((/**
             * @param {?} state
             * @return {?}
             */
            function (state) { return state.SessionState.language; }));
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @param {?} reuse
     * @return {?}
     */
    LocalizationService.prototype.setRouteReuse = /**
     * @param {?} reuse
     * @return {?}
     */
    function (reuse) {
        this.router.routeReuseStrategy.shouldReuseRoute = reuse;
    };
    /**
     * @param {?} locale
     * @return {?}
     */
    LocalizationService.prototype.registerLocale = /**
     * @param {?} locale
     * @return {?}
     */
    function (locale) {
        var _this = this;
        var shouldReuseRoute = this.router.routeReuseStrategy.shouldReuseRoute;
        this.setRouteReuse((/**
         * @return {?}
         */
        function () { return false; }));
        this.router.navigated = false;
        return registerLocale(locale).then((/**
         * @return {?}
         */
        function () {
            _this.ngZone.run((/**
             * @return {?}
             */
            function () { return tslib_1.__awaiter(_this, void 0, void 0, function () {
                return tslib_1.__generator(this, function (_a) {
                    switch (_a.label) {
                        case 0: return [4 /*yield*/, this.router.navigateByUrl(this.router.url).catch(noop)];
                        case 1:
                            _a.sent();
                            this.setRouteReuse(shouldReuseRoute);
                            return [2 /*return*/];
                    }
                });
            }); }));
        }));
    };
    /**
     * @param {?} key
     * @param {...?} interpolateParams
     * @return {?}
     */
    LocalizationService.prototype.get = /**
     * @param {?} key
     * @param {...?} interpolateParams
     * @return {?}
     */
    function (key) {
        var interpolateParams = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            interpolateParams[_i - 1] = arguments[_i];
        }
        return this.store.select(ConfigState.getLocalization.apply(ConfigState, tslib_1.__spread([key], interpolateParams)));
    };
    /**
     * @param {?} key
     * @param {...?} interpolateParams
     * @return {?}
     */
    LocalizationService.prototype.instant = /**
     * @param {?} key
     * @param {...?} interpolateParams
     * @return {?}
     */
    function (key) {
        var interpolateParams = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            interpolateParams[_i - 1] = arguments[_i];
        }
        return this.store.selectSnapshot(ConfigState.getLocalization.apply(ConfigState, tslib_1.__spread([key], interpolateParams)));
    };
    LocalizationService.decorators = [
        { type: Injectable, args: [{ providedIn: 'root' },] }
    ];
    /** @nocollapse */
    LocalizationService.ctorParameters = function () { return [
        { type: Store },
        { type: Router },
        { type: NgZone },
        { type: LocalizationService, decorators: [{ type: Optional }, { type: SkipSelf }] }
    ]; };
    /** @nocollapse */ LocalizationService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function LocalizationService_Factory() { return new LocalizationService(i0.ɵɵinject(i1.Store), i0.ɵɵinject(i2.Router), i0.ɵɵinject(i0.NgZone), i0.ɵɵinject(LocalizationService, 12)); }, token: LocalizationService, providedIn: "root" });
    return LocalizationService;
}());
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvbG9jYWxpemF0aW9uLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN2RSxPQUFPLEVBQTBCLE1BQU0sRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQ2pFLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLElBQUksRUFBYyxNQUFNLE1BQU0sQ0FBQztBQUN4QyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sd0JBQXdCLENBQUM7QUFDckQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHdCQUF3QixDQUFDOzs7O0FBS3hEO0lBTUUsNkJBQ1UsS0FBWSxFQUNaLE1BQWMsRUFDZCxNQUFjLEVBR3RCLGFBQWtDO1FBTDFCLFVBQUssR0FBTCxLQUFLLENBQU87UUFDWixXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQ2QsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUt0QixJQUFJLGFBQWE7WUFBRSxNQUFNLElBQUksS0FBSyxDQUFDLG9EQUFvRCxDQUFDLENBQUM7SUFDM0YsQ0FBQztJQWJELHNCQUFJLDRDQUFXOzs7O1FBQWY7WUFDRSxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYzs7OztZQUFDLFVBQUEsS0FBSyxJQUFJLE9BQUEsS0FBSyxDQUFDLFlBQVksQ0FBQyxRQUFRLEVBQTNCLENBQTJCLEVBQUMsQ0FBQztRQUN6RSxDQUFDOzs7T0FBQTs7Ozs7SUFhRCwyQ0FBYTs7OztJQUFiLFVBQWMsS0FBdUI7UUFDbkMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxrQkFBa0IsQ0FBQyxnQkFBZ0IsR0FBRyxLQUFLLENBQUM7SUFDMUQsQ0FBQzs7Ozs7SUFFRCw0Q0FBYzs7OztJQUFkLFVBQWUsTUFBYztRQUE3QixpQkFXQztRQVZTLElBQUEsa0VBQWdCO1FBQ3hCLElBQUksQ0FBQyxhQUFhOzs7UUFBQyxjQUFNLE9BQUEsS0FBSyxFQUFMLENBQUssRUFBQyxDQUFDO1FBQ2hDLElBQUksQ0FBQyxNQUFNLENBQUMsU0FBUyxHQUFHLEtBQUssQ0FBQztRQUU5QixPQUFPLGNBQWMsQ0FBQyxNQUFNLENBQUMsQ0FBQyxJQUFJOzs7UUFBQztZQUNqQyxLQUFJLENBQUMsTUFBTSxDQUFDLEdBQUc7OztZQUFDOzs7Z0NBQ2QscUJBQU0sSUFBSSxDQUFDLE1BQU0sQ0FBQyxhQUFhLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxHQUFHLENBQUMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEVBQUE7OzRCQUE1RCxTQUE0RCxDQUFDOzRCQUM3RCxJQUFJLENBQUMsYUFBYSxDQUFDLGdCQUFnQixDQUFDLENBQUM7Ozs7aUJBQ3RDLEVBQUMsQ0FBQztRQUNMLENBQUMsRUFBQyxDQUFDO0lBQ0wsQ0FBQzs7Ozs7O0lBRUQsaUNBQUc7Ozs7O0lBQUgsVUFDRSxHQUE0QztRQUM1QywyQkFBOEI7YUFBOUIsVUFBOEIsRUFBOUIscUJBQThCLEVBQTlCLElBQThCO1lBQTlCLDBDQUE4Qjs7UUFFOUIsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLE1BQU0sQ0FBQyxXQUFXLENBQUMsZUFBZSxPQUEzQixXQUFXLG9CQUFpQixHQUFHLEdBQUssaUJBQWlCLEdBQUUsQ0FBQztJQUNuRixDQUFDOzs7Ozs7SUFFRCxxQ0FBTzs7Ozs7SUFBUCxVQUFRLEdBQTRDO1FBQUUsMkJBQThCO2FBQTlCLFVBQThCLEVBQTlCLHFCQUE4QixFQUE5QixJQUE4QjtZQUE5QiwwQ0FBOEI7O1FBQ2xGLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLGVBQWUsT0FBM0IsV0FBVyxvQkFBaUIsR0FBRyxHQUFLLGlCQUFpQixHQUFFLENBQUM7SUFDM0YsQ0FBQzs7Z0JBM0NGLFVBQVUsU0FBQyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUU7Ozs7Z0JBUnpCLEtBQUs7Z0JBRG1CLE1BQU07Z0JBRGxCLE1BQU07Z0JBc0JSLG1CQUFtQix1QkFGakMsUUFBUSxZQUNSLFFBQVE7Ozs4QkFyQmI7Q0FzREMsQUE1Q0QsSUE0Q0M7U0EzQ1ksbUJBQW1COzs7Ozs7SUFNNUIsb0NBQW9COzs7OztJQUNwQixxQ0FBc0I7Ozs7O0lBQ3RCLHFDQUFzQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUsIE5nWm9uZSwgT3B0aW9uYWwsIFNraXBTZWxmIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90LCBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBub29wLCBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcy9jb25maWcuc3RhdGUnO1xuaW1wb3J0IHsgcmVnaXN0ZXJMb2NhbGUgfSBmcm9tICcuLi91dGlscy9pbml0aWFsLXV0aWxzJztcbmltcG9ydCB7IENvbmZpZyB9IGZyb20gJy4uL21vZGVscy9jb25maWcnO1xuXG50eXBlIFNob3VsZFJldXNlUm91dGUgPSAoZnV0dXJlOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90LCBjdXJyOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90KSA9PiBib29sZWFuO1xuXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxuZXhwb3J0IGNsYXNzIExvY2FsaXphdGlvblNlcnZpY2Uge1xuICBnZXQgY3VycmVudExhbmcoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChzdGF0ZSA9PiBzdGF0ZS5TZXNzaW9uU3RhdGUubGFuZ3VhZ2UpO1xuICB9XG5cbiAgY29uc3RydWN0b3IoXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXG4gICAgcHJpdmF0ZSByb3V0ZXI6IFJvdXRlcixcbiAgICBwcml2YXRlIG5nWm9uZTogTmdab25lLFxuICAgIEBPcHRpb25hbCgpXG4gICAgQFNraXBTZWxmKClcbiAgICBvdGhlckluc3RhbmNlOiBMb2NhbGl6YXRpb25TZXJ2aWNlLFxuICApIHtcbiAgICBpZiAob3RoZXJJbnN0YW5jZSkgdGhyb3cgbmV3IEVycm9yKCdMb2NhbGl6YXRpb25TZXJ2aWNlIHNob3VsZCBoYXZlIG9ubHkgb25lIGluc3RhbmNlLicpO1xuICB9XG5cbiAgc2V0Um91dGVSZXVzZShyZXVzZTogU2hvdWxkUmV1c2VSb3V0ZSkge1xuICAgIHRoaXMucm91dGVyLnJvdXRlUmV1c2VTdHJhdGVneS5zaG91bGRSZXVzZVJvdXRlID0gcmV1c2U7XG4gIH1cblxuICByZWdpc3RlckxvY2FsZShsb2NhbGU6IHN0cmluZykge1xuICAgIGNvbnN0IHsgc2hvdWxkUmV1c2VSb3V0ZSB9ID0gdGhpcy5yb3V0ZXIucm91dGVSZXVzZVN0cmF0ZWd5O1xuICAgIHRoaXMuc2V0Um91dGVSZXVzZSgoKSA9PiBmYWxzZSk7XG4gICAgdGhpcy5yb3V0ZXIubmF2aWdhdGVkID0gZmFsc2U7XG5cbiAgICByZXR1cm4gcmVnaXN0ZXJMb2NhbGUobG9jYWxlKS50aGVuKCgpID0+IHtcbiAgICAgIHRoaXMubmdab25lLnJ1bihhc3luYyAoKSA9PiB7XG4gICAgICAgIGF3YWl0IHRoaXMucm91dGVyLm5hdmlnYXRlQnlVcmwodGhpcy5yb3V0ZXIudXJsKS5jYXRjaChub29wKTtcbiAgICAgICAgdGhpcy5zZXRSb3V0ZVJldXNlKHNob3VsZFJldXNlUm91dGUpO1xuICAgICAgfSk7XG4gICAgfSk7XG4gIH1cblxuICBnZXQoXG4gICAga2V5OiBzdHJpbmcgfCBDb25maWcuTG9jYWxpemF0aW9uV2l0aERlZmF1bHQsXG4gICAgLi4uaW50ZXJwb2xhdGVQYXJhbXM6IHN0cmluZ1tdXG4gICk6IE9ic2VydmFibGU8c3RyaW5nPiB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0KENvbmZpZ1N0YXRlLmdldExvY2FsaXphdGlvbihrZXksIC4uLmludGVycG9sYXRlUGFyYW1zKSk7XG4gIH1cblxuICBpbnN0YW50KGtleTogc3RyaW5nIHwgQ29uZmlnLkxvY2FsaXphdGlvbldpdGhEZWZhdWx0LCAuLi5pbnRlcnBvbGF0ZVBhcmFtczogc3RyaW5nW10pOiBzdHJpbmcge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldExvY2FsaXphdGlvbihrZXksIC4uLmludGVycG9sYXRlUGFyYW1zKSk7XG4gIH1cbn1cbiJdfQ==