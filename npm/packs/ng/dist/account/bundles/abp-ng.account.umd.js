(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@abp/ng.core'), require('@angular/core'), require('@angular/router'), require('@angular/forms'), require('@ngxs/router-plugin'), require('@ngxs/store'), require('angular-oauth2-oidc'), require('rxjs'), require('@ngx-validate/core'), require('@abp/ng.theme.shared'), require('@ng-bootstrap/ng-bootstrap'), require('primeng/table')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.account', ['exports', '@abp/ng.core', '@angular/core', '@angular/router', '@angular/forms', '@ngxs/router-plugin', '@ngxs/store', 'angular-oauth2-oidc', 'rxjs', '@ngx-validate/core', '@abp/ng.theme.shared', '@ng-bootstrap/ng-bootstrap', 'primeng/table'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng.account = {}), global.ng_core, global.ng.core, global.ng.router, global.ng.forms, global.routerPlugin, global.store, global.angularOauth2Oidc, global.rxjs, global.core$1, global.ng_theme_shared, global.ngBootstrap, global.table));
}(this, function (exports, ng_core, core, router, forms, routerPlugin, store, angularOauth2Oidc, rxjs, core$1, ng_theme_shared, ngBootstrap, table) { 'use strict';

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
        function LoginComponent(fb, oauthService, store, options) {
            this.fb = fb;
            this.oauthService = oauthService;
            this.store = store;
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
            rxjs.from(this.oauthService.fetchTokenUsingPasswordFlow(this.form.get('username').value, this.form.get('password').value)).subscribe({
                next: (/**
                 * @return {?}
                 */
                function () {
                    /** @type {?} */
                    var redirectUrl = window.history.state.redirectUrl || _this.options.redirectUrl;
                    _this.store
                        .dispatch(new ng_core.ConfigGetAppConfiguration())
                        .subscribe((/**
                     * @return {?}
                     */
                    function () { return _this.store.dispatch(new routerPlugin.Navigate([redirectUrl || '/'])); }));
                }),
                error: (/**
                 * @return {?}
                 */
                function () { return console.error('an error occured'); }),
            });
        };
        LoginComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-login',
                        template: "<div class=\"row\">\n  <div class=\"col col-md-4 offset-md-4\">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class=\"abp-account-container\">\n      <h2>{{ 'AbpAccount::Login' | abpLocalization }}</h2>\n      <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n        <div class=\"form-group\">\n          <label for=\"login-input-user-name-or-email-address\">{{\n            'AbpAccount::UserNameOrEmailAddress' | abpLocalization\n          }}</label>\n          <input\n            class=\"form-control\"\n            type=\"text\"\n            id=\"login-input-user-name-or-email-address\"\n            formControlName=\"username\"\n          />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"login-input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label>\n          <input class=\"form-control\" type=\"password\" id=\"login-input-password\" formControlName=\"password\" />\n        </div>\n        <div class=\"form-check\" validationTarget validationStyle>\n          <label class=\"form-check-label\" for=\"login-input-remember-me\">\n            <input class=\"form-check-input\" type=\"checkbox\" id=\"login-input-remember-me\" formControlName=\"remember\" />\n            {{ 'AbpAccount::RememberMe' | abpLocalization }}\n          </label>\n        </div>\n        <div class=\"mt-2\">\n          <button type=\"button\" name=\"Action\" value=\"Cancel\" class=\"btn btn-secondary\">\n            {{ 'AbpAccount::Cancel' | abpLocalization }}\n          </button>\n          <button type=\"submit\" name=\"Action\" value=\"Login\" class=\"btn btn-primary ml-1\">\n            {{ 'AbpAccount::Login' | abpLocalization }}\n          </button>\n        </div>\n      </form>\n      <div style=\"padding-top: 20px\">\n        <a routerLink=\"/account/register\">{{ 'AbpAccount::Register' | abpLocalization }}</a>\n      </div>\n    </div>\n  </div>\n</div>\n"
                    }] }
        ];
        /** @nocollapse */
        LoginComponent.ctorParameters = function () { return [
            { type: forms.FormBuilder },
            { type: angularOauth2Oidc.OAuthService },
            { type: store.Store },
            { type: undefined, decorators: [{ type: core.Optional }, { type: core.Inject, args: ['ACCOUNT_OPTIONS',] }] }
        ]; };
        return LoginComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var maxLength$1 = forms.Validators.maxLength, minLength$1 = forms.Validators.minLength, required$1 = forms.Validators.required, email = forms.Validators.email;
    var RegisterComponent = /** @class */ (function () {
        function RegisterComponent(fb, oauthService, router) {
            this.fb = fb;
            this.oauthService = oauthService;
            this.router = router;
            this.form = this.fb.group({
                username: ['', [required$1, maxLength$1(255)]],
                password: [
                    '',
                    [required$1, maxLength$1(32), minLength$1(6), core$1.validatePassword(['small', 'capital', 'number', 'special'])],
                ],
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
            if (this.form.invalid)
                return;
        };
        RegisterComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-register',
                        template: "<div class=\"row\">\n  <div class=\"col col-md-4 offset-md-4\">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class=\"abp-account-container\">\n      <h2>Register</h2>\n      <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n        <div class=\"form-group\">\n          <label for=\"input-user-name\">User name</label><span> * </span\n          ><input autofocus type=\"text\" id=\"input-user-name\" class=\"form-control\" formControlName=\"username\" />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"input-email-address\">Email address</label><span> * </span\n          ><input type=\"email\" id=\"input-email-address\" class=\"form-control\" formControlName=\"email\" />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"input-password\">Password</label><span> * </span\n          ><input type=\"password\" id=\"input-password\" class=\"form-control\" formControlName=\"password\" />\n        </div>\n        <button type=\"submit\" class=\"btn btn-primary\">Register</button>\n      </form>\n    </div>\n  </div>\n</div>\n"
                    }] }
        ];
        /** @nocollapse */
        RegisterComponent.ctorParameters = function () { return [
            { type: forms.FormBuilder },
            { type: angularOauth2Oidc.OAuthService },
            { type: router.Router }
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
        function TenantBoxComponent(modalService, fb) {
            this.modalService = modalService;
            this.fb = fb;
        }
        /**
         * @return {?}
         */
        TenantBoxComponent.prototype.createForm = /**
         * @return {?}
         */
        function () {
            this.form = this.fb.group({
                name: [this.selected.name],
            });
        };
        /**
         * @return {?}
         */
        TenantBoxComponent.prototype.openModal = /**
         * @return {?}
         */
        function () {
            this.createForm();
            this.modalService.open(this.modalContent);
        };
        /**
         * @return {?}
         */
        TenantBoxComponent.prototype.onSwitch = /**
         * @return {?}
         */
        function () {
            this.selected = (/** @type {?} */ ({}));
            this.openModal();
        };
        /**
         * @return {?}
         */
        TenantBoxComponent.prototype.save = /**
         * @return {?}
         */
        function () {
            this.selected = this.form.value;
            this.modalService.dismissAll();
        };
        TenantBoxComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-tenant-box',
                        template: "<div\n  class=\"tenant-switch-box\"\n  style=\"background-color: #eee; margin-bottom: 20px; color: #000; padding: 10px; text-align: center;\"\n>\n  <span style=\"color: #666;\">{{ 'AbpUiMultiTenancy::Tenant' | abpLocalization }}: </span>\n  <strong>\n    <i>{{ selected?.name ? selected.name : ('AbpUiMultiTenancy::NotSelected' | abpLocalization) }}</i>\n  </strong>\n  (<a id=\"abp-tenant-switch-link\" style=\"color: #333; cursor: pointer\" (click)=\"onSwitch()\">{{\n    'AbpUiMultiTenancy::Switch' | abpLocalization\n  }}</a\n  >)\n</div>\n\n<ng-template #modalContent let-modal>\n  <div class=\"modal-header\">\n    <h5 class=\"modal-title\" id=\"modal-basic-title\">\n      SwitchTenant\n    </h5>\n    <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n      <span aria-hidden=\"true\">&times;</span>\n    </button>\n  </div>\n  <form [formGroup]=\"form\" (ngSubmit)=\"save()\">\n    <div class=\"modal-body\">\n      <div class=\"mt-2\">\n        <div class=\"form-group\">\n          <label for=\"name\">{{ 'AbpUiMultiTenancy::Name' | abpLocalization }}</label>\n          <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n        </div>\n        <p>{{ 'AbpUiMultiTenancy::SwitchTenantHint' | abpLocalization }}</p>\n      </div>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpTenantManagement::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </form>\n</ng-template>\n"
                    }] }
        ];
        /** @nocollapse */
        TenantBoxComponent.ctorParameters = function () { return [
            { type: ngBootstrap.NgbModal },
            { type: forms.FormBuilder }
        ]; };
        TenantBoxComponent.propDecorators = {
            modalContent: [{ type: core.ViewChild, args: ['modalContent', { static: false },] }]
        };
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
    exports.ɵd = TenantBoxComponent;
    exports.ɵe = AccountRoutingModule;
    exports.ɵf = optionsFactory;
    exports.ɵg = ACCOUNT_OPTIONS;

    Object.defineProperty(exports, '__esModule', { value: true });

}));
//# sourceMappingURL=abp-ng.account.umd.js.map
