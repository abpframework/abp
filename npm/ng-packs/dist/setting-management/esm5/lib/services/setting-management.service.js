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
var SettingManagementService = /** @class */ (function () {
    function SettingManagementService(actions, router, store, oAuthService) {
        var _this = this;
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
        function () { return _this.setSettings(); }), 0);
        this.actions
            .pipe(ofActionSuccessful(GetAppConfiguration))
            .pipe(takeUntil(this.destroy$))
            .subscribe((/**
         * @return {?}
         */
        function () {
            if (_this.oAuthService.hasValidAccessToken()) {
                _this.setSettings();
            }
        }));
    }
    /**
     * @return {?}
     */
    SettingManagementService.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () {
        this.destroy$.next();
    };
    /**
     * @return {?}
     */
    SettingManagementService.prototype.setSettings = /**
     * @return {?}
     */
    function () {
        var _this = this;
        /** @type {?} */
        var route = this.router.config.find((/**
         * @param {?} r
         * @return {?}
         */
        function (r) { return r.path === 'setting-management'; }));
        this.settings = ((/** @type {?} */ (route.data.settings)))
            .filter((/**
         * @param {?} setting
         * @return {?}
         */
        function (setting) { return _this.store.selectSnapshot(ConfigState.getGrantedPolicy(setting.requiredPolicy)); }))
            .sort((/**
         * @param {?} a
         * @param {?} b
         * @return {?}
         */
        function (a, b) { return a.order - b.order; }));
        this.checkSelected();
    };
    /**
     * @return {?}
     */
    SettingManagementService.prototype.checkSelected = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.selected = this.settings.find((/**
         * @param {?} setting
         * @return {?}
         */
        function (setting) { return setting.url === _this.router.url; })) || ((/** @type {?} */ ({})));
        if (!this.selected.name && this.settings.length) {
            this.setSelected(this.settings[0]);
        }
    };
    /**
     * @param {?} selected
     * @return {?}
     */
    SettingManagementService.prototype.setSelected = /**
     * @param {?} selected
     * @return {?}
     */
    function (selected) {
        this.selected = selected;
        this.store.dispatch(new Navigate([selected.url]));
    };
    SettingManagementService.decorators = [
        { type: Injectable, args: [{ providedIn: 'root' },] }
    ];
    /** @nocollapse */
    SettingManagementService.ctorParameters = function () { return [
        { type: Actions },
        { type: Router },
        { type: Store },
        { type: OAuthService }
    ]; };
    /** @nocollapse */ SettingManagementService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function SettingManagementService_Factory() { return new SettingManagementService(i0.ɵɵinject(i1.Actions), i0.ɵɵinject(i2.Router), i0.ɵɵinject(i1.Store), i0.ɵɵinject(i3.OAuthService)); }, token: SettingManagementService, providedIn: "root" });
    return SettingManagementService;
}());
export { SettingManagementService };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnNldHRpbmctbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9zZXR0aW5nLW1hbmFnZW1lbnQuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQ0EsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDekMsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQy9DLE9BQU8sRUFBRSxLQUFLLEVBQUUsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ2pFLE9BQU8sRUFBRSxPQUFPLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDL0IsT0FBTyxFQUFFLFdBQVcsRUFBRSxtQkFBbUIsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNoRSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0scUJBQXFCLENBQUM7QUFDbkQsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLGdCQUFnQixDQUFDOzs7OztBQUUzQztJQVFFLGtDQUNVLE9BQWdCLEVBQ2hCLE1BQWMsRUFDZCxLQUFZLEVBQ1osWUFBMEI7UUFKcEMsaUJBZ0JDO1FBZlMsWUFBTyxHQUFQLE9BQU8sQ0FBUztRQUNoQixXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQ2QsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUNaLGlCQUFZLEdBQVosWUFBWSxDQUFjO1FBVnBDLGFBQVEsR0FBaUIsRUFBRSxDQUFDO1FBRTVCLGFBQVEsR0FBRyxtQkFBQSxFQUFFLEVBQWMsQ0FBQztRQUVwQixhQUFRLEdBQUcsSUFBSSxPQUFPLEVBQUUsQ0FBQztRQVEvQixVQUFVOzs7UUFBQyxjQUFNLE9BQUEsS0FBSSxDQUFDLFdBQVcsRUFBRSxFQUFsQixDQUFrQixHQUFFLENBQUMsQ0FBQyxDQUFDO1FBRXhDLElBQUksQ0FBQyxPQUFPO2FBQ1QsSUFBSSxDQUFDLGtCQUFrQixDQUFDLG1CQUFtQixDQUFDLENBQUM7YUFDN0MsSUFBSSxDQUFDLFNBQVMsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUM7YUFDOUIsU0FBUzs7O1FBQUM7WUFDVCxJQUFJLEtBQUksQ0FBQyxZQUFZLENBQUMsbUJBQW1CLEVBQUUsRUFBRTtnQkFDM0MsS0FBSSxDQUFDLFdBQVcsRUFBRSxDQUFDO2FBQ3BCO1FBQ0gsQ0FBQyxFQUFDLENBQUM7SUFDUCxDQUFDOzs7O0lBRUQsOENBQVc7OztJQUFYO1FBQ0UsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLEVBQUUsQ0FBQztJQUN2QixDQUFDOzs7O0lBRUQsOENBQVc7OztJQUFYO1FBQUEsaUJBTUM7O1lBTE8sS0FBSyxHQUFHLElBQUksQ0FBQyxNQUFNLENBQUMsTUFBTSxDQUFDLElBQUk7Ozs7UUFBQyxVQUFBLENBQUMsSUFBSSxPQUFBLENBQUMsQ0FBQyxJQUFJLEtBQUssb0JBQW9CLEVBQS9CLENBQStCLEVBQUM7UUFDM0UsSUFBSSxDQUFDLFFBQVEsR0FBRyxDQUFDLG1CQUFBLEtBQUssQ0FBQyxJQUFJLENBQUMsUUFBUSxFQUFnQixDQUFDO2FBQ2xELE1BQU07Ozs7UUFBQyxVQUFBLE9BQU8sSUFBSSxPQUFBLEtBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxnQkFBZ0IsQ0FBQyxPQUFPLENBQUMsY0FBYyxDQUFDLENBQUMsRUFBL0UsQ0FBK0UsRUFBQzthQUNsRyxJQUFJOzs7OztRQUFDLFVBQUMsQ0FBQyxFQUFFLENBQUMsSUFBSyxPQUFBLENBQUMsQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDLEtBQUssRUFBakIsQ0FBaUIsRUFBQyxDQUFDO1FBQ3JDLElBQUksQ0FBQyxhQUFhLEVBQUUsQ0FBQztJQUN2QixDQUFDOzs7O0lBRUQsZ0RBQWE7OztJQUFiO1FBQUEsaUJBTUM7UUFMQyxJQUFJLENBQUMsUUFBUSxHQUFHLElBQUksQ0FBQyxRQUFRLENBQUMsSUFBSTs7OztRQUFDLFVBQUEsT0FBTyxJQUFJLE9BQUEsT0FBTyxDQUFDLEdBQUcsS0FBSyxLQUFJLENBQUMsTUFBTSxDQUFDLEdBQUcsRUFBL0IsQ0FBK0IsRUFBQyxJQUFJLENBQUMsbUJBQUEsRUFBRSxFQUFjLENBQUMsQ0FBQztRQUVyRyxJQUFJLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLElBQUksSUFBSSxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDL0MsSUFBSSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7U0FDcEM7SUFDSCxDQUFDOzs7OztJQUVELDhDQUFXOzs7O0lBQVgsVUFBWSxRQUFvQjtRQUM5QixJQUFJLENBQUMsUUFBUSxHQUFHLFFBQVEsQ0FBQztRQUN6QixJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLFFBQVEsQ0FBQyxDQUFDLFFBQVEsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLENBQUM7SUFDcEQsQ0FBQzs7Z0JBakRGLFVBQVUsU0FBQyxFQUFFLFVBQVUsRUFBRSxNQUFNLEVBQUU7Ozs7Z0JBTmxCLE9BQU87Z0JBRmQsTUFBTTtnQkFFTixLQUFLO2dCQUdMLFlBQVk7OzttQ0FQckI7Q0E0REMsQUFsREQsSUFrREM7U0FqRFksd0JBQXdCOzs7SUFDbkMsNENBQTRCOztJQUU1Qiw0Q0FBNEI7Ozs7O0lBRTVCLDRDQUFpQzs7Ozs7SUFHL0IsMkNBQXdCOzs7OztJQUN4QiwwQ0FBc0I7Ozs7O0lBQ3RCLHlDQUFvQjs7Ozs7SUFDcEIsZ0RBQWtDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU2V0dGluZ1RhYiB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBOYXZpZ2F0ZSB9IGZyb20gJ0BuZ3hzL3JvdXRlci1wbHVnaW4nO1xuaW1wb3J0IHsgU3RvcmUsIEFjdGlvbnMsIG9mQWN0aW9uU3VjY2Vzc2Z1bCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IFN1YmplY3QgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IENvbmZpZ1N0YXRlLCBHZXRBcHBDb25maWd1cmF0aW9uIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IE9BdXRoU2VydmljZSB9IGZyb20gJ2FuZ3VsYXItb2F1dGgyLW9pZGMnO1xuaW1wb3J0IHsgdGFrZVVudGlsIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuXG5ASW5qZWN0YWJsZSh7IHByb3ZpZGVkSW46ICdyb290JyB9KVxuZXhwb3J0IGNsYXNzIFNldHRpbmdNYW5hZ2VtZW50U2VydmljZSB7XG4gIHNldHRpbmdzOiBTZXR0aW5nVGFiW10gPSBbXTtcblxuICBzZWxlY3RlZCA9IHt9IGFzIFNldHRpbmdUYWI7XG5cbiAgcHJpdmF0ZSBkZXN0cm95JCA9IG5ldyBTdWJqZWN0KCk7XG5cbiAgY29uc3RydWN0b3IoXG4gICAgcHJpdmF0ZSBhY3Rpb25zOiBBY3Rpb25zLFxuICAgIHByaXZhdGUgcm91dGVyOiBSb3V0ZXIsXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXG4gICAgcHJpdmF0ZSBvQXV0aFNlcnZpY2U6IE9BdXRoU2VydmljZSxcbiAgKSB7XG4gICAgc2V0VGltZW91dCgoKSA9PiB0aGlzLnNldFNldHRpbmdzKCksIDApO1xuXG4gICAgdGhpcy5hY3Rpb25zXG4gICAgICAucGlwZShvZkFjdGlvblN1Y2Nlc3NmdWwoR2V0QXBwQ29uZmlndXJhdGlvbikpXG4gICAgICAucGlwZSh0YWtlVW50aWwodGhpcy5kZXN0cm95JCkpXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgaWYgKHRoaXMub0F1dGhTZXJ2aWNlLmhhc1ZhbGlkQWNjZXNzVG9rZW4oKSkge1xuICAgICAgICAgIHRoaXMuc2V0U2V0dGluZ3MoKTtcbiAgICAgICAgfVxuICAgICAgfSk7XG4gIH1cblxuICBuZ09uRGVzdHJveSgpIHtcbiAgICB0aGlzLmRlc3Ryb3kkLm5leHQoKTtcbiAgfVxuXG4gIHNldFNldHRpbmdzKCkge1xuICAgIGNvbnN0IHJvdXRlID0gdGhpcy5yb3V0ZXIuY29uZmlnLmZpbmQociA9PiByLnBhdGggPT09ICdzZXR0aW5nLW1hbmFnZW1lbnQnKTtcbiAgICB0aGlzLnNldHRpbmdzID0gKHJvdXRlLmRhdGEuc2V0dGluZ3MgYXMgU2V0dGluZ1RhYltdKVxuICAgICAgLmZpbHRlcihzZXR0aW5nID0+IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0R3JhbnRlZFBvbGljeShzZXR0aW5nLnJlcXVpcmVkUG9saWN5KSkpXG4gICAgICAuc29ydCgoYSwgYikgPT4gYS5vcmRlciAtIGIub3JkZXIpO1xuICAgIHRoaXMuY2hlY2tTZWxlY3RlZCgpO1xuICB9XG5cbiAgY2hlY2tTZWxlY3RlZCgpIHtcbiAgICB0aGlzLnNlbGVjdGVkID0gdGhpcy5zZXR0aW5ncy5maW5kKHNldHRpbmcgPT4gc2V0dGluZy51cmwgPT09IHRoaXMucm91dGVyLnVybCkgfHwgKHt9IGFzIFNldHRpbmdUYWIpO1xuXG4gICAgaWYgKCF0aGlzLnNlbGVjdGVkLm5hbWUgJiYgdGhpcy5zZXR0aW5ncy5sZW5ndGgpIHtcbiAgICAgIHRoaXMuc2V0U2VsZWN0ZWQodGhpcy5zZXR0aW5nc1swXSk7XG4gICAgfVxuICB9XG5cbiAgc2V0U2VsZWN0ZWQoc2VsZWN0ZWQ6IFNldHRpbmdUYWIpIHtcbiAgICB0aGlzLnNlbGVjdGVkID0gc2VsZWN0ZWQ7XG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgTmF2aWdhdGUoW3NlbGVjdGVkLnVybF0pKTtcbiAgfVxufVxuIl19