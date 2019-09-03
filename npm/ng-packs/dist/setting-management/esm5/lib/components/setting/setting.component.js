/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
import { InitialService } from '../services/initial.service';
var SettingComponent = /** @class */ (function () {
    function SettingComponent(initialService) {
        this.initialService = initialService;
        this.selected = (/** @type {?} */ ({}));
    }
    /**
     * @return {?}
     */
    SettingComponent.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        this.settings = this.initialService.settings;
        this.selected = this.settings[0];
    };
    SettingComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-setting',
                    template: "<div class=\"row entry-row\">\n  <div class=\"col-auto\">\n    <h1 class=\"content-header-title\">{{ 'AbpSettingManagement::Settings' | abpLocalization }}</h1>\n  </div>\n  <div id=\"breadcrumb\" class=\"col-md-auto pl-md-0\">\n    <abp-breadcrumb></abp-breadcrumb>\n  </div>\n  <div class=\"col\">\n    <div class=\"text-lg-right pt-2\" id=\"AbpContentToolbar\"></div>\n  </div>\n</div>\n\n<div id=\"SettingManagementWrapper\">\n  <div class=\"card\">\n    <div class=\"card-body\">\n      <div class=\"row\">\n        <div class=\"col-3\">\n          <ul class=\"nav flex-column nav-pills\" id=\"T515ccf3324254f41a4a9a6b34b0dae56\" role=\"tablist\">\n            <li\n              *ngFor=\"let setting of settings\"\n              (click)=\"selected = setting\"\n              class=\"nav-item\"\n              [abpPermission]=\"setting.requiredPolicy\"\n            >\n              <a\n                class=\"nav-link\"\n                [id]=\"setting.name + '-tab'\"\n                role=\"tab\"\n                [class.active]=\"setting.name === selected.name\"\n                >{{ setting.name | abpLocalization }}</a\n              >\n            </li>\n          </ul>\n        </div>\n        <div class=\"col-9\">\n          <div class=\"tab-content\">\n            <div class=\"tab-pane fade show active\" [id]=\"selected.name + '-tab'\" role=\"tabpanel\">\n              <h2>{{ selected.name | abpLocalization }}</h2>\n              <hr class=\"my-4\" />\n\n              <ng-container\n                *ngComponentOutlet=\"selected.component\"\n                [abpPermission]=\"selected.requiredPolicy\"\n              ></ng-container>\n            </div>\n          </div>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n"
                }] }
    ];
    /** @nocollapse */
    SettingComponent.ctorParameters = function () { return [
        { type: InitialService }
    ]; };
    return SettingComponent;
}());
export { SettingComponent };
if (false) {
    /** @type {?} */
    SettingComponent.prototype.settings;
    /** @type {?} */
    SettingComponent.prototype.selected;
    /**
     * @type {?}
     * @private
     */
    SettingComponent.prototype.initialService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnNldHRpbmctbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL3NldHRpbmcvc2V0dGluZy5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFFbEQsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLDZCQUE2QixDQUFDO0FBRTdEO0lBU0UsMEJBQW9CLGNBQThCO1FBQTlCLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtRQUZsRCxhQUFRLEdBQUcsbUJBQUEsRUFBRSxFQUFjLENBQUM7SUFFeUIsQ0FBQzs7OztJQUV0RCxtQ0FBUTs7O0lBQVI7UUFDRSxJQUFJLENBQUMsUUFBUSxHQUFHLElBQUksQ0FBQyxjQUFjLENBQUMsUUFBUSxDQUFDO1FBQzdDLElBQUksQ0FBQyxRQUFRLEdBQUcsSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDLENBQUMsQ0FBQztJQUNuQyxDQUFDOztnQkFkRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGFBQWE7b0JBQ3ZCLHN1REFBdUM7aUJBQ3hDOzs7O2dCQUxRLGNBQWM7O0lBaUJ2Qix1QkFBQztDQUFBLEFBZkQsSUFlQztTQVhZLGdCQUFnQjs7O0lBQzNCLG9DQUF1Qjs7SUFFdkIsb0NBQTRCOzs7OztJQUVoQiwwQ0FBc0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU2V0dGluZ1RhYiwgZmFkZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IEluaXRpYWxTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvaW5pdGlhbC5zZXJ2aWNlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXNldHRpbmcnLFxuICB0ZW1wbGF0ZVVybDogJy4vc2V0dGluZy5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFNldHRpbmdDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBzZXR0aW5nczogU2V0dGluZ1RhYltdO1xuXG4gIHNlbGVjdGVkID0ge30gYXMgU2V0dGluZ1RhYjtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGluaXRpYWxTZXJ2aWNlOiBJbml0aWFsU2VydmljZSkge31cblxuICBuZ09uSW5pdCgpIHtcbiAgICB0aGlzLnNldHRpbmdzID0gdGhpcy5pbml0aWFsU2VydmljZS5zZXR0aW5ncztcbiAgICB0aGlzLnNlbGVjdGVkID0gdGhpcy5zZXR0aW5nc1swXTtcbiAgfVxufVxuIl19