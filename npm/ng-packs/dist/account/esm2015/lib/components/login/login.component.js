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
            const redirectUrl =
              snq(
                /**
                 * @return {?}
                 */
                (() => window.history.state),
              ).redirectUrl ||
              (this.options || {}).redirectUrl ||
              '/';
            this.store.dispatch(new Navigate([redirectUrl]));
          },
        ),
        catchError(
          /**
           * @param {?} err
           * @return {?}
           */
          err => {
            this.toasterService.error(
              snq(
                /**
                 * @return {?}
                 */
                () => err.error.error_description,
              ) ||
                snq(
                  /**
                   * @return {?}
                   */
                  () => err.error.error.message,
                  'AbpAccount::DefaultErrorMessage',
                ),
              'Error',
              { life: 7000 },
            );
            return throwError(err);
          },
        ),
        finalize(
          /**
           * @return {?}
           */
          () => (this.inProgress = false),
        ),
      )
      .subscribe();
  }
}
LoginComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-login',
                template: "<abp-auth-wrapper [mainContentRef]=\"mainContentRef\" [cancelContentRef]=\"cancelContentRef\">\r\n  <ng-template #mainContentRef>\r\n    <h4>{{ 'AbpAccount::Login' | abpLocalization }}</h4>\r\n    <strong>\r\n      {{ 'AbpAccount::AreYouANewUser' | abpLocalization }}\r\n      <a class=\"text-decoration-none\" routerLink=\"/account/register\">{{ 'AbpAccount::Register' | abpLocalization }}</a>\r\n    </strong>\r\n    <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" validateOnSubmit class=\"mt-4\">\r\n      <div class=\"form-group\">\r\n        <label for=\"login-input-user-name-or-email-address\">{{\r\n          'AbpAccount::UserNameOrEmailAddress' | abpLocalization\r\n        }}</label>\r\n        <input\r\n          class=\"form-control\"\r\n          type=\"text\"\r\n          id=\"login-input-user-name-or-email-address\"\r\n          formControlName=\"username\"\r\n          autofocus\r\n        />\r\n      </div>\r\n      <div class=\"form-group\">\r\n        <label for=\"login-input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label>\r\n        <input class=\"form-control\" type=\"password\" id=\"login-input-password\" formControlName=\"password\" />\r\n      </div>\r\n      <div class=\"form-check\" validationTarget validationStyle>\r\n        <label class=\"form-check-label\" for=\"login-input-remember-me\">\r\n          <input class=\"form-check-input\" type=\"checkbox\" id=\"login-input-remember-me\" formControlName=\"remember\" />\r\n          {{ 'AbpAccount::RememberMe' | abpLocalization }}\r\n        </label>\r\n      </div>\r\n      <abp-button\r\n        [loading]=\"inProgress\"\r\n        buttonType=\"submit\"\r\n        name=\"Action\"\r\n        buttonClass=\"btn-block btn-lg mt-3 btn btn-primary\"\r\n      >\r\n        {{ 'AbpAccount::Login' | abpLocalization }}\r\n      </abp-button>\r\n    </form>\r\n  </ng-template>\r\n  <ng-template #cancelContentRef>\r\n    <div class=\"card-footer text-center border-0\">\r\n      <a routerLink=\"/\">\r\n        <button type=\"button\" name=\"Action\" value=\"Cancel\" class=\"px-2 py-0 btn btn-link\">\r\n          {{ 'AbpAccount::Cancel' | abpLocalization }}\r\n        </button>\r\n      </a>\r\n    </div>\r\n  </ng-template>\r\n</abp-auth-wrapper>\r\n"
            }] }
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9naW4uY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvbG9naW4vbG9naW4uY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLG1CQUFtQixFQUFFLFdBQVcsRUFBRSxZQUFZLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDOUUsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzVELE9BQU8sRUFBRSxXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDcEUsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQy9DLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQ25ELE9BQU8sRUFBRSxJQUFJLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBRXhDLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsVUFBVSxFQUFFLFFBQVEsRUFBRSxTQUFTLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdEUsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztNQUU3QyxFQUFFLFNBQVMsRUFBRSxTQUFTLEVBQUUsUUFBUSxFQUFFLEdBQUcsVUFBVTtBQU1yRCxNQUFNLE9BQU8sY0FBYzs7Ozs7Ozs7SUFLekIsWUFDVSxFQUFlLEVBQ2YsWUFBMEIsRUFDMUIsS0FBWSxFQUNaLGNBQThCLEVBQ1MsT0FBZ0I7UUFKdkQsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUNmLGlCQUFZLEdBQVosWUFBWSxDQUFjO1FBQzFC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O1lBaEJRLFdBQVc7WUFHWCxZQUFZO1lBRFosS0FBSztZQUlMLGNBQWM7NENBcUJsQixRQUFRLFlBQUksTUFBTSxTQUFDLGlCQUFpQjs7OztJQVR2Qyw4QkFBZ0I7O0lBRWhCLG9DQUFvQjs7Ozs7SUFHbEIsNEJBQXVCOzs7OztJQUN2QixzQ0FBa0M7Ozs7O0lBQ2xDLCtCQUFvQjs7Ozs7SUFDcEIsd0NBQXNDOzs7OztJQUN0QyxpQ0FBK0QiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBHZXRBcHBDb25maWd1cmF0aW9uLCBDb25maWdTdGF0ZSwgU2Vzc2lvblN0YXRlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgQ29tcG9uZW50LCBJbmplY3QsIE9wdGlvbmFsIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IEZvcm1CdWlsZGVyLCBGb3JtR3JvdXAsIFZhbGlkYXRvcnMgfSBmcm9tICdAYW5ndWxhci9mb3Jtcyc7XHJcbmltcG9ydCB7IE5hdmlnYXRlIH0gZnJvbSAnQG5neHMvcm91dGVyLXBsdWdpbic7XHJcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBPQXV0aFNlcnZpY2UgfSBmcm9tICdhbmd1bGFyLW9hdXRoMi1vaWRjJztcclxuaW1wb3J0IHsgZnJvbSwgdGhyb3dFcnJvciB9IGZyb20gJ3J4anMnO1xyXG5pbXBvcnQgeyBPcHRpb25zIH0gZnJvbSAnLi4vLi4vbW9kZWxzL29wdGlvbnMnO1xyXG5pbXBvcnQgeyBUb2FzdGVyU2VydmljZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcclxuaW1wb3J0IHsgY2F0Y2hFcnJvciwgZmluYWxpemUsIHN3aXRjaE1hcCwgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XHJcbmltcG9ydCB7IEh0dHBIZWFkZXJzIH0gZnJvbSAnQGFuZ3VsYXIvY29tbW9uL2h0dHAnO1xyXG5cclxuY29uc3QgeyBtYXhMZW5ndGgsIG1pbkxlbmd0aCwgcmVxdWlyZWQgfSA9IFZhbGlkYXRvcnM7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1sb2dpbicsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL2xvZ2luLmNvbXBvbmVudC5odG1sJyxcclxufSlcclxuZXhwb3J0IGNsYXNzIExvZ2luQ29tcG9uZW50IHtcclxuICBmb3JtOiBGb3JtR3JvdXA7XHJcblxyXG4gIGluUHJvZ3Jlc3M6IGJvb2xlYW47XHJcblxyXG4gIGNvbnN0cnVjdG9yKFxyXG4gICAgcHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIsXHJcbiAgICBwcml2YXRlIG9hdXRoU2VydmljZTogT0F1dGhTZXJ2aWNlLFxyXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXHJcbiAgICBwcml2YXRlIHRvYXN0ZXJTZXJ2aWNlOiBUb2FzdGVyU2VydmljZSxcclxuICAgIEBPcHRpb25hbCgpIEBJbmplY3QoJ0FDQ09VTlRfT1BUSU9OUycpIHByaXZhdGUgb3B0aW9uczogT3B0aW9ucyxcclxuICApIHtcclxuICAgIHRoaXMub2F1dGhTZXJ2aWNlLmNvbmZpZ3VyZSh0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldE9uZSgnZW52aXJvbm1lbnQnKSkub0F1dGhDb25maWcpO1xyXG4gICAgdGhpcy5vYXV0aFNlcnZpY2UubG9hZERpc2NvdmVyeURvY3VtZW50KCk7XHJcblxyXG4gICAgdGhpcy5mb3JtID0gdGhpcy5mYi5ncm91cCh7XHJcbiAgICAgIHVzZXJuYW1lOiBbJycsIFtyZXF1aXJlZCwgbWF4TGVuZ3RoKDI1NSldXSxcclxuICAgICAgcGFzc3dvcmQ6IFsnJywgW3JlcXVpcmVkLCBtYXhMZW5ndGgoMzIpXV0sXHJcbiAgICAgIHJlbWVtYmVyOiBbZmFsc2VdLFxyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBvblN1Ym1pdCgpIHtcclxuICAgIGlmICh0aGlzLmZvcm0uaW52YWxpZCkgcmV0dXJuO1xyXG4gICAgLy8gdGhpcy5vYXV0aFNlcnZpY2Uuc2V0U3RvcmFnZSh0aGlzLmZvcm0udmFsdWUucmVtZW1iZXIgPyBsb2NhbFN0b3JhZ2UgOiBzZXNzaW9uU3RvcmFnZSk7XHJcblxyXG4gICAgdGhpcy5pblByb2dyZXNzID0gdHJ1ZTtcclxuICAgIGNvbnN0IHRlbmFudCA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoU2Vzc2lvblN0YXRlLmdldFRlbmFudCk7XHJcbiAgICBmcm9tKFxyXG4gICAgICB0aGlzLm9hdXRoU2VydmljZS5mZXRjaFRva2VuVXNpbmdQYXNzd29yZEZsb3coXHJcbiAgICAgICAgdGhpcy5mb3JtLmdldCgndXNlcm5hbWUnKS52YWx1ZSxcclxuICAgICAgICB0aGlzLmZvcm0uZ2V0KCdwYXNzd29yZCcpLnZhbHVlLFxyXG4gICAgICAgIG5ldyBIdHRwSGVhZGVycyh7IC4uLih0ZW5hbnQgJiYgdGVuYW50LmlkICYmIHsgX190ZW5hbnQ6IHRlbmFudC5pZCB9KSB9KSxcclxuICAgICAgKSxcclxuICAgIClcclxuICAgICAgLnBpcGUoXHJcbiAgICAgICAgc3dpdGNoTWFwKCgpID0+IHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IEdldEFwcENvbmZpZ3VyYXRpb24oKSkpLFxyXG4gICAgICAgIHRhcCgoKSA9PiB7XHJcbiAgICAgICAgICBjb25zdCByZWRpcmVjdFVybCA9IHNucSgoKSA9PiB3aW5kb3cuaGlzdG9yeS5zdGF0ZSkucmVkaXJlY3RVcmwgfHwgKHRoaXMub3B0aW9ucyB8fCB7fSkucmVkaXJlY3RVcmwgfHwgJy8nO1xyXG4gICAgICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgTmF2aWdhdGUoW3JlZGlyZWN0VXJsXSkpO1xyXG4gICAgICAgIH0pLFxyXG4gICAgICAgIGNhdGNoRXJyb3IoZXJyID0+IHtcclxuICAgICAgICAgIHRoaXMudG9hc3RlclNlcnZpY2UuZXJyb3IoXHJcbiAgICAgICAgICAgIHNucSgoKSA9PiBlcnIuZXJyb3IuZXJyb3JfZGVzY3JpcHRpb24pIHx8XHJcbiAgICAgICAgICAgICAgc25xKCgpID0+IGVyci5lcnJvci5lcnJvci5tZXNzYWdlLCAnQWJwQWNjb3VudDo6RGVmYXVsdEVycm9yTWVzc2FnZScpLFxyXG4gICAgICAgICAgICAnRXJyb3InLFxyXG4gICAgICAgICAgICB7IGxpZmU6IDcwMDAgfSxcclxuICAgICAgICAgICk7XHJcbiAgICAgICAgICByZXR1cm4gdGhyb3dFcnJvcihlcnIpO1xyXG4gICAgICAgIH0pLFxyXG4gICAgICAgIGZpbmFsaXplKCgpID0+ICh0aGlzLmluUHJvZ3Jlc3MgPSBmYWxzZSkpLFxyXG4gICAgICApXHJcbiAgICAgIC5zdWJzY3JpYmUoKTtcclxuICB9XHJcbn1cclxuIl19
