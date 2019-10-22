/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Component, EventEmitter, Input, Output, Renderer2 } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { map, pluck, take } from 'rxjs/operators';
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
    isGrantedByRole(grantedProviders) {
        if (grantedProviders.length) {
            return grantedProviders.findIndex((/**
             * @param {?} p
             * @return {?}
             */
            p => p.providerName === 'Role')) > -1;
        }
        return false;
    }
    /**
     * @param {?} clickedPermission
     * @param {?} value
     * @return {?}
     */
    onClickCheckbox(clickedPermission, value) {
        if (clickedPermission.isGranted && this.isGrantedByRole(clickedPermission.grantedProviders))
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
                if (permission.isGranted && this.isGrantedByRole(permission.grantedProviders))
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
                    ...this.permissions.slice(index + 1)
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
                permissions: changedPermissions
            }))
                .subscribe((/**
             * @return {?}
             */
            () => {
                this.modalBusy = false;
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
            providerName: this.providerName
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
                template: "<abp-modal [(visible)]=\"visible\" (init)=\"initModal()\" [busy]=\"modalBusy\">\r\n  <ng-container *ngIf=\"{ entityName: entityName$ | async } as data\">\r\n    <ng-template #abpHeader>\r\n      <h4>{{ 'AbpPermissionManagement::Permissions' | abpLocalization }} - {{ data.entityName }}</h4>\r\n    </ng-template>\r\n    <ng-template #abpBody>\r\n      <div class=\"custom-checkbox custom-control mb-2\">\r\n        <input\r\n          type=\"checkbox\"\r\n          id=\"select-all-in-all-tabs\"\r\n          name=\"select-all-in-all-tabs\"\r\n          class=\"custom-control-input\"\r\n          [(ngModel)]=\"selectAllTab\"\r\n          (click)=\"onClickSelectAll()\"\r\n        />\r\n        <label class=\"custom-control-label\" for=\"select-all-in-all-tabs\">{{\r\n          'AbpPermissionManagement::SelectAllInAllTabs' | abpLocalization\r\n        }}</label>\r\n      </div>\r\n\r\n      <hr class=\"mt-2 mb-2\" />\r\n      <div class=\"row\">\r\n        <div class=\"col-4\">\r\n          <ul class=\"nav nav-pills flex-column\">\r\n            <li *ngFor=\"let group of groups$ | async; trackBy: trackByFn\" class=\"nav-item\">\r\n              <a\r\n                class=\"nav-link pointer\"\r\n                [class.active]=\"selectedGroup?.name === group?.name\"\r\n                (click)=\"onChangeGroup(group)\"\r\n                >{{ group?.displayName }}</a\r\n              >\r\n            </li>\r\n          </ul>\r\n        </div>\r\n        <div class=\"col-8\">\r\n          <h4>{{ selectedGroup?.displayName }}</h4>\r\n          <hr class=\"mt-2 mb-3\" />\r\n          <div class=\"pl-1 pt-1\">\r\n            <div class=\"custom-checkbox custom-control mb-2\">\r\n              <input\r\n                type=\"checkbox\"\r\n                id=\"select-all-in-this-tabs\"\r\n                name=\"select-all-in-this-tabs\"\r\n                class=\"custom-control-input\"\r\n                [(ngModel)]=\"selectThisTab\"\r\n                (click)=\"onClickSelectThisTab()\"\r\n              />\r\n              <label class=\"custom-control-label\" for=\"select-all-in-this-tabs\">{{\r\n                'AbpPermissionManagement::SelectAllInThisTab' | abpLocalization\r\n              }}</label>\r\n            </div>\r\n            <hr class=\"mb-3\" />\r\n            <div\r\n              *ngFor=\"let permission of selectedGroupPermissions$ | async; let i = index; trackBy: trackByFn\"\r\n              [style.margin-left]=\"permission.margin + 'px'\"\r\n              class=\"custom-checkbox custom-control mb-2\"\r\n            >\r\n              <input\r\n                #permissionCheckbox\r\n                type=\"checkbox\"\r\n                [checked]=\"getChecked(permission.name)\"\r\n                [value]=\"getChecked(permission.name)\"\r\n                [attr.id]=\"permission.name\"\r\n                class=\"custom-control-input\"\r\n                [disabled]=\"isGrantedByRole(permission.grantedProviders)\"\r\n              />\r\n              <label\r\n                class=\"custom-control-label\"\r\n                [attr.for]=\"permission.name\"\r\n                (click)=\"onClickCheckbox(permission, permissionCheckbox.value)\"\r\n                >{{ permission.displayName }}\r\n                <span *ngFor=\"let provider of permission.grantedProviders\" class=\"badge badge-light\"\r\n                  >{{ provider.providerName }}: {{ provider.providerKey }}</span\r\n                ></label\r\n              >\r\n            </div>\r\n          </div>\r\n        </div>\r\n      </div>\r\n    </ng-template>\r\n    <ng-template #abpFooter>\r\n      <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\r\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\r\n      </button>\r\n      <abp-button iconClass=\"fa fa-check\" (click)=\"submit()\">{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button>\r\n    </ng-template>\r\n  </ng-container>\r\n</abp-modal>\r\n"
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
    Select(PermissionManagementState.getEntitiyDisplayName),
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcucGVybWlzc2lvbi1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFDTCxTQUFTLEVBQ1QsWUFBWSxFQUNaLEtBQUssRUFHTCxNQUFNLEVBQ04sU0FBUyxFQUdWLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBUSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDeEMsT0FBTyxFQUFFLEdBQUcsRUFBRSxLQUFLLEVBQUUsSUFBSSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDbEQsT0FBTyxFQUFFLGNBQWMsRUFBRSxpQkFBaUIsRUFBRSxNQUFNLDBDQUEwQyxDQUFDO0FBRTdGLE9BQU8sRUFBRSx5QkFBeUIsRUFBRSxNQUFNLHVDQUF1QyxDQUFDO0FBVWxGLE1BQU0sT0FBTyw2QkFBNkI7Ozs7O0lBK0R4QyxZQUFvQixLQUFZLEVBQVUsUUFBbUI7UUFBekMsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUFVLGFBQVEsR0FBUixRQUFRLENBQVc7UUF0QzFDLGtCQUFhLEdBQUcsSUFBSSxZQUFZLEVBQVcsQ0FBQztRQVUvRCxnQkFBVyxHQUFzQyxFQUFFLENBQUM7UUFFcEQsa0JBQWEsR0FBRyxLQUFLLENBQUM7UUFFdEIsaUJBQVksR0FBRyxLQUFLLENBQUM7UUFFckIsY0FBUyxHQUFHLEtBQUssQ0FBQztRQUVsQixjQUFTOzs7OztRQUFnRCxDQUFDLENBQUMsRUFBRSxJQUFJLEVBQUUsRUFBRSxDQUFDLElBQUksQ0FBQyxJQUFJLEVBQUM7SUFvQmhCLENBQUM7Ozs7SUF0RGpFLElBQ0ksT0FBTztRQUNU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QXdLL0QsU0FBUyxVQUFVLENBQUMsV0FBOEMsRUFBRSxVQUEyQzs7VUFDdkcsZ0JBQWdCLEdBQUcsV0FBVyxDQUFDLElBQUk7Ozs7SUFBQyxHQUFHLENBQUMsRUFBRSxDQUFDLEdBQUcsQ0FBQyxJQUFJLEtBQUssVUFBVSxDQUFDLFVBQVUsRUFBQztJQUVwRixJQUFJLGdCQUFnQixJQUFJLGdCQUFnQixDQUFDLFVBQVUsRUFBRTs7WUFDL0MsTUFBTSxHQUFHLEVBQUU7UUFDZixPQUFPLENBQUMsTUFBTSxJQUFJLFVBQVUsQ0FBQyxXQUFXLEVBQUUsZ0JBQWdCLENBQUMsQ0FBQyxDQUFDO0tBQzlEO0lBRUQsT0FBTyxnQkFBZ0IsQ0FBQyxDQUFDLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7QUFDbkMsQ0FBQzs7Ozs7QUFFRCxTQUFTLGNBQWMsQ0FBQyxNQUFvQztJQUMxRCxPQUFPLE1BQU0sQ0FBQyxNQUFNOzs7OztJQUFDLENBQUMsR0FBRyxFQUFFLEdBQUcsRUFBRSxFQUFFLENBQUMsQ0FBQyxHQUFHLEdBQUcsRUFBRSxHQUFHLEdBQUcsQ0FBQyxXQUFXLENBQUMsR0FBRSxFQUFFLENBQUMsQ0FBQztBQUN2RSxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcclxuICBDb21wb25lbnQsXHJcbiAgRXZlbnRFbWl0dGVyLFxyXG4gIElucHV0LFxyXG4gIE9uQ2hhbmdlcyxcclxuICBPbkluaXQsXHJcbiAgT3V0cHV0LFxyXG4gIFJlbmRlcmVyMixcclxuICBTaW1wbGVDaGFuZ2VzLFxyXG4gIFRyYWNrQnlGdW5jdGlvblxyXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBmcm9tLCBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IG1hcCwgcGx1Y2ssIHRha2UgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XHJcbmltcG9ydCB7IEdldFBlcm1pc3Npb25zLCBVcGRhdGVQZXJtaXNzaW9ucyB9IGZyb20gJy4uL2FjdGlvbnMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LmFjdGlvbnMnO1xyXG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudCB9IGZyb20gJy4uL21vZGVscy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQnO1xyXG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL3Blcm1pc3Npb24tbWFuYWdlbWVudC5zdGF0ZSc7XHJcblxyXG50eXBlIFBlcm1pc3Npb25XaXRoTWFyZ2luID0gUGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbiAmIHtcclxuICBtYXJnaW46IG51bWJlcjtcclxufTtcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLXBlcm1pc3Npb24tbWFuYWdlbWVudCcsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL3Blcm1pc3Npb24tbWFuYWdlbWVudC5jb21wb25lbnQuaHRtbCdcclxufSlcclxuZXhwb3J0IGNsYXNzIFBlcm1pc3Npb25NYW5hZ2VtZW50Q29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0LCBPbkNoYW5nZXMge1xyXG4gIEBJbnB1dCgpXHJcbiAgcHJvdmlkZXJOYW1lOiBzdHJpbmc7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgcHJvdmlkZXJLZXk6IHN0cmluZztcclxuXHJcbiAgcHJvdGVjdGVkIF92aXNpYmxlO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGdldCB2aXNpYmxlKCk6IGJvb2xlYW4ge1xyXG4gICAgcmV0dXJuIHRoaXMuX3Zpc2libGU7XHJcbiAgfVxyXG5cclxuICBzZXQgdmlzaWJsZSh2YWx1ZTogYm9vbGVhbikge1xyXG4gICAgaWYgKCF0aGlzLnNlbGVjdGVkR3JvdXApIHJldHVybjtcclxuXHJcbiAgICB0aGlzLl92aXNpYmxlID0gdmFsdWU7XHJcbiAgICB0aGlzLnZpc2libGVDaGFuZ2UuZW1pdCh2YWx1ZSk7XHJcblxyXG4gICAgaWYgKCF2YWx1ZSkge1xyXG4gICAgICB0aGlzLnNlbGVjdGVkR3JvdXAgPSBudWxsO1xyXG4gICAgfVxyXG4gIH1cclxuXHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IHZpc2libGVDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPGJvb2xlYW4+KCk7XHJcblxyXG4gIEBTZWxlY3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRQZXJtaXNzaW9uR3JvdXBzKVxyXG4gIGdyb3VwcyQ6IE9ic2VydmFibGU8UGVybWlzc2lvbk1hbmFnZW1lbnQuR3JvdXBbXT47XHJcblxyXG4gIEBTZWxlY3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRFbnRpdGl5RGlzcGxheU5hbWUpXHJcbiAgZW50aXR5TmFtZSQ6IE9ic2VydmFibGU8c3RyaW5nPjtcclxuXHJcbiAgc2VsZWN0ZWRHcm91cDogUGVybWlzc2lvbk1hbmFnZW1lbnQuR3JvdXA7XHJcblxyXG4gIHBlcm1pc3Npb25zOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uW10gPSBbXTtcclxuXHJcbiAgc2VsZWN0VGhpc1RhYiA9IGZhbHNlO1xyXG5cclxuICBzZWxlY3RBbGxUYWIgPSBmYWxzZTtcclxuXHJcbiAgbW9kYWxCdXN5ID0gZmFsc2U7XHJcblxyXG4gIHRyYWNrQnlGbjogVHJhY2tCeUZ1bmN0aW9uPFBlcm1pc3Npb25NYW5hZ2VtZW50Lkdyb3VwPiA9IChfLCBpdGVtKSA9PiBpdGVtLm5hbWU7XHJcblxyXG4gIGdldCBzZWxlY3RlZEdyb3VwUGVybWlzc2lvbnMkKCk6IE9ic2VydmFibGU8UGVybWlzc2lvbldpdGhNYXJnaW5bXT4ge1xyXG4gICAgcmV0dXJuIHRoaXMuZ3JvdXBzJC5waXBlKFxyXG4gICAgICBtYXAoZ3JvdXBzID0+XHJcbiAgICAgICAgdGhpcy5zZWxlY3RlZEdyb3VwID8gZ3JvdXBzLmZpbmQoZ3JvdXAgPT4gZ3JvdXAubmFtZSA9PT0gdGhpcy5zZWxlY3RlZEdyb3VwLm5hbWUpLnBlcm1pc3Npb25zIDogW11cclxuICAgICAgKSxcclxuICAgICAgbWFwPFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb25bXSwgUGVybWlzc2lvbldpdGhNYXJnaW5bXT4ocGVybWlzc2lvbnMgPT5cclxuICAgICAgICBwZXJtaXNzaW9ucy5tYXAoXHJcbiAgICAgICAgICBwZXJtaXNzaW9uID0+XHJcbiAgICAgICAgICAgICgoe1xyXG4gICAgICAgICAgICAgIC4uLnBlcm1pc3Npb24sXHJcbiAgICAgICAgICAgICAgbWFyZ2luOiBmaW5kTWFyZ2luKHBlcm1pc3Npb25zLCBwZXJtaXNzaW9uKSxcclxuICAgICAgICAgICAgICBpc0dyYW50ZWQ6IHRoaXMucGVybWlzc2lvbnMuZmluZChwZXIgPT4gcGVyLm5hbWUgPT09IHBlcm1pc3Npb24ubmFtZSkuaXNHcmFudGVkXHJcbiAgICAgICAgICAgIH0gYXMgYW55KSBhcyBQZXJtaXNzaW9uV2l0aE1hcmdpbilcclxuICAgICAgICApXHJcbiAgICAgIClcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSwgcHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyKSB7fVxyXG5cclxuICBuZ09uSW5pdCgpOiB2b2lkIHt9XHJcblxyXG4gIGdldENoZWNrZWQobmFtZTogc3RyaW5nKSB7XHJcbiAgICByZXR1cm4gKHRoaXMucGVybWlzc2lvbnMuZmluZChwZXIgPT4gcGVyLm5hbWUgPT09IG5hbWUpIHx8IHsgaXNHcmFudGVkOiBmYWxzZSB9KS5pc0dyYW50ZWQ7XHJcbiAgfVxyXG5cclxuICBpc0dyYW50ZWRCeVJvbGUoZ3JhbnRlZFByb3ZpZGVyczogUGVybWlzc2lvbk1hbmFnZW1lbnQuR3JhbnRlZFByb3ZpZGVyW10pOiBib29sZWFuIHtcclxuICAgIGlmIChncmFudGVkUHJvdmlkZXJzLmxlbmd0aCkge1xyXG4gICAgICByZXR1cm4gZ3JhbnRlZFByb3ZpZGVycy5maW5kSW5kZXgocCA9PiBwLnByb3ZpZGVyTmFtZSA9PT0gJ1JvbGUnKSA+IC0xO1xyXG4gICAgfVxyXG4gICAgcmV0dXJuIGZhbHNlO1xyXG4gIH1cclxuXHJcbiAgb25DbGlja0NoZWNrYm94KGNsaWNrZWRQZXJtaXNzaW9uOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uLCB2YWx1ZSkge1xyXG4gICAgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLmlzR3JhbnRlZCAmJiB0aGlzLmlzR3JhbnRlZEJ5Um9sZShjbGlja2VkUGVybWlzc2lvbi5ncmFudGVkUHJvdmlkZXJzKSkgcmV0dXJuO1xyXG5cclxuICAgIHNldFRpbWVvdXQoKCkgPT4ge1xyXG4gICAgICB0aGlzLnBlcm1pc3Npb25zID0gdGhpcy5wZXJtaXNzaW9ucy5tYXAocGVyID0+IHtcclxuICAgICAgICBpZiAoY2xpY2tlZFBlcm1pc3Npb24ubmFtZSA9PT0gcGVyLm5hbWUpIHtcclxuICAgICAgICAgIHJldHVybiB7IC4uLnBlciwgaXNHcmFudGVkOiAhcGVyLmlzR3JhbnRlZCB9O1xyXG4gICAgICAgIH0gZWxzZSBpZiAoY2xpY2tlZFBlcm1pc3Npb24ubmFtZSA9PT0gcGVyLnBhcmVudE5hbWUgJiYgY2xpY2tlZFBlcm1pc3Npb24uaXNHcmFudGVkKSB7XHJcbiAgICAgICAgICByZXR1cm4geyAuLi5wZXIsIGlzR3JhbnRlZDogZmFsc2UgfTtcclxuICAgICAgICB9IGVsc2UgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLnBhcmVudE5hbWUgPT09IHBlci5uYW1lICYmICFjbGlja2VkUGVybWlzc2lvbi5pc0dyYW50ZWQpIHtcclxuICAgICAgICAgIHJldHVybiB7IC4uLnBlciwgaXNHcmFudGVkOiB0cnVlIH07XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICByZXR1cm4gcGVyO1xyXG4gICAgICB9KTtcclxuXHJcbiAgICAgIHRoaXMuc2V0VGFiQ2hlY2tib3hTdGF0ZSgpO1xyXG4gICAgICB0aGlzLnNldEdyYW50Q2hlY2tib3hTdGF0ZSgpO1xyXG4gICAgfSwgMCk7XHJcbiAgfVxyXG5cclxuICBzZXRUYWJDaGVja2JveFN0YXRlKCkge1xyXG4gICAgdGhpcy5zZWxlY3RlZEdyb3VwUGVybWlzc2lvbnMkLnBpcGUodGFrZSgxKSkuc3Vic2NyaWJlKHBlcm1pc3Npb25zID0+IHtcclxuICAgICAgY29uc3Qgc2VsZWN0ZWRQZXJtaXNzaW9ucyA9IHBlcm1pc3Npb25zLmZpbHRlcihwZXIgPT4gcGVyLmlzR3JhbnRlZCk7XHJcbiAgICAgIGNvbnN0IGVsZW1lbnQgPSBkb2N1bWVudC5xdWVyeVNlbGVjdG9yKCcjc2VsZWN0LWFsbC1pbi10aGlzLXRhYnMnKSBhcyBhbnk7XHJcblxyXG4gICAgICBpZiAoc2VsZWN0ZWRQZXJtaXNzaW9ucy5sZW5ndGggPT09IHBlcm1pc3Npb25zLmxlbmd0aCkge1xyXG4gICAgICAgIGVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IGZhbHNlO1xyXG4gICAgICAgIHRoaXMuc2VsZWN0VGhpc1RhYiA9IHRydWU7XHJcbiAgICAgIH0gZWxzZSBpZiAoc2VsZWN0ZWRQZXJtaXNzaW9ucy5sZW5ndGggPT09IDApIHtcclxuICAgICAgICBlbGVtZW50LmluZGV0ZXJtaW5hdGUgPSBmYWxzZTtcclxuICAgICAgICB0aGlzLnNlbGVjdFRoaXNUYWIgPSBmYWxzZTtcclxuICAgICAgfSBlbHNlIHtcclxuICAgICAgICBlbGVtZW50LmluZGV0ZXJtaW5hdGUgPSB0cnVlO1xyXG4gICAgICB9XHJcbiAgICB9KTtcclxuICB9XHJcblxyXG4gIHNldEdyYW50Q2hlY2tib3hTdGF0ZSgpIHtcclxuICAgIGNvbnN0IHNlbGVjdGVkQWxsUGVybWlzc2lvbnMgPSB0aGlzLnBlcm1pc3Npb25zLmZpbHRlcihwZXIgPT4gcGVyLmlzR3JhbnRlZCk7XHJcbiAgICBjb25zdCBjaGVja2JveEVsZW1lbnQgPSBkb2N1bWVudC5xdWVyeVNlbGVjdG9yKCcjc2VsZWN0LWFsbC1pbi1hbGwtdGFicycpIGFzIGFueTtcclxuXHJcbiAgICBpZiAoc2VsZWN0ZWRBbGxQZXJtaXNzaW9ucy5sZW5ndGggPT09IHRoaXMucGVybWlzc2lvbnMubGVuZ3RoKSB7XHJcbiAgICAgIGNoZWNrYm94RWxlbWVudC5pbmRldGVybWluYXRlID0gZmFsc2U7XHJcbiAgICAgIHRoaXMuc2VsZWN0QWxsVGFiID0gdHJ1ZTtcclxuICAgIH0gZWxzZSBpZiAoc2VsZWN0ZWRBbGxQZXJtaXNzaW9ucy5sZW5ndGggPT09IDApIHtcclxuICAgICAgY2hlY2tib3hFbGVtZW50LmluZGV0ZXJtaW5hdGUgPSBmYWxzZTtcclxuICAgICAgdGhpcy5zZWxlY3RBbGxUYWIgPSBmYWxzZTtcclxuICAgIH0gZWxzZSB7XHJcbiAgICAgIGNoZWNrYm94RWxlbWVudC5pbmRldGVybWluYXRlID0gdHJ1ZTtcclxuICAgIH1cclxuICB9XHJcblxyXG4gIG9uQ2xpY2tTZWxlY3RUaGlzVGFiKCkge1xyXG4gICAgdGhpcy5zZWxlY3RlZEdyb3VwUGVybWlzc2lvbnMkLnBpcGUodGFrZSgxKSkuc3Vic2NyaWJlKHBlcm1pc3Npb25zID0+IHtcclxuICAgICAgcGVybWlzc2lvbnMuZm9yRWFjaChwZXJtaXNzaW9uID0+IHtcclxuICAgICAgICBpZiAocGVybWlzc2lvbi5pc0dyYW50ZWQgJiYgdGhpcy5pc0dyYW50ZWRCeVJvbGUocGVybWlzc2lvbi5ncmFudGVkUHJvdmlkZXJzKSkgcmV0dXJuO1xyXG5cclxuICAgICAgICBjb25zdCBpbmRleCA9IHRoaXMucGVybWlzc2lvbnMuZmluZEluZGV4KHBlciA9PiBwZXIubmFtZSA9PT0gcGVybWlzc2lvbi5uYW1lKTtcclxuXHJcbiAgICAgICAgdGhpcy5wZXJtaXNzaW9ucyA9IFtcclxuICAgICAgICAgIC4uLnRoaXMucGVybWlzc2lvbnMuc2xpY2UoMCwgaW5kZXgpLFxyXG4gICAgICAgICAgeyAuLi50aGlzLnBlcm1pc3Npb25zW2luZGV4XSwgaXNHcmFudGVkOiAhdGhpcy5zZWxlY3RUaGlzVGFiIH0sXHJcbiAgICAgICAgICAuLi50aGlzLnBlcm1pc3Npb25zLnNsaWNlKGluZGV4ICsgMSlcclxuICAgICAgICBdO1xyXG4gICAgICB9KTtcclxuICAgIH0pO1xyXG5cclxuICAgIHRoaXMuc2V0R3JhbnRDaGVja2JveFN0YXRlKCk7XHJcbiAgfVxyXG5cclxuICBvbkNsaWNrU2VsZWN0QWxsKCkge1xyXG4gICAgdGhpcy5wZXJtaXNzaW9ucyA9IHRoaXMucGVybWlzc2lvbnMubWFwKHBlcm1pc3Npb24gPT4gKHtcclxuICAgICAgLi4ucGVybWlzc2lvbixcclxuICAgICAgaXNHcmFudGVkOiAhdGhpcy5zZWxlY3RBbGxUYWJcclxuICAgIH0pKTtcclxuXHJcbiAgICB0aGlzLnNlbGVjdFRoaXNUYWIgPSAhdGhpcy5zZWxlY3RBbGxUYWI7XHJcbiAgfVxyXG5cclxuICBvbkNoYW5nZUdyb3VwKGdyb3VwOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5Hcm91cCkge1xyXG4gICAgdGhpcy5zZWxlY3RlZEdyb3VwID0gZ3JvdXA7XHJcbiAgICB0aGlzLnNldFRhYkNoZWNrYm94U3RhdGUoKTtcclxuICB9XHJcblxyXG4gIHN1Ym1pdCgpIHtcclxuICAgIHRoaXMubW9kYWxCdXN5ID0gdHJ1ZTtcclxuICAgIGNvbnN0IHVuY2hhbmdlZFBlcm1pc3Npb25zID0gZ2V0UGVybWlzc2lvbnMoXHJcbiAgICAgIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRQZXJtaXNzaW9uR3JvdXBzKVxyXG4gICAgKTtcclxuXHJcbiAgICBjb25zdCBjaGFuZ2VkUGVybWlzc2lvbnM6IFBlcm1pc3Npb25NYW5hZ2VtZW50Lk1pbmltdW1QZXJtaXNzaW9uW10gPSB0aGlzLnBlcm1pc3Npb25zXHJcbiAgICAgIC5maWx0ZXIocGVyID0+XHJcbiAgICAgICAgdW5jaGFuZ2VkUGVybWlzc2lvbnMuZmluZCh1bmNoYW5nZWQgPT4gdW5jaGFuZ2VkLm5hbWUgPT09IHBlci5uYW1lKS5pc0dyYW50ZWQgPT09IHBlci5pc0dyYW50ZWQgPyBmYWxzZSA6IHRydWVcclxuICAgICAgKVxyXG4gICAgICAubWFwKCh7IG5hbWUsIGlzR3JhbnRlZCB9KSA9PiAoeyBuYW1lLCBpc0dyYW50ZWQgfSkpO1xyXG5cclxuICAgIGlmIChjaGFuZ2VkUGVybWlzc2lvbnMubGVuZ3RoKSB7XHJcbiAgICAgIHRoaXMuc3RvcmVcclxuICAgICAgICAuZGlzcGF0Y2goXHJcbiAgICAgICAgICBuZXcgVXBkYXRlUGVybWlzc2lvbnMoe1xyXG4gICAgICAgICAgICBwcm92aWRlcktleTogdGhpcy5wcm92aWRlcktleSxcclxuICAgICAgICAgICAgcHJvdmlkZXJOYW1lOiB0aGlzLnByb3ZpZGVyTmFtZSxcclxuICAgICAgICAgICAgcGVybWlzc2lvbnM6IGNoYW5nZWRQZXJtaXNzaW9uc1xyXG4gICAgICAgICAgfSlcclxuICAgICAgICApXHJcbiAgICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XHJcbiAgICAgICAgICB0aGlzLm1vZGFsQnVzeSA9IGZhbHNlO1xyXG4gICAgICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XHJcbiAgICAgICAgfSk7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICB0aGlzLm1vZGFsQnVzeSA9IGZhbHNlO1xyXG4gICAgICB0aGlzLnZpc2libGUgPSBmYWxzZTtcclxuICAgIH1cclxuICB9XHJcblxyXG4gIG9wZW5Nb2RhbCgpIHtcclxuICAgIGlmICghdGhpcy5wcm92aWRlcktleSB8fCAhdGhpcy5wcm92aWRlck5hbWUpIHtcclxuICAgICAgdGhyb3cgbmV3IEVycm9yKCdQcm92aWRlciBLZXkgYW5kIFByb3ZpZGVyIE5hbWUgYXJlIHJlcXVpcmVkLicpO1xyXG4gICAgfVxyXG5cclxuICAgIHRoaXMuc3RvcmVcclxuICAgICAgLmRpc3BhdGNoKFxyXG4gICAgICAgIG5ldyBHZXRQZXJtaXNzaW9ucyh7XHJcbiAgICAgICAgICBwcm92aWRlcktleTogdGhpcy5wcm92aWRlcktleSxcclxuICAgICAgICAgIHByb3ZpZGVyTmFtZTogdGhpcy5wcm92aWRlck5hbWVcclxuICAgICAgICB9KVxyXG4gICAgICApXHJcbiAgICAgIC5waXBlKHBsdWNrKCdQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlJywgJ3Blcm1pc3Npb25SZXMnKSlcclxuICAgICAgLnN1YnNjcmliZSgocGVybWlzc2lvblJlczogUGVybWlzc2lvbk1hbmFnZW1lbnQuUmVzcG9uc2UpID0+IHtcclxuICAgICAgICB0aGlzLnNlbGVjdGVkR3JvdXAgPSBwZXJtaXNzaW9uUmVzLmdyb3Vwc1swXTtcclxuICAgICAgICB0aGlzLnBlcm1pc3Npb25zID0gZ2V0UGVybWlzc2lvbnMocGVybWlzc2lvblJlcy5ncm91cHMpO1xyXG5cclxuICAgICAgICB0aGlzLnZpc2libGUgPSB0cnVlO1xyXG4gICAgICB9KTtcclxuICB9XHJcblxyXG4gIGluaXRNb2RhbCgpIHtcclxuICAgIHRoaXMuc2V0VGFiQ2hlY2tib3hTdGF0ZSgpO1xyXG4gICAgdGhpcy5zZXRHcmFudENoZWNrYm94U3RhdGUoKTtcclxuICB9XHJcblxyXG4gIG5nT25DaGFuZ2VzKHsgdmlzaWJsZSB9OiBTaW1wbGVDaGFuZ2VzKTogdm9pZCB7XHJcbiAgICBpZiAoIXZpc2libGUpIHJldHVybjtcclxuXHJcbiAgICBpZiAodmlzaWJsZS5jdXJyZW50VmFsdWUpIHtcclxuICAgICAgdGhpcy5vcGVuTW9kYWwoKTtcclxuICAgIH0gZWxzZSBpZiAodmlzaWJsZS5jdXJyZW50VmFsdWUgPT09IGZhbHNlICYmIHRoaXMudmlzaWJsZSkge1xyXG4gICAgICB0aGlzLnZpc2libGUgPSBmYWxzZTtcclxuICAgIH1cclxuICB9XHJcbn1cclxuXHJcbmZ1bmN0aW9uIGZpbmRNYXJnaW4ocGVybWlzc2lvbnM6IFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb25bXSwgcGVybWlzc2lvbjogUGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbikge1xyXG4gIGNvbnN0IHBhcmVudFBlcm1pc3Npb24gPSBwZXJtaXNzaW9ucy5maW5kKHBlciA9PiBwZXIubmFtZSA9PT0gcGVybWlzc2lvbi5wYXJlbnROYW1lKTtcclxuXHJcbiAgaWYgKHBhcmVudFBlcm1pc3Npb24gJiYgcGFyZW50UGVybWlzc2lvbi5wYXJlbnROYW1lKSB7XHJcbiAgICBsZXQgbWFyZ2luID0gMjA7XHJcbiAgICByZXR1cm4gKG1hcmdpbiArPSBmaW5kTWFyZ2luKHBlcm1pc3Npb25zLCBwYXJlbnRQZXJtaXNzaW9uKSk7XHJcbiAgfVxyXG5cclxuICByZXR1cm4gcGFyZW50UGVybWlzc2lvbiA/IDIwIDogMDtcclxufVxyXG5cclxuZnVuY3Rpb24gZ2V0UGVybWlzc2lvbnMoZ3JvdXBzOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5Hcm91cFtdKTogUGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbltdIHtcclxuICByZXR1cm4gZ3JvdXBzLnJlZHVjZSgoYWNjLCB2YWwpID0+IFsuLi5hY2MsIC4uLnZhbC5wZXJtaXNzaW9uc10sIFtdKTtcclxufVxyXG4iXX0=