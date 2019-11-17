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
var SettingManagementComponent = /** @class */ (function () {
    function SettingManagementComponent(router, store) {
        this.router = router;
        this.store = store;
        this.settings = [];
        this.trackByFn = (/**
         * @param {?} _
         * @param {?} item
         * @return {?}
         */
        function (_, item) { return item.name; });
    }
    Object.defineProperty(SettingManagementComponent.prototype, "selected", {
        get: /**
         * @return {?}
         */
        function () {
            /** @type {?} */
            var value = this.store.selectSnapshot(SettingManagementState.getSelectedTab);
            if ((!value || !value.component) && this.settings.length) {
                return this.settings[0];
            }
            return value;
        },
        set: /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this.store.dispatch(new SetSelectedSettingTab(value));
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @return {?}
     */
    SettingManagementComponent.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.settings = getSettingTabs()
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
        if (!this.selected && this.settings.length) {
            this.selected = this.settings[0];
        }
    };
    SettingManagementComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-setting-management',
                    template: "<div class=\"row entry-row\">\r\n  <div class=\"col-auto\">\r\n    <h1 class=\"content-header-title\">{{ 'AbpSettingManagement::Settings' | abpLocalization }}</h1>\r\n  </div>\r\n  <div id=\"breadcrumb\" class=\"col-md-auto pl-md-0\">\r\n    <abp-breadcrumb></abp-breadcrumb>\r\n  </div>\r\n  <div class=\"col\">\r\n    <div class=\"text-lg-right pt-2\" id=\"AbpContentToolbar\"></div>\r\n  </div>\r\n</div>\r\n\r\n<div id=\"SettingManagementWrapper\">\r\n  <div class=\"card\">\r\n    <div class=\"card-body\">\r\n      <div class=\"row\">\r\n        <div class=\"col-12 col-md-3\">\r\n          <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\r\n            <li\r\n              *abpFor=\"let setting of settings; trackBy: trackByFn\"\r\n              (click)=\"selected = setting\"\r\n              class=\"nav-item\"\r\n              [abpPermission]=\"setting.requiredPolicy\"\r\n            >\r\n              <a\r\n                class=\"nav-link\"\r\n                [id]=\"setting.name + '-tab'\"\r\n                role=\"tab\"\r\n                [class.active]=\"setting.name === selected.name\"\r\n                >{{ setting.name | abpLocalization }}</a\r\n              >\r\n            </li>\r\n          </ul>\r\n        </div>\r\n        <div class=\"col-12 col-md-9\">\r\n          <div *ngIf=\"settings.length\" class=\"tab-content\">\r\n            <div class=\"tab-pane fade show active\" [id]=\"selected.name + '-tab'\" role=\"tabpanel\">\r\n              <ng-container *ngComponentOutlet=\"selected.component\"></ng-container>\r\n            </div>\r\n          </div>\r\n        </div>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n"
                }] }
    ];
    /** @nocollapse */
    SettingManagementComponent.ctorParameters = function () { return [
        { type: Router },
        { type: Store }
    ]; };
    return SettingManagementComponent;
}());
export { SettingManagementComponent };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuc2V0dGluZy1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQTJCLE1BQU0sZUFBZSxDQUFDO0FBQ25FLE9BQU8sRUFBYyxjQUFjLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUNsRSxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDekMsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzNDLE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxNQUFNLG9DQUFvQyxDQUFDO0FBQzVFLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLHVDQUF1QyxDQUFDO0FBRzlFO0lBc0JFLG9DQUFvQixNQUFjLEVBQVUsS0FBWTtRQUFwQyxXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQWpCeEQsYUFBUSxHQUFpQixFQUFFLENBQUM7UUFlNUIsY0FBUzs7Ozs7UUFBZ0MsVUFBQyxDQUFDLEVBQUUsSUFBSSxJQUFLLE9BQUEsSUFBSSxDQUFDLElBQUksRUFBVCxDQUFTLEVBQUM7SUFFTCxDQUFDO0lBZjVELHNCQUFJLGdEQUFROzs7O1FBR1o7O2dCQUNRLEtBQUssR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxzQkFBc0IsQ0FBQyxjQUFjLENBQUM7WUFFOUUsSUFBSSxDQUFDLENBQUMsS0FBSyxJQUFJLENBQUMsS0FBSyxDQUFDLFNBQVMsQ0FBQyxJQUFJLElBQUksQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO2dCQUN4RCxPQUFPLElBQUksQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDLENBQUM7YUFDekI7WUFFRCxPQUFPLEtBQUssQ0FBQztRQUNmLENBQUM7Ozs7O1FBWEQsVUFBYSxLQUFpQjtZQUM1QixJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLHFCQUFxQixDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUM7UUFDeEQsQ0FBQzs7O09BQUE7Ozs7SUFlRCw2Q0FBUTs7O0lBQVI7UUFBQSxpQkFRQztRQVBDLElBQUksQ0FBQyxRQUFRLEdBQUcsY0FBYyxFQUFFO2FBQzdCLE1BQU07Ozs7UUFBQyxVQUFBLE9BQU8sSUFBSSxPQUFBLEtBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxnQkFBZ0IsQ0FBQyxPQUFPLENBQUMsY0FBYyxDQUFDLENBQUMsRUFBL0UsQ0FBK0UsRUFBQzthQUNsRyxJQUFJOzs7OztRQUFDLFVBQUMsQ0FBQyxFQUFFLENBQUMsSUFBSyxPQUFBLENBQUMsQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDLEtBQUssRUFBakIsQ0FBaUIsRUFBQyxDQUFDO1FBRXJDLElBQUksQ0FBQyxJQUFJLENBQUMsUUFBUSxJQUFJLElBQUksQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO1lBQzFDLElBQUksQ0FBQyxRQUFRLEdBQUcsSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDLENBQUMsQ0FBQztTQUNsQztJQUNILENBQUM7O2dCQWhDRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLHdCQUF3QjtvQkFDbEMsc3FEQUFrRDtpQkFDbkQ7Ozs7Z0JBVlEsTUFBTTtnQkFDTixLQUFLOztJQXVDZCxpQ0FBQztDQUFBLEFBakNELElBaUNDO1NBN0JZLDBCQUEwQjs7O0lBQ3JDLDhDQUE0Qjs7SUFlNUIsK0NBQWdFOzs7OztJQUVwRCw0Q0FBc0I7Ozs7O0lBQUUsMkNBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBUcmFja0J5RnVuY3Rpb24sIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBTZXR0aW5nVGFiLCBnZXRTZXR0aW5nVGFicyB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcclxuaW1wb3J0IHsgUm91dGVyIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcclxuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgU2V0dGluZ01hbmFnZW1lbnRTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcy9zZXR0aW5nLW1hbmFnZW1lbnQuc3RhdGUnO1xyXG5pbXBvcnQgeyBTZXRTZWxlY3RlZFNldHRpbmdUYWIgfSBmcm9tICcuLi9hY3Rpb25zL3NldHRpbmctbWFuYWdlbWVudC5hY3Rpb25zJztcclxuaW1wb3J0IHsgUm91dGVyU3RhdGUgfSBmcm9tICdAbmd4cy9yb3V0ZXItcGx1Z2luJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLXNldHRpbmctbWFuYWdlbWVudCcsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL3NldHRpbmctbWFuYWdlbWVudC5jb21wb25lbnQuaHRtbCcsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBTZXR0aW5nTWFuYWdlbWVudENvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XHJcbiAgc2V0dGluZ3M6IFNldHRpbmdUYWJbXSA9IFtdO1xyXG5cclxuICBzZXQgc2VsZWN0ZWQodmFsdWU6IFNldHRpbmdUYWIpIHtcclxuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFNldFNlbGVjdGVkU2V0dGluZ1RhYih2YWx1ZSkpO1xyXG4gIH1cclxuICBnZXQgc2VsZWN0ZWQoKTogU2V0dGluZ1RhYiB7XHJcbiAgICBjb25zdCB2YWx1ZSA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoU2V0dGluZ01hbmFnZW1lbnRTdGF0ZS5nZXRTZWxlY3RlZFRhYik7XHJcblxyXG4gICAgaWYgKCghdmFsdWUgfHwgIXZhbHVlLmNvbXBvbmVudCkgJiYgdGhpcy5zZXR0aW5ncy5sZW5ndGgpIHtcclxuICAgICAgcmV0dXJuIHRoaXMuc2V0dGluZ3NbMF07XHJcbiAgICB9XHJcblxyXG4gICAgcmV0dXJuIHZhbHVlO1xyXG4gIH1cclxuXHJcbiAgdHJhY2tCeUZuOiBUcmFja0J5RnVuY3Rpb248U2V0dGluZ1RhYj4gPSAoXywgaXRlbSkgPT4gaXRlbS5uYW1lO1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJvdXRlcjogUm91dGVyLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cclxuXHJcbiAgbmdPbkluaXQoKSB7XHJcbiAgICB0aGlzLnNldHRpbmdzID0gZ2V0U2V0dGluZ1RhYnMoKVxyXG4gICAgICAuZmlsdGVyKHNldHRpbmcgPT4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRHcmFudGVkUG9saWN5KHNldHRpbmcucmVxdWlyZWRQb2xpY3kpKSlcclxuICAgICAgLnNvcnQoKGEsIGIpID0+IGEub3JkZXIgLSBiLm9yZGVyKTtcclxuXHJcbiAgICBpZiAoIXRoaXMuc2VsZWN0ZWQgJiYgdGhpcy5zZXR0aW5ncy5sZW5ndGgpIHtcclxuICAgICAgdGhpcy5zZWxlY3RlZCA9IHRoaXMuc2V0dGluZ3NbMF07XHJcbiAgICB9XHJcbiAgfVxyXG59XHJcbiJdfQ==