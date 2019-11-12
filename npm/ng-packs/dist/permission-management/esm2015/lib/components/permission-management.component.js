/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/permission-management.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Component, EventEmitter, Input, Output, Renderer2, } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { map, pluck, take, finalize } from 'rxjs/operators';
import { GetPermissions, UpdatePermissions } from '../actions/permission-management.actions';
import { PermissionManagementState } from '../states/permission-management.state';
export class PermissionManagementComponent {
    /**
     * @param {?} store
     * @param {?} renderer
     */
    constructor(store, renderer) {
        this.store = store;
        this.renderer = renderer;
        this.visibleChange = new EventEmitter();
        this.permissions = [];
        this.selectThisTab = false;
        this.selectAllTab = false;
        this.modalBusy = false;
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
    get visible() {
        return this._visible;
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set visible(value) {
        if (!this.selectedGroup)
            return;
        this._visible = value;
        this.visibleChange.emit(value);
        if (!value) {
            this.selectedGroup = null;
        }
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
     * @param {?} grantedProviders
     * @return {?}
     */
    isGrantedByOtherProviderName(grantedProviders) {
        if (grantedProviders.length) {
            return grantedProviders.findIndex((/**
             * @param {?} p
             * @return {?}
             */
            p => p.providerName !== this.providerName)) > -1;
        }
        return false;
    }
    /**
     * @param {?} clickedPermission
     * @param {?} value
     * @return {?}
     */
    onClickCheckbox(clickedPermission, value) {
        if (clickedPermission.isGranted && this.isGrantedByOtherProviderName(clickedPermission.grantedProviders))
            return;
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
                if (permission.isGranted && this.isGrantedByOtherProviderName(permission.grantedProviders))
                    return;
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
        permission => (Object.assign({}, permission, { isGranted: this.isGrantedByOtherProviderName(permission.grantedProviders) || !this.selectAllTab }))));
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
    submit() {
        this.modalBusy = true;
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
                .dispatch(new UpdatePermissions({
                providerKey: this.providerKey,
                providerName: this.providerName,
                permissions: changedPermissions,
            }))
                .pipe(finalize((/**
             * @return {?}
             */
            () => (this.modalBusy = false))))
                .subscribe((/**
             * @return {?}
             */
            () => {
                this.visible = false;
            }));
        }
        else {
            this.modalBusy = false;
            this.visible = false;
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
            .dispatch(new GetPermissions({
            providerKey: this.providerKey,
            providerName: this.providerName,
        }))
            .pipe(pluck('PermissionManagementState', 'permissionRes'))
            .subscribe((/**
         * @param {?} permissionRes
         * @return {?}
         */
        (permissionRes) => {
            this.selectedGroup = permissionRes.groups[0];
            this.permissions = getPermissions(permissionRes.groups);
            this.visible = true;
        }));
    }
    /**
     * @return {?}
     */
    initModal() {
        this.setTabCheckboxState();
        this.setGrantCheckboxState();
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
        else if (visible.currentValue === false && this.visible) {
            this.visible = false;
        }
    }
}
PermissionManagementComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-permission-management',
                template: "<abp-modal [(visible)]=\"visible\" (init)=\"initModal()\" [busy]=\"modalBusy\">\r\n  <ng-container *ngIf=\"{ entityName: entityName$ | async } as data\">\r\n    <ng-template #abpHeader>\r\n      <h4>{{ 'AbpPermissionManagement::Permissions' | abpLocalization }} - {{ data.entityName }}</h4>\r\n    </ng-template>\r\n    <ng-template #abpBody>\r\n      <div class=\"custom-checkbox custom-control mb-2\">\r\n        <input\r\n          type=\"checkbox\"\r\n          id=\"select-all-in-all-tabs\"\r\n          name=\"select-all-in-all-tabs\"\r\n          class=\"custom-control-input\"\r\n          [(ngModel)]=\"selectAllTab\"\r\n          (click)=\"onClickSelectAll()\"\r\n        />\r\n        <label class=\"custom-control-label\" for=\"select-all-in-all-tabs\">{{\r\n          'AbpPermissionManagement::SelectAllInAllTabs' | abpLocalization\r\n        }}</label>\r\n      </div>\r\n\r\n      <hr class=\"mt-2 mb-2\" />\r\n      <div class=\"row\">\r\n        <div class=\"col-4\">\r\n          <ul class=\"nav nav-pills flex-column\">\r\n            <li *ngFor=\"let group of groups$ | async; trackBy: trackByFn\" class=\"nav-item\">\r\n              <a\r\n                class=\"nav-link pointer\"\r\n                [class.active]=\"selectedGroup?.name === group?.name\"\r\n                (click)=\"onChangeGroup(group)\"\r\n                >{{ group?.displayName }}</a\r\n              >\r\n            </li>\r\n          </ul>\r\n        </div>\r\n        <div class=\"col-8\">\r\n          <h4>{{ selectedGroup?.displayName }}</h4>\r\n          <hr class=\"mt-2 mb-3\" />\r\n          <div class=\"pl-1 pt-1\">\r\n            <div class=\"custom-checkbox custom-control mb-2\">\r\n              <input\r\n                type=\"checkbox\"\r\n                id=\"select-all-in-this-tabs\"\r\n                name=\"select-all-in-this-tabs\"\r\n                class=\"custom-control-input\"\r\n                [(ngModel)]=\"selectThisTab\"\r\n                (click)=\"onClickSelectThisTab()\"\r\n              />\r\n              <label class=\"custom-control-label\" for=\"select-all-in-this-tabs\">{{\r\n                'AbpPermissionManagement::SelectAllInThisTab' | abpLocalization\r\n              }}</label>\r\n            </div>\r\n            <hr class=\"mb-3\" />\r\n            <div\r\n              *ngFor=\"let permission of selectedGroupPermissions$ | async; let i = index; trackBy: trackByFn\"\r\n              [style.margin-left]=\"permission.margin + 'px'\"\r\n              class=\"custom-checkbox custom-control mb-2\"\r\n            >\r\n              <input\r\n                #permissionCheckbox\r\n                type=\"checkbox\"\r\n                [checked]=\"getChecked(permission.name)\"\r\n                [value]=\"getChecked(permission.name)\"\r\n                [attr.id]=\"permission.name\"\r\n                class=\"custom-control-input\"\r\n                [disabled]=\"isGrantedByOtherProviderName(permission.grantedProviders)\"\r\n              />\r\n              <label\r\n                class=\"custom-control-label\"\r\n                [attr.for]=\"permission.name\"\r\n                (click)=\"onClickCheckbox(permission, permissionCheckbox.value)\"\r\n                >{{ permission.displayName }}\r\n                <span *ngFor=\"let provider of permission.grantedProviders\" class=\"badge badge-light\"\r\n                  >{{ provider.providerName }}: {{ provider.providerKey }}</span\r\n                ></label\r\n              >\r\n            </div>\r\n          </div>\r\n        </div>\r\n      </div>\r\n    </ng-template>\r\n    <ng-template #abpFooter>\r\n      <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\r\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\r\n      </button>\r\n      <abp-button iconClass=\"fa fa-check\" (click)=\"submit()\">{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button>\r\n    </ng-template>\r\n  </ng-container>\r\n</abp-modal>\r\n"
            }] }
];
/** @nocollapse */
PermissionManagementComponent.ctorParameters = () => [
    { type: Store },
    { type: Renderer2 }
];
PermissionManagementComponent.propDecorators = {
    providerName: [{ type: Input }],
    providerKey: [{ type: Input }],
    visible: [{ type: Input }],
    visibleChange: [{ type: Output }]
};
tslib_1.__decorate([
    Select(PermissionManagementState.getPermissionGroups),
    tslib_1.__metadata("design:type", Observable)
], PermissionManagementComponent.prototype, "groups$", void 0);
tslib_1.__decorate([
    Select(PermissionManagementState.getEntityDisplayName),
    tslib_1.__metadata("design:type", Observable)
], PermissionManagementComponent.prototype, "entityName$", void 0);
if (false) {
    /** @type {?} */
    PermissionManagementComponent.prototype.providerName;
    /** @type {?} */
    PermissionManagementComponent.prototype.providerKey;
    /**
     * @type {?}
     * @protected
     */
    PermissionManagementComponent.prototype._visible;
    /** @type {?} */
    PermissionManagementComponent.prototype.visibleChange;
    /** @type {?} */
    PermissionManagementComponent.prototype.groups$;
    /** @type {?} */
    PermissionManagementComponent.prototype.entityName$;
    /** @type {?} */
    PermissionManagementComponent.prototype.selectedGroup;
    /** @type {?} */
    PermissionManagementComponent.prototype.permissions;
    /** @type {?} */
    PermissionManagementComponent.prototype.selectThisTab;
    /** @type {?} */
    PermissionManagementComponent.prototype.selectAllTab;
    /** @type {?} */
    PermissionManagementComponent.prototype.modalBusy;
    /** @type {?} */
    PermissionManagementComponent.prototype.trackByFn;
    /**
     * @type {?}
     * @private
     */
    PermissionManagementComponent.prototype.store;
    /**
     * @type {?}
     * @private
     */
    PermissionManagementComponent.prototype.renderer;
}
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcucGVybWlzc2lvbi1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQ0wsU0FBUyxFQUNULFlBQVksRUFDWixLQUFLLEVBR0wsTUFBTSxFQUNOLFNBQVMsR0FHVixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1QyxPQUFPLEVBQVEsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ3hDLE9BQU8sRUFBRSxHQUFHLEVBQUUsS0FBSyxFQUFFLElBQUksRUFBRSxRQUFRLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUM1RCxPQUFPLEVBQUUsY0FBYyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sMENBQTBDLENBQUM7QUFFN0YsT0FBTyxFQUFFLHlCQUF5QixFQUFFLE1BQU0sdUNBQXVDLENBQUM7QUFVbEYsTUFBTSxPQUFPLDZCQUE2Qjs7Ozs7SUErRHhDLFlBQW9CLEtBQVksRUFBVSxRQUFtQjtRQUF6QyxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQVUsYUFBUSxHQUFSLFFBQVEsQ0FBVztRQXRDMUMsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBVyxDQUFDO1FBVS9ELGdCQUFXLEdBQXNDLEVBQUUsQ0FBQztRQUVwRCxrQkFBYSxHQUFHLEtBQUssQ0FBQztRQUV0QixpQkFBWSxHQUFHLEtBQUssQ0FBQztRQUVyQixjQUFTLEdBQUcsS0FBSyxDQUFDO1FBRWxCLGNBQVM7Ozs7O1FBQWdELENBQUMsQ0FBQyxFQUFFLElBQUksRUFBRSxFQUFFLENBQUMsSUFBSSxDQUFDLElBQUksRUFBQztJQW9CaEIsQ0FBQzs7OztJQXREakUsSUFD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Z0M7O0lBRWhDLHNEQUEwQzs7SUFFMUMsb0RBQW9EOztJQUVwRCxzREFBc0I7O0lBRXRCLHFEQUFxQjs7SUFFckIsa0RBQWtCOztJQUVsQixrREFBZ0Y7Ozs7O0lBb0JwRSw4Q0FBb0I7Ozs7O0lBQUUsaURBQTJCOzs7Ozs7O0FBd0svRCxTQUFTLFVBQVUsQ0FBQyxXQUE4QyxFQUFFLFVBQTJDOztVQUN2RyxnQkFBZ0IsR0FBRyxXQUFXLENBQUMsSUFBSTs7OztJQUFDLEdBQUcsQ0FBQyxFQUFFLENBQUMsR0FBRyxDQUFDLElBQUksS0FBSyxVQUFVLENBQUMsVUFBVSxFQUFDO0lBRXBGLElBQUksZ0JBQWdCLElBQUksZ0JBQWdCLENBQUMsVUFBVSxFQUFFOztZQUMvQyxNQUFNLEdBQUcsRUFBRTtRQUNmLE9BQU8sQ0FBQyxNQUFNLElBQUksVUFBVSxDQUFDLFdBQVcsRUFBRSxnQkFBZ0IsQ0FBQyxDQUFDLENBQUM7S0FDOUQ7SUFFRCxPQUFPLGdCQUFnQixDQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQztBQUNuQyxDQUFDOzs7OztBQUVELFNBQVMsY0FBYyxDQUFDLE1BQW9DO0lBQzFELE9BQU8sTUFBTSxDQUFDLE1BQU07Ozs7O0lBQUMsQ0FBQyxHQUFHLEVBQUUsR0FBRyxFQUFFLEVBQUUsQ0FBQyxDQUFDLEdBQUcsR0FBRyxFQUFFLEdBQUcsR0FBRyxDQUFDLFdBQVcsQ0FBQyxHQUFFLEVBQUUsQ0FBQyxDQUFDO0FBQ3ZFLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xyXG4gIENvbXBvbmVudCxcclxuICBFdmVudEVtaXR0ZXIsXHJcbiAgSW5wdXQsXHJcbiAgT25DaGFuZ2VzLFxyXG4gIE9uSW5pdCxcclxuICBPdXRwdXQsXHJcbiAgUmVuZGVyZXIyLFxyXG4gIFNpbXBsZUNoYW5nZXMsXHJcbiAgVHJhY2tCeUZ1bmN0aW9uLFxyXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBmcm9tLCBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IG1hcCwgcGx1Y2ssIHRha2UsIGZpbmFsaXplIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5pbXBvcnQgeyBHZXRQZXJtaXNzaW9ucywgVXBkYXRlUGVybWlzc2lvbnMgfSBmcm9tICcuLi9hY3Rpb25zL3Blcm1pc3Npb24tbWFuYWdlbWVudC5hY3Rpb25zJztcclxuaW1wb3J0IHsgUGVybWlzc2lvbk1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvcGVybWlzc2lvbi1tYW5hZ2VtZW50JztcclxuaW1wb3J0IHsgUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQuc3RhdGUnO1xyXG5cclxudHlwZSBQZXJtaXNzaW9uV2l0aE1hcmdpbiA9IFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb24gJiB7XHJcbiAgbWFyZ2luOiBudW1iZXI7XHJcbn07XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1wZXJtaXNzaW9uLW1hbmFnZW1lbnQnLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9wZXJtaXNzaW9uLW1hbmFnZW1lbnQuY29tcG9uZW50Lmh0bWwnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgUGVybWlzc2lvbk1hbmFnZW1lbnRDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQsIE9uQ2hhbmdlcyB7XHJcbiAgQElucHV0KClcclxuICBwcm92aWRlck5hbWU6IHN0cmluZztcclxuXHJcbiAgQElucHV0KClcclxuICBwcm92aWRlcktleTogc3RyaW5nO1xyXG5cclxuICBwcm90ZWN0ZWQgX3Zpc2libGU7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgZ2V0IHZpc2libGUoKTogYm9vbGVhbiB7XHJcbiAgICByZXR1cm4gdGhpcy5fdmlzaWJsZTtcclxuICB9XHJcblxyXG4gIHNldCB2aXNpYmxlKHZhbHVlOiBib29sZWFuKSB7XHJcbiAgICBpZiAoIXRoaXMuc2VsZWN0ZWRHcm91cCkgcmV0dXJuO1xyXG5cclxuICAgIHRoaXMuX3Zpc2libGUgPSB2YWx1ZTtcclxuICAgIHRoaXMudmlzaWJsZUNoYW5nZS5lbWl0KHZhbHVlKTtcclxuXHJcbiAgICBpZiAoIXZhbHVlKSB7XHJcbiAgICAgIHRoaXMuc2VsZWN0ZWRHcm91cCA9IG51bGw7XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgdmlzaWJsZUNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8Ym9vbGVhbj4oKTtcclxuXHJcbiAgQFNlbGVjdChQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlLmdldFBlcm1pc3Npb25Hcm91cHMpXHJcbiAgZ3JvdXBzJDogT2JzZXJ2YWJsZTxQZXJtaXNzaW9uTWFuYWdlbWVudC5Hcm91cFtdPjtcclxuXHJcbiAgQFNlbGVjdChQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlLmdldEVudGl0eURpc3BsYXlOYW1lKVxyXG4gIGVudGl0eU5hbWUkOiBPYnNlcnZhYmxlPHN0cmluZz47XHJcblxyXG4gIHNlbGVjdGVkR3JvdXA6IFBlcm1pc3Npb25NYW5hZ2VtZW50Lkdyb3VwO1xyXG5cclxuICBwZXJtaXNzaW9uczogUGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbltdID0gW107XHJcblxyXG4gIHNlbGVjdFRoaXNUYWIgPSBmYWxzZTtcclxuXHJcbiAgc2VsZWN0QWxsVGFiID0gZmFsc2U7XHJcblxyXG4gIG1vZGFsQnVzeSA9IGZhbHNlO1xyXG5cclxuICB0cmFja0J5Rm46IFRyYWNrQnlGdW5jdGlvbjxQZXJtaXNzaW9uTWFuYWdlbWVudC5Hcm91cD4gPSAoXywgaXRlbSkgPT4gaXRlbS5uYW1lO1xyXG5cclxuICBnZXQgc2VsZWN0ZWRHcm91cFBlcm1pc3Npb25zJCgpOiBPYnNlcnZhYmxlPFBlcm1pc3Npb25XaXRoTWFyZ2luW10+IHtcclxuICAgIHJldHVybiB0aGlzLmdyb3VwcyQucGlwZShcclxuICAgICAgbWFwKGdyb3VwcyA9PlxyXG4gICAgICAgIHRoaXMuc2VsZWN0ZWRHcm91cCA/IGdyb3Vwcy5maW5kKGdyb3VwID0+IGdyb3VwLm5hbWUgPT09IHRoaXMuc2VsZWN0ZWRHcm91cC5uYW1lKS5wZXJtaXNzaW9ucyA6IFtdLFxyXG4gICAgICApLFxyXG4gICAgICBtYXA8UGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbltdLCBQZXJtaXNzaW9uV2l0aE1hcmdpbltdPihwZXJtaXNzaW9ucyA9PlxyXG4gICAgICAgIHBlcm1pc3Npb25zLm1hcChcclxuICAgICAgICAgIHBlcm1pc3Npb24gPT5cclxuICAgICAgICAgICAgKCh7XHJcbiAgICAgICAgICAgICAgLi4ucGVybWlzc2lvbixcclxuICAgICAgICAgICAgICBtYXJnaW46IGZpbmRNYXJnaW4ocGVybWlzc2lvbnMsIHBlcm1pc3Npb24pLFxyXG4gICAgICAgICAgICAgIGlzR3JhbnRlZDogdGhpcy5wZXJtaXNzaW9ucy5maW5kKHBlciA9PiBwZXIubmFtZSA9PT0gcGVybWlzc2lvbi5uYW1lKS5pc0dyYW50ZWQsXHJcbiAgICAgICAgICAgIH0gYXMgYW55KSBhcyBQZXJtaXNzaW9uV2l0aE1hcmdpbiksXHJcbiAgICAgICAgKSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSwgcHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyKSB7fVxyXG5cclxuICBuZ09uSW5pdCgpOiB2b2lkIHt9XHJcblxyXG4gIGdldENoZWNrZWQobmFtZTogc3RyaW5nKSB7XHJcbiAgICByZXR1cm4gKHRoaXMucGVybWlzc2lvbnMuZmluZChwZXIgPT4gcGVyLm5hbWUgPT09IG5hbWUpIHx8IHsgaXNHcmFudGVkOiBmYWxzZSB9KS5pc0dyYW50ZWQ7XHJcbiAgfVxyXG5cclxuICBpc0dyYW50ZWRCeU90aGVyUHJvdmlkZXJOYW1lKGdyYW50ZWRQcm92aWRlcnM6IFBlcm1pc3Npb25NYW5hZ2VtZW50LkdyYW50ZWRQcm92aWRlcltdKTogYm9vbGVhbiB7XHJcbiAgICBpZiAoZ3JhbnRlZFByb3ZpZGVycy5sZW5ndGgpIHtcclxuICAgICAgcmV0dXJuIGdyYW50ZWRQcm92aWRlcnMuZmluZEluZGV4KHAgPT4gcC5wcm92aWRlck5hbWUgIT09IHRoaXMucHJvdmlkZXJOYW1lKSA+IC0xO1xyXG4gICAgfVxyXG4gICAgcmV0dXJuIGZhbHNlO1xyXG4gIH1cclxuXHJcbiAgb25DbGlja0NoZWNrYm94KGNsaWNrZWRQZXJtaXNzaW9uOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uLCB2YWx1ZSkge1xyXG4gICAgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLmlzR3JhbnRlZCAmJiB0aGlzLmlzR3JhbnRlZEJ5T3RoZXJQcm92aWRlck5hbWUoY2xpY2tlZFBlcm1pc3Npb24uZ3JhbnRlZFByb3ZpZGVycykpIHJldHVybjtcclxuXHJcbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcclxuICAgICAgdGhpcy5wZXJtaXNzaW9ucyA9IHRoaXMucGVybWlzc2lvbnMubWFwKHBlciA9PiB7XHJcbiAgICAgICAgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLm5hbWUgPT09IHBlci5uYW1lKSB7XHJcbiAgICAgICAgICByZXR1cm4geyAuLi5wZXIsIGlzR3JhbnRlZDogIXBlci5pc0dyYW50ZWQgfTtcclxuICAgICAgICB9IGVsc2UgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLm5hbWUgPT09IHBlci5wYXJlbnROYW1lICYmIGNsaWNrZWRQZXJtaXNzaW9uLmlzR3JhbnRlZCkge1xyXG4gICAgICAgICAgcmV0dXJuIHsgLi4ucGVyLCBpc0dyYW50ZWQ6IGZhbHNlIH07XHJcbiAgICAgICAgfSBlbHNlIGlmIChjbGlja2VkUGVybWlzc2lvbi5wYXJlbnROYW1lID09PSBwZXIubmFtZSAmJiAhY2xpY2tlZFBlcm1pc3Npb24uaXNHcmFudGVkKSB7XHJcbiAgICAgICAgICByZXR1cm4geyAuLi5wZXIsIGlzR3JhbnRlZDogdHJ1ZSB9O1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgcmV0dXJuIHBlcjtcclxuICAgICAgfSk7XHJcblxyXG4gICAgICB0aGlzLnNldFRhYkNoZWNrYm94U3RhdGUoKTtcclxuICAgICAgdGhpcy5zZXRHcmFudENoZWNrYm94U3RhdGUoKTtcclxuICAgIH0sIDApO1xyXG4gIH1cclxuXHJcbiAgc2V0VGFiQ2hlY2tib3hTdGF0ZSgpIHtcclxuICAgIHRoaXMuc2VsZWN0ZWRHcm91cFBlcm1pc3Npb25zJC5waXBlKHRha2UoMSkpLnN1YnNjcmliZShwZXJtaXNzaW9ucyA9PiB7XHJcbiAgICAgIGNvbnN0IHNlbGVjdGVkUGVybWlzc2lvbnMgPSBwZXJtaXNzaW9ucy5maWx0ZXIocGVyID0+IHBlci5pc0dyYW50ZWQpO1xyXG4gICAgICBjb25zdCBlbGVtZW50ID0gZG9jdW1lbnQucXVlcnlTZWxlY3RvcignI3NlbGVjdC1hbGwtaW4tdGhpcy10YWJzJykgYXMgYW55O1xyXG5cclxuICAgICAgaWYgKHNlbGVjdGVkUGVybWlzc2lvbnMubGVuZ3RoID09PSBwZXJtaXNzaW9ucy5sZW5ndGgpIHtcclxuICAgICAgICBlbGVtZW50LmluZGV0ZXJtaW5hdGUgPSBmYWxzZTtcclxuICAgICAgICB0aGlzLnNlbGVjdFRoaXNUYWIgPSB0cnVlO1xyXG4gICAgICB9IGVsc2UgaWYgKHNlbGVjdGVkUGVybWlzc2lvbnMubGVuZ3RoID09PSAwKSB7XHJcbiAgICAgICAgZWxlbWVudC5pbmRldGVybWluYXRlID0gZmFsc2U7XHJcbiAgICAgICAgdGhpcy5zZWxlY3RUaGlzVGFiID0gZmFsc2U7XHJcbiAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgZWxlbWVudC5pbmRldGVybWluYXRlID0gdHJ1ZTtcclxuICAgICAgfVxyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBzZXRHcmFudENoZWNrYm94U3RhdGUoKSB7XHJcbiAgICBjb25zdCBzZWxlY3RlZEFsbFBlcm1pc3Npb25zID0gdGhpcy5wZXJtaXNzaW9ucy5maWx0ZXIocGVyID0+IHBlci5pc0dyYW50ZWQpO1xyXG4gICAgY29uc3QgY2hlY2tib3hFbGVtZW50ID0gZG9jdW1lbnQucXVlcnlTZWxlY3RvcignI3NlbGVjdC1hbGwtaW4tYWxsLXRhYnMnKSBhcyBhbnk7XHJcblxyXG4gICAgaWYgKHNlbGVjdGVkQWxsUGVybWlzc2lvbnMubGVuZ3RoID09PSB0aGlzLnBlcm1pc3Npb25zLmxlbmd0aCkge1xyXG4gICAgICBjaGVja2JveEVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IGZhbHNlO1xyXG4gICAgICB0aGlzLnNlbGVjdEFsbFRhYiA9IHRydWU7XHJcbiAgICB9IGVsc2UgaWYgKHNlbGVjdGVkQWxsUGVybWlzc2lvbnMubGVuZ3RoID09PSAwKSB7XHJcbiAgICAgIGNoZWNrYm94RWxlbWVudC5pbmRldGVybWluYXRlID0gZmFsc2U7XHJcbiAgICAgIHRoaXMuc2VsZWN0QWxsVGFiID0gZmFsc2U7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICBjaGVja2JveEVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IHRydWU7XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBvbkNsaWNrU2VsZWN0VGhpc1RhYigpIHtcclxuICAgIHRoaXMuc2VsZWN0ZWRHcm91cFBlcm1pc3Npb25zJC5waXBlKHRha2UoMSkpLnN1YnNjcmliZShwZXJtaXNzaW9ucyA9PiB7XHJcbiAgICAgIHBlcm1pc3Npb25zLmZvckVhY2gocGVybWlzc2lvbiA9PiB7XHJcbiAgICAgICAgaWYgKHBlcm1pc3Npb24uaXNHcmFudGVkICYmIHRoaXMuaXNHcmFudGVkQnlPdGhlclByb3ZpZGVyTmFtZShwZXJtaXNzaW9uLmdyYW50ZWRQcm92aWRlcnMpKSByZXR1cm47XHJcblxyXG4gICAgICAgIGNvbnN0IGluZGV4ID0gdGhpcy5wZXJtaXNzaW9ucy5maW5kSW5kZXgocGVyID0+IHBlci5uYW1lID09PSBwZXJtaXNzaW9uLm5hbWUpO1xyXG5cclxuICAgICAgICB0aGlzLnBlcm1pc3Npb25zID0gW1xyXG4gICAgICAgICAgLi4udGhpcy5wZXJtaXNzaW9ucy5zbGljZSgwLCBpbmRleCksXHJcbiAgICAgICAgICB7IC4uLnRoaXMucGVybWlzc2lvbnNbaW5kZXhdLCBpc0dyYW50ZWQ6ICF0aGlzLnNlbGVjdFRoaXNUYWIgfSxcclxuICAgICAgICAgIC4uLnRoaXMucGVybWlzc2lvbnMuc2xpY2UoaW5kZXggKyAxKSxcclxuICAgICAgICBdO1xyXG4gICAgICB9KTtcclxuICAgIH0pO1xyXG5cclxuICAgIHRoaXMuc2V0R3JhbnRDaGVja2JveFN0YXRlKCk7XHJcbiAgfVxyXG5cclxuICBvbkNsaWNrU2VsZWN0QWxsKCkge1xyXG4gICAgdGhpcy5wZXJtaXNzaW9ucyA9IHRoaXMucGVybWlzc2lvbnMubWFwKHBlcm1pc3Npb24gPT4gKHtcclxuICAgICAgLi4ucGVybWlzc2lvbixcclxuICAgICAgaXNHcmFudGVkOiB0aGlzLmlzR3JhbnRlZEJ5T3RoZXJQcm92aWRlck5hbWUocGVybWlzc2lvbi5ncmFudGVkUHJvdmlkZXJzKSB8fCAhdGhpcy5zZWxlY3RBbGxUYWIsXHJcbiAgICB9KSk7XHJcblxyXG4gICAgdGhpcy5zZWxlY3RUaGlzVGFiID0gIXRoaXMuc2VsZWN0QWxsVGFiO1xyXG4gIH1cclxuXHJcbiAgb25DaGFuZ2VHcm91cChncm91cDogUGVybWlzc2lvbk1hbmFnZW1lbnQuR3JvdXApIHtcclxuICAgIHRoaXMuc2VsZWN0ZWRHcm91cCA9IGdyb3VwO1xyXG4gICAgdGhpcy5zZXRUYWJDaGVja2JveFN0YXRlKCk7XHJcbiAgfVxyXG5cclxuICBzdWJtaXQoKSB7XHJcbiAgICB0aGlzLm1vZGFsQnVzeSA9IHRydWU7XHJcbiAgICBjb25zdCB1bmNoYW5nZWRQZXJtaXNzaW9ucyA9IGdldFBlcm1pc3Npb25zKFxyXG4gICAgICB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUuZ2V0UGVybWlzc2lvbkdyb3VwcyksXHJcbiAgICApO1xyXG5cclxuICAgIGNvbnN0IGNoYW5nZWRQZXJtaXNzaW9uczogUGVybWlzc2lvbk1hbmFnZW1lbnQuTWluaW11bVBlcm1pc3Npb25bXSA9IHRoaXMucGVybWlzc2lvbnNcclxuICAgICAgLmZpbHRlcihwZXIgPT5cclxuICAgICAgICB1bmNoYW5nZWRQZXJtaXNzaW9ucy5maW5kKHVuY2hhbmdlZCA9PiB1bmNoYW5nZWQubmFtZSA9PT0gcGVyLm5hbWUpLmlzR3JhbnRlZCA9PT0gcGVyLmlzR3JhbnRlZCA/IGZhbHNlIDogdHJ1ZSxcclxuICAgICAgKVxyXG4gICAgICAubWFwKCh7IG5hbWUsIGlzR3JhbnRlZCB9KSA9PiAoeyBuYW1lLCBpc0dyYW50ZWQgfSkpO1xyXG5cclxuICAgIGlmIChjaGFuZ2VkUGVybWlzc2lvbnMubGVuZ3RoKSB7XHJcbiAgICAgIHRoaXMuc3RvcmVcclxuICAgICAgICAuZGlzcGF0Y2goXHJcbiAgICAgICAgICBuZXcgVXBkYXRlUGVybWlzc2lvbnMoe1xyXG4gICAgICAgICAgICBwcm92aWRlcktleTogdGhpcy5wcm92aWRlcktleSxcclxuICAgICAgICAgICAgcHJvdmlkZXJOYW1lOiB0aGlzLnByb3ZpZGVyTmFtZSxcclxuICAgICAgICAgICAgcGVybWlzc2lvbnM6IGNoYW5nZWRQZXJtaXNzaW9ucyxcclxuICAgICAgICAgIH0pLFxyXG4gICAgICAgIClcclxuICAgICAgICAucGlwZShmaW5hbGl6ZSgoKSA9PiAodGhpcy5tb2RhbEJ1c3kgPSBmYWxzZSkpKVxyXG4gICAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xyXG4gICAgICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XHJcbiAgICAgICAgfSk7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICB0aGlzLm1vZGFsQnVzeSA9IGZhbHNlO1xyXG4gICAgICB0aGlzLnZpc2libGUgPSBmYWxzZTtcclxuICAgIH1cclxuICB9XHJcblxyXG4gIG9wZW5Nb2RhbCgpIHtcclxuICAgIGlmICghdGhpcy5wcm92aWRlcktleSB8fCAhdGhpcy5wcm92aWRlck5hbWUpIHtcclxuICAgICAgdGhyb3cgbmV3IEVycm9yKCdQcm92aWRlciBLZXkgYW5kIFByb3ZpZGVyIE5hbWUgYXJlIHJlcXVpcmVkLicpO1xyXG4gICAgfVxyXG5cclxuICAgIHRoaXMuc3RvcmVcclxuICAgICAgLmRpc3BhdGNoKFxyXG4gICAgICAgIG5ldyBHZXRQZXJtaXNzaW9ucyh7XHJcbiAgICAgICAgICBwcm92aWRlcktleTogdGhpcy5wcm92aWRlcktleSxcclxuICAgICAgICAgIHByb3ZpZGVyTmFtZTogdGhpcy5wcm92aWRlck5hbWUsXHJcbiAgICAgICAgfSksXHJcbiAgICAgIClcclxuICAgICAgLnBpcGUocGx1Y2soJ1Blcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUnLCAncGVybWlzc2lvblJlcycpKVxyXG4gICAgICAuc3Vic2NyaWJlKChwZXJtaXNzaW9uUmVzOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5SZXNwb25zZSkgPT4ge1xyXG4gICAgICAgIHRoaXMuc2VsZWN0ZWRHcm91cCA9IHBlcm1pc3Npb25SZXMuZ3JvdXBzWzBdO1xyXG4gICAgICAgIHRoaXMucGVybWlzc2lvbnMgPSBnZXRQZXJtaXNzaW9ucyhwZXJtaXNzaW9uUmVzLmdyb3Vwcyk7XHJcblxyXG4gICAgICAgIHRoaXMudmlzaWJsZSA9IHRydWU7XHJcbiAgICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgaW5pdE1vZGFsKCkge1xyXG4gICAgdGhpcy5zZXRUYWJDaGVja2JveFN0YXRlKCk7XHJcbiAgICB0aGlzLnNldEdyYW50Q2hlY2tib3hTdGF0ZSgpO1xyXG4gIH1cclxuXHJcbiAgbmdPbkNoYW5nZXMoeyB2aXNpYmxlIH06IFNpbXBsZUNoYW5nZXMpOiB2b2lkIHtcclxuICAgIGlmICghdmlzaWJsZSkgcmV0dXJuO1xyXG5cclxuICAgIGlmICh2aXNpYmxlLmN1cnJlbnRWYWx1ZSkge1xyXG4gICAgICB0aGlzLm9wZW5Nb2RhbCgpO1xyXG4gICAgfSBlbHNlIGlmICh2aXNpYmxlLmN1cnJlbnRWYWx1ZSA9PT0gZmFsc2UgJiYgdGhpcy52aXNpYmxlKSB7XHJcbiAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xyXG4gICAgfVxyXG4gIH1cclxufVxyXG5cclxuZnVuY3Rpb24gZmluZE1hcmdpbihwZXJtaXNzaW9uczogUGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbltdLCBwZXJtaXNzaW9uOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uKSB7XHJcbiAgY29uc3QgcGFyZW50UGVybWlzc2lvbiA9IHBlcm1pc3Npb25zLmZpbmQocGVyID0+IHBlci5uYW1lID09PSBwZXJtaXNzaW9uLnBhcmVudE5hbWUpO1xyXG5cclxuICBpZiAocGFyZW50UGVybWlzc2lvbiAmJiBwYXJlbnRQZXJtaXNzaW9uLnBhcmVudE5hbWUpIHtcclxuICAgIGxldCBtYXJnaW4gPSAyMDtcclxuICAgIHJldHVybiAobWFyZ2luICs9IGZpbmRNYXJnaW4ocGVybWlzc2lvbnMsIHBhcmVudFBlcm1pc3Npb24pKTtcclxuICB9XHJcblxyXG4gIHJldHVybiBwYXJlbnRQZXJtaXNzaW9uID8gMjAgOiAwO1xyXG59XHJcblxyXG5mdW5jdGlvbiBnZXRQZXJtaXNzaW9ucyhncm91cHM6IFBlcm1pc3Npb25NYW5hZ2VtZW50Lkdyb3VwW10pOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uW10ge1xyXG4gIHJldHVybiBncm91cHMucmVkdWNlKChhY2MsIHZhbCkgPT4gWy4uLmFjYywgLi4udmFsLnBlcm1pc3Npb25zXSwgW10pO1xyXG59XHJcbiJdfQ==