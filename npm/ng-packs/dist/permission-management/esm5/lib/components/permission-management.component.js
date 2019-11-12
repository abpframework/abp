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
var PermissionManagementComponent = /** @class */ (function () {
    function PermissionManagementComponent(store, renderer) {
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
        function (_, item) { return item.name; });
    }
    Object.defineProperty(PermissionManagementComponent.prototype, "visible", {
        get: /**
         * @return {?}
         */
        function () {
            return this._visible;
        },
        set: /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            if (!this.selectedGroup)
                return;
            this._visible = value;
            this.visibleChange.emit(value);
            if (!value) {
                this.selectedGroup = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PermissionManagementComponent.prototype, "selectedGroupPermissions$", {
        get: /**
         * @return {?}
         */
        function () {
            var _this = this;
            return this.groups$.pipe(map((/**
             * @param {?} groups
             * @return {?}
             */
            function (groups) {
                return _this.selectedGroup ? groups.find((/**
                 * @param {?} group
                 * @return {?}
                 */
                function (group) { return group.name === _this.selectedGroup.name; })).permissions : [];
            })), map((/**
             * @param {?} permissions
             * @return {?}
             */
            function (permissions) {
                return permissions.map((/**
                 * @param {?} permission
                 * @return {?}
                 */
                function (permission) {
                    return ((/** @type {?} */ (((/** @type {?} */ (tslib_1.__assign({}, permission, { margin: findMargin(permissions, permission), isGranted: _this.permissions.find((/**
                         * @param {?} per
                         * @return {?}
                         */
                        function (per) { return per.name === permission.name; })).isGranted })))))));
                }));
            })));
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @return {?}
     */
    PermissionManagementComponent.prototype.ngOnInit = /**
     * @return {?}
     */
    function () { };
    /**
     * @param {?} name
     * @return {?}
     */
    PermissionManagementComponent.prototype.getChecked = /**
     * @param {?} name
     * @return {?}
     */
    function (name) {
        return (this.permissions.find((/**
         * @param {?} per
         * @return {?}
         */
        function (per) { return per.name === name; })) || { isGranted: false }).isGranted;
    };
    /**
     * @param {?} grantedProviders
     * @return {?}
     */
    PermissionManagementComponent.prototype.isGrantedByOtherProviderName = /**
     * @param {?} grantedProviders
     * @return {?}
     */
    function (grantedProviders) {
        var _this = this;
        if (grantedProviders.length) {
            return grantedProviders.findIndex((/**
             * @param {?} p
             * @return {?}
             */
            function (p) { return p.providerName !== _this.providerName; })) > -1;
        }
        return false;
    };
    /**
     * @param {?} clickedPermission
     * @param {?} value
     * @return {?}
     */
    PermissionManagementComponent.prototype.onClickCheckbox = /**
     * @param {?} clickedPermission
     * @param {?} value
     * @return {?}
     */
    function (clickedPermission, value) {
        var _this = this;
        if (clickedPermission.isGranted && this.isGrantedByOtherProviderName(clickedPermission.grantedProviders))
            return;
        setTimeout((/**
         * @return {?}
         */
        function () {
            _this.permissions = _this.permissions.map((/**
             * @param {?} per
             * @return {?}
             */
            function (per) {
                if (clickedPermission.name === per.name) {
                    return tslib_1.__assign({}, per, { isGranted: !per.isGranted });
                }
                else if (clickedPermission.name === per.parentName && clickedPermission.isGranted) {
                    return tslib_1.__assign({}, per, { isGranted: false });
                }
                else if (clickedPermission.parentName === per.name && !clickedPermission.isGranted) {
                    return tslib_1.__assign({}, per, { isGranted: true });
                }
                return per;
            }));
            _this.setTabCheckboxState();
            _this.setGrantCheckboxState();
        }), 0);
    };
    /**
     * @return {?}
     */
    PermissionManagementComponent.prototype.setTabCheckboxState = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.selectedGroupPermissions$.pipe(take(1)).subscribe((/**
         * @param {?} permissions
         * @return {?}
         */
        function (permissions) {
            /** @type {?} */
            var selectedPermissions = permissions.filter((/**
             * @param {?} per
             * @return {?}
             */
            function (per) { return per.isGranted; }));
            /** @type {?} */
            var element = (/** @type {?} */ (document.querySelector('#select-all-in-this-tabs')));
            if (selectedPermissions.length === permissions.length) {
                element.indeterminate = false;
                _this.selectThisTab = true;
            }
            else if (selectedPermissions.length === 0) {
                element.indeterminate = false;
                _this.selectThisTab = false;
            }
            else {
                element.indeterminate = true;
            }
        }));
    };
    /**
     * @return {?}
     */
    PermissionManagementComponent.prototype.setGrantCheckboxState = /**
     * @return {?}
     */
    function () {
        /** @type {?} */
        var selectedAllPermissions = this.permissions.filter((/**
         * @param {?} per
         * @return {?}
         */
        function (per) { return per.isGranted; }));
        /** @type {?} */
        var checkboxElement = (/** @type {?} */ (document.querySelector('#select-all-in-all-tabs')));
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
    };
    /**
     * @return {?}
     */
    PermissionManagementComponent.prototype.onClickSelectThisTab = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.selectedGroupPermissions$.pipe(take(1)).subscribe((/**
         * @param {?} permissions
         * @return {?}
         */
        function (permissions) {
            permissions.forEach((/**
             * @param {?} permission
             * @return {?}
             */
            function (permission) {
                if (permission.isGranted && _this.isGrantedByOtherProviderName(permission.grantedProviders))
                    return;
                /** @type {?} */
                var index = _this.permissions.findIndex((/**
                 * @param {?} per
                 * @return {?}
                 */
                function (per) { return per.name === permission.name; }));
                _this.permissions = tslib_1.__spread(_this.permissions.slice(0, index), [
                    tslib_1.__assign({}, _this.permissions[index], { isGranted: !_this.selectThisTab })
                ], _this.permissions.slice(index + 1));
            }));
        }));
        this.setGrantCheckboxState();
    };
    /**
     * @return {?}
     */
    PermissionManagementComponent.prototype.onClickSelectAll = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.permissions = this.permissions.map((/**
         * @param {?} permission
         * @return {?}
         */
        function (permission) { return (tslib_1.__assign({}, permission, { isGranted: _this.isGrantedByOtherProviderName(permission.grantedProviders) || !_this.selectAllTab })); }));
        this.selectThisTab = !this.selectAllTab;
    };
    /**
     * @param {?} group
     * @return {?}
     */
    PermissionManagementComponent.prototype.onChangeGroup = /**
     * @param {?} group
     * @return {?}
     */
    function (group) {
        this.selectedGroup = group;
        this.setTabCheckboxState();
    };
    /**
     * @return {?}
     */
    PermissionManagementComponent.prototype.submit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.modalBusy = true;
        /** @type {?} */
        var unchangedPermissions = getPermissions(this.store.selectSnapshot(PermissionManagementState.getPermissionGroups));
        /** @type {?} */
        var changedPermissions = this.permissions
            .filter((/**
         * @param {?} per
         * @return {?}
         */
        function (per) {
            return unchangedPermissions.find((/**
             * @param {?} unchanged
             * @return {?}
             */
            function (unchanged) { return unchanged.name === per.name; })).isGranted === per.isGranted ? false : true;
        }))
            .map((/**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var name = _a.name, isGranted = _a.isGranted;
            return ({ name: name, isGranted: isGranted });
        }));
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
            function () { return (_this.modalBusy = false); })))
                .subscribe((/**
             * @return {?}
             */
            function () {
                _this.visible = false;
            }));
        }
        else {
            this.modalBusy = false;
            this.visible = false;
        }
    };
    /**
     * @return {?}
     */
    PermissionManagementComponent.prototype.openModal = /**
     * @return {?}
     */
    function () {
        var _this = this;
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
        function (permissionRes) {
            _this.selectedGroup = permissionRes.groups[0];
            _this.permissions = getPermissions(permissionRes.groups);
            _this.visible = true;
        }));
    };
    /**
     * @return {?}
     */
    PermissionManagementComponent.prototype.initModal = /**
     * @return {?}
     */
    function () {
        this.setTabCheckboxState();
        this.setGrantCheckboxState();
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    PermissionManagementComponent.prototype.ngOnChanges = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var visible = _a.visible;
        if (!visible)
            return;
        if (visible.currentValue) {
            this.openModal();
        }
        else if (visible.currentValue === false && this.visible) {
            this.visible = false;
        }
    };
    PermissionManagementComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-permission-management',
                    template: "<abp-modal [(visible)]=\"visible\" (init)=\"initModal()\" [busy]=\"modalBusy\">\r\n  <ng-container *ngIf=\"{ entityName: entityName$ | async } as data\">\r\n    <ng-template #abpHeader>\r\n      <h4>{{ 'AbpPermissionManagement::Permissions' | abpLocalization }} - {{ data.entityName }}</h4>\r\n    </ng-template>\r\n    <ng-template #abpBody>\r\n      <div class=\"custom-checkbox custom-control mb-2\">\r\n        <input\r\n          type=\"checkbox\"\r\n          id=\"select-all-in-all-tabs\"\r\n          name=\"select-all-in-all-tabs\"\r\n          class=\"custom-control-input\"\r\n          [(ngModel)]=\"selectAllTab\"\r\n          (click)=\"onClickSelectAll()\"\r\n        />\r\n        <label class=\"custom-control-label\" for=\"select-all-in-all-tabs\">{{\r\n          'AbpPermissionManagement::SelectAllInAllTabs' | abpLocalization\r\n        }}</label>\r\n      </div>\r\n\r\n      <hr class=\"mt-2 mb-2\" />\r\n      <div class=\"row\">\r\n        <div class=\"col-4\">\r\n          <ul class=\"nav nav-pills flex-column\">\r\n            <li *ngFor=\"let group of groups$ | async; trackBy: trackByFn\" class=\"nav-item\">\r\n              <a\r\n                class=\"nav-link pointer\"\r\n                [class.active]=\"selectedGroup?.name === group?.name\"\r\n                (click)=\"onChangeGroup(group)\"\r\n                >{{ group?.displayName }}</a\r\n              >\r\n            </li>\r\n          </ul>\r\n        </div>\r\n        <div class=\"col-8\">\r\n          <h4>{{ selectedGroup?.displayName }}</h4>\r\n          <hr class=\"mt-2 mb-3\" />\r\n          <div class=\"pl-1 pt-1\">\r\n            <div class=\"custom-checkbox custom-control mb-2\">\r\n              <input\r\n                type=\"checkbox\"\r\n                id=\"select-all-in-this-tabs\"\r\n                name=\"select-all-in-this-tabs\"\r\n                class=\"custom-control-input\"\r\n                [(ngModel)]=\"selectThisTab\"\r\n                (click)=\"onClickSelectThisTab()\"\r\n              />\r\n              <label class=\"custom-control-label\" for=\"select-all-in-this-tabs\">{{\r\n                'AbpPermissionManagement::SelectAllInThisTab' | abpLocalization\r\n              }}</label>\r\n            </div>\r\n            <hr class=\"mb-3\" />\r\n            <div\r\n              *ngFor=\"let permission of selectedGroupPermissions$ | async; let i = index; trackBy: trackByFn\"\r\n              [style.margin-left]=\"permission.margin + 'px'\"\r\n              class=\"custom-checkbox custom-control mb-2\"\r\n            >\r\n              <input\r\n                #permissionCheckbox\r\n                type=\"checkbox\"\r\n                [checked]=\"getChecked(permission.name)\"\r\n                [value]=\"getChecked(permission.name)\"\r\n                [attr.id]=\"permission.name\"\r\n                class=\"custom-control-input\"\r\n                [disabled]=\"isGrantedByOtherProviderName(permission.grantedProviders)\"\r\n              />\r\n              <label\r\n                class=\"custom-control-label\"\r\n                [attr.for]=\"permission.name\"\r\n                (click)=\"onClickCheckbox(permission, permissionCheckbox.value)\"\r\n                >{{ permission.displayName }}\r\n                <span *ngFor=\"let provider of permission.grantedProviders\" class=\"badge badge-light\"\r\n                  >{{ provider.providerName }}: {{ provider.providerKey }}</span\r\n                ></label\r\n              >\r\n            </div>\r\n          </div>\r\n        </div>\r\n      </div>\r\n    </ng-template>\r\n    <ng-template #abpFooter>\r\n      <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\r\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\r\n      </button>\r\n      <abp-button iconClass=\"fa fa-check\" (click)=\"submit()\">{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button>\r\n    </ng-template>\r\n  </ng-container>\r\n</abp-modal>\r\n"
                }] }
    ];
    /** @nocollapse */
    PermissionManagementComponent.ctorParameters = function () { return [
        { type: Store },
        { type: Renderer2 }
    ]; };
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
    return PermissionManagementComponent;
}());
export { PermissionManagementComponent };
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
    var parentPermission = permissions.find((/**
     * @param {?} per
     * @return {?}
     */
    function (per) { return per.name === permission.parentName; }));
    if (parentPermission && parentPermission.parentName) {
        /** @type {?} */
        var margin = 20;
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
    function (acc, val) { return tslib_1.__spread(acc, val.permissions); }), []);
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcucGVybWlzc2lvbi1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQ0wsU0FBUyxFQUNULFlBQVksRUFDWixLQUFLLEVBR0wsTUFBTSxFQUNOLFNBQVMsR0FHVixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1QyxPQUFPLEVBQVEsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ3hDLE9BQU8sRUFBRSxHQUFHLEVBQUUsS0FBSyxFQUFFLElBQUksRUFBRSxRQUFRLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUM1RCxPQUFPLEVBQUUsY0FBYyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sMENBQTBDLENBQUM7QUFFN0YsT0FBTyxFQUFFLHlCQUF5QixFQUFFLE1BQU0sdUNBQXVDLENBQUM7QUFNbEY7SUFtRUUsdUNBQW9CLEtBQVksRUFBVSxRQUFtQjtRQUF6QyxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQVUsYUFBUSxHQUFSLFFBQVEsQ0FBVztRQXRDMUMsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBVyxDQUFDO1FBVS9ELGdCQUFXLEdBQXNDLEVBQUUsQ0FBQztRQUVwRCxrQkFBYSxHQUFHLEtBQUssQ0FBQztRQUV0QixpQkFBWSxHQUFHLEtBQUssQ0FBQztRQUVyQixjQUFTLEdBQUcsS0FBSyxDQUFDO1FBRWxCLGNBQVM7Ozs7O1FBQWdELFVBQUMsQ0FBQyxFQUFFLElBQUksSUFBSyxPQUFBLElBQUksQ0FBQyxJQUFJLEVBQVQsQ0FBUyxFQUFDO0lBb0JoQixDQUFDO0lBdERqRSxzQkFDSSxrREFBTzs7OztRQURYO1lB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QzFELE9BQU8sTUFBTSxDQUFDLE1BQU07Ozs7O0lBQUMsVUFBQyxHQUFHLEVBQUUsR0FBRyxJQUFLLHdCQUFJLEdBQUcsRUFBSyxHQUFHLENBQUMsV0FBVyxHQUEzQixDQUE0QixHQUFFLEVBQUUsQ0FBQyxDQUFDO0FBQ3ZFLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xyXG4gIENvbXBvbmVudCxcclxuICBFdmVudEVtaXR0ZXIsXHJcbiAgSW5wdXQsXHJcbiAgT25DaGFuZ2VzLFxyXG4gIE9uSW5pdCxcclxuICBPdXRwdXQsXHJcbiAgUmVuZGVyZXIyLFxyXG4gIFNpbXBsZUNoYW5nZXMsXHJcbiAgVHJhY2tCeUZ1bmN0aW9uLFxyXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBmcm9tLCBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IG1hcCwgcGx1Y2ssIHRha2UsIGZpbmFsaXplIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5pbXBvcnQgeyBHZXRQZXJtaXNzaW9ucywgVXBkYXRlUGVybWlzc2lvbnMgfSBmcm9tICcuLi9hY3Rpb25zL3Blcm1pc3Npb24tbWFuYWdlbWVudC5hY3Rpb25zJztcclxuaW1wb3J0IHsgUGVybWlzc2lvbk1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvcGVybWlzc2lvbi1tYW5hZ2VtZW50JztcclxuaW1wb3J0IHsgUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQuc3RhdGUnO1xyXG5cclxudHlwZSBQZXJtaXNzaW9uV2l0aE1hcmdpbiA9IFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb24gJiB7XHJcbiAgbWFyZ2luOiBudW1iZXI7XHJcbn07XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1wZXJtaXNzaW9uLW1hbmFnZW1lbnQnLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9wZXJtaXNzaW9uLW1hbmFnZW1lbnQuY29tcG9uZW50Lmh0bWwnLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgUGVybWlzc2lvbk1hbmFnZW1lbnRDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQsIE9uQ2hhbmdlcyB7XHJcbiAgQElucHV0KClcclxuICBwcm92aWRlck5hbWU6IHN0cmluZztcclxuXHJcbiAgQElucHV0KClcclxuICBwcm92aWRlcktleTogc3RyaW5nO1xyXG5cclxuICBwcm90ZWN0ZWQgX3Zpc2libGU7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgZ2V0IHZpc2libGUoKTogYm9vbGVhbiB7XHJcbiAgICByZXR1cm4gdGhpcy5fdmlzaWJsZTtcclxuICB9XHJcblxyXG4gIHNldCB2aXNpYmxlKHZhbHVlOiBib29sZWFuKSB7XHJcbiAgICBpZiAoIXRoaXMuc2VsZWN0ZWRHcm91cCkgcmV0dXJuO1xyXG5cclxuICAgIHRoaXMuX3Zpc2libGUgPSB2YWx1ZTtcclxuICAgIHRoaXMudmlzaWJsZUNoYW5nZS5lbWl0KHZhbHVlKTtcclxuXHJcbiAgICBpZiAoIXZhbHVlKSB7XHJcbiAgICAgIHRoaXMuc2VsZWN0ZWRHcm91cCA9IG51bGw7XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgdmlzaWJsZUNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8Ym9vbGVhbj4oKTtcclxuXHJcbiAgQFNlbGVjdChQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlLmdldFBlcm1pc3Npb25Hcm91cHMpXHJcbiAgZ3JvdXBzJDogT2JzZXJ2YWJsZTxQZXJtaXNzaW9uTWFuYWdlbWVudC5Hcm91cFtdPjtcclxuXHJcbiAgQFNlbGVjdChQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlLmdldEVudGl0eURpc3BsYXlOYW1lKVxyXG4gIGVudGl0eU5hbWUkOiBPYnNlcnZhYmxlPHN0cmluZz47XHJcblxyXG4gIHNlbGVjdGVkR3JvdXA6IFBlcm1pc3Npb25NYW5hZ2VtZW50Lkdyb3VwO1xyXG5cclxuICBwZXJtaXNzaW9uczogUGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbltdID0gW107XHJcblxyXG4gIHNlbGVjdFRoaXNUYWIgPSBmYWxzZTtcclxuXHJcbiAgc2VsZWN0QWxsVGFiID0gZmFsc2U7XHJcblxyXG4gIG1vZGFsQnVzeSA9IGZhbHNlO1xyXG5cclxuICB0cmFja0J5Rm46IFRyYWNrQnlGdW5jdGlvbjxQZXJtaXNzaW9uTWFuYWdlbWVudC5Hcm91cD4gPSAoXywgaXRlbSkgPT4gaXRlbS5uYW1lO1xyXG5cclxuICBnZXQgc2VsZWN0ZWRHcm91cFBlcm1pc3Npb25zJCgpOiBPYnNlcnZhYmxlPFBlcm1pc3Npb25XaXRoTWFyZ2luW10+IHtcclxuICAgIHJldHVybiB0aGlzLmdyb3VwcyQucGlwZShcclxuICAgICAgbWFwKGdyb3VwcyA9PlxyXG4gICAgICAgIHRoaXMuc2VsZWN0ZWRHcm91cCA/IGdyb3Vwcy5maW5kKGdyb3VwID0+IGdyb3VwLm5hbWUgPT09IHRoaXMuc2VsZWN0ZWRHcm91cC5uYW1lKS5wZXJtaXNzaW9ucyA6IFtdLFxyXG4gICAgICApLFxyXG4gICAgICBtYXA8UGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbltdLCBQZXJtaXNzaW9uV2l0aE1hcmdpbltdPihwZXJtaXNzaW9ucyA9PlxyXG4gICAgICAgIHBlcm1pc3Npb25zLm1hcChcclxuICAgICAgICAgIHBlcm1pc3Npb24gPT5cclxuICAgICAgICAgICAgKCh7XHJcbiAgICAgICAgICAgICAgLi4ucGVybWlzc2lvbixcclxuICAgICAgICAgICAgICBtYXJnaW46IGZpbmRNYXJnaW4ocGVybWlzc2lvbnMsIHBlcm1pc3Npb24pLFxyXG4gICAgICAgICAgICAgIGlzR3JhbnRlZDogdGhpcy5wZXJtaXNzaW9ucy5maW5kKHBlciA9PiBwZXIubmFtZSA9PT0gcGVybWlzc2lvbi5uYW1lKS5pc0dyYW50ZWQsXHJcbiAgICAgICAgICAgIH0gYXMgYW55KSBhcyBQZXJtaXNzaW9uV2l0aE1hcmdpbiksXHJcbiAgICAgICAgKSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSwgcHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyKSB7fVxyXG5cclxuICBuZ09uSW5pdCgpOiB2b2lkIHt9XHJcblxyXG4gIGdldENoZWNrZWQobmFtZTogc3RyaW5nKSB7XHJcbiAgICByZXR1cm4gKHRoaXMucGVybWlzc2lvbnMuZmluZChwZXIgPT4gcGVyLm5hbWUgPT09IG5hbWUpIHx8IHsgaXNHcmFudGVkOiBmYWxzZSB9KS5pc0dyYW50ZWQ7XHJcbiAgfVxyXG5cclxuICBpc0dyYW50ZWRCeU90aGVyUHJvdmlkZXJOYW1lKGdyYW50ZWRQcm92aWRlcnM6IFBlcm1pc3Npb25NYW5hZ2VtZW50LkdyYW50ZWRQcm92aWRlcltdKTogYm9vbGVhbiB7XHJcbiAgICBpZiAoZ3JhbnRlZFByb3ZpZGVycy5sZW5ndGgpIHtcclxuICAgICAgcmV0dXJuIGdyYW50ZWRQcm92aWRlcnMuZmluZEluZGV4KHAgPT4gcC5wcm92aWRlck5hbWUgIT09IHRoaXMucHJvdmlkZXJOYW1lKSA+IC0xO1xyXG4gICAgfVxyXG4gICAgcmV0dXJuIGZhbHNlO1xyXG4gIH1cclxuXHJcbiAgb25DbGlja0NoZWNrYm94KGNsaWNrZWRQZXJtaXNzaW9uOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uLCB2YWx1ZSkge1xyXG4gICAgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLmlzR3JhbnRlZCAmJiB0aGlzLmlzR3JhbnRlZEJ5T3RoZXJQcm92aWRlck5hbWUoY2xpY2tlZFBlcm1pc3Npb24uZ3JhbnRlZFByb3ZpZGVycykpIHJldHVybjtcclxuXHJcbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcclxuICAgICAgdGhpcy5wZXJtaXNzaW9ucyA9IHRoaXMucGVybWlzc2lvbnMubWFwKHBlciA9PiB7XHJcbiAgICAgICAgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLm5hbWUgPT09IHBlci5uYW1lKSB7XHJcbiAgICAgICAgICByZXR1cm4geyAuLi5wZXIsIGlzR3JhbnRlZDogIXBlci5pc0dyYW50ZWQgfTtcclxuICAgICAgICB9IGVsc2UgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLm5hbWUgPT09IHBlci5wYXJlbnROYW1lICYmIGNsaWNrZWRQZXJtaXNzaW9uLmlzR3JhbnRlZCkge1xyXG4gICAgICAgICAgcmV0dXJuIHsgLi4ucGVyLCBpc0dyYW50ZWQ6IGZhbHNlIH07XHJcbiAgICAgICAgfSBlbHNlIGlmIChjbGlja2VkUGVybWlzc2lvbi5wYXJlbnROYW1lID09PSBwZXIubmFtZSAmJiAhY2xpY2tlZFBlcm1pc3Npb24uaXNHcmFudGVkKSB7XHJcbiAgICAgICAgICByZXR1cm4geyAuLi5wZXIsIGlzR3JhbnRlZDogdHJ1ZSB9O1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgcmV0dXJuIHBlcjtcclxuICAgICAgfSk7XHJcblxyXG4gICAgICB0aGlzLnNldFRhYkNoZWNrYm94U3RhdGUoKTtcclxuICAgICAgdGhpcy5zZXRHcmFudENoZWNrYm94U3RhdGUoKTtcclxuICAgIH0sIDApO1xyXG4gIH1cclxuXHJcbiAgc2V0VGFiQ2hlY2tib3hTdGF0ZSgpIHtcclxuICAgIHRoaXMuc2VsZWN0ZWRHcm91cFBlcm1pc3Npb25zJC5waXBlKHRha2UoMSkpLnN1YnNjcmliZShwZXJtaXNzaW9ucyA9PiB7XHJcbiAgICAgIGNvbnN0IHNlbGVjdGVkUGVybWlzc2lvbnMgPSBwZXJtaXNzaW9ucy5maWx0ZXIocGVyID0+IHBlci5pc0dyYW50ZWQpO1xyXG4gICAgICBjb25zdCBlbGVtZW50ID0gZG9jdW1lbnQucXVlcnlTZWxlY3RvcignI3NlbGVjdC1hbGwtaW4tdGhpcy10YWJzJykgYXMgYW55O1xyXG5cclxuICAgICAgaWYgKHNlbGVjdGVkUGVybWlzc2lvbnMubGVuZ3RoID09PSBwZXJtaXNzaW9ucy5sZW5ndGgpIHtcclxuICAgICAgICBlbGVtZW50LmluZGV0ZXJtaW5hdGUgPSBmYWxzZTtcclxuICAgICAgICB0aGlzLnNlbGVjdFRoaXNUYWIgPSB0cnVlO1xyXG4gICAgICB9IGVsc2UgaWYgKHNlbGVjdGVkUGVybWlzc2lvbnMubGVuZ3RoID09PSAwKSB7XHJcbiAgICAgICAgZWxlbWVudC5pbmRldGVybWluYXRlID0gZmFsc2U7XHJcbiAgICAgICAgdGhpcy5zZWxlY3RUaGlzVGFiID0gZmFsc2U7XHJcbiAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgZWxlbWVudC5pbmRldGVybWluYXRlID0gdHJ1ZTtcclxuICAgICAgfVxyXG4gICAgfSk7XHJcbiAgfVxyXG5cclxuICBzZXRHcmFudENoZWNrYm94U3RhdGUoKSB7XHJcbiAgICBjb25zdCBzZWxlY3RlZEFsbFBlcm1pc3Npb25zID0gdGhpcy5wZXJtaXNzaW9ucy5maWx0ZXIocGVyID0+IHBlci5pc0dyYW50ZWQpO1xyXG4gICAgY29uc3QgY2hlY2tib3hFbGVtZW50ID0gZG9jdW1lbnQucXVlcnlTZWxlY3RvcignI3NlbGVjdC1hbGwtaW4tYWxsLXRhYnMnKSBhcyBhbnk7XHJcblxyXG4gICAgaWYgKHNlbGVjdGVkQWxsUGVybWlzc2lvbnMubGVuZ3RoID09PSB0aGlzLnBlcm1pc3Npb25zLmxlbmd0aCkge1xyXG4gICAgICBjaGVja2JveEVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IGZhbHNlO1xyXG4gICAgICB0aGlzLnNlbGVjdEFsbFRhYiA9IHRydWU7XHJcbiAgICB9IGVsc2UgaWYgKHNlbGVjdGVkQWxsUGVybWlzc2lvbnMubGVuZ3RoID09PSAwKSB7XHJcbiAgICAgIGNoZWNrYm94RWxlbWVudC5pbmRldGVybWluYXRlID0gZmFsc2U7XHJcbiAgICAgIHRoaXMuc2VsZWN0QWxsVGFiID0gZmFsc2U7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICBjaGVja2JveEVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IHRydWU7XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBvbkNsaWNrU2VsZWN0VGhpc1RhYigpIHtcclxuICAgIHRoaXMuc2VsZWN0ZWRHcm91cFBlcm1pc3Npb25zJC5waXBlKHRha2UoMSkpLnN1YnNjcmliZShwZXJtaXNzaW9ucyA9PiB7XHJcbiAgICAgIHBlcm1pc3Npb25zLmZvckVhY2gocGVybWlzc2lvbiA9PiB7XHJcbiAgICAgICAgaWYgKHBlcm1pc3Npb24uaXNHcmFudGVkICYmIHRoaXMuaXNHcmFudGVkQnlPdGhlclByb3ZpZGVyTmFtZShwZXJtaXNzaW9uLmdyYW50ZWRQcm92aWRlcnMpKSByZXR1cm47XHJcblxyXG4gICAgICAgIGNvbnN0IGluZGV4ID0gdGhpcy5wZXJtaXNzaW9ucy5maW5kSW5kZXgocGVyID0+IHBlci5uYW1lID09PSBwZXJtaXNzaW9uLm5hbWUpO1xyXG5cclxuICAgICAgICB0aGlzLnBlcm1pc3Npb25zID0gW1xyXG4gICAgICAgICAgLi4udGhpcy5wZXJtaXNzaW9ucy5zbGljZSgwLCBpbmRleCksXHJcbiAgICAgICAgICB7IC4uLnRoaXMucGVybWlzc2lvbnNbaW5kZXhdLCBpc0dyYW50ZWQ6ICF0aGlzLnNlbGVjdFRoaXNUYWIgfSxcclxuICAgICAgICAgIC4uLnRoaXMucGVybWlzc2lvbnMuc2xpY2UoaW5kZXggKyAxKSxcclxuICAgICAgICBdO1xyXG4gICAgICB9KTtcclxuICAgIH0pO1xyXG5cclxuICAgIHRoaXMuc2V0R3JhbnRDaGVja2JveFN0YXRlKCk7XHJcbiAgfVxyXG5cclxuICBvbkNsaWNrU2VsZWN0QWxsKCkge1xyXG4gICAgdGhpcy5wZXJtaXNzaW9ucyA9IHRoaXMucGVybWlzc2lvbnMubWFwKHBlcm1pc3Npb24gPT4gKHtcclxuICAgICAgLi4ucGVybWlzc2lvbixcclxuICAgICAgaXNHcmFudGVkOiB0aGlzLmlzR3JhbnRlZEJ5T3RoZXJQcm92aWRlck5hbWUocGVybWlzc2lvbi5ncmFudGVkUHJvdmlkZXJzKSB8fCAhdGhpcy5zZWxlY3RBbGxUYWIsXHJcbiAgICB9KSk7XHJcblxyXG4gICAgdGhpcy5zZWxlY3RUaGlzVGFiID0gIXRoaXMuc2VsZWN0QWxsVGFiO1xyXG4gIH1cclxuXHJcbiAgb25DaGFuZ2VHcm91cChncm91cDogUGVybWlzc2lvbk1hbmFnZW1lbnQuR3JvdXApIHtcclxuICAgIHRoaXMuc2VsZWN0ZWRHcm91cCA9IGdyb3VwO1xyXG4gICAgdGhpcy5zZXRUYWJDaGVja2JveFN0YXRlKCk7XHJcbiAgfVxyXG5cclxuICBzdWJtaXQoKSB7XHJcbiAgICB0aGlzLm1vZGFsQnVzeSA9IHRydWU7XHJcbiAgICBjb25zdCB1bmNoYW5nZWRQZXJtaXNzaW9ucyA9IGdldFBlcm1pc3Npb25zKFxyXG4gICAgICB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUuZ2V0UGVybWlzc2lvbkdyb3VwcyksXHJcbiAgICApO1xyXG5cclxuICAgIGNvbnN0IGNoYW5nZWRQZXJtaXNzaW9uczogUGVybWlzc2lvbk1hbmFnZW1lbnQuTWluaW11bVBlcm1pc3Npb25bXSA9IHRoaXMucGVybWlzc2lvbnNcclxuICAgICAgLmZpbHRlcihwZXIgPT5cclxuICAgICAgICB1bmNoYW5nZWRQZXJtaXNzaW9ucy5maW5kKHVuY2hhbmdlZCA9PiB1bmNoYW5nZWQubmFtZSA9PT0gcGVyLm5hbWUpLmlzR3JhbnRlZCA9PT0gcGVyLmlzR3JhbnRlZCA/IGZhbHNlIDogdHJ1ZSxcclxuICAgICAgKVxyXG4gICAgICAubWFwKCh7IG5hbWUsIGlzR3JhbnRlZCB9KSA9PiAoeyBuYW1lLCBpc0dyYW50ZWQgfSkpO1xyXG5cclxuICAgIGlmIChjaGFuZ2VkUGVybWlzc2lvbnMubGVuZ3RoKSB7XHJcbiAgICAgIHRoaXMuc3RvcmVcclxuICAgICAgICAuZGlzcGF0Y2goXHJcbiAgICAgICAgICBuZXcgVXBkYXRlUGVybWlzc2lvbnMoe1xyXG4gICAgICAgICAgICBwcm92aWRlcktleTogdGhpcy5wcm92aWRlcktleSxcclxuICAgICAgICAgICAgcHJvdmlkZXJOYW1lOiB0aGlzLnByb3ZpZGVyTmFtZSxcclxuICAgICAgICAgICAgcGVybWlzc2lvbnM6IGNoYW5nZWRQZXJtaXNzaW9ucyxcclxuICAgICAgICAgIH0pLFxyXG4gICAgICAgIClcclxuICAgICAgICAucGlwZShmaW5hbGl6ZSgoKSA9PiAodGhpcy5tb2RhbEJ1c3kgPSBmYWxzZSkpKVxyXG4gICAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xyXG4gICAgICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XHJcbiAgICAgICAgfSk7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICB0aGlzLm1vZGFsQnVzeSA9IGZhbHNlO1xyXG4gICAgICB0aGlzLnZpc2libGUgPSBmYWxzZTtcclxuICAgIH1cclxuICB9XHJcblxyXG4gIG9wZW5Nb2RhbCgpIHtcclxuICAgIGlmICghdGhpcy5wcm92aWRlcktleSB8fCAhdGhpcy5wcm92aWRlck5hbWUpIHtcclxuICAgICAgdGhyb3cgbmV3IEVycm9yKCdQcm92aWRlciBLZXkgYW5kIFByb3ZpZGVyIE5hbWUgYXJlIHJlcXVpcmVkLicpO1xyXG4gICAgfVxyXG5cclxuICAgIHRoaXMuc3RvcmVcclxuICAgICAgLmRpc3BhdGNoKFxyXG4gICAgICAgIG5ldyBHZXRQZXJtaXNzaW9ucyh7XHJcbiAgICAgICAgICBwcm92aWRlcktleTogdGhpcy5wcm92aWRlcktleSxcclxuICAgICAgICAgIHByb3ZpZGVyTmFtZTogdGhpcy5wcm92aWRlck5hbWUsXHJcbiAgICAgICAgfSksXHJcbiAgICAgIClcclxuICAgICAgLnBpcGUocGx1Y2soJ1Blcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUnLCAncGVybWlzc2lvblJlcycpKVxyXG4gICAgICAuc3Vic2NyaWJlKChwZXJtaXNzaW9uUmVzOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5SZXNwb25zZSkgPT4ge1xyXG4gICAgICAgIHRoaXMuc2VsZWN0ZWRHcm91cCA9IHBlcm1pc3Npb25SZXMuZ3JvdXBzWzBdO1xyXG4gICAgICAgIHRoaXMucGVybWlzc2lvbnMgPSBnZXRQZXJtaXNzaW9ucyhwZXJtaXNzaW9uUmVzLmdyb3Vwcyk7XHJcblxyXG4gICAgICAgIHRoaXMudmlzaWJsZSA9IHRydWU7XHJcbiAgICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgaW5pdE1vZGFsKCkge1xyXG4gICAgdGhpcy5zZXRUYWJDaGVja2JveFN0YXRlKCk7XHJcbiAgICB0aGlzLnNldEdyYW50Q2hlY2tib3hTdGF0ZSgpO1xyXG4gIH1cclxuXHJcbiAgbmdPbkNoYW5nZXMoeyB2aXNpYmxlIH06IFNpbXBsZUNoYW5nZXMpOiB2b2lkIHtcclxuICAgIGlmICghdmlzaWJsZSkgcmV0dXJuO1xyXG5cclxuICAgIGlmICh2aXNpYmxlLmN1cnJlbnRWYWx1ZSkge1xyXG4gICAgICB0aGlzLm9wZW5Nb2RhbCgpO1xyXG4gICAgfSBlbHNlIGlmICh2aXNpYmxlLmN1cnJlbnRWYWx1ZSA9PT0gZmFsc2UgJiYgdGhpcy52aXNpYmxlKSB7XHJcbiAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xyXG4gICAgfVxyXG4gIH1cclxufVxyXG5cclxuZnVuY3Rpb24gZmluZE1hcmdpbihwZXJtaXNzaW9uczogUGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbltdLCBwZXJtaXNzaW9uOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uKSB7XHJcbiAgY29uc3QgcGFyZW50UGVybWlzc2lvbiA9IHBlcm1pc3Npb25zLmZpbmQocGVyID0+IHBlci5uYW1lID09PSBwZXJtaXNzaW9uLnBhcmVudE5hbWUpO1xyXG5cclxuICBpZiAocGFyZW50UGVybWlzc2lvbiAmJiBwYXJlbnRQZXJtaXNzaW9uLnBhcmVudE5hbWUpIHtcclxuICAgIGxldCBtYXJnaW4gPSAyMDtcclxuICAgIHJldHVybiAobWFyZ2luICs9IGZpbmRNYXJnaW4ocGVybWlzc2lvbnMsIHBhcmVudFBlcm1pc3Npb24pKTtcclxuICB9XHJcblxyXG4gIHJldHVybiBwYXJlbnRQZXJtaXNzaW9uID8gMjAgOiAwO1xyXG59XHJcblxyXG5mdW5jdGlvbiBnZXRQZXJtaXNzaW9ucyhncm91cHM6IFBlcm1pc3Npb25NYW5hZ2VtZW50Lkdyb3VwW10pOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uW10ge1xyXG4gIHJldHVybiBncm91cHMucmVkdWNlKChhY2MsIHZhbCkgPT4gWy4uLmFjYywgLi4udmFsLnBlcm1pc3Npb25zXSwgW10pO1xyXG59XHJcbiJdfQ==