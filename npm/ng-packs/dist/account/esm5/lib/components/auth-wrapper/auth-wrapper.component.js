/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/auth-wrapper/auth-wrapper.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, Input, TemplateRef } from '@angular/core';
var AuthWrapperComponent = /** @class */ (function () {
    function AuthWrapperComponent() {
    }
    AuthWrapperComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-auth-wrapper',
                    template: "<div class=\"row\">\n  <div class=\"mx-auto col col-md-5\">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class=\"abp-account-container\">\n      <div class=\"card mt-3 shadow-sm rounded\">\n        <div class=\"card-body p-5\">\n          <ng-content *ngTemplateOutlet=\"mainContentRef\"></ng-content>\n        </div>\n        <ng-content *ngTemplateOutlet=\"cancelContentRef\"></ng-content>\n      </div>\n    </div>\n  </div>\n</div>\n"
                }] }
    ];
    AuthWrapperComponent.propDecorators = {
        mainContentRef: [{ type: Input }],
        cancelContentRef: [{ type: Input }]
    };
    return AuthWrapperComponent;
}());
export { AuthWrapperComponent };
if (false) {
    /** @type {?} */
    AuthWrapperComponent.prototype.mainContentRef;
    /** @type {?} */
    AuthWrapperComponent.prototype.cancelContentRef;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXV0aC13cmFwcGVyLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuYWNjb3VudC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL2F1dGgtd3JhcHBlci9hdXRoLXdyYXBwZXIuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBRSxLQUFLLEVBQUUsV0FBVyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRTlEO0lBQUE7SUFVQSxDQUFDOztnQkFWQSxTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGtCQUFrQjtvQkFDNUIsdWNBQTRDO2lCQUM3Qzs7O2lDQUVFLEtBQUs7bUNBR0wsS0FBSzs7SUFFUiwyQkFBQztDQUFBLEFBVkQsSUFVQztTQU5ZLG9CQUFvQjs7O0lBQy9CLDhDQUNpQzs7SUFFakMsZ0RBQ21DIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBJbnB1dCwgVGVtcGxhdGVSZWYgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWF1dGgtd3JhcHBlcicsXG4gIHRlbXBsYXRlVXJsOiAnLi9hdXRoLXdyYXBwZXIuY29tcG9uZW50Lmh0bWwnLFxufSlcbmV4cG9ydCBjbGFzcyBBdXRoV3JhcHBlckNvbXBvbmVudCB7XG4gIEBJbnB1dCgpXG4gIG1haW5Db250ZW50UmVmOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBJbnB1dCgpXG4gIGNhbmNlbENvbnRlbnRSZWY6IFRlbXBsYXRlUmVmPGFueT47XG59XG4iXX0=