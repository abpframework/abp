(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@abp/ng.core'), require('@angular/core'), require('@ngxs/store'), require('rxjs'), require('@ng-bootstrap/ng-bootstrap'), require('@angular/forms'), require('rxjs/operators'), require('@abp/ng.theme.shared'), require('@angular/router'), require('snq'), require('@ngx-validate/core'), require('@abp/ng.permission-management'), require('primeng/table')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.identity', ['exports', '@abp/ng.core', '@angular/core', '@ngxs/store', 'rxjs', '@ng-bootstrap/ng-bootstrap', '@angular/forms', 'rxjs/operators', '@abp/ng.theme.shared', '@angular/router', 'snq', '@ngx-validate/core', '@abp/ng.permission-management', 'primeng/table'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng.identity = {}), global.ng_core, global.ng.core, global.store, global.rxjs, global.ngBootstrap, global.ng.forms, global.rxjs.operators, global.ng_theme_shared, global.ng.router, global.snq, global.core$1, global.ng_permissionManagement, global.table));
}(this, function (exports, ng_core, core, store, rxjs, ngBootstrap, forms, operators, ng_theme_shared, router, snq, core$1, ng_permissionManagement, table) { 'use strict';

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

    function __decorate(decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    }

    function __metadata(metadataKey, metadataValue) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(metadataKey, metadataValue);
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

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var IdentityGetRoles = /** @class */ (function () {
        function IdentityGetRoles() {
        }
        IdentityGetRoles.type = '[Identity] Get Roles';
        return IdentityGetRoles;
    }());
    var IdentityGetRoleById = /** @class */ (function () {
        function IdentityGetRoleById(payload) {
            this.payload = payload;
        }
        IdentityGetRoleById.type = '[Identity] Get Role By Id';
        return IdentityGetRoleById;
    }());
    var IdentityDeleteRole = /** @class */ (function () {
        function IdentityDeleteRole(payload) {
            this.payload = payload;
        }
        IdentityDeleteRole.type = '[Identity] Delete Role';
        return IdentityDeleteRole;
    }());
    var IdentityAddRole = /** @class */ (function () {
        function IdentityAddRole(payload) {
            this.payload = payload;
        }
        IdentityAddRole.type = '[Identity] Add Role';
        return IdentityAddRole;
    }());
    var IdentityUpdateRole = /** @class */ (function () {
        function IdentityUpdateRole(payload) {
            this.payload = payload;
        }
        IdentityUpdateRole.type = '[Identity] Update Role';
        return IdentityUpdateRole;
    }());
    var IdentityGetUsers = /** @class */ (function () {
        function IdentityGetUsers(payload) {
            this.payload = payload;
        }
        IdentityGetUsers.type = '[Identity] Get Users';
        return IdentityGetUsers;
    }());
    var IdentityGetUserById = /** @class */ (function () {
        function IdentityGetUserById(payload) {
            this.payload = payload;
        }
        IdentityGetUserById.type = '[Identity] Get User By Id';
        return IdentityGetUserById;
    }());
    var IdentityDeleteUser = /** @class */ (function () {
        function IdentityDeleteUser(payload) {
            this.payload = payload;
        }
        IdentityDeleteUser.type = '[Identity] Delete User';
        return IdentityDeleteUser;
    }());
    var IdentityAddUser = /** @class */ (function () {
        function IdentityAddUser(payload) {
            this.payload = payload;
        }
        IdentityAddUser.type = '[Identity] Add User';
        return IdentityAddUser;
    }());
    var IdentityUpdateUser = /** @class */ (function () {
        function IdentityUpdateUser(payload) {
            this.payload = payload;
        }
        IdentityUpdateUser.type = '[Identity] Update User';
        return IdentityUpdateUser;
    }());
    var IdentityGetUserRoles = /** @class */ (function () {
        function IdentityGetUserRoles(payload) {
            this.payload = payload;
        }
        IdentityGetUserRoles.type = '[Identity] Get User Roles';
        return IdentityGetUserRoles;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var IdentityService = /** @class */ (function () {
        function IdentityService(rest) {
            this.rest = rest;
        }
        /**
         * @return {?}
         */
        IdentityService.prototype.getRoles = /**
         * @return {?}
         */
        function () {
            /** @type {?} */
            var request = {
                method: 'GET',
                url: '/api/identity/roles',
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
        IdentityService.prototype.addRole = /**
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
        IdentityService.prototype.addUser = /**
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
            var url = "/identity/users/" + body.id;
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
         * @return {?}
         */
        IdentityState.prototype.getRoles = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var patchState = _a.patchState;
            return this.identityService.getRoles().pipe(operators.tap((/**
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
            function () { return dispatch(new IdentityGetRoles()); })));
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
            return this.identityService.addRole(payload).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new IdentityGetRoles()); })));
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
            return dispatch(new IdentityGetRoleById(payload.id)).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return _this.identityService.updateRole(__assign({}, getState().selectedRole, payload)); })), operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new IdentityGetRoles()); })));
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
            function () { return dispatch(new IdentityGetUsers()); })));
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
            return this.identityService.addUser(payload).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new IdentityGetUsers()); })));
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
            return dispatch(new IdentityGetUserById(payload.id)).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return _this.identityService.updateUser(__assign({}, getState().selectedUser, payload)); })), operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new IdentityGetUsers()); })));
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
            store.Action(IdentityGetRoles),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "getRoles", null);
        __decorate([
            store.Action(IdentityGetRoleById),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, IdentityGetRoleById]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "getRole", null);
        __decorate([
            store.Action(IdentityDeleteRole),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, IdentityGetRoleById]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "deleteRole", null);
        __decorate([
            store.Action(IdentityAddRole),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, IdentityAddRole]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "addRole", null);
        __decorate([
            store.Action(IdentityUpdateRole),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, IdentityUpdateRole]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "updateRole", null);
        __decorate([
            store.Action(IdentityGetUsers),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, IdentityGetUsers]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "getUsers", null);
        __decorate([
            store.Action(IdentityGetUserById),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, IdentityGetUserById]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "getUser", null);
        __decorate([
            store.Action(IdentityDeleteUser),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, IdentityGetUserById]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "deleteUser", null);
        __decorate([
            store.Action(IdentityAddUser),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, IdentityAddUser]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "addUser", null);
        __decorate([
            store.Action(IdentityUpdateUser),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, IdentityUpdateUser]),
            __metadata("design:returntype", void 0)
        ], IdentityState.prototype, "updateUser", null);
        __decorate([
            store.Action(IdentityGetUserRoles),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, IdentityGetUserRoles]),
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

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var RolesComponent = /** @class */ (function () {
        function RolesComponent(confirmationService, modalService, fb, store) {
            this.confirmationService = confirmationService;
            this.modalService = modalService;
            this.fb = fb;
            this.store = store;
            this.visiblePermissions = false;
        }
        /**
         * @return {?}
         */
        RolesComponent.prototype.createForm = /**
         * @return {?}
         */
        function () {
            this.form = this.fb.group({
                name: [this.selected.name || '', [forms.Validators.required, forms.Validators.maxLength(256)]],
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
            this.modalService.open(this.modalContent);
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
                .dispatch(new IdentityGetRoleById(id))
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
            this.store
                .dispatch(this.selected.id
                ? new IdentityUpdateRole(__assign({}, this.form.value, { id: this.selected.id }))
                : new IdentityAddRole(this.form.value))
                .subscribe((/**
             * @return {?}
             */
            function () { return _this.modalService.dismissAll(); }));
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
                    _this.store.dispatch(new IdentityDeleteRole(id));
                }
            }));
        };
        RolesComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-roles',
                        template: "<div id=\"identity-roles-wrapper\" class=\"card\">\n  <div class=\"card-header\">\n    <div class=\"row\">\n      <div class=\"col col-md-6\">\n        <h5 class=\"card-title\">{{ 'AbpIdentity::Roles' | abpLocalization }}</h5>\n      </div>\n      <div class=\"text-right col col-md-6\">\n        <button id=\"create-role\" class=\"btn btn-primary\" type=\"button\" (click)=\"onAdd()\">\n          <i class=\"fa fa-plus mr-1\"></i> <span>{{ 'AbpIdentity::NewRole' | abpLocalization }}</span>\n        </button>\n      </div>\n    </div>\n  </div>\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          placeholder=\"Search\"\n          (input)=\"dt.filterGlobal($event.target.value, 'contains')\"\n      /></label>\n    </div>\n    <p-table #dt [value]=\"roles$ | async\" [globalFilterFields]=\"['name']\" [paginator]=\"true\" [rows]=\"10\">\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpIdentity::Actions' | abpLocalization }}</th>\n          <th>{{ 'AbpIdentity::RoleName' | abpLocalization }}</th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpIdentity::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button ngbDropdownItem (click)=\"onEdit(data.id)\">{{ 'AbpIdentity::Edit' | abpLocalization }}</button>\n                <button ngbDropdownItem (click)=\"providerKey = data.name; visiblePermissions = true\">\n                  {{ 'AbpIdentity::Permissions' | abpLocalization }}\n                </button>\n                <button ngbDropdownItem (click)=\"delete(data.id, data.name)\">\n                  {{ 'AbpIdentity::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<ng-template #modalContent let-modal>\n  <div class=\"modal-header\">\n    <h5 class=\"modal-title\" id=\"modal-basic-title\">\n      {{ (selected?.id ? 'AbpIdentity::Edit' : 'AbpIdentity::NewRole') | abpLocalization }}\n    </h5>\n    <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n      <span aria-hidden=\"true\">&times;</span>\n    </button>\n  </div>\n  <form [formGroup]=\"form\" (ngSubmit)=\"save()\">\n    <div class=\"modal-body\">\n      <div class=\"form-group\">\n        <label for=\"role-name\">{{ 'AbpIdentity::RoleName' | abpLocalization }}</label\n        ><span> * </span>\n        <input type=\"text\" id=\"role-name\" class=\"form-control\" formControlName=\"name\" />\n      </div>\n\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input type=\"checkbox\" id=\"role-is-default\" class=\"custom-control-input\" formControlName=\"isDefault\" />\n        <label class=\"custom-control-label\" for=\"role-is-default\">{{\n          'AbpIdentity::DisplayName:IsDefault' | abpLocalization\n        }}</label>\n      </div>\n\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input type=\"checkbox\" id=\"role-is-public\" class=\"custom-control-input\" formControlName=\"isPublic\" />\n        <label class=\"custom-control-label\" for=\"role-is-public\">{{\n          'AbpIdentity::DisplayName:IsPublic' | abpLocalization\n        }}</label>\n      </div>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </form>\n</ng-template>\n\n<abp-permission-management\n  [(visible)]=\"visiblePermissions\"\n  providerName=\"Role\"\n  [providerKey]=\"providerKey\"\n></abp-permission-management>\n"
                    }] }
        ];
        /** @nocollapse */
        RolesComponent.ctorParameters = function () { return [
            { type: ng_theme_shared.ConfirmationService },
            { type: ngBootstrap.NgbModal },
            { type: forms.FormBuilder },
            { type: store.Store }
        ]; };
        RolesComponent.propDecorators = {
            modalContent: [{ type: core.ViewChild, args: ['modalContent', { static: false },] }]
        };
        __decorate([
            store.Select(IdentityState.getRoles),
            __metadata("design:type", rxjs.Observable)
        ], RolesComponent.prototype, "roles$", void 0);
        return RolesComponent;
    }());

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
            return roles && roles.length ? null : this.store.dispatch(new IdentityGetRoles());
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

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var UsersComponent = /** @class */ (function () {
        function UsersComponent(confirmationService, modalService, fb, store) {
            this.confirmationService = confirmationService;
            this.modalService = modalService;
            this.fb = fb;
            this.store = store;
            this.visiblePermissions = false;
            this.pageQuery = {
                sorting: 'userName',
            };
            this.loading = false;
            this.search$ = new rxjs.Subject();
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
         * @return {?}
         */
        UsersComponent.prototype.ngOnInit = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.search$.pipe(operators.debounceTime(300)).subscribe((/**
             * @param {?} value
             * @return {?}
             */
            function (value) {
                _this.pageQuery.filter = value;
                _this.get();
            }));
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
                password: [
                    '',
                    [
                        forms.Validators.required,
                        forms.Validators.maxLength(32),
                        forms.Validators.minLength(6),
                        core$1.validatePassword(['small', 'capital', 'number', 'special']),
                    ],
                ],
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
        };
        /**
         * @return {?}
         */
        UsersComponent.prototype.openModal = /**
         * @return {?}
         */
        function () {
            this.buildForm();
            this.modalService.open(this.modalContent);
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
            rxjs.combineLatest([this.store.dispatch(new IdentityGetUserById(id)), this.store.dispatch(new IdentityGetUserRoles(id))])
                .pipe(operators.filter((/**
             * @param {?} __0
             * @return {?}
             */
            function (_a) {
                var _b = __read(_a, 2), res1 = _b[0], res2 = _b[1];
                return res1 && res2;
            })), operators.map((/**
             * @param {?} __0
             * @return {?}
             */
            function (_a) {
                var _b = __read(_a, 2), state = _b[0], _ = _b[1];
                return state;
            })), operators.pluck('IdentityState'), operators.take(1))
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
                ? new IdentityUpdateUser(__assign({}, this.form.value, { id: this.selected.id, roleNames: mappedRoleNames }))
                : new IdentityAddUser(__assign({}, this.form.value, { roleNames: mappedRoleNames })))
                .subscribe((/**
             * @return {?}
             */
            function () { return _this.modalService.dismissAll(); }));
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
                    _this.store.dispatch(new IdentityDeleteUser(id));
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
            this.store.dispatch(new IdentityGetUsers(this.pageQuery)).subscribe((/**
             * @return {?}
             */
            function () { return (_this.loading = false); }));
        };
        UsersComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-users',
                        template: "<div id=\"identity-roles-wrapper\" class=\"card\">\n  <div class=\"card-header\">\n    <div class=\"row\">\n      <div class=\"col col-md-6\">\n        <h5 class=\"card-title\">{{ 'AbpIdentity::Users' | abpLocalization }}</h5>\n      </div>\n      <div class=\"text-right col col-md-6\">\n        <button\n          [abpPermission]=\"'AbpIdentity.Users.Create'\"\n          id=\"create-role\"\n          class=\"btn btn-primary\"\n          type=\"button\"\n          (click)=\"onAdd()\"\n        >\n          <i class=\"fa fa-plus mr-1\"></i> <span>{{ 'AbpIdentity::NewUser' | abpLocalization }}</span>\n        </button>\n      </div>\n    </div>\n  </div>\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          placeholder=\"Search\"\n          (input)=\"search$.next($event.target.value)\"\n      /></label>\n    </div>\n    <p-table\n      [value]=\"data$ | async\"\n      [lazy]=\"true\"\n      [lazyLoadOnInit]=\"false\"\n      [paginator]=\"true\"\n      [rows]=\"10\"\n      [totalRecords]=\"totalCount$ | async\"\n      [loading]=\"loading\"\n      (onLazyLoad)=\"onPageChange($event)\"\n    >\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpIdentity::Actions' | abpLocalization }}</th>\n          <th>{{ 'AbpIdentity::UserName' | abpLocalization }}</th>\n          <th>{{ 'AbpIdentity::EmailAddress' | abpLocalization }}</th>\n          <th>{{ 'AbpIdentity::PhoneNumber' | abpLocalization }}</th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpIdentity::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button ngbDropdownItem (click)=\"onEdit(data.id)\">{{ 'AbpIdentity::Edit' | abpLocalization }}</button>\n                <button ngbDropdownItem (click)=\"providerKey = data.id; visiblePermissions = true\">\n                  {{ 'AbpIdentity::Permissions' | abpLocalization }}\n                </button>\n                <button ngbDropdownItem (click)=\"delete(data.id, data.userName)\">\n                  {{ 'AbpIdentity::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.userName }}</td>\n          <td>{{ data.email }}</td>\n          <td>{{ data.phoneNumber }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<ng-template #modalContent let-modal>\n  <div class=\"modal-header\">\n    <h5 class=\"modal-title\" id=\"modal-basic-title\">\n      {{ (selected?.id ? 'AbpIdentity::Edit' : 'AbpIdentity::NewUser') | abpLocalization }}\n    </h5>\n    <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n      <span aria-hidden=\"true\">&times;</span>\n    </button>\n  </div>\n  <form [formGroup]=\"form\" (ngSubmit)=\"save()\">\n    <div class=\"modal-body\">\n      <ngb-tabset>\n        <ngb-tab [title]=\"'AbpIdentity::UserInformations' | abpLocalization\">\n          <ng-template ngbTabContent>\n            <div class=\"mt-2\">\n              <div class=\"form-group\">\n                <label for=\"user-name\">{{ 'AbpIdentity::UserName' | abpLocalization }}</label\n                ><span> * </span>\n                <input type=\"text\" id=\"user-name\" class=\"form-control\" formControlName=\"userName\" />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"name\">{{ 'AbpIdentity::Name' | abpLocalization }}</label>\n                <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"surname\">{{ 'AbpIdentity::DisplayName:Surname' | abpLocalization }}</label>\n                <input type=\"text\" id=\"surname\" class=\"form-control\" formControlName=\"surname\" />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"password\">{{ 'AbpIdentity::Password' | abpLocalization }}</label\n                ><span> * </span>\n                <input\n                  type=\"password\"\n                  id=\"password\"\n                  autocomplete=\"new-password\"\n                  class=\"form-control\"\n                  formControlName=\"password\"\n                />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"email\">{{ 'AbpIdentity::EmailAddress' | abpLocalization }}</label\n                ><span> * </span>\n                <input type=\"text\" id=\"email\" class=\"form-control\" formControlName=\"email\" />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"phone-number\">{{ 'AbpIdentity::PhoneNumber' | abpLocalization }}</label>\n                <input type=\"text\" id=\"phone-number\" class=\"form-control\" formControlName=\"phoneNumber\" />\n              </div>\n\n              <div class=\"custom-checkbox custom-control mb-2\">\n                <input\n                  type=\"checkbox\"\n                  id=\"lockout-checkbox\"\n                  class=\"custom-control-input\"\n                  formControlName=\"lockoutEnabled\"\n                />\n                <label class=\"custom-control-label\" for=\"lockout-checkbox\">{{\n                  'AbpIdentity::DisplayName:LockoutEnabled' | abpLocalization\n                }}</label>\n              </div>\n\n              <div class=\"custom-checkbox custom-control mb-2\">\n                <input\n                  type=\"checkbox\"\n                  id=\"two-factor-checkbox\"\n                  class=\"custom-control-input\"\n                  formControlName=\"twoFactorEnabled\"\n                />\n                <label class=\"custom-control-label\" for=\"two-factor-checkbox\">{{\n                  'AbpIdentity::DisplayName:TwoFactorEnabled' | abpLocalization\n                }}</label>\n              </div>\n            </div>\n          </ng-template>\n        </ngb-tab>\n        <ngb-tab [title]=\"'AbpIdentity::Roles' | abpLocalization\">\n          <ng-template ngbTabContent>\n            <div class=\"mt-2\">\n              <div\n                *ngFor=\"let roleGroup of roleGroups; let i = index; trackBy: trackByFn\"\n                class=\"custom-checkbox custom-control mb-2\"\n              >\n                <input\n                  type=\"checkbox\"\n                  name=\"Roles[0].IsAssigned\"\n                  value=\"true\"\n                  class=\"custom-control-input\"\n                  [attr.id]=\"'roles-' + i\"\n                  [formControl]=\"roleGroup.controls[roles[i].name]\"\n                />\n                <label class=\"custom-control-label\" [attr.for]=\"'roles-' + i\">{{ roles[i].name }}</label>\n              </div>\n            </div>\n          </ng-template>\n        </ngb-tab>\n      </ngb-tabset>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </form>\n</ng-template>\n<abp-permission-management\n  [(visible)]=\"visiblePermissions\"\n  providerName=\"User\"\n  [providerKey]=\"providerKey\"\n></abp-permission-management>\n"
                    }] }
        ];
        /** @nocollapse */
        UsersComponent.ctorParameters = function () { return [
            { type: ng_theme_shared.ConfirmationService },
            { type: ngBootstrap.NgbModal },
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
            return users && users.length ? null : this.store.dispatch(new IdentityGetUsers());
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
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /** @type {?} */
    var IDENTITY_ROUTES = (/** @type {?} */ ([
        {
            name: 'Administration',
            path: '',
            order: 1,
            wrapper: true,
        },
        {
            name: 'Identity',
            path: 'identity',
            order: 1,
            parentName: 'Administration',
            layout: "application" /* application */,
            children: [
                { path: 'roles', name: 'Roles', order: 2, requiredPolicy: 'AbpIdentity.Roles' },
                { path: 'users', name: 'Users', order: 1, requiredPolicy: 'AbpIdentity.Users' },
            ],
        },
    ]));

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
        /**
         * @record
         */
        function RoleSaveRequest() { }
        Identity.RoleSaveRequest = RoleSaveRequest;
        /**
         * @record
         */
        function RoleItem() { }
        Identity.RoleItem = RoleItem;
        /**
         * @record
         */
        function UserItem() { }
        Identity.UserItem = UserItem;
        /**
         * @record
         */
        function User() { }
        Identity.User = User;
        /**
         * @record
         */
        function UserSaveRequest() { }
        Identity.UserSaveRequest = UserSaveRequest;
    })(Identity || (Identity = {}));

    exports.IDENTITY_ROUTES = IDENTITY_ROUTES;
    exports.IdentityAddRole = IdentityAddRole;
    exports.IdentityAddUser = IdentityAddUser;
    exports.IdentityDeleteRole = IdentityDeleteRole;
    exports.IdentityDeleteUser = IdentityDeleteUser;
    exports.IdentityGetRoleById = IdentityGetRoleById;
    exports.IdentityGetRoles = IdentityGetRoles;
    exports.IdentityGetUserById = IdentityGetUserById;
    exports.IdentityGetUserRoles = IdentityGetUserRoles;
    exports.IdentityGetUsers = IdentityGetUsers;
    exports.IdentityModule = IdentityModule;
    exports.IdentityService = IdentityService;
    exports.IdentityState = IdentityState;
    exports.IdentityUpdateRole = IdentityUpdateRole;
    exports.IdentityUpdateUser = IdentityUpdateUser;
    exports.RoleResolver = RoleResolver;
    exports.RolesComponent = RolesComponent;
    exports.ɵb = UsersComponent;
    exports.ɵc = IdentityRoutingModule;
    exports.ɵd = UserResolver;

    Object.defineProperty(exports, '__esModule', { value: true });

}));
//# sourceMappingURL=abp-ng.identity.umd.js.map
