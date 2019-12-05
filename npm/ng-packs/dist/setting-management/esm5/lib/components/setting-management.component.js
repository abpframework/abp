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
                    template: "<div class=\"row entry-row\">\n  <div class=\"col-auto\">\n    <h1 class=\"content-header-title\">{{ 'AbpSettingManagement::Settings' | abpLocalization }}</h1>\n  </div>\n  <div id=\"breadcrumb\" class=\"col-md-auto pl-md-0\">\n    <abp-breadcrumb></abp-breadcrumb>\n  </div>\n  <div class=\"col\">\n    <div class=\"text-lg-right pt-2\" id=\"AbpContentToolbar\"></div>\n  </div>\n</div>\n\n<div id=\"SettingManagementWrapper\">\n  <div class=\"card\">\n    <div class=\"card-body\">\n      <div class=\"row\">\n        <div class=\"col-12 col-md-3\">\n          <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\n            <li\n              *abpFor=\"let setting of settings; trackBy: trackByFn\"\n              (click)=\"selected = setting\"\n              class=\"nav-item pointer\"\n              [abpPermission]=\"setting.requiredPolicy\"\n            >\n              <a\n                class=\"nav-link\"\n                [id]=\"setting.name + '-tab'\"\n                role=\"tab\"\n                [class.active]=\"setting.name === selected.name\"\n                >{{ setting.name | abpLocalization }}</a\n              >\n            </li>\n          </ul>\n        </div>\n        <div class=\"col-12 col-md-9\">\n          <div *ngIf=\"settings.length\" class=\"tab-content\">\n            <div class=\"tab-pane fade show active\" [id]=\"selected.name + '-tab'\" role=\"tabpanel\">\n              <ng-container *ngComponentOutlet=\"selected.component\"></ng-container>\n            </div>\n          </div>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuc2V0dGluZy1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQTJCLE1BQU0sZUFBZSxDQUFDO0FBQ25FLE9BQU8sRUFBYyxjQUFjLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUNsRSxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFDekMsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQzNDLE9BQU8sRUFBRSxzQkFBc0IsRUFBRSxNQUFNLG9DQUFvQyxDQUFDO0FBQzVFLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLHVDQUF1QyxDQUFDO0FBRzlFO0lBc0JFLG9DQUFvQixNQUFjLEVBQVUsS0FBWTtRQUFwQyxXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQWpCeEQsYUFBUSxHQUFpQixFQUFFLENBQUM7UUFlNUIsY0FBUzs7Ozs7UUFBZ0MsVUFBQyxDQUFDLEVBQUUsSUFBSSxJQUFLLE9BQUEsSUFBSSxDQUFDLElBQUksRUFBVCxDQUFTLEVBQUM7SUFFTCxDQUFDO0lBZjVELHNCQUFJLGdEQUFROzs7O1FBR1o7O2dCQUNRLEtBQUssR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxzQkFBc0IsQ0FBQyxjQUFjLENBQUM7WUFFOUUsSUFBSSxDQUFDLENBQUMsS0FBSyxJQUFJLENBQUMsS0FBSyxDQUFDLFNBQVMsQ0FBQyxJQUFJLElBQUksQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO2dCQUN4RCxPQUFPLElBQUksQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDLENBQUM7YUFDekI7WUFFRCxPQUFPLEtBQUssQ0FBQztRQUNmLENBQUM7Ozs7O1FBWEQsVUFBYSxLQUFpQjtZQUM1QixJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLHFCQUFxQixDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUM7UUFDeEQsQ0FBQzs7O09BQUE7Ozs7SUFlRCw2Q0FBUTs7O0lBQVI7UUFBQSxpQkFRQztRQVBDLElBQUksQ0FBQyxRQUFRLEdBQUcsY0FBYyxFQUFFO2FBQzdCLE1BQU07Ozs7UUFBQyxVQUFBLE9BQU8sSUFBSSxPQUFBLEtBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxnQkFBZ0IsQ0FBQyxPQUFPLENBQUMsY0FBYyxDQUFDLENBQUMsRUFBL0UsQ0FBK0UsRUFBQzthQUNsRyxJQUFJOzs7OztRQUFDLFVBQUMsQ0FBQyxFQUFFLENBQUMsSUFBSyxPQUFBLENBQUMsQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDLEtBQUssRUFBakIsQ0FBaUIsRUFBQyxDQUFDO1FBRXJDLElBQUksQ0FBQyxJQUFJLENBQUMsUUFBUSxJQUFJLElBQUksQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO1lBQzFDLElBQUksQ0FBQyxRQUFRLEdBQUcsSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDLENBQUMsQ0FBQztTQUNsQztJQUNILENBQUM7O2dCQWhDRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLHdCQUF3QjtvQkFDbEMsb2xEQUFrRDtpQkFDbkQ7Ozs7Z0JBVlEsTUFBTTtnQkFDTixLQUFLOztJQXVDZCxpQ0FBQztDQUFBLEFBakNELElBaUNDO1NBN0JZLDBCQUEwQjs7O0lBQ3JDLDhDQUE0Qjs7SUFlNUIsK0NBQWdFOzs7OztJQUVwRCw0Q0FBc0I7Ozs7O0lBQUUsMkNBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBUcmFja0J5RnVuY3Rpb24sIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU2V0dGluZ1RhYiwgZ2V0U2V0dGluZ1RhYnMgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBTZXR0aW5nTWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL3NldHRpbmctbWFuYWdlbWVudC5zdGF0ZSc7XG5pbXBvcnQgeyBTZXRTZWxlY3RlZFNldHRpbmdUYWIgfSBmcm9tICcuLi9hY3Rpb25zL3NldHRpbmctbWFuYWdlbWVudC5hY3Rpb25zJztcbmltcG9ydCB7IFJvdXRlclN0YXRlIH0gZnJvbSAnQG5neHMvcm91dGVyLXBsdWdpbic7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1zZXR0aW5nLW1hbmFnZW1lbnQnLFxuICB0ZW1wbGF0ZVVybDogJy4vc2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgU2V0dGluZ01hbmFnZW1lbnRDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBzZXR0aW5nczogU2V0dGluZ1RhYltdID0gW107XG5cbiAgc2V0IHNlbGVjdGVkKHZhbHVlOiBTZXR0aW5nVGFiKSB7XG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgU2V0U2VsZWN0ZWRTZXR0aW5nVGFiKHZhbHVlKSk7XG4gIH1cbiAgZ2V0IHNlbGVjdGVkKCk6IFNldHRpbmdUYWIge1xuICAgIGNvbnN0IHZhbHVlID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXR0aW5nTWFuYWdlbWVudFN0YXRlLmdldFNlbGVjdGVkVGFiKTtcblxuICAgIGlmICgoIXZhbHVlIHx8ICF2YWx1ZS5jb21wb25lbnQpICYmIHRoaXMuc2V0dGluZ3MubGVuZ3RoKSB7XG4gICAgICByZXR1cm4gdGhpcy5zZXR0aW5nc1swXTtcbiAgICB9XG5cbiAgICByZXR1cm4gdmFsdWU7XG4gIH1cblxuICB0cmFja0J5Rm46IFRyYWNrQnlGdW5jdGlvbjxTZXR0aW5nVGFiPiA9IChfLCBpdGVtKSA9PiBpdGVtLm5hbWU7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSByb3V0ZXI6IFJvdXRlciwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgbmdPbkluaXQoKSB7XG4gICAgdGhpcy5zZXR0aW5ncyA9IGdldFNldHRpbmdUYWJzKClcbiAgICAgIC5maWx0ZXIoc2V0dGluZyA9PiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEdyYW50ZWRQb2xpY3koc2V0dGluZy5yZXF1aXJlZFBvbGljeSkpKVxuICAgICAgLnNvcnQoKGEsIGIpID0+IGEub3JkZXIgLSBiLm9yZGVyKTtcblxuICAgIGlmICghdGhpcy5zZWxlY3RlZCAmJiB0aGlzLnNldHRpbmdzLmxlbmd0aCkge1xuICAgICAgdGhpcy5zZWxlY3RlZCA9IHRoaXMuc2V0dGluZ3NbMF07XG4gICAgfVxuICB9XG59XG4iXX0=