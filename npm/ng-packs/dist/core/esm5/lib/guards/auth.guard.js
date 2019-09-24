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
var AuthGuard = /** @class */ (function () {
    function AuthGuard(oauthService, store, router) {
        this.oauthService = oauthService;
        this.store = store;
        this.router = router;
    }
    /**
     * @param {?} _
     * @param {?} state
     * @return {?}
     */
    AuthGuard.prototype.canActivate = /**
     * @param {?} _
     * @param {?} state
     * @return {?}
     */
    function (_, state) {
        /** @type {?} */
        var hasValidAccessToken = this.oauthService.hasValidAccessToken();
        if (hasValidAccessToken) {
            return hasValidAccessToken;
        }
        return this.router.createUrlTree(['/account/login'], { state: { redirectUrl: state.url } });
    };
    AuthGuard.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    AuthGuard.ctorParameters = function () { return [
        { type: OAuthService },
        { type: Store },
        { type: Router }
    ]; };
    /** @nocollapse */ AuthGuard.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function AuthGuard_Factory() { return new AuthGuard(i0.ɵɵinject(i1.OAuthService), i0.ɵɵinject(i2.Store), i0.ɵɵinject(i3.Router)); }, token: AuthGuard, providedIn: "root" });
    return AuthGuard;
}());
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
    AuthGuard.prototype.store;
    /**
     * @type {?}
     * @private
     */
    AuthGuard.prototype.router;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXV0aC5ndWFyZC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9ndWFyZHMvYXV0aC5ndWFyZC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQXFFLE1BQU0sRUFBRSxNQUFNLGlCQUFpQixDQUFDO0FBQzVHLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHFCQUFxQixDQUFDOzs7OztBQUluRDtJQUlFLG1CQUFvQixZQUEwQixFQUFVLEtBQVksRUFBVSxNQUFjO1FBQXhFLGlCQUFZLEdBQVosWUFBWSxDQUFjO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUFVLFdBQU0sR0FBTixNQUFNLENBQVE7SUFBRyxDQUFDOzs7Ozs7SUFFaEcsK0JBQVc7Ozs7O0lBQVgsVUFBWSxDQUF5QixFQUFFLEtBQTBCOztZQUN6RCxtQkFBbUIsR0FBRyxJQUFJLENBQUMsWUFBWSxDQUFDLG1CQUFtQixFQUFFO1FBQ25FLElBQUksbUJBQW1CLEVBQUU7WUFDdkIsT0FBTyxtQkFBbUIsQ0FBQztTQUM1QjtRQUVELE9BQU8sSUFBSSxDQUFDLE1BQU0sQ0FBQyxhQUFhLENBQUMsQ0FBQyxnQkFBZ0IsQ0FBQyxFQUFFLEVBQUUsS0FBSyxFQUFFLEVBQUUsV0FBVyxFQUFFLEtBQUssQ0FBQyxHQUFHLEVBQUUsRUFBRSxDQUFDLENBQUM7SUFDOUYsQ0FBQzs7Z0JBYkYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFOUSxZQUFZO2dCQURaLEtBQUs7Z0JBRDhELE1BQU07OztvQkFEbEY7Q0FxQkMsQUFkRCxJQWNDO1NBWFksU0FBUzs7Ozs7O0lBQ1IsaUNBQWtDOzs7OztJQUFFLDBCQUFvQjs7Ozs7SUFBRSwyQkFBc0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90LCBDYW5BY3RpdmF0ZSwgUm91dGVyU3RhdGVTbmFwc2hvdCwgVXJsVHJlZSwgUm91dGVyIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgT0F1dGhTZXJ2aWNlIH0gZnJvbSAnYW5ndWxhci1vYXV0aDItb2lkYyc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBOYXZpZ2F0ZSB9IGZyb20gJ0BuZ3hzL3JvdXRlci1wbHVnaW4nO1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290Jyxcbn0pXG5leHBvcnQgY2xhc3MgQXV0aEd1YXJkIGltcGxlbWVudHMgQ2FuQWN0aXZhdGUge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIG9hdXRoU2VydmljZTogT0F1dGhTZXJ2aWNlLCBwcml2YXRlIHN0b3JlOiBTdG9yZSwgcHJpdmF0ZSByb3V0ZXI6IFJvdXRlcikge31cblxuICBjYW5BY3RpdmF0ZShfOiBBY3RpdmF0ZWRSb3V0ZVNuYXBzaG90LCBzdGF0ZTogUm91dGVyU3RhdGVTbmFwc2hvdCk6IE9ic2VydmFibGU8Ym9vbGVhbj4gfCBib29sZWFuIHwgVXJsVHJlZSB7XG4gICAgY29uc3QgaGFzVmFsaWRBY2Nlc3NUb2tlbiA9IHRoaXMub2F1dGhTZXJ2aWNlLmhhc1ZhbGlkQWNjZXNzVG9rZW4oKTtcbiAgICBpZiAoaGFzVmFsaWRBY2Nlc3NUb2tlbikge1xuICAgICAgcmV0dXJuIGhhc1ZhbGlkQWNjZXNzVG9rZW47XG4gICAgfVxuXG4gICAgcmV0dXJuIHRoaXMucm91dGVyLmNyZWF0ZVVybFRyZWUoWycvYWNjb3VudC9sb2dpbiddLCB7IHN0YXRlOiB7IHJlZGlyZWN0VXJsOiBzdGF0ZS51cmwgfSB9KTtcbiAgfVxufVxuIl19