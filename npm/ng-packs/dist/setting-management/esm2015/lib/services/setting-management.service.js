/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Navigate } from '@ngxs/router-plugin';
import { Store, Actions, ofActionSuccessful } from '@ngxs/store';
import { Subject } from 'rxjs';
import { ConfigState, GetAppConfiguration } from '@abp/ng.core';
import { OAuthService } from 'angular-oauth2-oidc';
import { takeUntil } from 'rxjs/operators';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
import * as i2 from "@angular/router";
import * as i3 from "angular-oauth2-oidc";
export class SettingManagementService {
    /**
     * @param {?} actions
     * @param {?} router
     * @param {?} store
     * @param {?} oAuthService
     */
    constructor(actions, router, store, oAuthService) {
        this.actions = actions;
        this.router = router;
        this.store = store;
        this.oAuthService = oAuthService;
        this.settings = [];
        this.selected = (/** @type {?} */ ({}));
        this.destroy$ = new Subject();
        setTimeout((/**
         * @return {?}
         */
        () => this.setSettings()), 0);
        this.actions
            .pipe(ofActionSuccessful(GetAppConfiguration))
            .pipe(takeUntil(this.destroy$))
            .subscribe((/**
         * @return {?}
         */
        () => {
            if (this.oAuthService.hasValidAccessToken()) {
                this.setSettings();
            }
        }));
    }
    /**
     * @return {?}
     */
    ngOnDestroy() {
        this.destroy$.next();
    }
    /**
     * @return {?}
     */
    setSettings() {
        /** @type {?} */
        const route = this.router.config.find((/**
         * @param {?} r
         * @return {?}
         */
        r => r.path === 'setting-management'));
        this.settings = ((/** @type {?} */ (route.data.settings)))
            .filter((/**
         * @param {?} setting
         * @return {?}
         */
        setting => this.store.selectSnapshot(ConfigState.getGrantedPolicy(setting.requiredPolicy))))
            .sort((/**
         * @param {?} a
         * @param {?} b
         * @return {?}
         */
        (a, b) => a.order - b.order));
        this.checkSelected();
    }
    /**
     * @return {?}
     */
    checkSelected() {
        this.selected = this.settings.find((/**
         * @param {?} setting
         * @return {?}
         */
        setting => setting.url === this.router.url)) || ((/** @type {?} */ ({})));
        if (!this.selected.name && this.settings.length) {
            this.setSelected(this.settings[0]);
        }
    }
    /**
     * @param {?} selected
     * @return {?}
     */
    setSelected(selected) {
        this.selected = selected;
        this.store.dispatch(new Navigate([selected.url]));
    }
}
SettingManagementService.decorators = [
    { type: Injectable, args: [{ providedIn: 'root' },] }
];
/** @nocollapse */
SettingManagementService.ctorParameters = () => [
    { type: Actions },
    { type: Router },
    { type: Store },
    { type: OAuthService }
];
/** @nocollapse */ SettingManagementService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function SettingManagementService_Factory() { return new SettingManagementService(i0.ɵɵinject(i1.Actions), i0.ɵɵinject(i2.Router), i0.ɵɵinject(i1.Store), i0.ɵɵinject(i3.OAuthService)); }, token: SettingManagementService, providedIn: "root" });
if (false) {
    /** @type {?} */
    SettingManagementService.prototype.settings;
    /** @type {?} */
    SettingManagementService.prototype.selected;
    /**
     * @type {?}
     * @private
     */
    SettingManagementService.prototype.destroy$;
    /**
     * @type {?}
     * @private
     */
    SettingManagementService.prototype.actions;
    /**
     * @type {?}
     * @private
     */
    SettingManagementService.prototype.router;
    /**
     * @type {?}
     * @private
     */
    SettingManagementService.prototype.store;
    /**
     * @type {?}
     * @private
     */
    SettingManagementService.prototype.oAuthService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnNldHRpbmctbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9zZXR0aW5nLW1hbmFnZW1lbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQ0EsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDekMsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQy9DLE9BQU8sRUFBRSxLQUFLLEVBQUUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ2pFLE9BQU8sRUFBRSxPQUFPLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDL0IsT0FBTyxFQUFFLFdBQVcsRUFBRSxtQkFBbUIsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNoRSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0scUJBQXFCLENBQUM7QUFDbkQsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLGdCQUFnQixDQUFDOzs7OztBQUczQyxNQUFNLE9BQU8sd0JBQXdCOzs7Ozs7O0lBT25DLFlBQ1UsT0FBZ0IsRUFDaEIsTUFBYyxFQUNkLEtBQVksRUFDWixZQUEwQjtRQUgxQixZQUFPLEdBQVAsT0FBTyxDQUFTO1FBQ2hCLFdBQU0sR0FBTixNQUFNLENBQVE7UUFDZCxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQ1osaUJBQVksR0FBWixZQUFZLENBQWM7UUFWcEMsYUFBUSxHQUFpQixFQUFFLENBQUM7UUFFNUIsYUFBUSxHQUFHLG1CQUFBLEVBQUUsRUFBYyxDQUFDO1FBRXBCLGFBQVEsR0FBRyxJQUFJLE9BQU8sRUFBRSxDQUFDO1FBUS9CLFVBQVU7OztRQUFDLEdBQUcsRUFBRSxDQUFDLElBQUksQ0FBQyxXQUFXLEVBQUUsR0FBRSxDQUFDLENBQUMsQ0FBQztRQUV4QyxJQUFJLENBQUMsT0FBTzthQUNULElBQUksQ0FBQyxrQkFBa0IsQ0FBQyxtQkFBbUIsQ0FBQyxDQUFDO2FBQzdDLElBQUksQ0FBQyxTQUFTLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDO2FBQzlCLFNBQVM7OztRQUFDLEdBQUcsRUFBRTtZQUNkLElBQUksSUFBSSxDQUFDLFlBQVksQ0FBQyxtQkFBbUIsRUFBRSxFQUFFO2dCQUMzQyxJQUFJLENBQUMsV0FBVyxFQUFFLENBQUM7YUFDcEI7UUFDSCxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7Ozs7SUFFRCxXQUFXO1FBQ1QsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLEVBQUUsQ0FBQztJQUN2QixDQUFDOzs7O0lBRUQsV0FBVzs7Y0FDSCxLQUFLLEdBQUcsSUFBSSxDQUFDLE1BQU0sQ0FBQyxNQUFNLENBQUMsSUFBSTs7OztRQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLElBQUksS0FBSyxvQkFBb0IsRUFBQztRQUMzRSxJQUFJLENBQUMsUUFBUSxHQUFHLENBQUMsbUJBQUEsS0FBSyxDQUFDLElBQUksQ0FBQyxRQUFRLEVBQWdCLENBQUM7YUFDbEQsTUFBTTs7OztRQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLGdCQUFnQixDQUFDLE9BQU8sQ0FBQyxjQUFjLENBQUMsQ0FBQyxFQUFDO2FBQ2xHLElBQUk7Ozs7O1FBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxFQUFFLEVBQUUsQ0FBQyxDQUFDLENBQUMsS0FBSyxHQUFHLENBQUMsQ0FBQyxLQUFLLEVBQUMsQ0FBQztRQUNyQyxJQUFJLENBQUMsYUFBYSxFQUFFLENBQUM7SUFDdkIsQ0FBQzs7OztJQUVELGFBQWE7UUFDWCxJQUFJLENBQUMsUUFBUSxHQUFHLElBQUksQ0FBQyxRQUFRLENBQUMsSUFBSTs7OztRQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsT0FBTyxDQUFDLEdBQUcsS0FBSyxJQUFJLENBQUMsTUFBTSxDQUFDLEdBQUcsRUFBQyxJQUFJLENBQUMsbUJBQUEsRUFBRSxFQUFjLENBQUMsQ0FBQztRQUVyRyxJQUFJLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLElBQUksSUFBSSxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDL0MsSUFBSSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7U0FDcEM7SUFDSCxDQUFDOzs7OztJQUVELFdBQVcsQ0FBQyxRQUFvQjtRQUM5QixJQUFJLENBQUMsUUFBUSxHQUFHLFFBQVEsQ0FBQztRQUN6QixJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLFFBQVEsQ0FBQyxDQUFDLFFBQVEsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLENBQUM7SUFDcEQsQ0FBQzs7O1lBakRGLFVBQVUsU0FBQyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUU7Ozs7WUFObEIsT0FBTztZQUZkLE1BQU07WUFFTixLQUFLO1lBR0wsWUFBWTs7Ozs7SUFLbkIsNENBQTRCOztJQUU1Qiw0Q0FBNEI7Ozs7O0lBRTVCLDRDQUFpQzs7Ozs7SUFHL0IsMkNBQXdCOzs7OztJQUN4QiwwQ0FBc0I7Ozs7O0lBQ3RCLHlDQUFvQjs7Ozs7SUFDcEIsZ0RBQWtDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU2V0dGluZ1RhYiB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBOYXZpZ2F0ZSB9IGZyb20gJ0BuZ3hzL3JvdXRlci1wbHVnaW4nO1xuaW1wb3J0IHsgU3RvcmUsIEFjdGlvbnMsIG9mQWN0aW9uU3VjY2Vzc2Z1bCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IFN1YmplY3QgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IENvbmZpZ1N0YXRlLCBHZXRBcHBDb25maWd1cmF0aW9uIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IE9BdXRoU2VydmljZSB9IGZyb20gJ2FuZ3VsYXItb2F1dGgyLW9pZGMnO1xuaW1wb3J0IHsgdGFrZVVudGlsIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxuZXhwb3J0IGNsYXNzIFNldHRpbmdNYW5hZ2VtZW50U2VydmljZSB7XG4gIHNldHRpbmdzOiBTZXR0aW5nVGFiW10gPSBbXTtcblxuICBzZWxlY3RlZCA9IHt9IGFzIFNldHRpbmdUYWI7XG5cbiAgcHJpdmF0ZSBkZXN0cm95JCA9IG5ldyBTdWJqZWN0KCk7XG5cbiAgY29uc3RydWN0b3IoXG4gICAgcHJpdmF0ZSBhY3Rpb25zOiBBY3Rpb25zLFxuICAgIHByaXZhdGUgcm91dGVyOiBSb3V0ZXIsXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXG4gICAgcHJpdmF0ZSBvQXV0aFNlcnZpY2U6IE9BdXRoU2VydmljZSxcbiAgKSB7XG4gICAgc2V0VGltZW91dCgoKSA9PiB0aGlzLnNldFNldHRpbmdzKCksIDApO1xuXG4gICAgdGhpcy5hY3Rpb25zXG4gICAgICAucGlwZShvZkFjdGlvblN1Y2Nlc3NmdWwoR2V0QXBwQ29uZmlndXJhdGlvbikpXG4gICAgICAucGlwZSh0YWtlVW50aWwodGhpcy5kZXN0cm95JCkpXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgaWYgKHRoaXMub0F1dGhTZXJ2aWNlLmhhc1ZhbGlkQWNjZXNzVG9rZW4oKSkge1xuICAgICAgICAgIHRoaXMuc2V0U2V0dGluZ3MoKTtcbiAgICAgICAgfVxuICAgICAgfSk7XG4gIH1cblxuICBuZ09uRGVzdHJveSgpIHtcbiAgICB0aGlzLmRlc3Ryb3kkLm5leHQoKTtcbiAgfVxuXG4gIHNldFNldHRpbmdzKCkge1xuICAgIGNvbnN0IHJvdXRlID0gdGhpcy5yb3V0ZXIuY29uZmlnLmZpbmQociA9PiByLnBhdGggPT09ICdzZXR0aW5nLW1hbmFnZW1lbnQnKTtcbiAgICB0aGlzLnNldHRpbmdzID0gKHJvdXRlLmRhdGEuc2V0dGluZ3MgYXMgU2V0dGluZ1RhYltdKVxuICAgICAgLmZpbHRlcihzZXR0aW5nID0+IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0R3JhbnRlZFBvbGljeShzZXR0aW5nLnJlcXVpcmVkUG9saWN5KSkpXG4gICAgICAuc29ydCgoYSwgYikgPT4gYS5vcmRlciAtIGIub3JkZXIpO1xuICAgIHRoaXMuY2hlY2tTZWxlY3RlZCgpO1xuICB9XG5cbiAgY2hlY2tTZWxlY3RlZCgpIHtcbiAgICB0aGlzLnNlbGVjdGVkID0gdGhpcy5zZXR0aW5ncy5maW5kKHNldHRpbmcgPT4gc2V0dGluZy51cmwgPT09IHRoaXMucm91dGVyLnVybCkgfHwgKHt9IGFzIFNldHRpbmdUYWIpO1xuXG4gICAgaWYgKCF0aGlzLnNlbGVjdGVkLm5hbWUgJiYgdGhpcy5zZXR0aW5ncy5sZW5ndGgpIHtcbiAgICAgIHRoaXMuc2V0U2VsZWN0ZWQodGhpcy5zZXR0aW5nc1swXSk7XG4gICAgfVxuICB9XG5cbiAgc2V0U2VsZWN0ZWQoc2VsZWN0ZWQ6IFNldHRpbmdUYWIpIHtcbiAgICB0aGlzLnNlbGVjdGVkID0gc2VsZWN0ZWQ7XG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgTmF2aWdhdGUoW3NlbGVjdGVkLnVybF0pKTtcbiAgfVxufVxuIl19