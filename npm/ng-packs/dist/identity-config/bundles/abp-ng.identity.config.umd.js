(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@abp/ng.core'), require('@angular/core'), require('@angular/router')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.identity.config', ['exports', '@abp/ng.core', '@angular/core', '@angular/router'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng.identity = global.abp.ng.identity || {}, global.abp.ng.identity.config = {}), global.ng_core, global.ng.core, global.ng.router));
}(this, (function (exports, ng_core, core, router) { 'use strict';
          order: 1,
          parentName: 'AbpUiNavigation::Menu:Administration',
          layout: 'application' /* application */,
          iconClass: 'fa fa-id-card-o',
          children: [
            { path: 'roles', name: 'AbpIdentity::Roles', order: 1, requiredPolicy: 'AbpIdentity.Roles' },
            { path: 'users', name: 'AbpIdentity::Users', order: 2, requiredPolicy: 'AbpIdentity.Users' },
          ],
        },
      ]);
    }
    IdentityConfigService.decorators = [
      {
        type: core.Injectable,
        args: [
          {
            providedIn: 'root',
          },
        ],
      },
    ];
    /** @nocollapse */
    IdentityConfigService.ctorParameters = function() {
      return [{ type: router.Router }, { type: ng_core.RestService }];
    };
    /** @nocollapse */ IdentityConfigService.ngInjectableDef = core.ɵɵdefineInjectable({
      factory: function IdentityConfigService_Factory() {
        return new IdentityConfigService(core.ɵɵinject(router.Router), core.ɵɵinject(ng_core.RestService));
      },
      token: IdentityConfigService,
      providedIn: 'root',
    });
    return IdentityConfigService;
  })();
  if (false) {
    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/identity-config.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var IdentityConfigService = /** @class */ (function () {
        function IdentityConfigService(router, restService) {
            this.router = router;
            this.restService = restService;
            ng_core.addAbpRoutes([
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
     * @fileoverview added by tsickle
     * Generated from: lib/services/identity-config.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var IdentityConfigService = /** @class */ (function () {
        function IdentityConfigService(router, restService) {
            this.router = router;
            this.restService = restService;
            ng_core.addAbpRoutes([
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
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        IdentityConfigService.ctorParameters = function () { return [
            { type: router.Router },
            { type: ng_core.RestService }
        ]; };
        /** @nocollapse */ IdentityConfigService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function IdentityConfigService_Factory() { return new IdentityConfigService(core.ɵɵinject(router.Router), core.ɵɵinject(ng_core.RestService)); }, token: IdentityConfigService, providedIn: "root" });
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
})));
