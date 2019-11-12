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
export class LocalizationService {
    /**
     * @param {?} store
     * @param {?} router
     * @param {?} ngZone
     * @param {?} otherInstance
     */
    constructor(store, router, ngZone, otherInstance) {
        this.store = store;
        this.router = router;
        this.ngZone = ngZone;
        if (otherInstance)
            throw new Error('LocaleService should have only one instance.');
    }
    /**
     * @return {?}
     */
    get currentLang() {
        return this.store.selectSnapshot((/**
         * @param {?} state
         * @return {?}
         */
        state => state.SessionState.language));
    }
    /**
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
     * @param {?} key
     * @param {...?} interpolateParams
     * @return {?}
     */
    get(key, ...interpolateParams) {
        return this.store.select(ConfigState.getLocalization(key, ...interpolateParams));
    }
    /**
     * @param {?} key
     * @param {...?} interpolateParams
     * @return {?}
     */
    instant(key, ...interpolateParams) {
        return this.store.selectSnapshot(ConfigState.getLocalization(key, ...interpolateParams));
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
    { type: LocalizationService, decorators: [{ type: Optional }, { type: SkipSelf }] }
];
/** @nocollapse */ LocalizationService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function LocalizationService_Factory() { return new LocalizationService(i0.ɵɵinject(i1.Store), i0.ɵɵinject(i2.Router), i0.ɵɵinject(i0.NgZone), i0.ɵɵinject(LocalizationService, 12)); }, token: LocalizationService, providedIn: "root" });
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvbG9jYWxpemF0aW9uLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUN2RSxPQUFPLEVBQTBCLE1BQU0sRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQ2pFLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLElBQUksRUFBYyxNQUFNLE1BQU0sQ0FBQztBQUN4QyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sd0JBQXdCLENBQUM7QUFDckQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHdCQUF3QixDQUFDOzs7O0FBS3hELE1BQU0sT0FBTyxtQkFBbUI7Ozs7Ozs7SUFLOUIsWUFDVSxLQUFZLEVBQ1osTUFBYyxFQUNkLE1BQWMsRUFHdEIsYUFBa0M7UUFMMUIsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUNaLFdBQU0sR0FBTixNQUFNLENBQVE7UUFDZCxXQUFNLEdBQU4sTUFBTSxDQUFRO1FBS3RCLElBQUksYUFBYTtZQUFFLE1BQU0sSUFBSSxLQUFLLENBQUMsOENBQThDLENBQUMsQ0FBQztJQUNyRixDQUFDOzs7O0lBYkQsSUFBSSxXQUFXO1FBQ2IsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWM7Ozs7UUFBQyxLQUFLLENBQUMsRUFBRSxDQUFDLEtBQUssQ0FBQyxZQUFZLENBQUMsUUFBUSxFQUFDLENBQUM7SUFDekUsQ0FBQzs7Ozs7SUFhRCxhQUFhLENBQUMsS0FBdUI7UUFDbkMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxrQkFBa0IsQ0FBQyxnQkFBZ0IsR0FBRyxLQUFLLENBQUM7SUFDMUQsQ0FBQzs7Ozs7SUFFRCxjQUFjLENBQUMsTUFBYztjQUNyQixFQUFFLGdCQUFnQixFQUFFLEdBQUcsSUFBSSxDQUFDLE1BQU0sQ0FBQyxrQkFBa0I7UUFDM0QsSUFBSSxDQUFDLGFBQWE7OztRQUFDLEdBQUcsRUFBRSxDQUFDLEtBQUssRUFBQyxDQUFDO1FBQ2hDLElBQUksQ0FBQyxNQUFNLENBQUMsU0FBUyxHQUFHLEtBQUssQ0FBQztRQUU5QixPQUFPLGNBQWMsQ0FBQyxNQUFNLENBQUMsQ0FBQyxJQUFJOzs7UUFBQyxHQUFHLEVBQUU7WUFDdEMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxHQUFHOzs7WUFBQyxHQUFTLEVBQUU7Z0JBQ3pCLE1BQU0sSUFBSSxDQUFDLE1BQU0sQ0FBQyxhQUFhLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxHQUFHLENBQUMsQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLENBQUM7Z0JBQzdELElBQUksQ0FBQyxhQUFhLENBQUMsZ0JBQWdCLENBQUMsQ0FBQztZQUN2QyxDQUFDLENBQUEsRUFBQyxDQUFDO1FBQ0wsQ0FBQyxFQUFDLENBQUM7SUFDTCxDQUFDOzs7Ozs7SUFFRCxHQUFHLENBQUMsR0FBVyxFQUFFLEdBQUcsaUJBQTJCO1FBQzdDLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxNQUFNLENBQUMsV0FBVyxDQUFDLGVBQWUsQ0FBQyxHQUFHLEVBQUUsR0FBRyxpQkFBaUIsQ0FBQyxDQUFDLENBQUM7SUFDbkYsQ0FBQzs7Ozs7O0lBRUQsT0FBTyxDQUFDLEdBQVcsRUFBRSxHQUFHLGlCQUEyQjtRQUNqRCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxlQUFlLENBQUMsR0FBRyxFQUFFLEdBQUcsaUJBQWlCLENBQUMsQ0FBQyxDQUFDO0lBQzNGLENBQUM7OztZQXhDRixVQUFVLFNBQUMsRUFBRSxVQUFVLEVBQUUsTUFBTSxFQUFFOzs7O1lBUHpCLEtBQUs7WUFEbUIsTUFBTTtZQURsQixNQUFNO1lBcUJSLG1CQUFtQix1QkFGakMsUUFBUSxZQUNSLFFBQVE7Ozs7Ozs7O0lBSlQsb0NBQW9COzs7OztJQUNwQixxQ0FBc0I7Ozs7O0lBQ3RCLHFDQUFzQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUsIE5nWm9uZSwgT3B0aW9uYWwsIFNraXBTZWxmIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IEFjdGl2YXRlZFJvdXRlU25hcHNob3QsIFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XHJcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBub29wLCBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL2NvbmZpZy5zdGF0ZSc7XHJcbmltcG9ydCB7IHJlZ2lzdGVyTG9jYWxlIH0gZnJvbSAnLi4vdXRpbHMvaW5pdGlhbC11dGlscyc7XHJcblxyXG50eXBlIFNob3VsZFJldXNlUm91dGUgPSAoZnV0dXJlOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90LCBjdXJyOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90KSA9PiBib29sZWFuO1xyXG5cclxuQEluamVjdGFibGUoeyBwcm92aWRlZEluOiAncm9vdCcgfSlcclxuZXhwb3J0IGNsYXNzIExvY2FsaXphdGlvblNlcnZpY2Uge1xyXG4gIGdldCBjdXJyZW50TGFuZygpOiBzdHJpbmcge1xyXG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3Qoc3RhdGUgPT4gc3RhdGUuU2Vzc2lvblN0YXRlLmxhbmd1YWdlKTtcclxuICB9XHJcblxyXG4gIGNvbnN0cnVjdG9yKFxyXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXHJcbiAgICBwcml2YXRlIHJvdXRlcjogUm91dGVyLFxyXG4gICAgcHJpdmF0ZSBuZ1pvbmU6IE5nWm9uZSxcclxuICAgIEBPcHRpb25hbCgpXHJcbiAgICBAU2tpcFNlbGYoKVxyXG4gICAgb3RoZXJJbnN0YW5jZTogTG9jYWxpemF0aW9uU2VydmljZSxcclxuICApIHtcclxuICAgIGlmIChvdGhlckluc3RhbmNlKSB0aHJvdyBuZXcgRXJyb3IoJ0xvY2FsZVNlcnZpY2Ugc2hvdWxkIGhhdmUgb25seSBvbmUgaW5zdGFuY2UuJyk7XHJcbiAgfVxyXG5cclxuICBzZXRSb3V0ZVJldXNlKHJldXNlOiBTaG91bGRSZXVzZVJvdXRlKSB7XHJcbiAgICB0aGlzLnJvdXRlci5yb3V0ZVJldXNlU3RyYXRlZ3kuc2hvdWxkUmV1c2VSb3V0ZSA9IHJldXNlO1xyXG4gIH1cclxuXHJcbiAgcmVnaXN0ZXJMb2NhbGUobG9jYWxlOiBzdHJpbmcpIHtcclxuICAgIGNvbnN0IHsgc2hvdWxkUmV1c2VSb3V0ZSB9ID0gdGhpcy5yb3V0ZXIucm91dGVSZXVzZVN0cmF0ZWd5O1xyXG4gICAgdGhpcy5zZXRSb3V0ZVJldXNlKCgpID0+IGZhbHNlKTtcclxuICAgIHRoaXMucm91dGVyLm5hdmlnYXRlZCA9IGZhbHNlO1xyXG5cclxuICAgIHJldHVybiByZWdpc3RlckxvY2FsZShsb2NhbGUpLnRoZW4oKCkgPT4ge1xyXG4gICAgICB0aGlzLm5nWm9uZS5ydW4oYXN5bmMgKCkgPT4ge1xyXG4gICAgICAgIGF3YWl0IHRoaXMucm91dGVyLm5hdmlnYXRlQnlVcmwodGhpcy5yb3V0ZXIudXJsKS5jYXRjaChub29wKTtcclxuICAgICAgICB0aGlzLnNldFJvdXRlUmV1c2Uoc2hvdWxkUmV1c2VSb3V0ZSk7XHJcbiAgICAgIH0pO1xyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBnZXQoa2V5OiBzdHJpbmcsIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXSk6IE9ic2VydmFibGU8c3RyaW5nPiB7XHJcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3QoQ29uZmlnU3RhdGUuZ2V0TG9jYWxpemF0aW9uKGtleSwgLi4uaW50ZXJwb2xhdGVQYXJhbXMpKTtcclxuICB9XHJcblxyXG4gIGluc3RhbnQoa2V5OiBzdHJpbmcsIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXSk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRMb2NhbGl6YXRpb24oa2V5LCAuLi5pbnRlcnBvbGF0ZVBhcmFtcykpO1xyXG4gIH1cclxufVxyXG4iXX0=