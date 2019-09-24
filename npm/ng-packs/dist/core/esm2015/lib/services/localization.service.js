/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Injectable, Optional, SkipSelf, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, Store } from '@ngxs/store';
import { noop } from 'rxjs';
import { ConfigState } from '../states/config.state';
import { SessionState } from '../states/session.state';
import { registerLocale } from '../utils/initial-utils';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
import * as i2 from "@angular/router";
export class LocalizationService {
    /**
     * @param {?} store
     * @param {?} router
     * @param {?} ngZone
     * @param {?} actions
     * @param {?} otherInstance
     */
    constructor(store, router, ngZone, actions, otherInstance) {
        this.store = store;
        this.router = router;
        this.ngZone = ngZone;
        this.actions = actions;
        if (otherInstance)
            throw new Error('LocaleService should have only one instance.');
    }
    /**
     * @return {?}
     */
    get currentLang() {
        return this.store.selectSnapshot(SessionState.getLanguage);
    }
    /**
     * @private
     * @param {?} reuse
     * @return {?}
     */
    setRouteReuse(reuse) {
        this.router.routeReuseStrategy.shouldReuseRoute = reuse;
    }
    /**
     * @param {?} locale
     * @return {?}
     */
    registerLocale(locale) {
        const { shouldReuseRoute } = this.router.routeReuseStrategy;
        this.setRouteReuse((/**
         * @return {?}
         */
        () => false));
        this.router.navigated = false;
        return registerLocale(locale).then((/**
         * @return {?}
         */
        () => {
            this.ngZone.run((/**
             * @return {?}
             */
            () => tslib_1.__awaiter(this, void 0, void 0, function* () {
                yield this.router.navigateByUrl(this.router.url).catch(noop);
                this.setRouteReuse(shouldReuseRoute);
            })));
        }));
    }
    /**
     * @param {?} keys
     * @param {...?} interpolateParams
     * @return {?}
     */
    get(keys, ...interpolateParams) {
        return this.store.select(ConfigState.getCopy(keys, ...interpolateParams));
    }
    /**
     * @param {?} keys
     * @param {...?} interpolateParams
     * @return {?}
     */
    instant(keys, ...interpolateParams) {
        return this.store.selectSnapshot(ConfigState.getCopy(keys, ...interpolateParams));
    }
}
LocalizationService.decorators = [
    { type: Injectable, args: [{ providedIn: 'root' },] }
];
/** @nocollapse */
LocalizationService.ctorParameters = () => [
    { type: Store },
    { type: Router },
    { type: NgZone },
    { type: Actions },
    { type: LocalizationService, decorators: [{ type: Optional }, { type: SkipSelf }] }
];
/** @nocollapse */ LocalizationService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function LocalizationService_Factory() { return new LocalizationService(i0.ɵɵinject(i1.Store), i0.ɵɵinject(i2.Router), i0.ɵɵinject(i0.NgZone), i0.ɵɵinject(i1.Actions), i0.ɵɵinject(LocalizationService, 12)); }, token: LocalizationService, providedIn: "root" });
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
    /**
     * @type {?}
     * @private
     */
    LocalizationService.prototype.actions;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvbG9jYWxpemF0aW9uLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLFFBQVEsRUFBRSxRQUFRLEVBQUUsTUFBTSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQ3ZFLE9BQU8sRUFBMEIsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDakUsT0FBTyxFQUFFLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDN0MsT0FBTyxFQUFFLElBQUksRUFBYyxNQUFNLE1BQU0sQ0FBQztBQUN4QyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sd0JBQXdCLENBQUM7QUFDckQsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHlCQUF5QixDQUFDO0FBQ3ZELE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSx3QkFBd0IsQ0FBQzs7OztBQUt4RCxNQUFNLE9BQU8sbUJBQW1COzs7Ozs7OztJQUs5QixZQUNVLEtBQVksRUFDWixNQUFjLEVBQ2QsTUFBYyxFQUNkLE9BQWdCLEVBR3hCLGFBQWtDO1FBTjFCLFVBQUssR0FBTCxLQUFLLENBQU87UUFDWixXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQ2QsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUNkLFlBQU8sR0FBUCxPQUFPLENBQVM7UUFLeEIsSUFBSSxhQUFhO1lBQUUsTUFBTSxJQUFJLEtBQUssQ0FBQyw4Q0FBOEMsQ0FBQyxDQUFDO0lBQ3JGLENBQUM7Ozs7SUFkRCxJQUFJLFdBQVc7UUFDYixPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFlBQVksQ0FBQyxXQUFXLENBQUMsQ0FBQztJQUM3RCxDQUFDOzs7Ozs7SUFjTyxhQUFhLENBQUMsS0FBdUI7UUFDM0MsSUFBSSxDQUFDLE1BQU0sQ0FBQyxrQkFBa0IsQ0FBQyxnQkFBZ0IsR0FBRyxLQUFLLENBQUM7SUFDMUQsQ0FBQzs7Ozs7SUFFRCxjQUFjLENBQUMsTUFBYztjQUNyQixFQUFFLGdCQUFnQixFQUFFLEdBQUcsSUFBSSxDQUFDLE1BQU0sQ0FBQyxrQkFBa0I7UUFFM0QsSUFBSSxDQUFDLGFBQWE7OztRQUFDLEdBQUcsRUFBRSxDQUFDLEtBQUssRUFBQyxDQUFDO1FBQ2hDLElBQUksQ0FBQyxNQUFNLENBQUMsU0FBUyxHQUFHLEtBQUssQ0FBQztRQUU5QixPQUFPLGNBQWMsQ0FBQyxNQUFNLENBQUMsQ0FBQyxJQUFJOzs7UUFBQyxHQUFHLEVBQUU7WUFDdEMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxHQUFHOzs7WUFBQyxHQUFTLEVBQUU7Z0JBQ3pCLE1BQU0sSUFBSSxDQUFDLE1BQU0sQ0FBQyxhQUFhLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxHQUFHLENBQUMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLENBQUM7Z0JBQzdELElBQUksQ0FBQyxhQUFhLENBQUMsZ0JBQWdCLENBQUMsQ0FBQztZQUN2QyxDQUFDLENBQUEsRUFBQyxDQUFDO1FBQ0wsQ0FBQyxFQUFDLENBQUM7SUFDTCxDQUFDOzs7Ozs7SUFFRCxHQUFHLENBQUMsSUFBWSxFQUFFLEdBQUcsaUJBQTJCO1FBQzlDLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxNQUFNLENBQUMsV0FBVyxDQUFDLE9BQU8sQ0FBQyxJQUFJLEVBQUUsR0FBRyxpQkFBaUIsQ0FBQyxDQUFDLENBQUM7SUFDNUUsQ0FBQzs7Ozs7O0lBRUQsT0FBTyxDQUFDLElBQVksRUFBRSxHQUFHLGlCQUEyQjtRQUNsRCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxPQUFPLENBQUMsSUFBSSxFQUFFLEdBQUcsaUJBQWlCLENBQUMsQ0FBQyxDQUFDO0lBQ3BGLENBQUM7OztZQTFDRixVQUFVLFNBQUMsRUFBRSxVQUFVLEVBQUUsTUFBTSxFQUFFOzs7O1lBUmhCLEtBQUs7WUFEVSxNQUFNO1lBREUsTUFBTTtZQUV0QyxPQUFPO1lBcUJHLG1CQUFtQix1QkFGakMsUUFBUSxZQUNSLFFBQVE7Ozs7Ozs7O0lBTFQsb0NBQW9COzs7OztJQUNwQixxQ0FBc0I7Ozs7O0lBQ3RCLHFDQUFzQjs7Ozs7SUFDdEIsc0NBQXdCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSwgT3B0aW9uYWwsIFNraXBTZWxmLCBOZ1pvbmUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEFjdGl2YXRlZFJvdXRlU25hcHNob3QsIFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBBY3Rpb25zLCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IG5vb3AsIE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL2NvbmZpZy5zdGF0ZSc7XG5pbXBvcnQgeyBTZXNzaW9uU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMvc2Vzc2lvbi5zdGF0ZSc7XG5pbXBvcnQgeyByZWdpc3RlckxvY2FsZSB9IGZyb20gJy4uL3V0aWxzL2luaXRpYWwtdXRpbHMnO1xuXG50eXBlIFNob3VsZFJldXNlUm91dGUgPSAoZnV0dXJlOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90LCBjdXJyOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90KSA9PiBib29sZWFuO1xuXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxuZXhwb3J0IGNsYXNzIExvY2FsaXphdGlvblNlcnZpY2Uge1xuICBnZXQgY3VycmVudExhbmcoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXNzaW9uU3RhdGUuZ2V0TGFuZ3VhZ2UpO1xuICB9XG5cbiAgY29uc3RydWN0b3IoXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXG4gICAgcHJpdmF0ZSByb3V0ZXI6IFJvdXRlcixcbiAgICBwcml2YXRlIG5nWm9uZTogTmdab25lLFxuICAgIHByaXZhdGUgYWN0aW9uczogQWN0aW9ucyxcbiAgICBAT3B0aW9uYWwoKVxuICAgIEBTa2lwU2VsZigpXG4gICAgb3RoZXJJbnN0YW5jZTogTG9jYWxpemF0aW9uU2VydmljZSxcbiAgKSB7XG4gICAgaWYgKG90aGVySW5zdGFuY2UpIHRocm93IG5ldyBFcnJvcignTG9jYWxlU2VydmljZSBzaG91bGQgaGF2ZSBvbmx5IG9uZSBpbnN0YW5jZS4nKTtcbiAgfVxuXG4gIHByaXZhdGUgc2V0Um91dGVSZXVzZShyZXVzZTogU2hvdWxkUmV1c2VSb3V0ZSkge1xuICAgIHRoaXMucm91dGVyLnJvdXRlUmV1c2VTdHJhdGVneS5zaG91bGRSZXVzZVJvdXRlID0gcmV1c2U7XG4gIH1cblxuICByZWdpc3RlckxvY2FsZShsb2NhbGU6IHN0cmluZykge1xuICAgIGNvbnN0IHsgc2hvdWxkUmV1c2VSb3V0ZSB9ID0gdGhpcy5yb3V0ZXIucm91dGVSZXVzZVN0cmF0ZWd5O1xuXG4gICAgdGhpcy5zZXRSb3V0ZVJldXNlKCgpID0+IGZhbHNlKTtcbiAgICB0aGlzLnJvdXRlci5uYXZpZ2F0ZWQgPSBmYWxzZTtcblxuICAgIHJldHVybiByZWdpc3RlckxvY2FsZShsb2NhbGUpLnRoZW4oKCkgPT4ge1xuICAgICAgdGhpcy5uZ1pvbmUucnVuKGFzeW5jICgpID0+IHtcbiAgICAgICAgYXdhaXQgdGhpcy5yb3V0ZXIubmF2aWdhdGVCeVVybCh0aGlzLnJvdXRlci51cmwpLmNhdGNoKG5vb3ApO1xuICAgICAgICB0aGlzLnNldFJvdXRlUmV1c2Uoc2hvdWxkUmV1c2VSb3V0ZSk7XG4gICAgICB9KTtcbiAgICB9KTtcbiAgfVxuXG4gIGdldChrZXlzOiBzdHJpbmcsIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXSk6IE9ic2VydmFibGU8c3RyaW5nPiB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0KENvbmZpZ1N0YXRlLmdldENvcHkoa2V5cywgLi4uaW50ZXJwb2xhdGVQYXJhbXMpKTtcbiAgfVxuXG4gIGluc3RhbnQoa2V5czogc3RyaW5nLCAuLi5pbnRlcnBvbGF0ZVBhcmFtczogc3RyaW5nW10pOiBzdHJpbmcge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldENvcHkoa2V5cywgLi4uaW50ZXJwb2xhdGVQYXJhbXMpKTtcbiAgfVxufVxuIl19