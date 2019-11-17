(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@angular/core'), require('@angular/router'), require('@ngxs/store'), require('rxjs'), require('snq'), require('rxjs/operators'), require('@angular/common/http'), require('@angular/common'), require('just-compare'), require('just-clone'), require('@angular/forms'), require('angular-oauth2-oidc'), require('@ngxs/router-plugin'), require('@ngxs/storage-plugin'), require('@ngx-validate/core')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.core', ['exports', '@angular/core', '@angular/router', '@ngxs/store', 'rxjs', 'snq', 'rxjs/operators', '@angular/common/http', '@angular/common', 'just-compare', 'just-clone', '@angular/forms', 'angular-oauth2-oidc', '@ngxs/router-plugin', '@ngxs/storage-plugin', '@ngx-validate/core'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng.core = {}), global.ng.core, global.ng.router, global.store, global.rxjs, global.snq, global.rxjs.operators, global.ng.common.http, global.ng.common, global.compare, global.clone, global.ng.forms, global.angularOauth2Oidc, global.routerPlugin, global.storagePlugin, global.core$1));
}(this, (function (exports, core, router, store, rxjs, snq, operators, http, common, compare, clone, forms, angularOauth2Oidc, routerPlugin, storagePlugin, core$1) { 'use strict';

    snq = snq && snq.hasOwnProperty('default') ? snq['default'] : snq;
    compare = compare && compare.hasOwnProperty('default') ? compare['default'] : compare;
    clone = clone && clone.hasOwnProperty('default') ? clone['default'] : clone;

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
     * Generated from: lib/abstracts/ng-model.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @template T
     */
    var AbstractNgModelComponent = /** @class */ (function () {
        function AbstractNgModelComponent(injector) {
            this.injector = injector;
            this.cdRef = injector.get((/** @type {?} */ (core.ChangeDetectorRef)));
        }
        Object.defineProperty(AbstractNgModelComponent.prototype, "value", {
            get: /**
             * @return {?}
             */
            function () {
                return this._value;
            },
            set: /**
             * @param {?} value
             * @return {?}
             */
            function (value) {
                this._value = value;
                this.notifyValueChange();
            },
            enumerable: true,
            configurable: true
        });
        /**
         * @return {?}
         */
        AbstractNgModelComponent.prototype.notifyValueChange = /**
         * @return {?}
         */
        function () {
            if (this.onChange) {
                this.onChange(this.value);
            }
        };
        /**
         * @param {?} value
         * @return {?}
         */
        AbstractNgModelComponent.prototype.writeValue = /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            var _this = this;
            this._value = value;
            setTimeout((/**
             * @return {?}
             */
            function () { return _this.cdRef.detectChanges(); }), 0);
        };
        /**
         * @param {?} fn
         * @return {?}
         */
        AbstractNgModelComponent.prototype.registerOnChange = /**
         * @param {?} fn
         * @return {?}
         */
        function (fn) {
            this.onChange = fn;
        };
        /**
         * @param {?} fn
         * @return {?}
         */
        AbstractNgModelComponent.prototype.registerOnTouched = /**
         * @param {?} fn
         * @return {?}
         */
        function (fn) {
            this.onTouched = fn;
        };
        /**
         * @param {?} isDisabled
         * @return {?}
         */
        AbstractNgModelComponent.prototype.setDisabledState = /**
         * @param {?} isDisabled
         * @return {?}
         */
        function (isDisabled) {
            this.disabled = isDisabled;
        };
        AbstractNgModelComponent.decorators = [
            { type: core.Component, args: [{ selector: 'abp-abstract-ng-model', template: '' }] }
        ];
        /** @nocollapse */
        AbstractNgModelComponent.ctorParameters = function () { return [
            { type: core.Injector }
        ]; };
        AbstractNgModelComponent.propDecorators = {
            disabled: [{ type: core.Input }],
            value: [{ type: core.Input }]
        };
        return AbstractNgModelComponent;
    }());
    if (false) {
        /** @type {?} */
        AbstractNgModelComponent.prototype.disabled;
        /** @type {?} */
        AbstractNgModelComponent.prototype.onChange;
        /** @type {?} */
        AbstractNgModelComponent.prototype.onTouched;
        /**
         * @type {?}
         * @protected
         */
        AbstractNgModelComponent.prototype._value;
        /**
         * @type {?}
         * @protected
         */
        AbstractNgModelComponent.prototype.cdRef;
        /** @type {?} */
        AbstractNgModelComponent.prototype.injector;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/abstracts/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/actions/config.actions.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var PatchRouteByName = /** @class */ (function () {
        function PatchRouteByName(name, newValue) {
            this.name = name;
            this.newValue = newValue;
        }
        PatchRouteByName.type = '[Config] Patch Route By Name';
        return PatchRouteByName;
    }());
    if (false) {
        /** @type {?} */
        PatchRouteByName.type;
        /** @type {?} */
        PatchRouteByName.prototype.name;
        /** @type {?} */
        PatchRouteByName.prototype.newValue;
    }
    var GetAppConfiguration = /** @class */ (function () {
        function GetAppConfiguration() {
        }
        GetAppConfiguration.type = '[Config] Get App Configuration';
        return GetAppConfiguration;
    }());
    if (false) {
        /** @type {?} */
        GetAppConfiguration.type;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/actions/loader.actions.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var StartLoader = /** @class */ (function () {
        function StartLoader(payload) {
            this.payload = payload;
        }
        StartLoader.type = '[Loader] Start';
        return StartLoader;
    }());
    if (false) {
        /** @type {?} */
        StartLoader.type;
        /** @type {?} */
        StartLoader.prototype.payload;
    }
    var StopLoader = /** @class */ (function () {
        function StopLoader(payload) {
            this.payload = payload;
        }
        StopLoader.type = '[Loader] Stop';
        return StopLoader;
    }());
    if (false) {
        /** @type {?} */
        StopLoader.type;
        /** @type {?} */
        StopLoader.prototype.payload;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/actions/profile.actions.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var GetProfile = /** @class */ (function () {
        function GetProfile() {
        }
        GetProfile.type = '[Profile] Get';
        return GetProfile;
    }());
    if (false) {
        /** @type {?} */
        GetProfile.type;
    }
    var UpdateProfile = /** @class */ (function () {
        function UpdateProfile(payload) {
            this.payload = payload;
        }
        UpdateProfile.type = '[Profile] Update';
        return UpdateProfile;
    }());
    if (false) {
        /** @type {?} */
        UpdateProfile.type;
        /** @type {?} */
        UpdateProfile.prototype.payload;
    }
    var ChangePassword = /** @class */ (function () {
        function ChangePassword(payload) {
            this.payload = payload;
        }
        ChangePassword.type = '[Profile] Change Password';
        return ChangePassword;
    }());
    if (false) {
        /** @type {?} */
        ChangePassword.type;
        /** @type {?} */
        ChangePassword.prototype.payload;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/actions/rest.actions.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var RestOccurError = /** @class */ (function () {
        function RestOccurError(payload) {
            this.payload = payload;
        }
        RestOccurError.type = '[Rest] Error';
        return RestOccurError;
    }());
    if (false) {
        /** @type {?} */
        RestOccurError.type;
        /** @type {?} */
        RestOccurError.prototype.payload;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/actions/session.actions.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var SetLanguage = /** @class */ (function () {
        function SetLanguage(payload) {
            this.payload = payload;
        }
        SetLanguage.type = '[Session] Set Language';
        return SetLanguage;
    }());
    if (false) {
        /** @type {?} */
        SetLanguage.type;
        /** @type {?} */
        SetLanguage.prototype.payload;
    }
    var SetTenant = /** @class */ (function () {
        function SetTenant(payload) {
            this.payload = payload;
        }
        SetTenant.type = '[Session] Set Tenant';
        return SetTenant;
    }());
    if (false) {
        /** @type {?} */
        SetTenant.type;
        /** @type {?} */
        SetTenant.prototype.payload;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/actions/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/rest.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var RestService = /** @class */ (function () {
        function RestService(http, store) {
            this.http = http;
            this.store = store;
        }
        /**
         * @param {?} err
         * @return {?}
         */
        RestService.prototype.handleError = /**
         * @param {?} err
         * @return {?}
         */
        function (err) {
            this.store.dispatch(new RestOccurError(err));
            console.error(err);
            return rxjs.throwError(err);
        };
        /**
         * @template T, R
         * @param {?} request
         * @param {?=} config
         * @param {?=} api
         * @return {?}
         */
        RestService.prototype.request = /**
         * @template T, R
         * @param {?} request
         * @param {?=} config
         * @param {?=} api
         * @return {?}
         */
        function (request, config, api) {
            var _this = this;
            config = config || ((/** @type {?} */ ({})));
            var _a = config.observe, observe = _a === void 0 ? "body" /* Body */ : _a, skipHandleError = config.skipHandleError;
            /** @type {?} */
            var url = (api || this.store.selectSnapshot(ConfigState.getApiUrl())) + request.url;
            var method = request.method, options = __rest(request, ["method"]);
            return this.http.request(method, url, (/** @type {?} */ (__assign({ observe: observe }, options)))).pipe(observe === "body" /* Body */ ? operators.take(1) : operators.tap(), operators.catchError((/**
             * @param {?} err
             * @return {?}
             */
            function (err) {
                if (skipHandleError) {
                    return rxjs.throwError(err);
                }
                return _this.handleError(err);
            })));
        };
        RestService.decorators = [
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        RestService.ctorParameters = function () { return [
            { type: http.HttpClient },
            { type: store.Store }
        ]; };
        /** @nocollapse */ RestService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function RestService_Factory() { return new RestService(core.ɵɵinject(http.HttpClient), core.ɵɵinject(store.Store)); }, token: RestService, providedIn: "root" });
        return RestService;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        RestService.prototype.http;
        /**
         * @type {?}
         * @private
         */
        RestService.prototype.store;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/application-configuration.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ApplicationConfigurationService = /** @class */ (function () {
        function ApplicationConfigurationService(rest) {
            this.rest = rest;
        }
        /**
         * @return {?}
         */
        ApplicationConfigurationService.prototype.getConfiguration = /**
         * @return {?}
         */
        function () {
            /** @type {?} */
            var request = {
                method: 'GET',
                url: '/api/abp/application-configuration',
            };
            return this.rest.request(request);
        };
        ApplicationConfigurationService.decorators = [
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        ApplicationConfigurationService.ctorParameters = function () { return [
            { type: RestService }
        ]; };
        /** @nocollapse */ ApplicationConfigurationService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function ApplicationConfigurationService_Factory() { return new ApplicationConfigurationService(core.ɵɵinject(RestService)); }, token: ApplicationConfigurationService, providedIn: "root" });
        return ApplicationConfigurationService;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        ApplicationConfigurationService.prototype.rest;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/utils/route-utils.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @param {?} routes
     * @param {?=} wrappers
     * @param {?=} parentNameArr
     * @param {?=} parentName
     * @return {?}
     */
    function organizeRoutes(routes, wrappers, parentNameArr, parentName) {
        if (wrappers === void 0) { wrappers = []; }
        if (parentNameArr === void 0) { parentNameArr = (/** @type {?} */ ([])); }
        if (parentName === void 0) { parentName = null; }
        /** @type {?} */
        var filter = (/**
         * @param {?} route
         * @return {?}
         */
        function (route) {
            if (route.children && route.children.length) {
                route.children = organizeRoutes(route.children, wrappers, parentNameArr, route.name);
            }
            if (route.parentName && route.parentName !== parentName) {
                parentNameArr.push(route);
                return false;
            }
            return true;
        });
        if (parentName) {
            // recursive block
            return routes.filter(filter);
        }
        /** @type {?} */
        var filteredRoutes = routes.filter(filter);
        if (parentNameArr.length) {
            return sortRoutes(setChildRoute(__spread(filteredRoutes, wrappers), parentNameArr));
        }
        return filteredRoutes;
    }
    /**
     * @param {?} routes
     * @param {?} parentNameArr
     * @return {?}
     */
    function setChildRoute(routes, parentNameArr) {
        return routes.map((/**
         * @param {?} route
         * @return {?}
         */
        function (route) {
            if (route.children && route.children.length) {
                route.children = setChildRoute(route.children, parentNameArr);
            }
            /** @type {?} */
            var foundedChildren = parentNameArr.filter((/**
             * @param {?} parent
             * @return {?}
             */
            function (parent) { return parent.parentName === route.name; }));
            if (foundedChildren && foundedChildren.length) {
                route.children = __spread((route.children || []), foundedChildren);
            }
            return route;
        }));
    }
    /**
     * @param {?=} routes
     * @return {?}
     */
    function sortRoutes(routes) {
        if (routes === void 0) { routes = []; }
        if (!routes.length)
            return [];
        return routes
            .map((/**
         * @param {?} route
         * @param {?} index
         * @return {?}
         */
        function (route, index) {
            return __assign({}, route, { order: typeof route.order === 'undefined' ? index + 1 : route.order });
        }))
            .sort((/**
         * @param {?} a
         * @param {?} b
         * @return {?}
         */
        function (a, b) { return a.order - b.order; }))
            .map((/**
         * @param {?} route
         * @return {?}
         */
        function (route) {
            if (route.children && route.children.length) {
                route.children = sortRoutes(route.children);
            }
            return route;
        }));
    }
    /** @type {?} */
    var ABP_ROUTES = (/** @type {?} */ ([]));
    /**
     * @param {?} routes
     * @return {?}
     */
    function addAbpRoutes(routes) {
        if (!Array.isArray(routes)) {
            routes = [routes];
        }
        ABP_ROUTES.push.apply(ABP_ROUTES, __spread(routes));
    }
    /**
     * @return {?}
     */
    function getAbpRoutes() {
        return ABP_ROUTES;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/constants/different-locales.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    // Different locales from .NET
    // Key is .NET locale, value is Angular locale
    var localesMapping = {
        'ar-sa': 'ar-SA',
        'ca-ES-valencia': 'ca-ES-VALENCIA',
        'de-de': 'de',
        'es-ES': 'es',
        'en-US': 'en',
        'fil-Latn': 'en',
        'ku-Arab': 'en',
        'ky-Cyrl': 'en',
        'mi-Latn': 'en',
        'prs-Arab': 'en',
        'qut-Latn': 'en',
        nso: 'en',
        quz: 'en',
        'fr-FR': 'fr',
        'gd-Latn': 'gd',
        'ha-Latn': 'ha',
        'ig-Latn': 'ig',
        'it-it': 'it',
        'mn-Cyrl': 'mn',
        'pt-BR': 'pt',
        'sd-Arab': 'pa-Arab',
        'sr-Cyrl-RS': 'sr-Cyrl',
        'sr-Latn-RS': 'sr-Latn',
        'tg-Cyrl': 'tg',
        'tk-Latn': 'tk',
        'tt-Cyrl': 'tt',
        'ug-Arab': 'ug',
        'yo-Latn': 'yo',
    };

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/utils/initial-utils.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @param {?} injector
     * @return {?}
     */
    function getInitialData(injector) {
        /** @type {?} */
        var fn = (/**
         * @return {?}
         */
        function () {
            /** @type {?} */
            var store$1 = injector.get(store.Store);
            return store$1.dispatch(new GetAppConfiguration()).toPromise();
        });
        return fn;
    }
    /**
     * @param {?} injector
     * @return {?}
     */
    function localeInitializer(injector) {
        /** @type {?} */
        var fn = (/**
         * @return {?}
         */
        function () {
            /** @type {?} */
            var store$1 = injector.get(store.Store);
            /** @type {?} */
            var lang = store$1.selectSnapshot((/**
             * @param {?} state
             * @return {?}
             */
            function (state) { return state.SessionState.language; })) || 'en';
            return new Promise((/**
             * @param {?} resolve
             * @param {?} reject
             * @return {?}
             */
            function (resolve, reject) {
                registerLocale(lang).then((/**
                 * @return {?}
                 */
                function () { return resolve('resolved'); }), reject);
            }));
        });
        return fn;
    }
    /**
     * @param {?} locale
     * @return {?}
     */
    function registerLocale(locale) {
        return import(
        /* webpackInclude: /(af|am|ar-SA|as|az-Latn|be|bg|bn-BD|bn-IN|bs|ca|ca-ES-VALENCIA|cs|cy|da|de|de|el|en-GB|en|es|en|es-US|es-MX|et|eu|fa|fi|en|fr|fr|fr-CA|ga|gd|gl|gu|ha|he|hi|hr|hu|hy|id|ig|is|it|it|ja|ka|kk|km|kn|ko|kok|en|en|lb|lt|lv|en|mk|ml|mn|mr|ms|mt|nb|ne|nl|nl-BE|nn|en|or|pa|pa-Arab|pl|en|pt|pt-PT|en|en|ro|ru|rw|pa-Arab|si|sk|sl|sq|sr-Cyrl-BA|sr-Cyrl|sr-Latn|sv|sw|ta|te|tg|th|ti|tk|tn|tr|tt|ug|uk|ur|uz-Latn|vi|wo|xh|yo|zh-Hans|zh-Hant|zu)\.js$/ */
        "@angular/common/locales/" + (localesMapping[locale] || locale) + ".js").then((/**
         * @param {?} module
         * @return {?}
         */
        function (module) {
            common.registerLocaleData(module.default);
        }));
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/localization.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var LocalizationService = /** @class */ (function () {
        function LocalizationService(store, router, ngZone, otherInstance) {
            this.store = store;
            this.router = router;
            this.ngZone = ngZone;
            if (otherInstance)
                throw new Error('LocaleService should have only one instance.');
        }
        Object.defineProperty(LocalizationService.prototype, "currentLang", {
            get: /**
             * @return {?}
             */
            function () {
                return this.store.selectSnapshot((/**
                 * @param {?} state
                 * @return {?}
                 */
                function (state) { return state.SessionState.language; }));
            },
            enumerable: true,
            configurable: true
        });
        /**
         * @param {?} reuse
         * @return {?}
         */
        LocalizationService.prototype.setRouteReuse = /**
         * @param {?} reuse
         * @return {?}
         */
        function (reuse) {
            this.router.routeReuseStrategy.shouldReuseRoute = reuse;
        };
        /**
         * @param {?} locale
         * @return {?}
         */
        LocalizationService.prototype.registerLocale = /**
         * @param {?} locale
         * @return {?}
         */
        function (locale) {
            var _this = this;
            var shouldReuseRoute = this.router.routeReuseStrategy.shouldReuseRoute;
            this.setRouteReuse((/**
             * @return {?}
             */
            function () { return false; }));
            this.router.navigated = false;
            return registerLocale(locale).then((/**
             * @return {?}
             */
            function () {
                _this.ngZone.run((/**
                 * @return {?}
                 */
                function () { return __awaiter(_this, void 0, void 0, function () {
                    return __generator(this, function (_a) {
                        switch (_a.label) {
                            case 0: return [4 /*yield*/, this.router.navigateByUrl(this.router.url).catch(rxjs.noop)];
                            case 1:
                                _a.sent();
                                this.setRouteReuse(shouldReuseRoute);
                                return [2 /*return*/];
                        }
                    });
                }); }));
            }));
        };
        /**
         * @param {?} key
         * @param {...?} interpolateParams
         * @return {?}
         */
        LocalizationService.prototype.get = /**
         * @param {?} key
         * @param {...?} interpolateParams
         * @return {?}
         */
        function (key) {
            var interpolateParams = [];
            for (var _i = 1; _i < arguments.length; _i++) {
                interpolateParams[_i - 1] = arguments[_i];
            }
            return this.store.select(ConfigState.getLocalization.apply(ConfigState, __spread([key], interpolateParams)));
        };
        /**
         * @param {?} key
         * @param {...?} interpolateParams
         * @return {?}
         */
        LocalizationService.prototype.instant = /**
         * @param {?} key
         * @param {...?} interpolateParams
         * @return {?}
         */
        function (key) {
            var interpolateParams = [];
            for (var _i = 1; _i < arguments.length; _i++) {
                interpolateParams[_i - 1] = arguments[_i];
            }
            return this.store.selectSnapshot(ConfigState.getLocalization.apply(ConfigState, __spread([key], interpolateParams)));
        };
        LocalizationService.decorators = [
            { type: core.Injectable, args: [{ providedIn: 'root' },] }
        ];
        /** @nocollapse */
        LocalizationService.ctorParameters = function () { return [
            { type: store.Store },
            { type: router.Router },
            { type: core.NgZone },
            { type: LocalizationService, decorators: [{ type: core.Optional }, { type: core.SkipSelf }] }
        ]; };
        /** @nocollapse */ LocalizationService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function LocalizationService_Factory() { return new LocalizationService(core.ɵɵinject(store.Store), core.ɵɵinject(router.Router), core.ɵɵinject(core.NgZone), core.ɵɵinject(LocalizationService, 12)); }, token: LocalizationService, providedIn: "root" });
        return LocalizationService;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        LocalizationService.prototype.store;
        /**
         * @type {?}
         * @private
         */
        LocalizationService.prototype.router;
        /**
         * @type {?}
         * @private
         */
        LocalizationService.prototype.ngZone;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/states/session.state.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var SessionState = /** @class */ (function () {
        function SessionState(localizationService) {
            this.localizationService = localizationService;
        }
        /**
         * @param {?} __0
         * @return {?}
         */
        SessionState.getLanguage = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var language = _a.language;
            return language;
        };
        /**
         * @param {?} __0
         * @return {?}
         */
        SessionState.getTenant = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var tenant = _a.tenant;
            return tenant;
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        SessionState.prototype.setLanguage = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var _this = this;
            var patchState = _a.patchState, dispatch = _a.dispatch;
            var payload = _b.payload;
            patchState({
                language: payload,
            });
            return dispatch(new GetAppConfiguration()).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return rxjs.from(_this.localizationService.registerLocale(payload)); })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        SessionState.prototype.setTenant = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var patchState = _a.patchState;
            var payload = _b.payload;
            patchState({
                tenant: payload,
            });
        };
        SessionState.ctorParameters = function () { return [
            { type: LocalizationService }
        ]; };
        __decorate([
            store.Action(SetLanguage),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, SetLanguage]),
            __metadata("design:returntype", void 0)
        ], SessionState.prototype, "setLanguage", null);
        __decorate([
            store.Action(SetTenant),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, SetTenant]),
            __metadata("design:returntype", void 0)
        ], SessionState.prototype, "setTenant", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", String)
        ], SessionState, "getLanguage", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", Object)
        ], SessionState, "getTenant", null);
        SessionState = __decorate([
            store.State({
                name: 'SessionState',
                defaults: (/** @type {?} */ ({})),
            }),
            __metadata("design:paramtypes", [LocalizationService])
        ], SessionState);
        return SessionState;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        SessionState.prototype.localizationService;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/states/config.state.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ConfigState = /** @class */ (function () {
        function ConfigState(appConfigurationService, store) {
            this.appConfigurationService = appConfigurationService;
            this.store = store;
        }
        ConfigState_1 = ConfigState;
        /**
         * @param {?} state
         * @return {?}
         */
        ConfigState.getAll = /**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            return state;
        };
        /**
         * @param {?} state
         * @return {?}
         */
        ConfigState.getApplicationInfo = /**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            return state.environment.application || ((/** @type {?} */ ({})));
        };
        /**
         * @param {?} key
         * @return {?}
         */
        ConfigState.getOne = /**
         * @param {?} key
         * @return {?}
         */
        function (key) {
            /** @type {?} */
            var selector = store.createSelector([ConfigState_1], (/**
             * @param {?} state
             * @return {?}
             */
            function (state) {
                return state[key];
            }));
            return selector;
        };
        /**
         * @param {?} keys
         * @return {?}
         */
        ConfigState.getDeep = /**
         * @param {?} keys
         * @return {?}
         */
        function (keys) {
            if (typeof keys === 'string') {
                keys = keys.split('.');
            }
            if (!Array.isArray(keys)) {
                throw new Error('The argument must be a dot string or an string array.');
            }
            /** @type {?} */
            var selector = store.createSelector([ConfigState_1], (/**
             * @param {?} state
             * @return {?}
             */
            function (state) {
                return ((/** @type {?} */ (keys))).reduce((/**
                 * @param {?} acc
                 * @param {?} val
                 * @return {?}
                 */
                function (acc, val) {
                    if (acc) {
                        return acc[val];
                    }
                    return undefined;
                }), state);
            }));
            return selector;
        };
        /**
         * @param {?=} path
         * @param {?=} name
         * @return {?}
         */
        ConfigState.getRoute = /**
         * @param {?=} path
         * @param {?=} name
         * @return {?}
         */
        function (path, name) {
            /** @type {?} */
            var selector = store.createSelector([ConfigState_1], (/**
             * @param {?} state
             * @return {?}
             */
            function (state) {
                var flattedRoutes = state.flattedRoutes;
                return ((/** @type {?} */ (flattedRoutes))).find((/**
                 * @param {?} route
                 * @return {?}
                 */
                function (route) {
                    if (path && route.path === path) {
                        return route;
                    }
                    else if (name && route.name === name) {
                        return route;
                    }
                }));
            }));
            return selector;
        };
        /**
         * @param {?=} key
         * @return {?}
         */
        ConfigState.getApiUrl = /**
         * @param {?=} key
         * @return {?}
         */
        function (key) {
            /** @type {?} */
            var selector = store.createSelector([ConfigState_1], (/**
             * @param {?} state
             * @return {?}
             */
            function (state) {
                return state.environment.apis[key || 'default'].url;
            }));
            return selector;
        };
        /**
         * @param {?} key
         * @return {?}
         */
        ConfigState.getSetting = /**
         * @param {?} key
         * @return {?}
         */
        function (key) {
            /** @type {?} */
            var selector = store.createSelector([ConfigState_1], (/**
             * @param {?} state
             * @return {?}
             */
            function (state) {
                return snq((/**
                 * @return {?}
                 */
                function () { return state.setting.values[key]; }));
            }));
            return selector;
        };
        /**
         * @param {?=} keyword
         * @return {?}
         */
        ConfigState.getSettings = /**
         * @param {?=} keyword
         * @return {?}
         */
        function (keyword) {
            /** @type {?} */
            var selector = store.createSelector([ConfigState_1], (/**
             * @param {?} state
             * @return {?}
             */
            function (state) {
                if (keyword) {
                    /** @type {?} */
                    var keys = snq((/**
                     * @return {?}
                     */
                    function () { return Object.keys(state.setting.values).filter((/**
                     * @param {?} key
                     * @return {?}
                     */
                    function (key) { return key.indexOf(keyword) > -1; })); }), []);
                    if (keys.length) {
                        return keys.reduce((/**
                         * @param {?} acc
                         * @param {?} key
                         * @return {?}
                         */
                        function (acc, key) {
                            var _a;
                            return (__assign({}, acc, (_a = {}, _a[key] = state.setting.values[key], _a)));
                        }), {});
                    }
                }
                return snq((/**
                 * @return {?}
                 */
                function () { return state.setting.values; }), {});
            }));
            return selector;
        };
        /**
         * @param {?} key
         * @return {?}
         */
        ConfigState.getGrantedPolicy = /**
         * @param {?} key
         * @return {?}
         */
        function (key) {
            /** @type {?} */
            var selector = store.createSelector([ConfigState_1], (/**
             * @param {?} state
             * @return {?}
             */
            function (state) {
                if (!key)
                    return true;
                return snq((/**
                 * @return {?}
                 */
                function () { return state.auth.grantedPolicies[key]; }), false);
            }));
            return selector;
        };
        /**
         * @param {?} key
         * @param {...?} interpolateParams
         * @return {?}
         */
        ConfigState.getLocalization = /**
         * @param {?} key
         * @param {...?} interpolateParams
         * @return {?}
         */
        function (key) {
            var interpolateParams = [];
            for (var _i = 1; _i < arguments.length; _i++) {
                interpolateParams[_i - 1] = arguments[_i];
            }
            /** @type {?} */
            var defaultValue;
            if (typeof key !== 'string') {
                defaultValue = key.defaultValue;
                key = key.key;
            }
            if (!key)
                key = '';
            /** @type {?} */
            var keys = (/** @type {?} */ (key.split('::')));
            /** @type {?} */
            var selector = store.createSelector([ConfigState_1], (/**
             * @param {?} state
             * @return {?}
             */
            function (state) {
                if (!state.localization)
                    return defaultValue || key;
                var defaultResourceName = state.environment.localization.defaultResourceName;
                if (keys[0] === '') {
                    if (!defaultResourceName) {
                        throw new Error("Please check your environment. May you forget set defaultResourceName?\n              Here is the example:\n               { production: false,\n                 localization: {\n                   defaultResourceName: 'MyProjectName'\n                  }\n               }");
                    }
                    keys[0] = snq((/**
                     * @return {?}
                     */
                    function () { return defaultResourceName; }));
                }
                /** @type {?} */
                var localization = ((/** @type {?} */ (keys))).reduce((/**
                 * @param {?} acc
                 * @param {?} val
                 * @return {?}
                 */
                function (acc, val) {
                    if (acc) {
                        return acc[val];
                    }
                    return undefined;
                }), state.localization.values);
                interpolateParams = interpolateParams.filter((/**
                 * @param {?} params
                 * @return {?}
                 */
                function (params) { return params != null; }));
                if (localization && interpolateParams && interpolateParams.length) {
                    interpolateParams.forEach((/**
                     * @param {?} param
                     * @return {?}
                     */
                    function (param) {
                        localization = localization.replace(/[\'\"]?\{[\d]+\}[\'\"]?/, param);
                    }));
                }
                if (typeof localization !== 'string')
                    localization = '';
                return localization || defaultValue || key;
            }));
            return selector;
        };
        /**
         * @param {?} __0
         * @return {?}
         */
        ConfigState.prototype.addData = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var _this = this;
            var patchState = _a.patchState, dispatch = _a.dispatch;
            return this.appConfigurationService.getConfiguration().pipe(operators.tap((/**
             * @param {?} configuration
             * @return {?}
             */
            function (configuration) {
                return patchState(__assign({}, configuration));
            })), operators.switchMap((/**
             * @param {?} configuration
             * @return {?}
             */
            function (configuration) {
                /** @type {?} */
                var defaultLang = configuration.setting.values['Abp.Localization.DefaultLanguage'];
                if (defaultLang.includes(';')) {
                    defaultLang = defaultLang.split(';')[0];
                }
                return _this.store.selectSnapshot(SessionState.getLanguage) ? rxjs.of(null) : dispatch(new SetLanguage(defaultLang));
            })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        ConfigState.prototype.patchRoute = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var patchState = _a.patchState, getState = _a.getState;
            var name = _b.name, newValue = _b.newValue;
            /** @type {?} */
            var routes = getState().routes;
            /** @type {?} */
            var index = routes.findIndex((/**
             * @param {?} route
             * @return {?}
             */
            function (route) { return route.name === name; }));
            routes = patchRouteDeep(routes, name, newValue);
            return patchState({
                routes: routes,
            });
        };
        var ConfigState_1;
        ConfigState.ctorParameters = function () { return [
            { type: ApplicationConfigurationService },
            { type: store.Store }
        ]; };
        __decorate([
            store.Action(GetAppConfiguration),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", void 0)
        ], ConfigState.prototype, "addData", null);
        __decorate([
            store.Action(PatchRouteByName),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, PatchRouteByName]),
            __metadata("design:returntype", void 0)
        ], ConfigState.prototype, "patchRoute", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", void 0)
        ], ConfigState, "getAll", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", Object)
        ], ConfigState, "getApplicationInfo", null);
        ConfigState = ConfigState_1 = __decorate([
            store.State({
                name: 'ConfigState',
                defaults: (/** @type {?} */ ({})),
            }),
            __metadata("design:paramtypes", [ApplicationConfigurationService, store.Store])
        ], ConfigState);
        return ConfigState;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        ConfigState.prototype.appConfigurationService;
        /**
         * @type {?}
         * @private
         */
        ConfigState.prototype.store;
    }
    /**
     * @param {?} routes
     * @param {?} name
     * @param {?} newValue
     * @param {?=} parentUrl
     * @return {?}
     */
    function patchRouteDeep(routes, name, newValue, parentUrl) {
        if (parentUrl === void 0) { parentUrl = ''; }
        routes = routes.map((/**
         * @param {?} route
         * @return {?}
         */
        function (route) {
            if (route.name === name) {
                newValue.url = parentUrl + "/" + ((!newValue.path && newValue.path === '' ? route.path : newValue.path) || '');
                if (newValue.children && newValue.children.length) {
                    newValue.children = newValue.children.map((/**
                     * @param {?} child
                     * @return {?}
                     */
                    function (child) { return (__assign({}, child, { url: (newValue.url + "/" + child.path).replace('//', '/') })); }));
                }
                return __assign({}, route, newValue);
            }
            else if (route.children && route.children.length) {
                route.children = patchRouteDeep(route.children, name, newValue, (parentUrl || '/') + route.path);
            }
            return route;
        }));
        if (parentUrl) {
            // recursive block
            return routes;
        }
        return organizeRoutes(routes);
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/utils/rxjs-utils.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @param {?} value
     * @return {?}
     */
    function isFunction(value) {
        return typeof value === 'function';
    }
    /** @type {?} */
    var takeUntilDestroy = (/**
     * @param {?} componentInstance
     * @param {?=} destroyMethodName
     * @return {?}
     */
    function (componentInstance, destroyMethodName) {
        if (destroyMethodName === void 0) { destroyMethodName = 'ngOnDestroy'; }
        return (/**
         * @template T
         * @param {?} source
         * @return {?}
         */
        function (source) {
            /** @type {?} */
            var originalDestroy = componentInstance[destroyMethodName];
            if (isFunction(originalDestroy) === false) {
                throw new Error(componentInstance.constructor.name + " is using untilDestroyed but doesn't implement " + destroyMethodName);
            }
            if (!componentInstance['__takeUntilDestroy']) {
                componentInstance['__takeUntilDestroy'] = new rxjs.Subject();
                componentInstance[destroyMethodName] = (/**
                 * @return {?}
                 */
                function () {
                    // tslint:disable-next-line: no-unused-expression
                    isFunction(originalDestroy) && originalDestroy.apply(this, arguments);
                    componentInstance['__takeUntilDestroy'].next(true);
                    componentInstance['__takeUntilDestroy'].complete();
                });
            }
            return source.pipe(operators.takeUntil(componentInstance['__takeUntilDestroy']));
        });
    });

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/dynamic-layout.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var DynamicLayoutComponent = /** @class */ (function () {
        function DynamicLayoutComponent(router$1, route, store) {
            var _this = this;
            this.router = router$1;
            this.route = route;
            this.store = store;
            var _a = this.store.selectSnapshot(ConfigState.getAll), layouts = _a.requirements.layouts, routes = _a.routes;
            if ((this.route.snapshot.data || {}).layout) {
                this.layout = layouts
                    .filter((/**
                 * @param {?} l
                 * @return {?}
                 */
                function (l) { return !!l; }))
                    .find((/**
                 * @param {?} l
                 * @return {?}
                 */
                function (l) { return snq((/**
                 * @return {?}
                 */
                function () { return l.type.toLowerCase().indexOf(_this.route.snapshot.data.layout); }), -1) > -1; }));
            }
            this.router.events.pipe(takeUntilDestroy(this)).subscribe((/**
             * @param {?} event
             * @return {?}
             */
            function (event) {
                if (event instanceof router.NavigationEnd) {
                    var segments = _this.router.parseUrl(event.url).root.children.primary.segments;
                    /** @type {?} */
                    var layout_1 = (_this.route.snapshot.data || {}).layout || findLayout(segments, routes);
                    _this.layout = layouts
                        .filter((/**
                     * @param {?} l
                     * @return {?}
                     */
                    function (l) { return !!l; }))
                        .find((/**
                     * @param {?} l
                     * @return {?}
                     */
                    function (l) { return snq((/**
                     * @return {?}
                     */
                    function () { return l.type.toLowerCase().indexOf(layout_1); }), -1) > -1; }));
                }
            }));
        }
        /**
         * @return {?}
         */
        DynamicLayoutComponent.prototype.ngOnDestroy = /**
         * @return {?}
         */
        function () { };
        DynamicLayoutComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-dynamic-layout',
                        template: "\n    <ng-container *ngTemplateOutlet=\"layout ? componentOutlet : routerOutlet\"></ng-container>\n    <ng-template #routerOutlet><router-outlet></router-outlet></ng-template>\n    <ng-template #componentOutlet><ng-container *ngComponentOutlet=\"layout\"></ng-container></ng-template>\n  "
                    }] }
        ];
        /** @nocollapse */
        DynamicLayoutComponent.ctorParameters = function () { return [
            { type: router.Router },
            { type: router.ActivatedRoute },
            { type: store.Store }
        ]; };
        __decorate([
            store.Select(ConfigState.getOne('requirements')),
            __metadata("design:type", rxjs.Observable)
        ], DynamicLayoutComponent.prototype, "requirements$", void 0);
        return DynamicLayoutComponent;
    }());
    if (false) {
        /** @type {?} */
        DynamicLayoutComponent.prototype.requirements$;
        /** @type {?} */
        DynamicLayoutComponent.prototype.layout;
        /**
         * @type {?}
         * @private
         */
        DynamicLayoutComponent.prototype.router;
        /**
         * @type {?}
         * @private
         */
        DynamicLayoutComponent.prototype.route;
        /**
         * @type {?}
         * @private
         */
        DynamicLayoutComponent.prototype.store;
    }
    /**
     * @param {?} segments
     * @param {?} routes
     * @return {?}
     */
    function findLayout(segments, routes) {
        /** @type {?} */
        var layout = "empty" /* empty */;
        /** @type {?} */
        var route = routes
            .reduce((/**
         * @param {?} acc
         * @param {?} val
         * @return {?}
         */
        function (acc, val) { return (val.wrapper ? __spread(acc, val.children) : __spread(acc, [val])); }), [])
            .find((/**
         * @param {?} r
         * @return {?}
         */
        function (r) { return r.path === segments[0].path; }));
        if (route) {
            if (route.layout) {
                layout = route.layout;
            }
            if (route.children && route.children.length && segments.length > 1) {
                /** @type {?} */
                var child = route.children.find((/**
                 * @param {?} c
                 * @return {?}
                 */
                function (c) { return c.path === segments[1].path; }));
                if (child && child.layout) {
                    layout = child.layout;
                }
            }
        }
        return layout;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/router-outlet.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var RouterOutletComponent = /** @class */ (function () {
        function RouterOutletComponent() {
        }
        RouterOutletComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-router-outlet',
                        template: "\n    <router-outlet></router-outlet>\n  "
                    }] }
        ];
        return RouterOutletComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/constants/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/directives/autofocus.directive.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var AutofocusDirective = /** @class */ (function () {
        function AutofocusDirective(elRef) {
            this.elRef = elRef;
            this.delay = 0;
        }
        /**
         * @return {?}
         */
        AutofocusDirective.prototype.ngAfterViewInit = /**
         * @return {?}
         */
        function () {
            var _this = this;
            setTimeout((/**
             * @return {?}
             */
            function () { return _this.elRef.nativeElement.focus(); }), this.delay);
        };
        AutofocusDirective.decorators = [
            { type: core.Directive, args: [{
                        // tslint:disable-next-line: directive-selector
                        selector: '[autofocus]'
                    },] }
        ];
        /** @nocollapse */
        AutofocusDirective.ctorParameters = function () { return [
            { type: core.ElementRef }
        ]; };
        AutofocusDirective.propDecorators = {
            delay: [{ type: core.Input, args: ['autofocus',] }]
        };
        return AutofocusDirective;
    }());
    if (false) {
        /** @type {?} */
        AutofocusDirective.prototype.delay;
        /**
         * @type {?}
         * @private
         */
        AutofocusDirective.prototype.elRef;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/directives/ellipsis.directive.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var EllipsisDirective = /** @class */ (function () {
        function EllipsisDirective(cdRef, elRef) {
            this.cdRef = cdRef;
            this.elRef = elRef;
            this.enabled = true;
        }
        Object.defineProperty(EllipsisDirective.prototype, "inlineClass", {
            get: /**
             * @return {?}
             */
            function () {
                return this.enabled && this.width;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(EllipsisDirective.prototype, "class", {
            get: /**
             * @return {?}
             */
            function () {
                return this.enabled && !this.width;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(EllipsisDirective.prototype, "maxWidth", {
            get: /**
             * @return {?}
             */
            function () {
                return this.enabled && this.width ? this.width || '170px' : undefined;
            },
            enumerable: true,
            configurable: true
        });
        /**
         * @return {?}
         */
        EllipsisDirective.prototype.ngAfterViewInit = /**
         * @return {?}
         */
        function () {
            this.title = this.title || ((/** @type {?} */ (this.elRef.nativeElement))).innerText;
            this.cdRef.detectChanges();
        };
        EllipsisDirective.decorators = [
            { type: core.Directive, args: [{
                        selector: '[abpEllipsis]',
                    },] }
        ];
        /** @nocollapse */
        EllipsisDirective.ctorParameters = function () { return [
            { type: core.ChangeDetectorRef },
            { type: core.ElementRef }
        ]; };
        EllipsisDirective.propDecorators = {
            width: [{ type: core.Input, args: ['abpEllipsis',] }],
            title: [{ type: core.HostBinding, args: ['title',] }, { type: core.Input }],
            enabled: [{ type: core.Input, args: ['abpEllipsisEnabled',] }],
            inlineClass: [{ type: core.HostBinding, args: ['class.abp-ellipsis-inline',] }],
            class: [{ type: core.HostBinding, args: ['class.abp-ellipsis',] }],
            maxWidth: [{ type: core.HostBinding, args: ['style.max-width',] }]
        };
        return EllipsisDirective;
    }());
    if (false) {
        /** @type {?} */
        EllipsisDirective.prototype.width;
        /** @type {?} */
        EllipsisDirective.prototype.title;
        /** @type {?} */
        EllipsisDirective.prototype.enabled;
        /**
         * @type {?}
         * @private
         */
        EllipsisDirective.prototype.cdRef;
        /**
         * @type {?}
         * @private
         */
        EllipsisDirective.prototype.elRef;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/directives/for.directive.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var AbpForContext = /** @class */ (function () {
        function AbpForContext($implicit, index, count, list) {
            this.$implicit = $implicit;
            this.index = index;
            this.count = count;
            this.list = list;
        }
        return AbpForContext;
    }());
    if (false) {
        /** @type {?} */
        AbpForContext.prototype.$implicit;
        /** @type {?} */
        AbpForContext.prototype.index;
        /** @type {?} */
        AbpForContext.prototype.count;
        /** @type {?} */
        AbpForContext.prototype.list;
    }
    var RecordView = /** @class */ (function () {
        function RecordView(record, view) {
            this.record = record;
            this.view = view;
        }
        return RecordView;
    }());
    if (false) {
        /** @type {?} */
        RecordView.prototype.record;
        /** @type {?} */
        RecordView.prototype.view;
    }
    var ForDirective = /** @class */ (function () {
        function ForDirective(tempRef, vcRef, differs) {
            this.tempRef = tempRef;
            this.vcRef = vcRef;
            this.differs = differs;
        }
        Object.defineProperty(ForDirective.prototype, "compareFn", {
            get: /**
             * @return {?}
             */
            function () {
                return this.compareBy || compare;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(ForDirective.prototype, "trackByFn", {
            get: /**
             * @return {?}
             */
            function () {
                return this.trackBy || ((/**
                 * @param {?} index
                 * @param {?} item
                 * @return {?}
                 */
                function (index, item) { return ((/** @type {?} */ (item))).id || index; }));
            },
            enumerable: true,
            configurable: true
        });
        /**
         * @private
         * @param {?} changes
         * @return {?}
         */
        ForDirective.prototype.iterateOverAppliedOperations = /**
         * @private
         * @param {?} changes
         * @return {?}
         */
        function (changes) {
            var _this = this;
            /** @type {?} */
            var rw = [];
            changes.forEachOperation((/**
             * @param {?} record
             * @param {?} previousIndex
             * @param {?} currentIndex
             * @return {?}
             */
            function (record, previousIndex, currentIndex) {
                if (record.previousIndex == null) {
                    /** @type {?} */
                    var view = _this.vcRef.createEmbeddedView(_this.tempRef, new AbpForContext(null, -1, -1, _this.items), currentIndex);
                    rw.push(new RecordView(record, view));
                }
                else if (currentIndex == null) {
                    _this.vcRef.remove(previousIndex);
                }
                else {
                    /** @type {?} */
                    var view = _this.vcRef.get(previousIndex);
                    _this.vcRef.move(view, currentIndex);
                    rw.push(new RecordView(record, (/** @type {?} */ (view))));
                }
            }));
            for (var i = 0, l = rw.length; i < l; i++) {
                rw[i].view.context.$implicit = rw[i].record.item;
            }
        };
        /**
         * @private
         * @param {?} changes
         * @return {?}
         */
        ForDirective.prototype.iterateOverAttachedViews = /**
         * @private
         * @param {?} changes
         * @return {?}
         */
        function (changes) {
            var _this = this;
            for (var i = 0, l = this.vcRef.length; i < l; i++) {
                /** @type {?} */
                var viewRef = (/** @type {?} */ (this.vcRef.get(i)));
                viewRef.context.index = i;
                viewRef.context.count = l;
                viewRef.context.list = this.items;
            }
            changes.forEachIdentityChange((/**
             * @param {?} record
             * @return {?}
             */
            function (record) {
                /** @type {?} */
                var viewRef = (/** @type {?} */ (_this.vcRef.get(record.currentIndex)));
                viewRef.context.$implicit = record.item;
            }));
        };
        /**
         * @private
         * @param {?} items
         * @return {?}
         */
        ForDirective.prototype.projectItems = /**
         * @private
         * @param {?} items
         * @return {?}
         */
        function (items) {
            if (!items.length && this.emptyRef) {
                this.vcRef.clear();
                // tslint:disable-next-line: no-unused-expression
                this.vcRef.createEmbeddedView(this.emptyRef).rootNodes;
                this.isShowEmptyRef = true;
                this.differ = null;
                return;
            }
            if (this.emptyRef && this.isShowEmptyRef) {
                this.vcRef.clear();
                this.isShowEmptyRef = false;
            }
            if (!this.differ && items) {
                this.differ = this.differs.find(items).create(this.trackByFn);
            }
            if (this.differ) {
                /** @type {?} */
                var changes = this.differ.diff(items);
                if (changes) {
                    this.iterateOverAppliedOperations(changes);
                    this.iterateOverAttachedViews(changes);
                }
            }
        };
        /**
         * @private
         * @param {?} items
         * @return {?}
         */
        ForDirective.prototype.sortItems = /**
         * @private
         * @param {?} items
         * @return {?}
         */
        function (items) {
            var _this = this;
            if (this.orderBy) {
                items.sort((/**
                 * @param {?} a
                 * @param {?} b
                 * @return {?}
                 */
                function (a, b) { return (a[_this.orderBy] > b[_this.orderBy] ? 1 : a[_this.orderBy] < b[_this.orderBy] ? -1 : 0); }));
            }
            else {
                items.sort();
            }
        };
        /**
         * @return {?}
         */
        ForDirective.prototype.ngOnChanges = /**
         * @return {?}
         */
        function () {
            var _this = this;
            /** @type {?} */
            var items = (/** @type {?} */ (clone(this.items)));
            if (!Array.isArray(items))
                return;
            /** @type {?} */
            var compareFn = this.compareFn;
            if (typeof this.filterBy !== 'undefined' && this.filterVal) {
                items = items.filter((/**
                 * @param {?} item
                 * @return {?}
                 */
                function (item) { return compareFn(item[_this.filterBy], _this.filterVal); }));
            }
            switch (this.orderDir) {
                case 'ASC':
                    this.sortItems(items);
                    this.projectItems(items);
                    break;
                case 'DESC':
                    this.sortItems(items);
                    items.reverse();
                    this.projectItems(items);
                    break;
                default:
                    this.projectItems(items);
            }
        };
        ForDirective.decorators = [
            { type: core.Directive, args: [{
                        selector: '[abpFor]',
                    },] }
        ];
        /** @nocollapse */
        ForDirective.ctorParameters = function () { return [
            { type: core.TemplateRef },
            { type: core.ViewContainerRef },
            { type: core.IterableDiffers }
        ]; };
        ForDirective.propDecorators = {
            items: [{ type: core.Input, args: ['abpForOf',] }],
            orderBy: [{ type: core.Input, args: ['abpForOrderBy',] }],
            orderDir: [{ type: core.Input, args: ['abpForOrderDir',] }],
            filterBy: [{ type: core.Input, args: ['abpForFilterBy',] }],
            filterVal: [{ type: core.Input, args: ['abpForFilterVal',] }],
            trackBy: [{ type: core.Input, args: ['abpForTrackBy',] }],
            compareBy: [{ type: core.Input, args: ['abpForCompareBy',] }],
            emptyRef: [{ type: core.Input, args: ['abpForEmptyRef',] }]
        };
        return ForDirective;
    }());
    if (false) {
        /** @type {?} */
        ForDirective.prototype.items;
        /** @type {?} */
        ForDirective.prototype.orderBy;
        /** @type {?} */
        ForDirective.prototype.orderDir;
        /** @type {?} */
        ForDirective.prototype.filterBy;
        /** @type {?} */
        ForDirective.prototype.filterVal;
        /** @type {?} */
        ForDirective.prototype.trackBy;
        /** @type {?} */
        ForDirective.prototype.compareBy;
        /** @type {?} */
        ForDirective.prototype.emptyRef;
        /**
         * @type {?}
         * @private
         */
        ForDirective.prototype.differ;
        /**
         * @type {?}
         * @private
         */
        ForDirective.prototype.isShowEmptyRef;
        /**
         * @type {?}
         * @private
         */
        ForDirective.prototype.tempRef;
        /**
         * @type {?}
         * @private
         */
        ForDirective.prototype.vcRef;
        /**
         * @type {?}
         * @private
         */
        ForDirective.prototype.differs;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/utils/common-utils.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @return {?}
     */
    function noop() {
        // tslint:disable-next-line: only-arrow-functions
        /** @type {?} */
        var fn = (/**
         * @return {?}
         */
        function () { });
        return fn;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/utils/generator-utils.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @param {?=} a
     * @return {?}
     */
    function uuid(a) {
        return a
            ? // tslint:disable-next-line: no-bitwise
                (a ^ ((Math.random() * 16) >> (a / 4))).toString(16)
            : ('' + 1e7 + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, uuid);
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/utils/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/directives/form-submit.directive.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var FormSubmitDirective = /** @class */ (function () {
        function FormSubmitDirective(formGroupDirective, host, cdRef) {
            this.formGroupDirective = formGroupDirective;
            this.host = host;
            this.cdRef = cdRef;
            this.ngSubmit = new core.EventEmitter();
            this.executedNgSubmit = false;
        }
        /**
         * @return {?}
         */
        FormSubmitDirective.prototype.ngOnInit = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.formGroupDirective.ngSubmit.pipe(takeUntilDestroy(this)).subscribe((/**
             * @return {?}
             */
            function () {
                _this.markAsDirty();
                _this.executedNgSubmit = true;
            }));
            rxjs.fromEvent((/** @type {?} */ (this.host.nativeElement)), 'keyup')
                .pipe(operators.debounceTime(200), operators.filter((/**
             * @param {?} key
             * @return {?}
             */
            function (key) { return key && key.key === 'Enter'; })), takeUntilDestroy(this))
                .subscribe((/**
             * @return {?}
             */
            function () {
                if (!_this.executedNgSubmit) {
                    _this.host.nativeElement.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
                }
                _this.executedNgSubmit = false;
            }));
        };
        /**
         * @return {?}
         */
        FormSubmitDirective.prototype.ngOnDestroy = /**
         * @return {?}
         */
        function () { };
        /**
         * @return {?}
         */
        FormSubmitDirective.prototype.markAsDirty = /**
         * @return {?}
         */
        function () {
            var form = this.formGroupDirective.form;
            setDirty((/** @type {?} */ (form.controls)));
            form.markAsDirty();
            this.cdRef.detectChanges();
        };
        FormSubmitDirective.decorators = [
            { type: core.Directive, args: [{
                        // tslint:disable-next-line: directive-selector
                        selector: 'form[ngSubmit][formGroup]',
                    },] }
        ];
        /** @nocollapse */
        FormSubmitDirective.ctorParameters = function () { return [
            { type: forms.FormGroupDirective, decorators: [{ type: core.Self }] },
            { type: core.ElementRef },
            { type: core.ChangeDetectorRef }
        ]; };
        FormSubmitDirective.propDecorators = {
            notValidateOnSubmit: [{ type: core.Input }],
            ngSubmit: [{ type: core.Output }]
        };
        return FormSubmitDirective;
    }());
    if (false) {
        /** @type {?} */
        FormSubmitDirective.prototype.notValidateOnSubmit;
        /** @type {?} */
        FormSubmitDirective.prototype.ngSubmit;
        /** @type {?} */
        FormSubmitDirective.prototype.executedNgSubmit;
        /**
         * @type {?}
         * @private
         */
        FormSubmitDirective.prototype.formGroupDirective;
        /**
         * @type {?}
         * @private
         */
        FormSubmitDirective.prototype.host;
        /**
         * @type {?}
         * @private
         */
        FormSubmitDirective.prototype.cdRef;
    }
    /**
     * @param {?} controls
     * @return {?}
     */
    function setDirty(controls) {
        if (Array.isArray(controls)) {
            controls.forEach((/**
             * @param {?} group
             * @return {?}
             */
            function (group) {
                setDirty((/** @type {?} */ (group.controls)));
            }));
            return;
        }
        Object.keys(controls).forEach((/**
         * @param {?} key
         * @return {?}
         */
        function (key) {
            controls[key].markAsDirty();
            controls[key].updateValueAndValidity();
        }));
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/profile.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ProfileService = /** @class */ (function () {
        function ProfileService(rest) {
            this.rest = rest;
        }
        /**
         * @return {?}
         */
        ProfileService.prototype.get = /**
         * @return {?}
         */
        function () {
            /** @type {?} */
            var request = {
                method: 'GET',
                url: '/api/identity/my-profile',
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} body
         * @return {?}
         */
        ProfileService.prototype.update = /**
         * @param {?} body
         * @return {?}
         */
        function (body) {
            /** @type {?} */
            var request = {
                method: 'PUT',
                url: '/api/identity/my-profile',
                body: body,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} body
         * @param {?=} skipHandleError
         * @return {?}
         */
        ProfileService.prototype.changePassword = /**
         * @param {?} body
         * @param {?=} skipHandleError
         * @return {?}
         */
        function (body, skipHandleError) {
            if (skipHandleError === void 0) { skipHandleError = false; }
            /** @type {?} */
            var request = {
                method: 'POST',
                url: '/api/identity/my-profile/change-password',
                body: body,
            };
            return this.rest.request(request, { skipHandleError: skipHandleError });
        };
        ProfileService.decorators = [
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        ProfileService.ctorParameters = function () { return [
            { type: RestService }
        ]; };
        /** @nocollapse */ ProfileService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function ProfileService_Factory() { return new ProfileService(core.ɵɵinject(RestService)); }, token: ProfileService, providedIn: "root" });
        return ProfileService;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        ProfileService.prototype.rest;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/states/profile.state.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ProfileState = /** @class */ (function () {
        function ProfileState(profileService) {
            this.profileService = profileService;
        }
        /**
         * @param {?} __0
         * @return {?}
         */
        ProfileState.getProfile = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var profile = _a.profile;
            return profile;
        };
        /**
         * @param {?} __0
         * @return {?}
         */
        ProfileState.prototype.getProfile = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var patchState = _a.patchState;
            return this.profileService.get().pipe(operators.tap((/**
             * @param {?} profile
             * @return {?}
             */
            function (profile) {
                return patchState({
                    profile: profile,
                });
            })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        ProfileState.prototype.updateProfile = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var patchState = _a.patchState;
            var payload = _b.payload;
            return this.profileService.update(payload).pipe(operators.tap((/**
             * @param {?} profile
             * @return {?}
             */
            function (profile) {
                return patchState({
                    profile: profile,
                });
            })));
        };
        /**
         * @param {?} _
         * @param {?} __1
         * @return {?}
         */
        ProfileState.prototype.changePassword = /**
         * @param {?} _
         * @param {?} __1
         * @return {?}
         */
        function (_, _a) {
            var payload = _a.payload;
            return this.profileService.changePassword(payload, true);
        };
        ProfileState.ctorParameters = function () { return [
            { type: ProfileService }
        ]; };
        __decorate([
            store.Action(GetProfile),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", void 0)
        ], ProfileState.prototype, "getProfile", null);
        __decorate([
            store.Action(UpdateProfile),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, UpdateProfile]),
            __metadata("design:returntype", void 0)
        ], ProfileState.prototype, "updateProfile", null);
        __decorate([
            store.Action(ChangePassword),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, ChangePassword]),
            __metadata("design:returntype", void 0)
        ], ProfileState.prototype, "changePassword", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", Object)
        ], ProfileState, "getProfile", null);
        ProfileState = __decorate([
            store.State({
                name: 'ProfileState',
                defaults: (/** @type {?} */ ({})),
            }),
            __metadata("design:paramtypes", [ProfileService])
        ], ProfileState);
        return ProfileState;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        ProfileState.prototype.profileService;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/states/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/directives/permission.directive.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var PermissionDirective = /** @class */ (function () {
        function PermissionDirective(elRef, renderer, store) {
            this.elRef = elRef;
            this.renderer = renderer;
            this.store = store;
        }
        /**
         * @return {?}
         */
        PermissionDirective.prototype.ngOnInit = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (this.condition) {
                this.store
                    .select(ConfigState.getGrantedPolicy(this.condition))
                    .pipe(takeUntilDestroy(this))
                    .subscribe((/**
                 * @param {?} isGranted
                 * @return {?}
                 */
                function (isGranted) {
                    if (!isGranted) {
                        _this.renderer.removeChild(((/** @type {?} */ (_this.elRef.nativeElement))).parentElement, _this.elRef.nativeElement);
                    }
                }));
            }
        };
        /**
         * @return {?}
         */
        PermissionDirective.prototype.ngOnDestroy = /**
         * @return {?}
         */
        function () { };
        PermissionDirective.decorators = [
            { type: core.Directive, args: [{
                        selector: '[abpPermission]',
                    },] }
        ];
        /** @nocollapse */
        PermissionDirective.ctorParameters = function () { return [
            { type: core.ElementRef, decorators: [{ type: core.Optional }] },
            { type: core.Renderer2 },
            { type: store.Store }
        ]; };
        PermissionDirective.propDecorators = {
            condition: [{ type: core.Input, args: ['abpPermission',] }]
        };
        return PermissionDirective;
    }());
    if (false) {
        /** @type {?} */
        PermissionDirective.prototype.condition;
        /**
         * @type {?}
         * @private
         */
        PermissionDirective.prototype.elRef;
        /**
         * @type {?}
         * @private
         */
        PermissionDirective.prototype.renderer;
        /**
         * @type {?}
         * @private
         */
        PermissionDirective.prototype.store;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/directives/visibility.directive.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var VisibilityDirective = /** @class */ (function () {
        function VisibilityDirective(elRef, renderer) {
            this.elRef = elRef;
            this.renderer = renderer;
            this.completed$ = new rxjs.Subject();
        }
        /**
         * @return {?}
         */
        VisibilityDirective.prototype.ngAfterViewInit = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (!this.focusedElement && this.elRef) {
                this.focusedElement = this.elRef.nativeElement;
            }
            /** @type {?} */
            var observer;
            observer = new MutationObserver((/**
             * @param {?} mutations
             * @return {?}
             */
            function (mutations) {
                mutations.forEach((/**
                 * @param {?} mutation
                 * @return {?}
                 */
                function (mutation) {
                    if (!mutation.target)
                        return;
                    /** @type {?} */
                    var htmlNodes = snq((/**
                     * @return {?}
                     */
                    function () { return Array.from(mutation.target.childNodes).filter((/**
                     * @param {?} node
                     * @return {?}
                     */
                    function (node) { return node instanceof HTMLElement; })); }), []);
                    if (!htmlNodes.length) {
                        _this.removeFromDOM();
                    }
                }));
            }));
            observer.observe(this.focusedElement, {
                childList: true,
            });
            setTimeout((/**
             * @return {?}
             */
            function () {
                /** @type {?} */
                var htmlNodes = snq((/**
                 * @return {?}
                 */
                function () { return Array.from(_this.focusedElement.childNodes).filter((/**
                 * @param {?} node
                 * @return {?}
                 */
                function (node) { return node instanceof HTMLElement; })); }), []);
                if (!htmlNodes.length)
                    _this.removeFromDOM();
            }), 0);
            this.completed$.subscribe((/**
             * @return {?}
             */
            function () { return observer.disconnect(); }));
        };
        /**
         * @return {?}
         */
        VisibilityDirective.prototype.disconnect = /**
         * @return {?}
         */
        function () {
            this.completed$.next();
            this.completed$.complete();
        };
        /**
         * @return {?}
         */
        VisibilityDirective.prototype.removeFromDOM = /**
         * @return {?}
         */
        function () {
            if (!this.elRef.nativeElement)
                return;
            this.renderer.removeChild(this.elRef.nativeElement.parentElement, this.elRef.nativeElement);
            this.disconnect();
        };
        VisibilityDirective.decorators = [
            { type: core.Directive, args: [{
                        selector: '[abpVisibility]',
                    },] }
        ];
        /** @nocollapse */
        VisibilityDirective.ctorParameters = function () { return [
            { type: core.ElementRef, decorators: [{ type: core.Optional }] },
            { type: core.Renderer2 }
        ]; };
        VisibilityDirective.propDecorators = {
            focusedElement: [{ type: core.Input, args: ['abpVisibility',] }]
        };
        return VisibilityDirective;
    }());
    if (false) {
        /** @type {?} */
        VisibilityDirective.prototype.focusedElement;
        /** @type {?} */
        VisibilityDirective.prototype.completed$;
        /**
         * @type {?}
         * @private
         */
        VisibilityDirective.prototype.elRef;
        /**
         * @type {?}
         * @private
         */
        VisibilityDirective.prototype.renderer;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/directives/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/enums/common.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /** @enum {string} */
    var eLayoutType = {
        account: "account",
        application: "application",
        empty: "empty",
        /**
         * @deprecated since version 0.9.0
         */
        setting: "setting",
    };

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/enums/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/guards/auth.guard.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var AuthGuard = /** @class */ (function () {
        function AuthGuard(oauthService, router) {
            this.oauthService = oauthService;
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
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        AuthGuard.ctorParameters = function () { return [
            { type: angularOauth2Oidc.OAuthService },
            { type: router.Router }
        ]; };
        /** @nocollapse */ AuthGuard.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function AuthGuard_Factory() { return new AuthGuard(core.ɵɵinject(angularOauth2Oidc.OAuthService), core.ɵɵinject(router.Router)); }, token: AuthGuard, providedIn: "root" });
        return AuthGuard;
    }());
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
        AuthGuard.prototype.router;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/guards/permission.guard.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var PermissionGuard = /** @class */ (function () {
        function PermissionGuard(store) {
            this.store = store;
        }
        /**
         * @param {?} __0
         * @return {?}
         */
        PermissionGuard.prototype.canActivate = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var _this = this;
            var data = _a.data;
            /** @type {?} */
            var resource = (/** @type {?} */ (data.requiredPolicy));
            return this.store.select(ConfigState.getGrantedPolicy(resource)).pipe(operators.tap((/**
             * @param {?} access
             * @return {?}
             */
            function (access) {
                if (!access) {
                    _this.store.dispatch(new RestOccurError({ status: 403 }));
                }
            })));
        };
        PermissionGuard.decorators = [
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        PermissionGuard.ctorParameters = function () { return [
            { type: store.Store }
        ]; };
        /** @nocollapse */ PermissionGuard.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function PermissionGuard_Factory() { return new PermissionGuard(core.ɵɵinject(store.Store)); }, token: PermissionGuard, providedIn: "root" });
        return PermissionGuard;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        PermissionGuard.prototype.store;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/guards/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/interceptors/api.interceptor.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ApiInterceptor = /** @class */ (function () {
        function ApiInterceptor(oAuthService, store) {
            this.oAuthService = oAuthService;
            this.store = store;
        }
        /**
         * @param {?} request
         * @param {?} next
         * @return {?}
         */
        ApiInterceptor.prototype.intercept = /**
         * @param {?} request
         * @param {?} next
         * @return {?}
         */
        function (request, next) {
            var _this = this;
            this.store.dispatch(new StartLoader(request));
            /** @type {?} */
            var headers = (/** @type {?} */ ({}));
            /** @type {?} */
            var token = this.oAuthService.getAccessToken();
            if (!request.headers.has('Authorization') && token) {
                headers['Authorization'] = "Bearer " + token;
            }
            /** @type {?} */
            var lang = this.store.selectSnapshot(SessionState.getLanguage);
            if (!request.headers.has('Accept-Language') && lang) {
                headers['Accept-Language'] = lang;
            }
            /** @type {?} */
            var tenant = this.store.selectSnapshot(SessionState.getTenant);
            if (!request.headers.has('__tenant') && tenant) {
                headers['__tenant'] = tenant.id;
            }
            return next
                .handle(request.clone({
                setHeaders: headers,
            }))
                .pipe(operators.finalize((/**
             * @return {?}
             */
            function () { return _this.store.dispatch(new StopLoader(request)); })));
        };
        ApiInterceptor.decorators = [
            { type: core.Injectable }
        ];
        /** @nocollapse */
        ApiInterceptor.ctorParameters = function () { return [
            { type: angularOauth2Oidc.OAuthService },
            { type: store.Store }
        ]; };
        return ApiInterceptor;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        ApiInterceptor.prototype.oAuthService;
        /**
         * @type {?}
         * @private
         */
        ApiInterceptor.prototype.store;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/interceptors/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/application-configuration.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ApplicationConfiguration;
    (function (ApplicationConfiguration) {
        /**
         * @record
         */
        function Response() { }
        ApplicationConfiguration.Response = Response;
        if (false) {
            /** @type {?} */
            Response.prototype.localization;
            /** @type {?} */
            Response.prototype.auth;
            /** @type {?} */
            Response.prototype.setting;
            /** @type {?} */
            Response.prototype.currentUser;
            /** @type {?} */
            Response.prototype.features;
        }
        /**
         * @record
         */
        function Localization() { }
        ApplicationConfiguration.Localization = Localization;
        if (false) {
            /** @type {?} */
            Localization.prototype.values;
            /** @type {?} */
            Localization.prototype.languages;
        }
        /**
         * @record
         */
        function LocalizationValue() { }
        ApplicationConfiguration.LocalizationValue = LocalizationValue;
        /**
         * @record
         */
        function Language() { }
        ApplicationConfiguration.Language = Language;
        if (false) {
            /** @type {?} */
            Language.prototype.cultureName;
            /** @type {?} */
            Language.prototype.uiCultureName;
            /** @type {?} */
            Language.prototype.displayName;
            /** @type {?} */
            Language.prototype.flagIcon;
        }
        /**
         * @record
         */
        function Auth() { }
        ApplicationConfiguration.Auth = Auth;
        if (false) {
            /** @type {?} */
            Auth.prototype.policies;
            /** @type {?} */
            Auth.prototype.grantedPolicies;
        }
        /**
         * @record
         */
        function Policy() { }
        ApplicationConfiguration.Policy = Policy;
        /**
         * @record
         */
        function Value() { }
        ApplicationConfiguration.Value = Value;
        if (false) {
            /** @type {?} */
            Value.prototype.values;
        }
        /**
         * @record
         */
        function CurrentUser() { }
        ApplicationConfiguration.CurrentUser = CurrentUser;
        if (false) {
            /** @type {?} */
            CurrentUser.prototype.isAuthenticated;
            /** @type {?} */
            CurrentUser.prototype.id;
            /** @type {?} */
            CurrentUser.prototype.tenantId;
            /** @type {?} */
            CurrentUser.prototype.userName;
        }
    })(ApplicationConfiguration || (ApplicationConfiguration = {}));

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/common.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ABP;
    (function (ABP) {
        /**
         * @record
         */
        function Root() { }
        ABP.Root = Root;
        if (false) {
            /** @type {?} */
            Root.prototype.environment;
            /** @type {?} */
            Root.prototype.requirements;
        }
        /**
         * @record
         * @template T
         */
        function PagedItemsResponse() { }
        ABP.PagedItemsResponse = PagedItemsResponse;
        if (false) {
            /** @type {?} */
            PagedItemsResponse.prototype.items;
        }
        /**
         * @record
         */
        function PageQueryParams() { }
        ABP.PageQueryParams = PageQueryParams;
        if (false) {
            /** @type {?|undefined} */
            PageQueryParams.prototype.filter;
            /** @type {?|undefined} */
            PageQueryParams.prototype.sorting;
            /** @type {?|undefined} */
            PageQueryParams.prototype.skipCount;
            /** @type {?|undefined} */
            PageQueryParams.prototype.maxResultCount;
        }
        /**
         * @record
         */
        function Route() { }
        ABP.Route = Route;
        if (false) {
            /** @type {?|undefined} */
            Route.prototype.children;
            /** @type {?|undefined} */
            Route.prototype.invisible;
            /** @type {?|undefined} */
            Route.prototype.layout;
            /** @type {?} */
            Route.prototype.name;
            /** @type {?|undefined} */
            Route.prototype.order;
            /** @type {?|undefined} */
            Route.prototype.parentName;
            /** @type {?} */
            Route.prototype.path;
            /** @type {?|undefined} */
            Route.prototype.requiredPolicy;
            /** @type {?|undefined} */
            Route.prototype.iconClass;
        }
        /**
         * @record
         */
        function FullRoute() { }
        ABP.FullRoute = FullRoute;
        if (false) {
            /** @type {?|undefined} */
            FullRoute.prototype.url;
            /** @type {?|undefined} */
            FullRoute.prototype.wrapper;
        }
        /**
         * @record
         */
        function BasicItem() { }
        ABP.BasicItem = BasicItem;
        if (false) {
            /** @type {?} */
            BasicItem.prototype.id;
            /** @type {?} */
            BasicItem.prototype.name;
        }
        /**
         * @record
         * @template T
         */
        function Dictionary() { }
        ABP.Dictionary = Dictionary;
    })(ABP || (ABP = {}));

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/config.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var Config;
    (function (Config) {
        /**
         * @record
         */
        function Environment() { }
        Config.Environment = Environment;
        if (false) {
            /** @type {?} */
            Environment.prototype.application;
            /** @type {?} */
            Environment.prototype.production;
            /** @type {?} */
            Environment.prototype.oAuthConfig;
            /** @type {?} */
            Environment.prototype.apis;
            /** @type {?} */
            Environment.prototype.localization;
        }
        /**
         * @record
         */
        function Application() { }
        Config.Application = Application;
        if (false) {
            /** @type {?} */
            Application.prototype.name;
            /** @type {?|undefined} */
            Application.prototype.logoUrl;
        }
        /**
         * @record
         */
        function Apis() { }
        Config.Apis = Apis;
        /**
         * @record
         */
        function Requirements() { }
        Config.Requirements = Requirements;
        if (false) {
            /** @type {?} */
            Requirements.prototype.layouts;
        }
        /**
         * @record
         */
        function LocalizationWithDefault() { }
        Config.LocalizationWithDefault = LocalizationWithDefault;
        if (false) {
            /** @type {?} */
            LocalizationWithDefault.prototype.key;
            /** @type {?} */
            LocalizationWithDefault.prototype.defaultValue;
        }
    })(Config || (Config = {}));

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/rest.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    (function (Rest) {
        /**
         * @record
         */
        function Config() { }
        Rest.Config = Config;
        if (false) {
            /** @type {?|undefined} */
            Config.prototype.skipHandleError;
            /** @type {?|undefined} */
            Config.prototype.observe;
        }
        /**
         * @record
         * @template T
         */
        function Request() { }
        Rest.Request = Request;
        if (false) {
            /** @type {?|undefined} */
            Request.prototype.body;
            /** @type {?|undefined} */
            Request.prototype.headers;
            /** @type {?} */
            Request.prototype.method;
            /** @type {?|undefined} */
            Request.prototype.params;
            /** @type {?|undefined} */
            Request.prototype.reportProgress;
            /** @type {?|undefined} */
            Request.prototype.responseType;
            /** @type {?} */
            Request.prototype.url;
            /** @type {?|undefined} */
            Request.prototype.withCredentials;
        }
    })(exports.Rest || (exports.Rest = {}));

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/session.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var Session;
    (function (Session) {
        /**
         * @record
         */
        function State() { }
        Session.State = State;
        if (false) {
            /** @type {?} */
            State.prototype.language;
            /** @type {?} */
            State.prototype.tenant;
        }
    })(Session || (Session = {}));

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/profile.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var Profile;
    (function (Profile) {
        /**
         * @record
         */
        function State() { }
        Profile.State = State;
        if (false) {
            /** @type {?} */
            State.prototype.profile;
        }
        /**
         * @record
         */
        function Response() { }
        Profile.Response = Response;
        if (false) {
            /** @type {?} */
            Response.prototype.userName;
            /** @type {?} */
            Response.prototype.email;
            /** @type {?} */
            Response.prototype.name;
            /** @type {?} */
            Response.prototype.surname;
            /** @type {?} */
            Response.prototype.phoneNumber;
        }
        /**
         * @record
         */
        function ChangePasswordRequest() { }
        Profile.ChangePasswordRequest = ChangePasswordRequest;
        if (false) {
            /** @type {?} */
            ChangePasswordRequest.prototype.currentPassword;
            /** @type {?} */
            ChangePasswordRequest.prototype.newPassword;
        }
    })(Profile || (Profile = {}));

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/pipes/localization.pipe.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var LocalizationPipe = /** @class */ (function () {
        function LocalizationPipe(store) {
            this.store = store;
        }
        /**
         * @param {?=} value
         * @param {...?} interpolateParams
         * @return {?}
         */
        LocalizationPipe.prototype.transform = /**
         * @param {?=} value
         * @param {...?} interpolateParams
         * @return {?}
         */
        function (value) {
            if (value === void 0) { value = ''; }
            var interpolateParams = [];
            for (var _i = 1; _i < arguments.length; _i++) {
                interpolateParams[_i - 1] = arguments[_i];
            }
            return this.store.selectSnapshot(ConfigState.getLocalization.apply(ConfigState, __spread([value], interpolateParams.reduce((/**
             * @param {?} acc
             * @param {?} val
             * @return {?}
             */
            function (acc, val) { return (Array.isArray(val) ? __spread(acc, val) : __spread(acc, [val])); }), []))));
        };
        LocalizationPipe.decorators = [
            { type: core.Injectable },
            { type: core.Pipe, args: [{
                        name: 'abpLocalization',
                    },] }
        ];
        /** @nocollapse */
        LocalizationPipe.ctorParameters = function () { return [
            { type: store.Store }
        ]; };
        return LocalizationPipe;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        LocalizationPipe.prototype.store;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/pipes/sort.pipe.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var SortPipe = /** @class */ (function () {
        function SortPipe() {
        }
        /**
         * @param {?} value
         * @param {?=} sortOrder
         * @param {?=} sortKey
         * @return {?}
         */
        SortPipe.prototype.transform = /**
         * @param {?} value
         * @param {?=} sortOrder
         * @param {?=} sortKey
         * @return {?}
         */
        function (value, sortOrder, sortKey) {
            if (sortOrder === void 0) { sortOrder = 'asc'; }
            sortOrder = sortOrder && ((/** @type {?} */ (sortOrder.toLowerCase())));
            if (!value || (sortOrder !== 'asc' && sortOrder !== 'desc'))
                return value;
            /** @type {?} */
            var numberArray = [];
            /** @type {?} */
            var stringArray = [];
            if (!sortKey) {
                numberArray = value.filter((/**
                 * @param {?} item
                 * @return {?}
                 */
                function (item) { return typeof item === 'number'; })).sort();
                stringArray = value.filter((/**
                 * @param {?} item
                 * @return {?}
                 */
                function (item) { return typeof item === 'string'; })).sort();
            }
            else {
                numberArray = value.filter((/**
                 * @param {?} item
                 * @return {?}
                 */
                function (item) { return typeof item[sortKey] === 'number'; })).sort((/**
                 * @param {?} a
                 * @param {?} b
                 * @return {?}
                 */
                function (a, b) { return a[sortKey] - b[sortKey]; }));
                stringArray = value
                    .filter((/**
                 * @param {?} item
                 * @return {?}
                 */
                function (item) { return typeof item[sortKey] === 'string'; }))
                    .sort((/**
                 * @param {?} a
                 * @param {?} b
                 * @return {?}
                 */
                function (a, b) {
                    if (a[sortKey] < b[sortKey])
                        return -1;
                    else if (a[sortKey] > b[sortKey])
                        return 1;
                    else
                        return 0;
                }));
            }
            /** @type {?} */
            var sorted = numberArray.concat(stringArray);
            return sortOrder === 'asc' ? sorted : sorted.reverse();
        };
        SortPipe.decorators = [
            { type: core.Injectable },
            { type: core.Pipe, args: [{
                        name: 'abpSort',
                    },] }
        ];
        return SortPipe;
    }());

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/pipes/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/plugins/config.plugin.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /** @type {?} */
    var NGXS_CONFIG_PLUGIN_OPTIONS = new core.InjectionToken('NGXS_CONFIG_PLUGIN_OPTIONS');
    var ConfigPlugin = /** @class */ (function () {
        function ConfigPlugin(options, router) {
            this.options = options;
            this.router = router;
            this.initialized = false;
        }
        /**
         * @param {?} state
         * @param {?} event
         * @param {?} next
         * @return {?}
         */
        ConfigPlugin.prototype.handle = /**
         * @param {?} state
         * @param {?} event
         * @param {?} next
         * @return {?}
         */
        function (state, event, next) {
            /** @type {?} */
            var matches = store.actionMatcher(event);
            /** @type {?} */
            var isInitAction = matches(store.InitState) || matches(store.UpdateState);
            if (isInitAction && !this.initialized) {
                /** @type {?} */
                var transformedRoutes = transformRoutes(this.router.config);
                var routes = transformedRoutes.routes;
                var wrappers = transformedRoutes.wrappers;
                routes = organizeRoutes(routes, wrappers);
                /** @type {?} */
                var flattedRoutes = flatRoutes(clone(routes));
                state = store.setValue(state, 'ConfigState', __assign({}, (state.ConfigState && __assign({}, state.ConfigState)), this.options, { routes: routes,
                    flattedRoutes: flattedRoutes }));
                this.initialized = true;
            }
            return next(state, event);
        };
        ConfigPlugin.decorators = [
            { type: core.Injectable }
        ];
        /** @nocollapse */
        ConfigPlugin.ctorParameters = function () { return [
            { type: undefined, decorators: [{ type: core.Inject, args: [NGXS_CONFIG_PLUGIN_OPTIONS,] }] },
            { type: router.Router }
        ]; };
        return ConfigPlugin;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        ConfigPlugin.prototype.initialized;
        /**
         * @type {?}
         * @private
         */
        ConfigPlugin.prototype.options;
        /**
         * @type {?}
         * @private
         */
        ConfigPlugin.prototype.router;
    }
    /**
     * @param {?=} routes
     * @param {?=} wrappers
     * @return {?}
     */
    function transformRoutes(routes, wrappers) {
        if (routes === void 0) { routes = []; }
        if (wrappers === void 0) { wrappers = []; }
        // TODO: remove in v1
        /** @type {?} */
        var oldAbpRoutes = routes
            .filter((/**
         * @param {?} route
         * @return {?}
         */
        function (route) {
            return snq((/**
             * @return {?}
             */
            function () { return route.data.routes.routes.find((/**
             * @param {?} r
             * @return {?}
             */
            function (r) { return r.path === route.path; })); }), false);
        }))
            .reduce((/**
         * @param {?} acc
         * @param {?} val
         * @return {?}
         */
        function (acc, val) { return __spread(acc, val.data.routes.routes); }), []);
        // tslint:disable-next-line: deprecation
        /** @type {?} */
        var abpRoutes = __spread(getAbpRoutes(), oldAbpRoutes);
        wrappers = abpRoutes.filter((/**
         * @param {?} ar
         * @return {?}
         */
        function (ar) { return ar.wrapper; }));
        /** @type {?} */
        var transformed = (/** @type {?} */ ([]));
        routes
            .filter((/**
         * @param {?} route
         * @return {?}
         */
        function (route) { return route.component || route.loadChildren; }))
            .forEach((/**
         * @param {?} route
         * @return {?}
         */
        function (route) {
            /** @type {?} */
            var abpPackage = abpRoutes.find((/**
             * @param {?} abp
             * @return {?}
             */
            function (abp) { return abp.path.toLowerCase() === route.path.toLowerCase() && !abp.wrapper; }));
            var length = transformed.length;
            if (abpPackage) {
                transformed.push(abpPackage);
            }
            if (transformed.length === length && (route.data || {}).routes) {
                transformed.push((/** @type {?} */ (__assign({}, route.data.routes, { path: route.path, name: snq((/**
                     * @return {?}
                     */
                    function () { return route.data.routes.name; }), route.path), children: route.data.routes.children || [] }))));
            }
        }));
        return { routes: setUrls(transformed), wrappers: wrappers };
    }
    /**
     * @param {?} routes
     * @param {?=} parentUrl
     * @return {?}
     */
    function setUrls(routes, parentUrl) {
        if (parentUrl) {
            // this if block using for only recursive call
            return routes.map((/**
             * @param {?} route
             * @return {?}
             */
            function (route) { return (__assign({}, route, { url: parentUrl + "/" + route.path }, (route.children &&
                route.children.length && {
                children: setUrls(route.children, parentUrl + "/" + route.path),
            }))); }));
        }
        return routes.map((/**
         * @param {?} route
         * @return {?}
         */
        function (route) { return (__assign({}, route, { url: "/" + route.path }, (route.children &&
            route.children.length && {
            children: setUrls(route.children, "/" + route.path),
        }))); }));
    }
    /**
     * @param {?} routes
     * @return {?}
     */
    function flatRoutes(routes) {
        /** @type {?} */
        var flat = (/**
         * @param {?} r
         * @return {?}
         */
        function (r) {
            return r.reduce((/**
             * @param {?} acc
             * @param {?} val
             * @return {?}
             */
            function (acc, val) {
                /** @type {?} */
                var value = [val];
                if (val.children) {
                    value = __spread([val], flat(val.children));
                }
                return __spread(acc, value);
            }), []);
        });
        return flat(routes);
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/plugins/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/config-state.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ConfigStateService = /** @class */ (function () {
        function ConfigStateService(store) {
            this.store = store;
        }
        /**
         * @return {?}
         */
        ConfigStateService.prototype.getAll = /**
         * @return {?}
         */
        function () {
            return this.store.selectSnapshot(ConfigState.getAll);
        };
        /**
         * @return {?}
         */
        ConfigStateService.prototype.getApplicationInfo = /**
         * @return {?}
         */
        function () {
            return this.store.selectSnapshot(ConfigState.getApplicationInfo);
        };
        /**
         * @param {...?} args
         * @return {?}
         */
        ConfigStateService.prototype.getOne = /**
         * @param {...?} args
         * @return {?}
         */
        function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
            return this.store.selectSnapshot(ConfigState.getOne.apply(ConfigState, __spread(args)));
        };
        /**
         * @param {...?} args
         * @return {?}
         */
        ConfigStateService.prototype.getDeep = /**
         * @param {...?} args
         * @return {?}
         */
        function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
            return this.store.selectSnapshot(ConfigState.getDeep.apply(ConfigState, __spread(args)));
        };
        /**
         * @param {...?} args
         * @return {?}
         */
        ConfigStateService.prototype.getRoute = /**
         * @param {...?} args
         * @return {?}
         */
        function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
            return this.store.selectSnapshot(ConfigState.getRoute.apply(ConfigState, __spread(args)));
        };
        /**
         * @param {...?} args
         * @return {?}
         */
        ConfigStateService.prototype.getApiUrl = /**
         * @param {...?} args
         * @return {?}
         */
        function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
            return this.store.selectSnapshot(ConfigState.getApiUrl.apply(ConfigState, __spread(args)));
        };
        /**
         * @param {...?} args
         * @return {?}
         */
        ConfigStateService.prototype.getSetting = /**
         * @param {...?} args
         * @return {?}
         */
        function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
            return this.store.selectSnapshot(ConfigState.getSetting.apply(ConfigState, __spread(args)));
        };
        /**
         * @param {...?} args
         * @return {?}
         */
        ConfigStateService.prototype.getSettings = /**
         * @param {...?} args
         * @return {?}
         */
        function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
            return this.store.selectSnapshot(ConfigState.getSettings.apply(ConfigState, __spread(args)));
        };
        /**
         * @param {...?} args
         * @return {?}
         */
        ConfigStateService.prototype.getGrantedPolicy = /**
         * @param {...?} args
         * @return {?}
         */
        function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
            return this.store.selectSnapshot(ConfigState.getGrantedPolicy.apply(ConfigState, __spread(args)));
        };
        /**
         * @param {...?} args
         * @return {?}
         */
        ConfigStateService.prototype.getLocalization = /**
         * @param {...?} args
         * @return {?}
         */
        function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
            return this.store.selectSnapshot(ConfigState.getLocalization.apply(ConfigState, __spread(args)));
        };
        ConfigStateService.decorators = [
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        ConfigStateService.ctorParameters = function () { return [
            { type: store.Store }
        ]; };
        /** @nocollapse */ ConfigStateService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function ConfigStateService_Factory() { return new ConfigStateService(core.ɵɵinject(store.Store)); }, token: ConfigStateService, providedIn: "root" });
        return ConfigStateService;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        ConfigStateService.prototype.store;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/lazy-load.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var LazyLoadService = /** @class */ (function () {
        function LazyLoadService() {
            this.loadedLibraries = {};
        }
        /**
         * @param {?} urlOrUrls
         * @param {?} type
         * @param {?=} content
         * @param {?=} targetQuery
         * @param {?=} position
         * @return {?}
         */
        LazyLoadService.prototype.load = /**
         * @param {?} urlOrUrls
         * @param {?} type
         * @param {?=} content
         * @param {?=} targetQuery
         * @param {?=} position
         * @return {?}
         */
        function (urlOrUrls, type, content, targetQuery, position) {
            var _this = this;
            if (content === void 0) { content = ''; }
            if (targetQuery === void 0) { targetQuery = 'body'; }
            if (position === void 0) { position = 'afterend'; }
            if (!urlOrUrls && !content) {
                return rxjs.throwError('Should pass url or content');
            }
            else if (!urlOrUrls && content) {
                urlOrUrls = [null];
            }
            if (!Array.isArray(urlOrUrls)) {
                urlOrUrls = [urlOrUrls];
            }
            return new rxjs.Observable((/**
             * @param {?} subscriber
             * @return {?}
             */
            function (subscriber) {
                ((/** @type {?} */ (urlOrUrls))).forEach((/**
                 * @param {?} url
                 * @param {?} index
                 * @return {?}
                 */
                function (url, index) {
                    /** @type {?} */
                    var key = url ? url.slice(url.lastIndexOf('/') + 1) : uuid();
                    if (_this.loadedLibraries[key]) {
                        subscriber.next();
                        subscriber.complete();
                        return;
                    }
                    _this.loadedLibraries[key] = new rxjs.ReplaySubject();
                    /** @type {?} */
                    var library;
                    if (type === 'script') {
                        library = document.createElement('script');
                        library.type = 'text/javascript';
                        if (url) {
                            ((/** @type {?} */ (library))).src = url;
                        }
                        ((/** @type {?} */ (library))).text = content;
                    }
                    else if (url) {
                        library = document.createElement('link');
                        library.type = 'text/css';
                        ((/** @type {?} */ (library))).rel = 'stylesheet';
                        if (url) {
                            ((/** @type {?} */ (library))).href = url;
                        }
                    }
                    else {
                        library = document.createElement('style');
                        ((/** @type {?} */ (library))).textContent = content;
                    }
                    library.onload = (/**
                     * @return {?}
                     */
                    function () {
                        _this.loadedLibraries[key].next();
                        _this.loadedLibraries[key].complete();
                        if (index === urlOrUrls.length - 1) {
                            subscriber.next();
                            subscriber.complete();
                        }
                    });
                    document.querySelector(targetQuery).insertAdjacentElement(position, library);
                }));
            }));
        };
        LazyLoadService.decorators = [
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */ LazyLoadService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function LazyLoadService_Factory() { return new LazyLoadService(); }, token: LazyLoadService, providedIn: "root" });
        return LazyLoadService;
    }());
    if (false) {
        /** @type {?} */
        LazyLoadService.prototype.loadedLibraries;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/profile-state.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ProfileStateService = /** @class */ (function () {
        function ProfileStateService(store) {
            this.store = store;
        }
        /**
         * @return {?}
         */
        ProfileStateService.prototype.getProfile = /**
         * @return {?}
         */
        function () {
            return this.store.selectSnapshot(ProfileState.getProfile);
        };
        ProfileStateService.decorators = [
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        ProfileStateService.ctorParameters = function () { return [
            { type: store.Store }
        ]; };
        /** @nocollapse */ ProfileStateService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function ProfileStateService_Factory() { return new ProfileStateService(core.ɵɵinject(store.Store)); }, token: ProfileStateService, providedIn: "root" });
        return ProfileStateService;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        ProfileStateService.prototype.store;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/session-state.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var SessionStateService = /** @class */ (function () {
        function SessionStateService(store) {
            this.store = store;
        }
        /**
         * @return {?}
         */
        SessionStateService.prototype.getLanguage = /**
         * @return {?}
         */
        function () {
            return this.store.selectSnapshot(SessionState.getLanguage);
        };
        /**
         * @return {?}
         */
        SessionStateService.prototype.getTenant = /**
         * @return {?}
         */
        function () {
            return this.store.selectSnapshot(SessionState.getTenant);
        };
        SessionStateService.decorators = [
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        SessionStateService.ctorParameters = function () { return [
            { type: store.Store }
        ]; };
        /** @nocollapse */ SessionStateService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function SessionStateService_Factory() { return new SessionStateService(core.ɵɵinject(store.Store)); }, token: SessionStateService, providedIn: "root" });
        return SessionStateService;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        SessionStateService.prototype.store;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/tokens/common.token.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @param {?} environment
     * @return {?}
     */
    function environmentFactory(environment) {
        return __assign({}, environment);
    }
    /**
     * @param {?} config
     * @return {?}
     */
    function configFactory(config) {
        return __assign({}, config);
    }
    /** @type {?} */
    var ENVIRONMENT = new core.InjectionToken('ENVIRONMENT');
    /** @type {?} */
    var CONFIG = new core.InjectionToken('CONFIG');

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/tokens/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/directives/debounce.directive.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var InputEventDebounceDirective = /** @class */ (function () {
        function InputEventDebounceDirective(el) {
            this.el = el;
            this.debounce = 300;
            this.debounceEvent = new core.EventEmitter();
        }
        /**
         * @return {?}
         */
        InputEventDebounceDirective.prototype.ngOnInit = /**
         * @return {?}
         */
        function () {
            var _this = this;
            rxjs.fromEvent(this.el.nativeElement, 'input')
                .pipe(operators.debounceTime(this.debounce), core$1.takeUntilDestroy(this))
                .subscribe((/**
             * @param {?} event
             * @return {?}
             */
            function (event) {
                _this.debounceEvent.emit(event);
            }));
        };
        /**
         * @return {?}
         */
        InputEventDebounceDirective.prototype.ngOnDestroy = /**
         * @return {?}
         */
        function () { };
        InputEventDebounceDirective.decorators = [
            { type: core.Directive, args: [{
                        // tslint:disable-next-line: directive-selector
                        selector: '[input.debounce]',
                    },] }
        ];
        /** @nocollapse */
        InputEventDebounceDirective.ctorParameters = function () { return [
            { type: core.ElementRef }
        ]; };
        InputEventDebounceDirective.propDecorators = {
            debounce: [{ type: core.Input }],
            debounceEvent: [{ type: core.Output, args: ['input.debounce',] }]
        };
        return InputEventDebounceDirective;
    }());
    if (false) {
        /** @type {?} */
        InputEventDebounceDirective.prototype.debounce;
        /** @type {?} */
        InputEventDebounceDirective.prototype.debounceEvent;
        /**
         * @type {?}
         * @private
         */
        InputEventDebounceDirective.prototype.el;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/directives/stop-propagation.directive.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ClickEventStopPropagationDirective = /** @class */ (function () {
        function ClickEventStopPropagationDirective(el) {
            this.el = el;
            this.stopPropEvent = new core.EventEmitter();
        }
        /**
         * @return {?}
         */
        ClickEventStopPropagationDirective.prototype.ngOnInit = /**
         * @return {?}
         */
        function () {
            var _this = this;
            rxjs.fromEvent(this.el.nativeElement, 'click')
                .pipe(core$1.takeUntilDestroy(this))
                .subscribe((/**
             * @param {?} event
             * @return {?}
             */
            function (event) {
                event.stopPropagation();
                _this.stopPropEvent.emit(event);
            }));
        };
        /**
         * @return {?}
         */
        ClickEventStopPropagationDirective.prototype.ngOnDestroy = /**
         * @return {?}
         */
        function () { };
        ClickEventStopPropagationDirective.decorators = [
            { type: core.Directive, args: [{
                        // tslint:disable-next-line: directive-selector
                        selector: '[click.stop]',
                    },] }
        ];
        /** @nocollapse */
        ClickEventStopPropagationDirective.ctorParameters = function () { return [
            { type: core.ElementRef }
        ]; };
        ClickEventStopPropagationDirective.propDecorators = {
            stopPropEvent: [{ type: core.Output, args: ['click.stop',] }]
        };
        return ClickEventStopPropagationDirective;
    }());
    if (false) {
        /** @type {?} */
        ClickEventStopPropagationDirective.prototype.stopPropEvent;
        /**
         * @type {?}
         * @private
         */
        ClickEventStopPropagationDirective.prototype.el;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/providers/locale.provider.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var LocaleId = /** @class */ (function (_super) {
        __extends(LocaleId, _super);
        function LocaleId(localizationService) {
            var _this = _super.call(this) || this;
            _this.localizationService = localizationService;
            return _this;
        }
        /**
         * @return {?}
         */
        LocaleId.prototype.toString = /**
         * @return {?}
         */
        function () {
            var currentLang = this.localizationService.currentLang;
            return localesMapping[currentLang] || currentLang;
        };
        /**
         * @return {?}
         */
        LocaleId.prototype.valueOf = /**
         * @return {?}
         */
        function () {
            return this.toString();
        };
        return LocaleId;
    }(String));
    if (false) {
        /**
         * @type {?}
         * @private
         */
        LocaleId.prototype.localizationService;
    }
    /** @type {?} */
    var LocaleProvider = {
        provide: core.LOCALE_ID,
        useClass: LocaleId,
        deps: [LocalizationService],
    };

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/core.module.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var CoreModule = /** @class */ (function () {
        function CoreModule() {
        }
        /**
         * @param {?=} options
         * @return {?}
         */
        CoreModule.forRoot = /**
         * @param {?=} options
         * @return {?}
         */
        function (options) {
            if (options === void 0) { options = (/** @type {?} */ ({})); }
            return {
                ngModule: CoreModule,
                providers: [
                    LocaleProvider,
                    {
                        provide: store.NGXS_PLUGINS,
                        useClass: ConfigPlugin,
                        multi: true,
                    },
                    {
                        provide: NGXS_CONFIG_PLUGIN_OPTIONS,
                        useValue: options,
                    },
                    {
                        provide: http.HTTP_INTERCEPTORS,
                        useClass: ApiInterceptor,
                        multi: true,
                    },
                    {
                        provide: core.APP_INITIALIZER,
                        multi: true,
                        deps: [core.Injector],
                        useFactory: getInitialData,
                    },
                    {
                        provide: core.APP_INITIALIZER,
                        multi: true,
                        deps: [core.Injector],
                        useFactory: localeInitializer,
                    },
                ],
            };
        };
        CoreModule.decorators = [
            { type: core.NgModule, args: [{
                        imports: [
                            store.NgxsModule.forFeature([ProfileState, SessionState, ConfigState]),
                            routerPlugin.NgxsRouterPluginModule.forRoot(),
                            storagePlugin.NgxsStoragePluginModule.forRoot({ key: ['SessionState'] }),
                            angularOauth2Oidc.OAuthModule.forRoot(),
                            common.CommonModule,
                            http.HttpClientModule,
                            forms.FormsModule,
                            forms.ReactiveFormsModule,
                            router.RouterModule,
                        ],
                        declarations: [
                            RouterOutletComponent,
                            DynamicLayoutComponent,
                            AutofocusDirective,
                            EllipsisDirective,
                            ForDirective,
                            FormSubmitDirective,
                            LocalizationPipe,
                            SortPipe,
                            PermissionDirective,
                            VisibilityDirective,
                            InputEventDebounceDirective,
                            ClickEventStopPropagationDirective,
                            AbstractNgModelComponent,
                        ],
                        exports: [
                            common.CommonModule,
                            http.HttpClientModule,
                            forms.FormsModule,
                            forms.ReactiveFormsModule,
                            router.RouterModule,
                            RouterOutletComponent,
                            DynamicLayoutComponent,
                            AutofocusDirective,
                            EllipsisDirective,
                            ForDirective,
                            FormSubmitDirective,
                            LocalizationPipe,
                            SortPipe,
                            PermissionDirective,
                            VisibilityDirective,
                            InputEventDebounceDirective,
                            LocalizationPipe,
                            ClickEventStopPropagationDirective,
                            AbstractNgModelComponent,
                        ],
                        providers: [LocalizationPipe],
                        entryComponents: [RouterOutletComponent, DynamicLayoutComponent],
                    },] }
        ];
        return CoreModule;
    }());

    exports.AbstractNgModelComponent = AbstractNgModelComponent;
    exports.ApiInterceptor = ApiInterceptor;
    exports.ApplicationConfigurationService = ApplicationConfigurationService;
    exports.AuthGuard = AuthGuard;
    exports.AutofocusDirective = AutofocusDirective;
    exports.CONFIG = CONFIG;
    exports.ChangePassword = ChangePassword;
    exports.ConfigPlugin = ConfigPlugin;
    exports.ConfigState = ConfigState;
    exports.ConfigStateService = ConfigStateService;
    exports.CoreModule = CoreModule;
    exports.DynamicLayoutComponent = DynamicLayoutComponent;
    exports.ENVIRONMENT = ENVIRONMENT;
    exports.EllipsisDirective = EllipsisDirective;
    exports.ForDirective = ForDirective;
    exports.FormSubmitDirective = FormSubmitDirective;
    exports.GetAppConfiguration = GetAppConfiguration;
    exports.GetProfile = GetProfile;
    exports.LazyLoadService = LazyLoadService;
    exports.LocalizationPipe = LocalizationPipe;
    exports.LocalizationService = LocalizationService;
    exports.NGXS_CONFIG_PLUGIN_OPTIONS = NGXS_CONFIG_PLUGIN_OPTIONS;
    exports.PatchRouteByName = PatchRouteByName;
    exports.PermissionDirective = PermissionDirective;
    exports.PermissionGuard = PermissionGuard;
    exports.ProfileService = ProfileService;
    exports.ProfileState = ProfileState;
    exports.ProfileStateService = ProfileStateService;
    exports.RestOccurError = RestOccurError;
    exports.RestService = RestService;
    exports.RouterOutletComponent = RouterOutletComponent;
    exports.SessionState = SessionState;
    exports.SessionStateService = SessionStateService;
    exports.SetLanguage = SetLanguage;
    exports.SetTenant = SetTenant;
    exports.SortPipe = SortPipe;
    exports.StartLoader = StartLoader;
    exports.StopLoader = StopLoader;
    exports.UpdateProfile = UpdateProfile;
    exports.VisibilityDirective = VisibilityDirective;
    exports.addAbpRoutes = addAbpRoutes;
    exports.configFactory = configFactory;
    exports.environmentFactory = environmentFactory;
    exports.getAbpRoutes = getAbpRoutes;
    exports.getInitialData = getInitialData;
    exports.localeInitializer = localeInitializer;
    exports.noop = noop;
    exports.organizeRoutes = organizeRoutes;
    exports.registerLocale = registerLocale;
    exports.setChildRoute = setChildRoute;
    exports.sortRoutes = sortRoutes;
    exports.takeUntilDestroy = takeUntilDestroy;
    exports.uuid = uuid;
    exports.ɵa = ProfileState;
    exports.ɵb = ProfileService;
    exports.ɵba = InputEventDebounceDirective;
    exports.ɵbb = ClickEventStopPropagationDirective;
    exports.ɵbc = AbstractNgModelComponent;
    exports.ɵbd = LocaleId;
    exports.ɵbe = LocaleProvider;
    exports.ɵbf = NGXS_CONFIG_PLUGIN_OPTIONS;
    exports.ɵbg = ConfigPlugin;
    exports.ɵbh = ApiInterceptor;
    exports.ɵbi = getInitialData;
    exports.ɵbj = localeInitializer;
    exports.ɵc = RestService;
    exports.ɵd = GetProfile;
    exports.ɵe = UpdateProfile;
    exports.ɵf = ChangePassword;
    exports.ɵh = SessionState;
    exports.ɵi = LocalizationService;
    exports.ɵj = SetLanguage;
    exports.ɵk = SetTenant;
    exports.ɵm = ConfigState;
    exports.ɵn = ApplicationConfigurationService;
    exports.ɵo = PatchRouteByName;
    exports.ɵp = GetAppConfiguration;
    exports.ɵq = RouterOutletComponent;
    exports.ɵr = DynamicLayoutComponent;
    exports.ɵs = AutofocusDirective;
    exports.ɵt = EllipsisDirective;
    exports.ɵu = ForDirective;
    exports.ɵv = FormSubmitDirective;
    exports.ɵw = LocalizationPipe;
    exports.ɵx = SortPipe;
    exports.ɵy = PermissionDirective;
    exports.ɵz = VisibilityDirective;

    Object.defineProperty(exports, '__esModule', { value: true });

})));
//# sourceMappingURL=abp-ng.core.umd.js.map
