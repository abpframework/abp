/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/setting-management.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
import { getSettingTabs } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { ConfigState } from '@abp/ng.core';
import { SettingManagementState } from '../states/setting-management.state';
import { SetSelectedSettingTab } from '../actions/setting-management.actions';
export class SettingManagementComponent {
    /**
     * @param {?} router
     * @param {?} store
     */
    constructor(router, store) {
        this.router = router;
        this.store = store;
        this.settings = [];
        this.trackByFn = (/**
         * @param {?} _
         * @param {?} item
         * @return {?}
         */
        (_, item) => item.name);
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set selected(value) {
        this.store.dispatch(new SetSelectedSettingTab(value));
    }
    /**
     * @return {?}
     */
    get selected() {
        /** @type {?} */
        const value = this.store.selectSnapshot(SettingManagementState.getSelectedTab);
        if ((!value || !value.component) && this.settings.length) {
            return this.settings[0];
        }
        return value;
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        this.settings = getSettingTabs()
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
        if (!this.selected && this.settings.length) {
            this.selected = this.settings[0];
        }
    }
}
SettingManagementComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-setting-management',
                template: "<div class=\"row entry-row\">\n  <div class=\"col-auto\">\n    <h1 class=\"content-header-title\">{{ 'AbpSettingManagement::Settings' | abpLocalization }}</h1>\n  </div>\n  <div id=\"breadcrumb\" class=\"col-md-auto pl-md-0\">\n    <abp-breadcrumb></abp-breadcrumb>\n  </div>\n  <div class=\"col\">\n    <div class=\"text-lg-right pt-2\" id=\"AbpContentToolbar\"></div>\n  </div>\n</div>\n\n<div id=\"SettingManagementWrapper\">\n  <div class=\"card\">\n    <div class=\"card-body\">\n      <div class=\"row\">\n        <div class=\"col-12 col-md-3\">\n          <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\n            <li\n              *abpFor=\"let setting of settings; trackBy: trackByFn\"\n              (click)=\"selected = setting\"\n              class=\"nav-item pointer\"\n              [abpPermission]=\"setting.requiredPolicy\"\n            >\n              <a\n                class=\"nav-link\"\n                [id]=\"setting.name + '-tab'\"\n                role=\"tab\"\n                [class.active]=\"setting.name === selected.name\"\n                >{{ setting.name | abpLocalization }}</a\n              >\n            </li>\n          </ul>\n        </div>\n        <div class=\"col-12 col-md-9\">\n          <div *ngIf=\"settings.length\" class=\"tab-content\">\n            <div class=\"tab-pane fade show active\" [id]=\"selected.name + '-tab'\" role=\"tabpanel\">\n              <ng-container *ngComponentOutlet=\"selected.component\"></ng-container>\n            </div>\n          </div>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n"
            }] }
];
/** @nocollapse */
SettingManagementComponent.ctorParameters = () => [
    { type: Router },
    { type: Store }
];
if (false) {
    /** @type {?} */
    SettingManagementComponent.prototype.settings;
    /** @type {?} */
    SettingManagementComponent.prototype.trackByFn;
    /**
     * @type {?}
     * @private
     */
    SettingManagementComponent.prototype.router;
    /**
     * @type {?}
     * @private
     */
    SettingManagementComponent.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuc2V0dGluZy1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQTJCLE1BQU0sZUFBZSxDQUFDO0FBQ25FLE9BQU8sRUFBYyxjQUFjLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUNsRSxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDekMsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzNDLE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxNQUFNLG9DQUFvQyxDQUFDO0FBQzVFLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLHVDQUF1QyxDQUFDO0FBTzlFLE1BQU0sT0FBTywwQkFBMEI7Ozs7O0lBa0JyQyxZQUFvQixNQUFjLEVBQVUsS0FBWTtRQUFwQyxXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQWpCeEQsYUFBUSxHQUFpQixFQUFFLENBQUM7UUFlNUIsY0FBUzs7Ozs7UUFBZ0MsQ0FBQyxDQUFDLEVBQUUsSUFBSSxFQUFFLEVBQUUsQ0FBQyxJQUFJLENBQUMsSUFBSSxFQUFDO0lBRUwsQ0FBQzs7Ozs7SUFmNUQsSUFBSSxRQUFRLENBQUMsS0FBaUI7UUFDNUIsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxxQkFBcUIsQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDO0lBQ3hELENBQUM7Ozs7SUFDRCxJQUFJLFFBQVE7O2NBQ0osS0FBSyxHQUFHLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLHNCQUFzQixDQUFDLGNBQWMsQ0FBQztRQUU5RSxJQUFJLENBQUMsQ0FBQyxLQUFLLElBQUksQ0FBQyxLQUFLLENBQUMsU0FBUyxDQUFDLElBQUksSUFBSSxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDeEQsT0FBTyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUMsQ0FBQyxDQUFDO1NBQ3pCO1FBRUQsT0FBTyxLQUFLLENBQUM7SUFDZixDQUFDOzs7O0lBTUQsUUFBUTtRQUNOLElBQUksQ0FBQyxRQUFRLEdBQUcsY0FBYyxFQUFFO2FBQzdCLE1BQU07Ozs7UUFBQyxPQUFPLENBQUMsRUFBRSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxnQkFBZ0IsQ0FBQyxPQUFPLENBQUMsY0FBYyxDQUFDLENBQUMsRUFBQzthQUNsRyxJQUFJOzs7OztRQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsRUFBRSxFQUFFLENBQUMsQ0FBQyxDQUFDLEtBQUssR0FBRyxDQUFDLENBQUMsS0FBSyxFQUFDLENBQUM7UUFFckMsSUFBSSxDQUFDLElBQUksQ0FBQyxRQUFRLElBQUksSUFBSSxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDMUMsSUFBSSxDQUFDLFFBQVEsR0FBRyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUMsQ0FBQyxDQUFDO1NBQ2xDO0lBQ0gsQ0FBQzs7O1lBaENGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsd0JBQXdCO2dCQUNsQyxvbERBQWtEO2FBQ25EOzs7O1lBVlEsTUFBTTtZQUNOLEtBQUs7Ozs7SUFXWiw4Q0FBNEI7O0lBZTVCLCtDQUFnRTs7Ozs7SUFFcEQsNENBQXNCOzs7OztJQUFFLDJDQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgVHJhY2tCeUZ1bmN0aW9uLCBPbkluaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFNldHRpbmdUYWIsIGdldFNldHRpbmdUYWJzIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgUm91dGVyIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgU2V0dGluZ01hbmFnZW1lbnRTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcy9zZXR0aW5nLW1hbmFnZW1lbnQuc3RhdGUnO1xuaW1wb3J0IHsgU2V0U2VsZWN0ZWRTZXR0aW5nVGFiIH0gZnJvbSAnLi4vYWN0aW9ucy9zZXR0aW5nLW1hbmFnZW1lbnQuYWN0aW9ucyc7XG5pbXBvcnQgeyBSb3V0ZXJTdGF0ZSB9IGZyb20gJ0BuZ3hzL3JvdXRlci1wbHVnaW4nO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtc2V0dGluZy1tYW5hZ2VtZW50JyxcbiAgdGVtcGxhdGVVcmw6ICcuL3NldHRpbmctbWFuYWdlbWVudC5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFNldHRpbmdNYW5hZ2VtZW50Q29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0IHtcbiAgc2V0dGluZ3M6IFNldHRpbmdUYWJbXSA9IFtdO1xuXG4gIHNldCBzZWxlY3RlZCh2YWx1ZTogU2V0dGluZ1RhYikge1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFNldFNlbGVjdGVkU2V0dGluZ1RhYih2YWx1ZSkpO1xuICB9XG4gIGdldCBzZWxlY3RlZCgpOiBTZXR0aW5nVGFiIHtcbiAgICBjb25zdCB2YWx1ZSA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoU2V0dGluZ01hbmFnZW1lbnRTdGF0ZS5nZXRTZWxlY3RlZFRhYik7XG5cbiAgICBpZiAoKCF2YWx1ZSB8fCAhdmFsdWUuY29tcG9uZW50KSAmJiB0aGlzLnNldHRpbmdzLmxlbmd0aCkge1xuICAgICAgcmV0dXJuIHRoaXMuc2V0dGluZ3NbMF07XG4gICAgfVxuXG4gICAgcmV0dXJuIHZhbHVlO1xuICB9XG5cbiAgdHJhY2tCeUZuOiBUcmFja0J5RnVuY3Rpb248U2V0dGluZ1RhYj4gPSAoXywgaXRlbSkgPT4gaXRlbS5uYW1lO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcm91dGVyOiBSb3V0ZXIsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIG5nT25Jbml0KCkge1xuICAgIHRoaXMuc2V0dGluZ3MgPSBnZXRTZXR0aW5nVGFicygpXG4gICAgICAuZmlsdGVyKHNldHRpbmcgPT4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRHcmFudGVkUG9saWN5KHNldHRpbmcucmVxdWlyZWRQb2xpY3kpKSlcbiAgICAgIC5zb3J0KChhLCBiKSA9PiBhLm9yZGVyIC0gYi5vcmRlcik7XG5cbiAgICBpZiAoIXRoaXMuc2VsZWN0ZWQgJiYgdGhpcy5zZXR0aW5ncy5sZW5ndGgpIHtcbiAgICAgIHRoaXMuc2VsZWN0ZWQgPSB0aGlzLnNldHRpbmdzWzBdO1xuICAgIH1cbiAgfVxufVxuIl19