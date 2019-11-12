/**
 * @fileoverview added by tsickle
 * Generated from: lib/guards/auth.guard.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import * as i0 from "@angular/core";
import * as i1 from "angular-oauth2-oidc";
import * as i2 from "@angular/router";
export class AuthGuard {
    /**
     * @param {?} oauthService
     * @param {?} router
     */
    constructor(oauthService, router) {
        this.oauthService = oauthService;
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
    { type: Router }
];
/** @nocollapse */ AuthGuard.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function AuthGuard_Factory() { return new AuthGuard(i0.ɵɵinject(i1.OAuthService), i0.ɵɵinject(i2.Router)); }, token: AuthGuard, providedIn: "root" });
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
    AuthGuard.prototype.router;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXV0aC5ndWFyZC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9ndWFyZHMvYXV0aC5ndWFyZC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUF1QyxNQUFNLEVBQWdDLE1BQU0saUJBQWlCLENBQUM7QUFDNUcsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHFCQUFxQixDQUFDOzs7O0FBTW5ELE1BQU0sT0FBTyxTQUFTOzs7OztJQUNwQixZQUFvQixZQUEwQixFQUFVLE1BQWM7UUFBbEQsaUJBQVksR0FBWixZQUFZLENBQWM7UUFBVSxXQUFNLEdBQU4sTUFBTSxDQUFRO0lBQUcsQ0FBQzs7Ozs7O0lBRTFFLFdBQVcsQ0FBQyxDQUF5QixFQUFFLEtBQTBCOztjQUN6RCxtQkFBbUIsR0FBRyxJQUFJLENBQUMsWUFBWSxDQUFDLG1CQUFtQixFQUFFO1FBQ25FLElBQUksbUJBQW1CLEVBQUU7WUFDdkIsT0FBTyxtQkFBbUIsQ0FBQztTQUM1QjtRQUVELE9BQU8sSUFBSSxDQUFDLE1BQU0sQ0FBQyxhQUFhLENBQUMsQ0FBQyxnQkFBZ0IsQ0FBQyxFQUFFLEVBQUUsS0FBSyxFQUFFLEVBQUUsV0FBVyxFQUFFLEtBQUssQ0FBQyxHQUFHLEVBQUUsRUFBRSxDQUFDLENBQUM7SUFDOUYsQ0FBQzs7O1lBYkYsVUFBVSxTQUFDO2dCQUNWLFVBQVUsRUFBRSxNQUFNO2FBQ25COzs7O1lBTFEsWUFBWTtZQUR5QixNQUFNOzs7Ozs7OztJQVF0QyxpQ0FBa0M7Ozs7O0lBQUUsMkJBQXNCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90LCBDYW5BY3RpdmF0ZSwgUm91dGVyLCBSb3V0ZXJTdGF0ZVNuYXBzaG90LCBVcmxUcmVlIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcclxuaW1wb3J0IHsgT0F1dGhTZXJ2aWNlIH0gZnJvbSAnYW5ndWxhci1vYXV0aDItb2lkYyc7XHJcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcclxuXHJcbkBJbmplY3RhYmxlKHtcclxuICBwcm92aWRlZEluOiAncm9vdCcsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBBdXRoR3VhcmQgaW1wbGVtZW50cyBDYW5BY3RpdmF0ZSB7XHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBvYXV0aFNlcnZpY2U6IE9BdXRoU2VydmljZSwgcHJpdmF0ZSByb3V0ZXI6IFJvdXRlcikge31cclxuXHJcbiAgY2FuQWN0aXZhdGUoXzogQWN0aXZhdGVkUm91dGVTbmFwc2hvdCwgc3RhdGU6IFJvdXRlclN0YXRlU25hcHNob3QpOiBPYnNlcnZhYmxlPGJvb2xlYW4+IHwgYm9vbGVhbiB8IFVybFRyZWUge1xyXG4gICAgY29uc3QgaGFzVmFsaWRBY2Nlc3NUb2tlbiA9IHRoaXMub2F1dGhTZXJ2aWNlLmhhc1ZhbGlkQWNjZXNzVG9rZW4oKTtcclxuICAgIGlmIChoYXNWYWxpZEFjY2Vzc1Rva2VuKSB7XHJcbiAgICAgIHJldHVybiBoYXNWYWxpZEFjY2Vzc1Rva2VuO1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiB0aGlzLnJvdXRlci5jcmVhdGVVcmxUcmVlKFsnL2FjY291bnQvbG9naW4nXSwgeyBzdGF0ZTogeyByZWRpcmVjdFVybDogc3RhdGUudXJsIH0gfSk7XHJcbiAgfVxyXG59XHJcbiJdfQ==