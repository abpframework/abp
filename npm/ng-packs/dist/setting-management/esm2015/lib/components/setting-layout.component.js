/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { timer } from 'rxjs';
import { SettingManagementService } from '../services/setting-management.service';
export class SettingLayoutComponent {
    /**
     * @param {?} settingManagementService
     * @param {?} router
     */
    constructor(settingManagementService, router) {
        this.settingManagementService = settingManagementService;
        this.router = router;
        this.trackByFn = (/**
         * @param {?} _
         * @param {?} item
         * @return {?}
         */
        (_, item) => item.name);
        if (settingManagementService.selected &&
            this.router.url !== settingManagementService.selected.url &&
            settingManagementService.settings.length) {
            settingManagementService.setSelected(settingManagementService.settings[0]);
        }
    }
    /**
     * @return {?}
     */
    ngOnDestroy() { }
    /**
     * @return {?}
     */
    ngAfterViewInit() {
        timer(250).subscribe((/**
         * @return {?}
         */
        () => {
            if (!this.settingManagementService.settings.length) {
                this.settingManagementService.setSettings();
            }
        }));
    }
}
// required for dynamic component
SettingLayoutComponent.type = "setting" /* setting */;
SettingLayoutComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-setting-layout',
                template: "<div class=\"row entry-row\">\n  <div class=\"col-auto\">\n    <h1 class=\"content-header-title\">{{ 'AbpSettingManagement::Settings' | abpLocalization }}</h1>\n  </div>\n  <!-- <div id=\"breadcrumb\" class=\"col-md-auto pl-md-0\">\n    <abp-breadcrumb></abp-breadcrumb>\n  </div> -->\n  <div class=\"col\">\n    <div class=\"text-lg-right pt-2\" id=\"AbpContentToolbar\"></div>\n  </div>\n</div>\n\n<div id=\"SettingManagementWrapper\">\n  <div class=\"card\">\n    <div class=\"card-body\">\n      <div *ngIf=\"!settingManagementService.settings.length\" class=\"text-center\">\n        <i class=\"fa fa-spinner fa-spin\"></i>\n      </div>\n      <div class=\"row\">\n        <div class=\"col-3\">\n          <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\n            <li\n              *abpFor=\"\n                let setting of settingManagementService.settings;\n                trackBy: trackByFn;\n                orderBy: 'order';\n                orderDir: 'ASC'\n              \"\n              (click)=\"settingManagementService.setSelected(setting)\"\n              class=\"nav-item\"\n              [abpPermission]=\"setting.requiredPolicy\"\n            >\n              <a\n                class=\"nav-link\"\n                [id]=\"setting.name + '-tab'\"\n                role=\"tab\"\n                [class.active]=\"setting.name === settingManagementService.selected.name\"\n                >{{ setting.name | abpLocalization }}</a\n              >\n            </li>\n          </ul>\n        </div>\n        <div class=\"col-9\">\n          <div *ngIf=\"settingManagementService.settings.length\" class=\"tab-content\">\n            <div\n              class=\"tab-pane fade show active\"\n              [id]=\"settingManagementService.selected.name + '-tab'\"\n              role=\"tabpanel\"\n            >\n              <h2>{{ settingManagementService.selected.name | abpLocalization }}</h2>\n              <hr class=\"my-4\" />\n              <router-outlet></router-outlet>\n            </div>\n          </div>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n"
            }] }
];
/** @nocollapse */
SettingLayoutComponent.ctorParameters = () => [
    { type: SettingManagementService },
    { type: Router }
];
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1sYXlvdXQuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9zZXR0aW5nLWxheW91dC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUVBLE9BQU8sRUFBRSxTQUFTLEVBQW1CLE1BQU0sZUFBZSxDQUFDO0FBQzNELE9BQU8sRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUN6QyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzdCLE9BQU8sRUFBRSx3QkFBd0IsRUFBRSxNQUFNLHdDQUF3QyxDQUFDO0FBTWxGLE1BQU0sT0FBTyxzQkFBc0I7Ozs7O0lBTWpDLFlBQW1CLHdCQUFrRCxFQUFVLE1BQWM7UUFBMUUsNkJBQXdCLEdBQXhCLHdCQUF3QixDQUEwQjtRQUFVLFdBQU0sR0FBTixNQUFNLENBQVE7UUFGN0YsY0FBUzs7Ozs7UUFBZ0MsQ0FBQyxDQUFDLEVBQUUsSUFBSSxFQUFFLEVBQUUsQ0FBQyxJQUFJLENBQUMsSUFBSSxFQUFDO1FBRzlELElBQ0Usd0JBQXdCLENBQUMsUUFBUTtZQUNqQyxJQUFJLENBQUMsTUFBTSxDQUFDLEdBQUcsS0FBSyx3QkFBd0IsQ0FBQyxRQUFRLENBQUMsR0FBRztZQUN6RCx3QkFBd0IsQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUN4QztZQUNBLHdCQUF3QixDQUFDLFdBQVcsQ0FBQyx3QkFBd0IsQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQztTQUM1RTtJQUNILENBQUM7Ozs7SUFFRCxXQUFXLEtBQUksQ0FBQzs7OztJQUVoQixlQUFlO1FBQ2IsS0FBSyxDQUFDLEdBQUcsQ0FBQyxDQUFDLFNBQVM7OztRQUFDLEdBQUcsRUFBRTtZQUN4QixJQUFJLENBQUMsSUFBSSxDQUFDLHdCQUF3QixDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7Z0JBQ2xELElBQUksQ0FBQyx3QkFBd0IsQ0FBQyxXQUFXLEVBQUUsQ0FBQzthQUM3QztRQUNILENBQUMsRUFBQyxDQUFDO0lBQ0wsQ0FBQzs7O0FBdEJNLDJCQUFJLDJCQUF1Qjs7WUFObkMsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxvQkFBb0I7Z0JBQzlCLDZsRUFBOEM7YUFDL0M7Ozs7WUFMUSx3QkFBd0I7WUFGeEIsTUFBTTs7OztJQVViLDRCQUFrQzs7SUFFbEMsMkNBQWdFOztJQUVwRCwwREFBeUQ7Ozs7O0lBQUUsd0NBQXNCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgZUxheW91dFR5cGUgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgU2V0dGluZ1RhYiB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IENvbXBvbmVudCwgVHJhY2tCeUZ1bmN0aW9uIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgdGltZXIgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IFNldHRpbmdNYW5hZ2VtZW50U2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL3NldHRpbmctbWFuYWdlbWVudC5zZXJ2aWNlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXNldHRpbmctbGF5b3V0JyxcbiAgdGVtcGxhdGVVcmw6ICcuL3NldHRpbmctbGF5b3V0LmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgU2V0dGluZ0xheW91dENvbXBvbmVudCB7XG4gIC8vIHJlcXVpcmVkIGZvciBkeW5hbWljIGNvbXBvbmVudFxuICBzdGF0aWMgdHlwZSA9IGVMYXlvdXRUeXBlLnNldHRpbmc7XG5cbiAgdHJhY2tCeUZuOiBUcmFja0J5RnVuY3Rpb248U2V0dGluZ1RhYj4gPSAoXywgaXRlbSkgPT4gaXRlbS5uYW1lO1xuXG4gIGNvbnN0cnVjdG9yKHB1YmxpYyBzZXR0aW5nTWFuYWdlbWVudFNlcnZpY2U6IFNldHRpbmdNYW5hZ2VtZW50U2VydmljZSwgcHJpdmF0ZSByb3V0ZXI6IFJvdXRlcikge1xuICAgIGlmIChcbiAgICAgIHNldHRpbmdNYW5hZ2VtZW50U2VydmljZS5zZWxlY3RlZCAmJlxuICAgICAgdGhpcy5yb3V0ZXIudXJsICE9PSBzZXR0aW5nTWFuYWdlbWVudFNlcnZpY2Uuc2VsZWN0ZWQudXJsICYmXG4gICAgICBzZXR0aW5nTWFuYWdlbWVudFNlcnZpY2Uuc2V0dGluZ3MubGVuZ3RoXG4gICAgKSB7XG4gICAgICBzZXR0aW5nTWFuYWdlbWVudFNlcnZpY2Uuc2V0U2VsZWN0ZWQoc2V0dGluZ01hbmFnZW1lbnRTZXJ2aWNlLnNldHRpbmdzWzBdKTtcbiAgICB9XG4gIH1cblxuICBuZ09uRGVzdHJveSgpIHt9XG5cbiAgbmdBZnRlclZpZXdJbml0KCkge1xuICAgIHRpbWVyKDI1MCkuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgIGlmICghdGhpcy5zZXR0aW5nTWFuYWdlbWVudFNlcnZpY2Uuc2V0dGluZ3MubGVuZ3RoKSB7XG4gICAgICAgIHRoaXMuc2V0dGluZ01hbmFnZW1lbnRTZXJ2aWNlLnNldFNldHRpbmdzKCk7XG4gICAgICB9XG4gICAgfSk7XG4gIH1cbn1cbiJdfQ==