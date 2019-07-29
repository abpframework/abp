/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Component, EventEmitter, Input, Output, Renderer2, TemplateRef, ViewChild, } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Select, Store } from '@ngxs/store';
import { from, Observable } from 'rxjs';
import { map, pluck, take } from 'rxjs/operators';
import { PermissionManagementGetPermissions, PermissionManagementUpdatePermissions, } from '../actions/permission-management.actions';
import { PermissionManagementState } from '../states/permission-management.state';
export class PermissionManagementComponent {
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
    /** @type {?} */
    PermissionManagementComponent.prototype.visible;
    /** @type {?} */
    PermissionManagementComponent.prototype.visibleChange;
    /** @type {?} */
    PermissionManagementComponent.prototype.modalContent;
    /** @type {?} */
    PermissionManagementComponent.prototype.groups$;
    /** @type {?} */
    PermissionManagementComponent.prototype.entityName$;
    /** @type {?} */
    PermissionManagementComponent.prototype.modalRef;
    /** @type {?} */
    PermissionManagementComponent.prototype.selectedGroup;
    /** @type {?} */
    PermissionManagementComponent.prototype.permissions;
    /** @type {?} */
    PermissionManagementComponent.prototype.selectThisTab;
    /** @type {?} */
    PermissionManagementComponent.prototype.selectAllTab;
    /** @type {?} */
    PermissionManagementComponent.prototype.trackByFn;
    /**
     * @type {?}
     * @private
     */
    PermissionManagementComponent.prototype.modalService;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcucGVybWlzc2lvbi1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFDTCxTQUFTLEVBQ1QsWUFBWSxFQUNaLEtBQUssRUFHTCxNQUFNLEVBQ04sU0FBUyxFQUVULFdBQVcsRUFFWCxTQUFTLEdBQ1YsTUFBTSxlQUFlLENBQUM7QUFDdkIsT0FBTyxFQUFFLFFBQVEsRUFBZSxNQUFNLDRCQUE0QixDQUFDO0FBQ25FLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxJQUFJLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ3hDLE9BQU8sRUFBRSxHQUFHLEVBQUUsS0FBSyxFQUFFLElBQUksRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ2xELE9BQU8sRUFDTCxrQ0FBa0MsRUFDbEMscUNBQXFDLEdBQ3RDLE1BQU0sMENBQTBDLENBQUM7QUFFbEQsT0FBTyxFQUFFLHlCQUF5QixFQUFFLE1BQU0sdUNBQXVDLENBQUM7QUFRbEYsTUFBTSxPQUFPLDZCQUE2Qjs7Ozs7O0lBb0R4QyxZQUFvQixZQUFzQixFQUFVLEtBQVksRUFBVSxRQUFtQjtRQUF6RSxpQkFBWSxHQUFaLFlBQVksQ0FBVTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87UUFBVSxhQUFRLEdBQVIsUUFBUSxDQUFXO1FBekM3RixrQkFBYSxHQUFHLElBQUksWUFBWSxFQUFXLENBQUM7UUFlNUMsZ0JBQVcsR0FBc0MsRUFBRSxDQUFDO1FBRXBELGtCQUFhLEdBQVksS0FBSyxDQUFDO1FBRS9CLGlCQUFZLEdBQVksS0FBSyxDQUFDO1FBRTlCLGNBQVM7Ozs7O1FBQWdELENB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Z0JBQWdCLEdBQUcsV0FBVyxDQUFDLElBQUk7Ozs7SUFBQyxHQUFHLENBQUMsRUFBRSxDQUFDLEdBQUcsQ0FBQyxJQUFJLEtBQUssVUFBVSxDQUFDLFVBQVUsRUFBQztJQUVwRixJQUFJLGdCQUFnQixJQUFJLGdCQUFnQixDQUFDLFVBQVUsRUFBRTs7WUFDL0MsTUFBTSxHQUFHLEVBQUU7UUFDZixPQUFPLENBQUMsTUFBTSxJQUFJLFVBQVUsQ0FBQyxXQUFXLEVBQUUsZ0JBQWdCLENBQUMsQ0FBQyxDQUFDO0tBQzlEO0lBRUQsT0FBTyxnQkFBZ0IsQ0FBQyxDQUFDLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7QUFDbkMsQ0FBQzs7Ozs7QUFFRCxTQUFTLGNBQWMsQ0FBQyxNQUFvQztJQUMxRCxPQUFPLE1BQU0sQ0FBQyxNQUFNOzs7OztJQUFDLENBQUMsR0FBRyxFQUFFLEdBQUcsRUFBRSxFQUFFLENBQUMsQ0FBQyxHQUFHLEdBQUcsRUFBRSxHQUFHLEdBQUcsQ0FBQyxXQUFXLENBQUMsR0FBRSxFQUFFLENBQUMsQ0FBQztBQUN2RSxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcbiAgQ29tcG9uZW50LFxuICBFdmVudEVtaXR0ZXIsXG4gIElucHV0LFxuICBPbkNoYW5nZXMsXG4gIE9uSW5pdCxcbiAgT3V0cHV0LFxuICBSZW5kZXJlcjIsXG4gIFNpbXBsZUNoYW5nZXMsXG4gIFRlbXBsYXRlUmVmLFxuICBUcmFja0J5RnVuY3Rpb24sXG4gIFZpZXdDaGlsZCxcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBOZ2JNb2RhbCwgTmdiTW9kYWxSZWYgfSBmcm9tICdAbmctYm9vdHN0cmFwL25nLWJvb3RzdHJhcCc7XG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgZnJvbSwgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgbWFwLCBwbHVjaywgdGFrZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7XG4gIFBlcm1pc3Npb25NYW5hZ2VtZW50R2V0UGVybWlzc2lvbnMsXG4gIFBlcm1pc3Npb25NYW5hZ2VtZW50VXBkYXRlUGVybWlzc2lvbnMsXG59IGZyb20gJy4uL2FjdGlvbnMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LmFjdGlvbnMnO1xuaW1wb3J0IHsgUGVybWlzc2lvbk1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvcGVybWlzc2lvbi1tYW5hZ2VtZW50JztcbmltcG9ydCB7IFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LnN0YXRlJztcblxudHlwZSBQZXJtaXNzaW9uV2l0aE1hcmdpbiA9IFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb24gJiB7IG1hcmdpbjogbnVtYmVyIH07XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1wZXJtaXNzaW9uLW1hbmFnZW1lbnQnLFxuICB0ZW1wbGF0ZVVybDogJy4vcGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgUGVybWlzc2lvbk1hbmFnZW1lbnRDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQsIE9uQ2hhbmdlcyB7XG4gIEBJbnB1dCgpXG4gIHByb3ZpZGVyTmFtZTogc3RyaW5nO1xuXG4gIEBJbnB1dCgpXG4gIHByb3ZpZGVyS2V5OiBzdHJpbmc7XG5cbiAgQElucHV0KClcbiAgdmlzaWJsZTogYm9vbGVhbjtcblxuICBAT3V0cHV0KClcbiAgdmlzaWJsZUNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8Ym9vbGVhbj4oKTtcblxuICBAVmlld0NoaWxkKCdtb2RhbENvbnRlbnQnLCB7IHN0YXRpYzogZmFsc2UgfSlcbiAgbW9kYWxDb250ZW50OiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBTZWxlY3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRQZXJtaXNzaW9uR3JvdXBzKVxuICBncm91cHMkOiBPYnNlcnZhYmxlPFBlcm1pc3Npb25NYW5hZ2VtZW50Lkdyb3VwW10+O1xuXG4gIEBTZWxlY3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRFbnRpdGl5RGlzcGxheU5hbWUpXG4gIGVudGl0eU5hbWUkOiBPYnNlcnZhYmxlPHN0cmluZz47XG5cbiAgbW9kYWxSZWY6IE5nYk1vZGFsUmVmO1xuXG4gIHNlbGVjdGVkR3JvdXA6IFBlcm1pc3Npb25NYW5hZ2VtZW50Lkdyb3VwO1xuXG4gIHBlcm1pc3Npb25zOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uW10gPSBbXTtcblxuICBzZWxlY3RUaGlzVGFiOiBib29sZWFuID0gZmFsc2U7XG5cbiAgc2VsZWN0QWxsVGFiOiBib29sZWFuID0gZmFsc2U7XG5cbiAgdHJhY2tCeUZuOiBUcmFja0J5RnVuY3Rpb248UGVybWlzc2lvbk1hbmFnZW1lbnQuR3JvdXA+ID0gKF8sIGl0ZW0pID0+IGl0ZW0ubmFtZTtcblxuICBnZXQgc2VsZWN0ZWRHcm91cFBlcm1pc3Npb25zJCgpOiBPYnNlcnZhYmxlPFBlcm1pc3Npb25XaXRoTWFyZ2luW10+IHtcbiAgICByZXR1cm4gdGhpcy5ncm91cHMkLnBpcGUoXG4gICAgICBtYXAoZ3JvdXBzID0+XG4gICAgICAgIHRoaXMuc2VsZWN0ZWRHcm91cCA/IGdyb3Vwcy5maW5kKGdyb3VwID0+IGdyb3VwLm5hbWUgPT09IHRoaXMuc2VsZWN0ZWRHcm91cC5uYW1lKS5wZXJtaXNzaW9ucyA6IFtdLFxuICAgICAgKSxcbiAgICAgIG1hcDxQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uW10sIFBlcm1pc3Npb25XaXRoTWFyZ2luW10+KHBlcm1pc3Npb25zID0+XG4gICAgICAgIHBlcm1pc3Npb25zLm1hcChcbiAgICAgICAgICBwZXJtaXNzaW9uID0+XG4gICAgICAgICAgICAoKHtcbiAgICAgICAgICAgICAgLi4ucGVybWlzc2lvbixcbiAgICAgICAgICAgICAgbWFyZ2luOiBmaW5kTWFyZ2luKHBlcm1pc3Npb25zLCBwZXJtaXNzaW9uKSxcbiAgICAgICAgICAgICAgaXNHcmFudGVkOiB0aGlzLnBlcm1pc3Npb25zLmZpbmQocGVyID0+IHBlci5uYW1lID09PSBwZXJtaXNzaW9uLm5hbWUpLmlzR3JhbnRlZCxcbiAgICAgICAgICAgIH0gYXMgYW55KSBhcyBQZXJtaXNzaW9uV2l0aE1hcmdpbiksXG4gICAgICAgICksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIG1vZGFsU2VydmljZTogTmdiTW9kYWwsIHByaXZhdGUgc3RvcmU6IFN0b3JlLCBwcml2YXRlIHJlbmRlcmVyOiBSZW5kZXJlcjIpIHt9XG5cbiAgbmdPbkluaXQoKTogdm9pZCB7fVxuXG4gIGdldENoZWNrZWQobmFtZTogc3RyaW5nKSB7XG4gICAgcmV0dXJuICh0aGlzLnBlcm1pc3Npb25zLmZpbmQocGVyID0+IHBlci5uYW1lID09PSBuYW1lKSB8fCB7IGlzR3JhbnRlZDogZmFsc2UgfSkuaXNHcmFudGVkO1xuICB9XG5cbiAgb25DbGlja0NoZWNrYm94KGNsaWNrZWRQZXJtaXNzaW9uOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uLCB2YWx1ZSkge1xuICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgdGhpcy5wZXJtaXNzaW9ucyA9IHRoaXMucGVybWlzc2lvbnMubWFwKHBlciA9PiB7XG4gICAgICAgIGlmIChjbGlja2VkUGVybWlzc2lvbi5uYW1lID09PSBwZXIubmFtZSkge1xuICAgICAgICAgIHJldHVybiB7IC4uLnBlciwgaXNHcmFudGVkOiAhcGVyLmlzR3JhbnRlZCB9O1xuICAgICAgICB9IGVsc2UgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLm5hbWUgPT09IHBlci5wYXJlbnROYW1lICYmIGNsaWNrZWRQZXJtaXNzaW9uLmlzR3JhbnRlZCkge1xuICAgICAgICAgIHJldHVybiB7IC4uLnBlciwgaXNHcmFudGVkOiBmYWxzZSB9O1xuICAgICAgICB9IGVsc2UgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLnBhcmVudE5hbWUgPT09IHBlci5uYW1lICYmICFjbGlja2VkUGVybWlzc2lvbi5pc0dyYW50ZWQpIHtcbiAgICAgICAgICByZXR1cm4geyAuLi5wZXIsIGlzR3JhbnRlZDogdHJ1ZSB9O1xuICAgICAgICB9XG5cbiAgICAgICAgcmV0dXJuIHBlcjtcbiAgICAgIH0pO1xuXG4gICAgICB0aGlzLnNldFRhYkNoZWNrYm94U3RhdGUoKTtcbiAgICAgIHRoaXMuc2V0R3JhbnRDaGVja2JveFN0YXRlKCk7XG4gICAgfSwgMCk7XG4gIH1cblxuICBzZXRUYWJDaGVja2JveFN0YXRlKCkge1xuICAgIHRoaXMuc2VsZWN0ZWRHcm91cFBlcm1pc3Npb25zJC5waXBlKHRha2UoMSkpLnN1YnNjcmliZShwZXJtaXNzaW9ucyA9PiB7XG4gICAgICBjb25zdCBzZWxlY3RlZFBlcm1pc3Npb25zID0gcGVybWlzc2lvbnMuZmlsdGVyKHBlciA9PiBwZXIuaXNHcmFudGVkKTtcbiAgICAgIGNvbnN0IGVsZW1lbnQgPSBkb2N1bWVudC5xdWVyeVNlbGVjdG9yKCcjc2VsZWN0LWFsbC1pbi10aGlzLXRhYnMnKSBhcyBhbnk7XG5cbiAgICAgIGlmIChzZWxlY3RlZFBlcm1pc3Npb25zLmxlbmd0aCA9PT0gcGVybWlzc2lvbnMubGVuZ3RoKSB7XG4gICAgICAgIGVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IGZhbHNlO1xuICAgICAgICB0aGlzLnNlbGVjdFRoaXNUYWIgPSB0cnVlO1xuICAgICAgfSBlbHNlIGlmIChzZWxlY3RlZFBlcm1pc3Npb25zLmxlbmd0aCA9PT0gMCkge1xuICAgICAgICBlbGVtZW50LmluZGV0ZXJtaW5hdGUgPSBmYWxzZTtcbiAgICAgICAgdGhpcy5zZWxlY3RUaGlzVGFiID0gZmFsc2U7XG4gICAgICB9IGVsc2Uge1xuICAgICAgICBlbGVtZW50LmluZGV0ZXJtaW5hdGUgPSB0cnVlO1xuICAgICAgfVxuICAgIH0pO1xuICB9XG5cbiAgc2V0R3JhbnRDaGVja2JveFN0YXRlKCkge1xuICAgIGNvbnN0IHNlbGVjdGVkQWxsUGVybWlzc2lvbnMgPSB0aGlzLnBlcm1pc3Npb25zLmZpbHRlcihwZXIgPT4gcGVyLmlzR3JhbnRlZCk7XG4gICAgY29uc3QgY2hlY2tib3hFbGVtZW50ID0gZG9jdW1lbnQucXVlcnlTZWxlY3RvcignI3NlbGVjdC1hbGwtaW4tYWxsLXRhYnMnKSBhcyBhbnk7XG5cbiAgICBpZiAoc2VsZWN0ZWRBbGxQZXJtaXNzaW9ucy5sZW5ndGggPT09IHRoaXMucGVybWlzc2lvbnMubGVuZ3RoKSB7XG4gICAgICBjaGVja2JveEVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IGZhbHNlO1xuICAgICAgdGhpcy5zZWxlY3RBbGxUYWIgPSB0cnVlO1xuICAgIH0gZWxzZSBpZiAoc2VsZWN0ZWRBbGxQZXJtaXNzaW9ucy5sZW5ndGggPT09IDApIHtcbiAgICAgIGNoZWNrYm94RWxlbWVudC5pbmRldGVybWluYXRlID0gZmFsc2U7XG4gICAgICB0aGlzLnNlbGVjdEFsbFRhYiA9IGZhbHNlO1xuICAgIH0gZWxzZSB7XG4gICAgICBjaGVja2JveEVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IHRydWU7XG4gICAgfVxuICB9XG5cbiAgb25DbGlja1NlbGVjdFRoaXNUYWIoKSB7XG4gICAgdGhpcy5zZWxlY3RlZEdyb3VwUGVybWlzc2lvbnMkLnBpcGUodGFrZSgxKSkuc3Vic2NyaWJlKHBlcm1pc3Npb25zID0+IHtcbiAgICAgIHBlcm1pc3Npb25zLmZvckVhY2gocGVybWlzc2lvbiA9PiB7XG4gICAgICAgIGNvbnN0IGluZGV4ID0gdGhpcy5wZXJtaXNzaW9ucy5maW5kSW5kZXgocGVyID0+IHBlci5uYW1lID09PSBwZXJtaXNzaW9uLm5hbWUpO1xuXG4gICAgICAgIHRoaXMucGVybWlzc2lvbnMgPSBbXG4gICAgICAgICAgLi4udGhpcy5wZXJtaXNzaW9ucy5zbGljZSgwLCBpbmRleCksXG4gICAgICAgICAgeyAuLi50aGlzLnBlcm1pc3Npb25zW2luZGV4XSwgaXNHcmFudGVkOiAhdGhpcy5zZWxlY3RUaGlzVGFiIH0sXG4gICAgICAgICAgLi4udGhpcy5wZXJtaXNzaW9ucy5zbGljZShpbmRleCArIDEpLFxuICAgICAgICBdO1xuICAgICAgfSk7XG4gICAgfSk7XG5cbiAgICB0aGlzLnNldEdyYW50Q2hlY2tib3hTdGF0ZSgpO1xuICB9XG5cbiAgb25DbGlja1NlbGVjdEFsbCgpIHtcbiAgICB0aGlzLnBlcm1pc3Npb25zID0gdGhpcy5wZXJtaXNzaW9ucy5tYXAocGVybWlzc2lvbiA9PiAoeyAuLi5wZXJtaXNzaW9uLCBpc0dyYW50ZWQ6ICF0aGlzLnNlbGVjdEFsbFRhYiB9KSk7XG5cbiAgICB0aGlzLnNlbGVjdFRoaXNUYWIgPSAhdGhpcy5zZWxlY3RBbGxUYWI7XG4gIH1cblxuICBvbkNoYW5nZUdyb3VwKGdyb3VwOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5Hcm91cCkge1xuICAgIHRoaXMuc2VsZWN0ZWRHcm91cCA9IGdyb3VwO1xuICAgIHRoaXMuc2V0VGFiQ2hlY2tib3hTdGF0ZSgpO1xuICB9XG5cbiAgb25TdWJtaXQoKSB7XG4gICAgY29uc3QgdW5jaGFuZ2VkUGVybWlzc2lvbnMgPSBnZXRQZXJtaXNzaW9ucyhcbiAgICAgIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRQZXJtaXNzaW9uR3JvdXBzKSxcbiAgICApO1xuXG4gICAgY29uc3QgY2hhbmdlZFBlcm1pc3Npb25zOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5NaW5pbXVtUGVybWlzc2lvbltdID0gdGhpcy5wZXJtaXNzaW9uc1xuICAgICAgLmZpbHRlcihwZXIgPT5cbiAgICAgICAgdW5jaGFuZ2VkUGVybWlzc2lvbnMuZmluZCh1bmNoYW5nZWQgPT4gdW5jaGFuZ2VkLm5hbWUgPT09IHBlci5uYW1lKS5pc0dyYW50ZWQgPT09IHBlci5pc0dyYW50ZWQgPyBmYWxzZSA6IHRydWUsXG4gICAgICApXG4gICAgICAubWFwKCh7IG5hbWUsIGlzR3JhbnRlZCB9KSA9PiAoeyBuYW1lLCBpc0dyYW50ZWQgfSkpO1xuXG4gICAgaWYgKGNoYW5nZWRQZXJtaXNzaW9ucy5sZW5ndGgpIHtcbiAgICAgIHRoaXMuc3RvcmVcbiAgICAgICAgLmRpc3BhdGNoKFxuICAgICAgICAgIG5ldyBQZXJtaXNzaW9uTWFuYWdlbWVudFVwZGF0ZVBlcm1pc3Npb25zKHtcbiAgICAgICAgICAgIHByb3ZpZGVyS2V5OiB0aGlzLnByb3ZpZGVyS2V5LFxuICAgICAgICAgICAgcHJvdmlkZXJOYW1lOiB0aGlzLnByb3ZpZGVyTmFtZSxcbiAgICAgICAgICAgIHBlcm1pc3Npb25zOiBjaGFuZ2VkUGVybWlzc2lvbnMsXG4gICAgICAgICAgfSksXG4gICAgICAgIClcbiAgICAgICAgLnN1YnNjcmliZSgoKSA9PiB0aGlzLm1vZGFsUmVmLmNsb3NlKCkpO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aGlzLm1vZGFsUmVmLmNsb3NlKCk7XG4gICAgfVxuICB9XG5cbiAgb3Blbk1vZGFsKCkge1xuICAgIGlmICghdGhpcy5wcm92aWRlcktleSB8fCAhdGhpcy5wcm92aWRlck5hbWUpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcignUHJvdmlkZXIgS2V5IGFuZCBQcm92aWRlciBOYW1lIGFyZSByZXF1aXJlZC4nKTtcbiAgICB9XG5cbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2goXG4gICAgICAgIG5ldyBQZXJtaXNzaW9uTWFuYWdlbWVudEdldFBlcm1pc3Npb25zKHsgcHJvdmlkZXJLZXk6IHRoaXMucHJvdmlkZXJLZXksIHByb3ZpZGVyTmFtZTogdGhpcy5wcm92aWRlck5hbWUgfSksXG4gICAgICApXG4gICAgICAucGlwZShwbHVjaygnUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZScsICdwZXJtaXNzaW9uUmVzJykpXG4gICAgICAuc3Vic2NyaWJlKChwZXJtaXNzaW9uUmVzOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5SZXNwb25zZSkgPT4ge1xuICAgICAgICB0aGlzLnNlbGVjdGVkR3JvdXAgPSBwZXJtaXNzaW9uUmVzLmdyb3Vwc1swXTtcbiAgICAgICAgdGhpcy5wZXJtaXNzaW9ucyA9IGdldFBlcm1pc3Npb25zKHBlcm1pc3Npb25SZXMuZ3JvdXBzKTtcblxuICAgICAgICB0aGlzLm1vZGFsUmVmID0gdGhpcy5tb2RhbFNlcnZpY2Uub3Blbih0aGlzLm1vZGFsQ29udGVudCwgeyBzaXplOiAnbGcnIH0pO1xuICAgICAgICB0aGlzLnZpc2libGVDaGFuZ2UuZW1pdCh0cnVlKTtcblxuICAgICAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgICAgICB0aGlzLnNldFRhYkNoZWNrYm94U3RhdGUoKTtcbiAgICAgICAgICB0aGlzLnNldEdyYW50Q2hlY2tib3hTdGF0ZSgpO1xuICAgICAgICB9LCAwKTtcblxuICAgICAgICBmcm9tKHRoaXMubW9kYWxSZWYucmVzdWx0KVxuICAgICAgICAgIC5waXBlKHRha2UoMSkpXG4gICAgICAgICAgLnN1YnNjcmliZShcbiAgICAgICAgICAgIGRhdGEgPT4ge1xuICAgICAgICAgICAgICB0aGlzLnNldFZpc2libGUoZmFsc2UpO1xuICAgICAgICAgICAgfSxcbiAgICAgICAgICAgIHJlYXNvbiA9PiB7XG4gICAgICAgICAgICAgIHRoaXMuc2V0VmlzaWJsZShmYWxzZSk7XG4gICAgICAgICAgICB9LFxuICAgICAgICAgICk7XG4gICAgICB9KTtcbiAgfVxuXG4gIHNldFZpc2libGUodmFsdWU6IGJvb2xlYW4pIHtcbiAgICB0aGlzLnZpc2libGUgPSB2YWx1ZTtcbiAgICB0aGlzLnZpc2libGVDaGFuZ2UuZW1pdCh2YWx1ZSk7XG4gIH1cblxuICBuZ09uQ2hhbmdlcyh7IHZpc2libGUgfTogU2ltcGxlQ2hhbmdlcyk6IHZvaWQge1xuICAgIGlmICghdmlzaWJsZSkgcmV0dXJuO1xuXG4gICAgaWYgKHZpc2libGUuY3VycmVudFZhbHVlKSB7XG4gICAgICB0aGlzLm9wZW5Nb2RhbCgpO1xuICAgIH0gZWxzZSBpZiAodmlzaWJsZS5jdXJyZW50VmFsdWUgPT09IGZhbHNlICYmIHRoaXMubW9kYWxTZXJ2aWNlLmhhc09wZW5Nb2RhbHMoKSkge1xuICAgICAgdGhpcy5tb2RhbFJlZi5jbG9zZSgpO1xuICAgIH1cbiAgfVxufVxuXG5mdW5jdGlvbiBmaW5kTWFyZ2luKHBlcm1pc3Npb25zOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uW10sIHBlcm1pc3Npb246IFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb24pIHtcbiAgY29uc3QgcGFyZW50UGVybWlzc2lvbiA9IHBlcm1pc3Npb25zLmZpbmQocGVyID0+IHBlci5uYW1lID09PSBwZXJtaXNzaW9uLnBhcmVudE5hbWUpO1xuXG4gIGlmIChwYXJlbnRQZXJtaXNzaW9uICYmIHBhcmVudFBlcm1pc3Npb24ucGFyZW50TmFtZSkge1xuICAgIGxldCBtYXJnaW4gPSAyMDtcbiAgICByZXR1cm4gKG1hcmdpbiArPSBmaW5kTWFyZ2luKHBlcm1pc3Npb25zLCBwYXJlbnRQZXJtaXNzaW9uKSk7XG4gIH1cblxuICByZXR1cm4gcGFyZW50UGVybWlzc2lvbiA/IDIwIDogMDtcbn1cblxuZnVuY3Rpb24gZ2V0UGVybWlzc2lvbnMoZ3JvdXBzOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5Hcm91cFtdKTogUGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbltdIHtcbiAgcmV0dXJuIGdyb3Vwcy5yZWR1Y2UoKGFjYywgdmFsKSA9PiBbLi4uYWNjLCAuLi52YWwucGVybWlzc2lvbnNdLCBbXSk7XG59XG4iXX0=