/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { validatePassword } from '@ngx-validate/core';
import { OAuthService } from 'angular-oauth2-oidc';
const { maxLength, minLength, required, email } = Validators;
export class RegisterComponent {
    /**
     * @param {?} fb
     * @param {?} oauthService
     * @param {?} router
     */
    constructor(fb, oauthService, router) {
        this.fb = fb;
        this.oauthService = oauthService;
        this.router = router;
        this.form = this.fb.group({
            username: ['', [required, maxLength(255)]],
            password: [
                '',
                [required, maxLength(32), minLength(6), validatePassword(['small', 'capital', 'number', 'special'])],
            ],
            email: ['', [required, email]],
        });
    }
    /**
     * @return {?}
     */
    onSubmit() {
        if (this.form.invalid)
            return;
    }
}
RegisterComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-register',
                template: "<div class=\"row\">\n  <div class=\"col col-md-4 offset-md-4\">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class=\"abp-account-container\">\n      <h2>Register</h2>\n      <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n        <div class=\"form-group\">\n          <label for=\"input-user-name\">User name</label><span> * </span\n          ><input autofocus type=\"text\" id=\"input-user-name\" class=\"form-control\" formControlName=\"username\" />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"input-email-address\">Email address</label><span> * </span\n          ><input type=\"email\" id=\"input-email-address\" class=\"form-control\" formControlName=\"email\" />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"input-password\">Password</label><span> * </span\n          ><input type=\"password\" id=\"input-password\" class=\"form-control\" formControlName=\"password\" />\n        </div>\n        <button type=\"submit\" class=\"btn btn-primary\">Register</button>\n      </form>\n    </div>\n  </div>\n</div>\n"
            }] }
];
/** @nocollapse */
RegisterComponent.ctorParameters = () => [
    { type: FormBuilder },
    { type: OAuthService },
    { type: Router }
];
if (false) {
    /** @type {?} */
    RegisterComponent.prototype.form;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.fb;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.oauthService;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.router;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmVnaXN0ZXIuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcmVnaXN0ZXIvcmVnaXN0ZXIuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzFDLE9BQU8sRUFBRSxXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDcEUsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQ3pDLE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBQ3RELE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztNQUU3QyxFQUFFLFNBQVMsRUFBRSxTQUFTLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBRSxHQUFHLFVBQVU7QUFNNUQsTUFBTSxPQUFPLGlCQUFpQjs7Ozs7O0lBRzVCLFlBQW9CLEVBQWUsRUFBVSxZQUEwQixFQUFVLE1BQWM7UUFBM0UsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUFVLGlCQUFZLEdBQVosWUFBWSxDQUFjO1FBQVUsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUM3RixJQUFJLENBQUMsSUFBSSxHQUFHLElBQUksQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUFDO1lBQ3hCLFFBQVEsRUFBRSxDQUFDLEVBQUUsRUFBRSxDQUFDLFFBQVEsRUFBRSxTQUFTLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQztZQUMxQyxRQUFRLEVBQUU7Z0JBQ1IsRUFBRTtnQkFDRixDQUFDLFFBQVEsRUFBRSxTQUFTLENBQUMsRUFBRSxDQUFDLEVBQUUsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFFLGdCQUFnQixDQUFDLENBQUMsT0FBTyxFQUFFLFNBQVMsRUFBRSxRQUFRLEVBQUUsU0FBUyxDQUFDLENBQUMsQ0FBQzthQUNyRztZQUNELEtBQUssRUFBRSxDQUFDLEVBQUUsRUFBRSxDQUFDLFFBQVEsRUFBRSxLQUFLLENBQUMsQ0FBQztTQUMvQixDQUFDLENBQUM7SUFDTCxDQUFDOzs7O0lBRUQsUUFBUTtRQUNOLElBQUksSUFBSSxDQUFDLElBQUksQ0FBQyxPQUFPO1lBQUUsT0FBTztJQUNoQyxDQUFDOzs7WUFwQkYsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxjQUFjO2dCQUN4QiwybENBQXdDO2FBQ3pDOzs7O1lBVlEsV0FBVztZQUdYLFlBQVk7WUFGWixNQUFNOzs7O0lBV2IsaUNBQWdCOzs7OztJQUVKLCtCQUF1Qjs7Ozs7SUFBRSx5Q0FBa0M7Ozs7O0lBQUUsbUNBQXNCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBGb3JtQnVpbGRlciwgRm9ybUdyb3VwLCBWYWxpZGF0b3JzIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgUm91dGVyIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IHZhbGlkYXRlUGFzc3dvcmQgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xuaW1wb3J0IHsgT0F1dGhTZXJ2aWNlIH0gZnJvbSAnYW5ndWxhci1vYXV0aDItb2lkYyc7XG5cbmNvbnN0IHsgbWF4TGVuZ3RoLCBtaW5MZW5ndGgsIHJlcXVpcmVkLCBlbWFpbCB9ID0gVmFsaWRhdG9ycztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXJlZ2lzdGVyJyxcbiAgdGVtcGxhdGVVcmw6ICcuL3JlZ2lzdGVyLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgUmVnaXN0ZXJDb21wb25lbnQge1xuICBmb3JtOiBGb3JtR3JvdXA7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIsIHByaXZhdGUgb2F1dGhTZXJ2aWNlOiBPQXV0aFNlcnZpY2UsIHByaXZhdGUgcm91dGVyOiBSb3V0ZXIpIHtcbiAgICB0aGlzLmZvcm0gPSB0aGlzLmZiLmdyb3VwKHtcbiAgICAgIHVzZXJuYW1lOiBbJycsIFtyZXF1aXJlZCwgbWF4TGVuZ3RoKDI1NSldXSxcbiAgICAgIHBhc3N3b3JkOiBbXG4gICAgICAgICcnLFxuICAgICAgICBbcmVxdWlyZWQsIG1heExlbmd0aCgzMiksIG1pbkxlbmd0aCg2KSwgdmFsaWRhdGVQYXNzd29yZChbJ3NtYWxsJywgJ2NhcGl0YWwnLCAnbnVtYmVyJywgJ3NwZWNpYWwnXSldLFxuICAgICAgXSxcbiAgICAgIGVtYWlsOiBbJycsIFtyZXF1aXJlZCwgZW1haWxdXSxcbiAgICB9KTtcbiAgfVxuXG4gIG9uU3VibWl0KCkge1xuICAgIGlmICh0aGlzLmZvcm0uaW52YWxpZCkgcmV0dXJuO1xuICB9XG59XG4iXX0=