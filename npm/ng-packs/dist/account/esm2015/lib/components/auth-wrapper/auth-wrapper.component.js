/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/auth-wrapper/auth-wrapper.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, Input, TemplateRef } from '@angular/core';
export class AuthWrapperComponent {
}
AuthWrapperComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-auth-wrapper',
                template: "<div class=\"row\">\r\n  <div class=\"mx-auto col col-md-5\">\r\n    <abp-tenant-box></abp-tenant-box>\r\n\r\n    <div class=\"abp-account-container\">\r\n      <div class=\"card mt-3 shadow-sm rounded\">\r\n        <div class=\"card-body p-5\">\r\n          <ng-content *ngTemplateOutlet=\"mainContentRef\"></ng-content>\r\n        </div>\r\n        <ng-content *ngTemplateOutlet=\"cancelContentRef\"></ng-content>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n"
            }] }
];
AuthWrapperComponent.propDecorators = {
    mainContentRef: [{ type: Input }],
    cancelContentRef: [{ type: Input }]
};
if (false) {
    /** @type {?} */
    AuthWrapperComponent.prototype.mainContentRef;
    /** @type {?} */
    AuthWrapperComponent.prototype.cancelContentRef;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXV0aC13cmFwcGVyLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuYWNjb3VudC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2F1dGgtd3JhcHBlci9hdXRoLXdyYXBwZXIuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxLQUFLLEVBQUUsV0FBVyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBTTlELE1BQU0sT0FBTyxvQkFBb0I7OztZQUpoQyxTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGtCQUFrQjtnQkFDNUIsbWVBQTRDO2FBQzdDOzs7NkJBRUUsS0FBSzsrQkFHTCxLQUFLOzs7O0lBSE4sOENBQ2lDOztJQUVqQyxnREFDbUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIElucHV0LCBUZW1wbGF0ZVJlZiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdhYnAtYXV0aC13cmFwcGVyJyxcclxuICB0ZW1wbGF0ZVVybDogJy4vYXV0aC13cmFwcGVyLmNvbXBvbmVudC5odG1sJyxcclxufSlcclxuZXhwb3J0IGNsYXNzIEF1dGhXcmFwcGVyQ29tcG9uZW50IHtcclxuICBASW5wdXQoKVxyXG4gIG1haW5Db250ZW50UmVmOiBUZW1wbGF0ZVJlZjxhbnk+O1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGNhbmNlbENvbnRlbnRSZWY6IFRlbXBsYXRlUmVmPGFueT47XHJcbn1cclxuIl19