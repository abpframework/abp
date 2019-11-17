/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/manage-profile/manage-profile.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { fadeIn } from '@abp/ng.theme.shared';
import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
export class ManageProfileComponent {
    constructor() {
        this.selectedTab = 0;
    }
}
ManageProfileComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-manage-profile',
                template: "<div id=\"AbpContentToolbar\"></div>\r\n\r\n<div class=\"card border-0 shadow-sm\">\r\n  <div class=\"card-body\">\r\n    <div class=\"row\">\r\n      <div class=\"col-3\">\r\n        <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\r\n          <li class=\"nav-item\" (click)=\"selectedTab = 0\">\r\n            <a class=\"nav-link\" [ngClass]=\"{ active: selectedTab === 0 }\" role=\"tab\" href=\"javascript:void(0)\">{{\r\n              'AbpUi::ChangePassword' | abpLocalization\r\n            }}</a>\r\n          </li>\r\n          <li class=\"nav-item\" (click)=\"selectedTab = 1\">\r\n            <a class=\"nav-link\" [ngClass]=\"{ active: selectedTab === 1 }\" role=\"tab\" href=\"javascript:void(0)\">{{\r\n              'AbpAccount::PersonalSettings' | abpLocalization\r\n            }}</a>\r\n          </li>\r\n        </ul>\r\n      </div>\r\n      <div class=\"col-9\">\r\n        <div class=\"tab-content\" *ngIf=\"selectedTab === 0\" [@fadeIn]>\r\n          <div class=\"tab-pane active\" role=\"tabpanel\">\r\n            <h4>\r\n              {{ 'AbpIdentity::ChangePassword' | abpLocalization }}\r\n              <hr />\r\n            </h4>\r\n            <abp-change-password-form></abp-change-password-form>\r\n          </div>\r\n        </div>\r\n        <div class=\"tab-content\" *ngIf=\"selectedTab === 1\" [@fadeIn]>\r\n          <div class=\"tab-pane active\" role=\"tabpanel\">\r\n            <h4>\r\n              {{ 'AbpIdentity::PersonalSettings' | abpLocalization }}\r\n              <hr />\r\n            </h4>\r\n            <abp-personal-settings-form></abp-personal-settings-form>\r\n          </div>\r\n        </div>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n",
                animations: [trigger('fadeIn', [transition(':enter', useAnimation(fadeIn))])]
            }] }
];
if (false) {
    /** @type {?} */
    ManageProfileComponent.prototype.selectedTab;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWFuYWdlLXByb2ZpbGUuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvbWFuYWdlLXByb2ZpbGUvbWFuYWdlLXByb2ZpbGUuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQzlDLE9BQU8sRUFBRSxVQUFVLEVBQUUsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQ3hFLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFPMUMsTUFBTSxPQUFPLHNCQUFzQjtJQUxuQztRQU1FLGdCQUFXLEdBQUcsQ0FBQyxDQUFDO0lBQ2xCLENBQUM7OztZQVBBLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsb0JBQW9CO2dCQUM5QixtdERBQThDO2dCQUM5QyxVQUFVLEVBQUUsQ0FBQyxPQUFPLENBQUMsUUFBUSxFQUFFLENBQUMsVUFBVSxDQUFDLFFBQVEsRUFBRSxZQUFZLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7YUFDOUU7Ozs7SUFFQyw2Q0FBZ0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBmYWRlSW4gfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XHJcbmltcG9ydCB7IHRyYW5zaXRpb24sIHRyaWdnZXIsIHVzZUFuaW1hdGlvbiB9IGZyb20gJ0Bhbmd1bGFyL2FuaW1hdGlvbnMnO1xyXG5pbXBvcnQgeyBDb21wb25lbnQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLW1hbmFnZS1wcm9maWxlJyxcclxuICB0ZW1wbGF0ZVVybDogJy4vbWFuYWdlLXByb2ZpbGUuY29tcG9uZW50Lmh0bWwnLFxyXG4gIGFuaW1hdGlvbnM6IFt0cmlnZ2VyKCdmYWRlSW4nLCBbdHJhbnNpdGlvbignOmVudGVyJywgdXNlQW5pbWF0aW9uKGZhZGVJbikpXSldLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgTWFuYWdlUHJvZmlsZUNvbXBvbmVudCB7XHJcbiAgc2VsZWN0ZWRUYWIgPSAwO1xyXG59XHJcbiJdfQ==