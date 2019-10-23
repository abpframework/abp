/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { GetAppConfiguration, ConfigState } from '@abp/ng.core';
import { Component, Inject, Optional } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Navigate } from '@ngxs/router-plugin';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { from, throwError } from 'rxjs';
import { ToasterService } from '@abp/ng.theme.shared';
import { catchError, finalize, switchMap, tap } from 'rxjs/operators';
import snq from 'snq';
var maxLength = Validators.maxLength, minLength = Validators.minLength, required = Validators.required;
var LoginComponent = /** @class */ (function () {
    function LoginComponent(fb, oauthService, store, toasterService, options) {
        this.fb = fb;
        this.oauthService = oauthService;
        this.store = store;
        this.toasterService = toasterService;
        this.options = options;
        this.oauthService.configure(this.store.selectSnapshot(ConfigState.getOne('environment')).oAuthConfig);
        this.oauthService.loadDiscoveryDocument();
        this.form = this.fb.group({
            username: ['', [required, maxLength(255)]],
            password: ['', [required, maxLength(32)]],
            remember: [false],
        });
    }
    /**
     * @return {?}
     */
    LoginComponent.prototype.onSubmit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        if (this.form.invalid)
            return;
        // this.oauthService.setStorage(this.form.value.remember ? localStorage : sessionStorage);
        this.inProgress = true;
        from(this.oauthService.fetchTokenUsingPasswordFlow(this.form.get('username').value, this.form.get('password').value))
            .pipe(switchMap((/**
         * @return {?}
         */
        function () { return _this.store.dispatch(new GetAppConfiguration()); })), tap((/**
         * @return {?}
         */
        function () {
            /** @type {?} */
            var redirectUrl = snq((/**
             * @return {?}
             */
            function () { return window.history.state; })).redirectUrl || (_this.options || {}).redirectUrl || '/';
            _this.store.dispatch(new Navigate([redirectUrl]));
        })), catchError((/**
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
    LoginComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-login',
                    template: "<div class=\"row\">\r\n  <div class=\"col col-md-4 offset-md-4\">\r\n    <abp-tenant-box></abp-tenant-box>\r\n\r\n    <div class=\"abp-account-container\">\r\n      <h2>{{ 'AbpAccount::Login' | abpLocalization }}</h2>\r\n      <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\r\n        <div class=\"form-group\">\r\n          <label for=\"login-input-user-name-or-email-address\">{{\r\n            'AbpAccount::UserNameOrEmailAddress' | abpLocalization\r\n          }}</label>\r\n          <input\r\n            class=\"form-control\"\r\n            type=\"text\"\r\n            id=\"login-input-user-name-or-email-address\"\r\n            formControlName=\"username\"\r\n            autofocus\r\n          />\r\n        </div>\r\n        <div class=\"form-group\">\r\n          <label for=\"login-input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label>\r\n          <input class=\"form-control\" type=\"password\" id=\"login-input-password\" formControlName=\"password\" />\r\n        </div>\r\n        <div class=\"form-check\" validationTarget validationStyle>\r\n          <label class=\"form-check-label\" for=\"login-input-remember-me\">\r\n            <input class=\"form-check-input\" type=\"checkbox\" id=\"login-input-remember-me\" formControlName=\"remember\" />\r\n            {{ 'AbpAccount::RememberMe' | abpLocalization }}\r\n          </label>\r\n        </div>\r\n        <div class=\"mt-2\">\r\n          <abp-button [loading]=\"inProgress\" type=\"submit\">\r\n            {{ 'AbpAccount::Login' | abpLocalization }}\r\n          </abp-button>\r\n        </div>\r\n      </form>\r\n      <div style=\"padding-top: 20px\">\r\n        <a routerLink=\"/account/register\">{{ 'AbpAccount::Register' | abpLocalization }}</a>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n"
                }] }
    ];
    /** @nocollapse */
    LoginComponent.ctorParameters = function () { return [
        { type: FormBuilder },
        { type: OAuthService },
        { type: Store },
        { type: ToasterService },
        { type: undefined, decorators: [{ type: Optional }, { type: Inject, args: ['ACCOUNT_OPTIONS',] }] }
    ]; };
    return LoginComponent;
}());
export { LoginComponent };
if (false) {
    /** @type {?} */
    LoginComponent.prototype.form;
    /** @type {?} */
    LoginComponent.prototype.inProgress;
    /**
     * @type {?}
     * @private
     */
    LoginComponent.prototype.fb;
    /**
     * @type {?}
     * @private
     */
    LoginComponent.prototype.oauthService;
    /**
     * @type {?}
     * @private
     */
    LoginComponent.prototype.store;
    /**
     * @type {?}
     * @private
     */
    LoginComponent.prototype.toasterService;
    /**
     * @type {?}
     * @private
     */
    LoginComponent.prototype.options;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9naW4uY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvbG9naW4vbG9naW4uY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ2hFLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUM1RCxPQUFPLEVBQUUsV0FBVyxFQUFhLFVBQVUsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUMvQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUNuRCxPQUFPLEVBQUUsSUFBSSxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUV4QyxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdEQsT0FBTyxFQUFFLFVBQVUsRUFBRSxRQUFRLEVBQUUsU0FBUyxFQUFFLEdBQUcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3RFLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUVkLElBQUEsZ0NBQVMsRUFBRSxnQ0FBUyxFQUFFLDhCQUFRO0FBRXRDO0lBU0Usd0JBQ1UsRUFBZSxFQUNmLFlBQTBCLEVBQzFCLEtBQVksRUFDWixjQUE4QixFQUNTLE9BQWdCO1FBSnZELE9BQUUsR0FBRixFQUFFLENBQWE7UUFDZixpQkFBWSxHQUFaLFlBQVksQ0FBYztRQUMxQixVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQ1osbUJBQWMsR0FBZCxjQUFjLENBQWdCO1FBQ1MsWUFBTyxHQUFQLE9BQU8sQ0FBUztRQUUvRCxJQUFJLENBQUMs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Ozs7OztJQUN0QyxpQ0FBK0QiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBHZXRBcHBDb25maWd1cmF0aW9uLCBDb25maWdTdGF0ZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcbmltcG9ydCB7IENvbXBvbmVudCwgSW5qZWN0LCBPcHRpb25hbCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBGb3JtQnVpbGRlciwgRm9ybUdyb3VwLCBWYWxpZGF0b3JzIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xyXG5pbXBvcnQgeyBOYXZpZ2F0ZSB9IGZyb20gJ0BuZ3hzL3JvdXRlci1wbHVnaW4nO1xyXG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgT0F1dGhTZXJ2aWNlIH0gZnJvbSAnYW5ndWxhci1vYXV0aDItb2lkYyc7XHJcbmltcG9ydCB7IGZyb20sIHRocm93RXJyb3IgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgT3B0aW9ucyB9IGZyb20gJy4uLy4uL21vZGVscy9vcHRpb25zJztcclxuaW1wb3J0IHsgVG9hc3RlclNlcnZpY2UgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XHJcbmltcG9ydCB7IGNhdGNoRXJyb3IsIGZpbmFsaXplLCBzd2l0Y2hNYXAsIHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcclxuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xyXG5cclxuY29uc3QgeyBtYXhMZW5ndGgsIG1pbkxlbmd0aCwgcmVxdWlyZWQgfSA9IFZhbGlkYXRvcnM7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1sb2dpbicsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL2xvZ2luLmNvbXBvbmVudC5odG1sJyxcclxufSlcclxuZXhwb3J0IGNsYXNzIExvZ2luQ29tcG9uZW50IHtcclxuICBmb3JtOiBGb3JtR3JvdXA7XHJcblxyXG4gIGluUHJvZ3Jlc3M6IGJvb2xlYW47XHJcblxyXG4gIGNvbnN0cnVjdG9yKFxyXG4gICAgcHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIsXHJcbiAgICBwcml2YXRlIG9hdXRoU2VydmljZTogT0F1dGhTZXJ2aWNlLFxyXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXHJcbiAgICBwcml2YXRlIHRvYXN0ZXJTZXJ2aWNlOiBUb2FzdGVyU2VydmljZSxcclxuICAgIEBPcHRpb25hbCgpIEBJbmplY3QoJ0FDQ09VTlRfT1BUSU9OUycpIHByaXZhdGUgb3B0aW9uczogT3B0aW9ucyxcclxuICApIHtcclxuICAgIHRoaXMub2F1dGhTZXJ2aWNlLmNvbmZpZ3VyZSh0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldE9uZSgnZW52aXJvbm1lbnQnKSkub0F1dGhDb25maWcpO1xyXG4gICAgdGhpcy5vYXV0aFNlcnZpY2UubG9hZERpc2NvdmVyeURvY3VtZW50KCk7XHJcblxyXG4gICAgdGhpcy5mb3JtID0gdGhpcy5mYi5ncm91cCh7XHJcbiAgICAgIHVzZXJuYW1lOiBbJycsIFtyZXF1aXJlZCwgbWF4TGVuZ3RoKDI1NSldXSxcclxuICAgICAgcGFzc3dvcmQ6IFsnJywgW3JlcXVpcmVkLCBtYXhMZW5ndGgoMzIpXV0sXHJcbiAgICAgIHJlbWVtYmVyOiBbZmFsc2VdLFxyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBvblN1Ym1pdCgpIHtcclxuICAgIGlmICh0aGlzLmZvcm0uaW52YWxpZCkgcmV0dXJuO1xyXG4gICAgLy8gdGhpcy5vYXV0aFNlcnZpY2Uuc2V0U3RvcmFnZSh0aGlzLmZvcm0udmFsdWUucmVtZW1iZXIgPyBsb2NhbFN0b3JhZ2UgOiBzZXNzaW9uU3RvcmFnZSk7XHJcblxyXG4gICAgdGhpcy5pblByb2dyZXNzID0gdHJ1ZTtcclxuICAgIGZyb20oXHJcbiAgICAgIHRoaXMub2F1dGhTZXJ2aWNlLmZldGNoVG9rZW5Vc2luZ1Bhc3N3b3JkRmxvdyh0aGlzLmZvcm0uZ2V0KCd1c2VybmFtZScpLnZhbHVlLCB0aGlzLmZvcm0uZ2V0KCdwYXNzd29yZCcpLnZhbHVlKSxcclxuICAgIClcclxuICAgICAgLnBpcGUoXHJcbiAgICAgICAgc3dpdGNoTWFwKCgpID0+IHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IEdldEFwcENvbmZpZ3VyYXRpb24oKSkpLFxyXG4gICAgICAgIHRhcCgoKSA9PiB7XHJcbiAgICAgICAgICBjb25zdCByZWRpcmVjdFVybCA9IHNucSgoKSA9PiB3aW5kb3cuaGlzdG9yeS5zdGF0ZSkucmVkaXJlY3RVcmwgfHwgKHRoaXMub3B0aW9ucyB8fCB7fSkucmVkaXJlY3RVcmwgfHwgJy8nO1xyXG4gICAgICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgTmF2aWdhdGUoW3JlZGlyZWN0VXJsXSkpO1xyXG4gICAgICAgIH0pLFxyXG4gICAgICAgIGNhdGNoRXJyb3IoZXJyID0+IHtcclxuICAgICAgICAgIHRoaXMudG9hc3RlclNlcnZpY2UuZXJyb3IoXHJcbiAgICAgICAgICAgIHNucSgoKSA9PiBlcnIuZXJyb3IuZXJyb3JfZGVzY3JpcHRpb24pIHx8XHJcbiAgICAgICAgICAgICAgc25xKCgpID0+IGVyci5lcnJvci5lcnJvci5tZXNzYWdlLCAnQWJwQWNjb3VudDo6RGVmYXVsdEVycm9yTWVzc2FnZScpLFxyXG4gICAgICAgICAgICAnRXJyb3InLFxyXG4gICAgICAgICAgICB7IGxpZmU6IDcwMDAgfSxcclxuICAgICAgICAgICk7XHJcbiAgICAgICAgICByZXR1cm4gdGhyb3dFcnJvcihlcnIpO1xyXG4gICAgICAgIH0pLFxyXG4gICAgICAgIGZpbmFsaXplKCgpID0+ICh0aGlzLmluUHJvZ3Jlc3MgPSBmYWxzZSkpLFxyXG4gICAgICApXHJcbiAgICAgIC5zdWJzY3JpYmUoKTtcclxuICB9XHJcbn1cclxuIl19