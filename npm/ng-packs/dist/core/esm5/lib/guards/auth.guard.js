/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import * as i0 from '@angular/core';
import * as i1 from 'angular-oauth2-oidc';
import * as i2 from '@angular/router';
var AuthGuard = /** @class */ (function() {
  function AuthGuard(oauthService, router) {
    this.oauthService = oauthService;
    this.router = router;
  }
  /**
   * @param {?} _
   * @param {?} state
   * @return {?}
   */
  AuthGuard.prototype.canActivate
  /**
   * @param {?} _
   * @param {?} state
   * @return {?}
   */ = function(_, state) {
    /** @type {?} */
    var hasValidAccessToken = this.oauthService.hasValidAccessToken();
    if (hasValidAccessToken) {
      return hasValidAccessToken;
    }
    return this.router.createUrlTree(['/account/login'], { state: { redirectUrl: state.url } });
  };
  AuthGuard.decorators = [
    {
      type: Injectable,
      args: [
        {
          providedIn: 'root',
        },
      ],
    },
  ];
  /** @nocollapse */
  AuthGuard.ctorParameters = function() {
    return [{ type: OAuthService }, { type: Router }];
  };
  /** @nocollapse */ AuthGuard.ngInjectableDef = i0.ɵɵdefineInjectable({
    factory: function AuthGuard_Factory() {
      return new AuthGuard(i0.ɵɵinject(i1.OAuthService), i0.ɵɵinject(i2.Router));
    },
    token: AuthGuard,
    providedIn: 'root',
  });
  return AuthGuard;
})();
export { AuthGuard };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXV0aC5ndWFyZC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9ndWFyZHMvYXV0aC5ndWFyZC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQXVDLE1BQU0sRUFBZ0MsTUFBTSxpQkFBaUIsQ0FBQztBQUM1RyxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0scUJBQXFCLENBQUM7Ozs7QUFHbkQ7SUFJRSxtQkFBb0IsWUFBMEIsRUFBVSxNQUFjO1FBQWxELGlCQUFZLEdBQVosWUFBWSxDQUFjO1FBQVUsV0FBTSxHQUFOLE1BQU0sQ0FBUTtJQUFHLENBQUM7Ozs7OztJQUUxRSwrQkFBVzs7Ozs7SUFBWCxVQUFZLENBQXlCLEVBQUUsS0FBMEI7O1lBQ3pELG1CQUFtQixHQUFHLElBQUksQ0FBQyxZQUFZLENBQUMsbUJBQW1CLEVBQUU7UUFDbkUsSUFBSSxtQkFBbUIsRUFBRTtZQUN2QixPQUFPLG1CQUFtQixDQUFDO1NBQzVCO1FBRUQsT0FBTyxJQUFJLENBQUMsTUFBTSxDQUFDLGFBQWEsQ0FBQyxDQUFDLGdCQUFnQixDQUFDLEVBQUUsRUFBRSxLQUFLLEVBQUUsRUFBRSxXQUFXLEVBQUUsS0FBSyxDQUFDLEdBQUcsRUFBRSxFQUFFLENBQUMsQ0FBQztJQUM5RixDQUFDOztnQkFiRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQUxRLFlBQVk7Z0JBRHlCLE1BQU07OztvQkFEcEQ7Q0FtQkMsQUFkRCxJQWNDO1NBWFksU0FBUzs7Ozs7O0lBQ1IsaUNBQWtDOzs7OztJQUFFLDJCQUFzQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEFjdGl2YXRlZFJvdXRlU25hcHNob3QsIENhbkFjdGl2YXRlLCBSb3V0ZXIsIFJvdXRlclN0YXRlU25hcHNob3QsIFVybFRyZWUgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgT0F1dGhTZXJ2aWNlIH0gZnJvbSAnYW5ndWxhci1vYXV0aDItb2lkYyc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBBdXRoR3VhcmQgaW1wbGVtZW50cyBDYW5BY3RpdmF0ZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgb2F1dGhTZXJ2aWNlOiBPQXV0aFNlcnZpY2UsIHByaXZhdGUgcm91dGVyOiBSb3V0ZXIpIHt9XG5cbiAgY2FuQWN0aXZhdGUoXzogQWN0aXZhdGVkUm91dGVTbmFwc2hvdCwgc3RhdGU6IFJvdXRlclN0YXRlU25hcHNob3QpOiBPYnNlcnZhYmxlPGJvb2xlYW4+IHwgYm9vbGVhbiB8IFVybFRyZWUge1xuICAgIGNvbnN0IGhhc1ZhbGlkQWNjZXNzVG9rZW4gPSB0aGlzLm9hdXRoU2VydmljZS5oYXNWYWxpZEFjY2Vzc1Rva2VuKCk7XG4gICAgaWYgKGhhc1ZhbGlkQWNjZXNzVG9rZW4pIHtcbiAgICAgIHJldHVybiBoYXNWYWxpZEFjY2Vzc1Rva2VuO1xuICAgIH1cblxuICAgIHJldHVybiB0aGlzLnJvdXRlci5jcmVhdGVVcmxUcmVlKFsnL2FjY291bnQvbG9naW4nXSwgeyBzdGF0ZTogeyByZWRpcmVjdFVybDogc3RhdGUudXJsIH0gfSk7XG4gIH1cbn1cbiJdfQ==
