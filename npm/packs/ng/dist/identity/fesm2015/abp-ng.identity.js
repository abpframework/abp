import { RestService, DynamicLayoutComponent, AuthGuard, PermissionGuard, CoreModule } from '@abp/ng.core';
import { Injectable, ɵɵdefineInjectable, ɵɵinject, Component, ViewChild, NgModule } from '@angular/core';
import { Action, Selector, State, Store, Select, NgxsModule } from '@ngxs/store';
import { __decorate, __metadata } from 'tslib';
import { Observable, Subject, combineLatest } from 'rxjs';
import { NgbModal, NgbTabsetModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { Validators, FormBuilder } from '@angular/forms';
import { tap, switchMap, pluck, debounceTime, filter, map, take } from 'rxjs/operators';
import { ConfirmationService, ThemeSharedModule } from '@abp/ng.theme.shared';
import { RouterModule } from '@angular/router';
import snq from 'snq';
import { validatePassword, NgxValidateCoreModule } from '@ngx-validate/core';
import { PermissionManagementModule } from '@abp/ng.permission-management';
import { TableModule } from 'primeng/table';

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class IdentityGetRoles {
}
IdentityGetRoles.type = '[Identity] Get Roles';
class IdentityGetRoleById {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
IdentityGetRoleById.type = '[Identity] Get Role By Id';
class IdentityDeleteRole {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
IdentityDeleteRole.type = '[Identity] Delete Role';
class IdentityAddRole {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
IdentityAddRole.type = '[Identity] Add Role';
class IdentityUpdateRole {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
IdentityUpdateRole.type = '[Identity] Update Role';
class IdentityGetUsers {
    /**
     * @param {?=} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
IdentityGetUsers.type = '[Identity] Get Users';
class IdentityGetUserById {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
IdentityGetUserById.type = '[Identity] Get User By Id';
class IdentityDeleteUser {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
IdentityDeleteUser.type = '[Identity] Delete User';
class IdentityAddUser {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
IdentityAddUser.type = '[Identity] Add User';
class IdentityUpdateUser {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
IdentityUpdateUser.type = '[Identity] Update User';
class IdentityGetUserRoles {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
IdentityGetUserRoles.type = '[Identity] Get User Roles';

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class IdentityService {
    /**
     * @param {?} rest
     */
    constructor(rest) {
        this.rest = rest;
    }
    /**
     * @return {?}
     */
    getRoles() {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: '/api/identity/role',
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    getRoleById(id) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: `/api/identity/role/${id}`,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    deleteRole(id) {
        /** @type {?} */
        const request = {
            method: 'DELETE',
            url: `/api/identity/role/${id}`,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} body
     * @return {?}
     */
    addRole(body) {
        /** @type {?} */
        const request = {
            method: 'POST',
            url: '/api/identity/role',
            body,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} body
     * @return {?}
     */
    updateRole(body) {
        /** @type {?} */
        const url = `/api/identity/role/${body.id}`;
        delete body.id;
        /** @type {?} */
        const request = {
            method: 'PUT',
            url,
            body,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?=} params
     * @return {?}
     */
    getUsers(params = (/** @type {?} */ ({}))) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: '/api/identity/user',
            params,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    getUserById(id) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: `/api/identity/user/${id}`,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    getUserRoles(id) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: `/api/identity/user/${id}/roles`,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    deleteUser(id) {
        /** @type {?} */
        const request = {
            method: 'DELETE',
            url: `/api/identity/user/${id}`,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} body
     * @return {?}
     */
    addUser(body) {
        /** @type {?} */
        const request = {
            method: 'POST',
            url: '/api/identity/user',
            body,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} body
     * @return {?}
     */
    updateUser(body) {
        /** @type {?} */
        const url = `/identity/users/${body.id}`;
        delete body.id;
        /** @type {?} */
        const request = {
            method: 'PUT',
            url,
            body,
        };
        return this.rest.request(request);
    }
}
IdentityService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
IdentityService.ctorParameters = () => [
    { type: RestService }
];
/** @nocollapse */ IdentityService.ngInjectableDef = ɵɵdefineInjectable({ factory: function IdentityService_Factory() { return new IdentityService(ɵɵinject(RestService)); }, token: IdentityService, providedIn: "root" });

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
let IdentityState = class IdentityState {
    /**
     * @param {?} identityService
     */
    constructor(identityService) {
        this.identityService = identityService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getRoles({ roles }) {
        return roles.items;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getRolesTotalCount({ roles }) {
        return roles.totalCount;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getUsers({ users }) {
        return users.items;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getUsersTotalCount({ users }) {
        return users.totalCount;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    getRoles({ patchState }) {
        return this.identityService.getRoles().pipe(tap((/**
         * @param {?} roles
         * @return {?}
         */
        roles => patchState({
            roles,
        }))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    getRole({ patchState }, { payload }) {
        return this.identityService.getRoleById(payload).pipe(tap((/**
         * @param {?} selectedRole
         * @return {?}
         */
        selectedRole => patchState({
            selectedRole,
        }))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    deleteRole({ dispatch }, { payload }) {
        return this.identityService.deleteRole(payload).pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new IdentityGetRoles()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    addRole({ dispatch }, { payload }) {
        return this.identityService.addRole(payload).pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new IdentityGetRoles()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    updateRole({ getState, dispatch }, { payload }) {
        return dispatch(new IdentityGetRoleById(payload.id)).pipe(switchMap((/**
         * @return {?}
         */
        () => this.identityService.updateRole(Object.assign({}, getState().selectedRole, payload)))), switchMap((/**
         * @return {?}
         */
        () => dispatch(new IdentityGetRoles()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    getUsers({ patchState }, { payload }) {
        return this.identityService.getUsers(payload).pipe(tap((/**
         * @param {?} users
         * @return {?}
         */
        users => patchState({
            users,
        }))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    getUser({ patchState }, { payload }) {
        return this.identityService.getUserById(payload).pipe(tap((/**
         * @param {?} selectedUser
         * @return {?}
         */
        selectedUser => patchState({
            selectedUser,
        }))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    deleteUser({ dispatch }, { payload }) {
        return this.identityService.deleteUser(payload).pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new IdentityGetUsers()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    addUser({ dispatch }, { payload }) {
        return this.identityService.addUser(payload).pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new IdentityGetUsers()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    updateUser({ getState, dispatch }, { payload }) {
        return dispatch(new IdentityGetUserById(payload.id)).pipe(switchMap((/**
         * @return {?}
         */
        () => this.identityService.updateUser(Object.assign({}, getState().selectedUser, payload)))), switchMap((/**
         * @return {?}
         */
        () => dispatch(new IdentityGetUsers()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    getUserRoles({ patchState }, { payload }) {
        return this.identityService.getUserRoles(payload).pipe(pluck('items'), tap((/**
         * @param {?} selectedUserRoles
         * @return {?}
         */
        selectedUserRoles => patchState({
            selectedUserRoles,
        }))));
    }
};
__decorate([
    Action(IdentityGetRoles),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object]),
    __metadata("design:returntype", void 0)
], IdentityState.prototype, "getRoles", null);
__decorate([
    Action(IdentityGetRoleById),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object, IdentityGetRoleById]),
    __metadata("design:returntype", void 0)
], IdentityState.prototype, "getRole", null);
__decorate([
    Action(IdentityDeleteRole),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object, IdentityGetRoleById]),
    __metadata("design:returntype", void 0)
], IdentityState.prototype, "deleteRole", null);
__decorate([
    Action(IdentityAddRole),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object, IdentityAddRole]),
    __metadata("design:returntype", void 0)
], IdentityState.prototype, "addRole", null);
__decorate([
    Action(IdentityUpdateRole),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object, IdentityUpdateRole]),
    __metadata("design:returntype", void 0)
], IdentityState.prototype, "updateRole", null);
__decorate([
    Action(IdentityGetUsers),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object, IdentityGetUsers]),
    __metadata("design:returntype", void 0)
], IdentityState.prototype, "getUsers", null);
__decorate([
    Action(IdentityGetUserById),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object, IdentityGetUserById]),
    __metadata("design:returntype", void 0)
], IdentityState.prototype, "getUser", null);
__decorate([
    Action(IdentityDeleteUser),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object, IdentityGetUserById]),
    __metadata("design:returntype", void 0)
], IdentityState.prototype, "deleteUser", null);
__decorate([
    Action(IdentityAddUser),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object, IdentityAddUser]),
    __metadata("design:returntype", void 0)
], IdentityState.prototype, "addUser", null);
__decorate([
    Action(IdentityUpdateUser),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object, IdentityUpdateUser]),
    __metadata("design:returntype", void 0)
], IdentityState.prototype, "updateUser", null);
__decorate([
    Action(IdentityGetUserRoles),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object, IdentityGetUserRoles]),
    __metadata("design:returntype", void 0)
], IdentityState.prototype, "getUserRoles", null);
__decorate([
    Selector(),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object]),
    __metadata("design:returntype", Array)
], IdentityState, "getRoles", null);
__decorate([
    Selector(),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object]),
    __metadata("design:returntype", Number)
], IdentityState, "getRolesTotalCount", null);
__decorate([
    Selector(),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object]),
    __metadata("design:returntype", Array)
], IdentityState, "getUsers", null);
__decorate([
    Selector(),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object]),
    __metadata("design:returntype", Number)
], IdentityState, "getUsersTotalCount", null);
IdentityState = __decorate([
    State({
        name: 'IdentityState',
        defaults: (/** @type {?} */ ({ roles: {}, selectedRole: {}, users: {}, selectedUser: {} })),
    }),
    __metadata("design:paramtypes", [IdentityService])
], IdentityState);

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class RolesComponent {
    /**
     * @param {?} confirmationService
     * @param {?} modalService
     * @param {?} fb
     * @param {?} store
     */
    constructor(confirmationService, modalService, fb, store) {
        this.confirmationService = confirmationService;
        this.modalService = modalService;
        this.fb = fb;
        this.store = store;
        this.visiblePermissions = false;
    }
    /**
     * @return {?}
     */
    createForm() {
        this.form = this.fb.group({
            name: [this.selected.name || '', [Validators.required, Validators.maxLength(256)]],
            isDefault: [this.selected.isDefault || false],
            isPublic: [this.selected.isPublic || false],
        });
    }
    /**
     * @return {?}
     */
    openModal() {
        this.createForm();
        this.modalService.open(this.modalContent);
    }
    /**
     * @return {?}
     */
    onAdd() {
        this.selected = (/** @type {?} */ ({}));
        this.openModal();
    }
    /**
     * @param {?} id
     * @return {?}
     */
    onEdit(id) {
        this.store
            .dispatch(new IdentityGetRoleById(id))
            .pipe(pluck('IdentityState', 'selectedRole'))
            .subscribe((/**
         * @param {?} selectedRole
         * @return {?}
         */
        selectedRole => {
            this.selected = selectedRole;
            this.openModal();
        }));
    }
    /**
     * @return {?}
     */
    save() {
        if (!this.form.valid)
            return;
        this.store
            .dispatch(this.selected.id
            ? new IdentityUpdateRole(Object.assign({}, this.form.value, { id: this.selected.id }))
            : new IdentityAddRole(this.form.value))
            .subscribe((/**
         * @return {?}
         */
        () => this.modalService.dismissAll()));
    }
    /**
     * @param {?} id
     * @param {?} name
     * @return {?}
     */
    delete(id, name) {
        this.confirmationService
            .warn('AbpIdentity::RoleDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
            messageLocalizationParams: [name],
        })
            .subscribe((/**
         * @param {?} status
         * @return {?}
         */
        (status) => {
            if (status === "confirm" /* confirm */) {
                this.store.dispatch(new IdentityDeleteRole(id));
            }
        }));
    }
}
RolesComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-roles',
                template: "<div id=\"identity-roles-wrapper\" class=\"card\">\n  <div class=\"card-header\">\n    <div class=\"row\">\n      <div class=\"col col-md-6\">\n        <h5 class=\"card-title\">{{ 'AbpIdentity::Roles' | abpLocalization }}</h5>\n      </div>\n      <div class=\"text-right col col-md-6\">\n        <button id=\"create-role\" class=\"btn btn-primary\" type=\"button\" (click)=\"onAdd()\">\n          <i class=\"fa fa-plus mr-1\"></i> <span>{{ 'AbpIdentity::NewRole' | abpLocalization }}</span>\n        </button>\n      </div>\n    </div>\n  </div>\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          placeholder=\"Search\"\n          (input)=\"dt.filterGlobal($event.target.value, 'contains')\"\n      /></label>\n    </div>\n    <p-table #dt [value]=\"roles$ | async\" [globalFilterFields]=\"['name']\" [paginator]=\"true\" [rows]=\"10\">\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpIdentity::Actions' | abpLocalization }}</th>\n          <th>{{ 'AbpIdentity::RoleName' | abpLocalization }}</th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpIdentity::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button ngbDropdownItem (click)=\"onEdit(data.id)\">{{ 'AbpIdentity::Edit' | abpLocalization }}</button>\n                <button ngbDropdownItem (click)=\"providerKey = data.name; visiblePermissions = true\">\n                  {{ 'AbpIdentity::Permissions' | abpLocalization }}\n                </button>\n                <button ngbDropdownItem (click)=\"delete(data.id, data.name)\">\n                  {{ 'AbpIdentity::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<ng-template #modalContent let-modal>\n  <div class=\"modal-header\">\n    <h5 class=\"modal-title\" id=\"modal-basic-title\">\n      {{ (selected?.id ? 'AbpIdentity::Edit' : 'AbpIdentity::NewRole') | abpLocalization }}\n    </h5>\n    <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n      <span aria-hidden=\"true\">&times;</span>\n    </button>\n  </div>\n  <form [formGroup]=\"form\" (ngSubmit)=\"save()\">\n    <div class=\"modal-body\">\n      <div class=\"form-group\">\n        <label for=\"role-name\">{{ 'AbpIdentity::RoleName' | abpLocalization }}</label\n        ><span> * </span>\n        <input type=\"text\" id=\"role-name\" class=\"form-control\" formControlName=\"name\" />\n      </div>\n\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input type=\"checkbox\" id=\"role-is-default\" class=\"custom-control-input\" formControlName=\"isDefault\" />\n        <label class=\"custom-control-label\" for=\"role-is-default\">{{\n          'AbpIdentity::DisplayName:IsDefault' | abpLocalization\n        }}</label>\n      </div>\n\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input type=\"checkbox\" id=\"role-is-public\" class=\"custom-control-input\" formControlName=\"isPublic\" />\n        <label class=\"custom-control-label\" for=\"role-is-public\">{{\n          'AbpIdentity::DisplayName:IsPublic' | abpLocalization\n        }}</label>\n      </div>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </form>\n</ng-template>\n\n<abp-permission-management\n  [(visible)]=\"visiblePermissions\"\n  providerName=\"Role\"\n  [providerKey]=\"providerKey\"\n></abp-permission-management>\n"
            }] }
];
/** @nocollapse */
RolesComponent.ctorParameters = () => [
    { type: ConfirmationService },
    { type: NgbModal },
    { type: FormBuilder },
    { type: Store }
];
RolesComponent.propDecorators = {
    modalContent: [{ type: ViewChild, args: ['modalContent', { static: false },] }]
};
__decorate([
    Select(IdentityState.getRoles),
    __metadata("design:type", Observable)
], RolesComponent.prototype, "roles$", void 0);

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class RoleResolver {
    /**
     * @param {?} store
     */
    constructor(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    resolve() {
        /** @type {?} */
        const roles = this.store.selectSnapshot(IdentityState.getRoles);
        return roles && roles.length ? null : this.store.dispatch(new IdentityGetRoles());
    }
}
RoleResolver.decorators = [
    { type: Injectable }
];
/** @nocollapse */
RoleResolver.ctorParameters = () => [
    { type: Store }
];

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class UsersComponent {
    /**
     * @param {?} confirmationService
     * @param {?} modalService
     * @param {?} fb
     * @param {?} store
     */
    constructor(confirmationService, modalService, fb, store) {
        this.confirmationService = confirmationService;
        this.modalService = modalService;
        this.fb = fb;
        this.store = store;
        this.visiblePermissions = false;
        this.pageQuery = {
            sorting: 'userName',
        };
        this.loading = false;
        this.search$ = new Subject();
        this.trackByFn = (/**
         * @param {?} index
         * @param {?} item
         * @return {?}
         */
        (index, item) => Object.keys(item)[0] || index);
    }
    /**
     * @return {?}
     */
    get roleGroups() {
        return snq((/**
         * @return {?}
         */
        () => (/** @type {?} */ (((/** @type {?} */ (this.form.get('roleNames')))).controls))), []);
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        this.search$.pipe(debounceTime(300)).subscribe((/**
         * @param {?} value
         * @return {?}
         */
        value => {
            this.pageQuery.filter = value;
            this.get();
        }));
    }
    /**
     * @return {?}
     */
    buildForm() {
        this.roles = this.store.selectSnapshot(IdentityState.getRoles);
        this.form = this.fb.group({
            password: [
                '',
                [
                    Validators.required,
                    Validators.maxLength(32),
                    Validators.minLength(6),
                    validatePassword(['small', 'capital', 'number', 'special']),
                ],
            ],
            userName: [this.selected.userName || '', [Validators.required, Validators.maxLength(256)]],
            email: [this.selected.email || '', [Validators.required, Validators.email, Validators.maxLength(256)]],
            name: [this.selected.name || '', [Validators.maxLength(64)]],
            surname: [this.selected.surname || '', [Validators.maxLength(64)]],
            phoneNumber: [this.selected.phoneNumber || '', [Validators.maxLength(16)]],
            lockoutEnabled: [this.selected.twoFactorEnabled || (this.selected.id ? false : true)],
            twoFactorEnabled: [this.selected.twoFactorEnabled || (this.selected.id ? false : true)],
            roleNames: this.fb.array(this.roles.map((/**
             * @param {?} role
             * @return {?}
             */
            role => this.fb.group({
                [role.name]: [!!snq((/**
                     * @return {?}
                     */
                    () => this.selectedUserRoles.find((/**
                     * @param {?} userRole
                     * @return {?}
                     */
                    userRole => userRole.id === role.id))))],
            })))),
        });
    }
    /**
     * @return {?}
     */
    openModal() {
        this.buildForm();
        this.modalService.open(this.modalContent);
    }
    /**
     * @return {?}
     */
    onAdd() {
        this.selected = (/** @type {?} */ ({}));
        this.selectedUserRoles = (/** @type {?} */ ([]));
        this.openModal();
    }
    /**
     * @param {?} id
     * @return {?}
     */
    onEdit(id) {
        combineLatest([this.store.dispatch(new IdentityGetUserById(id)), this.store.dispatch(new IdentityGetUserRoles(id))])
            .pipe(filter((/**
         * @param {?} __0
         * @return {?}
         */
        ([res1, res2]) => res1 && res2)), map((/**
         * @param {?} __0
         * @return {?}
         */
        ([state, _]) => state)), pluck('IdentityState'), take(1))
            .subscribe((/**
         * @param {?} state
         * @return {?}
         */
        (state) => {
            this.selected = state.selectedUser;
            this.selectedUserRoles = state.selectedUserRoles;
            this.openModal();
        }));
    }
    /**
     * @return {?}
     */
    save() {
        if (!this.form.valid)
            return;
        const { roleNames } = this.form.value;
        /** @type {?} */
        const mappedRoleNames = snq((/**
         * @return {?}
         */
        () => roleNames.filter((/**
         * @param {?} role
         * @return {?}
         */
        role => !!role[Object.keys(role)[0]])).map((/**
         * @param {?} role
         * @return {?}
         */
        role => Object.keys(role)[0]))), []);
        this.store
            .dispatch(this.selected.id
            ? new IdentityUpdateUser(Object.assign({}, this.form.value, { id: this.selected.id, roleNames: mappedRoleNames }))
            : new IdentityAddUser(Object.assign({}, this.form.value, { roleNames: mappedRoleNames })))
            .subscribe((/**
         * @return {?}
         */
        () => this.modalService.dismissAll()));
    }
    /**
     * @param {?} id
     * @param {?} userName
     * @return {?}
     */
    delete(id, userName) {
        this.confirmationService
            .warn('AbpIdentity::UserDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
            messageLocalizationParams: [userName],
        })
            .subscribe((/**
         * @param {?} status
         * @return {?}
         */
        (status) => {
            if (status === "confirm" /* confirm */) {
                this.store.dispatch(new IdentityDeleteUser(id));
            }
        }));
    }
    /**
     * @param {?} data
     * @return {?}
     */
    onPageChange(data) {
        this.pageQuery.skipCount = data.first;
        this.pageQuery.maxResultCount = data.rows;
        this.get();
    }
    /**
     * @return {?}
     */
    get() {
        this.loading = true;
        this.store.dispatch(new IdentityGetUsers(this.pageQuery)).subscribe((/**
         * @return {?}
         */
        () => (this.loading = false)));
    }
}
UsersComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-users',
                template: "<div id=\"identity-roles-wrapper\" class=\"card\">\n  <div class=\"card-header\">\n    <div class=\"row\">\n      <div class=\"col col-md-6\">\n        <h5 class=\"card-title\">{{ 'AbpIdentity::Users' | abpLocalization }}</h5>\n      </div>\n      <div class=\"text-right col col-md-6\">\n        <button\n          [abpPermission]=\"'AbpIdentity.Users.Create'\"\n          id=\"create-role\"\n          class=\"btn btn-primary\"\n          type=\"button\"\n          (click)=\"onAdd()\"\n        >\n          <i class=\"fa fa-plus mr-1\"></i> <span>{{ 'AbpIdentity::NewUser' | abpLocalization }}</span>\n        </button>\n      </div>\n    </div>\n  </div>\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          placeholder=\"Search\"\n          (input)=\"search$.next($event.target.value)\"\n      /></label>\n    </div>\n    <p-table\n      [value]=\"data$ | async\"\n      [lazy]=\"true\"\n      [lazyLoadOnInit]=\"false\"\n      [paginator]=\"true\"\n      [rows]=\"10\"\n      [totalRecords]=\"totalCount$ | async\"\n      [loading]=\"loading\"\n      (onLazyLoad)=\"onPageChange($event)\"\n    >\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpIdentity::Actions' | abpLocalization }}</th>\n          <th>{{ 'AbpIdentity::UserName' | abpLocalization }}</th>\n          <th>{{ 'AbpIdentity::EmailAddress' | abpLocalization }}</th>\n          <th>{{ 'AbpIdentity::PhoneNumber' | abpLocalization }}</th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpIdentity::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button ngbDropdownItem (click)=\"onEdit(data.id)\">{{ 'AbpIdentity::Edit' | abpLocalization }}</button>\n                <button ngbDropdownItem (click)=\"providerKey = data.id; visiblePermissions = true\">\n                  {{ 'AbpIdentity::Permissions' | abpLocalization }}\n                </button>\n                <button ngbDropdownItem (click)=\"delete(data.id, data.userName)\">\n                  {{ 'AbpIdentity::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.userName }}</td>\n          <td>{{ data.email }}</td>\n          <td>{{ data.phoneNumber }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<ng-template #modalContent let-modal>\n  <div class=\"modal-header\">\n    <h5 class=\"modal-title\" id=\"modal-basic-title\">\n      {{ (selected?.id ? 'AbpIdentity::Edit' : 'AbpIdentity::NewUser') | abpLocalization }}\n    </h5>\n    <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n      <span aria-hidden=\"true\">&times;</span>\n    </button>\n  </div>\n  <form [formGroup]=\"form\" (ngSubmit)=\"save()\">\n    <div class=\"modal-body\">\n      <ngb-tabset>\n        <ngb-tab [title]=\"'AbpIdentity::UserInformations' | abpLocalization\">\n          <ng-template ngbTabContent>\n            <div class=\"mt-2\">\n              <div class=\"form-group\">\n                <label for=\"user-name\">{{ 'AbpIdentity::UserName' | abpLocalization }}</label\n                ><span> * </span>\n                <input type=\"text\" id=\"user-name\" class=\"form-control\" formControlName=\"userName\" />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"name\">{{ 'AbpIdentity::Name' | abpLocalization }}</label>\n                <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"surname\">{{ 'AbpIdentity::DisplayName:Surname' | abpLocalization }}</label>\n                <input type=\"text\" id=\"surname\" class=\"form-control\" formControlName=\"surname\" />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"password\">{{ 'AbpIdentity::Password' | abpLocalization }}</label\n                ><span> * </span>\n                <input\n                  type=\"password\"\n                  id=\"password\"\n                  autocomplete=\"new-password\"\n                  class=\"form-control\"\n                  formControlName=\"password\"\n                />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"email\">{{ 'AbpIdentity::EmailAddress' | abpLocalization }}</label\n                ><span> * </span>\n                <input type=\"text\" id=\"email\" class=\"form-control\" formControlName=\"email\" />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"phone-number\">{{ 'AbpIdentity::PhoneNumber' | abpLocalization }}</label>\n                <input type=\"text\" id=\"phone-number\" class=\"form-control\" formControlName=\"phoneNumber\" />\n              </div>\n\n              <div class=\"custom-checkbox custom-control mb-2\">\n                <input\n                  type=\"checkbox\"\n                  id=\"lockout-checkbox\"\n                  class=\"custom-control-input\"\n                  formControlName=\"lockoutEnabled\"\n                />\n                <label class=\"custom-control-label\" for=\"lockout-checkbox\">{{\n                  'AbpIdentity::DisplayName:LockoutEnabled' | abpLocalization\n                }}</label>\n              </div>\n\n              <div class=\"custom-checkbox custom-control mb-2\">\n                <input\n                  type=\"checkbox\"\n                  id=\"two-factor-checkbox\"\n                  class=\"custom-control-input\"\n                  formControlName=\"twoFactorEnabled\"\n                />\n                <label class=\"custom-control-label\" for=\"two-factor-checkbox\">{{\n                  'AbpIdentity::DisplayName:TwoFactorEnabled' | abpLocalization\n                }}</label>\n              </div>\n            </div>\n          </ng-template>\n        </ngb-tab>\n        <ngb-tab [title]=\"'AbpIdentity::Roles' | abpLocalization\">\n          <ng-template ngbTabContent>\n            <div class=\"mt-2\">\n              <div\n                *ngFor=\"let roleGroup of roleGroups; let i = index; trackBy: trackByFn\"\n                class=\"custom-checkbox custom-control mb-2\"\n              >\n                <input\n                  type=\"checkbox\"\n                  name=\"Roles[0].IsAssigned\"\n                  value=\"true\"\n                  class=\"custom-control-input\"\n                  [attr.id]=\"'roles-' + i\"\n                  [formControl]=\"roleGroup.controls[roles[i].name]\"\n                />\n                <label class=\"custom-control-label\" [attr.for]=\"'roles-' + i\">{{ roles[i].name }}</label>\n              </div>\n            </div>\n          </ng-template>\n        </ngb-tab>\n      </ngb-tabset>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </form>\n</ng-template>\n<abp-permission-management\n  [(visible)]=\"visiblePermissions\"\n  providerName=\"User\"\n  [providerKey]=\"providerKey\"\n></abp-permission-management>\n"
            }] }
];
/** @nocollapse */
UsersComponent.ctorParameters = () => [
    { type: ConfirmationService },
    { type: NgbModal },
    { type: FormBuilder },
    { type: Store }
];
UsersComponent.propDecorators = {
    modalContent: [{ type: ViewChild, args: ['modalContent', { static: false },] }]
};
__decorate([
    Select(IdentityState.getUsers),
    __metadata("design:type", Observable)
], UsersComponent.prototype, "data$", void 0);
__decorate([
    Select(IdentityState.getUsersTotalCount),
    __metadata("design:type", Observable)
], UsersComponent.prototype, "totalCount$", void 0);

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class UserResolver {
    /**
     * @param {?} store
     */
    constructor(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    resolve() {
        /** @type {?} */
        const users = this.store.selectSnapshot(IdentityState.getUsers);
        return users && users.length ? null : this.store.dispatch(new IdentityGetUsers());
    }
}
UserResolver.decorators = [
    { type: Injectable }
];
/** @nocollapse */
UserResolver.ctorParameters = () => [
    { type: Store }
];

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
const ɵ0 = { requiredPolicy: 'AbpIdentity.Roles' }, ɵ1 = { requiredPolicy: 'AbpIdentity.Users' };
/** @type {?} */
const routes = [
    { path: '', redirectTo: 'roles', pathMatch: 'full' },
    {
        path: 'roles',
        component: DynamicLayoutComponent,
        canActivate: [AuthGuard, PermissionGuard],
        data: ɵ0,
        children: [{ path: '', component: RolesComponent, resolve: [RoleResolver] }],
    },
    {
        path: 'users',
        component: DynamicLayoutComponent,
        canActivate: [AuthGuard, PermissionGuard],
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
class IdentityRoutingModule {
}
IdentityRoutingModule.decorators = [
    { type: NgModule, args: [{
                imports: [RouterModule.forChild(routes)],
                exports: [RouterModule],
                providers: [RoleResolver, UserResolver],
            },] }
];

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class IdentityModule {
}
IdentityModule.decorators = [
    { type: NgModule, args: [{
                declarations: [RolesComponent, UsersComponent],
                imports: [
                    NgxsModule.forFeature([IdentityState]),
                    CoreModule,
                    IdentityRoutingModule,
                    NgbTabsetModule,
                    ThemeSharedModule,
                    TableModule,
                    NgbDropdownModule,
                    PermissionManagementModule,
                    NgxValidateCoreModule,
                ],
            },] }
];

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
const IDENTITY_ROUTES = (/** @type {?} */ ([
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

export { IDENTITY_ROUTES, IdentityAddRole, IdentityAddUser, IdentityDeleteRole, IdentityDeleteUser, IdentityGetRoleById, IdentityGetRoles, IdentityGetUserById, IdentityGetUserRoles, IdentityGetUsers, IdentityModule, IdentityService, IdentityState, IdentityUpdateRole, IdentityUpdateUser, RoleResolver, RolesComponent, UsersComponent as ɵb, IdentityRoutingModule as ɵc, UserResolver as ɵd };
//# sourceMappingURL=abp-ng.identity.js.map
