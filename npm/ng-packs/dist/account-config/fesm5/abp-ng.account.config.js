import { addAbpRoutes, RestService, noop, CoreModule } from '@abp/ng.core';
import { Injectable, ɵɵdefineInjectable, ɵɵinject, InjectionToken, NgModule, APP_INITIALIZER } from '@angular/core';
import { Router } from '@angular/router';
import { __assign } from 'tslib';
import { ThemeSharedModule } from '@abp/ng.theme.shared';

/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/account-config.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
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
    /** @nocollapse */ AccountConfigService.ngInjectableDef = ɵɵdefineInjectable({ factory: function AccountConfigService_Factory() { return new AccountConfigService(ɵɵinject(Router), ɵɵinject(RestService)); }, token: AccountConfigService, providedIn: "root" });
    return AccountConfigService;
}());
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

/**
 * @fileoverview added by tsickle
 * Generated from: lib/account-config.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/**
 * @record
 */
function AccountConfigOptions() { }
if (false) {
    /** @type {?|undefined} */
    AccountConfigOptions.prototype.redirectUrl;
}
/**
 * @param {?} options
 * @return {?}
 */
function accountOptionsFactory(options) {
    return __assign({ redirectUrl: '/' }, options);
}
/** @type {?} */
var ACCOUNT_OPTIONS = new InjectionToken('ACCOUNT_OPTIONS');
var ɵ0 = noop;
var AccountConfigModule = /** @class */ (function () {
    function AccountConfigModule() {
    }
    /**
     * @param {?=} options
     * @return {?}
     */
    AccountConfigModule.forRoot = /**
     * @param {?=} options
     * @return {?}
     */
    function (options) {
        if (options === void 0) { options = (/** @type {?} */ ({})); }
        return {
            ngModule: AccountConfigModule,
            providers: [
                { provide: ACCOUNT_OPTIONS, useValue: options },
                {
                    provide: 'ACCOUNT_OPTIONS',
                    useFactory: accountOptionsFactory,
                    deps: [ACCOUNT_OPTIONS],
                },
            ],
        };
    };
    AccountConfigModule.decorators = [
        { type: NgModule, args: [{
                    imports: [CoreModule, ThemeSharedModule],
                    providers: [{ provide: APP_INITIALIZER, multi: true, deps: [AccountConfigService], useFactory: ɵ0 }],
                },] }
    ];
    return AccountConfigModule;
}());

/**
 * @fileoverview added by tsickle
 * Generated from: public-api.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: abp-ng.account.config.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

export { ACCOUNT_OPTIONS, AccountConfigModule, AccountConfigService, accountOptionsFactory };
//# sourceMappingURL=abp-ng.account.config.js.map
