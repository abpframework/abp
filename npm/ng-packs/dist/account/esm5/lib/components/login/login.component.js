/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
                    template: "<div class=\"row\">\n  <div class=\"col col-md-4 offset-md-4\">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class=\"abp-account-container\">\n      <h2>{{ 'AbpAccount::Login' | abpLocalization }}</h2>\n      <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n        <div class=\"form-group\">\n          <label for=\"login-input-user-name-or-email-address\">{{\n            'AbpAccount::UserNameOrEmailAddress' | abpLocalization\n          }}</label>\n          <input\n            class=\"form-control\"\n            type=\"text\"\n            id=\"login-input-user-name-or-email-address\"\n            formControlName=\"username\"\n            autofocus\n          />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"login-input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label>\n          <input class=\"form-control\" type=\"password\" id=\"login-input-password\" formControlName=\"password\" />\n        </div>\n        <div class=\"form-check\" validationTarget validationStyle>\n          <label class=\"form-check-label\" for=\"login-input-remember-me\">\n            <input class=\"form-check-input\" type=\"checkbox\" id=\"login-input-remember-me\" formControlName=\"remember\" />\n            {{ 'AbpAccount::RememberMe' | abpLocalization }}\n          </label>\n        </div>\n        <div class=\"mt-2\">\n          <abp-button [loading]=\"inProgress\" type=\"submit\">\n            {{ 'AbpAccount::Login' | abpLocalization }}\n          </abp-button>\n        </div>\n      </form>\n      <div style=\"padding-top: 20px\">\n        <a routerLink=\"/account/register\">{{ 'AbpAccount::Register' | abpLocalization }}</a>\n      </div>\n    </div>\n  </div>\n</div>\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9naW4uY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvbG9naW4vbG9naW4uY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ2hFLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUM1RCxPQUFPLEVBQUUsV0FBVyxFQUFhLFVBQVUsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUMvQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUNuRCxPQUFPLEVBQUUsSUFBSSxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUV4QyxPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdEQsT0FBTyxFQUFFLFVBQVUsRUFBRSxRQUFRLEVBQUUsU0FBUyxFQUFFLEdBQUcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3RFLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUVkLElBQUEsZ0NBQVMsRUFBRSxnQ0FBUyxFQUFFLDhCQUFRO0FBRXRDO0lBU0Usd0JBQ1UsRUFBZSxFQUNmLFlBQTBCLEVBQzFCLEtBQVksRUFDWixjQUE4QixFQUNTLE9BQWdCO1FBSnZELE9BQUUsR0FBRixFQUFFLENBQWE7UUFDZixpQkFBWSxHQUFaLFlBQVksQ0FBYztRQUMxQixVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQ1osbUJBQWMsR0FBZCxjQUFjLENBQWdCO1FBQ1MsWUFBTyxHQUFQLE9BQU8sQ0FBUztRQUUvRCxJQUFJLENBQUMs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Ozs7OztJQUN0QyxpQ0FBK0QiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBHZXRBcHBDb25maWd1cmF0aW9uLCBDb25maWdTdGF0ZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBDb21wb25lbnQsIEluamVjdCwgT3B0aW9uYWwgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEZvcm1CdWlsZGVyLCBGb3JtR3JvdXAsIFZhbGlkYXRvcnMgfSBmcm9tICdAYW5ndWxhci9mb3Jtcyc7XG5pbXBvcnQgeyBOYXZpZ2F0ZSB9IGZyb20gJ0BuZ3hzL3JvdXRlci1wbHVnaW4nO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBPQXV0aFNlcnZpY2UgfSBmcm9tICdhbmd1bGFyLW9hdXRoMi1vaWRjJztcbmltcG9ydCB7IGZyb20sIHRocm93RXJyb3IgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IE9wdGlvbnMgfSBmcm9tICcuLi8uLi9tb2RlbHMvb3B0aW9ucyc7XG5pbXBvcnQgeyBUb2FzdGVyU2VydmljZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IGNhdGNoRXJyb3IsIGZpbmFsaXplLCBzd2l0Y2hNYXAsIHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcblxuY29uc3QgeyBtYXhMZW5ndGgsIG1pbkxlbmd0aCwgcmVxdWlyZWQgfSA9IFZhbGlkYXRvcnM7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1sb2dpbicsXG4gIHRlbXBsYXRlVXJsOiAnLi9sb2dpbi5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIExvZ2luQ29tcG9uZW50IHtcbiAgZm9ybTogRm9ybUdyb3VwO1xuXG4gIGluUHJvZ3Jlc3M6IGJvb2xlYW47XG5cbiAgY29uc3RydWN0b3IoXG4gICAgcHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIsXG4gICAgcHJpdmF0ZSBvYXV0aFNlcnZpY2U6IE9BdXRoU2VydmljZSxcbiAgICBwcml2YXRlIHN0b3JlOiBTdG9yZSxcbiAgICBwcml2YXRlIHRvYXN0ZXJTZXJ2aWNlOiBUb2FzdGVyU2VydmljZSxcbiAgICBAT3B0aW9uYWwoKSBASW5qZWN0KCdBQ0NPVU5UX09QVElPTlMnKSBwcml2YXRlIG9wdGlvbnM6IE9wdGlvbnMsXG4gICkge1xuICAgIHRoaXMub2F1dGhTZXJ2aWNlLmNvbmZpZ3VyZSh0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldE9uZSgnZW52aXJvbm1lbnQnKSkub0F1dGhDb25maWcpO1xuICAgIHRoaXMub2F1dGhTZXJ2aWNlLmxvYWREaXNjb3ZlcnlEb2N1bWVudCgpO1xuXG4gICAgdGhpcy5mb3JtID0gdGhpcy5mYi5ncm91cCh7XG4gICAgICB1c2VybmFtZTogWycnLCBbcmVxdWlyZWQsIG1heExlbmd0aCgyNTUpXV0sXG4gICAgICBwYXNzd29yZDogWycnLCBbcmVxdWlyZWQsIG1heExlbmd0aCgzMildXSxcbiAgICAgIHJlbWVtYmVyOiBbZmFsc2VdLFxuICAgIH0pO1xuICB9XG5cbiAgb25TdWJtaXQoKSB7XG4gICAgaWYgKHRoaXMuZm9ybS5pbnZhbGlkKSByZXR1cm47XG4gICAgLy8gdGhpcy5vYXV0aFNlcnZpY2Uuc2V0U3RvcmFnZSh0aGlzLmZvcm0udmFsdWUucmVtZW1iZXIgPyBsb2NhbFN0b3JhZ2UgOiBzZXNzaW9uU3RvcmFnZSk7XG5cbiAgICB0aGlzLmluUHJvZ3Jlc3MgPSB0cnVlO1xuICAgIGZyb20oXG4gICAgICB0aGlzLm9hdXRoU2VydmljZS5mZXRjaFRva2VuVXNpbmdQYXNzd29yZEZsb3codGhpcy5mb3JtLmdldCgndXNlcm5hbWUnKS52YWx1ZSwgdGhpcy5mb3JtLmdldCgncGFzc3dvcmQnKS52YWx1ZSksXG4gICAgKVxuICAgICAgLnBpcGUoXG4gICAgICAgIHN3aXRjaE1hcCgoKSA9PiB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBHZXRBcHBDb25maWd1cmF0aW9uKCkpKSxcbiAgICAgICAgdGFwKCgpID0+IHtcbiAgICAgICAgICBjb25zdCByZWRpcmVjdFVybCA9IHNucSgoKSA9PiB3aW5kb3cuaGlzdG9yeS5zdGF0ZSkucmVkaXJlY3RVcmwgfHwgKHRoaXMub3B0aW9ucyB8fCB7fSkucmVkaXJlY3RVcmwgfHwgJy8nO1xuICAgICAgICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IE5hdmlnYXRlKFtyZWRpcmVjdFVybF0pKTtcbiAgICAgICAgfSksXG4gICAgICAgIGNhdGNoRXJyb3IoZXJyID0+IHtcbiAgICAgICAgICB0aGlzLnRvYXN0ZXJTZXJ2aWNlLmVycm9yKFxuICAgICAgICAgICAgc25xKCgpID0+IGVyci5lcnJvci5lcnJvcl9kZXNjcmlwdGlvbikgfHxcbiAgICAgICAgICAgICAgc25xKCgpID0+IGVyci5lcnJvci5lcnJvci5tZXNzYWdlLCAnQWJwQWNjb3VudDo6RGVmYXVsdEVycm9yTWVzc2FnZScpLFxuICAgICAgICAgICAgJ0Vycm9yJyxcbiAgICAgICAgICAgIHsgbGlmZTogNzAwMCB9LFxuICAgICAgICAgICk7XG4gICAgICAgICAgcmV0dXJuIHRocm93RXJyb3IoZXJyKTtcbiAgICAgICAgfSksXG4gICAgICAgIGZpbmFsaXplKCgpID0+ICh0aGlzLmluUHJvZ3Jlc3MgPSBmYWxzZSkpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgpO1xuICB9XG59XG4iXX0=