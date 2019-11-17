(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@abp/ng.core'), require('@abp/ng.theme.shared'), require('@angular/core'), require('@ng-bootstrap/ng-bootstrap'), require('@ngxs/store'), require('primeng/table'), require('@angular/forms'), require('rxjs'), require('rxjs/operators'), require('@angular/router'), require('@abp/ng.feature-management'), require('@ngx-validate/core')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.tenant-management', ['exports', '@abp/ng.core', '@abp/ng.theme.shared', '@angular/core', '@ng-bootstrap/ng-bootstrap', '@ngxs/store', 'primeng/table', '@angular/forms', 'rxjs', 'rxjs/operators', '@angular/router', '@abp/ng.feature-management', '@ngx-validate/core'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng['tenant-management'] = {}), global.ng_core, global.ng_theme_shared, global.ng.core, global.ngBootstrap, global.store, global.table, global.ng.forms, global.rxjs, global.rxjs.operators, global.ng.router, global.ng_featureManagement, global.core$1));
}(this, (function (exports, ng_core, ng_theme_shared, core, ngBootstrap, store, table, forms, rxjs, operators, router, ng_featureManagement, core$1) { 'use strict';

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
     * Generated from: lib/actions/tenant-management.actions.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var GetTenants = /** @class */ (function () {
        function GetTenants(payload) {
            this.payload = payload;
        }
        GetTenants.type = '[TenantManagement] Get Tenant';
        return GetTenants;
    }());
    if (false) {
        /** @type {?} */
        GetTenants.type;
        /** @type {?} */
        GetTenants.prototype.payload;
    }
    var GetTenantById = /** @class */ (function () {
        function GetTenantById(payload) {
            this.payload = payload;
        }
        GetTenantById.type = '[TenantManagement] Get Tenant By Id';
        return GetTenantById;
    }());
    if (false) {
        /** @type {?} */
        GetTenantById.type;
        /** @type {?} */
        GetTenantById.prototype.payload;
    }
    var CreateTenant = /** @class */ (function () {
        function CreateTenant(payload) {
            this.payload = payload;
        }
        CreateTenant.type = '[TenantManagement] Create Tenant';
        return CreateTenant;
    }());
    if (false) {
        /** @type {?} */
        CreateTenant.type;
        /** @type {?} */
        CreateTenant.prototype.payload;
    }
    var UpdateTenant = /** @class */ (function () {
        function UpdateTenant(payload) {
            this.payload = payload;
        }
        UpdateTenant.type = '[TenantManagement] Update Tenant';
        return UpdateTenant;
    }());
    if (false) {
        /** @type {?} */
        UpdateTenant.type;
        /** @type {?} */
        UpdateTenant.prototype.payload;
    }
    var DeleteTenant = /** @class */ (function () {
        function DeleteTenant(payload) {
            this.payload = payload;
        }
        DeleteTenant.type = '[TenantManagement] Delete Tenant';
        return DeleteTenant;
    }());
    if (false) {
        /** @type {?} */
        DeleteTenant.type;
        /** @type {?} */
        DeleteTenant.prototype.payload;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/tenant-management.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var TenantManagementService = /** @class */ (function () {
        function TenantManagementService(rest) {
            this.rest = rest;
        }
        /**
         * @param {?=} params
         * @return {?}
         */
        TenantManagementService.prototype.getTenant = /**
         * @param {?=} params
         * @return {?}
         */
        function (params) {
            if (params === void 0) { params = (/** @type {?} */ ({})); }
            /** @type {?} */
            var request = {
                method: 'GET',
                url: '/api/multi-tenancy/tenants',
                params: params
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} id
         * @return {?}
         */
        TenantManagementService.prototype.getTenantById = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            /** @type {?} */
            var request = {
                method: 'GET',
                url: "/api/multi-tenancy/tenants/" + id
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} id
         * @return {?}
         */
        TenantManagementService.prototype.deleteTenant = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            /** @type {?} */
            var request = {
                method: 'DELETE',
                url: "/api/multi-tenancy/tenants/" + id
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} body
         * @return {?}
         */
        TenantManagementService.prototype.createTenant = /**
         * @param {?} body
         * @return {?}
         */
        function (body) {
            /** @type {?} */
            var request = {
                method: 'POST',
                url: '/api/multi-tenancy/tenants',
                body: body
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} body
         * @return {?}
         */
        TenantManagementService.prototype.updateTenant = /**
         * @param {?} body
         * @return {?}
         */
        function (body) {
            /** @type {?} */
            var url = "/api/multi-tenancy/tenants/" + body.id;
            delete body.id;
            /** @type {?} */
            var request = {
                method: 'PUT',
                url: url,
                body: body
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} id
         * @return {?}
         */
        TenantManagementService.prototype.getDefaultConnectionString = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            /** @type {?} */
            var url = "/api/multi-tenancy/tenants/" + id + "/default-connection-string";
            /** @type {?} */
            var request = {
                method: 'GET',
                responseType: "text" /* Text */,
                url: url
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} payload
         * @return {?}
         */
        TenantManagementService.prototype.updateDefaultConnectionString = /**
         * @param {?} payload
         * @return {?}
         */
        function (payload) {
            /** @type {?} */
            var url = "/api/multi-tenancy/tenants/" + payload.id + "/default-connection-string";
            /** @type {?} */
            var request = {
                method: 'PUT',
                url: url,
                params: { defaultConnectionString: payload.defaultConnectionString }
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} id
         * @return {?}
         */
        TenantManagementService.prototype.deleteDefaultConnectionString = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            /** @type {?} */
            var url = "/api/multi-tenancy/tenants/" + id + "/default-connection-string";
            /** @type {?} */
            var request = {
                method: 'DELETE',
                url: url
            };
            return this.rest.request(request);
        };
        TenantManagementService.decorators = [
            { type: core.Injectable, args: [{
                        providedIn: 'root'
                    },] }
        ];
        /** @nocollapse */
        TenantManagementService.ctorParameters = function () { return [
            { type: ng_core.RestService }
        ]; };
        /** @nocollapse */ TenantManagementService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function TenantManagementService_Factory() { return new TenantManagementService(core.ɵɵinject(ng_core.RestService)); }, token: TenantManagementService, providedIn: "root" });
        return TenantManagementService;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        TenantManagementService.prototype.rest;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/states/tenant-management.state.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var TenantManagementState = /** @class */ (function () {
        function TenantManagementState(tenantManagementService) {
            this.tenantManagementService = tenantManagementService;
        }
        /**
         * @param {?} __0
         * @return {?}
         */
        TenantManagementState.get = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var result = _a.result;
            return result.items || [];
        };
        /**
         * @param {?} __0
         * @return {?}
         */
        TenantManagementState.getTenantsTotalCount = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var result = _a.result;
            return result.totalCount;
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        TenantManagementState.prototype.get = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var patchState = _a.patchState;
            var payload = _b.payload;
            return this.tenantManagementService.getTenant(payload).pipe(operators.tap((/**
             * @param {?} result
             * @return {?}
             */
            function (result) {
                return patchState({
                    result: result,
                });
            })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        TenantManagementState.prototype.getById = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var patchState = _a.patchState;
            var payload = _b.payload;
            return this.tenantManagementService.getTenantById(payload).pipe(operators.tap((/**
             * @param {?} selectedItem
             * @return {?}
             */
            function (selectedItem) {
                return patchState({
                    selectedItem: selectedItem,
                });
            })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        TenantManagementState.prototype.delete = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var dispatch = _a.dispatch;
            var payload = _b.payload;
            return this.tenantManagementService.deleteTenant(payload).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new GetTenants()); })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        TenantManagementState.prototype.add = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var dispatch = _a.dispatch;
            var payload = _b.payload;
            return this.tenantManagementService.createTenant(payload).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new GetTenants()); })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        TenantManagementState.prototype.update = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var dispatch = _a.dispatch, getState = _a.getState;
            var payload = _b.payload;
            return this.tenantManagementService
                .updateTenant(__assign({}, getState().selectedItem, payload))
                .pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new GetTenants()); })));
        };
        TenantManagementState.ctorParameters = function () { return [
            { type: TenantManagementService }
        ]; };
        __decorate([
            store.Action(GetTenants),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, GetTenants]),
            __metadata("design:returntype", void 0)
        ], TenantManagementState.prototype, "get", null);
        __decorate([
            store.Action(GetTenantById),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, GetTenantById]),
            __metadata("design:returntype", void 0)
        ], TenantManagementState.prototype, "getById", null);
        __decorate([
            store.Action(DeleteTenant),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, DeleteTenant]),
            __metadata("design:returntype", void 0)
        ], TenantManagementState.prototype, "delete", null);
        __decorate([
            store.Action(CreateTenant),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, CreateTenant]),
            __metadata("design:returntype", void 0)
        ], TenantManagementState.prototype, "add", null);
        __decorate([
            store.Action(UpdateTenant),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, UpdateTenant]),
            __metadata("design:returntype", void 0)
        ], TenantManagementState.prototype, "update", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", Array)
        ], TenantManagementState, "get", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", Number)
        ], TenantManagementState, "getTenantsTotalCount", null);
        TenantManagementState = __decorate([
            store.State({
                name: 'TenantManagementState',
                defaults: (/** @type {?} */ ({ result: {}, selectedItem: {} })),
            }),
            __metadata("design:paramtypes", [TenantManagementService])
        ], TenantManagementState);
        return TenantManagementState;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        TenantManagementState.prototype.tenantManagementService;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/tenants/tenants.component.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     * @record
     */
    function SelectedModalContent() { }
    if (false) {
        /** @type {?} */
        SelectedModalContent.prototype.type;
        /** @type {?} */
        SelectedModalContent.prototype.title;
        /** @type {?} */
        SelectedModalContent.prototype.template;
    }
    var TenantsComponent = /** @class */ (function () {
        function TenantsComponent(confirmationService, tenantService, fb, store) {
            this.confirmationService = confirmationService;
            this.tenantService = tenantService;
            this.fb = fb;
            this.store = store;
            this.selectedModalContent = (/** @type {?} */ ({}));
            this.visibleFeatures = false;
            this.pageQuery = {};
            this.loading = false;
            this.modalBusy = false;
            this.sortOrder = '';
            this.sortKey = '';
        }
        Object.defineProperty(TenantsComponent.prototype, "useSharedDatabase", {
            get: /**
             * @return {?}
             */
            function () {
                return this.defaultConnectionStringForm.get('useSharedDatabase').value;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(TenantsComponent.prototype, "connectionString", {
            get: /**
             * @return {?}
             */
            function () {
                return this.defaultConnectionStringForm.get('defaultConnectionString').value;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(TenantsComponent.prototype, "isDisabledSaveButton", {
            get: /**
             * @return {?}
             */
            function () {
                if (!this.selectedModalContent)
                    return false;
                if (this.selectedModalContent.type === 'saveConnStr' && this.defaultConnectionStringForm.invalid) {
                    return true;
                }
                else if (this.selectedModalContent.type === 'saveTenant' && this.tenantForm.invalid) {
                    return true;
                }
                else {
                    return false;
                }
            },
            enumerable: true,
            configurable: true
        });
        /**
         * @return {?}
         */
        TenantsComponent.prototype.ngOnInit = /**
         * @return {?}
         */
        function () {
            this.get();
        };
        /**
         * @param {?} value
         * @return {?}
         */
        TenantsComponent.prototype.onSearch = /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this.pageQuery.filter = value;
            this.get();
        };
        /**
         * @private
         * @return {?}
         */
        TenantsComponent.prototype.createTenantForm = /**
         * @private
         * @return {?}
         */
        function () {
            this.tenantForm = this.fb.group({
                name: [this.selected.name || '', [forms.Validators.required, forms.Validators.maxLength(256)]],
            });
        };
        /**
         * @private
         * @return {?}
         */
        TenantsComponent.prototype.createDefaultConnectionStringForm = /**
         * @private
         * @return {?}
         */
        function () {
            this.defaultConnectionStringForm = this.fb.group({
                useSharedDatabase: this._useSharedDatabase,
                defaultConnectionString: [this.defaultConnectionString || ''],
            });
        };
        /**
         * @param {?} title
         * @param {?} template
         * @param {?} type
         * @return {?}
         */
        TenantsComponent.prototype.openModal = /**
         * @param {?} title
         * @param {?} template
         * @param {?} type
         * @return {?}
         */
        function (title, template, type) {
            this.selectedModalContent = {
                title: title,
                template: template,
                type: type,
            };
            this.isModalVisible = true;
        };
        /**
         * @param {?} id
         * @return {?}
         */
        TenantsComponent.prototype.onEditConnectionString = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            var _this = this;
            this.store
                .dispatch(new GetTenantById(id))
                .pipe(operators.pluck('TenantManagementState', 'selectedItem'), operators.switchMap((/**
             * @param {?} selected
             * @return {?}
             */
            function (selected) {
                _this.selected = selected;
                return _this.tenantService.getDefaultConnectionString(id);
            })))
                .subscribe((/**
             * @param {?} fetchedConnectionString
             * @return {?}
             */
            function (fetchedConnectionString) {
                _this._useSharedDatabase = fetchedConnectionString ? false : true;
                _this.defaultConnectionString = fetchedConnectionString ? fetchedConnectionString : '';
                _this.createDefaultConnectionStringForm();
                _this.openModal('AbpTenantManagement::ConnectionStrings', _this.connectionStringModalTemplate, 'saveConnStr');
            }));
        };
        /**
         * @return {?}
         */
        TenantsComponent.prototype.addTenant = /**
         * @return {?}
         */
        function () {
            this.selected = (/** @type {?} */ ({}));
            this.createTenantForm();
            this.openModal('AbpTenantManagement::NewTenant', this.tenantModalTemplate, 'saveTenant');
        };
        /**
         * @param {?} id
         * @return {?}
         */
        TenantsComponent.prototype.editTenant = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            var _this = this;
            this.store
                .dispatch(new GetTenantById(id))
                .pipe(operators.pluck('TenantManagementState', 'selectedItem'))
                .subscribe((/**
             * @param {?} selected
             * @return {?}
             */
            function (selected) {
                _this.selected = selected;
                _this.createTenantForm();
                _this.openModal('AbpTenantManagement::Edit', _this.tenantModalTemplate, 'saveTenant');
            }));
        };
        /**
         * @return {?}
         */
        TenantsComponent.prototype.save = /**
         * @return {?}
         */
        function () {
            var type = this.selectedModalContent.type;
            if (!type)
                return;
            if (type === 'saveTenant')
                this.saveTenant();
            else if (type === 'saveConnStr')
                this.saveConnectionString();
        };
        /**
         * @return {?}
         */
        TenantsComponent.prototype.saveConnectionString = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (this.modalBusy)
                return;
            this.modalBusy = true;
            if (this.useSharedDatabase || (!this.useSharedDatabase && !this.connectionString)) {
                this.tenantService
                    .deleteDefaultConnectionString(this.selected.id)
                    .pipe(operators.take(1), operators.finalize((/**
                 * @return {?}
                 */
                function () { return (_this.modalBusy = false); })))
                    .subscribe((/**
                 * @return {?}
                 */
                function () {
                    _this.isModalVisible = false;
                }));
            }
            else {
                this.tenantService
                    .updateDefaultConnectionString({ id: this.selected.id, defaultConnectionString: this.connectionString })
                    .pipe(operators.take(1), operators.finalize((/**
                 * @return {?}
                 */
                function () { return (_this.modalBusy = false); })))
                    .subscribe((/**
                 * @return {?}
                 */
                function () {
                    _this.isModalVisible = false;
                }));
            }
        };
        /**
         * @return {?}
         */
        TenantsComponent.prototype.saveTenant = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (!this.tenantForm.valid || this.modalBusy)
                return;
            this.modalBusy = true;
            this.store
                .dispatch(this.selected.id
                ? new UpdateTenant(__assign({}, this.selected, this.tenantForm.value, { id: this.selected.id }))
                : new CreateTenant(this.tenantForm.value))
                .pipe(operators.finalize((/**
             * @return {?}
             */
            function () { return (_this.modalBusy = false); })))
                .subscribe((/**
             * @return {?}
             */
            function () {
                _this.isModalVisible = false;
            }));
        };
        /**
         * @param {?} id
         * @param {?} name
         * @return {?}
         */
        TenantsComponent.prototype.delete = /**
         * @param {?} id
         * @param {?} name
         * @return {?}
         */
        function (id, name) {
            var _this = this;
            this.confirmationService
                .warn('AbpTenantManagement::TenantDeletionConfirmationMessage', 'AbpTenantManagement::AreYouSure', {
                messageLocalizationParams: [name],
            })
                .subscribe((/**
             * @param {?} status
             * @return {?}
             */
            function (status) {
                if (status === "confirm" /* confirm */) {
                    _this.store.dispatch(new DeleteTenant(id));
                }
            }));
        };
        /**
         * @param {?} data
         * @return {?}
         */
        TenantsComponent.prototype.onPageChange = /**
         * @param {?} data
         * @return {?}
         */
        function (data) {
            this.pageQuery.skipCount = data.first;
            this.pageQuery.maxResultCount = data.rows;
            this.get();
        };
        /**
         * @return {?}
         */
        TenantsComponent.prototype.get = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.loading = true;
            this.store
                .dispatch(new GetTenants(this.pageQuery))
                .pipe(operators.finalize((/**
             * @return {?}
             */
            function () { return (_this.loading = false); })))
                .subscribe();
        };
        /**
         * @param {?} value
         * @return {?}
         */
        TenantsComponent.prototype.onSharedDatabaseChange = /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            if (!value) {
                setTimeout((/**
                 * @return {?}
                 */
                function () {
                    /** @type {?} */
                    var defaultConnectionString = (/** @type {?} */ (document.getElementById('defaultConnectionString')));
                    if (defaultConnectionString) {
                        defaultConnectionString.focus();
                    }
                }), 0);
            }
        };
        TenantsComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-tenants',
                        template: "<div id=\"wrapper\" class=\"card\">\r\n  <div class=\"card-header\">\r\n    <div class=\"row\">\r\n      <div class=\"col col-md-6\">\r\n        <h5 class=\"card-title\">{{ 'AbpTenantManagement::Tenants' | abpLocalization }}</h5>\r\n      </div>\r\n      <div class=\"text-right col col-md-6\">\r\n        <button\r\n          [abpPermission]=\"'AbpTenantManagement.Tenants.Create'\"\r\n          id=\"create-tenants\"\r\n          class=\"btn btn-primary\"\r\n          type=\"button\"\r\n          (click)=\"addTenant()\"\r\n        >\r\n          <i class=\"fa fa-plus mr-1\"></i>\r\n          <span>{{ 'AbpTenantManagement::NewTenant' | abpLocalization }}</span>\r\n        </button>\r\n      </div>\r\n    </div>\r\n  </div>\r\n  <div class=\"card-body\">\r\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\r\n      <label\r\n        ><input\r\n          type=\"search\"\r\n          class=\"form-control form-control-sm\"\r\n          [placeholder]=\"'AbpUi::PagerSearch' | abpLocalization\"\r\n          (input.debounce)=\"onSearch($event.target.value)\"\r\n      /></label>\r\n    </div>\r\n    <p-table\r\n      *ngIf=\"[150, 0] as columnWidths\"\r\n      [value]=\"data$ | async\"\r\n      [abpTableSort]=\"{ key: sortKey, order: sortOrder }\"\r\n      [lazy]=\"true\"\r\n      [lazyLoadOnInit]=\"false\"\r\n      [paginator]=\"true\"\r\n      [rows]=\"10\"\r\n      [totalRecords]=\"totalCount$ | async\"\r\n      [loading]=\"loading\"\r\n      [resizableColumns]=\"true\"\r\n      [scrollable]=\"true\"\r\n      (onLazyLoad)=\"onPageChange($event)\"\r\n    >\r\n      <ng-template pTemplate=\"colgroup\">\r\n        <colgroup>\r\n          <col *ngFor=\"let width of columnWidths\" [ngStyle]=\"{ 'width.px': width || undefined }\" />\r\n        </colgroup>\r\n      </ng-template>\r\n      <ng-template pTemplate=\"emptymessage\" let-columns>\r\n        <tr\r\n          abp-table-empty-message\r\n          [attr.colspan]=\"columnWidths.length\"\r\n          localizationResource=\"AbpTenantManagement\"\r\n          localizationProp=\"NoDataAvailableInDatatable\"\r\n        ></tr>\r\n      </ng-template>\r\n      <ng-template pTemplate=\"header\" let-columns>\r\n        <tr>\r\n          <th>{{ 'AbpTenantManagement::Actions' | abpLocalization }}</th>\r\n          <th pResizableColumn (click)=\"sortOrderIcon.sort('name')\">\r\n            {{ 'AbpTenantManagement::TenantName' | abpLocalization }}\r\n            <abp-sort-order-icon #sortOrderIcon key=\"name\" [(selectedKey)]=\"sortKey\" [(order)]=\"sortOrder\">\r\n            </abp-sort-order-icon>\r\n          </th>\r\n        </tr>\r\n      </ng-template>\r\n      <ng-template pTemplate=\"body\" let-data>\r\n        <tr>\r\n          <td class=\"text-center\">\r\n            <div ngbDropdown container=\"body\" class=\"d-inline-block\">\r\n              <button\r\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\r\n                data-toggle=\"dropdown\"\r\n                aria-haspopup=\"true\"\r\n                ngbDropdownToggle\r\n              >\r\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpTenantManagement::Actions' | abpLocalization }}\r\n              </button>\r\n              <div ngbDropdownMenu>\r\n                <button\r\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Update'\"\r\n                  ngbDropdownItem\r\n                  (click)=\"editTenant(data.id)\"\r\n                >\r\n                  {{ 'AbpTenantManagement::Edit' | abpLocalization }}\r\n                </button>\r\n                <button\r\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageConnectionStrings'\"\r\n                  ngbDropdownItem\r\n                  (click)=\"onEditConnectionString(data.id)\"\r\n                >\r\n                  {{ 'AbpTenantManagement::Permission:ManageConnectionStrings' | abpLocalization }}\r\n                </button>\r\n                <button\r\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageFeatures'\"\r\n                  ngbDropdownItem\r\n                  (click)=\"providerKey = data.id; visibleFeatures = true\"\r\n                >\r\n                  {{ 'AbpTenantManagement::Permission:ManageFeatures' | abpLocalization }}\r\n                </button>\r\n                <button\r\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Delete'\"\r\n                  ngbDropdownItem\r\n                  (click)=\"delete(data.id, data.name)\"\r\n                >\r\n                  {{ 'AbpTenantManagement::Delete' | abpLocalization }}\r\n                </button>\r\n              </div>\r\n            </div>\r\n          </td>\r\n          <td>{{ data.name }}</td>\r\n        </tr>\r\n      </ng-template>\r\n    </p-table>\r\n  </div>\r\n</div>\r\n\r\n<abp-modal size=\"md\" [(visible)]=\"isModalVisible\" [busy]=\"modalBusy\">\r\n  <ng-template #abpHeader>\r\n    <h3>{{ selectedModalContent.title | abpLocalization }}</h3>\r\n  </ng-template>\r\n\r\n  <ng-template #abpBody>\r\n    <ng-container *ngTemplateOutlet=\"selectedModalContent?.template\"></ng-container>\r\n  </ng-template>\r\n\r\n  <ng-template #abpFooter>\r\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\r\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\r\n    </button>\r\n    <abp-button iconClass=\"fa fa-check\" (click)=\"save()\" [disabled]=\"isDisabledSaveButton\">{{\r\n      'AbpIdentity::Save' | abpLocalization\r\n    }}</abp-button>\r\n  </ng-template>\r\n</abp-modal>\r\n\r\n<ng-template #tenantModalTemplate>\r\n  <form [formGroup]=\"tenantForm\" (ngSubmit)=\"save()\" validateOnSubmit>\r\n    <div class=\"mt-2\">\r\n      <div class=\"form-group\">\r\n        <label for=\"name\">{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</label>\r\n        <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" autofocus />\r\n      </div>\r\n    </div>\r\n  </form>\r\n</ng-template>\r\n\r\n<ng-template #connectionStringModalTemplate>\r\n  <form [formGroup]=\"defaultConnectionStringForm\" (ngSubmit)=\"save()\" validateOnSubmit>\r\n    <label class=\"mt-2\">\r\n      <div class=\"form-group\">\r\n        <div class=\"custom-checkbox custom-control mb-2\">\r\n          <input\r\n            id=\"useSharedDatabase\"\r\n            type=\"checkbox\"\r\n            class=\"custom-control-input\"\r\n            formControlName=\"useSharedDatabase\"\r\n            autofocus\r\n            (ngModelChange)=\"onSharedDatabaseChange($event)\"\r\n          />\r\n          <label for=\"useSharedDatabase\" class=\"custom-control-label\">{{\r\n            'AbpTenantManagement::DisplayName:UseSharedDatabase' | abpLocalization\r\n          }}</label>\r\n        </div>\r\n      </div>\r\n      <label class=\"form-group\" *ngIf=\"!useSharedDatabase\">\r\n        <label for=\"defaultConnectionString\">{{\r\n          'AbpTenantManagement::DisplayName:DefaultConnectionString' | abpLocalization\r\n        }}</label>\r\n        <input\r\n          type=\"text\"\r\n          id=\"defaultConnectionString\"\r\n          class=\"form-control\"\r\n          formControlName=\"defaultConnectionString\"\r\n        />\r\n      </label>\r\n    </label>\r\n  </form>\r\n</ng-template>\r\n\r\n<abp-feature-management [(visible)]=\"visibleFeatures\" providerName=\"T\" [providerKey]=\"providerKey\">\r\n</abp-feature-management>\r\n"
                    }] }
        ];
        /** @nocollapse */
        TenantsComponent.ctorParameters = function () { return [
            { type: ng_theme_shared.ConfirmationService },
            { type: TenantManagementService },
            { type: forms.FormBuilder },
            { type: store.Store }
        ]; };
        TenantsComponent.propDecorators = {
            tenantModalTemplate: [{ type: core.ViewChild, args: ['tenantModalTemplate', { static: false },] }],
            connectionStringModalTemplate: [{ type: core.ViewChild, args: ['connectionStringModalTemplate', { static: false },] }]
        };
        __decorate([
            store.Select(TenantManagementState.get),
            __metadata("design:type", rxjs.Observable)
        ], TenantsComponent.prototype, "data$", void 0);
        __decorate([
            store.Select(TenantManagementState.getTenantsTotalCount),
            __metadata("design:type", rxjs.Observable)
        ], TenantsComponent.prototype, "totalCount$", void 0);
        return TenantsComponent;
    }());
    if (false) {
        /** @type {?} */
        TenantsComponent.prototype.data$;
        /** @type {?} */
        TenantsComponent.prototype.totalCount$;
        /** @type {?} */
        TenantsComponent.prototype.selected;
        /** @type {?} */
        TenantsComponent.prototype.tenantForm;
        /** @type {?} */
        TenantsComponent.prototype.defaultConnectionStringForm;
        /** @type {?} */
        TenantsComponent.prototype.defaultConnectionString;
        /** @type {?} */
        TenantsComponent.prototype.isModalVisible;
        /** @type {?} */
        TenantsComponent.prototype.selectedModalContent;
        /** @type {?} */
        TenantsComponent.prototype.visibleFeatures;
        /** @type {?} */
        TenantsComponent.prototype.providerKey;
        /** @type {?} */
        TenantsComponent.prototype._useSharedDatabase;
        /** @type {?} */
        TenantsComponent.prototype.pageQuery;
        /** @type {?} */
        TenantsComponent.prototype.loading;
        /** @type {?} */
        TenantsComponent.prototype.modalBusy;
        /** @type {?} */
        TenantsComponent.prototype.sortOrder;
        /** @type {?} */
        TenantsComponent.prototype.sortKey;
        /** @type {?} */
        TenantsComponent.prototype.tenantModalTemplate;
        /** @type {?} */
        TenantsComponent.prototype.connectionStringModalTemplate;
        /**
         * @type {?}
         * @private
         */
        TenantsComponent.prototype.confirmationService;
        /**
         * @type {?}
         * @private
         */
        TenantsComponent.prototype.tenantService;
        /**
         * @type {?}
         * @private
         */
        TenantsComponent.prototype.fb;
        /**
         * @type {?}
         * @private
         */
        TenantsComponent.prototype.store;
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/tenant-management-routing.module.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ɵ0 = { requiredPolicy: 'AbpTenantManagement.Tenants' };
    /** @type {?} */
    var routes = [
        { path: '', redirectTo: 'tenants', pathMatch: 'full' },
        {
            path: 'tenants',
            component: ng_core.DynamicLayoutComponent,
            canActivate: [ng_core.AuthGuard, ng_core.PermissionGuard],
            data: ɵ0,
            children: [{ path: '', component: TenantsComponent }],
        },
    ];
    var TenantManagementRoutingModule = /** @class */ (function () {
        function TenantManagementRoutingModule() {
        }
        TenantManagementRoutingModule.decorators = [
            { type: core.NgModule, args: [{
                        imports: [router.RouterModule.forChild(routes)],
                        exports: [router.RouterModule],
                    },] }
        ];
        return TenantManagementRoutingModule;
    }());

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/tenant-management.module.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var TenantManagementModule = /** @class */ (function () {
        function TenantManagementModule() {
        }
        TenantManagementModule.decorators = [
            { type: core.NgModule, args: [{
                        declarations: [TenantsComponent],
                        imports: [
                            TenantManagementRoutingModule,
                            store.NgxsModule.forFeature([TenantManagementState]),
                            core$1.NgxValidateCoreModule,
                            ng_core.CoreModule,
                            table.TableModule,
                            ng_theme_shared.ThemeSharedModule,
                            ngBootstrap.NgbDropdownModule,
                            ng_featureManagement.FeatureManagementModule,
                        ],
                    },] }
        ];
        return TenantManagementModule;
    }());
    /**
     *
     * @deprecated since version 0.9.0
     * @return {?}
     */
    function TenantManagementProviders() {
        return [];
    }

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/actions/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/components/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/constants/routes.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /**
     *
     * @deprecated since version 0.9.0
     * @type {?}
     */
    var TENANT_MANAGEMENT_ROUTES = {
        routes: (/** @type {?} */ ([
            {
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
            },
        ])),
    };

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/constants/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/tenant-management.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var TenantManagement;
    (function (TenantManagement) {
        /**
         * @record
         */
        function State() { }
        TenantManagement.State = State;
        if (false) {
            /** @type {?} */
            State.prototype.result;
            /** @type {?} */
            State.prototype.selectedItem;
        }
        /**
         * @record
         */
        function Item() { }
        TenantManagement.Item = Item;
        if (false) {
            /** @type {?} */
            Item.prototype.id;
            /** @type {?} */
            Item.prototype.name;
        }
        /**
         * @record
         */
        function AddRequest() { }
        TenantManagement.AddRequest = AddRequest;
        if (false) {
            /** @type {?} */
            AddRequest.prototype.name;
        }
        /**
         * @record
         */
        function UpdateRequest() { }
        TenantManagement.UpdateRequest = UpdateRequest;
        if (false) {
            /** @type {?} */
            UpdateRequest.prototype.id;
        }
        /**
         * @record
         */
        function DefaultConnectionStringRequest() { }
        TenantManagement.DefaultConnectionStringRequest = DefaultConnectionStringRequest;
        if (false) {
            /** @type {?} */
            DefaultConnectionStringRequest.prototype.id;
            /** @type {?} */
            DefaultConnectionStringRequest.prototype.defaultConnectionString;
        }
    })(TenantManagement || (TenantManagement = {}));

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/models/index.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * Generated from: lib/services/tenant-management-state.service.ts
     * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var TenantManagementStateService = /** @class */ (function () {
        function TenantManagementStateService(store) {
            this.store = store;
        }
        /**
         * @return {?}
         */
        TenantManagementStateService.prototype.get = /**
         * @return {?}
         */
        function () {
            return this.store.selectSnapshot(TenantManagementState.get);
        };
        /**
         * @return {?}
         */
        TenantManagementStateService.prototype.getTenantsTotalCount = /**
         * @return {?}
         */
        function () {
            return this.store.selectSnapshot(TenantManagementState.getTenantsTotalCount);
        };
        TenantManagementStateService.decorators = [
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        TenantManagementStateService.ctorParameters = function () { return [
            { type: store.Store }
        ]; };
        /** @nocollapse */ TenantManagementStateService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function TenantManagementStateService_Factory() { return new TenantManagementStateService(core.ɵɵinject(store.Store)); }, token: TenantManagementStateService, providedIn: "root" });
        return TenantManagementStateService;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        TenantManagementStateService.prototype.store;
    }

    exports.CreateTenant = CreateTenant;
    exports.DeleteTenant = DeleteTenant;
    exports.GetTenantById = GetTenantById;
    exports.GetTenants = GetTenants;
    exports.TENANT_MANAGEMENT_ROUTES = TENANT_MANAGEMENT_ROUTES;
    exports.TenantManagementModule = TenantManagementModule;
    exports.TenantManagementProviders = TenantManagementProviders;
    exports.TenantManagementService = TenantManagementService;
    exports.TenantManagementState = TenantManagementState;
    exports.TenantManagementStateService = TenantManagementStateService;
    exports.TenantsComponent = TenantsComponent;
    exports.UpdateTenant = UpdateTenant;
    exports.ɵa = TenantsComponent;
    exports.ɵb = TenantManagementState;
    exports.ɵc = TenantManagementService;
    exports.ɵd = GetTenants;
    exports.ɵe = GetTenantById;
    exports.ɵf = CreateTenant;
    exports.ɵg = UpdateTenant;
    exports.ɵh = DeleteTenant;
    exports.ɵj = TenantManagementRoutingModule;

    Object.defineProperty(exports, '__esModule', { value: true });

})));
//# sourceMappingURL=abp-ng.tenant-management.umd.js.map
