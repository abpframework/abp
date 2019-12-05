import { addAbpRoutes, RestService, noop } from '@abp/ng.core';
import { Injectable, ɵɵdefineInjectable, ɵɵinject, NgModule, APP_INITIALIZER } from '@angular/core';
import { Router } from '@angular/router';

/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/identity-config.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var IdentityConfigService = /** @class */ (function () {
    function IdentityConfigService(router, restService) {
        this.router = router;
        this.restService = restService;
        addAbpRoutes([
            {
                name: 'AbpUiNavigation::Menu:Administration',
                path: '',
                order: 1,
                wrapper: true,
                iconClass: 'fa fa-wrench',
            },
            {
                name: 'AbpIdentity::Menu:IdentityManagement',
                path: 'identity',
                order: 1,
                parentName: 'AbpUiNavigation::Menu:Administration',
                layout: "application" /* application */,
                iconClass: 'fa fa-id-card-o',
                children: [
                    { path: 'roles', name: 'AbpIdentity::Roles', order: 1, requiredPolicy: 'AbpIdentity.Roles' },
                    { path: 'users', name: 'AbpIdentity::Users', order: 2, requiredPolicy: 'AbpIdentity.Users' },
                ],
            },
        ]);
    }
    IdentityConfigService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    IdentityConfigService.ctorParameters = function () { return [
        { type: Router },
        { type: RestService }
    ]; };
    /** @nocollapse */ IdentityConfigService.ngInjectableDef = ɵɵdefineInjectable({ factory: function IdentityConfigService_Factory() { return new IdentityConfigService(ɵɵinject(Router), ɵɵinject(RestService)); }, token: IdentityConfigService, providedIn: "root" });
    return IdentityConfigService;
}());
if (false) {
    /**
     * @type {?}
     * @private
     */
    IdentityConfigService.prototype.router;
    /**
     * @type {?}
     * @private
     */
    IdentityConfigService.prototype.restService;
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/identity-config.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var ɵ0 = noop;
var IdentityConfigModule = /** @class */ (function () {
    function IdentityConfigModule() {
    }
    IdentityConfigModule.decorators = [
        { type: NgModule, args: [{
                    providers: [{ provide: APP_INITIALIZER, deps: [IdentityConfigService], useFactory: ɵ0, multi: true }],
                },] }
    ];
    return IdentityConfigModule;
}());

/**
 * @fileoverview added by tsickle
 * Generated from: public-api.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: abp-ng.identity.config.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

export { IdentityConfigModule, IdentityConfigService };
//# sourceMappingURL=abp-ng.identity.config.js.map
