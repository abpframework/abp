(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@abp/ng.core'), require('@angular/core'), require('@ngxs/store'), require('@abp/ng.theme.shared'), require('@angular/forms'), require('rxjs'), require('rxjs/operators'), require('@angular/router'), require('snq'), require('@ng-bootstrap/ng-bootstrap'), require('@abp/ng.permission-management'), require('primeng/table'), require('@ngx-validate/core')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.identity', ['exports', '@abp/ng.core', '@angular/core', '@ngxs/store', '@abp/ng.theme.shared', '@angular/forms', 'rxjs', 'rxjs/operators', '@angular/router', 'snq', '@ng-bootstrap/ng-bootstrap', '@abp/ng.permission-management', 'primeng/table', '@ngx-validate/core'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng.identity = {}), global.ng_core, global.ng.core, global.store, global.ng_theme_shared, global.ng.forms, global.rxjs, global.rxjs.operators, global.ng.router, global.snq, global.ngBootstrap, global.ng_permissionManagement, global.table, global.core$1));
}(this, function (exports, ng_core, core, store, ng_theme_shared, forms, rxjs, operators, router, snq, ngBootstrap, ng_permissionManagement, table, core$1) { 'use strict';

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
    var GetRoles = /** @class */ (function () {
        function GetRoles(payload) {
            this.payload = payload;
        }
        GetRoles.type = '[Identity] Get Roles';
        return GetRoles;
    }());
    if (false) {
        /** @type {?} */
        GetRoles.type;
        /** @type {?} */
        GetRoles.prototype.payload;
    }
    var GetRoleById = /** @class */ (function () {
        function GetRoleById(payload) {
            this.payload = payload;
        }
        GetRoleById.type = '[Identity] Get Role By Id';
        return GetRoleById;
    }());
    if (false) {
        /** @type {?} */
        GetRoleById.type;
        /** @type {?} */
        GetRoleById.prototype.payload;
    }
    var DeleteRole = /** @class */ (function () {
        function DeleteRole(payload) {
            this.payload = payload;
        }
        DeleteRole.type = '[Identity] Delete Role';
        return DeleteRole;
    }());
    if (false) {
        /** @type {?} */
        DeleteRole.type;
        /** @type {?} */
        DeleteRole.prototype.payload;
    }
    var CreateRole = /** @class */ (function () {
        function CreateRole(payload) {
            this.payload = payload;
        }
        CreateRole.type = '[Identity] Create Role';
        return CreateRole;
    }());
    if (false) {
        /** @type {?} */
        CreateRole.type;
        /** @type {?} */
        CreateRole.prototype.payload;
    }
    var UpdateRole = /** @class */ (function () {
        function UpdateRole(payload) {
            this.payload = payload;
        }
        UpdateRole.type = '[Identity] Update Role';
        return UpdateRole;
    }());
    if (false) {
        /** @type {?} */
        UpdateRole.type;
        /** @type {?} */
        UpdateRole.prototype.payload;
    }
    var GetUsers = /** @class */ (function () {
        function GetUsers(payload) {
            this.payload = payload;
        }
        GetUsers.type = '[Identity] Get Users';
        return GetUsers;
    }());
    if (false) {
        /** @type {?} */
        GetUsers.type;
        /** @type {?} */
        GetUsers.prototype.payload;
    }
    var GetUserById = /** @class */ (function () {
        function GetUserById(payload) {
            this.payload = payload;
        }
        GetUserById.type = '[Identity] Get User By Id';
        return GetUserById;
    }());
    if (false) {
        /** @type {?} */
        GetUserById.type;
        /** @type {?} */
        GetUserById.prototype.payload;
    }
    var DeleteUser = /** @class */ (function () {
        function DeleteUser(payload) {
            this.payload = payload;
        }
        DeleteUser.type = '[Identity] Delete User';
        return DeleteUser;
    }());
    if (false) {
        /** @type {?} */
        DeleteUser.type;
        /** @type {?} */
        DeleteUser.prototype.payload;
    }
    var CreateUser = /** @class */ (function () {
        function CreateUser(payload) {
            this.payload = payload;
        }
        CreateUser.type = '[Identity] Create User';
        return CreateUser;
    }());
    if (false) {
        /** @type {?} */
        CreateUser.type;
        /** @type {?} */
        CreateUser.prototype.payload;
    }
    var UpdateUser = /** @class */ (function () {
        function UpdateUser(payload) {
            this.payload = payload;
        }
        UpdateUser.type = '[Identity] Update User';
        return UpdateUser;
    }());
    if (false) {
        /** @type {?} */
        UpdateUser.type;
        /** @type {?} */
        UpdateUser.prototype.payload;
    }
    var GetUserRoles = /** @class */ (function () {
        function GetUserRoles(payload) {
            this.payload = payload;
        }
        GetUserRoles.type = '[Identity] Get User Roles';
        return GetUserRoles;
    }());
    if (false) {
        /** @type {?} */
        GetUserRoles.type;
        /** @type {?} */
        GetUserRoles.prototype.payload;
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var IdentityService = /** @class */ (function () {
        function IdentityService(rest) {
            this.rest = rest;
        }
        /**
         * @param {?=} params
         * @return {?}
         */
        IdentityService.prototype.getRoles = /**
         * @param {?=} params
         * @return {?}
         */
        function (params) {
            if (params === void 0) { params = (/** @type {?} */ ({})); }
            /** @type {?} */
            var request = {
                method: 'GET',
                url: '/api/identity/roles',
                params: params,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} id
         * @return {?}
         */
        IdentityService.prototype.getRoleById = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            /** @type {?} */
            var request = {
                method: 'GET',
                url: "/api/identity/roles/" + id,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} id
         * @return {?}
         */
        IdentityService.prototype.deleteRole = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            /** @type {?} */
            var request = {
                method: 'DELETE',
                url: "/api/identity/roles/" + id,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} body
         * @return {?}
         */
        IdentityService.prototype.createRole = /**
         * @param {?} body
         * @return {?}
         */
        function (body) {
            /** @type {?} */
            var request = {
                method: 'POST',
                url: '/api/identity/roles',
                body: body,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} body
         * @return {?}
         */
        IdentityService.prototype.updateRole = /**
         * @param {?} body
         * @return {?}
         */
        function (body) {
            /** @type {?} */
            var url = "/api/identity/roles/" + body.id;
            delete body.id;
            /** @type {?} */
            var request = {
                method: 'PUT',
                url: url,
                body: body,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?=} params
         * @return {?}
         */
        IdentityService.prototype.getUsers = /**
         * @param {?=} params
         * @return {?}
         */
        function (params) {
            if (params === void 0) { params = (/** @type {?} */ ({})); }
            /** @type {?} */
            var request = {
                method: 'GET',
                url: '/api/identity/users',
                params: params,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} id
         * @return {?}
         */
        IdentityService.prototype.getUserById = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            /** @type {?} */
            var request = {
                method: 'GET',
                url: "/api/identity/users/" + id,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} id
         * @return {?}
         */
        IdentityService.prototype.getUserRoles = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            /** @type {?} */
            var request = {
                method: 'GET',
                url: "/api/identity/users/" + id + "/roles",
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} id
         * @return {?}
         */
        IdentityService.prototype.deleteUser = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            /** @type {?} */
            var request = {
                method: 'DELETE',
                url: "/api/identity/users/" + id,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} body
         * @return {?}
         */
        IdentityService.prototype.createUser = /**
         * @param {?} body
         * @return {?}
         */
        function (body) {
            /** @type {?} */
            var request = {
                method: 'POST',
                url: '/api/identity/users',
                body: body,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} body
         * @return {?}
         */
        IdentityService.prototype.updateUser = /**
         * @param {?} body
         * @return {?}
         */
        function (body) {
            /** @type {?} */
            var url = "/api/identity/users/" + body.id;
            delete body.id;
            /** @type {?} */
            var request = {
                method: 'PUT',
                url: url,
                body: body,
            };
            return this.rest.request(request);
        };
        IdentityService.decorators = [
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        IdentityService.ctorParameters = function () { return [
            { type: ng_core.RestService }
        ]; };
        /** @nocollapse */ IdentityService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function IdentityService_Factory() { return new IdentityService(core.ɵɵinject(ng_core.RestService)); }, token: IdentityService, providedIn: "root" });
        return IdentityService;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        IdentityService.prototype.rest;
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var IdentityState = /** @class */ (function () {
        function IdentityState(identityService) {
            this.identityService = identityService;
        }
        /**
         * @param {?} __0
         * @return {?}
         */
        IdentityState.getRoles = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var roles = _a.roles;
            return roles.items;
        };
        /**
         * @param {?} __0
         * @return {?}
         */
        IdentityState.getRolesTotalCount = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var roles = _a.roles;
            return roles.totalCount;
        };
        /**
         * @param {?} __0
         * @return {?}
         */
        IdentityState.getUsers = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var users = _a.users;
            return users.items;
        };
        /**
         * @param {?} __0
         * @return {?}
         */
        IdentityState.getUsersTotalCount = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var users = _a.users;
            return users.totalCount;
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        IdentityState.prototype.getRoles = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var patchState = _a.patchState;
            var payload = _b.payload;
            return this.identityService.getRoles(payload).pipe(operators.tap((/**
             * @param {?} roles
             * @return {?}
             */
            function (roles) {
                return patchState({
                    roles: roles,
                });
            })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        IdentityState.prototype.getRole = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var patchState = _a.patchState;
            var payload = _b.payload;
            return this.identityService.getRoleById(payload).pipe(operators.tap((/**
             * @param {?} selectedRole
             * @return {?}
             */
            function (selectedRole) {
                return patchState({
                    selectedRole: selectedRole,
                });
            })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        IdentityState.prototype.deleteRole = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var dispatch = _a.dispatch;
            var payload = _b.payload;
            return this.identityService.deleteRole(payload).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new GetRoles()); })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        IdentityState.prototype.addRole = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var dispatch = _a.dispatch;
            var payload = _b.payload;
            return this.identityService.createRole(payload).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new GetRoles()); })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        IdentityState.prototype.updateRole = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var _this = this;
            var getState = _a.getState, dispatch = _a.dispatch;
            var payload = _b.payload;
            return dispatch(new GetRoleById(payload.id)).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return _this.identityService.updateRole(__assign({}, getState().selectedRole, payload)); })), operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new GetRoles()); })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        IdentityState.prototype.getUsers = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var patchState = _a.patchState;
            var payload = _b.payload;
            return this.identityService.getUsers(payload).pipe(operators.tap((/**
             * @param {?} users
             * @return {?}
             */
            function (users) {
                return patchState({
                    users: users,
                });
            })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        IdentityState.prototype.getUser = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var patchState = _a.patchState;
            var payload = _b.payload;
            return this.identityService.getUserById(payload).pipe(operators.tap((/**
             * @param {?} selectedUser
             * @return {?}
             */
            function (selectedUser) {
                return patchState({
                    selectedUser: selectedUser,
                });
            })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        IdentityState.prototype.deleteUser = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var dispatch = _a.dispatch;
            var payload = _b.payload;
            return this.identityService.deleteUser(payload).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new GetUsers()); })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        IdentityState.prototype.addUser = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var dispatch = _a.dispatch;
            var payload = _b.payload;
            return this.identityService.createUser(payload).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new GetUsers()); })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        IdentityState.prototype.updateUser = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var _this = this;
            var getState = _a.getState, dispatch = _a.dispatch;
            var payload = _b.payload;
            return dispatch(new GetUserById(payload.id)).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return _this.identityService.updateUser(__assign({}, getState().selectedUser, payload)); })), operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new GetUsers()); })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        IdentityState.prototype.getUserRoles = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var patchState = _a.patchState;
            var payload = _b.payload;
            return this.identityService.getUserRoles(payload).pipe(operators.pluck('items'), operators.tap((/**
             * @param {?} selectedUserRoles
             * @return {?}
             */
            function (selectedUserRoles) {
                return patchState({
                    selectedUserRoles: selectedUserRoles,
                });
            })));
        };
        __decorate([
            store.Action(GetRoles),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, GetRoles]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "getRoles", null);
        __decorate([
            store.Action(GetRoleById),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, GetRoleById]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "getRole", null);
        __decorate([
            store.Action(DeleteRole),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, GetRoleById]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "deleteRole", null);
        __decorate([
            store.Action(CreateRole),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, CreateRole]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "addRole", null);
        __decorate([
            store.Action(UpdateRole),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, UpdateRole]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "updateRole", null);
        __decorate([
            store.Action(GetUsers),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, GetUsers]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "getUsers", null);
        __decorate([
            store.Action(GetUserById),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, GetUserById]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "getUser", null);
        __decorate([
            store.Action(DeleteUser),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, GetUserById]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "deleteUser", null);
        __decorate([
            store.Action(CreateUser),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, CreateUser]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "addUser", null);
        __decorate([
            store.Action(UpdateUser),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, UpdateUser]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "updateUser", null);
        __decorate([
            store.Action(GetUserRoles),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, GetUserRoles]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "getUserRoles", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", Array)
        ], IdentityState, "getRoles", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", Number)
        ], IdentityState, "getRolesTotalCount", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", Array)
        ], IdentityState, "getUsers", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", Number)
        ], IdentityState, "getUsersTotalCount", null);
        IdentityState = __decorate([
            store.State({
                name: 'IdentityState',
                defaults: (/** @type {?} */ ({ roles: {}, selectedRole: {}, users: {}, selectedUser: {} })),
            }),
            __metadata("design:paramtypes", [IdentityService])
        ], IdentityState);
        return IdentityState;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        IdentityState.prototype.identityService;
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var RolesComponent = /** @class */ (function () {
        function RolesComponent(confirmationService, fb, store) {
            this.confirmationService = confirmationService;
            this.fb = fb;
            this.store = store;
            this.visiblePermissions = false;
            this.pageQuery = {
                sorting: 'name',
            };
            this.loading = false;
            this.modalBusy = false;
            this.sortOrder = 'asc';
        }
        /**
         * @param {?} value
         * @return {?}
         */
        RolesComponent.prototype.onSearch = /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this.pageQuery.filter = value;
            this.get();
        };
        /**
         * @return {?}
         */
        RolesComponent.prototype.createForm = /**
         * @return {?}
         */
        function () {
            this.form = this.fb.group({
                name: new forms.FormControl({ value: this.selected.name || '', disabled: this.selected.isStatic }, [
                    forms.Validators.required,
                    forms.Validators.maxLength(256),
                ]),
                isDefault: [this.selected.isDefault || false],
                isPublic: [this.selected.isPublic || false],
            });
        };
        /**
         * @return {?}
         */
        RolesComponent.prototype.openModal = /**
         * @return {?}
         */
        function () {
            this.createForm();
            this.isModalVisible = true;
        };
        /**
         * @return {?}
         */
        RolesComponent.prototype.onAdd = /**
         * @return {?}
         */
        function () {
            this.selected = (/** @type {?} */ ({}));
            this.openModal();
        };
        /**
         * @param {?} id
         * @return {?}
         */
        RolesComponent.prototype.onEdit = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            var _this = this;
            this.store
                .dispatch(new GetRoleById(id))
                .pipe(operators.pluck('IdentityState', 'selectedRole'))
                .subscribe((/**
             * @param {?} selectedRole
             * @return {?}
             */
            function (selectedRole) {
                _this.selected = selectedRole;
                _this.openModal();
            }));
        };
        /**
         * @return {?}
         */
        RolesComponent.prototype.save = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (!this.form.valid)
                return;
            this.modalBusy = true;
            this.store
                .dispatch(this.selected.id
                ? new UpdateRole(__assign({}, this.form.value, { id: this.selected.id }))
                : new CreateRole(this.form.value))
                .subscribe((/**
             * @return {?}
             */
            function () {
                _this.modalBusy = false;
                _this.isModalVisible = false;
            }));
        };
        /**
         * @param {?} id
         * @param {?} name
         * @return {?}
         */
        RolesComponent.prototype.delete = /**
         * @param {?} id
         * @param {?} name
         * @return {?}
         */
        function (id, name) {
            var _this = this;
            this.confirmationService
                .warn('AbpIdentity::RoleDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
                messageLocalizationParams: [name],
            })
                .subscribe((/**
             * @param {?} status
             * @return {?}
             */
            function (status) {
                if (status === "confirm" /* confirm */) {
                    _this.store.dispatch(new DeleteRole(id));
                }
            }));
        };
        /**
         * @param {?} data
         * @return {?}
         */
        RolesComponent.prototype.onPageChange = /**
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
        RolesComponent.prototype.get = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.loading = true;
            this.store
                .dispatch(new GetRoles(this.pageQuery))
                .pipe(operators.finalize((/**
             * @return {?}
             */
            function () { return (_this.loading = false); })))
                .subscribe();
        };
        /**
         * @return {?}
         */
        RolesComponent.prototype.changeSortOrder = /**
         * @return {?}
         */
        function () {
            this.sortOrder = this.sortOrder.toLowerCase() === 'asc' ? 'desc' : 'asc';
        };
        RolesComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-roles',
                        template: "<div class=\"row entry-row\">\n  <div class=\"col-auto\">\n    <h1 class=\"content-header-title\">{{ 'AbpIdentity::Roles' | abpLocalization }}</h1>\n  </div>\n  <div class=\"col\">\n    <div class=\"text-lg-right pt-2\" id=\"AbpContentToolbar\">\n      <button id=\"create-role\" class=\"btn btn-primary\" type=\"button\" (click)=\"onAdd()\">\n        <i class=\"fa fa-plus mr-1\"></i> <span>{{ 'AbpIdentity::NewRole' | abpLocalization }}</span>\n      </button>\n    </div>\n  </div>\n</div>\n\n<div id=\"identity-roles-wrapper\" class=\"card\">\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          [placeholder]=\"'AbpUi::PagerSearch' | abpLocalization\"\n          (input.debounce)=\"onSearch($event.target.value)\"\n      /></label>\n    </div>\n    <p-table\n      *ngIf=\"[130, 200] as columnWidths\"\n      [value]=\"data$ | async\"\n      [lazy]=\"true\"\n      [lazyLoadOnInit]=\"false\"\n      [paginator]=\"true\"\n      [rows]=\"10\"\n      [totalRecords]=\"totalCount$ | async\"\n      [loading]=\"loading\"\n      [resizableColumns]=\"true\"\n      [scrollable]=\"true\"\n      (onLazyLoad)=\"onPageChange($event)\"\n    >\n      <ng-template pTemplate=\"colgroup\">\n        <colgroup>\n          <col *ngFor=\"let width of columnWidths\" [ngStyle]=\"{ 'width.px': width }\" />\n        </colgroup>\n      </ng-template>\n      <ng-template pTemplate=\"emptymessage\" let-columns>\n        <tr\n          abp-table-empty-message\n          [attr.colspan]=\"columnWidths.length\"\n          localizationResource=\"AbpIdentityServer\"\n          localizationProp=\"NoDataAvailableInDatatable\"\n        ></tr>\n      </ng-template>\n      <ng-template pTemplate=\"header\" let-columns>\n        <tr>\n          <th>{{ 'AbpIdentity::Actions' | abpLocalization }}</th>\n          <th pResizableColumn (click)=\"changeSortOrder()\">\n            {{ 'AbpIdentity::RoleName' | abpLocalization }}\n            <span class=\"float-right\"\n              ><i [ngClass]=\"['fa', sortOrder === 'desc' ? 'fa-sort-desc' : 'fa-sort-asc']\"></i\n            ></span>\n          </th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown container=\"body\" class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpIdentity::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button ngbDropdownItem (click)=\"onEdit(data.id)\">{{ 'AbpIdentity::Edit' | abpLocalization }}</button>\n                <button ngbDropdownItem (click)=\"providerKey = data.name; visiblePermissions = true\">\n                  {{ 'AbpIdentity::Permissions' | abpLocalization }}\n                </button>\n                <button ngbDropdownItem (click)=\"delete(data.id, data.name)\">\n                  {{ 'AbpIdentity::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal size=\"md\" [(visible)]=\"isModalVisible\" [busy]=\"modalBusy\">\n  <ng-template #abpHeader>\n    <h3>{{ (selected?.id ? 'AbpIdentity::Edit' : 'AbpIdentity::NewRole') | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <form [formGroup]=\"form\" (ngSubmit)=\"save()\">\n      <div class=\"form-group\">\n        <label for=\"role-name\">{{ 'AbpIdentity::RoleName' | abpLocalization }}</label\n        ><span> * </span>\n        <input autofocus type=\"text\" id=\"role-name\" class=\"form-control\" formControlName=\"name\" />\n      </div>\n\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input type=\"checkbox\" id=\"role-is-default\" class=\"custom-control-input\" formControlName=\"isDefault\" />\n        <label class=\"custom-control-label\" for=\"role-is-default\">{{\n          'AbpIdentity::DisplayName:IsDefault' | abpLocalization\n        }}</label>\n      </div>\n\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input type=\"checkbox\" id=\"role-is-public\" class=\"custom-control-input\" formControlName=\"isPublic\" />\n        <label class=\"custom-control-label\" for=\"role-is-public\">{{\n          'AbpIdentity::DisplayName:IsPublic' | abpLocalization\n        }}</label>\n      </div>\n    </form>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\n      {{ 'AbpIdentity::Cancel' | abpLocalization }}\n    </button>\n    <abp-button iconClass=\"fa fa-check\" (click)=\"save()\">{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button>\n  </ng-template>\n</abp-modal>\n\n<abp-permission-management\n  [(visible)]=\"visiblePermissions\"\n  providerName=\"Role\"\n  [providerKey]=\"providerKey\"\n></abp-permission-management>\n"
                    }] }
        ];
        /** @nocollapse */
        RolesComponent.ctorParameters = function () { return [
            { type: ng_theme_shared.ConfirmationService },
            { type: forms.FormBuilder },
            { type: store.Store }
        ]; };
        RolesComponent.propDecorators = {
            modalContent: [{ type: core.ViewChild, args: ['modalContent', { static: false },] }]
        };
        __decorate([
            store.Select(IdentityState.getRoles),
            __metadata("design:type", rxjs.Observable)
        ], RolesComponent.prototype, "data$", void 0);
        __decorate([
            store.Select(IdentityState.getRolesTotalCount),
            __metadata("design:type", rxjs.Observable)
        ], RolesComponent.prototype, "totalCount$", void 0);
        return RolesComponent;
    }());
    if (false) {
        /** @type {?} */
        RolesComponent.prototype.data$;
        /** @type {?} */
        RolesComponent.prototype.totalCount$;
        /** @type {?} */
        RolesComponent.prototype.form;
        /** @type {?} */
        RolesComponent.prototype.selected;
        /** @type {?} */
        RolesComponent.prototype.isModalVisible;
        /** @type {?} */
        RolesComponent.prototype.visiblePermissions;
        /** @type {?} */
        RolesComponent.prototype.providerKey;
        /** @type {?} */
        RolesComponent.prototype.pageQuery;
        /** @type {?} */
        RolesComponent.prototype.loading;
        /** @type {?} */
        RolesComponent.prototype.modalBusy;
        /** @type {?} */
        RolesComponent.prototype.sortOrder;
        /** @type {?} */
        RolesComponent.prototype.modalContent;
        /**
         * @type {?}
         * @private
         */
        RolesComponent.prototype.confirmationService;
        /**
         * @type {?}
         * @private
         */
        RolesComponent.prototype.fb;
        /**
         * @type {?}
         * @private
         */
        RolesComponent.prototype.store;
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var RoleResolver = /** @class */ (function () {
        function RoleResolver(store) {
            this.store = store;
        }
        /**
         * @return {?}
         */
        RoleResolver.prototype.resolve = /**
         * @return {?}
         */
        function () {
            /** @type {?} */
            var roles = this.store.selectSnapshot(IdentityState.getRoles);
            return roles && roles.length ? null : this.store.dispatch(new GetRoles());
        };
        RoleResolver.decorators = [
            { type: core.Injectable }
        ];
        /** @nocollapse */
        RoleResolver.ctorParameters = function () { return [
            { type: store.Store }
        ]; };
        return RoleResolver;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        RoleResolver.prototype.store;
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var UsersComponent = /** @class */ (function () {
        function UsersComponent(confirmationService, fb, store) {
            this.confirmationService = confirmationService;
            this.fb = fb;
            this.store = store;
            this.visiblePermissions = false;
            this.pageQuery = {
                sorting: 'userName',
            };
            this.loading = false;
            this.modalBusy = false;
            this.sortOrder = 'asc';
            this.trackByFn = (/**
             * @param {?} index
             * @param {?} item
             * @return {?}
             */
            function (index, item) { return Object.keys(item)[0] || index; });
        }
        Object.defineProperty(UsersComponent.prototype, "roleGroups", {
            get: /**
             * @return {?}
             */
            function () {
                var _this = this;
                return snq((/**
                 * @return {?}
                 */
                function () { return (/** @type {?} */ (((/** @type {?} */ (_this.form.get('roleNames')))).controls)); }), []);
            },
            enumerable: true,
            configurable: true
        });
        /**
         * @param {?} value
         * @return {?}
         */
        UsersComponent.prototype.onSearch = /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this.pageQuery.filter = value;
            this.get();
        };
        /**
         * @return {?}
         */
        UsersComponent.prototype.buildForm = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.roles = this.store.selectSnapshot(IdentityState.getRoles);
            this.form = this.fb.group({
                userName: [this.selected.userName || '', [forms.Validators.required, forms.Validators.maxLength(256)]],
                email: [this.selected.email || '', [forms.Validators.required, forms.Validators.email, forms.Validators.maxLength(256)]],
                name: [this.selected.name || '', [forms.Validators.maxLength(64)]],
                surname: [this.selected.surname || '', [forms.Validators.maxLength(64)]],
                phoneNumber: [this.selected.phoneNumber || '', [forms.Validators.maxLength(16)]],
                lockoutEnabled: [this.selected.twoFactorEnabled || (this.selected.id ? false : true)],
                twoFactorEnabled: [this.selected.twoFactorEnabled || (this.selected.id ? false : true)],
                roleNames: this.fb.array(this.roles.map((/**
                 * @param {?} role
                 * @return {?}
                 */
                function (role) {
                    var _a;
                    return _this.fb.group((_a = {},
                        _a[role.name] = [!!snq((/**
                             * @return {?}
                             */
                            function () { return _this.selectedUserRoles.find((/**
                             * @param {?} userRole
                             * @return {?}
                             */
                            function (userRole) { return userRole.id === role.id; })); }))],
                        _a));
                }))),
            });
            if (!this.selected.userName) {
                this.form.addControl('password', new forms.FormControl('', [forms.Validators.required, forms.Validators.maxLength(32)]));
            }
        };
        /**
         * @return {?}
         */
        UsersComponent.prototype.openModal = /**
         * @return {?}
         */
        function () {
            this.buildForm();
            this.isModalVisible = true;
        };
        /**
         * @return {?}
         */
        UsersComponent.prototype.onAdd = /**
         * @return {?}
         */
        function () {
            this.selected = (/** @type {?} */ ({}));
            this.selectedUserRoles = (/** @type {?} */ ([]));
            this.openModal();
        };
        /**
         * @param {?} id
         * @return {?}
         */
        UsersComponent.prototype.onEdit = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            var _this = this;
            this.store
                .dispatch(new GetUserById(id))
                .pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return _this.store.dispatch(new GetUserRoles(id)); })), operators.pluck('IdentityState'), operators.take(1))
                .subscribe((/**
             * @param {?} state
             * @return {?}
             */
            function (state) {
                _this.selected = state.selectedUser;
                _this.selectedUserRoles = state.selectedUserRoles;
                _this.openModal();
            }));
        };
        /**
         * @return {?}
         */
        UsersComponent.prototype.save = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (!this.form.valid)
                return;
            this.modalBusy = true;
            var roleNames = this.form.value.roleNames;
            /** @type {?} */
            var mappedRoleNames = snq((/**
             * @return {?}
             */
            function () { return roleNames.filter((/**
             * @param {?} role
             * @return {?}
             */
            function (role) { return !!role[Object.keys(role)[0]]; })).map((/**
             * @param {?} role
             * @return {?}
             */
            function (role) { return Object.keys(role)[0]; })); }), []);
            this.store
                .dispatch(this.selected.id
                ? new UpdateUser(__assign({}, this.form.value, { id: this.selected.id, roleNames: mappedRoleNames }))
                : new CreateUser(__assign({}, this.form.value, { roleNames: mappedRoleNames })))
                .subscribe((/**
             * @return {?}
             */
            function () {
                _this.modalBusy = false;
                _this.isModalVisible = false;
            }));
        };
        /**
         * @param {?} id
         * @param {?} userName
         * @return {?}
         */
        UsersComponent.prototype.delete = /**
         * @param {?} id
         * @param {?} userName
         * @return {?}
         */
        function (id, userName) {
            var _this = this;
            this.confirmationService
                .warn('AbpIdentity::UserDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
                messageLocalizationParams: [userName],
            })
                .subscribe((/**
             * @param {?} status
             * @return {?}
             */
            function (status) {
                if (status === "confirm" /* confirm */) {
                    _this.store.dispatch(new DeleteUser(id));
                }
            }));
        };
        /**
         * @param {?} data
         * @return {?}
         */
        UsersComponent.prototype.onPageChange = /**
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
        UsersComponent.prototype.get = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.loading = true;
            this.store
                .dispatch(new GetUsers(this.pageQuery))
                .pipe(operators.finalize((/**
             * @return {?}
             */
            function () { return (_this.loading = false); })))
                .subscribe();
        };
        /**
         * @return {?}
         */
        UsersComponent.prototype.changeSortOrder = /**
         * @return {?}
         */
        function () {
            this.sortOrder = this.sortOrder.toLowerCase() === 'asc' ? 'desc' : 'asc';
        };
        UsersComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-users',
                        template: "<div class=\"row entry-row\">\n  <div class=\"col-auto\">\n    <h1 class=\"content-header-title\">{{ 'AbpIdentity::Users' | abpLocalization }}</h1>\n  </div>\n  <div class=\"col\">\n    <div class=\"text-lg-right pt-2\" id=\"AbpContentToolbar\">\n      <button\n        [abpPermission]=\"'AbpIdentity.Users.Create'\"\n        id=\"create-role\"\n        class=\"btn btn-primary\"\n        type=\"button\"\n        (click)=\"onAdd()\"\n      >\n        <i class=\"fa fa-plus mr-1\"></i> <span>{{ 'AbpIdentity::NewUser' | abpLocalization }}</span>\n      </button>\n    </div>\n  </div>\n</div>\n\n<div id=\"identity-roles-wrapper\" class=\"card\">\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          [placeholder]=\"'AbpUi::PagerSearch' | abpLocalization\"\n          (input.debounce)=\"onSearch($event.target.value)\"\n      /></label>\n    </div>\n    <p-table\n      *ngIf=\"[130, 200, 200, 200] as columnWidths\"\n      [value]=\"data$ | async\"\n      [lazy]=\"true\"\n      [lazyLoadOnInit]=\"false\"\n      [paginator]=\"true\"\n      [rows]=\"10\"\n      [totalRecords]=\"totalCount$ | async\"\n      [loading]=\"loading\"\n      [resizableColumns]=\"true\"\n      [scrollable]=\"true\"\n      (onLazyLoad)=\"onPageChange($event)\"\n    >\n      <ng-template pTemplate=\"colgroup\">\n        <colgroup>\n          <col *ngFor=\"let width of columnWidths\" [ngStyle]=\"{ 'width.px': width }\" />\n        </colgroup>\n      </ng-template>\n      <ng-template pTemplate=\"emptymessage\" let-columns>\n        <tr\n          abp-table-empty-message\n          [attr.colspan]=\"columnWidths.length\"\n          localizationResource=\"AbpIdentityServer\"\n          localizationProp=\"NoDataAvailableInDatatable\"\n        ></tr>\n      </ng-template>\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpIdentity::Actions' | abpLocalization }}</th>\n          <th pResizableColumn (click)=\"changeSortOrder()\">\n            {{ 'AbpIdentity::UserName' | abpLocalization }}\n            <span class=\"float-right\"\n              ><i [ngClass]=\"['fa', sortOrder === 'desc' ? 'fa-sort-desc' : 'fa-sort-asc']\"></i\n            ></span>\n          </th>\n          <th pResizableColumn>{{ 'AbpIdentity::EmailAddress' | abpLocalization }}</th>\n          <th pResizableColumn>{{ 'AbpIdentity::PhoneNumber' | abpLocalization }}</th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown container=\"body\" class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpIdentity::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button ngbDropdownItem (click)=\"onEdit(data.id)\">{{ 'AbpIdentity::Edit' | abpLocalization }}</button>\n                <button ngbDropdownItem (click)=\"providerKey = data.id; visiblePermissions = true\">\n                  {{ 'AbpIdentity::Permissions' | abpLocalization }}\n                </button>\n                <button ngbDropdownItem (click)=\"delete(data.id, data.userName)\">\n                  {{ 'AbpIdentity::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.userName }}</td>\n          <td>{{ data.email }}</td>\n          <td>{{ data.phoneNumber }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal [(visible)]=\"isModalVisible\" [busy]=\"modalBusy\">\n  <ng-template #abpHeader>\n    <h3>{{ (selected?.id ? 'AbpIdentity::Edit' : 'AbpIdentity::NewUser') | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <form [formGroup]=\"form\" (ngSubmit)=\"save()\">\n      <ngb-tabset>\n        <ngb-tab [title]=\"'AbpIdentity::UserInformations' | abpLocalization\">\n          <ng-template ngbTabContent>\n            <div class=\"mt-2 fade-in-top\">\n              <div class=\"form-group\">\n                <label for=\"user-name\">{{ 'AbpIdentity::UserName' | abpLocalization }}</label\n                ><span> * </span>\n                <input type=\"text\" id=\"user-name\" class=\"form-control\" formControlName=\"userName\" autofocus />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"name\">{{ 'AbpIdentity::DisplayName:Name' | abpLocalization }}</label>\n                <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"surname\">{{ 'AbpIdentity::DisplayName:Surname' | abpLocalization }}</label>\n                <input type=\"text\" id=\"surname\" class=\"form-control\" formControlName=\"surname\" />\n              </div>\n\n              <div *ngIf=\"!selected.userName\" class=\"form-group\">\n                <label for=\"password\">{{ 'AbpIdentity::Password' | abpLocalization }}</label\n                ><span> * </span>\n                <input\n                  type=\"password\"\n                  id=\"password\"\n                  autocomplete=\"new-password\"\n                  class=\"form-control\"\n                  formControlName=\"password\"\n                />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"email\">{{ 'AbpIdentity::EmailAddress' | abpLocalization }}</label\n                ><span> * </span>\n                <input type=\"text\" id=\"email\" class=\"form-control\" formControlName=\"email\" />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"phone-number\">{{ 'AbpIdentity::PhoneNumber' | abpLocalization }}</label>\n                <input type=\"text\" id=\"phone-number\" class=\"form-control\" formControlName=\"phoneNumber\" />\n              </div>\n\n              <div class=\"custom-checkbox custom-control mb-2\">\n                <input\n                  type=\"checkbox\"\n                  id=\"lockout-checkbox\"\n                  class=\"custom-control-input\"\n                  formControlName=\"lockoutEnabled\"\n                />\n                <label class=\"custom-control-label\" for=\"lockout-checkbox\">{{\n                  'AbpIdentity::DisplayName:LockoutEnabled' | abpLocalization\n                }}</label>\n              </div>\n\n              <div class=\"custom-checkbox custom-control mb-2\">\n                <input\n                  type=\"checkbox\"\n                  id=\"two-factor-checkbox\"\n                  class=\"custom-control-input\"\n                  formControlName=\"twoFactorEnabled\"\n                />\n                <label class=\"custom-control-label\" for=\"two-factor-checkbox\">{{\n                  'AbpIdentity::DisplayName:TwoFactorEnabled' | abpLocalization\n                }}</label>\n              </div>\n            </div>\n          </ng-template>\n        </ngb-tab>\n        <ngb-tab [title]=\"'AbpIdentity::Roles' | abpLocalization\">\n          <ng-template ngbTabContent>\n            <div class=\"mt-2 fade-in-top\">\n              <div\n                *ngFor=\"let roleGroup of roleGroups; let i = index; trackBy: trackByFn\"\n                class=\"custom-checkbox custom-control mb-2\"\n              >\n                <input\n                  type=\"checkbox\"\n                  name=\"Roles[0].IsAssigned\"\n                  value=\"true\"\n                  class=\"custom-control-input\"\n                  [attr.id]=\"'roles-' + i\"\n                  [formControl]=\"roleGroup.controls[roles[i].name]\"\n                />\n                <label class=\"custom-control-label\" [attr.for]=\"'roles-' + i\">{{ roles[i].name }}</label>\n              </div>\n            </div>\n          </ng-template>\n        </ngb-tab>\n      </ngb-tabset>\n    </form>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\n      {{ 'AbpIdentity::Cancel' | abpLocalization }}\n    </button>\n    <abp-button iconClass=\"fa fa-check\" (click)=\"save()\" [disabled]=\"form.invalid\">{{\n      'AbpIdentity::Save' | abpLocalization\n    }}</abp-button>\n  </ng-template>\n</abp-modal>\n\n<abp-permission-management\n  [(visible)]=\"visiblePermissions\"\n  providerName=\"User\"\n  [providerKey]=\"providerKey\"\n></abp-permission-management>\n"
                    }] }
        ];
        /** @nocollapse */
        UsersComponent.ctorParameters = function () { return [
            { type: ng_theme_shared.ConfirmationService },
            { type: forms.FormBuilder },
            { type: store.Store }
        ]; };
        UsersComponent.propDecorators = {
            modalContent: [{ type: core.ViewChild, args: ['modalContent', { static: false },] }]
        };
        __decorate([
            store.Select(IdentityState.getUsers),
            __metadata("design:type", rxjs.Observable)
        ], UsersComponent.prototype, "data$", void 0);
        __decorate([
            store.Select(IdentityState.getUsersTotalCount),
            __metadata("design:type", rxjs.Observable)
        ], UsersComponent.prototype, "totalCount$", void 0);
        return UsersComponent;
    }());
    if (false) {
        /** @type {?} */
        UsersComponent.prototype.data$;
        /** @type {?} */
        UsersComponent.prototype.totalCount$;
        /** @type {?} */
        UsersComponent.prototype.modalContent;
        /** @type {?} */
        UsersComponent.prototype.form;
        /** @type {?} */
        UsersComponent.prototype.selected;
        /** @type {?} */
        UsersComponent.prototype.selectedUserRoles;
        /** @type {?} */
        UsersComponent.prototype.roles;
        /** @type {?} */
        UsersComponent.prototype.visiblePermissions;
        /** @type {?} */
        UsersComponent.prototype.providerKey;
        /** @type {?} */
        UsersComponent.prototype.pageQuery;
        /** @type {?} */
        UsersComponent.prototype.isModalVisible;
        /** @type {?} */
        UsersComponent.prototype.loading;
        /** @type {?} */
        UsersComponent.prototype.modalBusy;
        /** @type {?} */
        UsersComponent.prototype.sortOrder;
        /** @type {?} */
        UsersComponent.prototype.trackByFn;
        /**
         * @type {?}
         * @private
         */
        UsersComponent.prototype.confirmationService;
        /**
         * @type {?}
         * @private
         */
        UsersComponent.prototype.fb;
        /**
         * @type {?}
         * @private
         */
        UsersComponent.prototype.store;
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var UserResolver = /** @class */ (function () {
        function UserResolver(store) {
            this.store = store;
        }
        /**
         * @return {?}
         */
        UserResolver.prototype.resolve = /**
         * @return {?}
         */
        function () {
            /** @type {?} */
            var users = this.store.selectSnapshot(IdentityState.getUsers);
            return users && users.length ? null : this.store.dispatch(new GetUsers());
        };
        UserResolver.decorators = [
            { type: core.Injectable }
        ];
        /** @nocollapse */
        UserResolver.ctorParameters = function () { return [
            { type: store.Store }
        ]; };
        return UserResolver;
    }());
    if (false) {
        /**
         * @type {?}
         * @private
         */
        UserResolver.prototype.store;
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ɵ0 = { requiredPolicy: 'AbpIdentity.Roles' }, ɵ1 = { requiredPolicy: 'AbpIdentity.Users' };
    /** @type {?} */
    var routes = [
        { path: '', redirectTo: 'roles', pathMatch: 'full' },
        {
            path: 'roles',
            component: ng_core.DynamicLayoutComponent,
            canActivate: [ng_core.AuthGuard, ng_core.PermissionGuard],
            data: ɵ0,
            children: [{ path: '', component: RolesComponent, resolve: [RoleResolver] }],
        },
        {
            path: 'users',
            component: ng_core.DynamicLayoutComponent,
            canActivate: [ng_core.AuthGuard, ng_core.PermissionGuard],
            data: ɵ1,
            children: [
                {
                    path: '',
                    component: UsersComponent,
                    resolve: [RoleResolver, UserResolver],
                },
            ],
        },
    ];
    var IdentityRoutingModule = /** @class */ (function () {
        function IdentityRoutingModule() {
        }
        IdentityRoutingModule.decorators = [
            { type: core.NgModule, args: [{
                        imports: [router.RouterModule.forChild(routes)],
                        exports: [router.RouterModule],
                        providers: [RoleResolver, UserResolver],
                    },] }
        ];
        return IdentityRoutingModule;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var IdentityModule = /** @class */ (function () {
        function IdentityModule() {
        }
        IdentityModule.decorators = [
            { type: core.NgModule, args: [{
                        declarations: [RolesComponent, UsersComponent],
                        imports: [
                            store.NgxsModule.forFeature([IdentityState]),
                            ng_core.CoreModule,
                            IdentityRoutingModule,
                            ngBootstrap.NgbTabsetModule,
                            ng_theme_shared.ThemeSharedModule,
                            table.TableModule,
                            ngBootstrap.NgbDropdownModule,
                            ng_permissionManagement.PermissionManagementModule,
                            core$1.NgxValidateCoreModule,
                        ],
                    },] }
        ];
        return IdentityModule;
    }());
    /**
     * @return {?}
     */
    function IdentityProviders() {
        return [];
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /** @type {?} */
    var IDENTITY_ROUTES = {
        routes: (/** @type {?} */ ([
            {
                name: 'AbpUiNavigation::Menu:Administration',
                path: '',
                order: 1,
                wrapper: true,
            },
            {
                name: 'AbpIdentity::Menu:IdentityManagement',
                path: 'identity',
                order: 1,
                parentName: 'AbpUiNavigation::Menu:Administration',
                layout: "application" /* application */,
                iconClass: 'fa fa-id-card-o',
                children: [
                    { path: 'roles', name: 'AbpIdentity::Roles', order: 2, requiredPolicy: 'AbpIdentity.Roles' },
                    { path: 'users', name: 'AbpIdentity::Users', order: 1, requiredPolicy: 'AbpIdentity.Users' },
                ],
            },
        ])),
    };

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var Identity;
    (function (Identity) {
        /**
         * @record
         */
        function State() { }
        Identity.State = State;
        if (false) {
            /** @type {?} */
            State.prototype.roles;
            /** @type {?} */
            State.prototype.users;
            /** @type {?} */
            State.prototype.selectedRole;
            /** @type {?} */
            State.prototype.selectedUser;
            /** @type {?} */
            State.prototype.selectedUserRoles;
        }
        /**
         * @record
         */
        function RoleSaveRequest() { }
        Identity.RoleSaveRequest = RoleSaveRequest;
        if (false) {
            /** @type {?} */
            RoleSaveRequest.prototype.name;
            /** @type {?} */
            RoleSaveRequest.prototype.isDefault;
            /** @type {?} */
            RoleSaveRequest.prototype.isPublic;
        }
        /**
         * @record
         */
        function RoleItem() { }
        Identity.RoleItem = RoleItem;
        if (false) {
            /** @type {?} */
            RoleItem.prototype.isStatic;
            /** @type {?} */
            RoleItem.prototype.concurrencyStamp;
            /** @type {?} */
            RoleItem.prototype.id;
        }
        /**
         * @record
         */
        function UserItem() { }
        Identity.UserItem = UserItem;
        if (false) {
            /** @type {?} */
            UserItem.prototype.tenantId;
            /** @type {?} */
            UserItem.prototype.emailConfirmed;
            /** @type {?} */
            UserItem.prototype.phoneNumberConfirmed;
            /** @type {?} */
            UserItem.prototype.isLockedOut;
            /** @type {?} */
            UserItem.prototype.concurrencyStamp;
            /** @type {?} */
            UserItem.prototype.id;
        }
        /**
         * @record
         */
        function User() { }
        Identity.User = User;
        if (false) {
            /** @type {?} */
            User.prototype.userName;
            /** @type {?} */
            User.prototype.name;
            /** @type {?} */
            User.prototype.surname;
            /** @type {?} */
            User.prototype.email;
            /** @type {?} */
            User.prototype.phoneNumber;
            /** @type {?} */
            User.prototype.twoFactorEnabled;
            /** @type {?} */
            User.prototype.lockoutEnabled;
        }
        /**
         * @record
         */
        function UserSaveRequest() { }
        Identity.UserSaveRequest = UserSaveRequest;
        if (false) {
            /** @type {?} */
            UserSaveRequest.prototype.password;
            /** @type {?} */
            UserSaveRequest.prototype.roleNames;
        }
    })(Identity || (Identity = {}));

    exports.CreateRole = CreateRole;
    exports.CreateUser = CreateUser;
    exports.DeleteRole = DeleteRole;
    exports.DeleteUser = DeleteUser;
    exports.GetRoleById = GetRoleById;
    exports.GetRoles = GetRoles;
    exports.GetUserById = GetUserById;
    exports.GetUserRoles = GetUserRoles;
    exports.GetUsers = GetUsers;
    exports.IDENTITY_ROUTES = IDENTITY_ROUTES;
    exports.IdentityModule = IdentityModule;
    exports.IdentityProviders = IdentityProviders;
    exports.IdentityService = IdentityService;
    exports.IdentityState = IdentityState;
    exports.RoleResolver = RoleResolver;
    exports.RolesComponent = RolesComponent;
    exports.UpdateRole = UpdateRole;
    exports.UpdateUser = UpdateUser;
    exports.ɵb = UsersComponent;
    exports.ɵc = IdentityRoutingModule;
    exports.ɵd = UserResolver;

    Object.defineProperty(exports, '__esModule', { value: true });

}));
//# sourceMappingURL=abp-ng.identity.umd.js.map
