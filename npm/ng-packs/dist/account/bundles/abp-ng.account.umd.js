(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@abp/ng.core'), require('@angular/core'), require('@angular/router'), require('@angular/forms'), require('@ngxs/router-plugin'), require('@ngxs/store'), require('angular-oauth2-oidc'), require('rxjs'), require('@abp/ng.theme.shared'), require('rxjs/operators'), require('snq'), require('primeng/table'), require('@ng-bootstrap/ng-bootstrap'), require('@ngx-validate/core')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.account', ['exports', '@abp/ng.core', '@angular/core', '@angular/router', '@angular/forms', '@ngxs/router-plugin', '@ngxs/store', 'angular-oauth2-oidc', 'rxjs', '@abp/ng.theme.shared', 'rxjs/operators', 'snq', 'primeng/table', '@ng-bootstrap/ng-bootstrap', '@ngx-validate/core'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng.account = {}), global.ng_core, global.ng.core, global.ng.router, global.ng.forms, global.routerPlugin, global.store, global.angularOauth2Oidc, global.rxjs, global.ng_theme_shared, global.rxjs.operators, global.snq, global.table, global.ngBootstrap, global.core$1));
}(this, function (exports, ng_core, core, router, forms, routerPlugin, store, angularOauth2Oidc, rxjs, ng_theme_shared, operators, snq, table, ngBootstrap, core$1) { 'use strict';

    snq = snq && snq.hasOwnProperty('default') ? snq['default'] : snq;

    /*! *****************************************************************************
    Copyright (c) Microsoft Corporation. All rights reserved.
    Licensed under the Apache License, Version 2.0 (the "License"); you may not use
    this file except in compliance with the License. You may obtain a copy of the
    License at http://www.apache.org/licenses/LICENSE-2.0

    THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
    KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
    WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
    MERCHANTABLITY OR NON-INFRINGEMENT.

    See the Apache Version 2.0 License for specific language governing permissions
    and limitations under the License.
    ***************************************************************************** */

    var __assign = function() {
        __assign = Object.assign || function __assign(t) {
            for (var s, i = 1, n = arguments.length; i < n; i++) {
                s = arguments[i];
                for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p)) t[p] = s[p];
            }
            return t;
        };
        return __assign.apply(this, arguments);
    };

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var maxLength = forms.Validators.maxLength, minLength = forms.Validators.minLength, required = forms.Validators.required;
    var LoginComponent = /** @class */ (function () {
        function LoginComponent(fb, oauthService, store, toasterService, options) {
            this.fb = fb;
            this.oauthService = oauthService;
            this.store = store;
            this.toasterService = toasterService;
            this.options = options;
            this.oauthService.configure(this.store.selectSnapshot(ng_core.ConfigState.getOne('environment')).oAuthConfig);
            this.oauthService.loadDiscoveryDocument();
            this.form = this.fb.group({
                username: ['', [required, maxLength(255)]],
                password: ['', [required, maxLength(32)]],
                remember: [false],
            });
        }
        /**
         * @return {?}
         */
        LoginComponent.prototype.onSubmit = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (this.form.invalid)
                return;
            this.oauthService.setStorage(this.form.value.remember ? localStorage : sessionStorage);
            this.inProgress = true;
            rxjs.from(this.oauthService.fetchTokenUsingPasswordFlow(this.form.get('username').value, this.form.get('password').value))
                .pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return _this.store.dispatch(new ng_core.GetAppConfiguration()); })), operators.tap((/**
             * @return {?}
             */
            function () {
                /** @type {?} */
                var redirectUrl = snq((/**
                 * @return {?}
                 */
                function () { return window.history.state; })).redirectUrl || (_this.options || {}).redirectUrl || '/';
                _this.store.dispatch(new routerPlugin.Navigate([redirectUrl]));
            })), operators.catchError((/**
             * @param {?} err
             * @return {?}
             */
            function (err) {
                _this.toasterService.error(snq((/**
                 * @return {?}
                 */
                function () { return err.error.error_description; })) ||
                    snq((/**
                     * @return {?}
                     */
                    function () { return err.error.error.message; }), 'AbpAccount::DefaultErrorMessage'), 'Error', { life: 7000 });
                return rxjs.throwError(err);
            })), operators.finalize((/**
             * @return {?}
             */
            function () { return (_this.inProgress = false); })))
                .subscribe();
        };
        LoginComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-login',
                        template: "<div class=\"row\">\n  <div class=\"col col-md-4 offset-md-4\">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class=\"abp-account-container\">\n      <h2>{{ 'AbpAccount::Login' | abpLocalization }}</h2>\n      <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n        <div class=\"form-group\">\n          <label for=\"login-input-user-name-or-email-address\">{{\n            'AbpAccount::UserNameOrEmailAddress' | abpLocalization\n          }}</label>\n          <input\n            class=\"form-control\"\n            type=\"text\"\n            id=\"login-input-user-name-or-email-address\"\n            formControlName=\"username\"\n            autofocus\n          />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"login-input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label>\n          <input class=\"form-control\" type=\"password\" id=\"login-input-password\" formControlName=\"password\" />\n        </div>\n        <div class=\"form-check\" validationTarget validationStyle>\n          <label class=\"form-check-label\" for=\"login-input-remember-me\">\n            <input class=\"form-check-input\" type=\"checkbox\" id=\"login-input-remember-me\" formControlName=\"remember\" />\n            {{ 'AbpAccount::RememberMe' | abpLocalization }}\n          </label>\n        </div>\n        <div class=\"mt-2\">\n          <button [disabled]=\"inProgress\" type=\"submit\" name=\"Action\" value=\"Login\" class=\"btn btn-primary ml-1\">\n            {{ 'AbpAccount::Login' | abpLocalization }}\n          </button>\n        </div>\n      </form>\n      <div style=\"padding-top: 20px\">\n        <a routerLink=\"/account/register\">{{ 'AbpAccount::Register' | abpLocalization }}</a>\n      </div>\n    </div>\n  </div>\n</div>\n"
                    }] }
        ];
        /** @nocollapse */
        LoginComponent.ctorParameters = function () { return [
            { type: forms.FormBuilder },
            { type: angularOauth2Oidc.OAuthService },
            { type: store.Store },
            { type: ng_theme_shared.ToasterService },
            { type: undefined, decorators: [{ type: core.Optional }, { type: core.Inject, args: ['ACCOUNT_OPTIONS',] }] }
        ]; };
        return LoginComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var AccountService = /** @class */ (function () {
        function AccountService(rest) {
            this.rest = rest;
        }
        /**
         * @param {?} tenantName
         * @return {?}
         */
        AccountService.prototype.findTenant = /**
         * @param {?} tenantName
         * @return {?}
         */
        function (tenantName) {
            /** @type {?} */
            var request = {
                method: 'GET',
                url: "/api/abp/multi-tenancy/find-tenant/" + tenantName,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} body
         * @return {?}
         */
        AccountService.prototype.register = /**
         * @param {?} body
         * @return {?}
         */
        function (body) {
            /** @type {?} */
            var request = {
                method: 'POST',
                url: "/api/account/register",
                body: body,
            };
            return this.rest.request(request, { throwErr: true });
        };
        AccountService.decorators = [
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        AccountService.ctorParameters = function () { return [
            { type: ng_core.RestService }
        ]; };
        /** @nocollapse */ AccountService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function AccountService_Factory() { return new AccountService(core.ɵɵinject(ng_core.RestService)); }, token: AccountService, providedIn: "root" });
        return AccountService;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var maxLength$1 = forms.Validators.maxLength, minLength$1 = forms.Validators.minLength, required$1 = forms.Validators.required, email = forms.Validators.email;
    var RegisterComponent = /** @class */ (function () {
        function RegisterComponent(fb, accountService, toasterService) {
            this.fb = fb;
            this.accountService = accountService;
            this.toasterService = toasterService;
            this.form = this.fb.group({
                username: ['', [required$1, maxLength$1(255)]],
                password: ['', [required$1, maxLength$1(32)]],
                email: ['', [required$1, email]],
            });
        }
        /**
         * @return {?}
         */
        RegisterComponent.prototype.onSubmit = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (this.form.invalid)
                return;
            this.inProgress = true;
            /** @type {?} */
            var newUser = (/** @type {?} */ ({
                userName: this.form.get('username').value,
                password: this.form.get('password').value,
                emailAddress: this.form.get('email').value,
                appName: 'angular',
            }));
            this.accountService
                .register(newUser)
                .pipe(operators.take(1), operators.catchError((/**
             * @param {?} err
             * @return {?}
             */
            function (err) {
                _this.toasterService.error(snq((/**
                 * @return {?}
                 */
                function () { return err.error.error_description; })) ||
                    snq((/**
                     * @return {?}
                     */
                    function () { return err.error.error.message; }), 'AbpAccount::DefaultErrorMessage'), 'Error', { life: 7000 });
                return rxjs.throwError(err);
            })), operators.finalize((/**
             * @return {?}
             */
            function () { return (_this.inProgress = false); })))
                .subscribe();
        };
        RegisterComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-register',
                        template: "<div class=\"row\">\n  <div class=\"col col-md-4 offset-md-4\">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class=\"abp-account-container\">\n      <h2>{{ 'AbpAccount::Register' | abpLocalization }}</h2>\n      <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n        <div class=\"form-group\">\n          <label for=\"input-user-name\">{{ 'AbpAccount::UserName' | abpLocalization }}</label\n          ><span> * </span\n          ><input autofocus type=\"text\" id=\"input-user-name\" class=\"form-control\" formControlName=\"username\" />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"input-email-address\">{{ 'AbpAccount::EmailAddress' | abpLocalization }}</label\n          ><span> * </span><input type=\"email\" id=\"input-email-address\" class=\"form-control\" formControlName=\"email\" />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label\n          ><span> * </span><input type=\"password\" id=\"input-password\" class=\"form-control\" formControlName=\"password\" />\n        </div>\n        <button [disabled]=\"inProgress\" type=\"submit\" name=\"Action\" value=\"Register\" class=\"btn btn-primary\">\n          {{ 'AbpAccount::Register' | abpLocalization }}\n        </button>\n      </form>\n      <div style=\"padding-top: 20px\">\n        <a routerLink=\"/account/login\">{{ 'AbpAccount::Login' | abpLocalization }}</a>\n      </div>\n    </div>\n  </div>\n</div>\n"
                    }] }
        ];
        /** @nocollapse */
        RegisterComponent.ctorParameters = function () { return [
            { type: forms.FormBuilder },
            { type: AccountService },
            { type: ng_theme_shared.ToasterService }
        ]; };
        return RegisterComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /** @type {?} */
    var routes = [
        { path: '', pathMatch: 'full', redirectTo: 'login' },
        {
            path: '',
            component: ng_core.DynamicLayoutComponent,
            children: [{ path: 'login', component: LoginComponent }, { path: 'register', component: RegisterComponent }],
        },
    ];
    var AccountRoutingModule = /** @class */ (function () {
        function AccountRoutingModule() {
        }
        AccountRoutingModule.decorators = [
            { type: core.NgModule, args: [{
                        imports: [router.RouterModule.forChild(routes)],
                        exports: [router.RouterModule],
                    },] }
        ];
        return AccountRoutingModule;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var TenantBoxComponent = /** @class */ (function () {
        function TenantBoxComponent(store, toasterService, accountService) {
            this.store = store;
            this.toasterService = toasterService;
            this.accountService = accountService;
            this.tenant = (/** @type {?} */ ({}));
        }
        /**
         * @return {?}
         */
        TenantBoxComponent.prototype.ngOnInit = /**
         * @return {?}
         */
        function () {
            this.tenant = this.store.selectSnapshot(ng_core.SessionState.getTenant) || ((/** @type {?} */ ({})));
            this.tenantName = this.tenant.name || '';
        };
        /**
         * @return {?}
         */
        TenantBoxComponent.prototype.onSwitch = /**
         * @return {?}
         */
        function () {
            this.isModalVisible = true;
        };
        /**
         * @return {?}
         */
        TenantBoxComponent.prototype.save = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (this.tenant.name) {
                this.accountService
                    .findTenant(this.tenant.name)
                    .pipe(operators.take(1), operators.catchError((/**
                 * @param {?} err
                 * @return {?}
                 */
                function (err) {
                    _this.toasterService.error(snq((/**
                     * @return {?}
                     */
                    function () { return err.error.error_description; }), 'AbpUi::DefaultErrorMessage'), 'AbpUi::Error');
                    return rxjs.throwError(err);
                })))
                    .subscribe((/**
                 * @param {?} __0
                 * @return {?}
                 */
                function (_a) {
                    var success = _a.success, tenantId = _a.tenantId;
                    if (success) {
                        _this.tenant = {
                            id: tenantId,
                            name: _this.tenant.name,
                        };
                        _this.tenantName = _this.tenant.name;
                        _this.isModalVisible = false;
                    }
                    else {
                        _this.toasterService.error("AbpUiMultiTenancy::GivenTenantIsNotAvailable", 'AbpUi::Error', {
                            messageLocalizationParams: [_this.tenant.name],
                        });
                        _this.tenant = (/** @type {?} */ ({}));
                    }
                    _this.store.dispatch(new ng_core.SetTenant(success ? _this.tenant : null));
                }));
            }
            else {
                this.store.dispatch(new ng_core.SetTenant(null));
                this.tenantName = null;
                this.isModalVisible = false;
            }
        };
        TenantBoxComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-tenant-box',
                        template: "<div\n  class=\"tenant-switch-box\"\n  style=\"background-color: #eee; margin-bottom: 20px; color: #000; padding: 10px; text-align: center;\"\n>\n  <span style=\"color: #666;\">{{ 'AbpUiMultiTenancy::Tenant' | abpLocalization }}: </span>\n  <strong>\n    <i>{{ tenantName || ('AbpUiMultiTenancy::NotSelected' | abpLocalization) }}</i>\n  </strong>\n  (<a id=\"abp-tenant-switch-link\" style=\"color: #333; cursor: pointer\" (click)=\"onSwitch()\">{{\n    'AbpUiMultiTenancy::Switch' | abpLocalization\n  }}</a\n  >)\n</div>\n\n<abp-modal [(visible)]=\"isModalVisible\" size=\"md\">\n  <ng-template #abpHeader>\n    <h5>Switch Tenant</h5>\n  </ng-template>\n  <ng-template #abpBody>\n    <form (ngSubmit)=\"save()\">\n      <div class=\"mt-2\">\n        <div class=\"form-group\">\n          <label for=\"name\">{{ 'AbpUiMultiTenancy::Name' | abpLocalization }}</label>\n          <input [(ngModel)]=\"tenant.name\" type=\"text\" id=\"name\" name=\"tenant\" class=\"form-control\" autofocus />\n        </div>\n        <p>{{ 'AbpUiMultiTenancy::SwitchTenantHint' | abpLocalization }}</p>\n      </div>\n    </form>\n  </ng-template>\n  <ng-template #abpFooter>\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n    </button>\n    <button type=\"button\" class=\"btn btn-primary\" (click)=\"save()\">\n      <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpTenantManagement::Save' | abpLocalization }}</span>\n    </button>\n  </ng-template>\n</abp-modal>\n"
                    }] }
        ];
        /** @nocollapse */
        TenantBoxComponent.ctorParameters = function () { return [
            { type: store.Store },
            { type: ng_theme_shared.ToasterService },
            { type: AccountService }
        ]; };
        return TenantBoxComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @param {?} options
     * @return {?}
     */
    function optionsFactory(options) {
        return __assign({ redirectUrl: '/' }, options);
    }
    /** @type {?} */
    var ACCOUNT_OPTIONS = new core.InjectionToken('ACCOUNT_OPTIONS');

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var AccountModule = /** @class */ (function () {
        function AccountModule() {
        }
        /**
         * @param {?=} options
         * @return {?}
         */
        AccountModule.forRoot = /**
         * @param {?=} options
         * @return {?}
         */
        function (options) {
            if (options === void 0) { options = (/** @type {?} */ ({})); }
            return {
                ngModule: AccountModule,
                providers: [
                    { provide: ACCOUNT_OPTIONS, useValue: options },
                    {
                        provide: 'ACCOUNT_OPTIONS',
                        useFactory: optionsFactory,
                        deps: [ACCOUNT_OPTIONS],
                    },
                ],
            };
        };
        AccountModule.decorators = [
            { type: core.NgModule, args: [{
                        declarations: [LoginComponent, RegisterComponent, TenantBoxComponent],
                        imports: [ng_core.CoreModule, AccountRoutingModule, ng_theme_shared.ThemeSharedModule, table.TableModule, ngBootstrap.NgbDropdownModule, core$1.NgxValidateCoreModule],
                        exports: [],
                    },] }
        ];
        return AccountModule;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /** @type {?} */
    var ACCOUNT_ROUTES = (/** @type {?} */ ([
        {
            name: 'Account',
            path: 'account',
            invisible: true,
            layout: "application" /* application */,
            children: [{ path: 'login', name: 'Login', order: 1 }, { path: 'register', name: 'Register', order: 2 }],
        },
    ]));

    exports.ACCOUNT_OPTIONS = ACCOUNT_OPTIONS;
    exports.ACCOUNT_ROUTES = ACCOUNT_ROUTES;
    exports.AccountModule = AccountModule;
    exports.LoginComponent = LoginComponent;
    exports.RegisterComponent = RegisterComponent;
    exports.optionsFactory = optionsFactory;
    exports.ɵa = LoginComponent;
    exports.ɵc = RegisterComponent;
    exports.ɵd = AccountService;
    exports.ɵe = TenantBoxComponent;
    exports.ɵf = AccountRoutingModule;
    exports.ɵg = optionsFactory;
    exports.ɵh = ACCOUNT_OPTIONS;

    Object.defineProperty(exports, '__esModule', { value: true });

}));
//# sourceMappingURL=abp-ng.account.umd.js.map
