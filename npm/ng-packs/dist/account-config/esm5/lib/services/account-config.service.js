/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/account-config.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { RestService, addAbpRoutes } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import * as i0 from "@angular/core";
import * as i1 from "@angular/router";
import * as i2 from "@abp/ng.core";
var AccountConfigService = /** @class */ (function () {
    function AccountConfigService(router, restService) {
        this.router = router;
        this.restService = restService;
        addAbpRoutes({
            name: 'AbpAccount::Menu:Account',
            path: 'account',
            invisible: true,
            layout: "application" /* application */,
            children: [
                { path: 'login', name: 'AbpAccount::Login', order: 1 },
                { path: 'register', name: 'AbpAccount::Register', order: 2 },
                { path: 'manage-profile', name: 'AbpAccount::ManageYourProfile', order: 3 },
            ],
        });
    }
    AccountConfigService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    AccountConfigService.ctorParameters = function () { return [
        { type: Router },
        { type: RestService }
    ]; };
    /** @nocollapse */ AccountConfigService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function AccountConfigService_Factory() { return new AccountConfigService(i0.ɵɵinject(i1.Router), i0.ɵɵinject(i2.RestService)); }, token: AccountConfigService, providedIn: "root" });
    return AccountConfigService;
}());
export { AccountConfigService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    AccountConfigService.prototype.router;
    /**
     * @type {?}
     * @private
     */
    AccountConfigService.prototype.restService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYWNjb3VudC1jb25maWcuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuYWNjb3VudC5jb25maWcvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvYWNjb3VudC1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBZSxXQUFXLEVBQUUsWUFBWSxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ3RFLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLE1BQU0sRUFBRSxNQUFNLGlCQUFpQixDQUFDOzs7O0FBRXpDO0lBSUUsOEJBQW9CLE1BQWMsRUFBVSxXQUF3QjtRQUFoRCxXQUFNLEdBQU4sTUFBTSxDQUFRO1FBQVUsZ0JBQVcsR0FBWCxXQUFXLENBQWE7UUFDbEUsWUFBWSxDQUFDO1lBQ1gsSUFBSSxFQUFFLDBCQUEwQjtZQUNoQyxJQUFJLEVBQUUsU0FBUztZQUNmLFNBQVMsRUFBRSxJQUFJO1lBQ2YsTUFBTSxpQ0FBeUI7WUFDL0IsUUFBUSxFQUFFO2dCQUNSLEVBQUUsSUFBSSxFQUFFLE9BQU8sRUFBRSxJQUFJLEVBQUUsbUJBQW1CLEVBQUUsS0FBSyxFQUFFLENBQUMsRUFBRTtnQkFDdEQsRUFBRSxJQUFJLEVBQUUsVUFBVSxFQUFFLElBQUksRUFBRSxzQkFBc0IsRUFBRSxLQUFLLEVBQUUsQ0FBQyxFQUFFO2dCQUM1RCxFQUFFLElBQUksRUFBRSxnQkFBZ0IsRUFBRSxJQUFJLEVBQUUsK0JBQStCLEVBQUUsS0FBSyxFQUFFLENBQUMsRUFBRTthQUM1RTtTQUNGLENBQUMsQ0FBQztJQUNMLENBQUM7O2dCQWhCRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQUpRLE1BQU07Z0JBRk8sV0FBVzs7OytCQUFqQztDQXFCQyxBQWpCRCxJQWlCQztTQWRZLG9CQUFvQjs7Ozs7O0lBQ25CLHNDQUFzQjs7Ozs7SUFBRSwyQ0FBZ0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBlTGF5b3V0VHlwZSwgUmVzdFNlcnZpY2UsIGFkZEFicFJvdXRlcyB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcbmltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgUm91dGVyIH0gZnJvbSAnQGFuZ3VsYXIvcm91dGVyJztcclxuXHJcbkBJbmplY3RhYmxlKHtcclxuICBwcm92aWRlZEluOiAncm9vdCcsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBBY2NvdW50Q29uZmlnU2VydmljZSB7XHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSByb3V0ZXI6IFJvdXRlciwgcHJpdmF0ZSByZXN0U2VydmljZTogUmVzdFNlcnZpY2UpIHtcclxuICAgIGFkZEFicFJvdXRlcyh7XHJcbiAgICAgIG5hbWU6ICdBYnBBY2NvdW50OjpNZW51OkFjY291bnQnLFxyXG4gICAgICBwYXRoOiAnYWNjb3VudCcsXHJcbiAgICAgIGludmlzaWJsZTogdHJ1ZSxcclxuICAgICAgbGF5b3V0OiBlTGF5b3V0VHlwZS5hcHBsaWNhdGlvbixcclxuICAgICAgY2hpbGRyZW46IFtcclxuICAgICAgICB7IHBhdGg6ICdsb2dpbicsIG5hbWU6ICdBYnBBY2NvdW50OjpMb2dpbicsIG9yZGVyOiAxIH0sXHJcbiAgICAgICAgeyBwYXRoOiAncmVnaXN0ZXInLCBuYW1lOiAnQWJwQWNjb3VudDo6UmVnaXN0ZXInLCBvcmRlcjogMiB9LFxyXG4gICAgICAgIHsgcGF0aDogJ21hbmFnZS1wcm9maWxlJywgbmFtZTogJ0FicEFjY291bnQ6Ok1hbmFnZVlvdXJQcm9maWxlJywgb3JkZXI6IDMgfSxcclxuICAgICAgXSxcclxuICAgIH0pO1xyXG4gIH1cclxufVxyXG4iXX0=