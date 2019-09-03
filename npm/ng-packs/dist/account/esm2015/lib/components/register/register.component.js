/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ConfigState, GetAppConfiguration } from '@abp/ng.core';
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
const { maxLength, minLength, required, email } = Validators;
export class RegisterComponent {
    /**
     * @param {?} fb
     * @param {?} accountService
     * @param {?} oauthService
     * @param {?} store
     * @param {?} toasterService
     */
    constructor(fb, accountService, oauthService, store, toasterService) {
        this.fb = fb;
        this.accountService = accountService;
        this.oauthService = oauthService;
        this.store = store;
        this.toasterService = toasterService;
        this.oauthService.configure(this.store.selectSnapshot(ConfigState.getOne('environment')).oAuthConfig);
        this.oauthService.loadDiscoveryDocument();
        this.form = this.fb.group({
            username: ['', [required, maxLength(255)]],
            password: ['', [required, maxLength(32)]],
            email: ['', [required, email]],
        });
    }
    /**
     * @return {?}
     */
    onSubmit() {
        if (this.form.invalid)
            return;
        this.inProgress = true;
        /** @type {?} */
        const newUser = (/** @type {?} */ ({
            userName: this.form.get('username').value,
            password: this.form.get('password').value,
            emailAddress: this.form.get('email').value,
            appName: 'Angular',
        }));
        this.accountService
            .register(newUser)
            .pipe(switchMap((/**
         * @return {?}
         */
        () => from(this.oauthService.fetchTokenUsingPasswordFlow(newUser.userName, newUser.password)))), switchMap((/**
         * @return {?}
         */
        () => this.store.dispatch(new GetAppConfiguration()))), tap((/**
         * @return {?}
         */
        () => this.store.dispatch(new Navigate(['/'])))), take(1), catchError((/**
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
RegisterComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-register',
                template: "<div class=\"row\">\n  <div class=\"col col-md-4 offset-md-4\">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class=\"abp-account-container\">\n      <h2>{{ 'AbpAccount::Register' | abpLocalization }}</h2>\n      <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n        <div class=\"form-group\">\n          <label for=\"input-user-name\">{{ 'AbpAccount::UserName' | abpLocalization }}</label\n          ><span> * </span\n          ><input autofocus type=\"text\" id=\"input-user-name\" class=\"form-control\" formControlName=\"username\" />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"input-email-address\">{{ 'AbpAccount::EmailAddress' | abpLocalization }}</label\n          ><span> * </span><input type=\"email\" id=\"input-email-address\" class=\"form-control\" formControlName=\"email\" />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label\n          ><span> * </span><input type=\"password\" id=\"input-password\" class=\"form-control\" formControlName=\"password\" />\n        </div>\n        <abp-button [loading]=\"inProgress\" type=\"submit\">\n          {{ 'AbpAccount::Register' | abpLocalization }}\n        </abp-button>\n      </form>\n      <div style=\"padding-top: 20px\">\n        <a routerLink=\"/account/login\">{{ 'AbpAccount::Login' | abpLocalization }}</a>\n      </div>\n    </div>\n  </div>\n</div>\n"
            }] }
];
/** @nocollapse */
RegisterComponent.ctorParameters = () => [
    { type: FormBuilder },
    { type: AccountService },
    { type: OAuthService },
    { type: Store },
    { type: ToasterService }
];
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicmVnaXN0ZXIuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5hY2NvdW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcmVnaXN0ZXIvcmVnaXN0ZXIuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsV0FBVyxFQUFFLG1CQUFtQixFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ2hFLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzFDLE9BQU8sRUFBRSxXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDcEUsT0FBTyxFQUFFLFFBQVEsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQy9DLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQ25ELE9BQU8sRUFBRSxJQUFJLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ3hDLE9BQU8sRUFBRSxVQUFVLEVBQUUsUUFBUSxFQUFFLFNBQVMsRUFBRSxJQUFJLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDNUUsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBRXRCLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxnQ0FBZ0MsQ0FBQztNQUMxRCxFQUFFLFNBQVMsRUFBRSxTQUFTLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBRSxHQUFHLFVBQVU7QUFNNUQsTUFBTSxPQUFPLGlCQUFpQjs7Ozs7Ozs7SUFLNUIsWUFDVSxFQUFlLEVBQ2YsY0FBOEIsRUFDOUIsWUFBMEIsRUFDMUIsS0FBWSxFQUNaLGNBQThCO1FBSjlCLE9BQUUsR0FBRixFQUFFLENBQWE7UUFDZixtQkFBYyxHQUFkLGNBQWMsQ0FBZ0I7UUFDOUIsaUJBQVksR0FBWixZQUFZLENBQWM7UUFDMUIsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUNaLG1CQUFjLEdBQWQsY0FBYyxDQUFnQjtRQUV0QyxJQUFJLENBQUMsWUFBWSxDQUFDLFNBQVMsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsTUFBTSxDQUFDLGFBQWEsQ0FBQyxDQUFDLENBQUMsV0FBVyxDQUFDLENBQUM7UUFDdEcsSUFBSSxDQUFDLFlBQVksQ0FBQyxxQkFBcUIsRUFBRSxDQUFDO1FBRTFDLElBQUksQ0FBQyxJQUFJLEdBQUcsSUFBSSxDQUFDLEVBQUUsQ0FBQyxLQUFLLENBQUM7WUFDeEIsUUFBUSxFQUFFLENBQUMsRUFBRSxFQUFFLENBQUMsUUFBUSxFQUFFLFNBQVMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO1lBQzFDLFFBQVEsRUFBRSxDQUFDLEVBQUUsRUFBRSxDQUFDLFFBQVEsRUFBRSxTQUFTLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQztZQUN6QyxLQUFLLEVBQUUsQ0FBQyxFQUFFLEVBQUUsQ0FBQyxRQUFRLEVBQUUsS0FBSyxDQUFDLENBQUM7U0FDL0IsQ0FBQyxDQUFDO0lBQ0wsQ0FBQzs7OztJQUVELFFBQVE7UUFDTixJQUFJLElBQUksQ0FBQyxJQUFJLENBQUMsT0FBTztZQUFFLE9BQU87UUFFOUIsSUFBSSxDQUFDLFVBQVUsR0FBRyxJQUFJLENBQUM7O2NBRWpCLE9BQU8sR0FBRyxtQkFBQTtZQUNkLFFBQVEsRUFBRSxJQUFJLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxVQUFVLENBQUMsQ0FBQyxLQUFLO1lBQ3pDLFFBQVEsRUFBRSxJQUFJLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxVQUFVLENBQUMsQ0FBQyxLQUFLO1lBQ3pDLFlBQVksRUFBRSxJQUFJLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxPQUFPLENBQUMsQ0FBQyxLQUFLO1lBQzFDLE9BQU8sRUFBRSxTQUFTO1NBQ25CLEVBQW1CO1FBRXBCLElBQUksQ0FBQyxjQUFjO2FBQ2hCLFFBQVEsQ0FBQyxPQUFPLENBQUM7YUFDakIsSUFBSSxDQUNILFNBQVM7OztRQUFDLEdBQUcsRUFBRSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsWUFBWSxDQUFDLDJCQUEyQixDQUFDLE9BQU8sQ0FBQyxRQUFRLEVBQUUsT0FBTyxDQUFDLFFBQVEsQ0FBQyxDQUFDLEVBQUMsRUFDeEcsU0FBUzs7O1FBQUMsR0FBRyxFQUFFLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxtQkFBbUIsRUFBRSxDQUFDLEVBQUMsRUFDL0QsR0FBRzs7O1FBQUMsR0FBRyxFQUFFLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxRQUFRLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLEVBQUMsRUFDbkQsSUFBSSxDQUFDLENBQUMsQ0FBQyxFQUNQLFVBQVU7Ozs7UUFBQyxHQUFHLENBQUMsRUFBRTtZQUNmLElBQUksQ0FBQyxjQUFjLENBQUMsS0FBSyxDQUN2QixHQUFHOzs7WUFBQyxHQUFHLEVBQUUsQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDLGlCQUFpQixFQUFDO2dCQUNwQyxHQUFHOzs7Z0JBQUMsR0FBRyxFQUFFLENBQUMsR0FBRyxDQUFDLEtBQUssQ0FBQyxLQUFLLENBQUMsT0FBTyxHQUFFLGlDQUFpQyxDQUFDLEVBQ3ZFLE9BQU8sRUFDUCxFQUFFLElBQUksRUFBRSxJQUFJLEVBQUUsQ0FDZixDQUFDO1lBQ0YsT0FBTyxVQUFVLENBQUMsR0FBRyxDQUFDLENBQUM7UUFDekIsQ0FBQyxFQUFDLEVBQ0YsUUFBUTs7O1FBQUMsR0FBRyxFQUFFLENBQUMsQ0FBQyxJQUFJLENBQUMsVUFBVSxHQUFHLEtBQUssQ0FBQyxFQUFDLENBQzFDO2FBQ0EsU0FBUyxFQUFFLENBQUM7SUFDakIsQ0FBQzs7O1lBekRGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsY0FBYztnQkFDeEIsaTlDQUF3QzthQUN6Qzs7OztZQWRRLFdBQVc7WUFRWCxjQUFjO1lBTGQsWUFBWTtZQURaLEtBQUs7WUFKTCxjQUFjOzs7O0lBa0JyQixpQ0FBZ0I7O0lBRWhCLHVDQUFvQjs7Ozs7SUFHbEIsK0JBQXVCOzs7OztJQUN2QiwyQ0FBc0M7Ozs7O0lBQ3RDLHlDQUFrQzs7Ozs7SUFDbEMsa0NBQW9COzs7OztJQUNwQiwyQ0FBc0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb25maWdTdGF0ZSwgR2V0QXBwQ29uZmlndXJhdGlvbiB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBUb2FzdGVyU2VydmljZSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IENvbXBvbmVudCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgRm9ybUJ1aWxkZXIsIEZvcm1Hcm91cCwgVmFsaWRhdG9ycyB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IE5hdmlnYXRlIH0gZnJvbSAnQG5neHMvcm91dGVyLXBsdWdpbic7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9BdXRoU2VydmljZSB9IGZyb20gJ2FuZ3VsYXItb2F1dGgyLW9pZGMnO1xuaW1wb3J0IHsgZnJvbSwgdGhyb3dFcnJvciB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgY2F0Y2hFcnJvciwgZmluYWxpemUsIHN3aXRjaE1hcCwgdGFrZSwgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuaW1wb3J0IHsgUmVnaXN0ZXJSZXF1ZXN0IH0gZnJvbSAnLi4vLi4vbW9kZWxzJztcbmltcG9ydCB7IEFjY291bnRTZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvYWNjb3VudC5zZXJ2aWNlJztcbmNvbnN0IHsgbWF4TGVuZ3RoLCBtaW5MZW5ndGgsIHJlcXVpcmVkLCBlbWFpbCB9ID0gVmFsaWRhdG9ycztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXJlZ2lzdGVyJyxcbiAgdGVtcGxhdGVVcmw6ICcuL3JlZ2lzdGVyLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgUmVnaXN0ZXJDb21wb25lbnQge1xuICBmb3JtOiBGb3JtR3JvdXA7XG5cbiAgaW5Qcm9ncmVzczogYm9vbGVhbjtcblxuICBjb25zdHJ1Y3RvcihcbiAgICBwcml2YXRlIGZiOiBGb3JtQnVpbGRlcixcbiAgICBwcml2YXRlIGFjY291bnRTZXJ2aWNlOiBBY2NvdW50U2VydmljZSxcbiAgICBwcml2YXRlIG9hdXRoU2VydmljZTogT0F1dGhTZXJ2aWNlLFxuICAgIHByaXZhdGUgc3RvcmU6IFN0b3JlLFxuICAgIHByaXZhdGUgdG9hc3RlclNlcnZpY2U6IFRvYXN0ZXJTZXJ2aWNlLFxuICApIHtcbiAgICB0aGlzLm9hdXRoU2VydmljZS5jb25maWd1cmUodGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRPbmUoJ2Vudmlyb25tZW50JykpLm9BdXRoQ29uZmlnKTtcbiAgICB0aGlzLm9hdXRoU2VydmljZS5sb2FkRGlzY292ZXJ5RG9jdW1lbnQoKTtcblxuICAgIHRoaXMuZm9ybSA9IHRoaXMuZmIuZ3JvdXAoe1xuICAgICAgdXNlcm5hbWU6IFsnJywgW3JlcXVpcmVkLCBtYXhMZW5ndGgoMjU1KV1dLFxuICAgICAgcGFzc3dvcmQ6IFsnJywgW3JlcXVpcmVkLCBtYXhMZW5ndGgoMzIpXV0sXG4gICAgICBlbWFpbDogWycnLCBbcmVxdWlyZWQsIGVtYWlsXV0sXG4gICAgfSk7XG4gIH1cblxuICBvblN1Ym1pdCgpIHtcbiAgICBpZiAodGhpcy5mb3JtLmludmFsaWQpIHJldHVybjtcblxuICAgIHRoaXMuaW5Qcm9ncmVzcyA9IHRydWU7XG5cbiAgICBjb25zdCBuZXdVc2VyID0ge1xuICAgICAgdXNlck5hbWU6IHRoaXMuZm9ybS5nZXQoJ3VzZXJuYW1lJykudmFsdWUsXG4gICAgICBwYXNzd29yZDogdGhpcy5mb3JtLmdldCgncGFzc3dvcmQnKS52YWx1ZSxcbiAgICAgIGVtYWlsQWRkcmVzczogdGhpcy5mb3JtLmdldCgnZW1haWwnKS52YWx1ZSxcbiAgICAgIGFwcE5hbWU6ICdBbmd1bGFyJyxcbiAgICB9IGFzIFJlZ2lzdGVyUmVxdWVzdDtcblxuICAgIHRoaXMuYWNjb3VudFNlcnZpY2VcbiAgICAgIC5yZWdpc3RlcihuZXdVc2VyKVxuICAgICAgLnBpcGUoXG4gICAgICAgIHN3aXRjaE1hcCgoKSA9PiBmcm9tKHRoaXMub2F1dGhTZXJ2aWNlLmZldGNoVG9rZW5Vc2luZ1Bhc3N3b3JkRmxvdyhuZXdVc2VyLnVzZXJOYW1lLCBuZXdVc2VyLnBhc3N3b3JkKSkpLFxuICAgICAgICBzd2l0Y2hNYXAoKCkgPT4gdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgR2V0QXBwQ29uZmlndXJhdGlvbigpKSksXG4gICAgICAgIHRhcCgoKSA9PiB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBOYXZpZ2F0ZShbJy8nXSkpKSxcbiAgICAgICAgdGFrZSgxKSxcbiAgICAgICAgY2F0Y2hFcnJvcihlcnIgPT4ge1xuICAgICAgICAgIHRoaXMudG9hc3RlclNlcnZpY2UuZXJyb3IoXG4gICAgICAgICAgICBzbnEoKCkgPT4gZXJyLmVycm9yLmVycm9yX2Rlc2NyaXB0aW9uKSB8fFxuICAgICAgICAgICAgICBzbnEoKCkgPT4gZXJyLmVycm9yLmVycm9yLm1lc3NhZ2UsICdBYnBBY2NvdW50OjpEZWZhdWx0RXJyb3JNZXNzYWdlJyksXG4gICAgICAgICAgICAnRXJyb3InLFxuICAgICAgICAgICAgeyBsaWZlOiA3MDAwIH0sXG4gICAgICAgICAgKTtcbiAgICAgICAgICByZXR1cm4gdGhyb3dFcnJvcihlcnIpO1xuICAgICAgICB9KSxcbiAgICAgICAgZmluYWxpemUoKCkgPT4gKHRoaXMuaW5Qcm9ncmVzcyA9IGZhbHNlKSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKCk7XG4gIH1cbn1cbiJdfQ==