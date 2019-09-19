/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { timer } from 'rxjs';
import { SettingManagementService } from '../services/setting-management.service';
var SettingLayoutComponent = /** @class */ (function () {
    function SettingLayoutComponent(settingManagementService, router) {
        this.settingManagementService = settingManagementService;
        this.router = router;
        this.trackByFn = (/**
         * @param {?} _
         * @param {?} item
         * @return {?}
         */
        function (_, item) { return item.name; });
        if (settingManagementService.selected &&
            this.router.url !== settingManagementService.selected.url &&
            settingManagementService.settings.length) {
            settingManagementService.setSelected(settingManagementService.settings[0]);
        }
    }
    /**
     * @return {?}
     */
    SettingLayoutComponent.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () { };
    /**
     * @return {?}
     */
    SettingLayoutComponent.prototype.ngAfterViewInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        timer(250).subscribe((/**
         * @return {?}
         */
        function () {
            if (!_this.settingManagementService.settings.length) {
                _this.settingManagementService.setSettings();
            }
        }));
    };
    // required for dynamic component
    SettingLayoutComponent.type = "setting" /* setting */;
    SettingLayoutComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-setting-layout',
                    template: "<div class=\"row entry-row\">\n  <div class=\"col-auto\">\n    <h1 class=\"content-header-title\">{{ 'AbpSettingManagement::Settings' | abpLocalization }}</h1>\n  </div>\n  <!-- <div id=\"breadcrumb\" class=\"col-md-auto pl-md-0\">\n    <abp-breadcrumb></abp-breadcrumb>\n  </div> -->\n  <div class=\"col\">\n    <div class=\"text-lg-right pt-2\" id=\"AbpContentToolbar\"></div>\n  </div>\n</div>\n\n<div id=\"SettingManagementWrapper\">\n  <div class=\"card\">\n    <div class=\"card-body\">\n      <div *ngIf=\"!settingManagementService.settings.length\" class=\"text-center\">\n        <i class=\"fa fa-spinner fa-spin\"></i>\n      </div>\n      <div class=\"row\">\n        <div class=\"col-3\">\n          <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\n            <li\n              *abpFor=\"\n                let setting of settingManagementService.settings;\n                trackBy: trackByFn;\n                orderBy: 'order';\n                orderDir: 'ASC'\n              \"\n              (click)=\"settingManagementService.setSelected(setting)\"\n              class=\"nav-item\"\n              [abpPermission]=\"setting.requiredPolicy\"\n            >\n              <a\n                class=\"nav-link\"\n                [id]=\"setting.name + '-tab'\"\n                role=\"tab\"\n                [class.active]=\"setting.name === settingManagementService.selected.name\"\n                >{{ setting.name | abpLocalization }}</a\n              >\n            </li>\n          </ul>\n        </div>\n        <div class=\"col-9\">\n          <div *ngIf=\"settingManagementService.settings.length\" class=\"tab-content\">\n            <div\n              class=\"tab-pane fade show active\"\n              [id]=\"settingManagementService.selected.name + '-tab'\"\n              role=\"tabpanel\"\n            >\n              <h2>{{ settingManagementService.selected.name | abpLocalization }}</h2>\n              <hr class=\"my-4\" />\n              <router-outlet></router-outlet>\n            </div>\n          </div>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n"
                }] }
    ];
    /** @nocollapse */
    SettingLayoutComponent.ctorParameters = function () { return [
        { type: SettingManagementService },
        { type: Router }
    ]; };
    return SettingLayoutComponent;
}());
export { SettingLayoutComponent };
if (false) {
    /** @type {?} */
    SettingLayoutComponent.type;
    /** @type {?} */
    SettingLayoutComponent.prototype.trackByFn;
    /** @type {?} */
    SettingLayoutComponent.prototype.settingManagementService;
    /**
     * @type {?}
     * @private
     */
    SettingLayoutComponent.prototype.router;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1sYXlvdXQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9zZXR0aW5nLWxheW91dC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUVBLE9BQU8sRUFBRSxTQUFTLEVBQW1CLE1BQU0sZUFBZSxDQUFDO0FBQzNELE9BQU8sRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUN6QyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzdCLE9BQU8sRUFBRSx3QkFBd0IsRUFBRSxNQUFNLHdDQUF3QyxDQUFDO0FBRWxGO0lBVUUsZ0NBQW1CLHdCQUFrRCxFQUFVLE1BQWM7UUFBMUUsNkJBQXdCLEdBQXhCLHdCQUF3QixDQUEwQjtRQUFVLFdBQU0sR0FBTixNQUFNLENBQVE7UUFGN0YsY0FBUzs7Ozs7UUFBZ0MsVUFBQyxDQUFDLEVBQUUsSUFBSSxJQUFLLE9BQUEsSUFBSSxDQUFDLElBQUksRUFBVCxDQUFTLEVBQUM7UUFHOUQsSUFDRSx3QkFBd0IsQ0FBQyxRQUFRO1lBQ2pDLElBQUksQ0FBQyxNQUFNLENBQUMsR0FBRyxLQUFLLHdCQUF3QixDQUFDLFFBQVEsQ0FBQyxHQUFHO1lBQ3pELHdCQUF3QixDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQ3hDO1lBQ0Esd0JBQXdCLENBQUMsV0FBVyxDQUFDLHdCQUF3QixDQUFDLFFBQVEsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDO1NBQzVFO0lBQ0gsQ0FBQzs7OztJQUVELDRDQUFXOzs7SUFBWCxjQUFlLENBQUM7Ozs7SUFFaEIsZ0RBQWU7OztJQUFmO1FBQUEsaUJBTUM7UUFMQyxLQUFLLENBQUMsR0FBRyxDQUFDLENBQUMsU0FBUzs7O1FBQUM7WUFDbkIsSUFBSSxDQUFDLEtBQUksQ0FBQyx3QkFBd0IsQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO2dCQUNsRCxLQUFJLENBQUMsd0JBQXdCLENBQUMsV0FBVyxFQUFFLENBQUM7YUFDN0M7UUFDSCxDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUM7O0lBdEJNLDJCQUFJLDJCQUF1Qjs7Z0JBTm5DLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsb0JBQW9CO29CQUM5Qiw2bEVBQThDO2lCQUMvQzs7OztnQkFMUSx3QkFBd0I7Z0JBRnhCLE1BQU07O0lBaUNmLDZCQUFDO0NBQUEsQUE3QkQsSUE2QkM7U0F6Qlksc0JBQXNCOzs7SUFFakMsNEJBQWtDOztJQUVsQywyQ0FBZ0U7O0lBRXBELDBEQUF5RDs7Ozs7SUFBRSx3Q0FBc0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBlTGF5b3V0VHlwZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBTZXR0aW5nVGFiIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgQ29tcG9uZW50LCBUcmFja0J5RnVuY3Rpb24gfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyB0aW1lciB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgU2V0dGluZ01hbmFnZW1lbnRTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvc2V0dGluZy1tYW5hZ2VtZW50LnNlcnZpY2UnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtc2V0dGluZy1sYXlvdXQnLFxuICB0ZW1wbGF0ZVVybDogJy4vc2V0dGluZy1sYXlvdXQuY29tcG9uZW50Lmh0bWwnLFxufSlcbmV4cG9ydCBjbGFzcyBTZXR0aW5nTGF5b3V0Q29tcG9uZW50IHtcbiAgLy8gcmVxdWlyZWQgZm9yIGR5bmFtaWMgY29tcG9uZW50XG4gIHN0YXRpYyB0eXBlID0gZUxheW91dFR5cGUuc2V0dGluZztcblxuICB0cmFja0J5Rm46IFRyYWNrQnlGdW5jdGlvbjxTZXR0aW5nVGFiPiA9IChfLCBpdGVtKSA9PiBpdGVtLm5hbWU7XG5cbiAgY29uc3RydWN0b3IocHVibGljIHNldHRpbmdNYW5hZ2VtZW50U2VydmljZTogU2V0dGluZ01hbmFnZW1lbnRTZXJ2aWNlLCBwcml2YXRlIHJvdXRlcjogUm91dGVyKSB7XG4gICAgaWYgKFxuICAgICAgc2V0dGluZ01hbmFnZW1lbnRTZXJ2aWNlLnNlbGVjdGVkICYmXG4gICAgICB0aGlzLnJvdXRlci51cmwgIT09IHNldHRpbmdNYW5hZ2VtZW50U2VydmljZS5zZWxlY3RlZC51cmwgJiZcbiAgICAgIHNldHRpbmdNYW5hZ2VtZW50U2VydmljZS5zZXR0aW5ncy5sZW5ndGhcbiAgICApIHtcbiAgICAgIHNldHRpbmdNYW5hZ2VtZW50U2VydmljZS5zZXRTZWxlY3RlZChzZXR0aW5nTWFuYWdlbWVudFNlcnZpY2Uuc2V0dGluZ3NbMF0pO1xuICAgIH1cbiAgfVxuXG4gIG5nT25EZXN0cm95KCkge31cblxuICBuZ0FmdGVyVmlld0luaXQoKSB7XG4gICAgdGltZXIoMjUwKS5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgaWYgKCF0aGlzLnNldHRpbmdNYW5hZ2VtZW50U2VydmljZS5zZXR0aW5ncy5sZW5ndGgpIHtcbiAgICAgICAgdGhpcy5zZXR0aW5nTWFuYWdlbWVudFNlcnZpY2Uuc2V0U2V0dGluZ3MoKTtcbiAgICAgIH1cbiAgICB9KTtcbiAgfVxufVxuIl19