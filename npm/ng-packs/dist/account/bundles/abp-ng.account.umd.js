(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@abp/ng.core'), require('@abp/ng.theme.shared'), require('@angular/core'), require('@ng-bootstrap/ng-bootstrap'), require('@ngx-validate/core'), require('primeng/table'), require('@angular/router'), require('@angular/forms'), require('@ngxs/router-plugin'), require('@ngxs/store'), require('angular-oauth2-oidc'), require('rxjs'), require('rxjs/operators'), require('snq')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.account', ['exports', '@abp/ng.core', '@abp/ng.theme.shared', '@angular/core', '@ng-bootstrap/ng-bootstrap', '@ngx-validate/core', 'primeng/table', '@angular/router', '@angular/forms', '@ngxs/router-plugin', '@ngxs/store', 'angular-oauth2-oidc', 'rxjs', 'rxjs/operators', 'snq'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng.account = {}), global.ng_core, global.ng_theme_shared, global.ng.core, global.ngBootstrap, global.core$1, global.table, global.ng.router, global.ng.forms, global.routerPlugin, global.store, global.angularOauth2Oidc, global.rxjs, global.rxjs.operators, global.snq));
}(this, function (exports, ng_core, ng_theme_shared, core, ngBootstrap, core$1, table, router, forms, routerPlugin, store, angularOauth2Oidc, rxjs, operators, snq) { 'use strict';

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
    /* global Reflect, Promise */

    var extendStatics = function(d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };

    function __extends(d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    }

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

    function __rest(s, e) {
        var t = {};
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
            t[p] = s[p];
        if (s != null && typeof Object.getOwnPropertySymbols === "function")
            for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) {
                if (e.indexOf(p[i]) < 0 && Object.prototype.propertyIsEnumerable.call(s, p[i]))
                    t[p[i]] = s[p[i]];
            }
        return t;
    }

    function __decorate(decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    }

    function __param(paramIndex, decorator) {
        return function (target, key) { decorator(target, key, paramIndex); }
    }

    function __metadata(metadataKey, metadataValue) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(metadataKey, metadataValue);
    }

    function __awaiter(thisArg, _arguments, P, generator) {
        return new (P || (P = Promise))(function (resolve, reject) {
            function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
            function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
            function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
            step((generator = generator.apply(thisArg, _arguments || [])).next());
        });
    }

    function __generator(thisArg, body) {
        var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
        return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
        function verb(n) { return function (v) { return step([n, v]); }; }
        function step(op) {
            if (f) throw new TypeError("Generator is already executing.");
            while (_) try {
                if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
                if (y = 0, t) op = [op[0] & 2, t.value];
                switch (op[0]) {
                    case 0: case 1: t = op; break;
                    case 4: _.label++; return { value: op[1], done: false };
                    case 5: _.label++; y = op[1]; op = [0]; continue;
                    case 7: op = _.ops.pop(); _.trys.pop(); continue;
                    default:
                        if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                        if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                        if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                        if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                        if (t[2]) _.ops.pop();
                        _.trys.pop(); continue;
                }
                op = body.call(thisArg, _);
            } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
            if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
        }
    }

    function __exportStar(m, exports) {
        for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
    }

    function __values(o) {
        var m = typeof Symbol === "function" && o[Symbol.iterator], i = 0;
        if (m) return m.call(o);
        return {
            next: function () {
                if (o && i >= o.length) o = void 0;
                return { value: o && o[i++], done: !o };
            }
        };
    }

    function __read(o, n) {
        var m = typeof Symbol === "function" && o[Symbol.iterator];
        if (!m) return o;
        var i = m.call(o), r, ar = [], e;
        try {
            while ((n === void 0 || n-- > 0) && !(r = i.next()).done) ar.push(r.value);
        }
        catch (error) { e = { error: error }; }
        finally {
            try {
                if (r && !r.done && (m = i["return"])) m.call(i);
            }
            finally { if (e) throw e.error; }
        }
        return ar;
    }

    function __spread() {
        for (var ar = [], i = 0; i < arguments.length; i++)
            ar = ar.concat(__read(arguments[i]));
        return ar;
    }

    function __spreadArrays() {
        for (var s = 0, i = 0, il = arguments.length; i < il; i++) s += arguments[i].length;
        for (var r = Array(s), k = 0, i = 0; i < il; i++)
            for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++)
                r[k] = a[j];
        return r;
    };

    function __await(v) {
        return this instanceof __await ? (this.v = v, this) : new __await(v);
    }

    function __asyncGenerator(thisArg, _arguments, generator) {
        if (!Symbol.asyncIterator) throw new TypeError("Symbol.asyncIterator is not defined.");
        var g = generator.apply(thisArg, _arguments || []), i, q = [];
        return i = {}, verb("next"), verb("throw"), verb("return"), i[Symbol.asyncIterator] = function () { return this; }, i;
        function verb(n) { if (g[n]) i[n] = function (v) { return new Promise(function (a, b) { q.push([n, v, a, b]) > 1 || resume(n, v); }); }; }
        function resume(n, v) { try { step(g[n](v)); } catch (e) { settle(q[0][3], e); } }
        function step(r) { r.value instanceof __await ? Promise.resolve(r.value.v).then(fulfill, reject) : settle(q[0][2], r); }
        function fulfill(value) { resume("next", value); }
        function reject(value) { resume("throw", value); }
        function settle(f, v) { if (f(v), q.shift(), q.length) resume(q[0][0], q[0][1]); }
    }

    function __asyncDelegator(o) {
        var i, p;
        return i = {}, verb("next"), verb("throw", function (e) { throw e; }), verb("return"), i[Symbol.iterator] = function () { return this; }, i;
        function verb(n, f) { i[n] = o[n] ? function (v) { return (p = !p) ? { value: __await(o[n](v)), done: n === "return" } : f ? f(v) : v; } : f; }
    }

    function __asyncValues(o) {
        if (!Symbol.asyncIterator) throw new TypeError("Symbol.asyncIterator is not defined.");
        var m = o[Symbol.asyncIterator], i;
        return m ? m.call(o) : (o = typeof __values === "function" ? __values(o) : o[Symbol.iterator](), i = {}, verb("next"), verb("throw"), verb("return"), i[Symbol.asyncIterator] = function () { return this; }, i);
        function verb(n) { i[n] = o[n] && function (v) { return new Promise(function (resolve, reject) { v = o[n](v), settle(resolve, reject, v.done, v.value); }); }; }
        function settle(resolve, reject, d, v) { Promise.resolve(v).then(function(v) { resolve({ value: v, done: d }); }, reject); }
    }

    function __makeTemplateObject(cooked, raw) {
        if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
        return cooked;
    };

    function __importStar(mod) {
        if (mod && mod.__esModule) return mod;
        var result = {};
        if (mod != null) for (var k in mod) if (Object.hasOwnProperty.call(mod, k)) result[k] = mod[k];
        result.default = mod;
        return result;
    }

    function __importDefault(mod) {
        return (mod && mod.__esModule) ? mod : { default: mod };
    }

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
            // this.oauthService.setStorage(this.form.value.remember ? localStorage : sessionStorage);
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
                        template: "<div class=\"row\">\n  <div class=\"col col-md-4 offset-md-4\">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class=\"abp-account-container\">\n      <h2>{{ 'AbpAccount::Login' | abpLocalization }}</h2>\n      <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n        <div class=\"form-group\">\n          <label for=\"login-input-user-name-or-email-address\">{{\n            'AbpAccount::UserNameOrEmailAddress' | abpLocalization\n          }}</label>\n          <input\n            class=\"form-control\"\n            type=\"text\"\n            id=\"login-input-user-name-or-email-address\"\n            formControlName=\"username\"\n            autofocus\n          />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"login-input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label>\n          <input class=\"form-control\" type=\"password\" id=\"login-input-password\" formControlName=\"password\" />\n        </div>\n        <div class=\"form-check\" validationTarget validationStyle>\n          <label class=\"form-check-label\" for=\"login-input-remember-me\">\n            <input class=\"form-check-input\" type=\"checkbox\" id=\"login-input-remember-me\" formControlName=\"remember\" />\n            {{ 'AbpAccount::RememberMe' | abpLocalization }}\n          </label>\n        </div>\n        <div class=\"mt-2\">\n          <abp-button [loading]=\"inProgress\" type=\"submit\">\n            {{ 'AbpAccount::Login' | abpLocalization }}\n          </abp-button>\n        </div>\n      </form>\n      <div style=\"padding-top: 20px\">\n        <a routerLink=\"/account/register\">{{ 'AbpAccount::Register' | abpLocalization }}</a>\n      </div>\n    </div>\n  </div>\n</div>\n"
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
    if (false) {
        /** @type {?} */
        LoginComponent.prototype.form;
        /** @type {?} */
        LoginComponent.prototype.inProgress;
        /**
         * @type {?}
         * @private
         */
        LoginComponent.prototype.fb;
        /**
         * @type {?}
         * @private
         */
        LoginComponent.prototype.oauthService;
        /**
         * @type {?}
         * @private
         */
        LoginComponent.prototype.store;
        /**
         * @type {?}
         * @private
         */
        LoginComponent.prototype.toasterService;
        /**
         * @type {?}
         * @private
         */
        LoginComponent.prototype.options;
    }

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
                url: "/api/abp/multi-tenancy/tenants/by-name/" + tenantName,
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
            return this.rest.request(request, { skipHandleError: true });
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
    if (false) {
        /**
         * @type {?}
         * @private
         */
        AccountService.prototype.rest;
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var maxLength$1 = forms.Validators.maxLength, minLength$1 = forms.Validators.minLength, required$1 = forms.Validators.required, email = forms.Validators.email;
    var RegisterComponent = /** @class */ (function () {
        function RegisterComponent(fb, accountService, oauthService, store, toasterService) {
            this.fb = fb;
            this.accountService = accountService;
            this.oauthService = oauthService;
            this.store = store;
            this.toasterService = toasterService;
            this.oauthService.configure(this.store.selectSnapshot(ng_core.ConfigState.getOne('environment')).oAuthConfig);
            this.oauthService.loadDiscoveryDocument();
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
                appName: 'Angular',
            }));
            this.accountService
                .register(newUser)
                .pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return rxjs.from(_this.oauthService.fetchTokenUsingPasswordFlow(newUser.userName, newUser.password)); })), operators.switchMap((/**
             * @return {?}
             */
            function () { return _this.store.dispatch(new ng_core.GetAppConfiguration()); })), operators.tap((/**
             * @return {?}
             */
            function () { return _this.store.dispatch(new routerPlugin.Navigate(['/'])); })), operators.take(1), operators.catchError((/**
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
                        template: "<div class=\"row\">\n  <div class=\"col col-md-4 offset-md-4\">\n    <abp-tenant-box></abp-tenant-box>\n\n    <div class=\"abp-account-container\">\n      <h2>{{ 'AbpAccount::Register' | abpLocalization }}</h2>\n      <form [formGroup]=\"form\" (ngSubmit)=\"onSubmit()\" novalidate>\n        <div class=\"form-group\">\n          <label for=\"input-user-name\">{{ 'AbpAccount::UserName' | abpLocalization }}</label\n          ><span> * </span\n          ><input autofocus type=\"text\" id=\"input-user-name\" class=\"form-control\" formControlName=\"username\" />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"input-email-address\">{{ 'AbpAccount::EmailAddress' | abpLocalization }}</label\n          ><span> * </span><input type=\"email\" id=\"input-email-address\" class=\"form-control\" formControlName=\"email\" />\n        </div>\n        <div class=\"form-group\">\n          <label for=\"input-password\">{{ 'AbpAccount::Password' | abpLocalization }}</label\n          ><span> * </span><input type=\"password\" id=\"input-password\" class=\"form-control\" formControlName=\"password\" />\n        </div>\n        <abp-button [loading]=\"inProgress\" type=\"submit\">\n          {{ 'AbpAccount::Register' | abpLocalization }}\n        </abp-button>\n      </form>\n      <div style=\"padding-top: 20px\">\n        <a routerLink=\"/account/login\">{{ 'AbpAccount::Login' | abpLocalization }}</a>\n      </div>\n    </div>\n  </div>\n</div>\n"
                    }] }
        ];
        /** @nocollapse */
        RegisterComponent.ctorParameters = function () { return [
            { type: forms.FormBuilder },
            { type: AccountService },
            { type: angularOauth2Oidc.OAuthService },
            { type: store.Store },
            { type: ng_theme_shared.ToasterService }
        ]; };
        return RegisterComponent;
    }());
    if (false) {
        /** @type {?} */
        RegisterComponent.prototype.form;
        /** @type {?} */
        RegisterComponent.prototype.inProgress;
        /**
         * @type {?}
         * @private
         */
        RegisterComponent.prototype.fb;
        /**
         * @type {?}
         * @private
         */
        RegisterComponent.prototype.accountService;
        /**
         * @type {?}
         * @private
         */
        RegisterComponent.prototype.oauthService;
        /**
         * @type {?}
         * @private
         */
        RegisterComponent.prototype.store;
        /**
         * @type {?}
         * @private
         */
        RegisterComponent.prototype.toasterService;
    }

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
    if (false) {
        /** @type {?} */
        TenantBoxComponent.prototype.tenant;
        /** @type {?} */
        TenantBoxComponent.prototype.tenantName;
        /** @type {?} */
        TenantBoxComponent.prototype.isModalVisible;
        /**
         * @type {?}
         * @private
         */
        TenantBoxComponent.prototype.store;
        /**
         * @type {?}
         * @private
         */
        TenantBoxComponent.prototype.toasterService;
        /**
         * @type {?}
         * @private
         */
        TenantBoxComponent.prototype.accountService;
    }

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
     * @param {?=} options
     * @return {?}
     */
    function AccountProviders(options) {
        if (options === void 0) { options = (/** @type {?} */ ({})); }
        return [
            { provide: ACCOUNT_OPTIONS, useValue: options },
            {
                provide: 'ACCOUNT_OPTIONS',
                useFactory: optionsFactory,
                deps: [ACCOUNT_OPTIONS],
            },
        ];
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /** @type {?} */
    var ACCOUNT_ROUTES = {
        routes: (/** @type {?} */ ([
            {
                name: 'Account',
                path: 'account',
                invisible: true,
                layout: "application" /* application */,
                children: [{ path: 'login', name: 'Login', order: 1 }, { path: 'register', name: 'Register', order: 2 }],
            },
        ])),
    };

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @record
     */
    function Options() { }
    if (false) {
        /** @type {?|undefined} */
        Options.prototype.redirectUrl;
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @record
     */
    function RegisterRequest() { }
    if (false) {
        /** @type {?} */
        RegisterRequest.prototype.userName;
        /** @type {?} */
        RegisterRequest.prototype.emailAddress;
        /** @type {?} */
        RegisterRequest.prototype.password;
        /** @type {?|undefined} */
        RegisterRequest.prototype.appName;
    }
    /**
     * @record
     */
    function RegisterResponse() { }
    if (false) {
        /** @type {?} */
        RegisterResponse.prototype.tenantId;
        /** @type {?} */
        RegisterResponse.prototype.userName;
        /** @type {?} */
        RegisterResponse.prototype.name;
        /** @type {?} */
        RegisterResponse.prototype.surname;
        /** @type {?} */
        RegisterResponse.prototype.email;
        /** @type {?} */
        RegisterResponse.prototype.emailConfirmed;
        /** @type {?} */
        RegisterResponse.prototype.phoneNumber;
        /** @type {?} */
        RegisterResponse.prototype.phoneNumberConfirmed;
        /** @type {?} */
        RegisterResponse.prototype.twoFactorEnabled;
        /** @type {?} */
        RegisterResponse.prototype.lockoutEnabled;
        /** @type {?} */
        RegisterResponse.prototype.lockoutEnd;
        /** @type {?} */
        RegisterResponse.prototype.concurrencyStamp;
        /** @type {?} */
        RegisterResponse.prototype.isDeleted;
        /** @type {?} */
        RegisterResponse.prototype.deleterId;
        /** @type {?} */
        RegisterResponse.prototype.deletionTime;
        /** @type {?} */
        RegisterResponse.prototype.lastModificationTime;
        /** @type {?} */
        RegisterResponse.prototype.lastModifierId;
        /** @type {?} */
        RegisterResponse.prototype.creationTime;
        /** @type {?} */
        RegisterResponse.prototype.creatorId;
        /** @type {?} */
        RegisterResponse.prototype.id;
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @record
     */
    function TenantIdResponse() { }
    if (false) {
        /** @type {?} */
        TenantIdResponse.prototype.success;
        /** @type {?} */
        TenantIdResponse.prototype.tenantId;
    }

    exports.ACCOUNT_OPTIONS = ACCOUNT_OPTIONS;
    exports.ACCOUNT_ROUTES = ACCOUNT_ROUTES;
    exports.AccountModule = AccountModule;
    exports.AccountProviders = AccountProviders;
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
