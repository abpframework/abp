/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import * as i0 from "@angular/core";
import * as i1 from "angular-oauth2-oidc";
import * as i2 from "@ngxs/store";
import * as i3 from "@angular/router";
export class AuthGuard {
    /**
     * @param {?} oauthService
     * @param {?} store
     * @param {?} router
     */
    constructor(oauthService, store, router) {
        this.oauthService = oauthService;
        this.store = store;
        this.router = router;
    }
    /**
     * @param {?} _
     * @param {?} state
     * @return {?}
     */
    canActivate(_, state) {
        /** @type {?} */
        const hasValidAccessToken = this.oauthService.hasValidAccessToken();
        if (hasValidAccessToken) {
            return hasValidAccessToken;
        }
        return this.router.createUrlTree(['/account/login'], { state: { redirectUrl: state.url } });
    }
}
AuthGuard.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
AuthGuard.ctorParameters = () => [
    { type: OAuthService },
    { type: Store },
    { type: Router }
];
/** @nocollapse */ AuthGuard.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function AuthGuard_Factory() { return new AuthGuard(i0.ɵɵinject(i1.OAuthService), i0.ɵɵinject(i2.Store), i0.ɵɵinject(i3.Router)); }, token: AuthGuard, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    AuthGuard.prototype.oauthService;
    /**
     * @type {?}
     * @private
     */
    AuthGuard.prototype.store;
    /**
     * @type {?}
     * @private
     */
    AuthGuard.prototype.router;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXV0aC5ndWFyZC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9ndWFyZHMvYXV0aC5ndWFyZC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQXFFLE1BQU0sRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQzVHLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHFCQUFxQixDQUFDOzs7OztBQU9uRCxNQUFNLE9BQU8sU0FBUzs7Ozs7O0lBQ3BCLFlBQW9CLFlBQTBCLEVBQVUsS0FBWSxFQUFVLE1BQWM7UUFBeEUsaUJBQVksR0FBWixZQUFZLENBQWM7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQVUsV0FBTSxHQUFOLE1BQU0sQ0FBUTtJQUFHLENBQUM7Ozs7OztJQUVoRyxXQUFXLENBQUMsQ0FBeUIsRUFBRSxLQUEwQjs7Y0FDekQsbUJBQW1CLEdBQUcsSUFBSSxDQUFDLFlBQVksQ0FBQyxtQkFBbUIsRUFBRTtRQUNuRSxJQUFJLG1CQUFtQixFQUFFO1lBQ3ZCLE9BQU8sbUJBQW1CLENBQUM7U0FDNUI7UUFFRCxPQUFPLElBQUksQ0FBQyxNQUFNLENBQUMsYUFBYSxDQUFDLENBQUMsZ0JBQWdCLENBQUMsRUFBRSxFQUFFLEtBQUssRUFBRSxFQUFFLFdBQVcsRUFBRSxLQUFLLENBQUMsR0FBRyxFQUFFLEVBQUUsQ0FBQyxDQUFDO0lBQzlGLENBQUM7OztZQWJGLFVBQVUsU0FBQztnQkFDVixVQUFVLEVBQUUsTUFBTTthQUNuQjs7OztZQU5RLFlBQVk7WUFEWixLQUFLO1lBRDhELE1BQU07Ozs7Ozs7O0lBVXBFLGlDQUFrQzs7Ozs7SUFBRSwwQkFBb0I7Ozs7O0lBQUUsMkJBQXNCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQWN0aXZhdGVkUm91dGVTbmFwc2hvdCwgQ2FuQWN0aXZhdGUsIFJvdXRlclN0YXRlU25hcHNob3QsIFVybFRyZWUsIFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9BdXRoU2VydmljZSB9IGZyb20gJ2FuZ3VsYXItb2F1dGgyLW9pZGMnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgTmF2aWdhdGUgfSBmcm9tICdAbmd4cy9yb3V0ZXItcGx1Z2luJztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIEF1dGhHdWFyZCBpbXBsZW1lbnRzIENhbkFjdGl2YXRlIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBvYXV0aFNlcnZpY2U6IE9BdXRoU2VydmljZSwgcHJpdmF0ZSBzdG9yZTogU3RvcmUsIHByaXZhdGUgcm91dGVyOiBSb3V0ZXIpIHt9XG5cbiAgY2FuQWN0aXZhdGUoXzogQWN0aXZhdGVkUm91dGVTbmFwc2hvdCwgc3RhdGU6IFJvdXRlclN0YXRlU25hcHNob3QpOiBPYnNlcnZhYmxlPGJvb2xlYW4+IHwgYm9vbGVhbiB8IFVybFRyZWUge1xuICAgIGNvbnN0IGhhc1ZhbGlkQWNjZXNzVG9rZW4gPSB0aGlzLm9hdXRoU2VydmljZS5oYXNWYWxpZEFjY2Vzc1Rva2VuKCk7XG4gICAgaWYgKGhhc1ZhbGlkQWNjZXNzVG9rZW4pIHtcbiAgICAgIHJldHVybiBoYXNWYWxpZEFjY2Vzc1Rva2VuO1xuICAgIH1cblxuICAgIHJldHVybiB0aGlzLnJvdXRlci5jcmVhdGVVcmxUcmVlKFsnL2FjY291bnQvbG9naW4nXSwgeyBzdGF0ZTogeyByZWRpcmVjdFVybDogc3RhdGUudXJsIH0gfSk7XG4gIH1cbn1cbiJdfQ==