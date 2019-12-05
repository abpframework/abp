import { Injectable, ɵɵdefineInjectable, NgModule, APP_INITIALIZER } from '@angular/core';
import { addAbpRoutes, noop } from '@abp/ng.core';

/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/tenant-management-config.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var TenantManagementConfigService = /** @class */ (function () {
    function TenantManagementConfigService() {
        addAbpRoutes({
            name: 'AbpTenantManagement::Menu:TenantManagement',
            path: 'tenant-management',
            parentName: 'AbpUiNavigation::Menu:Administration',
            layout: "application" /* application */,
            iconClass: 'fa fa-users',
            children: [
                {
                    path: 'tenants',
                    name: 'AbpTenantManagement::Tenants',
                    order: 1,
                    requiredPolicy: 'AbpTenantManagement.Tenants',
                },
            ],
        });
    }
    TenantManagementConfigService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    TenantManagementConfigService.ctorParameters = function () { return []; };
    /** @nocollapse */ TenantManagementConfigService.ngInjectableDef = ɵɵdefineInjectable({ factory: function TenantManagementConfigService_Factory() { return new TenantManagementConfigService(); }, token: TenantManagementConfigService, providedIn: "root" });
    return TenantManagementConfigService;
}());

/**
 * @fileoverview added by tsickle
 * Generated from: lib/tenant-management-config.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var ɵ0 = noop;
var TenantManagementConfigModule = /** @class */ (function () {
    function TenantManagementConfigModule() {
    }
    TenantManagementConfigModule.decorators = [
        { type: NgModule, args: [{
                    providers: [{ provide: APP_INITIALIZER, deps: [TenantManagementConfigService], useFactory: ɵ0, multi: true }],
                },] }
    ];
    return TenantManagementConfigModule;
}());

/**
 * @fileoverview added by tsickle
 * Generated from: public-api.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: abp-ng.tenant-management.config.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

export { TenantManagementConfigModule, TenantManagementConfigService };
//# sourceMappingURL=abp-ng.tenant-management.config.js.map
