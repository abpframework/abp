/**
 * @fileoverview added by tsickle
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
                template: "<div class=\"row entry-row\">\r\n  <div class=\"col-auto\">\r\n    <h1 class=\"content-header-title\">{{ 'AbpSettingManagement::Settings' | abpLocalization }}</h1>\r\n  </div>\r\n  <div id=\"breadcrumb\" class=\"col-md-auto pl-md-0\">\r\n    <abp-breadcrumb></abp-breadcrumb>\r\n  </div>\r\n  <div class=\"col\">\r\n    <div class=\"text-lg-right pt-2\" id=\"AbpContentToolbar\"></div>\r\n  </div>\r\n</div>\r\n\r\n<div id=\"SettingManagementWrapper\">\r\n  <div class=\"card\">\r\n    <div class=\"card-body\">\r\n      <div class=\"row\">\r\n        <div class=\"col-3\">\r\n          <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\r\n            <li\r\n              *abpFor=\"let setting of settings; trackBy: trackByFn\"\r\n              (click)=\"selected = setting\"\r\n              class=\"nav-item\"\r\n              [abpPermission]=\"setting.requiredPolicy\"\r\n            >\r\n              <a\r\n                class=\"nav-link\"\r\n                [id]=\"setting.name + '-tab'\"\r\n                role=\"tab\"\r\n                [class.active]=\"setting.name === selected.name\"\r\n                >{{ setting.name | abpLocalization }}</a\r\n              >\r\n            </li>\r\n          </ul>\r\n        </div>\r\n        <div class=\"col-9\">\r\n          <div *ngIf=\"settings.length\" class=\"tab-content\">\r\n            <div class=\"tab-pane fade show active\" [id]=\"selected.name + '-tab'\" role=\"tabpanel\">\r\n              <ng-container *ngComponentOutlet=\"selected.component\"></ng-container>\r\n            </div>\r\n          </div>\r\n        </div>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuc2V0dGluZy1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBMkIsTUFBTSxlQUFlLENBQUM7QUFDbkUsT0FBTyxFQUFjLGNBQWMsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ2xFLE9BQU8sRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUN6QyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDM0MsT0FBTyxFQUFFLHNCQUFzQixFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDNUUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sdUNBQXVDLENBQUM7QUFPOUUsTUFBTSxPQUFPLDBCQUEwQjs7Ozs7SUFrQnJDLFlBQW9CLE1BQWMsRUFBVSxLQUFZO1FBQXBDLFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBakJ4RCxhQUFRLEdBQWlCLEVBQUUsQ0FBQztRQWU1QixjQUFTOzs7OztRQUFnQyxDQUFDLENBQUMsRUFBRSxJQUFJLEVBQUUsRUFBRSxDQUFDLElBQUksQ0FBQyxJQUFJLEVBQUM7SUFFTCxDQUFDOzs7OztJQWY1RCxJQUFJLFFBQVEsQ0FBQyxLQUFpQjtRQUM1QixJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLHFCQUFxQixDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUM7SUFDeEQsQ0FBQzs7OztJQUNELElBQUksUUFBUTs7Y0FDSixLQUFLLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsc0JBQXNCLENBQUMsY0FBYyxDQUFDO1FBRTlFLElBQUksQ0FBQyxDQUFDLEtBQUssSUFBSSxDQUFDLEtBQUssQ0FBQyxTQUFTLENBQUMsSUFBSSxJQUFJLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUN4RCxPQUFPLElBQUksQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDLENBQUM7U0FDekI7UUFFRCxPQUFPLEtBQUssQ0FBQztJQUNmLENBQUM7Ozs7SUFNRCxRQUFRO1FBQ04sSUFBSSxDQUFDLFFBQVEsR0FBRyxjQUFjLEVBQUU7YUFDN0IsTUFBTTs7OztRQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLGdCQUFnQixDQUFDLE9BQU8sQ0FBQyxjQUFjLENBQUMsQ0FBQyxFQUFDO2FBQ2xHLElBQUk7Ozs7O1FBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxFQUFFLEVBQUUsQ0FBQyxDQUFDLENBQUMsS0FBSyxHQUFHLENBQUMsQ0FBQyxLQUFLLEVBQUMsQ0FBQztRQUVyQyxJQUFJLENBQUMsSUFBSSxDQUFDLFFBQVEsSUFBSSxJQUFJLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUMxQyxJQUFJLENBQUMsUUFBUSxHQUFHLElBQUksQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDLENBQUM7U0FDbEM7SUFDSCxDQUFDOzs7WUFoQ0YsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSx3QkFBd0I7Z0JBQ2xDLGtwREFBa0Q7YUFDbkQ7Ozs7WUFWUSxNQUFNO1lBQ04sS0FBSzs7OztJQVdaLDhDQUE0Qjs7SUFlNUIsK0NBQWdFOzs7OztJQUVwRCw0Q0FBc0I7Ozs7O0lBQUUsMkNBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBUcmFja0J5RnVuY3Rpb24sIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBTZXR0aW5nVGFiLCBnZXRTZXR0aW5nVGFicyB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcclxuaW1wb3J0IHsgUm91dGVyIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcclxuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgU2V0dGluZ01hbmFnZW1lbnRTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcy9zZXR0aW5nLW1hbmFnZW1lbnQuc3RhdGUnO1xyXG5pbXBvcnQgeyBTZXRTZWxlY3RlZFNldHRpbmdUYWIgfSBmcm9tICcuLi9hY3Rpb25zL3NldHRpbmctbWFuYWdlbWVudC5hY3Rpb25zJztcclxuaW1wb3J0IHsgUm91dGVyU3RhdGUgfSBmcm9tICdAbmd4cy9yb3V0ZXItcGx1Z2luJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLXNldHRpbmctbWFuYWdlbWVudCcsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL3NldHRpbmctbWFuYWdlbWVudC5jb21wb25lbnQuaHRtbCcsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBTZXR0aW5nTWFuYWdlbWVudENvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XHJcbiAgc2V0dGluZ3M6IFNldHRpbmdUYWJbXSA9IFtdO1xyXG5cclxuICBzZXQgc2VsZWN0ZWQodmFsdWU6IFNldHRpbmdUYWIpIHtcclxuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFNldFNlbGVjdGVkU2V0dGluZ1RhYih2YWx1ZSkpO1xyXG4gIH1cclxuICBnZXQgc2VsZWN0ZWQoKTogU2V0dGluZ1RhYiB7XHJcbiAgICBjb25zdCB2YWx1ZSA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoU2V0dGluZ01hbmFnZW1lbnRTdGF0ZS5nZXRTZWxlY3RlZFRhYik7XHJcblxyXG4gICAgaWYgKCghdmFsdWUgfHwgIXZhbHVlLmNvbXBvbmVudCkgJiYgdGhpcy5zZXR0aW5ncy5sZW5ndGgpIHtcclxuICAgICAgcmV0dXJuIHRoaXMuc2V0dGluZ3NbMF07XHJcbiAgICB9XHJcblxyXG4gICAgcmV0dXJuIHZhbHVlO1xyXG4gIH1cclxuXHJcbiAgdHJhY2tCeUZuOiBUcmFja0J5RnVuY3Rpb248U2V0dGluZ1RhYj4gPSAoXywgaXRlbSkgPT4gaXRlbS5uYW1lO1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJvdXRlcjogUm91dGVyLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cclxuXHJcbiAgbmdPbkluaXQoKSB7XHJcbiAgICB0aGlzLnNldHRpbmdzID0gZ2V0U2V0dGluZ1RhYnMoKVxyXG4gICAgICAuZmlsdGVyKHNldHRpbmcgPT4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRHcmFudGVkUG9saWN5KHNldHRpbmcucmVxdWlyZWRQb2xpY3kpKSlcclxuICAgICAgLnNvcnQoKGEsIGIpID0+IGEub3JkZXIgLSBiLm9yZGVyKTtcclxuXHJcbiAgICBpZiAoIXRoaXMuc2VsZWN0ZWQgJiYgdGhpcy5zZXR0aW5ncy5sZW5ndGgpIHtcclxuICAgICAgdGhpcy5zZWxlY3RlZCA9IHRoaXMuc2V0dGluZ3NbMF07XHJcbiAgICB9XHJcbiAgfVxyXG59XHJcbiJdfQ==