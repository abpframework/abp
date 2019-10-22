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
                    template: "<div class=\"row entry-row\">\r\n  <div class=\"col-auto\">\r\n    <h1 class=\"content-header-title\">{{ 'AbpSettingManagement::Settings' | abpLocalization }}</h1>\r\n  </div>\r\n  <div id=\"breadcrumb\" class=\"col-md-auto pl-md-0\">\r\n    <abp-breadcrumb></abp-breadcrumb>\r\n  </div>\r\n  <div class=\"col\">\r\n    <div class=\"text-lg-right pt-2\" id=\"AbpContentToolbar\"></div>\r\n  </div>\r\n</div>\r\n\r\n<div id=\"SettingManagementWrapper\">\r\n  <div class=\"card\">\r\n    <div class=\"card-body\">\r\n      <div class=\"row\">\r\n        <div class=\"col-3\">\r\n          <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\r\n            <li\r\n              *abpFor=\"let setting of settings; trackBy: trackByFn\"\r\n              (click)=\"selected = setting\"\r\n              class=\"nav-item\"\r\n              [abpPermission]=\"setting.requiredPolicy\"\r\n            >\r\n              <a\r\n                class=\"nav-link\"\r\n                [id]=\"setting.name + '-tab'\"\r\n                role=\"tab\"\r\n                [class.active]=\"setting.name === selected.name\"\r\n                >{{ setting.name | abpLocalization }}</a\r\n              >\r\n            </li>\r\n          </ul>\r\n        </div>\r\n        <div class=\"col-9\">\r\n          <div *ngIf=\"settings.length\" class=\"tab-content\">\r\n            <div class=\"tab-pane fade show active\" [id]=\"selected.name + '-tab'\" role=\"tabpanel\">\r\n              <ng-container *ngComponentOutlet=\"selected.component\"></ng-container>\r\n            </div>\r\n          </div>\r\n        </div>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuc2V0dGluZy1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBMkIsTUFBTSxlQUFlLENBQUM7QUFDbkUsT0FBTyxFQUFjLGNBQWMsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ2xFLE9BQU8sRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUN6QyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDM0MsT0FBTyxFQUFFLHNCQUFzQixFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDNUUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sdUNBQXVDLENBQUM7QUFHOUU7SUFzQkUsb0NBQW9CLE1BQWMsRUFBVSxLQUFZO1FBQXBDLFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBakJ4RCxhQUFRLEdBQWlCLEVBQUUsQ0FBQztRQWU1QixjQUFTOzs7OztRQUFnQyxVQUFDLENBQUMsRUFBRSxJQUFJLElBQUssT0FBQSxJQUFJLENBQUMsSUFBSSxFQUFULENBQVMsRUFBQztJQUVMLENBQUM7SUFmNUQsc0JBQUksZ0RBQVE7Ozs7UUFHWjs7Z0JBQ1EsS0FBSyxHQUFHLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLHNCQUFzQixDQUFDLGNBQWMsQ0FBQztZQUU5RSxJQUFJLENBQUMsQ0FBQyxLQUFLLElBQUksQ0FBQyxLQUFLLENBQUMsU0FBUyxDQUFDLElBQUksSUFBSSxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7Z0JBQ3hELE9BQU8sSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDLENBQUMsQ0FBQzthQUN6QjtZQUVELE9BQU8sS0FBSyxDQUFDO1FBQ2YsQ0FBQzs7Ozs7UUFYRCxVQUFhLEtBQWlCO1lBQzVCLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUkscUJBQXFCLENBQUMsS0FBSyxDQUFDLENBQUMsQ0FBQztRQUN4RCxDQUFDOzs7T0FBQTs7OztJQWVELDZDQUFROzs7SUFBUjtRQUFBLGlCQVFDO1FBUEMsSUFBSSxDQUFDLFFBQVEsR0FBRyxjQUFjLEVBQUU7YUFDN0IsTUFBTTs7OztRQUFDLFVBQUEsT0FBTyxJQUFJLE9BQUEsS0FBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLGdCQUFnQixDQUFDLE9BQU8sQ0FBQyxjQUFjLENBQUMsQ0FBQyxFQUEvRSxDQUErRSxFQUFDO2FBQ2xHLElBQUk7Ozs7O1FBQUMsVUFBQyxDQUFDLEVBQUUsQ0FBQyxJQUFLLE9BQUEsQ0FBQyxDQUFDLEtBQUssR0FBRyxDQUFDLENBQUMsS0FBSyxFQUFqQixDQUFpQixFQUFDLENBQUM7UUFFckMsSUFBSSxDQUFDLElBQUksQ0FBQyxRQUFRLElBQUksSUFBSSxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDMUMsSUFBSSxDQUFDLFFBQVEsR0FBRyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUMsQ0FBQyxDQUFDO1NBQ2xDO0lBQ0gsQ0FBQzs7Z0JBaENGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsd0JBQXdCO29CQUNsQyxrcERBQWtEO2lCQUNuRDs7OztnQkFWUSxNQUFNO2dCQUNOLEtBQUs7O0lBdUNkLGlDQUFDO0NBQUEsQUFqQ0QsSUFpQ0M7U0E3QlksMEJBQTBCOzs7SUFDckMsOENBQTRCOztJQWU1QiwrQ0FBZ0U7Ozs7O0lBRXBELDRDQUFzQjs7Ozs7SUFBRSwyQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIFRyYWNrQnlGdW5jdGlvbiwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFNldHRpbmdUYWIsIGdldFNldHRpbmdUYWJzIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xyXG5pbXBvcnQgeyBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xyXG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5pbXBvcnQgeyBTZXR0aW5nTWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL3NldHRpbmctbWFuYWdlbWVudC5zdGF0ZSc7XHJcbmltcG9ydCB7IFNldFNlbGVjdGVkU2V0dGluZ1RhYiB9IGZyb20gJy4uL2FjdGlvbnMvc2V0dGluZy1tYW5hZ2VtZW50LmFjdGlvbnMnO1xyXG5pbXBvcnQgeyBSb3V0ZXJTdGF0ZSB9IGZyb20gJ0BuZ3hzL3JvdXRlci1wbHVnaW4nO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdhYnAtc2V0dGluZy1tYW5hZ2VtZW50JyxcclxuICB0ZW1wbGF0ZVVybDogJy4vc2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC5odG1sJyxcclxufSlcclxuZXhwb3J0IGNsYXNzIFNldHRpbmdNYW5hZ2VtZW50Q29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0IHtcclxuICBzZXR0aW5nczogU2V0dGluZ1RhYltdID0gW107XHJcblxyXG4gIHNldCBzZWxlY3RlZCh2YWx1ZTogU2V0dGluZ1RhYikge1xyXG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgU2V0U2VsZWN0ZWRTZXR0aW5nVGFiKHZhbHVlKSk7XHJcbiAgfVxyXG4gIGdldCBzZWxlY3RlZCgpOiBTZXR0aW5nVGFiIHtcclxuICAgIGNvbnN0IHZhbHVlID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXR0aW5nTWFuYWdlbWVudFN0YXRlLmdldFNlbGVjdGVkVGFiKTtcclxuXHJcbiAgICBpZiAoKCF2YWx1ZSB8fCAhdmFsdWUuY29tcG9uZW50KSAmJiB0aGlzLnNldHRpbmdzLmxlbmd0aCkge1xyXG4gICAgICByZXR1cm4gdGhpcy5zZXR0aW5nc1swXTtcclxuICAgIH1cclxuXHJcbiAgICByZXR1cm4gdmFsdWU7XHJcbiAgfVxyXG5cclxuICB0cmFja0J5Rm46IFRyYWNrQnlGdW5jdGlvbjxTZXR0aW5nVGFiPiA9IChfLCBpdGVtKSA9PiBpdGVtLm5hbWU7XHJcblxyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcm91dGVyOiBSb3V0ZXIsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxyXG5cclxuICBuZ09uSW5pdCgpIHtcclxuICAgIHRoaXMuc2V0dGluZ3MgPSBnZXRTZXR0aW5nVGFicygpXHJcbiAgICAgIC5maWx0ZXIoc2V0dGluZyA9PiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEdyYW50ZWRQb2xpY3koc2V0dGluZy5yZXF1aXJlZFBvbGljeSkpKVxyXG4gICAgICAuc29ydCgoYSwgYikgPT4gYS5vcmRlciAtIGIub3JkZXIpO1xyXG5cclxuICAgIGlmICghdGhpcy5zZWxlY3RlZCAmJiB0aGlzLnNldHRpbmdzLmxlbmd0aCkge1xyXG4gICAgICB0aGlzLnNlbGVjdGVkID0gdGhpcy5zZXR0aW5nc1swXTtcclxuICAgIH1cclxuICB9XHJcbn1cclxuIl19