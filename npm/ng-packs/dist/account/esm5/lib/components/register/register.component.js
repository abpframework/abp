/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { validatePassword } from '@ngx-validate/core';
import { OAuthService } from 'angular-oauth2-oidc';
var maxLength = Validators.maxLength, minLength = Validators.minLength, required = Validators.required, email = Validators.email;
var RegisterComponent = /** @class */ (function () {
    function RegisterComponent(fb, oauthService, router) {
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
    RegisterComponent.prototype.onSubmit = /**
     * @return {?}
     */
    function () {
        if (this.form.invalid)
            return;
    };
    RegisterComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-register',
                    template: "<div class=\"row\">\n  <div class=\"col col-md-4 offset-md-4\">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class=\"abp-account-container\">\n      <h2>Register</h2>\n      <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n        <div class=\"form-group\">\n          <label for=\"input-user-name\">User name</label><span> * </span\n          ><input autofocus type=\"text\" id=\"input-user-name\" class=\"form-control\" formControlName=\"username\" />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"input-email-address\">Email address</label><span> * </span\n          ><input type=\"email\" id=\"input-email-address\" class=\"form-control\" formControlName=\"email\" />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"input-password\">Password</label><span> * </span\n          ><input type=\"password\" id=\"input-password\" class=\"form-control\" formControlName=\"password\" />\n        </div>\n        <button type=\"submit\" class=\"btn btn-primary\">Register</button>\n      </form>\n    </div>\n  </div>\n</div>\n"
                }] }
    ];
    /** @nocollapse */
    RegisterComponent.ctorParameters = function () { return [
        { type: FormBuilder },
        { type: OAuthService },
        { type: Router }
    ]; };
    return RegisterComponent;
}());
export { RegisterComponent };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmVnaXN0ZXIuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcmVnaXN0ZXIvcmVnaXN0ZXIuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzFDLE9BQU8sRUFBRSxXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDcEUsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQ3pDLE9BQU8sRUFBRSxnQkFBZ0IsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBQ3RELE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUUzQyxJQUFBLGdDQUFTLEVBQUUsZ0NBQVMsRUFBRSw4QkFBUSxFQUFFLHdCQUFLO0FBRTdDO0lBT0UsMkJBQW9CLEVBQWUsRUFBVSxZQUEwQixFQUFVLE1BQWM7UUFBM0UsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUFVLGlCQUFZLEdBQVosWUFBWSxDQUFjO1FBQVUsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUM3RixJQUFJLENBQUMsSUFBSSxHQUFHLElBQUksQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUFDO1lBQ3hCLFFBQVEsRUFBRSxDQUFDLEVBQUUsRUFBRSxDQUFDLFFBQVEsRUFBRSxTQUFTLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQztZQUMxQyxRQUFRLEVBQUU7Z0JBQ1IsRUFBRTtnQkFDRixDQUFDLFFBQVEsRUFBRSxTQUFTLENBQUMsRUFBRSxDQUFDLEVBQUUsU0FBUyxDQUFDLENBQUMsQ0FBQyxFQUFFLGdCQUFnQixDQUFDLENBQUMsT0FBTyxFQUFFLFNBQVMsRUFBRSxRQUFRLEVBQUUsU0FBUyxDQUFDLENBQUMsQ0FBQzthQUNyRztZQUNELEtBQUssRUFBRSxDQUFDLEVBQUUsRUFBRSxDQUFDLFFBQVEsRUFBRSxLQUFLLENBQUMsQ0FBQztTQUMvQixDQUFDLENBQUM7SUFDTCxDQUFDOzs7O0lBRUQsb0NBQVE7OztJQUFSO1FBQ0UsSUFBSSxJQUFJLENBQUMsSUFBSSxDQUFDLE9BQU87WUFBRSxPQUFPO0lBQ2hDLENBQUM7O2dCQXBCRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGNBQWM7b0JBQ3hCLDJsQ0FBd0M7aUJBQ3pDOzs7O2dCQVZRLFdBQVc7Z0JBR1gsWUFBWTtnQkFGWixNQUFNOztJQTJCZix3QkFBQztDQUFBLEFBckJELElBcUJDO1NBakJZLGlCQUFpQjs7O0lBQzVCLGlDQUFnQjs7Ozs7SUFFSiwrQkFBdUI7Ozs7O0lBQUUseUNBQWtDOzs7OztJQUFFLG1DQUFzQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgRm9ybUJ1aWxkZXIsIEZvcm1Hcm91cCwgVmFsaWRhdG9ycyB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyB2YWxpZGF0ZVBhc3N3b3JkIH0gZnJvbSAnQG5neC12YWxpZGF0ZS9jb3JlJztcbmltcG9ydCB7IE9BdXRoU2VydmljZSB9IGZyb20gJ2FuZ3VsYXItb2F1dGgyLW9pZGMnO1xuXG5jb25zdCB7IG1heExlbmd0aCwgbWluTGVuZ3RoLCByZXF1aXJlZCwgZW1haWwgfSA9IFZhbGlkYXRvcnM7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1yZWdpc3RlcicsXG4gIHRlbXBsYXRlVXJsOiAnLi9yZWdpc3Rlci5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFJlZ2lzdGVyQ29tcG9uZW50IHtcbiAgZm9ybTogRm9ybUdyb3VwO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgZmI6IEZvcm1CdWlsZGVyLCBwcml2YXRlIG9hdXRoU2VydmljZTogT0F1dGhTZXJ2aWNlLCBwcml2YXRlIHJvdXRlcjogUm91dGVyKSB7XG4gICAgdGhpcy5mb3JtID0gdGhpcy5mYi5ncm91cCh7XG4gICAgICB1c2VybmFtZTogWycnLCBbcmVxdWlyZWQsIG1heExlbmd0aCgyNTUpXV0sXG4gICAgICBwYXNzd29yZDogW1xuICAgICAgICAnJyxcbiAgICAgICAgW3JlcXVpcmVkLCBtYXhMZW5ndGgoMzIpLCBtaW5MZW5ndGgoNiksIHZhbGlkYXRlUGFzc3dvcmQoWydzbWFsbCcsICdjYXBpdGFsJywgJ251bWJlcicsICdzcGVjaWFsJ10pXSxcbiAgICAgIF0sXG4gICAgICBlbWFpbDogWycnLCBbcmVxdWlyZWQsIGVtYWlsXV0sXG4gICAgfSk7XG4gIH1cblxuICBvblN1Ym1pdCgpIHtcbiAgICBpZiAodGhpcy5mb3JtLmludmFsaWQpIHJldHVybjtcbiAgfVxufVxuIl19