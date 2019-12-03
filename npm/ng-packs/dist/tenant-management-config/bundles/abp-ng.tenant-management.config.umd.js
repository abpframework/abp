(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@angular/core'), require('@abp/ng.core')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.tenant-management.config', ['exports', '@angular/core', '@abp/ng.core'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng['tenant-management'] = global.abp.ng['tenant-management'] || {}, global.abp.ng['tenant-management'].config = {}), global.ng.core, global.ng_core));
}(this, (function (exports, core, ng_core) { 'use strict';

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/tenant-management-config.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var TenantManagementConfigService = /** @class */ (function () {
        function TenantManagementConfigService() {
            ng_core.addAbpRoutes({
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
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        TenantManagementConfigService.ctorParameters = function () { return []; };
        /** @nocollapse */ TenantManagementConfigService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function TenantManagementConfigService_Factory() { return new TenantManagementConfigService(); }, token: TenantManagementConfigService, providedIn: "root" });
        return TenantManagementConfigService;
    }());

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/tenant-management-config.module.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ɵ0 = ng_core.noop;
    var TenantManagementConfigModule = /** @class */ (function () {
        function TenantManagementConfigModule() {
        }
        TenantManagementConfigModule.decorators = [
            { type: core.NgModule, args: [{
                        providers: [{ provide: core.APP_INITIALIZER, deps: [TenantManagementConfigService], useFactory: ɵ0, multi: true }],
                    },] }
        ];
        return TenantManagementConfigModule;
    }());

    exports.TenantManagementConfigModule = TenantManagementConfigModule;
    exports.TenantManagementConfigService = TenantManagementConfigService;

    Object.defineProperty(exports, '__esModule', { value: true });

})));
//# sourceMappingURL=abp-ng.tenant-management.config.umd.js.map
