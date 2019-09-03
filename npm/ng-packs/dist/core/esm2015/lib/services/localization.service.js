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
export class LocalizationService {
    /**
     * @param {?} store
     * @param {?} router
     * @param {?} actions
     * @param {?} otherInstance
     */
    constructor(store, router, actions, otherInstance) {
        this.store = store;
        this.router = router;
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
        () => tslib_1.__awaiter(this, void 0, void 0, function* () {
            yield this.router.navigateByUrl(this.router.url).catch(noop);
            this.setRouteReuse(shouldReuseRoute);
        })));
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
    { type: Actions },
    { type: LocalizationService, decorators: [{ type: Optional }, { type: SkipSelf }] }
];
/** @nocollapse */ LocalizationService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function LocalizationService_Factory() { return new LocalizationService(i0.ɵɵinject(i1.Store), i0.ɵɵinject(i2.Router), i0.ɵɵinject(i1.Actions), i0.ɵɵinject(LocalizationService, 12)); }, token: LocalizationService, providedIn: "root" });
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9jYWxpemF0aW9uLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvbG9jYWxpemF0aW9uLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLFFBQVEsRUFBRSxRQUFRLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDL0QsT0FBTyxFQUEwQixNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUNqRSxPQUFPLEVBQUUsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM3QyxPQUFPLEVBQUUsSUFBSSxFQUFjLE1BQU0sTUFBTSxDQUFDO0FBQ3hDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSx3QkFBd0IsQ0FBQztBQUNyRCxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0seUJBQXlCLENBQUM7QUFDdkQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHdCQUF3QixDQUFDOzs7O0FBS3hELE1BQU0sT0FBTyxtQkFBbUI7Ozs7Ozs7SUFLOUIsWUFDVSxLQUFZLEVBQ1osTUFBYyxFQUNkLE9BQWdCLEVBR3hCLGFBQWtDO1FBTDFCLFVBQUssR0FBTCxLQUFLLENBQU87UUFDWixXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQ2QsWUFBTyxHQUFQLE9BQU8sQ0FBUztRQUt4QixJQUFJLGFBQWE7WUFBRSxNQUFNLElBQUksS0FBSyxDQUFDLDhDQUE4QyxDQUFDLENBQUM7SUFDckYsQ0FBQzs7OztJQWJELElBQUksV0FBVztRQUNiLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsWUFBWSxDQUFDLFdBQVcsQ0FBQyxDQUFDO0lBQzdELENBQUM7Ozs7OztJQWFPLGFBQWEsQ0FBQyxLQUF1QjtRQUMzQyxJQUFJLENBQUMsTUFBTSxDQUFDLGtCQUFrQixDQUFDLGdCQUFnQixHQUFHLEtBQUssQ0FBQztJQUMxRCxDQUFDOzs7OztJQUVELGNBQWMsQ0FBQyxNQUFjO2NBQ3JCLEVBQUUsZ0JBQWdCLEVBQUUsR0FBRyxJQUFJLENBQUMsTUFBTSxDQUFDLGtCQUFrQjtRQUUzRCxJQUFJLENBQUMsYUFBYTs7O1FBQUMsR0FBRyxFQUFFLENBQUMsS0FBSyxFQUFDLENBQUM7UUFDaEMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxTQUFTLEdBQUcsS0FBSyxDQUFDO1FBRTlCLE9BQU8sY0FBYyxDQUFDLE1BQU0sQ0FBQyxDQUFDLElBQUk7OztRQUFDLEdBQVMsRUFBRTtZQUM1QyxNQUFNLElBQUksQ0FBQyxNQUFNLENBQUMsYUFBYSxDQUFDLElBQUksQ0FBQyxNQUFNLENBQUMsR0FBRyxDQUFDLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxDQUFDO1lBQzdELElBQUksQ0FBQyxhQUFhLENBQUMsZ0JBQWdCLENBQUMsQ0FBQztRQUN2QyxDQUFDLENBQUEsRUFBQyxDQUFDO0lBQ0wsQ0FBQzs7Ozs7O0lBRUQsR0FBRyxDQUFDLElBQVksRUFBRSxHQUFHLGlCQUEyQjtRQUM5QyxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsTUFBTSxDQUFDLFdBQVcsQ0FBQyxPQUFPLENBQUMsSUFBSSxFQUFFLEdBQUcsaUJBQWlCLENBQUMsQ0FBQyxDQUFDO0lBQzVFLENBQUM7Ozs7OztJQUVELE9BQU8sQ0FBQyxJQUFZLEVBQUUsR0FBRyxpQkFBMkI7UUFDbEQsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsT0FBTyxDQUFDLElBQUksRUFBRSxHQUFHLGlCQUFpQixDQUFDLENBQUMsQ0FBQztJQUNwRixDQUFDOzs7WUF2Q0YsVUFBVSxTQUFDLEVBQUUsVUFBVSxFQUFFLE1BQU0sRUFBRTs7OztZQVJoQixLQUFLO1lBRFUsTUFBTTtZQUM5QixPQUFPO1lBb0JHLG1CQUFtQix1QkFGakMsUUFBUSxZQUNSLFFBQVE7Ozs7Ozs7O0lBSlQsb0NBQW9COzs7OztJQUNwQixxQ0FBc0I7Ozs7O0lBQ3RCLHNDQUF3QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUsIE9wdGlvbmFsLCBTa2lwU2VsZiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQWN0aXZhdGVkUm91dGVTbmFwc2hvdCwgUm91dGVyIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IEFjdGlvbnMsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgbm9vcCwgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMvY29uZmlnLnN0YXRlJztcbmltcG9ydCB7IFNlc3Npb25TdGF0ZSB9IGZyb20gJy4uL3N0YXRlcy9zZXNzaW9uLnN0YXRlJztcbmltcG9ydCB7IHJlZ2lzdGVyTG9jYWxlIH0gZnJvbSAnLi4vdXRpbHMvaW5pdGlhbC11dGlscyc7XG5cbnR5cGUgU2hvdWxkUmV1c2VSb3V0ZSA9IChmdXR1cmU6IEFjdGl2YXRlZFJvdXRlU25hcHNob3QsIGN1cnI6IEFjdGl2YXRlZFJvdXRlU25hcHNob3QpID0+IGJvb2xlYW47XG5cbkBJbmplY3RhYmxlKHsgcHJvdmlkZWRJbjogJ3Jvb3QnIH0pXG5leHBvcnQgY2xhc3MgTG9jYWxpemF0aW9uU2VydmljZSB7XG4gIGdldCBjdXJyZW50TGFuZygpOiBzdHJpbmcge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFNlc3Npb25TdGF0ZS5nZXRMYW5ndWFnZSk7XG4gIH1cblxuICBjb25zdHJ1Y3RvcihcbiAgICBwcml2YXRlIHN0b3JlOiBTdG9yZSxcbiAgICBwcml2YXRlIHJvdXRlcjogUm91dGVyLFxuICAgIHByaXZhdGUgYWN0aW9uczogQWN0aW9ucyxcbiAgICBAT3B0aW9uYWwoKVxuICAgIEBTa2lwU2VsZigpXG4gICAgb3RoZXJJbnN0YW5jZTogTG9jYWxpemF0aW9uU2VydmljZSxcbiAgKSB7XG4gICAgaWYgKG90aGVySW5zdGFuY2UpIHRocm93IG5ldyBFcnJvcignTG9jYWxlU2VydmljZSBzaG91bGQgaGF2ZSBvbmx5IG9uZSBpbnN0YW5jZS4nKTtcbiAgfVxuXG4gIHByaXZhdGUgc2V0Um91dGVSZXVzZShyZXVzZTogU2hvdWxkUmV1c2VSb3V0ZSkge1xuICAgIHRoaXMucm91dGVyLnJvdXRlUmV1c2VTdHJhdGVneS5zaG91bGRSZXVzZVJvdXRlID0gcmV1c2U7XG4gIH1cblxuICByZWdpc3RlckxvY2FsZShsb2NhbGU6IHN0cmluZykge1xuICAgIGNvbnN0IHsgc2hvdWxkUmV1c2VSb3V0ZSB9ID0gdGhpcy5yb3V0ZXIucm91dGVSZXVzZVN0cmF0ZWd5O1xuXG4gICAgdGhpcy5zZXRSb3V0ZVJldXNlKCgpID0+IGZhbHNlKTtcbiAgICB0aGlzLnJvdXRlci5uYXZpZ2F0ZWQgPSBmYWxzZTtcblxuICAgIHJldHVybiByZWdpc3RlckxvY2FsZShsb2NhbGUpLnRoZW4oYXN5bmMgKCkgPT4ge1xuICAgICAgYXdhaXQgdGhpcy5yb3V0ZXIubmF2aWdhdGVCeVVybCh0aGlzLnJvdXRlci51cmwpLmNhdGNoKG5vb3ApO1xuICAgICAgdGhpcy5zZXRSb3V0ZVJldXNlKHNob3VsZFJldXNlUm91dGUpO1xuICAgIH0pO1xuICB9XG5cbiAgZ2V0KGtleXM6IHN0cmluZywgLi4uaW50ZXJwb2xhdGVQYXJhbXM6IHN0cmluZ1tdKTogT2JzZXJ2YWJsZTxzdHJpbmc+IHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3QoQ29uZmlnU3RhdGUuZ2V0Q29weShrZXlzLCAuLi5pbnRlcnBvbGF0ZVBhcmFtcykpO1xuICB9XG5cbiAgaW5zdGFudChrZXlzOiBzdHJpbmcsIC4uLmludGVycG9sYXRlUGFyYW1zOiBzdHJpbmdbXSk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0Q29weShrZXlzLCAuLi5pbnRlcnBvbGF0ZVBhcmFtcykpO1xuICB9XG59XG4iXX0=