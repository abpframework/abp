/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/manage-profile/manage-profile.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { fadeIn } from '@abp/ng.theme.shared';
import { transition, trigger, useAnimation } from '@angular/animations';
import { Component } from '@angular/core';
var ManageProfileComponent = /** @class */ (function () {
    function ManageProfileComponent() {
        this.selectedTab = 0;
    }
    ManageProfileComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-manage-profile',
                    template: "<div id=\"AbpContentToolbar\"></div>\n\n<div class=\"card border-0 shadow-sm\">\n  <div class=\"card-body\">\n    <div class=\"row\">\n      <div class=\"col-3\">\n        <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\n          <li class=\"nav-item\" (click)=\"selectedTab = 0\">\n            <a class=\"nav-link\" [ngClass]=\"{ active: selectedTab === 0 }\" role=\"tab\" href=\"javascript:void(0)\">{{\n              'AbpUi::ChangePassword' | abpLocalization\n            }}</a>\n          </li>\n          <li class=\"nav-item\" (click)=\"selectedTab = 1\">\n            <a class=\"nav-link\" [ngClass]=\"{ active: selectedTab === 1 }\" role=\"tab\" href=\"javascript:void(0)\">{{\n              'AbpAccount::PersonalSettings' | abpLocalization\n            }}</a>\n          </li>\n        </ul>\n      </div>\n      <div class=\"col-9\">\n        <div class=\"tab-content\" *ngIf=\"selectedTab === 0\" [@fadeIn]>\n          <div class=\"tab-pane active\" role=\"tabpanel\">\n            <h4>\n              {{ 'AbpIdentity::ChangePassword' | abpLocalization }}\n              <hr />\n            </h4>\n            <abp-change-password-form></abp-change-password-form>\n          </div>\n        </div>\n        <div class=\"tab-content\" *ngIf=\"selectedTab === 1\" [@fadeIn]>\n          <div class=\"tab-pane active\" role=\"tabpanel\">\n            <h4>\n              {{ 'AbpIdentity::PersonalSettings' | abpLocalization }}\n              <hr />\n            </h4>\n            <abp-personal-settings-form></abp-personal-settings-form>\n          </div>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n",
                    animations: [trigger('fadeIn', [transition(':enter', useAnimation(fadeIn))])]
                }] }
    ];
    return ManageProfileComponent;
}());
export { ManageProfileComponent };
if (false) {
    /** @type {?} */
    ManageProfileComponent.prototype.selectedTab;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWFuYWdlLXByb2ZpbGUuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvbWFuYWdlLXByb2ZpbGUvbWFuYWdlLXByb2ZpbGUuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQzlDLE9BQU8sRUFBRSxVQUFVLEVBQUUsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQ3hFLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFMUM7SUFBQTtRQU1FLGdCQUFXLEdBQUcsQ0FBQyxDQUFDO0lBQ2xCLENBQUM7O2dCQVBBLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsb0JBQW9CO29CQUM5QiwrbkRBQThDO29CQUM5QyxVQUFVLEVBQUUsQ0FBQyxPQUFPLENBQUMsUUFBUSxFQUFFLENBQUMsVUFBVSxDQUFDLFFBQVEsRUFBRSxZQUFZLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7aUJBQzlFOztJQUdELDZCQUFDO0NBQUEsQUFQRCxJQU9DO1NBRlksc0JBQXNCOzs7SUFDakMsNkNBQWdCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgZmFkZUluIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgdHJhbnNpdGlvbiwgdHJpZ2dlciwgdXNlQW5pbWF0aW9uIH0gZnJvbSAnQGFuZ3VsYXIvYW5pbWF0aW9ucyc7XG5pbXBvcnQgeyBDb21wb25lbnQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLW1hbmFnZS1wcm9maWxlJyxcbiAgdGVtcGxhdGVVcmw6ICcuL21hbmFnZS1wcm9maWxlLmNvbXBvbmVudC5odG1sJyxcbiAgYW5pbWF0aW9uczogW3RyaWdnZXIoJ2ZhZGVJbicsIFt0cmFuc2l0aW9uKCc6ZW50ZXInLCB1c2VBbmltYXRpb24oZmFkZUluKSldKV0sXG59KVxuZXhwb3J0IGNsYXNzIE1hbmFnZVByb2ZpbGVDb21wb25lbnQge1xuICBzZWxlY3RlZFRhYiA9IDA7XG59XG4iXX0=