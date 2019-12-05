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
        this.hideBadges = false;
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
                return _this.selectedGroup
                    ? groups.find((/**
                     * @param {?} group
                     * @return {?}
                     */
                    function (group) { return group.name === _this.selectedGroup.name; })).permissions
                    : [];
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
        if (clickedPermission.isGranted &&
            this.isGrantedByOtherProviderName(clickedPermission.grantedProviders))
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
            function (unchanged) { return unchanged.name === per.name; })).isGranted ===
                per.isGranted
                ? false
                : true;
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
                    template: "<abp-modal [(visible)]=\"visible\" (init)=\"initModal()\" [busy]=\"modalBusy\">\n  <ng-container *ngIf=\"{ entityName: entityName$ | async } as data\">\n    <ng-template #abpHeader>\n      <h4>\n        {{ 'AbpPermissionManagement::Permissions' | abpLocalization }} - {{ data.entityName }}\n      </h4>\n    </ng-template>\n    <ng-template #abpBody>\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input\n          type=\"checkbox\"\n          id=\"select-all-in-all-tabs\"\n          name=\"select-all-in-all-tabs\"\n          class=\"custom-control-input\"\n          [(ngModel)]=\"selectAllTab\"\n          (click)=\"onClickSelectAll()\"\n        />\n        <label class=\"custom-control-label\" for=\"select-all-in-all-tabs\">{{\n          'AbpPermissionManagement::SelectAllInAllTabs' | abpLocalization\n        }}</label>\n      </div>\n\n      <hr class=\"mt-2 mb-2\" />\n      <div class=\"row\">\n        <div class=\"col-4\">\n          <ul class=\"nav nav-pills flex-column\">\n            <li *ngFor=\"let group of groups$ | async; trackBy: trackByFn\" class=\"nav-item\">\n              <a\n                class=\"nav-link pointer\"\n                [class.active]=\"selectedGroup?.name === group?.name\"\n                (click)=\"onChangeGroup(group)\"\n                >{{ group?.displayName }}</a\n              >\n            </li>\n          </ul>\n        </div>\n        <div class=\"col-8\">\n          <h4>{{ selectedGroup?.displayName }}</h4>\n          <hr class=\"mt-2 mb-3\" />\n          <div class=\"pl-1 pt-1\">\n            <div class=\"custom-checkbox custom-control mb-2\">\n              <input\n                type=\"checkbox\"\n                id=\"select-all-in-this-tabs\"\n                name=\"select-all-in-this-tabs\"\n                class=\"custom-control-input\"\n                [(ngModel)]=\"selectThisTab\"\n                (click)=\"onClickSelectThisTab()\"\n              />\n              <label class=\"custom-control-label\" for=\"select-all-in-this-tabs\">{{\n                'AbpPermissionManagement::SelectAllInThisTab' | abpLocalization\n              }}</label>\n            </div>\n            <hr class=\"mb-3\" />\n            <div\n              *ngFor=\"\n                let permission of selectedGroupPermissions$ | async;\n                let i = index;\n                trackBy: trackByFn\n              \"\n              [style.margin-left]=\"permission.margin + 'px'\"\n              class=\"custom-checkbox custom-control mb-2\"\n            >\n              <input\n                #permissionCheckbox\n                type=\"checkbox\"\n                [checked]=\"getChecked(permission.name)\"\n                [value]=\"getChecked(permission.name)\"\n                [attr.id]=\"permission.name\"\n                class=\"custom-control-input\"\n                [disabled]=\"isGrantedByOtherProviderName(permission.grantedProviders)\"\n              />\n              <label\n                class=\"custom-control-label\"\n                [attr.for]=\"permission.name\"\n                (click)=\"onClickCheckbox(permission, permissionCheckbox.value)\"\n                >{{ permission.displayName }}\n                <ng-container *ngIf=\"!hideBadges\">\n                  <span\n                    *ngFor=\"let provider of permission.grantedProviders\"\n                    class=\"badge badge-light\"\n                    >{{ provider.providerName }}: {{ provider.providerKey }}</span\n                  >\n                </ng-container>\n              </label>\n            </div>\n          </div>\n        </div>\n      </div>\n    </ng-template>\n    <ng-template #abpFooter>\n      <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\n      </button>\n      <abp-button iconClass=\"fa fa-check\" (click)=\"submit()\">{{\n        'AbpIdentity::Save' | abpLocalization\n      }}</abp-button>\n    </ng-template>\n  </ng-container>\n</abp-modal>\n"
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
        hideBadges: [{ type: Input }],
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
    /** @type {?} */
    PermissionManagementComponent.prototype.hideBadges;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcucGVybWlzc2lvbi1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQ0wsU0FBUyxFQUNULFlBQVksRUFDWixLQUFLLEVBR0wsTUFBTSxFQUNOLFNBQVMsR0FHVixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1QyxPQUFPLEVBQVEsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ3hDLE9BQU8sRUFBRSxHQUFHLEVBQUUsS0FBSyxFQUFFLElBQUksRUFBRSxRQUFRLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUM1RCxPQUFPLEVBQUUsY0FBYyxFQUFFLGlCQUFpQixFQUFFLE1BQU0sMENBQTBDLENBQUM7QUFFN0YsT0FBTyxFQUFFLHlCQUF5QixFQUFFLE1BQU0sdUNBQXVDLENBQUM7QUFNbEY7SUF3RUUsdUNBQW9CLEtBQVksRUFBVSxRQUFtQjtRQUF6QyxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQVUsYUFBUSxHQUFSLFFBQVEsQ0FBVztRQTVEN0QsZUFBVSxHQUFHLEtBQUssQ0FBQztRQW9CQSxrQkFBYSxHQUFHLElBQUksWUFBWSxFQUFXLENBQUM7UUFVL0QsZ0JBQVcsR0FBc0MsRUFBRSxDQUFDO1FBRXBELGtCQUFhLEdBQUcsS0FBSyxDQUFDO1FBRXRCLGlCQUFZLEdBQUcsS0FBSyxDQUFDO1FBRXJCLGNBQVMsR0FBRyxLQUFLLENBQUM7UUFFbEIsY0FBUzs7Ozs7UUFBZ0QsVUFBQyxDQUFDLEVBQUUsSUFBSSxJQUFLLE9BQUEsSUFBSSxDQUFDLElBQUksRUFBVCxDQUFTLEVBQUM7SUFzQmhCLENBQUM7SUF4RGpFLHNCQUNJLGtEQUFPOzs7O1FBRFg7WUFFRSxPQUFPLElBQUksQ0FBQyxRQUFRLENBQUM7UUFDdkIsQ0FBQzs7Ozs7UUFFRCxVQUFZLEtBQWM7WUFDeEIsSUFBSSxDQUFDLElBQUksQ0FBQyxhQUFhO2dCQUFFLE9BQU87WUFFaEMsSUFBSSxDQUFDLFFBQVEsR0FBRyxLQUFLLENBQUM7WUFDdEIsSUFBSSxDQUFDLGFBQWEsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7WUFFL0IsSUFBSSxDQUFDLEtBQUssRUFBRTtnQkFDVixJQUFJLENBQUMsYUFBYSxHQUFHLElBQUksQ0FBQzthQUMzQjtRQUNILENBQUM7OztPQVhBO0lBaUNELHNCQUFJLG9FQUF5Qjs7OztRQUE3QjtZQUFBLGlCQWtCQztZQWpCQyxPQUFPLElBQUksQ0FBQyxPQUFPLENBQUMsSUFBSSxDQUN0QixHQUFHOzs7O1lBQUMsVUFBQSxNQUFNO2dCQUNSLE9BQUEsS0FBSSxDQUFDLGFBQWE7b0JBQ2hCLENBQUMsQ0FBQyxNQUFNLENBQUMsSUFBSTs7OztvQkFBQyxVQUFBLEtBQUssSUFBSSxPQUFBLEtBQUssQ0FBQyxJQUFJLEtBQUssS0FBSSxDQUFDLGFBQWEsQ0FBQyxJQUFJLEVBQXRDLENBQXNDLEVBQUMsQ0FBQyxXQUFXO29CQUMxRSxDQUFDLENBQUMsRUFBRTtZQUZOLENBRU0sRUFDUCxFQUNELEdBQUc7Ozs7WUFBNEQsVUFBQSxXQUFXO2dCQUN4RSxPQUFBLFdBQVcsQ0FBQyxHQUFHOzs7O2dCQUNiLFVBQUEsVUFBVTtvQkFDUixPQUFBLENBQUMsbUJBQUEsQ0FBQyx3Q0FDRyxVQUFVLElBQ2IsTUFBTSxFQUFFLFVBQVUsQ0FBQyxXQUFXLEVBQUUsVUFBVSxDQUFDLEVBQzNDLFNBQVMsRUFBRSxLQUFJLENBQUMsV0FBVyxDQUFDLElBQUk7Ozs7d0JBQUMsVUFBQSxHQUFHLElBQUksT0FBQSxHQUFHLENBQUMsSUFBSSxLQUFLLFVBQVUsQ0FBQyxJQUFJLEVBQTVCLENBQTRCLEVBQUMsQ0FBQyxTQUFTLEtBQ3pFLENBQUMsRUFBd0IsQ0FBQztnQkFKbEMsQ0FJa0MsRUFDckM7WUFQRCxDQU9DLEVBQ0YsQ0FDRixDQUFDO1FBQ0osQ0FBQzs7O09BQUE7Ozs7SUFJRCxnREFBUTs7O0lBQVIsY0FBa0IsQ0FBQzs7Ozs7SUFFbkIsa0RBQVU7Ozs7SUFBVixVQUFXLElBQVk7UUFDckIsT0FBTyxDQUFDLElBQUksQ0FBQyxXQUFXLENBQUMsSUFBSTs7OztRQUFDLFVBQUEsR0FBRyxJQUFJLE9BQUEsR0FBRyxDQUFDLElBQUksS0FBSyxJQUFJLEVBQWpCLENBQWlCLEVBQUMsSUFBSSxFQUFFLFNBQVMsRUFBRSxLQUFLLEVBQUUsQ0FBQyxDQUFDLFNBQVMsQ0FBQztJQUM3RixDQUFDOzs7OztJQUVELG9FQUE0Qjs7OztJQUE1QixVQUE2QixnQkFBd0Q7UUFBckYsaUJBS0M7UUFKQyxJQUFJLGdCQUFnQixDQUFDLE1BQU0sRUFBRTtZQUMzQixPQUFPLGdCQUFnQixDQUFDLFNBQVM7Ozs7WUFBQyxVQUFBLENBQUMsSUFBSSxPQUFBLENBQUMsQ0FBQyxZQUFZLEtBQUssS0FBSSxDQUFDLFlBQVksRUFBcEMsQ0FBb0MsRUFBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO1NBQ25GO1FBQ0QsT0FBTyxLQUFLLENBQUM7SUFDZixDQUFDOzs7Ozs7SUFFRCx1REFBZTs7Ozs7SUFBZixVQUFnQixpQkFBa0QsRUFBRSxLQUFLO1FBQXpFLGlCQXVCQztRQXRCQyxJQUNFLGlCQUFpQixDQUFDLFNBQVM7WUFDM0IsSUFBSSxDQUFDLDRCQUE0QixDQUFDLGlCQUFpQixDQUFDLGdCQUFnQixDQUFDO1lBRXJFLE9BQU87UUFFVCxVQUFVOzs7UUFBQztZQUNULEtBQUksQ0FBQyxXQUFXLEdBQUcsS0FBSSxDQUFDLFdBQVcsQ0FBQyxHQUFHOzs7O1lBQUMsVUFBQSxHQUFHO2dCQUN6QyxJQUFJLGlCQUFpQixDQUFDLElBQUksS0FBSyxHQUFHLENBQUMsSUFBSSxFQUFFO29CQUN2Qyw0QkFBWSxHQUFHLElBQUUsU0FBUyxFQUFFLENBQUMsR0FBRyxDQUFDLFNBQVMsSUFBRztpQkFDOUM7cUJBQU0sSUFBSSxpQkFBaUIsQ0FBQyxJQUFJLEtBQUssR0FBRyxDQUFDLFVBQVUsSUFBSSxpQkFBaUIsQ0FBQyxTQUFTLEVBQUU7b0JBQ25GLDRCQUFZLEdBQUcsSUFBRSxTQUFTLEVBQUUsS0FBSyxJQUFHO2lCQUNyQztxQkFBTSxJQUFJLGlCQUFpQixDQUFDLFVBQVUsS0FBSyxHQUFHLENBQUMsSUFBSSxJQUFJLENBQUMsaUJBQWlCLENBQUMsU0FBUyxFQUFFO29CQUNwRiw0QkFBWSxHQUFHLElBQUUsU0FBUyxFQUFFLElBQUksSUFBRztpQkFDcEM7Z0JBRUQsT0FBTyxHQUFHLENBQUM7WUFDYixDQUFDLEVBQUMsQ0FBQztZQUVILEtBQUksQ0FBQyxtQkFBbUIsRUFBRSxDQUFDO1lBQzNCLEtBQUksQ0FBQyxxQkFBcUIsRUFBRSxDQUFDO1FBQy9CLENBQUMsR0FBRSxDQUFDLENBQUMsQ0FBQztJQUNSLENBQUM7Ozs7SUFFRCwyREFBbUI7OztJQUFuQjtRQUFBLGlCQWVDO1FBZEMsSUFBSSxDQUFDLHlCQUF5QixDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxTQUFTOzs7O1FBQUMsVUFBQSxXQUFXOztnQkFDMUQsbUJBQW1CLEdBQUcsV0FBVyxDQUFDLE1BQU07Ozs7WUFBQyxVQUFBLEdBQUcsSUFBSSxPQUFBLEdBQUcsQ0FBQyxTQUFTLEVBQWIsQ0FBYSxFQUFDOztnQkFDOUQsT0FBTyxHQUFHLG1CQUFBLFFBQVEsQ0FBQyxhQUFhLENBQUMsMEJBQTBCLENBQUMsRUFBTztZQUV6RSxJQUFJLG1CQUFtQixDQUFDLE1BQU0sS0FBSyxXQUFXLENBQUMsTUFBTSxFQUFFO2dCQUNyRCxPQUFPLENBQUMsYUFBYSxHQUFHLEtBQUssQ0FBQztnQkFDOUIsS0FBSSxDQUFDLGFBQWEsR0FBRyxJQUFJLENBQUM7YUFDM0I7aUJBQU0sSUFBSSxtQkFBbUIsQ0FBQyxNQUFNLEtBQUssQ0FBQyxFQUFFO2dCQUMzQyxPQUFPLENBQUMsYUFBYSxHQUFHLEtBQUssQ0FBQztnQkFDOUIsS0FBSSxDQUFDLGFBQWEsR0FBRyxLQUFLLENBQUM7YUFDNUI7aUJBQU07Z0JBQ0wsT0FBTyxDQUFDLGFBQWEsR0FBRyxJQUFJLENBQUM7YUFDOUI7UUFDSCxDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUM7Ozs7SUFFRCw2REFBcUI7OztJQUFyQjs7WUFDUSxzQkFBc0IsR0FBRyxJQUFJLENBQUMsV0FBVyxDQUFDLE1BQU07Ozs7UUFBQyxVQUFBLEdBQUcsSUFBSSxPQUFBLEdBQUcsQ0FBQyxTQUFTLEVBQWIsQ0FBYSxFQUFDOztZQUN0RSxlQUFlLEdBQUcsbUJBQUEsUUFBUSxDQUFDLGFBQWEsQ0FBQyx5QkFBeUIsQ0FBQyxFQUFPO1FBRWhGLElBQUksc0JBQXNCLENBQUMsTUFBTSxLQUFLLElBQUksQ0FBQyxXQUFXLENBQUMsTUFBTSxFQUFFO1lBQzdELGVBQWUsQ0FBQyxhQUFhLEdBQUcsS0FBSyxDQUFDO1lBQ3RDLElBQUksQ0FBQyxZQUFZLEdBQUcsSUFBSSxDQUFDO1NBQzFCO2FBQU0sSUFBSSxzQkFBc0IsQ0FBQyxNQUFNLEtBQUssQ0FBQyxFQUFFO1lBQzlDLGVBQWUsQ0FBQyxhQUFhLEdBQUcsS0FBSyxDQUFDO1lBQ3RDLElBQUksQ0FBQyxZQUFZLEdBQUcsS0FBSyxDQUFDO1NBQzNCO2FBQU07WUFDTCxlQUFlLENBQUMsYUFBYSxHQUFHLElBQUksQ0FBQztTQUN0QztJQUNILENBQUM7Ozs7SUFFRCw0REFBb0I7OztJQUFwQjtRQUFBLGlCQWlCQztRQWhCQyxJQUFJLENBQUMseUJBQXlCLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLFNBQVM7Ozs7UUFBQyxVQUFBLFdBQVc7WUFDaEUsV0FBVyxDQUFDLE9BQU87Ozs7WUFBQyxVQUFBLFVBQVU7Z0JBQzVCLElBQUksVUFBVSxDQUFDLFNBQVMsSUFBSSxLQUFJLENBQUMsNEJBQTRCLENBQUMsVUFBVSxDQUFDLGdCQUFnQixDQUFDO29CQUN4RixPQUFPOztvQkFFSCxLQUFLLEdBQUcsS0FBSSxDQUFDLFdBQVcsQ0FBQyxTQUFTOzs7O2dCQUFDLFVBQUEsR0FBRyxJQUFJLE9BQUEsR0FBRyxDQUFDLElBQUksS0FBSyxVQUFVLENBQUMsSUFBSSxFQUE1QixDQUE0QixFQUFDO2dCQUU3RSxLQUFJLENBQUMsV0FBVyxvQkFDWCxLQUFJLENBQUMsV0FBVyxDQUFDLEtBQUssQ0FBQyxDQUFDLEVBQUUsS0FBSyxDQUFDO3lDQUM5QixLQUFJLENBQUMsV0FBVyxDQUFDLEtBQUssQ0FBQyxJQUFFLFNBQVMsRUFBRSxDQUFDLEtBQUksQ0FBQyxhQUFhO21CQUN6RCxLQUFJLENBQUMsV0FBVyxDQUFDLEtBQUssQ0FBQyxLQUFLLEdBQUcsQ0FBQyxDQUFDLENBQ3JDLENBQUM7WUFDSixDQUFDLEVBQUMsQ0FBQztRQUNMLENBQUMsRUFBQyxDQUFDO1FBRUgsSUFBSSxDQUFDLHFCQUFxQixFQUFFLENBQUM7SUFDL0IsQ0FBQzs7OztJQUVELHdEQUFnQjs7O0lBQWhCO1FBQUEsaUJBUUM7UUFQQyxJQUFJLENBQUMsV0FBVyxHQUFHLElBQUksQ0FBQyxXQUFXLENBQUMsR0FBRzs7OztRQUFDLFVBQUEsVUFBVSxJQUFJLE9BQUEsc0JBQ2pELFVBQVUsSUFDYixTQUFTLEVBQ1AsS0FBSSxDQUFDLDRCQUE0QixDQUFDLFVBQVUsQ0FBQyxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsS0FBSSxDQUFDLFlBQVksSUFDdEYsRUFKb0QsQ0FJcEQsRUFBQyxDQUFDO1FBRUosSUFBSSxDQUFDLGFBQWEsR0FBRyxDQUFDLElBQUksQ0FBQyxZQUFZLENBQUM7SUFDMUMsQ0FBQzs7Ozs7SUFFRCxxREFBYTs7OztJQUFiLFVBQWMsS0FBaUM7UUFDN0MsSUFBSSxDQUFDLGFBQWEsR0FBRyxLQUFLLENBQUM7UUFDM0IsSUFBSSxDQUFDLG1CQUFtQixFQUFFLENBQUM7SUFDN0IsQ0FBQzs7OztJQUVELDhDQUFNOzs7SUFBTjtRQUFBLGlCQWdDQztRQS9CQyxJQUFJLENBQUMsU0FBUyxHQUFHLElBQUksQ0FBQzs7WUFDaEIsb0JBQW9CLEdBQUcsY0FBYyxDQUN6QyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyx5QkFBeUIsQ0FBQyxtQkFBbUIsQ0FBQyxDQUN6RTs7WUFFSyxrQkFBa0IsR0FBNkMsSUFBSSxDQUFDLFdBQVc7YUFDbEYsTUFBTTs7OztRQUFDLFVBQUEsR0FBRztZQUNULE9BQUEsb0JBQW9CLENBQUMsSUFBSTs7OztZQUFDLFVBQUEsU0FBUyxJQUFJLE9BQUEsU0FBUyxDQUFDLElBQUksS0FBSyxHQUFHLENBQUMsSUFBSSxFQUEzQixDQUEyQixFQUFDLENBQUMsU0FBUztnQkFDN0UsR0FBRyxDQUFDLFNBQVM7Z0JBQ1gsQ0FBQyxDQUFDLEtBQUs7Z0JBQ1AsQ0FBQyxDQUFDLElBQUk7UUFIUixDQUdRLEVBQ1Q7YUFDQSxHQUFHOzs7O1FBQUMsVUFBQyxFQUFtQjtnQkFBakIsY0FBSSxFQUFFLHdCQUFTO1lBQU8sT0FBQSxDQUFDLEVBQUUsSUFBSSxNQUFBLEVBQUUsU0FBUyxXQUFBLEVBQUUsQ0FBQztRQUFyQixDQUFxQixFQUFDO1FBRXRELElBQUksa0JBQWtCLENBQUMsTUFBTSxFQUFFO1lBQzdCLElBQUksQ0FBQyxLQUFLO2lCQUNQLFFBQVEsQ0FDUCxJQUFJLGlCQUFpQixDQUFDO2dCQUNwQixXQUFXLEVBQUUsSUFBSSxDQUFDLFdBQVc7Z0JBQzdCLFlBQVksRUFBRSxJQUFJLENBQUMsWUFBWTtnQkFDL0IsV0FBVyxFQUFFLGtCQUFrQjthQUNoQyxDQUFDLENBQ0g7aUJBQ0EsSUFBSSxDQUFDLFFBQVE7OztZQUFDLGNBQU0sT0FBQSxDQUFDLEtBQUksQ0FBQyxTQUFTLEdBQUcsS0FBSyxDQUFDLEVBQXhCLENBQXdCLEVBQUMsQ0FBQztpQkFDOUMsU0FBUzs7O1lBQUM7Z0JBQ1QsS0FBSSxDQUFDLE9BQU8sR0FBRyxLQUFLLENBQUM7WUFDdkIsQ0FBQyxFQUFDLENBQUM7U0FDTjthQUFNO1lBQ0wsSUFBSSxDQUFDLFNBQVMsR0FBRyxLQUFLLENBQUM7WUFDdkIsSUFBSSxDQUFDLE9BQU8sR0FBRyxLQUFLLENBQUM7U0FDdEI7SUFDSCxDQUFDOzs7O0lBRUQsaURBQVM7OztJQUFUO1FBQUEsaUJBbUJDO1FBbEJDLElBQUksQ0FBQyxJQUFJLENBQUMsV0FBVyxJQUFJLENBQUMsSUFBSSxDQUFDLFlBQVksRUFBRTtZQUMzQyxNQUFNLElBQUksS0FBSyxDQUFDLDhDQUE4QyxDQUFDLENBQUM7U0FDakU7UUFFRCxJQUFJLENBQUMsS0FBSzthQUNQLFFBQVEsQ0FDUCxJQUFJLGNBQWMsQ0FBQztZQUNqQixXQUFXLEVBQUUsSUFBSSxDQUFDLFdBQVc7WUFDN0IsWUFBWSxFQUFFLElBQUksQ0FBQyxZQUFZO1NBQ2hDLENBQUMsQ0FDSDthQUNBLElBQUksQ0FBQyxLQUFLLENBQUMsMkJBQTJCLEVBQUUsZUFBZSxDQUFDLENBQUM7YUFDekQsU0FBUzs7OztRQUFDLFVBQUMsYUFBNEM7WUFDdEQsS0FBSSxDQUFDLGFBQWEsR0FBRyxhQUFhLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxDQUFDO1lBQzdDLEtBQUksQ0FBQyxXQUFXLEdBQUcsY0FBYyxDQUFDLGFBQWEsQ0FBQyxNQUFNLENBQUMsQ0FBQztZQUV4RCxLQUFJLENBQUMsT0FBTyxHQUFHLElBQUksQ0FBQztRQUN0QixDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7Ozs7SUFFRCxpREFBUzs7O0lBQVQ7UUFDRSxJQUFJLENBQUMsbUJBQW1CLEVBQUUsQ0FBQztRQUMzQixJQUFJLENBQUMscUJBQXFCLEVBQUUsQ0FBQztJQUMvQixDQUFDOzs7OztJQUVELG1EQUFXOzs7O0lBQVgsVUFBWSxFQUEwQjtZQUF4QixvQkFBTztRQUNuQixJQUFJLENBQUMsT0FBTztZQUFFLE9BQU87UUFFckIsSUFBSSxPQUFPLENBQUMsWUFBWSxFQUFFO1lBQ3hCLElBQUksQ0FBQyxTQUFTLEVBQUUsQ0FBQztTQUNsQjthQUFNLElBQUksT0FBTyxDQUFDLFlBQVksS0FBSyxLQUFLLElBQUksSUFBSSxDQUFDLE9BQU8sRUFBRTtZQUN6RCxJQUFJLENBQUMsT0FBTyxHQUFHLEtBQUssQ0FBQztTQUN0QjtJQUNILENBQUM7O2dCQXRQRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLDJCQUEyQjtvQkFDckMsZzhIQUFxRDtpQkFDdEQ7Ozs7Z0JBZGdCLEtBQUs7Z0JBSnBCLFNBQVM7OzsrQkFvQlIsS0FBSzs4QkFHTCxLQUFLOzZCQUdMLEtBQUs7MEJBS0wsS0FBSztnQ0FnQkwsTUFBTTs7SUFHUDtRQURDLE1BQU0sQ0FBQyx5QkFBeUIsQ0FBQyxtQkFBbUIsQ0FBQzswQ0FDN0MsVUFBVTtrRUFBK0I7SUFHbEQ7UUFEQyxNQUFNLENBQUMseUJBQXlCLENBQUMsb0JBQW9CLENBQUM7MENBQzFDLFVBQVU7c0VBQVM7SUFpTmxDLG9DQUFDO0NBQUEsQUF2UEQsSUF1UEM7U0FuUFksNkJBQTZCOzs7SUFDeEMscURBQ3FCOztJQUVyQixvREFDb0I7O0lBRXBCLG1EQUNtQjs7Ozs7SUFFbkIsaURBQW1COztJQWtCbkIsc0RBQStEOztJQUUvRCxnREFDa0Q7O0lBRWxELG9EQUNnQzs7SUFFaEMsc0RBQTBDOztJQUUxQyxvREFBb0Q7O0lBRXBELHNEQUFzQjs7SUFFdEIscURBQXFCOztJQUVyQixrREFBa0I7O0lBRWxCLGtEQUFnRjs7Ozs7SUFzQnBFLDhDQUFvQjs7Ozs7SUFBRSxpREFBMkI7Ozs7Ozs7QUFpTC9ELFNBQVMsVUFBVSxDQUNqQixXQUE4QyxFQUM5QyxVQUEyQzs7UUFFckMsZ0JBQWdCLEdBQUcsV0FBVyxDQUFDLElBQUk7Ozs7SUFBQyxVQUFBLEdBQUcsSUFBSSxPQUFBLEdBQUcsQ0FBQyxJQUFJLEtBQUssVUFBVSxDQUFDLFVBQVUsRUFBbEMsQ0FBa0MsRUFBQztJQUVwRixJQUFJLGdCQUFnQixJQUFJLGdCQUFnQixDQUFDLFVBQVUsRUFBRTs7WUFDL0MsTUFBTSxHQUFHLEVBQUU7UUFDZixPQUFPLENBQUMsTUFBTSxJQUFJLFVBQVUsQ0FBQyxXQUFXLEVBQUUsZ0JBQWdCLENBQUMsQ0FBQyxDQUFDO0tBQzlEO0lBRUQsT0FBTyxnQkFBZ0IsQ0FBQyxDQUFDLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7QUFDbkMsQ0FBQzs7Ozs7QUFFRCxTQUFTLGNBQWMsQ0FBQyxNQUFvQztJQUMxRCxPQUFPLE1BQU0sQ0FBQyxNQUFNOzs7OztJQUFDLFVBQUMsR0FBRyxFQUFFLEdBQUcsSUFBSyx3QkFBSSxHQUFHLEVBQUssR0FBRyxDQUFDLFdBQVcsR0FBM0IsQ0FBNEIsR0FBRSxFQUFFLENBQUMsQ0FBQztBQUN2RSxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcbiAgQ29tcG9uZW50LFxuICBFdmVudEVtaXR0ZXIsXG4gIElucHV0LFxuICBPbkNoYW5nZXMsXG4gIE9uSW5pdCxcbiAgT3V0cHV0LFxuICBSZW5kZXJlcjIsXG4gIFNpbXBsZUNoYW5nZXMsXG4gIFRyYWNrQnlGdW5jdGlvbixcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgZnJvbSwgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgbWFwLCBwbHVjaywgdGFrZSwgZmluYWxpemUgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgeyBHZXRQZXJtaXNzaW9ucywgVXBkYXRlUGVybWlzc2lvbnMgfSBmcm9tICcuLi9hY3Rpb25zL3Blcm1pc3Npb24tbWFuYWdlbWVudC5hY3Rpb25zJztcbmltcG9ydCB7IFBlcm1pc3Npb25NYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3Blcm1pc3Npb24tbWFuYWdlbWVudCc7XG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL3Blcm1pc3Npb24tbWFuYWdlbWVudC5zdGF0ZSc7XG5cbnR5cGUgUGVybWlzc2lvbldpdGhNYXJnaW4gPSBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uICYge1xuICBtYXJnaW46IG51bWJlcjtcbn07XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1wZXJtaXNzaW9uLW1hbmFnZW1lbnQnLFxuICB0ZW1wbGF0ZVVybDogJy4vcGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgUGVybWlzc2lvbk1hbmFnZW1lbnRDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQsIE9uQ2hhbmdlcyB7XG4gIEBJbnB1dCgpXG4gIHByb3ZpZGVyTmFtZTogc3RyaW5nO1xuXG4gIEBJbnB1dCgpXG4gIHByb3ZpZGVyS2V5OiBzdHJpbmc7XG5cbiAgQElucHV0KClcbiAgaGlkZUJhZGdlcyA9IGZhbHNlO1xuXG4gIHByb3RlY3RlZCBfdmlzaWJsZTtcblxuICBASW5wdXQoKVxuICBnZXQgdmlzaWJsZSgpOiBib29sZWFuIHtcbiAgICByZXR1cm4gdGhpcy5fdmlzaWJsZTtcbiAgfVxuXG4gIHNldCB2aXNpYmxlKHZhbHVlOiBib29sZWFuKSB7XG4gICAgaWYgKCF0aGlzLnNlbGVjdGVkR3JvdXApIHJldHVybjtcblxuICAgIHRoaXMuX3Zpc2libGUgPSB2YWx1ZTtcbiAgICB0aGlzLnZpc2libGVDaGFuZ2UuZW1pdCh2YWx1ZSk7XG5cbiAgICBpZiAoIXZhbHVlKSB7XG4gICAgICB0aGlzLnNlbGVjdGVkR3JvdXAgPSBudWxsO1xuICAgIH1cbiAgfVxuXG4gIEBPdXRwdXQoKSByZWFkb25seSB2aXNpYmxlQ2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxib29sZWFuPigpO1xuXG4gIEBTZWxlY3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRQZXJtaXNzaW9uR3JvdXBzKVxuICBncm91cHMkOiBPYnNlcnZhYmxlPFBlcm1pc3Npb25NYW5hZ2VtZW50Lkdyb3VwW10+O1xuXG4gIEBTZWxlY3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRFbnRpdHlEaXNwbGF5TmFtZSlcbiAgZW50aXR5TmFtZSQ6IE9ic2VydmFibGU8c3RyaW5nPjtcblxuICBzZWxlY3RlZEdyb3VwOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5Hcm91cDtcblxuICBwZXJtaXNzaW9uczogUGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbltdID0gW107XG5cbiAgc2VsZWN0VGhpc1RhYiA9IGZhbHNlO1xuXG4gIHNlbGVjdEFsbFRhYiA9IGZhbHNlO1xuXG4gIG1vZGFsQnVzeSA9IGZhbHNlO1xuXG4gIHRyYWNrQnlGbjogVHJhY2tCeUZ1bmN0aW9uPFBlcm1pc3Npb25NYW5hZ2VtZW50Lkdyb3VwPiA9IChfLCBpdGVtKSA9PiBpdGVtLm5hbWU7XG5cbiAgZ2V0IHNlbGVjdGVkR3JvdXBQZXJtaXNzaW9ucyQoKTogT2JzZXJ2YWJsZTxQZXJtaXNzaW9uV2l0aE1hcmdpbltdPiB7XG4gICAgcmV0dXJuIHRoaXMuZ3JvdXBzJC5waXBlKFxuICAgICAgbWFwKGdyb3VwcyA9PlxuICAgICAgICB0aGlzLnNlbGVjdGVkR3JvdXBcbiAgICAgICAgICA/IGdyb3Vwcy5maW5kKGdyb3VwID0+IGdyb3VwLm5hbWUgPT09IHRoaXMuc2VsZWN0ZWRHcm91cC5uYW1lKS5wZXJtaXNzaW9uc1xuICAgICAgICAgIDogW10sXG4gICAgICApLFxuICAgICAgbWFwPFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb25bXSwgUGVybWlzc2lvbldpdGhNYXJnaW5bXT4ocGVybWlzc2lvbnMgPT5cbiAgICAgICAgcGVybWlzc2lvbnMubWFwKFxuICAgICAgICAgIHBlcm1pc3Npb24gPT5cbiAgICAgICAgICAgICgoe1xuICAgICAgICAgICAgICAuLi5wZXJtaXNzaW9uLFxuICAgICAgICAgICAgICBtYXJnaW46IGZpbmRNYXJnaW4ocGVybWlzc2lvbnMsIHBlcm1pc3Npb24pLFxuICAgICAgICAgICAgICBpc0dyYW50ZWQ6IHRoaXMucGVybWlzc2lvbnMuZmluZChwZXIgPT4gcGVyLm5hbWUgPT09IHBlcm1pc3Npb24ubmFtZSkuaXNHcmFudGVkLFxuICAgICAgICAgICAgfSBhcyBhbnkpIGFzIFBlcm1pc3Npb25XaXRoTWFyZ2luKSxcbiAgICAgICAgKSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlLCBwcml2YXRlIHJlbmRlcmVyOiBSZW5kZXJlcjIpIHt9XG5cbiAgbmdPbkluaXQoKTogdm9pZCB7fVxuXG4gIGdldENoZWNrZWQobmFtZTogc3RyaW5nKSB7XG4gICAgcmV0dXJuICh0aGlzLnBlcm1pc3Npb25zLmZpbmQocGVyID0+IHBlci5uYW1lID09PSBuYW1lKSB8fCB7IGlzR3JhbnRlZDogZmFsc2UgfSkuaXNHcmFudGVkO1xuICB9XG5cbiAgaXNHcmFudGVkQnlPdGhlclByb3ZpZGVyTmFtZShncmFudGVkUHJvdmlkZXJzOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5HcmFudGVkUHJvdmlkZXJbXSk6IGJvb2xlYW4ge1xuICAgIGlmIChncmFudGVkUHJvdmlkZXJzLmxlbmd0aCkge1xuICAgICAgcmV0dXJuIGdyYW50ZWRQcm92aWRlcnMuZmluZEluZGV4KHAgPT4gcC5wcm92aWRlck5hbWUgIT09IHRoaXMucHJvdmlkZXJOYW1lKSA+IC0xO1xuICAgIH1cbiAgICByZXR1cm4gZmFsc2U7XG4gIH1cblxuICBvbkNsaWNrQ2hlY2tib3goY2xpY2tlZFBlcm1pc3Npb246IFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb24sIHZhbHVlKSB7XG4gICAgaWYgKFxuICAgICAgY2xpY2tlZFBlcm1pc3Npb24uaXNHcmFudGVkICYmXG4gICAgICB0aGlzLmlzR3JhbnRlZEJ5T3RoZXJQcm92aWRlck5hbWUoY2xpY2tlZFBlcm1pc3Npb24uZ3JhbnRlZFByb3ZpZGVycylcbiAgICApXG4gICAgICByZXR1cm47XG5cbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgIHRoaXMucGVybWlzc2lvbnMgPSB0aGlzLnBlcm1pc3Npb25zLm1hcChwZXIgPT4ge1xuICAgICAgICBpZiAoY2xpY2tlZFBlcm1pc3Npb24ubmFtZSA9PT0gcGVyLm5hbWUpIHtcbiAgICAgICAgICByZXR1cm4geyAuLi5wZXIsIGlzR3JhbnRlZDogIXBlci5pc0dyYW50ZWQgfTtcbiAgICAgICAgfSBlbHNlIGlmIChjbGlja2VkUGVybWlzc2lvbi5uYW1lID09PSBwZXIucGFyZW50TmFtZSAmJiBjbGlja2VkUGVybWlzc2lvbi5pc0dyYW50ZWQpIHtcbiAgICAgICAgICByZXR1cm4geyAuLi5wZXIsIGlzR3JhbnRlZDogZmFsc2UgfTtcbiAgICAgICAgfSBlbHNlIGlmIChjbGlja2VkUGVybWlzc2lvbi5wYXJlbnROYW1lID09PSBwZXIubmFtZSAmJiAhY2xpY2tlZFBlcm1pc3Npb24uaXNHcmFudGVkKSB7XG4gICAgICAgICAgcmV0dXJuIHsgLi4ucGVyLCBpc0dyYW50ZWQ6IHRydWUgfTtcbiAgICAgICAgfVxuXG4gICAgICAgIHJldHVybiBwZXI7XG4gICAgICB9KTtcblxuICAgICAgdGhpcy5zZXRUYWJDaGVja2JveFN0YXRlKCk7XG4gICAgICB0aGlzLnNldEdyYW50Q2hlY2tib3hTdGF0ZSgpO1xuICAgIH0sIDApO1xuICB9XG5cbiAgc2V0VGFiQ2hlY2tib3hTdGF0ZSgpIHtcbiAgICB0aGlzLnNlbGVjdGVkR3JvdXBQZXJtaXNzaW9ucyQucGlwZSh0YWtlKDEpKS5zdWJzY3JpYmUocGVybWlzc2lvbnMgPT4ge1xuICAgICAgY29uc3Qgc2VsZWN0ZWRQZXJtaXNzaW9ucyA9IHBlcm1pc3Npb25zLmZpbHRlcihwZXIgPT4gcGVyLmlzR3JhbnRlZCk7XG4gICAgICBjb25zdCBlbGVtZW50ID0gZG9jdW1lbnQucXVlcnlTZWxlY3RvcignI3NlbGVjdC1hbGwtaW4tdGhpcy10YWJzJykgYXMgYW55O1xuXG4gICAgICBpZiAoc2VsZWN0ZWRQZXJtaXNzaW9ucy5sZW5ndGggPT09IHBlcm1pc3Npb25zLmxlbmd0aCkge1xuICAgICAgICBlbGVtZW50LmluZGV0ZXJtaW5hdGUgPSBmYWxzZTtcbiAgICAgICAgdGhpcy5zZWxlY3RUaGlzVGFiID0gdHJ1ZTtcbiAgICAgIH0gZWxzZSBpZiAoc2VsZWN0ZWRQZXJtaXNzaW9ucy5sZW5ndGggPT09IDApIHtcbiAgICAgICAgZWxlbWVudC5pbmRldGVybWluYXRlID0gZmFsc2U7XG4gICAgICAgIHRoaXMuc2VsZWN0VGhpc1RhYiA9IGZhbHNlO1xuICAgICAgfSBlbHNlIHtcbiAgICAgICAgZWxlbWVudC5pbmRldGVybWluYXRlID0gdHJ1ZTtcbiAgICAgIH1cbiAgICB9KTtcbiAgfVxuXG4gIHNldEdyYW50Q2hlY2tib3hTdGF0ZSgpIHtcbiAgICBjb25zdCBzZWxlY3RlZEFsbFBlcm1pc3Npb25zID0gdGhpcy5wZXJtaXNzaW9ucy5maWx0ZXIocGVyID0+IHBlci5pc0dyYW50ZWQpO1xuICAgIGNvbnN0IGNoZWNrYm94RWxlbWVudCA9IGRvY3VtZW50LnF1ZXJ5U2VsZWN0b3IoJyNzZWxlY3QtYWxsLWluLWFsbC10YWJzJykgYXMgYW55O1xuXG4gICAgaWYgKHNlbGVjdGVkQWxsUGVybWlzc2lvbnMubGVuZ3RoID09PSB0aGlzLnBlcm1pc3Npb25zLmxlbmd0aCkge1xuICAgICAgY2hlY2tib3hFbGVtZW50LmluZGV0ZXJtaW5hdGUgPSBmYWxzZTtcbiAgICAgIHRoaXMuc2VsZWN0QWxsVGFiID0gdHJ1ZTtcbiAgICB9IGVsc2UgaWYgKHNlbGVjdGVkQWxsUGVybWlzc2lvbnMubGVuZ3RoID09PSAwKSB7XG4gICAgICBjaGVja2JveEVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IGZhbHNlO1xuICAgICAgdGhpcy5zZWxlY3RBbGxUYWIgPSBmYWxzZTtcbiAgICB9IGVsc2Uge1xuICAgICAgY2hlY2tib3hFbGVtZW50LmluZGV0ZXJtaW5hdGUgPSB0cnVlO1xuICAgIH1cbiAgfVxuXG4gIG9uQ2xpY2tTZWxlY3RUaGlzVGFiKCkge1xuICAgIHRoaXMuc2VsZWN0ZWRHcm91cFBlcm1pc3Npb25zJC5waXBlKHRha2UoMSkpLnN1YnNjcmliZShwZXJtaXNzaW9ucyA9PiB7XG4gICAgICBwZXJtaXNzaW9ucy5mb3JFYWNoKHBlcm1pc3Npb24gPT4ge1xuICAgICAgICBpZiAocGVybWlzc2lvbi5pc0dyYW50ZWQgJiYgdGhpcy5pc0dyYW50ZWRCeU90aGVyUHJvdmlkZXJOYW1lKHBlcm1pc3Npb24uZ3JhbnRlZFByb3ZpZGVycykpXG4gICAgICAgICAgcmV0dXJuO1xuXG4gICAgICAgIGNvbnN0IGluZGV4ID0gdGhpcy5wZXJtaXNzaW9ucy5maW5kSW5kZXgocGVyID0+IHBlci5uYW1lID09PSBwZXJtaXNzaW9uLm5hbWUpO1xuXG4gICAgICAgIHRoaXMucGVybWlzc2lvbnMgPSBbXG4gICAgICAgICAgLi4udGhpcy5wZXJtaXNzaW9ucy5zbGljZSgwLCBpbmRleCksXG4gICAgICAgICAgeyAuLi50aGlzLnBlcm1pc3Npb25zW2luZGV4XSwgaXNHcmFudGVkOiAhdGhpcy5zZWxlY3RUaGlzVGFiIH0sXG4gICAgICAgICAgLi4udGhpcy5wZXJtaXNzaW9ucy5zbGljZShpbmRleCArIDEpLFxuICAgICAgICBdO1xuICAgICAgfSk7XG4gICAgfSk7XG5cbiAgICB0aGlzLnNldEdyYW50Q2hlY2tib3hTdGF0ZSgpO1xuICB9XG5cbiAgb25DbGlja1NlbGVjdEFsbCgpIHtcbiAgICB0aGlzLnBlcm1pc3Npb25zID0gdGhpcy5wZXJtaXNzaW9ucy5tYXAocGVybWlzc2lvbiA9PiAoe1xuICAgICAgLi4ucGVybWlzc2lvbixcbiAgICAgIGlzR3JhbnRlZDpcbiAgICAgICAgdGhpcy5pc0dyYW50ZWRCeU90aGVyUHJvdmlkZXJOYW1lKHBlcm1pc3Npb24uZ3JhbnRlZFByb3ZpZGVycykgfHwgIXRoaXMuc2VsZWN0QWxsVGFiLFxuICAgIH0pKTtcblxuICAgIHRoaXMuc2VsZWN0VGhpc1RhYiA9ICF0aGlzLnNlbGVjdEFsbFRhYjtcbiAgfVxuXG4gIG9uQ2hhbmdlR3JvdXAoZ3JvdXA6IFBlcm1pc3Npb25NYW5hZ2VtZW50Lkdyb3VwKSB7XG4gICAgdGhpcy5zZWxlY3RlZEdyb3VwID0gZ3JvdXA7XG4gICAgdGhpcy5zZXRUYWJDaGVja2JveFN0YXRlKCk7XG4gIH1cblxuICBzdWJtaXQoKSB7XG4gICAgdGhpcy5tb2RhbEJ1c3kgPSB0cnVlO1xuICAgIGNvbnN0IHVuY2hhbmdlZFBlcm1pc3Npb25zID0gZ2V0UGVybWlzc2lvbnMoXG4gICAgICB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUuZ2V0UGVybWlzc2lvbkdyb3VwcyksXG4gICAgKTtcblxuICAgIGNvbnN0IGNoYW5nZWRQZXJtaXNzaW9uczogUGVybWlzc2lvbk1hbmFnZW1lbnQuTWluaW11bVBlcm1pc3Npb25bXSA9IHRoaXMucGVybWlzc2lvbnNcbiAgICAgIC5maWx0ZXIocGVyID0+XG4gICAgICAgIHVuY2hhbmdlZFBlcm1pc3Npb25zLmZpbmQodW5jaGFuZ2VkID0+IHVuY2hhbmdlZC5uYW1lID09PSBwZXIubmFtZSkuaXNHcmFudGVkID09PVxuICAgICAgICBwZXIuaXNHcmFudGVkXG4gICAgICAgICAgPyBmYWxzZVxuICAgICAgICAgIDogdHJ1ZSxcbiAgICAgIClcbiAgICAgIC5tYXAoKHsgbmFtZSwgaXNHcmFudGVkIH0pID0+ICh7IG5hbWUsIGlzR3JhbnRlZCB9KSk7XG5cbiAgICBpZiAoY2hhbmdlZFBlcm1pc3Npb25zLmxlbmd0aCkge1xuICAgICAgdGhpcy5zdG9yZVxuICAgICAgICAuZGlzcGF0Y2goXG4gICAgICAgICAgbmV3IFVwZGF0ZVBlcm1pc3Npb25zKHtcbiAgICAgICAgICAgIHByb3ZpZGVyS2V5OiB0aGlzLnByb3ZpZGVyS2V5LFxuICAgICAgICAgICAgcHJvdmlkZXJOYW1lOiB0aGlzLnByb3ZpZGVyTmFtZSxcbiAgICAgICAgICAgIHBlcm1pc3Npb25zOiBjaGFuZ2VkUGVybWlzc2lvbnMsXG4gICAgICAgICAgfSksXG4gICAgICAgIClcbiAgICAgICAgLnBpcGUoZmluYWxpemUoKCkgPT4gKHRoaXMubW9kYWxCdXN5ID0gZmFsc2UpKSlcbiAgICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XG4gICAgICAgIH0pO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aGlzLm1vZGFsQnVzeSA9IGZhbHNlO1xuICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XG4gICAgfVxuICB9XG5cbiAgb3Blbk1vZGFsKCkge1xuICAgIGlmICghdGhpcy5wcm92aWRlcktleSB8fCAhdGhpcy5wcm92aWRlck5hbWUpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcignUHJvdmlkZXIgS2V5IGFuZCBQcm92aWRlciBOYW1lIGFyZSByZXF1aXJlZC4nKTtcbiAgICB9XG5cbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2goXG4gICAgICAgIG5ldyBHZXRQZXJtaXNzaW9ucyh7XG4gICAgICAgICAgcHJvdmlkZXJLZXk6IHRoaXMucHJvdmlkZXJLZXksXG4gICAgICAgICAgcHJvdmlkZXJOYW1lOiB0aGlzLnByb3ZpZGVyTmFtZSxcbiAgICAgICAgfSksXG4gICAgICApXG4gICAgICAucGlwZShwbHVjaygnUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZScsICdwZXJtaXNzaW9uUmVzJykpXG4gICAgICAuc3Vic2NyaWJlKChwZXJtaXNzaW9uUmVzOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5SZXNwb25zZSkgPT4ge1xuICAgICAgICB0aGlzLnNlbGVjdGVkR3JvdXAgPSBwZXJtaXNzaW9uUmVzLmdyb3Vwc1swXTtcbiAgICAgICAgdGhpcy5wZXJtaXNzaW9ucyA9IGdldFBlcm1pc3Npb25zKHBlcm1pc3Npb25SZXMuZ3JvdXBzKTtcblxuICAgICAgICB0aGlzLnZpc2libGUgPSB0cnVlO1xuICAgICAgfSk7XG4gIH1cblxuICBpbml0TW9kYWwoKSB7XG4gICAgdGhpcy5zZXRUYWJDaGVja2JveFN0YXRlKCk7XG4gICAgdGhpcy5zZXRHcmFudENoZWNrYm94U3RhdGUoKTtcbiAgfVxuXG4gIG5nT25DaGFuZ2VzKHsgdmlzaWJsZSB9OiBTaW1wbGVDaGFuZ2VzKTogdm9pZCB7XG4gICAgaWYgKCF2aXNpYmxlKSByZXR1cm47XG5cbiAgICBpZiAodmlzaWJsZS5jdXJyZW50VmFsdWUpIHtcbiAgICAgIHRoaXMub3Blbk1vZGFsKCk7XG4gICAgfSBlbHNlIGlmICh2aXNpYmxlLmN1cnJlbnRWYWx1ZSA9PT0gZmFsc2UgJiYgdGhpcy52aXNpYmxlKSB7XG4gICAgICB0aGlzLnZpc2libGUgPSBmYWxzZTtcbiAgICB9XG4gIH1cbn1cblxuZnVuY3Rpb24gZmluZE1hcmdpbihcbiAgcGVybWlzc2lvbnM6IFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb25bXSxcbiAgcGVybWlzc2lvbjogUGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbixcbikge1xuICBjb25zdCBwYXJlbnRQZXJtaXNzaW9uID0gcGVybWlzc2lvbnMuZmluZChwZXIgPT4gcGVyLm5hbWUgPT09IHBlcm1pc3Npb24ucGFyZW50TmFtZSk7XG5cbiAgaWYgKHBhcmVudFBlcm1pc3Npb24gJiYgcGFyZW50UGVybWlzc2lvbi5wYXJlbnROYW1lKSB7XG4gICAgbGV0IG1hcmdpbiA9IDIwO1xuICAgIHJldHVybiAobWFyZ2luICs9IGZpbmRNYXJnaW4ocGVybWlzc2lvbnMsIHBhcmVudFBlcm1pc3Npb24pKTtcbiAgfVxuXG4gIHJldHVybiBwYXJlbnRQZXJtaXNzaW9uID8gMjAgOiAwO1xufVxuXG5mdW5jdGlvbiBnZXRQZXJtaXNzaW9ucyhncm91cHM6IFBlcm1pc3Npb25NYW5hZ2VtZW50Lkdyb3VwW10pOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uW10ge1xuICByZXR1cm4gZ3JvdXBzLnJlZHVjZSgoYWNjLCB2YWwpID0+IFsuLi5hY2MsIC4uLnZhbC5wZXJtaXNzaW9uc10sIFtdKTtcbn1cbiJdfQ==