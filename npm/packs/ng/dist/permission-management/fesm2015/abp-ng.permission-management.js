import { RestService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { Injectable, ɵɵdefineInjectable, ɵɵinject, EventEmitter, Component, Renderer2, Input, Output, ViewChild, NgModule } from '@angular/core';
import { Action, Selector, State, Store, Select, NgxsModule } from '@ngxs/store';
import { __decorate, __metadata } from 'tslib';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { from, Observable } from 'rxjs';
import { tap, map, take, pluck } from 'rxjs/operators';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class PermissionManagementGetPermissions {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
PermissionManagementGetPermissions.type = '[PermissionManagement] Get Permissions';
class PermissionManagementUpdatePermissions {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
PermissionManagementUpdatePermissions.type = '[PermissionManagement] Update Permissions';

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class PermissionManagementService {
    /**
     * @param {?} rest
     */
    constructor(rest) {
        this.rest = rest;
    }
    /**
     * @param {?} params
     * @return {?}
     */
    getPermissions(params) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: '/api/abp/permissions',
            params,
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    updatePermissions({ permissions, providerKey, providerName, }) {
        /** @type {?} */
        const request = {
            method: 'PUT',
            url: '/api/abp/permissions',
            body: { permissions },
            params: { providerKey, providerName },
        };
        return this.rest.request(request);
    }
}
PermissionManagementService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
PermissionManagementService.ctorParameters = () => [
    { type: RestService }
];
/** @nocollapse */ PermissionManagementService.ngInjectableDef = ɵɵdefineInjectable({ factory: function PermissionManagementService_Factory() { return new PermissionManagementService(ɵɵinject(RestService)); }, token: PermissionManagementService, providedIn: "root" });

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
let PermissionManagementState = class PermissionManagementState {
    /**
     * @param {?} permissionManagementService
     */
    constructor(permissionManagementService) {
        this.permissionManagementService = permissionManagementService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getPermissionGroups({ permissionRes }) {
        return permissionRes.groups || [];
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getEntitiyDisplayName({ permissionRes }) {
        return permissionRes.entityDisplayName;
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    permissionManagementGet({ patchState }, { payload }) {
        return this.permissionManagementService.getPermissions(payload).pipe(tap((/**
         * @param {?} permissionResponse
         * @return {?}
         */
        permissionResponse => patchState({
            permissionRes: permissionResponse,
        }))));
    }
    /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    permissionManagementUpdate(_, { payload }) {
        return this.permissionManagementService.updatePermissions(payload);
    }
};
__decorate([
    Action(PermissionManagementGetPermissions),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object, PermissionManagementGetPermissions]),
    __metadata("design:returntype", void 0)
], PermissionManagementState.prototype, "permissionManagementGet", null);
__decorate([
    Action(PermissionManagementUpdatePermissions),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object, PermissionManagementUpdatePermissions]),
    __metadata("design:returntype", void 0)
], PermissionManagementState.prototype, "permissionManagementUpdate", null);
__decorate([
    Selector(),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object]),
    __metadata("design:returntype", void 0)
], PermissionManagementState, "getPermissionGroups", null);
__decorate([
    Selector(),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object]),
    __metadata("design:returntype", String)
], PermissionManagementState, "getEntitiyDisplayName", null);
PermissionManagementState = __decorate([
    State({
        name: 'PermissionManagementState',
        defaults: (/** @type {?} */ ({ permissionRes: {} })),
    }),
    __metadata("design:paramtypes", [PermissionManagementService])
], PermissionManagementState);

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class PermissionManagementComponent {
    /**
     * @param {?} modalService
     * @param {?} store
     * @param {?} renderer
     */
    constructor(modalService, store, renderer) {
        this.modalService = modalService;
        this.store = store;
        this.renderer = renderer;
        this.visibleChange = new EventEmitter();
        this.permissions = [];
        this.selectThisTab = false;
        this.selectAllTab = false;
        this.trackByFn = (/**
         * @param {?} _
         * @param {?} item
         * @return {?}
         */
        (_, item) => item.name);
    }
    /**
     * @return {?}
     */
    get selectedGroupPermissions$() {
        return this.groups$.pipe(map((/**
         * @param {?} groups
         * @return {?}
         */
        groups => this.selectedGroup ? groups.find((/**
         * @param {?} group
         * @return {?}
         */
        group => group.name === this.selectedGroup.name)).permissions : [])), map((/**
         * @param {?} permissions
         * @return {?}
         */
        permissions => permissions.map((/**
         * @param {?} permission
         * @return {?}
         */
        permission => ((/** @type {?} */ (((/** @type {?} */ (Object.assign({}, permission, { margin: findMargin(permissions, permission), isGranted: this.permissions.find((/**
             * @param {?} per
             * @return {?}
             */
            per => per.name === permission.name)).isGranted }))))))))))));
    }
    /**
     * @return {?}
     */
    ngOnInit() { }
    /**
     * @param {?} name
     * @return {?}
     */
    getChecked(name) {
        return (this.permissions.find((/**
         * @param {?} per
         * @return {?}
         */
        per => per.name === name)) || { isGranted: false }).isGranted;
    }
    /**
     * @param {?} clickedPermission
     * @param {?} value
     * @return {?}
     */
    onClickCheckbox(clickedPermission, value) {
        setTimeout((/**
         * @return {?}
         */
        () => {
            this.permissions = this.permissions.map((/**
             * @param {?} per
             * @return {?}
             */
            per => {
                if (clickedPermission.name === per.name) {
                    return Object.assign({}, per, { isGranted: !per.isGranted });
                }
                else if (clickedPermission.name === per.parentName && clickedPermission.isGranted) {
                    return Object.assign({}, per, { isGranted: false });
                }
                else if (clickedPermission.parentName === per.name && !clickedPermission.isGranted) {
                    return Object.assign({}, per, { isGranted: true });
                }
                return per;
            }));
            this.setTabCheckboxState();
            this.setGrantCheckboxState();
        }), 0);
    }
    /**
     * @return {?}
     */
    setTabCheckboxState() {
        this.selectedGroupPermissions$.pipe(take(1)).subscribe((/**
         * @param {?} permissions
         * @return {?}
         */
        permissions => {
            /** @type {?} */
            const selectedPermissions = permissions.filter((/**
             * @param {?} per
             * @return {?}
             */
            per => per.isGranted));
            /** @type {?} */
            const element = (/** @type {?} */ (document.querySelector('#select-all-in-this-tabs')));
            if (selectedPermissions.length === permissions.length) {
                element.indeterminate = false;
                this.selectThisTab = true;
            }
            else if (selectedPermissions.length === 0) {
                element.indeterminate = false;
                this.selectThisTab = false;
            }
            else {
                element.indeterminate = true;
            }
        }));
    }
    /**
     * @return {?}
     */
    setGrantCheckboxState() {
        /** @type {?} */
        const selectedAllPermissions = this.permissions.filter((/**
         * @param {?} per
         * @return {?}
         */
        per => per.isGranted));
        /** @type {?} */
        const checkboxElement = (/** @type {?} */ (document.querySelector('#select-all-in-all-tabs')));
        if (selectedAllPermissions.length === this.permissions.length) {
            checkboxElement.indeterminate = false;
            this.selectAllTab = true;
        }
        else if (selectedAllPermissions.length === 0) {
            checkboxElement.indeterminate = false;
            this.selectAllTab = false;
        }
        else {
            checkboxElement.indeterminate = true;
        }
    }
    /**
     * @return {?}
     */
    onClickSelectThisTab() {
        this.selectedGroupPermissions$.pipe(take(1)).subscribe((/**
         * @param {?} permissions
         * @return {?}
         */
        permissions => {
            permissions.forEach((/**
             * @param {?} permission
             * @return {?}
             */
            permission => {
                /** @type {?} */
                const index = this.permissions.findIndex((/**
                 * @param {?} per
                 * @return {?}
                 */
                per => per.name === permission.name));
                this.permissions = [
                    ...this.permissions.slice(0, index),
                    Object.assign({}, this.permissions[index], { isGranted: !this.selectThisTab }),
                    ...this.permissions.slice(index + 1),
                ];
            }));
        }));
        this.setGrantCheckboxState();
    }
    /**
     * @return {?}
     */
    onClickSelectAll() {
        this.permissions = this.permissions.map((/**
         * @param {?} permission
         * @return {?}
         */
        permission => (Object.assign({}, permission, { isGranted: !this.selectAllTab }))));
        this.selectThisTab = !this.selectAllTab;
    }
    /**
     * @param {?} group
     * @return {?}
     */
    onChangeGroup(group) {
        this.selectedGroup = group;
        this.setTabCheckboxState();
    }
    /**
     * @return {?}
     */
    onSubmit() {
        /** @type {?} */
        const unchangedPermissions = getPermissions(this.store.selectSnapshot(PermissionManagementState.getPermissionGroups));
        /** @type {?} */
        const changedPermissions = this.permissions
            .filter((/**
         * @param {?} per
         * @return {?}
         */
        per => unchangedPermissions.find((/**
         * @param {?} unchanged
         * @return {?}
         */
        unchanged => unchanged.name === per.name)).isGranted === per.isGranted ? false : true))
            .map((/**
         * @param {?} __0
         * @return {?}
         */
        ({ name, isGranted }) => ({ name, isGranted })));
        if (changedPermissions.length) {
            this.store
                .dispatch(new PermissionManagementUpdatePermissions({
                providerKey: this.providerKey,
                providerName: this.providerName,
                permissions: changedPermissions,
            }))
                .subscribe((/**
             * @return {?}
             */
            () => this.modalRef.close()));
        }
        else {
            this.modalRef.close();
        }
    }
    /**
     * @return {?}
     */
    openModal() {
        if (!this.providerKey || !this.providerName) {
            throw new Error('Provider Key and Provider Name are required.');
        }
        this.store
            .dispatch(new PermissionManagementGetPermissions({ providerKey: this.providerKey, providerName: this.providerName }))
            .pipe(pluck('PermissionManagementState', 'permissionRes'))
            .subscribe((/**
         * @param {?} permissionRes
         * @return {?}
         */
        (permissionRes) => {
            this.selectedGroup = permissionRes.groups[0];
            this.permissions = getPermissions(permissionRes.groups);
            this.modalRef = this.modalService.open(this.modalContent, { size: 'lg' });
            this.visibleChange.emit(true);
            setTimeout((/**
             * @return {?}
             */
            () => {
                this.setTabCheckboxState();
                this.setGrantCheckboxState();
            }), 0);
            from(this.modalRef.result)
                .pipe(take(1))
                .subscribe((/**
             * @param {?} data
             * @return {?}
             */
            data => {
                this.setVisible(false);
            }), (/**
             * @param {?} reason
             * @return {?}
             */
            reason => {
                this.setVisible(false);
            }));
        }));
    }
    /**
     * @param {?} value
     * @return {?}
     */
    setVisible(value) {
        this.visible = value;
        this.visibleChange.emit(value);
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    ngOnChanges({ visible }) {
        if (!visible)
            return;
        if (visible.currentValue) {
            this.openModal();
        }
        else if (visible.currentValue === false && this.modalService.hasOpenModals()) {
            this.modalRef.close();
        }
    }
}
PermissionManagementComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-permission-management',
                template: "<ng-template #modalContent let-modal>\n  <ng-container *ngIf=\"{ entityName: entityName$ | async } as data\">\n    <div class=\"modal-header\">\n      <h4 class=\"modal-title\" id=\"modal-basic-title\">\n        {{ 'AbpPermissionManagement::Permissions' | abpLocalization }} -\n        {{ data.entityName }}\n      </h4>\n      <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n        <span aria-hidden=\"true\">&times;</span>\n      </button>\n    </div>\n    <div class=\"modal-body\">\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input\n          type=\"checkbox\"\n          id=\"select-all-in-all-tabs\"\n          name=\"select-all-in-all-tabs\"\n          class=\"custom-control-input\"\n          [(ngModel)]=\"selectAllTab\"\n          (click)=\"onClickSelectAll()\"\n        />\n        <label class=\"custom-control-label\" for=\"select-all-in-all-tabs\">{{\n          'AbpPermissionManagement::SelectAllInAllTabs' | abpLocalization\n        }}</label>\n      </div>\n\n      <hr class=\"mt-2 mb-2\" />\n      <div class=\"row\">\n        <div class=\"col-4\">\n          <ul class=\"nav nav-pills flex-column\">\n            <perfect-scrollbar class=\"ps-show-always\" style=\"max-height: 70vh;\">\n              <li *ngFor=\"let group of groups$ | async; trackBy: trackByFn\" class=\"nav-item\">\n                <a class=\"nav-link\" [class.active]=\"selectedGroup.name === group.name\" (click)=\"onChangeGroup(group)\">{{\n                  group.displayName\n                }}</a>\n              </li>\n            </perfect-scrollbar>\n          </ul>\n        </div>\n        <div class=\"col-8\">\n          <h4>{{ selectedGroup.displayName }}</h4>\n          <hr class=\"mt-2 mb-3\" />\n          <div class=\"pl-1 pt-1\">\n            <div class=\"custom-checkbox custom-control mb-2\">\n              <input\n                type=\"checkbox\"\n                id=\"select-all-in-this-tabs\"\n                name=\"select-all-in-this-tabs\"\n                class=\"custom-control-input\"\n                [(ngModel)]=\"selectThisTab\"\n                (click)=\"onClickSelectThisTab()\"\n              />\n              <label class=\"custom-control-label\" for=\"select-all-in-this-tabs\">{{\n                'AbpPermissionManagement::SelectAllInThisTab' | abpLocalization\n              }}</label>\n            </div>\n            <hr class=\"mb-3\" />\n            <perfect-scrollbar class=\"ps-show-always\" style=\"max-height: 60vh;\">\n              <div\n                *ngFor=\"let permission of selectedGroupPermissions$ | async; let i = index; trackBy: trackByFn\"\n                [style.margin-left]=\"permission.margin + 'px'\"\n                class=\"custom-checkbox custom-control mb-2\"\n              >\n                <input\n                  #permissionCheckbox\n                  type=\"checkbox\"\n                  [checked]=\"getChecked(permission.name)\"\n                  [value]=\"getChecked(permission.name)\"\n                  [attr.id]=\"permission.name\"\n                  class=\"custom-control-input\"\n                />\n                <label\n                  class=\"custom-control-label\"\n                  [attr.for]=\"permission.name\"\n                  (click)=\"onClickCheckbox(permission, permissionCheckbox.value)\"\n                  >{{ permission.displayName }}</label\n                >\n              </div>\n            </perfect-scrollbar>\n          </div>\n        </div>\n      </div>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\" (click)=\"onSubmit()\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </ng-container>\n</ng-template>\n"
            }] }
];
/** @nocollapse */
PermissionManagementComponent.ctorParameters = () => [
    { type: NgbModal },
    { type: Store },
    { type: Renderer2 }
];
PermissionManagementComponent.propDecorators = {
    providerName: [{ type: Input }],
    providerKey: [{ type: Input }],
    visible: [{ type: Input }],
    visibleChange: [{ type: Output }],
    modalContent: [{ type: ViewChild, args: ['modalContent', { static: false },] }]
};
__decorate([
    Select(PermissionManagementState.getPermissionGroups),
    __metadata("design:type", Observable)
], PermissionManagementComponent.prototype, "groups$", void 0);
__decorate([
    Select(PermissionManagementState.getEntitiyDisplayName),
    __metadata("design:type", Observable)
], PermissionManagementComponent.prototype, "entityName$", void 0);
/**
 * @param {?} permissions
 * @param {?} permission
 * @return {?}
 */
function findMargin(permissions, permission) {
    /** @type {?} */
    const parentPermission = permissions.find((/**
     * @param {?} per
     * @return {?}
     */
    per => per.name === permission.parentName));
    if (parentPermission && parentPermission.parentName) {
        /** @type {?} */
        let margin = 20;
        return (margin += findMargin(permissions, parentPermission));
    }
    return parentPermission ? 20 : 0;
}
/**
 * @param {?} groups
 * @return {?}
 */
function getPermissions(groups) {
    return groups.reduce((/**
     * @param {?} acc
     * @param {?} val
     * @return {?}
     */
    (acc, val) => [...acc, ...val.permissions]), []);
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class PermissionManagementModule {
}
PermissionManagementModule.decorators = [
    { type: NgModule, args: [{
                declarations: [PermissionManagementComponent],
                imports: [CoreModule, ThemeSharedModule, NgxsModule.forFeature([PermissionManagementState]), PerfectScrollbarModule],
                exports: [PermissionManagementComponent],
            },] }
];

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var PermissionManagement;
(function (PermissionManagement) {
    /**
     * @record
     */
    function State() { }
    PermissionManagement.State = State;
    /**
     * @record
     */
    function Response() { }
    PermissionManagement.Response = Response;
    /**
     * @record
     */
    function Group() { }
    PermissionManagement.Group = Group;
    /**
     * @record
     */
    function MinimumPermission() { }
    PermissionManagement.MinimumPermission = MinimumPermission;
    /**
     * @record
     */
    function Permission() { }
    PermissionManagement.Permission = Permission;
    /**
     * @record
     */
    function GrantedProvider() { }
    PermissionManagement.GrantedProvider = GrantedProvider;
    /**
     * @record
     */
    function UpdateRequest() { }
    PermissionManagement.UpdateRequest = UpdateRequest;
})(PermissionManagement || (PermissionManagement = {}));

export { PermissionManagementComponent, PermissionManagementGetPermissions, PermissionManagementModule, PermissionManagementService, PermissionManagementState, PermissionManagementUpdatePermissions, PermissionManagementComponent as ɵa, PermissionManagementState as ɵb, PermissionManagementService as ɵc, PermissionManagementGetPermissions as ɵd, PermissionManagementUpdatePermissions as ɵe };
//# sourceMappingURL=abp-ng.permission-management.js.map
