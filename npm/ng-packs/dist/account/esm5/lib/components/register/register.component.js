/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/register/register.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { ConfigState, GetAppConfiguration, SessionState } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Navigate } from '@ngxs/router-plugin';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { from, throwError } from 'rxjs';
import { catchError, finalize, switchMap, take, tap } from 'rxjs/operators';
import snq from 'snq';
import { AccountService } from '../../services/account.service';
import { validatePassword } from '@ngx-validate/core';
import { HttpHeaders } from '@angular/common/http';
var maxLength = Validators.maxLength, minLength = Validators.minLength, required = Validators.required, email = Validators.email;
var RegisterComponent = /** @class */ (function () {
    function RegisterComponent(fb, accountService, oauthService, store, toasterService) {
        this.fb = fb;
        this.accountService = accountService;
        this.oauthService = oauthService;
        this.store = store;
        this.toasterService = toasterService;
        this.oauthService.configure(this.store.selectSnapshot(ConfigState.getOne('environment')).oAuthConfig);
        this.oauthService.loadDiscoveryDocument();
    }
    /**
     * @return {?}
     */
    RegisterComponent.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        /** @type {?} */
        var passwordRules = this.store.selectSnapshot(ConfigState.getSettings('Identity.Password'));
        /** @type {?} */
        var passwordRulesArr = (/** @type {?} */ ([]));
        /** @type {?} */
        var requiredLength = 1;
        if ((passwordRules['Abp.Identity.Password.RequireDigit'] || '').toLowerCase() === 'true') {
            passwordRulesArr.push('number');
        }
        if ((passwordRules['Abp.Identity.Password.RequireLowercase'] || '').toLowerCase() === 'true') {
            passwordRulesArr.push('small');
        }
        if ((passwordRules['Abp.Identity.Password.RequireUppercase'] || '').toLowerCase() === 'true') {
            passwordRulesArr.push('capital');
        }
        if (+(passwordRules['Abp.Identity.Password.RequiredUniqueChars'] || 0) > 0) {
            passwordRulesArr.push('special');
        }
        if (Number.isInteger(+passwordRules['Abp.Identity.Password.RequiredLength'])) {
            requiredLength = +passwordRules['Abp.Identity.Password.RequiredLength'];
        }
        this.form = this.fb.group({
            username: ['', [required, maxLength(255)]],
            password: [
                '',
                [required, validatePassword(passwordRulesArr), minLength(requiredLength), maxLength(32)],
            ],
            email: ['', [required, email]],
        });
    };
    /**
     * @return {?}
     */
    RegisterComponent.prototype.onSubmit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        if (this.form.invalid)
            return;
        this.inProgress = true;
        /** @type {?} */
        var newUser = (/** @type {?} */ ({
            userName: this.form.get('username').value,
            password: this.form.get('password').value,
            emailAddress: this.form.get('email').value,
            appName: 'Angular',
        }));
        /** @type {?} */
        var tenant = this.store.selectSnapshot(SessionState.getTenant);
        this.accountService
            .register(newUser)
            .pipe(switchMap((/**
         * @return {?}
         */
        function () {
            return from(_this.oauthService.fetchTokenUsingPasswordFlow(newUser.userName, newUser.password, new HttpHeaders(tslib_1.__assign({}, (tenant && tenant.id && { __tenant: tenant.id })))));
        })), switchMap((/**
         * @return {?}
         */
        function () { return _this.store.dispatch(new GetAppConfiguration()); })), tap((/**
         * @return {?}
         */
        function () { return _this.store.dispatch(new Navigate(['/'])); })), take(1), catchError((/**
         * @param {?} err
         * @return {?}
         */
        function (err) {
            _this.toasterService.error(snq((/**
             * @return {?}
             */
            function () { return err.error.error_description; })) ||
                snq((/**
                 * @return {?}
                 */
                function () { return err.error.error.message; }), 'AbpAccount::DefaultErrorMessage'), 'Error', { life: 7000 });
            return throwError(err);
        })), finalize((/**
         * @return {?}
         */
        function () { return (_this.inProgress = false); })))
            .subscribe();
    };
    RegisterComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-register',
                    template: "<abp-auth-wrapper [mainContentRef]=\"mainContentRef\">\n  <ng-template #mainContentRef>\n    <h4>{{ 'AbpAccount::Register' | abpLocalization }}</h4>\n    <strong>\n      {{ 'AbpAccount::AlreadyRegistered' | abpLocalization }}\n      <a class=\"text-decoration-none\" routerLink=\"/account/login\">{{ 'AbpAccount::Login' | abpLocalization }}</a>\n    </strong>\n    <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" validateOnSubmit class=\"mt-4\">\n      <div class=\"form-group\">\n        <label for=\"input-user-name\">{{ 'AbpAccount::UserName' | abpLocalization }}</label\n        ><span> * </span\n        ><input autofocus type=\"text\" id=\"input-user-name\" class=\"form-control\" formControlName=\"username\" />\n      </div>\n      <div class=\"form-group\">\n        <label for=\"input-email-address\">{{ 'AbpAccount::EmailAddress' | abpLocalization }}</label\n        ><span> * </span><input type=\"email\" id=\"input-email-address\" class=\"form-control\" formControlName=\"email\" />\n      </div>\n      <div class=\"form-group\">\n        <label for=\"input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label\n        ><span> * </span><input type=\"password\" id=\"input-password\" class=\"form-control\" formControlName=\"password\" />\n      </div>\n      <abp-button\n        [loading]=\"inProgress\"\n        buttonType=\"submit\"\n        name=\"Action\"\n        buttonClass=\"btn-block btn-lg mt-3 btn btn-primary\"\n      >\n        {{ 'AbpAccount::Register' | abpLocalization }}\n      </abp-button>\n    </form>\n  </ng-template>\n</abp-auth-wrapper>\n"
                }] }
    ];
    /** @nocollapse */
    RegisterComponent.ctorParameters = function () { return [
        { type: FormBuilder },
        { type: AccountService },
        { type: OAuthService },
        { type: Store },
        { type: ToasterService }
    ]; };
    return RegisterComponent;
}());
export { RegisterComponent };
if (false) {
    /** @type {?} */
    RegisterComponent.prototype.form;
    /** @type {?} */
    RegisterComponent.prototype.inProgress;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.fb;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.accountService;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.oauthService;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.store;
    /**
     * @type {?}
     * @private
     */
    RegisterComponent.prototype.toasterService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmVnaXN0ZXIuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcmVnaXN0ZXIvcmVnaXN0ZXIuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7OztBQUFBLE9BQU8sRUFBRSxXQUFXLEVBQUUsbUJBQW1CLEVBQU8sWUFBWSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ25GLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsU0FBUyxFQUFVLE1BQU0sZUFBZSxDQUFDO0FBQ2xELE9BQU8sRUFBRSxXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDcEUsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQy9DLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQ25ELE9BQU8sRUFBRSxJQUFJLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ3hDLE9BQU8sRUFBRSxVQUFVLEVBQUUsUUFBUSxFQUFFLFNBQVMsRUFBRSxJQUFJLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDNUUsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBRXRCLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxnQ0FBZ0MsQ0FBQztBQUNoRSxPQUFPLEVBQWlCLGdCQUFnQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFDckUsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQzNDLElBQUEsZ0NBQVMsRUFBRSxnQ0FBUyxFQUFFLDhCQUFRLEVBQUUsd0JBQUs7QUFFN0M7SUFTRSwyQkFDVSxFQUFlLEVBQ2YsY0FBOEIsRUFDOUIsWUFBMEIsRUFDMUIsS0FBWSxF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ZixDQUFDO1lBQ0YsT0FBTyxVQUFVLENBQUMsR0FBRyxDQUFDLENBQUM7UUFDekIsQ0FBQyxFQUFDLEVBQ0YsUUFBUTs7O1FBQUMsY0FBTSxPQUFBLENBQUMsS0FBSSxDQUFDLFVBQVUsR0FBRyxLQUFLLENBQUMsRUFBekIsQ0FBeUIsRUFBQyxDQUMxQzthQUNBLFNBQVMsRUFBRSxDQUFDO0lBQ2pCLENBQUM7O2dCQXRHRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGNBQWM7b0JBQ3hCLHVrREFBd0M7aUJBQ3pDOzs7O2dCQWhCUSxXQUFXO2dCQVFYLGNBQWM7Z0JBTGQsWUFBWTtnQkFEWixLQUFLO2dCQUpMLGNBQWM7O0lBc0h2Qix3QkFBQztDQUFBLEFBdkdELElBdUdDO1NBbkdZLGlCQUFpQjs7O0lBQzVCLGlDQUFnQjs7SUFFaEIsdUNBQW9COzs7OztJQUdsQiwrQkFBdUI7Ozs7O0lBQ3ZCLDJDQUFzQzs7Ozs7SUFDdEMseUNBQWtDOzs7OztJQUNsQyxrQ0FBb0I7Ozs7O0lBQ3BCLDJDQUFzQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbmZpZ1N0YXRlLCBHZXRBcHBDb25maWd1cmF0aW9uLCBBQlAsIFNlc3Npb25TdGF0ZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBUb2FzdGVyU2VydmljZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IENvbXBvbmVudCwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBGb3JtQnVpbGRlciwgRm9ybUdyb3VwLCBWYWxpZGF0b3JzIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgTmF2aWdhdGUgfSBmcm9tICdAbmd4cy9yb3V0ZXItcGx1Z2luJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgT0F1dGhTZXJ2aWNlIH0gZnJvbSAnYW5ndWxhci1vYXV0aDItb2lkYyc7XG5pbXBvcnQgeyBmcm9tLCB0aHJvd0Vycm9yIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBjYXRjaEVycm9yLCBmaW5hbGl6ZSwgc3dpdGNoTWFwLCB0YWtlLCB0YXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQgeyBSZWdpc3RlclJlcXVlc3QgfSBmcm9tICcuLi8uLi9tb2RlbHMnO1xuaW1wb3J0IHsgQWNjb3VudFNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcy9hY2NvdW50LnNlcnZpY2UnO1xuaW1wb3J0IHsgUGFzc3dvcmRSdWxlcywgdmFsaWRhdGVQYXNzd29yZCB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5pbXBvcnQgeyBIdHRwSGVhZGVycyB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbi9odHRwJztcbmNvbnN0IHsgbWF4TGVuZ3RoLCBtaW5MZW5ndGgsIHJlcXVpcmVkLCBlbWFpbCB9ID0gVmFsaWRhdG9ycztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXJlZ2lzdGVyJyxcbiAgdGVtcGxhdGVVcmw6ICcuL3JlZ2lzdGVyLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgUmVnaXN0ZXJDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBmb3JtOiBGb3JtR3JvdXA7XG5cbiAgaW5Qcm9ncmVzczogYm9vbGVhbjtcblxuICBjb25zdHJ1Y3RvcihcbiAgICBwcml2YXRlIGZiOiBGb3JtQnVpbGRlcixcbiAgICBwcml2YXRlIGFjY291bnRTZXJ2aWNlOiBBY2NvdW50U2VydmljZSxcbiAgICBwcml2YXRlIG9hdXRoU2VydmljZTogT0F1dGhTZXJ2aWNlLFxuICAgIHByaXZhdGUgc3RvcmU6IFN0b3JlLFxuICAgIHByaXZhdGUgdG9hc3RlclNlcnZpY2U6IFRvYXN0ZXJTZXJ2aWNlLFxuICApIHtcbiAgICB0aGlzLm9hdXRoU2VydmljZS5jb25maWd1cmUoXG4gICAgICB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldE9uZSgnZW52aXJvbm1lbnQnKSkub0F1dGhDb25maWcsXG4gICAgKTtcbiAgICB0aGlzLm9hdXRoU2VydmljZS5sb2FkRGlzY292ZXJ5RG9jdW1lbnQoKTtcbiAgfVxuXG4gIG5nT25Jbml0KCkge1xuICAgIGNvbnN0IHBhc3N3b3JkUnVsZXM6IEFCUC5EaWN0aW9uYXJ5PHN0cmluZz4gPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFxuICAgICAgQ29uZmlnU3RhdGUuZ2V0U2V0dGluZ3MoJ0lkZW50aXR5LlBhc3N3b3JkJyksXG4gICAgKTtcbiAgICBjb25zdCBwYXNzd29yZFJ1bGVzQXJyID0gW10gYXMgUGFzc3dvcmRSdWxlcztcbiAgICBsZXQgcmVxdWlyZWRMZW5ndGggPSAxO1xuXG4gICAgaWYgKChwYXNzd29yZFJ1bGVzWydBYnAuSWRlbnRpdHkuUGFzc3dvcmQuUmVxdWlyZURpZ2l0J10gfHwgJycpLnRvTG93ZXJDYXNlKCkgPT09ICd0cnVlJykge1xuICAgICAgcGFzc3dvcmRSdWxlc0Fyci5wdXNoKCdudW1iZXInKTtcbiAgICB9XG5cbiAgICBpZiAoKHBhc3N3b3JkUnVsZXNbJ0FicC5JZGVudGl0eS5QYXNzd29yZC5SZXF1aXJlTG93ZXJjYXNlJ10gfHwgJycpLnRvTG93ZXJDYXNlKCkgPT09ICd0cnVlJykge1xuICAgICAgcGFzc3dvcmRSdWxlc0Fyci5wdXNoKCdzbWFsbCcpO1xuICAgIH1cblxuICAgIGlmICgocGFzc3dvcmRSdWxlc1snQWJwLklkZW50aXR5LlBhc3N3b3JkLlJlcXVpcmVVcHBlcmNhc2UnXSB8fCAnJykudG9Mb3dlckNhc2UoKSA9PT0gJ3RydWUnKSB7XG4gICAgICBwYXNzd29yZFJ1bGVzQXJyLnB1c2goJ2NhcGl0YWwnKTtcbiAgICB9XG5cbiAgICBpZiAoKyhwYXNzd29yZFJ1bGVzWydBYnAuSWRlbnRpdHkuUGFzc3dvcmQuUmVxdWlyZWRVbmlxdWVDaGFycyddIHx8IDApID4gMCkge1xuICAgICAgcGFzc3dvcmRSdWxlc0Fyci5wdXNoKCdzcGVjaWFsJyk7XG4gICAgfVxuXG4gICAgaWYgKE51bWJlci5pc0ludGVnZXIoK3Bhc3N3b3JkUnVsZXNbJ0FicC5JZGVudGl0eS5QYXNzd29yZC5SZXF1aXJlZExlbmd0aCddKSkge1xuICAgICAgcmVxdWlyZWRMZW5ndGggPSArcGFzc3dvcmRSdWxlc1snQWJwLklkZW50aXR5LlBhc3N3b3JkLlJlcXVpcmVkTGVuZ3RoJ107XG4gICAgfVxuXG4gICAgdGhpcy5mb3JtID0gdGhpcy5mYi5ncm91cCh7XG4gICAgICB1c2VybmFtZTogWycnLCBbcmVxdWlyZWQsIG1heExlbmd0aCgyNTUpXV0sXG4gICAgICBwYXNzd29yZDogW1xuICAgICAgICAnJyxcbiAgICAgICAgW3JlcXVpcmVkLCB2YWxpZGF0ZVBhc3N3b3JkKHBhc3N3b3JkUnVsZXNBcnIpLCBtaW5MZW5ndGgocmVxdWlyZWRMZW5ndGgpLCBtYXhMZW5ndGgoMzIpXSxcbiAgICAgIF0sXG4gICAgICBlbWFpbDogWycnLCBbcmVxdWlyZWQsIGVtYWlsXV0sXG4gICAgfSk7XG4gIH1cblxuICBvblN1Ym1pdCgpIHtcbiAgICBpZiAodGhpcy5mb3JtLmludmFsaWQpIHJldHVybjtcblxuICAgIHRoaXMuaW5Qcm9ncmVzcyA9IHRydWU7XG5cbiAgICBjb25zdCBuZXdVc2VyID0ge1xuICAgICAgdXNlck5hbWU6IHRoaXMuZm9ybS5nZXQoJ3VzZXJuYW1lJykudmFsdWUsXG4gICAgICBwYXNzd29yZDogdGhpcy5mb3JtLmdldCgncGFzc3dvcmQnKS52YWx1ZSxcbiAgICAgIGVtYWlsQWRkcmVzczogdGhpcy5mb3JtLmdldCgnZW1haWwnKS52YWx1ZSxcbiAgICAgIGFwcE5hbWU6ICdBbmd1bGFyJyxcbiAgICB9IGFzIFJlZ2lzdGVyUmVxdWVzdDtcblxuICAgIGNvbnN0IHRlbmFudCA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoU2Vzc2lvblN0YXRlLmdldFRlbmFudCk7XG5cbiAgICB0aGlzLmFjY291bnRTZXJ2aWNlXG4gICAgICAucmVnaXN0ZXIobmV3VXNlcilcbiAgICAgIC5waXBlKFxuICAgICAgICBzd2l0Y2hNYXAoKCkgPT5cbiAgICAgICAgICBmcm9tKFxuICAgICAgICAgICAgdGhpcy5vYXV0aFNlcnZpY2UuZmV0Y2hUb2tlblVzaW5nUGFzc3dvcmRGbG93KFxuICAgICAgICAgICAgICBuZXdVc2VyLnVzZXJOYW1lLFxuICAgICAgICAgICAgICBuZXdVc2VyLnBhc3N3b3JkLFxuICAgICAgICAgICAgICBuZXcgSHR0cEhlYWRlcnMoe1xuICAgICAgICAgICAgICAgIC4uLih0ZW5hbnQgJiYgdGVuYW50LmlkICYmIHsgX190ZW5hbnQ6IHRlbmFudC5pZCB9KSxcbiAgICAgICAgICAgICAgfSksXG4gICAgICAgICAgICApLFxuICAgICAgICAgICksXG4gICAgICAgICksXG4gICAgICAgIHN3aXRjaE1hcCgoKSA9PiB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBHZXRBcHBDb25maWd1cmF0aW9uKCkpKSxcbiAgICAgICAgdGFwKCgpID0+IHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IE5hdmlnYXRlKFsnLyddKSkpLFxuICAgICAgICB0YWtlKDEpLFxuICAgICAgICBjYXRjaEVycm9yKGVyciA9PiB7XG4gICAgICAgICAgdGhpcy50b2FzdGVyU2VydmljZS5lcnJvcihcbiAgICAgICAgICAgIHNucSgoKSA9PiBlcnIuZXJyb3IuZXJyb3JfZGVzY3JpcHRpb24pIHx8XG4gICAgICAgICAgICAgIHNucSgoKSA9PiBlcnIuZXJyb3IuZXJyb3IubWVzc2FnZSwgJ0FicEFjY291bnQ6OkRlZmF1bHRFcnJvck1lc3NhZ2UnKSxcbiAgICAgICAgICAgICdFcnJvcicsXG4gICAgICAgICAgICB7IGxpZmU6IDcwMDAgfSxcbiAgICAgICAgICApO1xuICAgICAgICAgIHJldHVybiB0aHJvd0Vycm9yKGVycik7XG4gICAgICAgIH0pLFxuICAgICAgICBmaW5hbGl6ZSgoKSA9PiAodGhpcy5pblByb2dyZXNzID0gZmFsc2UpKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoKTtcbiAgfVxufVxuIl19