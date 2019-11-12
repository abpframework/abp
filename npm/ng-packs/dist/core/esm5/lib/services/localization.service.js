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
            throw new Error('LocaleService should have only one instance.');
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvbG9jYWxpemF0aW9uLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN2RSxPQUFPLEVBQTBCLE1BQU0sRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQ2pFLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLElBQUksRUFBYyxNQUFNLE1BQU0sQ0FBQztBQUN4QyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sd0JBQXdCLENBQUM7QUFDckQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHdCQUF3QixDQUFDOzs7O0FBSXhEO0lBTUUsNkJBQ1UsS0FBWSxFQUNaLE1BQWMsRUFDZCxNQUFjLEVBR3RCLGFBQWtDO1FBTDFCLFVBQUssR0FBTCxLQUFLLENBQU87UUFDWixXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQ2QsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUt0QixJQUFJLGFBQWE7WUFBRSxNQUFNLElBQUksS0FBSyxDQUFDLDhDQUE4QyxDQUFDLENBQUM7SUFDckYsQ0FBQztJQWJELHNCQUFJLDRDQUFXOzs7O1FBQWY7WUFDRSxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYzs7OztZQUFDLFVBQUEsS0FBSyxJQUFJLE9BQUEsS0FBSyxDQUFDLFlBQVksQ0FBQyxRQUFRLEVBQTNCLENBQTJCLEVBQUMsQ0FBQztRQUN6RSxDQUFDOzs7T0FBQTs7Ozs7SUFhRCwyQ0FBYTs7OztJQUFiLFVBQWMsS0FBdUI7UUFDbkMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxrQkFBa0IsQ0FBQyxnQkFBZ0IsR0FBRyxLQUFLLENBQUM7SUFDMUQsQ0FBQzs7Ozs7SUFFRCw0Q0FBYzs7OztJQUFkLFVBQWUsTUFBYztRQUE3QixpQkFXQztRQVZTLElBQUEsa0VBQWdCO1FBQ3hCLElBQUksQ0FBQyxhQUFhOzs7UUFBQyxjQUFNLE9BQUEsS0FBSyxFQUFMLENBQUssRUFBQyxDQUFDO1FBQ2hDLElBQUksQ0FBQyxNQUFNLENBQUMsU0FBUyxHQUFHLEtBQUssQ0FBQztRQUU5QixPQUFPLGNBQWMsQ0FBQyxNQUFNLENBQUMsQ0FBQyxJQUFJOzs7UUFBQztZQUNqQyxLQUFJLENBQUMsTUFBTSxDQUFDLEdBQUc7OztZQUFDOzs7Z0NBQ2QscUJBQU0sSUFBSSxDQUFDLE1BQU0sQ0FBQyxhQUFhLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxHQUFHLENBQUMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEVBQUE7OzRCQUE1RCxTQUE0RCxDQUFDOzRCQUM3RCxJQUFJLENBQUMsYUFBYSxDQUFDLGdCQUFnQixDQUFDLENBQUM7Ozs7aUJBQ3RDLEVBQUMsQ0FBQztRQUNMLENBQUMsRUFBQyxDQUFDO0lBQ0wsQ0FBQzs7Ozs7O0lBRUQsaUNBQUc7Ozs7O0lBQUgsVUFBSSxHQUFXO1FBQUUsMkJBQThCO2FBQTlCLFVBQThCLEVBQTlCLHFCQUE4QixFQUE5QixJQUE4QjtZQUE5QiwwQ0FBOEI7O1FBQzdDLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxNQUFNLENBQUMsV0FBVyxDQUFDLGVBQWUsT0FBM0IsV0FBVyxvQkFBaUIsR0FBRyxHQUFLLGlCQUFpQixHQUFFLENBQUM7SUFDbkYsQ0FBQzs7Ozs7O0lBRUQscUNBQU87Ozs7O0lBQVAsVUFBUSxHQUFXO1FBQUUsMkJBQThCO2FBQTlCLFVBQThCLEVBQTlCLHFCQUE4QixFQUE5QixJQUE4QjtZQUE5QiwwQ0FBOEI7O1FBQ2pELE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLGVBQWUsT0FBM0IsV0FBVyxvQkFBaUIsR0FBRyxHQUFLLGlCQUFpQixHQUFFLENBQUM7SUFDM0YsQ0FBQzs7Z0JBeENGLFVBQVUsU0FBQyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUU7Ozs7Z0JBUHpCLEtBQUs7Z0JBRG1CLE1BQU07Z0JBRGxCLE1BQU07Z0JBcUJSLG1CQUFtQix1QkFGakMsUUFBUSxZQUNSLFFBQVE7Ozs4QkFwQmI7Q0FrREMsQUF6Q0QsSUF5Q0M7U0F4Q1ksbUJBQW1COzs7Ozs7SUFNNUIsb0NBQW9COzs7OztJQUNwQixxQ0FBc0I7Ozs7O0lBQ3RCLHFDQUFzQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUsIE5nWm9uZSwgT3B0aW9uYWwsIFNraXBTZWxmIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IEFjdGl2YXRlZFJvdXRlU25hcHNob3QsIFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XHJcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBub29wLCBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL2NvbmZpZy5zdGF0ZSc7XHJcbmltcG9ydCB7IHJlZ2lzdGVyTG9jYWxlIH0gZnJvbSAnLi4vdXRpbHMvaW5pdGlhbC11dGlscyc7XHJcblxyXG50eXBlIFNob3VsZFJldXNlUm91dGUgPSAoZnV0dXJlOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90LCBjdXJyOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90KSA9PiBib29sZWFuO1xyXG5cclxuQEluamVjdGFibGUoeyBwcm92aWRlZEluOiAncm9vdCcgfSlcclxuZXhwb3J0IGNsYXNzIExvY2FsaXphdGlvblNlcnZpY2Uge1xyXG4gIGdldCBjdXJyZW50TGFuZygpOiBzdHJpbmcge1xyXG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3Qoc3RhdGUgPT4gc3RhdGUuU2Vzc2lvblN0YXRlLmxhbmd1YWdlKTtcclxuICB9XHJcblxyXG4gIGNvbnN0cnVjdG9yKFxyXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXHJcbiAgICBwcml2YXRlIHJvdXRlcjogUm91dGVyLFxyXG4gICAgcHJpdmF0ZSBuZ1pvbmU6IE5nWm9uZSxcclxuICAgIEBPcHRpb25hbCgpXHJcbiAgICBAU2tpcFNlbGYoKVxyXG4gICAgb3RoZXJJbnN0YW5jZTogTG9jYWxpemF0aW9uU2VydmljZSxcclxuICApIHtcclxuICAgIGlmIChvdGhlckluc3RhbmNlKSB0aHJvdyBuZXcgRXJyb3IoJ0xvY2FsZVNlcnZpY2Ugc2hvdWxkIGhhdmUgb25seSBvbmUgaW5zdGFuY2UuJyk7XHJcbiAgfVxyXG5cclxuICBzZXRSb3V0ZVJldXNlKHJldXNlOiBTaG91bGRSZXVzZVJvdXRlKSB7XHJcbiAgICB0aGlzLnJvdXRlci5yb3V0ZVJldXNlU3RyYXRlZ3kuc2hvdWxkUmV1c2VSb3V0ZSA9IHJldXNlO1xyXG4gIH1cclxuXHJcbiAgcmVnaXN0ZXJMb2NhbGUobG9jYWxlOiBzdHJpbmcpIHtcclxuICAgIGNvbnN0IHsgc2hvdWxkUmV1c2VSb3V0ZSB9ID0gdGhpcy5yb3V0ZXIucm91dGVSZXVzZVN0cmF0ZWd5O1xyXG4gICAgdGhpcy5zZXRSb3V0ZVJldXNlKCgpID0+IGZhbHNlKTtcclxuICAgIHRoaXMucm91dGVyLm5hdmlnYXRlZCA9IGZhbHNlO1xyXG5cclxuICAgIHJldHVybiByZWdpc3RlckxvY2FsZShsb2NhbGUpLnRoZW4oKCkgPT4ge1xyXG4gICAgICB0aGlzLm5nWm9uZS5ydW4oYXN5bmMgKCkgPT4ge1xyXG4gICAgICAgIGF3YWl0IHRoaXMucm91dGVyLm5hdmlnYXRlQnlVcmwodGhpcy5yb3V0ZXIudXJsKS5jYXRjaChub29wKTtcclxuICAgICAgICB0aGlzLnNldFJvdXRlUmV1c2Uoc2hvdWxkUmV1c2VSb3V0ZSk7XHJcbiAgICAgIH0pO1xyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBnZXQoa2V5OiBzdHJpbmcsIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXSk6IE9ic2VydmFibGU8c3RyaW5nPiB7XHJcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3QoQ29uZmlnU3RhdGUuZ2V0TG9jYWxpemF0aW9uKGtleSwgLi4uaW50ZXJwb2xhdGVQYXJhbXMpKTtcclxuICB9XHJcblxyXG4gIGluc3RhbnQoa2V5OiBzdHJpbmcsIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXSk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRMb2NhbGl6YXRpb24oa2V5LCAuLi5pbnRlcnBvbGF0ZVBhcmFtcykpO1xyXG4gIH1cclxufVxyXG4iXX0=