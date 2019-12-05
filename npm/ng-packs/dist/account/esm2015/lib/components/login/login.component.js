/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/login/login.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { GetAppConfiguration, ConfigState, SessionState } from '@abp/ng.core';
import { Component, Inject, Optional } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Navigate } from '@ngxs/router-plugin';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { from, throwError } from 'rxjs';
import { ToasterService } from '@abp/ng.theme.shared';
import { catchError, finalize, switchMap, tap } from 'rxjs/operators';
import snq from 'snq';
import { HttpHeaders } from '@angular/common/http';
const { maxLength, minLength, required } = Validators;
export class LoginComponent {
    /**
     * @param {?} fb
     * @param {?} oauthService
     * @param {?} store
     * @param {?} toasterService
     * @param {?} options
     */
    constructor(fb, oauthService, store, toasterService, options) {
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
    onSubmit() {
        if (this.form.invalid)
            return;
        // this.oauthService.setStorage(this.form.value.remember ? localStorage : sessionStorage);
        this.inProgress = true;
        /** @type {?} */
        const tenant = this.store.selectSnapshot(SessionState.getTenant);
        from(this.oauthService.fetchTokenUsingPasswordFlow(this.form.get('username').value, this.form.get('password').value, new HttpHeaders(Object.assign({}, (tenant && tenant.id && { __tenant: tenant.id })))))
            .pipe(switchMap((/**
         * @return {?}
         */
        () => this.store.dispatch(new GetAppConfiguration()))), tap((/**
         * @return {?}
         */
        () => {
            /** @type {?} */
            const redirectUrl = snq((/**
             * @return {?}
             */
            () => window.history.state)).redirectUrl || (this.options || {}).redirectUrl || '/';
            this.store.dispatch(new Navigate([redirectUrl]));
        })), catchError((/**
         * @param {?} err
         * @return {?}
         */
        err => {
            this.toasterService.error(snq((/**
             * @return {?}
             */
            () => err.error.error_description)) ||
                snq((/**
                 * @return {?}
                 */
                () => err.error.error.message), 'AbpAccount::DefaultErrorMessage'), 'Error', { life: 7000 });
            return throwError(err);
        })), finalize((/**
         * @return {?}
         */
        () => (this.inProgress = false))))
            .subscribe();
    }
}
LoginComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-login',
                template: "<abp-auth-wrapper [mainContentRef]=\"mainContentRef\" [cancelContentRef]=\"cancelContentRef\">\n  <ng-template #mainContentRef>\n    <h4>{{ 'AbpAccount::Login' | abpLocalization }}</h4>\n    <strong>\n      {{ 'AbpAccount::AreYouANewUser' | abpLocalization }}\n      <a class=\"text-decoration-none\" routerLink=\"/account/register\">{{ 'AbpAccount::Register' | abpLocalization }}</a>\n    </strong>\n    <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" validateOnSubmit class=\"mt-4\">\n      <div class=\"form-group\">\n        <label for=\"login-input-user-name-or-email-address\">{{\n          'AbpAccount::UserNameOrEmailAddress' | abpLocalization\n        }}</label>\n        <input\n          class=\"form-control\"\n          type=\"text\"\n          id=\"login-input-user-name-or-email-address\"\n          formControlName=\"username\"\n          autofocus\n        />\n      </div>\n      <div class=\"form-group\">\n        <label for=\"login-input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label>\n        <input class=\"form-control\" type=\"password\" id=\"login-input-password\" formControlName=\"password\" />\n      </div>\n      <div class=\"form-check\" validationTarget validationStyle>\n        <label class=\"form-check-label\" for=\"login-input-remember-me\">\n          <input class=\"form-check-input\" type=\"checkbox\" id=\"login-input-remember-me\" formControlName=\"remember\" />\n          {{ 'AbpAccount::RememberMe' | abpLocalization }}\n        </label>\n      </div>\n      <abp-button\n        [loading]=\"inProgress\"\n        buttonType=\"submit\"\n        name=\"Action\"\n        buttonClass=\"btn-block btn-lg mt-3 btn btn-primary\"\n      >\n        {{ 'AbpAccount::Login' | abpLocalization }}\n      </abp-button>\n    </form>\n  </ng-template>\n  <ng-template #cancelContentRef>\n    <div class=\"card-footer text-center border-0\">\n      <a routerLink=\"/\">\n        <button type=\"button\" name=\"Action\" value=\"Cancel\" class=\"px-2 py-0 btn btn-link\">\n          {{ 'AbpAccount::Cancel' | abpLocalization }}\n        </button>\n      </a>\n    </div>\n  </ng-template>\n</abp-auth-wrapper>\n"
            }] }
];
/** @nocollapse */
LoginComponent.ctorParameters = () => [
    { type: FormBuilder },
    { type: OAuthService },
    { type: Store },
    { type: ToasterService },
    { type: undefined, decorators: [{ type: Optional }, { type: Inject, args: ['ACCOUNT_OPTIONS',] }] }
];
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9naW4uY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvbG9naW4vbG9naW4uY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLG1CQUFtQixFQUFFLFdBQVcsRUFBRSxZQUFZLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDOUUsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzVELE9BQU8sRUFBRSxXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDcEUsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQy9DLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQ25ELE9BQU8sRUFBRSxJQUFJLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBRXhDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsVUFBVSxFQUFFLFFBQVEsRUFBRSxTQUFTLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdEUsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztNQUU3QyxFQUFFLFNBQVMsRUFBRSxTQUFTLEVBQUUsUUFBUSxFQUFFLEdBQUcsVUFBVTtBQU1yRCxNQUFNLE9BQU8sY0FBYzs7Ozs7Ozs7SUFLekIsWUFDVSxFQUFlLEVBQ2YsWUFBMEIsRUFDMUIsS0FBWSxFQUNaLGNBQThCLEVBQ1MsT0FBZ0I7UUFKdkQsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUNmLGlCQUFZLEdBQVosWUFBWSxDQUFjO1FBQzFC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O1lBaEJRLFdBQVc7WUFHWCxZQUFZO1lBRFosS0FBSztZQUlMLGNBQWM7NENBcUJsQixRQUFRLFlBQUksTUFBTSxTQUFDLGlCQUFpQjs7OztJQVR2Qyw4QkFBZ0I7O0lBRWhCLG9DQUFvQjs7Ozs7SUFHbEIsNEJBQXVCOzs7OztJQUN2QixzQ0FBa0M7Ozs7O0lBQ2xDLCtCQUFvQjs7Ozs7SUFDcEIsd0NBQXNDOzs7OztJQUN0QyxpQ0FBK0QiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBHZXRBcHBDb25maWd1cmF0aW9uLCBDb25maWdTdGF0ZSwgU2Vzc2lvblN0YXRlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IENvbXBvbmVudCwgSW5qZWN0LCBPcHRpb25hbCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgRm9ybUJ1aWxkZXIsIEZvcm1Hcm91cCwgVmFsaWRhdG9ycyB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IE5hdmlnYXRlIH0gZnJvbSAnQG5neHMvcm91dGVyLXBsdWdpbic7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9BdXRoU2VydmljZSB9IGZyb20gJ2FuZ3VsYXItb2F1dGgyLW9pZGMnO1xuaW1wb3J0IHsgZnJvbSwgdGhyb3dFcnJvciB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgT3B0aW9ucyB9IGZyb20gJy4uLy4uL21vZGVscy9vcHRpb25zJztcbmltcG9ydCB7IFRvYXN0ZXJTZXJ2aWNlIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgY2F0Y2hFcnJvciwgZmluYWxpemUsIHN3aXRjaE1hcCwgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuaW1wb3J0IHsgSHR0cEhlYWRlcnMgfSBmcm9tICdAYW5ndWxhci9jb21tb24vaHR0cCc7XG5cbmNvbnN0IHsgbWF4TGVuZ3RoLCBtaW5MZW5ndGgsIHJlcXVpcmVkIH0gPSBWYWxpZGF0b3JzO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtbG9naW4nLFxuICB0ZW1wbGF0ZVVybDogJy4vbG9naW4uY29tcG9uZW50Lmh0bWwnLFxufSlcbmV4cG9ydCBjbGFzcyBMb2dpbkNvbXBvbmVudCB7XG4gIGZvcm06IEZvcm1Hcm91cDtcblxuICBpblByb2dyZXNzOiBib29sZWFuO1xuXG4gIGNvbnN0cnVjdG9yKFxuICAgIHByaXZhdGUgZmI6IEZvcm1CdWlsZGVyLFxuICAgIHByaXZhdGUgb2F1dGhTZXJ2aWNlOiBPQXV0aFNlcnZpY2UsXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXG4gICAgcHJpdmF0ZSB0b2FzdGVyU2VydmljZTogVG9hc3RlclNlcnZpY2UsXG4gICAgQE9wdGlvbmFsKCkgQEluamVjdCgnQUNDT1VOVF9PUFRJT05TJykgcHJpdmF0ZSBvcHRpb25zOiBPcHRpb25zLFxuICApIHtcbiAgICB0aGlzLm9hdXRoU2VydmljZS5jb25maWd1cmUodGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRPbmUoJ2Vudmlyb25tZW50JykpLm9BdXRoQ29uZmlnKTtcbiAgICB0aGlzLm9hdXRoU2VydmljZS5sb2FkRGlzY292ZXJ5RG9jdW1lbnQoKTtcblxuICAgIHRoaXMuZm9ybSA9IHRoaXMuZmIuZ3JvdXAoe1xuICAgICAgdXNlcm5hbWU6IFsnJywgW3JlcXVpcmVkLCBtYXhMZW5ndGgoMjU1KV1dLFxuICAgICAgcGFzc3dvcmQ6IFsnJywgW3JlcXVpcmVkLCBtYXhMZW5ndGgoMzIpXV0sXG4gICAgICByZW1lbWJlcjogW2ZhbHNlXSxcbiAgICB9KTtcbiAgfVxuXG4gIG9uU3VibWl0KCkge1xuICAgIGlmICh0aGlzLmZvcm0uaW52YWxpZCkgcmV0dXJuO1xuICAgIC8vIHRoaXMub2F1dGhTZXJ2aWNlLnNldFN0b3JhZ2UodGhpcy5mb3JtLnZhbHVlLnJlbWVtYmVyID8gbG9jYWxTdG9yYWdlIDogc2Vzc2lvblN0b3JhZ2UpO1xuXG4gICAgdGhpcy5pblByb2dyZXNzID0gdHJ1ZTtcbiAgICBjb25zdCB0ZW5hbnQgPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFNlc3Npb25TdGF0ZS5nZXRUZW5hbnQpO1xuICAgIGZyb20oXG4gICAgICB0aGlzLm9hdXRoU2VydmljZS5mZXRjaFRva2VuVXNpbmdQYXNzd29yZEZsb3coXG4gICAgICAgIHRoaXMuZm9ybS5nZXQoJ3VzZXJuYW1lJykudmFsdWUsXG4gICAgICAgIHRoaXMuZm9ybS5nZXQoJ3Bhc3N3b3JkJykudmFsdWUsXG4gICAgICAgIG5ldyBIdHRwSGVhZGVycyh7IC4uLih0ZW5hbnQgJiYgdGVuYW50LmlkICYmIHsgX190ZW5hbnQ6IHRlbmFudC5pZCB9KSB9KSxcbiAgICAgICksXG4gICAgKVxuICAgICAgLnBpcGUoXG4gICAgICAgIHN3aXRjaE1hcCgoKSA9PiB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBHZXRBcHBDb25maWd1cmF0aW9uKCkpKSxcbiAgICAgICAgdGFwKCgpID0+IHtcbiAgICAgICAgICBjb25zdCByZWRpcmVjdFVybCA9IHNucSgoKSA9PiB3aW5kb3cuaGlzdG9yeS5zdGF0ZSkucmVkaXJlY3RVcmwgfHwgKHRoaXMub3B0aW9ucyB8fCB7fSkucmVkaXJlY3RVcmwgfHwgJy8nO1xuICAgICAgICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IE5hdmlnYXRlKFtyZWRpcmVjdFVybF0pKTtcbiAgICAgICAgfSksXG4gICAgICAgIGNhdGNoRXJyb3IoZXJyID0+IHtcbiAgICAgICAgICB0aGlzLnRvYXN0ZXJTZXJ2aWNlLmVycm9yKFxuICAgICAgICAgICAgc25xKCgpID0+IGVyci5lcnJvci5lcnJvcl9kZXNjcmlwdGlvbikgfHxcbiAgICAgICAgICAgICAgc25xKCgpID0+IGVyci5lcnJvci5lcnJvci5tZXNzYWdlLCAnQWJwQWNjb3VudDo6RGVmYXVsdEVycm9yTWVzc2FnZScpLFxuICAgICAgICAgICAgJ0Vycm9yJyxcbiAgICAgICAgICAgIHsgbGlmZTogNzAwMCB9LFxuICAgICAgICAgICk7XG4gICAgICAgICAgcmV0dXJuIHRocm93RXJyb3IoZXJyKTtcbiAgICAgICAgfSksXG4gICAgICAgIGZpbmFsaXplKCgpID0+ICh0aGlzLmluUHJvZ3Jlc3MgPSBmYWxzZSkpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgpO1xuICB9XG59XG4iXX0=