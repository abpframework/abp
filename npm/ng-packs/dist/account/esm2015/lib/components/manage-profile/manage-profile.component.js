/**
 * @fileoverview added by tsickle
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
                template: "<div class=\"row entry-row\">\r\n  <div class=\"col-auto\"></div>\r\n  <div id=\"breadcrumb\" class=\"col-md-auto pl-md-0\"></div>\r\n  <div class=\"col\"></div>\r\n</div>\r\n\r\n<div id=\"ManageProfileWrapper\">\r\n  <div class=\"row\">\r\n    <div class=\"col-3\">\r\n      <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\r\n        <li class=\"nav-item pointer\" (click)=\"selectedTab = 0\">\r\n          <a class=\"nav-link\" [ngClass]=\"{ active: selectedTab === 0 }\" role=\"tab\">{{\r\n            'AbpUi::ChangePassword' | abpLocalization\r\n          }}</a>\r\n        </li>\r\n        <li class=\"nav-item pointer\" (click)=\"selectedTab = 1\">\r\n          <a class=\"nav-link\" [ngClass]=\"{ active: selectedTab === 1 }\" role=\"tab\">{{\r\n            'AbpAccount::PersonalSettings' | abpLocalization\r\n          }}</a>\r\n        </li>\r\n      </ul>\r\n    </div>\r\n    <div class=\"col-9\">\r\n      <div class=\"tab-content\" *ngIf=\"selectedTab === 0\" [@fadeIn]>\r\n        <div class=\"tab-pane active\" role=\"tabpanel\">\r\n          <abp-change-password-form></abp-change-password-form>\r\n        </div>\r\n      </div>\r\n      <div class=\"tab-content\" *ngIf=\"selectedTab === 1\" [@fadeIn]>\r\n        <div class=\"tab-pane active\" role=\"tabpanel\">\r\n          <abp-personal-settings-form></abp-personal-settings-form>\r\n        </div>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n",
                animations: [trigger('fadeIn', [transition(':enter', useAnimation(fadeIn))])]
            }] }
];
if (false) {
    /** @type {?} */
    ManageProfileComponent.prototype.selectedTab;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibWFuYWdlLXByb2ZpbGUuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvbWFuYWdlLXByb2ZpbGUvbWFuYWdlLXByb2ZpbGUuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDOUMsT0FBTyxFQUFFLFVBQVUsRUFBRSxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0scUJBQXFCLENBQUM7QUFDeEUsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQU8xQyxNQUFNLE9BQU8sc0JBQXNCO0lBTG5DO1FBTUUsZ0JBQVcsR0FBRyxDQUFDLENBQUM7SUFDbEIsQ0FBQzs7O1lBUEEsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxvQkFBb0I7Z0JBQzlCLHU3Q0FBOEM7Z0JBQzlDLFVBQVUsRUFBRSxDQUFDLE9BQU8sQ0FBQyxRQUFRLEVBQUUsQ0FBQyxVQUFVLENBQUMsUUFBUSxFQUFFLFlBQVksQ0FBQyxNQUFNLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQzthQUM5RTs7OztJQUVDLDZDQUFnQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IGZhZGVJbiB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcclxuaW1wb3J0IHsgdHJhbnNpdGlvbiwgdHJpZ2dlciwgdXNlQW5pbWF0aW9uIH0gZnJvbSAnQGFuZ3VsYXIvYW5pbWF0aW9ucyc7XHJcbmltcG9ydCB7IENvbXBvbmVudCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdhYnAtbWFuYWdlLXByb2ZpbGUnLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9tYW5hZ2UtcHJvZmlsZS5jb21wb25lbnQuaHRtbCcsXHJcbiAgYW5pbWF0aW9uczogW3RyaWdnZXIoJ2ZhZGVJbicsIFt0cmFuc2l0aW9uKCc6ZW50ZXInLCB1c2VBbmltYXRpb24oZmFkZUluKSldKV0sXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBNYW5hZ2VQcm9maWxlQ29tcG9uZW50IHtcclxuICBzZWxlY3RlZFRhYiA9IDA7XHJcbn1cclxuIl19