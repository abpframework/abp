/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Injectable, Optional, SkipSelf } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, Store } from '@ngxs/store';
import { noop } from 'rxjs';
import { ConfigState } from '../states/config.state';
import { SessionState } from '../states/session.state';
import { registerLocale } from '../utils/initial-utils';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
import * as i2 from "@angular/router";
var LocalizationService = /** @class */ (function () {
    function LocalizationService(store, router, actions, otherInstance) {
        this.store = store;
        this.router = router;
        this.actions = actions;
        if (otherInstance)
            throw new Error('LocaleService should have only one instance.');
    }
    Object.defineProperty(LocalizationService.prototype, "currentLang", {
        get: /**
         * @return {?}
         */
        function () {
            return this.store.selectSnapshot(SessionState.getLanguage);
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @private
     * @param {?} reuse
     * @return {?}
     */
    LocalizationService.prototype.setRouteReuse = /**
     * @private
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
    };
    /**
     * @param {?} keys
     * @param {...?} interpolateParams
     * @return {?}
     */
    LocalizationService.prototype.get = /**
     * @param {?} keys
     * @param {...?} interpolateParams
     * @return {?}
     */
    function (keys) {
        var interpolateParams = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            interpolateParams[_i - 1] = arguments[_i];
        }
        return this.store.select(ConfigState.getCopy.apply(ConfigState, tslib_1.__spread([keys], interpolateParams)));
    };
    /**
     * @param {?} keys
     * @param {...?} interpolateParams
     * @return {?}
     */
    LocalizationService.prototype.instant = /**
     * @param {?} keys
     * @param {...?} interpolateParams
     * @return {?}
     */
    function (keys) {
        var interpolateParams = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            interpolateParams[_i - 1] = arguments[_i];
        }
        return this.store.selectSnapshot(ConfigState.getCopy.apply(ConfigState, tslib_1.__spread([keys], interpolateParams)));
    };
    LocalizationService.decorators = [
        { type: Injectable, args: [{ providedIn: 'root' },] }
    ];
    /** @nocollapse */
    LocalizationService.ctorParameters = function () { return [
        { type: Store },
        { type: Router },
        { type: Actions },
        { type: LocalizationService, decorators: [{ type: Optional }, { type: SkipSelf }] }
    ]; };
    /** @nocollapse */ LocalizationService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function LocalizationService_Factory() { return new LocalizationService(i0.ɵɵinject(i1.Store), i0.ɵɵinject(i2.Router), i0.ɵɵinject(i1.Actions), i0.ɵɵinject(LocalizationService, 12)); }, token: LocalizationService, providedIn: "root" });
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
    LocalizationService.prototype.actions;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvbG9jYWxpemF0aW9uLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLFFBQVEsRUFBRSxRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDL0QsT0FBTyxFQUEwQixNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUNqRSxPQUFPLEVBQUUsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM3QyxPQUFPLEVBQUUsSUFBSSxFQUFjLE1BQU0sTUFBTSxDQUFDO0FBQ3hDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSx3QkFBd0IsQ0FBQztBQUNyRCxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0seUJBQXlCLENBQUM7QUFDdkQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHdCQUF3QixDQUFDOzs7O0FBSXhEO0lBTUUsNkJBQ1UsS0FBWSxFQUNaLE1BQWMsRUFDZCxPQUFnQixFQUd4QixhQUFrQztRQUwxQixVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQ1osV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUNkLFlBQU8sR0FBUCxPQUFPLENBQVM7UUFLeEIsSUFBSSxhQUFhO1lBQUUsTUFBTSxJQUFJLEtBQUssQ0FBQyw4Q0FBOEMsQ0FBQyxDQUFDO0lBQ3JGLENBQUM7SUFiRCxzQkFBSSw0Q0FBVzs7OztRQUFmO1lBQ0UsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxZQUFZLENBQUMsV0FBVyxDQUFDLENBQUM7UUFDN0QsQ0FBQzs7O09BQUE7Ozs7OztJQWFPLDJDQUFhOzs7OztJQUFyQixVQUFzQixLQUF1QjtRQUMzQyxJQUFJLENBQUMsTUFBTSxDQUFDLGtCQUFrQixDQUFDLGdCQUFnQixHQUFHLEtBQUssQ0FBQztJQUMxRCxDQUFDOzs7OztJQUVELDRDQUFjOzs7O0lBQWQsVUFBZSxNQUFjO1FBQTdCLGlCQVVDO1FBVFMsSUFBQSxrRUFBZ0I7UUFFeEIsSUFBSSxDQUFDLGFBQWE7OztRQUFDLGNBQU0sT0FBQSxLQUFLLEVBQUwsQ0FBSyxFQUFDLENBQUM7UUFDaEMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxTQUFTLEdBQUcsS0FBSyxDQUFDO1FBRTlCLE9BQU8sY0FBYyxDQUFDLE1BQU0sQ0FBQyxDQUFDLElBQUk7OztRQUFDOzs7NEJBQ2pDLHFCQUFNLElBQUksQ0FBQyxNQUFNLENBQUMsYUFBYSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsR0FBRyxDQUFDLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxFQUFBOzt3QkFBNUQsU0FBNEQsQ0FBQzt3QkFDN0QsSUFBSSxDQUFDLGFBQWEsQ0FBQyxnQkFBZ0IsQ0FBQyxDQUFDOzs7O2FBQ3RDLEVBQUMsQ0FBQztJQUNMLENBQUM7Ozs7OztJQUVELGlDQUFHOzs7OztJQUFILFVBQUksSUFBWTtRQUFFLDJCQUE4QjthQUE5QixVQUE4QixFQUE5QixxQkFBOEIsRUFBOUIsSUFBOEI7WUFBOUIsMENBQThCOztRQUM5QyxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsTUFBTSxDQUFDLFdBQVcsQ0FBQyxPQUFPLE9BQW5CLFdBQVcsb0JBQVMsSUFBSSxHQUFLLGlCQUFpQixHQUFFLENBQUM7SUFDNUUsQ0FBQzs7Ozs7O0lBRUQscUNBQU87Ozs7O0lBQVAsVUFBUSxJQUFZO1FBQUUsMkJBQThCO2FBQTlCLFVBQThCLEVBQTlCLHFCQUE4QixFQUE5QixJQUE4QjtZQUE5QiwwQ0FBOEI7O1FBQ2xELE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLE9BQU8sT0FBbkIsV0FBVyxvQkFBUyxJQUFJLEdBQUssaUJBQWlCLEdBQUUsQ0FBQztJQUNwRixDQUFDOztnQkF2Q0YsVUFBVSxTQUFDLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRTs7OztnQkFSaEIsS0FBSztnQkFEVSxNQUFNO2dCQUM5QixPQUFPO2dCQW9CRyxtQkFBbUIsdUJBRmpDLFFBQVEsWUFDUixRQUFROzs7OEJBckJiO0NBa0RDLEFBeENELElBd0NDO1NBdkNZLG1CQUFtQjs7Ozs7O0lBTTVCLG9DQUFvQjs7Ozs7SUFDcEIscUNBQXNCOzs7OztJQUN0QixzQ0FBd0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlLCBPcHRpb25hbCwgU2tpcFNlbGYgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEFjdGl2YXRlZFJvdXRlU25hcHNob3QsIFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBBY3Rpb25zLCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IG5vb3AsIE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL2NvbmZpZy5zdGF0ZSc7XG5pbXBvcnQgeyBTZXNzaW9uU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMvc2Vzc2lvbi5zdGF0ZSc7XG5pbXBvcnQgeyByZWdpc3RlckxvY2FsZSB9IGZyb20gJy4uL3V0aWxzL2luaXRpYWwtdXRpbHMnO1xuXG50eXBlIFNob3VsZFJldXNlUm91dGUgPSAoZnV0dXJlOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90LCBjdXJyOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90KSA9PiBib29sZWFuO1xuXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxuZXhwb3J0IGNsYXNzIExvY2FsaXphdGlvblNlcnZpY2Uge1xuICBnZXQgY3VycmVudExhbmcoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXNzaW9uU3RhdGUuZ2V0TGFuZ3VhZ2UpO1xuICB9XG5cbiAgY29uc3RydWN0b3IoXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXG4gICAgcHJpdmF0ZSByb3V0ZXI6IFJvdXRlcixcbiAgICBwcml2YXRlIGFjdGlvbnM6IEFjdGlvbnMsXG4gICAgQE9wdGlvbmFsKClcbiAgICBAU2tpcFNlbGYoKVxuICAgIG90aGVySW5zdGFuY2U6IExvY2FsaXphdGlvblNlcnZpY2UsXG4gICkge1xuICAgIGlmIChvdGhlckluc3RhbmNlKSB0aHJvdyBuZXcgRXJyb3IoJ0xvY2FsZVNlcnZpY2Ugc2hvdWxkIGhhdmUgb25seSBvbmUgaW5zdGFuY2UuJyk7XG4gIH1cblxuICBwcml2YXRlIHNldFJvdXRlUmV1c2UocmV1c2U6IFNob3VsZFJldXNlUm91dGUpIHtcbiAgICB0aGlzLnJvdXRlci5yb3V0ZVJldXNlU3RyYXRlZ3kuc2hvdWxkUmV1c2VSb3V0ZSA9IHJldXNlO1xuICB9XG5cbiAgcmVnaXN0ZXJMb2NhbGUobG9jYWxlOiBzdHJpbmcpIHtcbiAgICBjb25zdCB7IHNob3VsZFJldXNlUm91dGUgfSA9IHRoaXMucm91dGVyLnJvdXRlUmV1c2VTdHJhdGVneTtcblxuICAgIHRoaXMuc2V0Um91dGVSZXVzZSgoKSA9PiBmYWxzZSk7XG4gICAgdGhpcy5yb3V0ZXIubmF2aWdhdGVkID0gZmFsc2U7XG5cbiAgICByZXR1cm4gcmVnaXN0ZXJMb2NhbGUobG9jYWxlKS50aGVuKGFzeW5jICgpID0+IHtcbiAgICAgIGF3YWl0IHRoaXMucm91dGVyLm5hdmlnYXRlQnlVcmwodGhpcy5yb3V0ZXIudXJsKS5jYXRjaChub29wKTtcbiAgICAgIHRoaXMuc2V0Um91dGVSZXVzZShzaG91bGRSZXVzZVJvdXRlKTtcbiAgICB9KTtcbiAgfVxuXG4gIGdldChrZXlzOiBzdHJpbmcsIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXSk6IE9ic2VydmFibGU8c3RyaW5nPiB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0KENvbmZpZ1N0YXRlLmdldENvcHkoa2V5cywgLi4uaW50ZXJwb2xhdGVQYXJhbXMpKTtcbiAgfVxuXG4gIGluc3RhbnQoa2V5czogc3RyaW5nLCAuLi5pbnRlcnBvbGF0ZVBhcmFtczogc3RyaW5nW10pOiBzdHJpbmcge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldENvcHkoa2V5cywgLi4uaW50ZXJwb2xhdGVQYXJhbXMpKTtcbiAgfVxufVxuIl19