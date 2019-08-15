/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Component, EventEmitter, Input, Output, Renderer2, } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { map, pluck, take } from 'rxjs/operators';
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
        if (clickedPermission.isGranted && clickedPermission.grantedProviders.length > 0)
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
                if (permission.isGranted && permission.grantedProviders.length > 0)
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
        function (permission) { return (tslib_1.__assign({}, permission, { isGranted: !_this.selectAllTab })); }));
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
    PermissionManagementComponent.prototype.onSubmit = /**
     * @return {?}
     */
    function () {
        var _this = this;
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
                .subscribe((/**
             * @return {?}
             */
            function () {
                _this.visible = false;
            }));
        }
        else {
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
            .dispatch(new GetPermissions({ providerKey: this.providerKey, providerName: this.providerName }))
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
                    template: "<abp-modal [(visible)]=\"visible\" (init)=\"initModal()\">\n  <ng-container *ngIf=\"{ entityName: entityName$ | async } as data\">\n    <ng-template #abpHeader>\n      <h4>{{ 'AbpPermissionManagement::Permissions' | abpLocalization }} - {{ data.entityName }}</h4>\n    </ng-template>\n    <ng-template #abpBody>\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input\n          type=\"checkbox\"\n          id=\"select-all-in-all-tabs\"\n          name=\"select-all-in-all-tabs\"\n          class=\"custom-control-input\"\n          [(ngModel)]=\"selectAllTab\"\n          (click)=\"onClickSelectAll()\"\n        />\n        <label class=\"custom-control-label\" for=\"select-all-in-all-tabs\">{{\n          'AbpPermissionManagement::SelectAllInAllTabs' | abpLocalization\n        }}</label>\n      </div>\n\n      <hr class=\"mt-2 mb-2\" />\n      <div class=\"row\">\n        <div class=\"col-4\">\n          <ul class=\"nav nav-pills flex-column\">\n            <perfect-scrollbar class=\"ps-show-always\" style=\"max-height: 70vh;\">\n              <li *ngFor=\"let group of groups$ | async; trackBy: trackByFn\" class=\"nav-item\">\n                <a\n                  class=\"nav-link pointer\"\n                  [class.active]=\"selectedGroup.name === group.name\"\n                  (click)=\"onChangeGroup(group)\"\n                  >{{ group?.displayName }}</a\n                >\n              </li>\n            </perfect-scrollbar>\n          </ul>\n        </div>\n        <div class=\"col-8\">\n          <h4>{{ selectedGroup.displayName }}</h4>\n          <hr class=\"mt-2 mb-3\" />\n          <div class=\"pl-1 pt-1\">\n            <div class=\"custom-checkbox custom-control mb-2\">\n              <input\n                type=\"checkbox\"\n                id=\"select-all-in-this-tabs\"\n                name=\"select-all-in-this-tabs\"\n                class=\"custom-control-input\"\n                [(ngModel)]=\"selectThisTab\"\n                (click)=\"onClickSelectThisTab()\"\n              />\n              <label class=\"custom-control-label\" for=\"select-all-in-this-tabs\">{{\n                'AbpPermissionManagement::SelectAllInThisTab' | abpLocalization\n              }}</label>\n            </div>\n            <hr class=\"mb-3\" />\n            <perfect-scrollbar class=\"ps-show-always\" style=\"max-height: 60vh;\">\n              <div\n                *ngFor=\"let permission of selectedGroupPermissions$ | async; let i = index; trackBy: trackByFn\"\n                [style.margin-left]=\"permission.margin + 'px'\"\n                class=\"custom-checkbox custom-control mb-2\"\n              >\n                <input\n                  #permissionCheckbox\n                  type=\"checkbox\"\n                  [checked]=\"getChecked(permission.name)\"\n                  [value]=\"getChecked(permission.name)\"\n                  [attr.id]=\"permission.name\"\n                  class=\"custom-control-input\"\n                  [disabled]=\"permission.isGranted && permission.grantedProviders.length > 0\"\n                />\n                <label\n                  class=\"custom-control-label\"\n                  [attr.for]=\"permission.name\"\n                  (click)=\"onClickCheckbox(permission, permissionCheckbox.value)\"\n                  >{{ permission.displayName }}\n                  <span *ngFor=\"let provider of permission.grantedProviders\" class=\"badge badge-light\"\n                    >{{ provider.providerName }}: {{ provider.providerKey }}</span\n                  ></label\n                >\n              </div>\n            </perfect-scrollbar>\n          </div>\n        </div>\n      </div>\n    </ng-template>\n    <ng-template #abpFooter>\n      <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\n      </button>\n      <abp-button\n        [requestType]=\"['PUT']\"\n        requestURLContainSearchValue=\"permissions\"\n        iconClass=\"fa fa-check\"\n        (click)=\"onSubmit()\"\n        >{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button\n      >\n    </ng-template>\n  </ng-container>\n</abp-modal>\n"
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
        Select(PermissionManagementState.getEntitiyDisplayName),
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcucGVybWlzc2lvbi1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFDTCxTQUFTLEVBQ1QsWUFBWSxFQUNaLEtBQUssRUFHTCxNQUFNLEVBQ04sU0FBUyxHQUdWLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBUSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDeEMsT0FBTyxFQUFFLEdBQUcsRUFBRSxLQUFLLEVBQUUsSUFBSSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDbEQsT0FBTyxFQUFFLGNBQWMsRUFBRSxpQkFBaUIsRUFBRSxNQUFNLDBDQUEwQyxDQUFDO0FBRTdGLE9BQU8sRUFBRSx5QkFBeUIsRUFBRSxNQUFNLHVDQUF1QyxDQUFDO0FBSWxGO0lBa0VFLHVDQUFvQixLQUFZLEVBQVUsUUFBbUI7UUFBekMsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUFVLGFBQVEsR0FBUixRQUFRLENBQVc7UUFwQzdELGtCQUFhLEdBQUcsSUFBSSxZQUFZLEVBQVcsQ0FBQztRQVU1QyxnQkFBVyxHQUFzQyxFQUFFLENBQUM7UUFFcEQsa0JBQWEsR0FBWSxLQUFLLENBQUM7UUFFL0IsaUJBQVksR0FBWSxLQUFLLENBQUM7UUFFOUIsY0FBUzs7Ozs7UUFBZ0QsVUFBQyxDQUFDLEVBQUUsSUFBSSxJQUFLLE9BQUEsSUFBSSxDQUFDLElBQUksRUFBVCxDQUFTLEVBQUM7SUFvQmhCLENBQUM7SUFyRGpFLHNCQUNJLGtEQUFPOzs7O1FBRFg7WUFFRSxPQUFPLElBQUksQ0FBQyxRQUFRLENBQUM7UUFDdkIsQ0FB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RSxVQUEyQzs7UUFDdkcsZ0JBQWdCLEdBQUcsV0FBVyxDQUFDLElBQUk7Ozs7SUFBQyxVQUFBLEdBQUcsSUFBSSxPQUFBLEdBQUcsQ0FBQyxJQUFJLEtBQUssVUFBVSxDQUFDLFVBQVUsRUFBbEMsQ0FBa0MsRUFBQztJQUVwRixJQUFJLGdCQUFnQixJQUFJLGdCQUFnQixDQUFDLFVBQVUsRUFBRTs7WUFDL0MsTUFBTSxHQUFHLEVBQUU7UUFDZixPQUFPLENBQUMsTUFBTSxJQUFJLFVBQVUsQ0FBQyxXQUFXLEVBQUUsZ0JBQWdCLENBQUMsQ0FBQyxDQUFDO0tBQzlEO0lBRUQsT0FBTyxnQkFBZ0IsQ0FBQyxDQUFDLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7QUFDbkMsQ0FBQzs7Ozs7QUFFRCxTQUFTLGNBQWMsQ0FBQyxNQUFvQztJQUMxRCxPQUFPLE1BQU0sQ0FBQyxNQUFNOzs7OztJQUFDLFVBQUMsR0FBRyxFQUFFLEdBQUcsSUFBSyx3QkFBSSxHQUFHLEVBQUssR0FBRyxDQUFDLFdBQVcsR0FBM0IsQ0FBNEIsR0FBRSxFQUFFLENBQUMsQ0FBQztBQUN2RSxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcbiAgQ29tcG9uZW50LFxuICBFdmVudEVtaXR0ZXIsXG4gIElucHV0LFxuICBPbkNoYW5nZXMsXG4gIE9uSW5pdCxcbiAgT3V0cHV0LFxuICBSZW5kZXJlcjIsXG4gIFNpbXBsZUNoYW5nZXMsXG4gIFRyYWNrQnlGdW5jdGlvbixcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgZnJvbSwgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgbWFwLCBwbHVjaywgdGFrZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7IEdldFBlcm1pc3Npb25zLCBVcGRhdGVQZXJtaXNzaW9ucyB9IGZyb20gJy4uL2FjdGlvbnMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LmFjdGlvbnMnO1xuaW1wb3J0IHsgUGVybWlzc2lvbk1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvcGVybWlzc2lvbi1tYW5hZ2VtZW50JztcbmltcG9ydCB7IFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LnN0YXRlJztcblxudHlwZSBQZXJtaXNzaW9uV2l0aE1hcmdpbiA9IFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb24gJiB7IG1hcmdpbjogbnVtYmVyIH07XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1wZXJtaXNzaW9uLW1hbmFnZW1lbnQnLFxuICB0ZW1wbGF0ZVVybDogJy4vcGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgUGVybWlzc2lvbk1hbmFnZW1lbnRDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQsIE9uQ2hhbmdlcyB7XG4gIEBJbnB1dCgpXG4gIHByb3ZpZGVyTmFtZTogc3RyaW5nO1xuXG4gIEBJbnB1dCgpXG4gIHByb3ZpZGVyS2V5OiBzdHJpbmc7XG5cbiAgcHJvdGVjdGVkIF92aXNpYmxlO1xuXG4gIEBJbnB1dCgpXG4gIGdldCB2aXNpYmxlKCk6IGJvb2xlYW4ge1xuICAgIHJldHVybiB0aGlzLl92aXNpYmxlO1xuICB9XG5cbiAgc2V0IHZpc2libGUodmFsdWU6IGJvb2xlYW4pIHtcbiAgICBpZiAoIXRoaXMuc2VsZWN0ZWRHcm91cCkgcmV0dXJuO1xuXG4gICAgdGhpcy5fdmlzaWJsZSA9IHZhbHVlO1xuICAgIHRoaXMudmlzaWJsZUNoYW5nZS5lbWl0KHZhbHVlKTtcblxuICAgIGlmICghdmFsdWUpIHtcbiAgICAgIHRoaXMuc2VsZWN0ZWRHcm91cCA9IG51bGw7XG4gICAgfVxuICB9XG5cbiAgQE91dHB1dCgpXG4gIHZpc2libGVDaGFuZ2UgPSBuZXcgRXZlbnRFbWl0dGVyPGJvb2xlYW4+KCk7XG5cbiAgQFNlbGVjdChQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlLmdldFBlcm1pc3Npb25Hcm91cHMpXG4gIGdyb3VwcyQ6IE9ic2VydmFibGU8UGVybWlzc2lvbk1hbmFnZW1lbnQuR3JvdXBbXT47XG5cbiAgQFNlbGVjdChQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlLmdldEVudGl0aXlEaXNwbGF5TmFtZSlcbiAgZW50aXR5TmFtZSQ6IE9ic2VydmFibGU8c3RyaW5nPjtcblxuICBzZWxlY3RlZEdyb3VwOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5Hcm91cDtcblxuICBwZXJtaXNzaW9uczogUGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbltdID0gW107XG5cbiAgc2VsZWN0VGhpc1RhYjogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIHNlbGVjdEFsbFRhYjogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIHRyYWNrQnlGbjogVHJhY2tCeUZ1bmN0aW9uPFBlcm1pc3Npb25NYW5hZ2VtZW50Lkdyb3VwPiA9IChfLCBpdGVtKSA9PiBpdGVtLm5hbWU7XG5cbiAgZ2V0IHNlbGVjdGVkR3JvdXBQZXJtaXNzaW9ucyQoKTogT2JzZXJ2YWJsZTxQZXJtaXNzaW9uV2l0aE1hcmdpbltdPiB7XG4gICAgcmV0dXJuIHRoaXMuZ3JvdXBzJC5waXBlKFxuICAgICAgbWFwKGdyb3VwcyA9PlxuICAgICAgICB0aGlzLnNlbGVjdGVkR3JvdXAgPyBncm91cHMuZmluZChncm91cCA9PiBncm91cC5uYW1lID09PSB0aGlzLnNlbGVjdGVkR3JvdXAubmFtZSkucGVybWlzc2lvbnMgOiBbXSxcbiAgICAgICksXG4gICAgICBtYXA8UGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbltdLCBQZXJtaXNzaW9uV2l0aE1hcmdpbltdPihwZXJtaXNzaW9ucyA9PlxuICAgICAgICBwZXJtaXNzaW9ucy5tYXAoXG4gICAgICAgICAgcGVybWlzc2lvbiA9PlxuICAgICAgICAgICAgKCh7XG4gICAgICAgICAgICAgIC4uLnBlcm1pc3Npb24sXG4gICAgICAgICAgICAgIG1hcmdpbjogZmluZE1hcmdpbihwZXJtaXNzaW9ucywgcGVybWlzc2lvbiksXG4gICAgICAgICAgICAgIGlzR3JhbnRlZDogdGhpcy5wZXJtaXNzaW9ucy5maW5kKHBlciA9PiBwZXIubmFtZSA9PT0gcGVybWlzc2lvbi5uYW1lKS5pc0dyYW50ZWQsXG4gICAgICAgICAgICB9IGFzIGFueSkgYXMgUGVybWlzc2lvbldpdGhNYXJnaW4pLFxuICAgICAgICApLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUsIHByaXZhdGUgcmVuZGVyZXI6IFJlbmRlcmVyMikge31cblxuICBuZ09uSW5pdCgpOiB2b2lkIHt9XG5cbiAgZ2V0Q2hlY2tlZChuYW1lOiBzdHJpbmcpIHtcbiAgICByZXR1cm4gKHRoaXMucGVybWlzc2lvbnMuZmluZChwZXIgPT4gcGVyLm5hbWUgPT09IG5hbWUpIHx8IHsgaXNHcmFudGVkOiBmYWxzZSB9KS5pc0dyYW50ZWQ7XG4gIH1cblxuICBvbkNsaWNrQ2hlY2tib3goY2xpY2tlZFBlcm1pc3Npb246IFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb24sIHZhbHVlKSB7XG4gICAgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLmlzR3JhbnRlZCAmJiBjbGlja2VkUGVybWlzc2lvbi5ncmFudGVkUHJvdmlkZXJzLmxlbmd0aCA+IDApIHJldHVybjtcblxuICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgdGhpcy5wZXJtaXNzaW9ucyA9IHRoaXMucGVybWlzc2lvbnMubWFwKHBlciA9PiB7XG4gICAgICAgIGlmIChjbGlja2VkUGVybWlzc2lvbi5uYW1lID09PSBwZXIubmFtZSkge1xuICAgICAgICAgIHJldHVybiB7IC4uLnBlciwgaXNHcmFudGVkOiAhcGVyLmlzR3JhbnRlZCB9O1xuICAgICAgICB9IGVsc2UgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLm5hbWUgPT09IHBlci5wYXJlbnROYW1lICYmIGNsaWNrZWRQZXJtaXNzaW9uLmlzR3JhbnRlZCkge1xuICAgICAgICAgIHJldHVybiB7IC4uLnBlciwgaXNHcmFudGVkOiBmYWxzZSB9O1xuICAgICAgICB9IGVsc2UgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLnBhcmVudE5hbWUgPT09IHBlci5uYW1lICYmICFjbGlja2VkUGVybWlzc2lvbi5pc0dyYW50ZWQpIHtcbiAgICAgICAgICByZXR1cm4geyAuLi5wZXIsIGlzR3JhbnRlZDogdHJ1ZSB9O1xuICAgICAgICB9XG5cbiAgICAgICAgcmV0dXJuIHBlcjtcbiAgICAgIH0pO1xuXG4gICAgICB0aGlzLnNldFRhYkNoZWNrYm94U3RhdGUoKTtcbiAgICAgIHRoaXMuc2V0R3JhbnRDaGVja2JveFN0YXRlKCk7XG4gICAgfSwgMCk7XG4gIH1cblxuICBzZXRUYWJDaGVja2JveFN0YXRlKCkge1xuICAgIHRoaXMuc2VsZWN0ZWRHcm91cFBlcm1pc3Npb25zJC5waXBlKHRha2UoMSkpLnN1YnNjcmliZShwZXJtaXNzaW9ucyA9PiB7XG4gICAgICBjb25zdCBzZWxlY3RlZFBlcm1pc3Npb25zID0gcGVybWlzc2lvbnMuZmlsdGVyKHBlciA9PiBwZXIuaXNHcmFudGVkKTtcbiAgICAgIGNvbnN0IGVsZW1lbnQgPSBkb2N1bWVudC5xdWVyeVNlbGVjdG9yKCcjc2VsZWN0LWFsbC1pbi10aGlzLXRhYnMnKSBhcyBhbnk7XG5cbiAgICAgIGlmIChzZWxlY3RlZFBlcm1pc3Npb25zLmxlbmd0aCA9PT0gcGVybWlzc2lvbnMubGVuZ3RoKSB7XG4gICAgICAgIGVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IGZhbHNlO1xuICAgICAgICB0aGlzLnNlbGVjdFRoaXNUYWIgPSB0cnVlO1xuICAgICAgfSBlbHNlIGlmIChzZWxlY3RlZFBlcm1pc3Npb25zLmxlbmd0aCA9PT0gMCkge1xuICAgICAgICBlbGVtZW50LmluZGV0ZXJtaW5hdGUgPSBmYWxzZTtcbiAgICAgICAgdGhpcy5zZWxlY3RUaGlzVGFiID0gZmFsc2U7XG4gICAgICB9IGVsc2Uge1xuICAgICAgICBlbGVtZW50LmluZGV0ZXJtaW5hdGUgPSB0cnVlO1xuICAgICAgfVxuICAgIH0pO1xuICB9XG5cbiAgc2V0R3JhbnRDaGVja2JveFN0YXRlKCkge1xuICAgIGNvbnN0IHNlbGVjdGVkQWxsUGVybWlzc2lvbnMgPSB0aGlzLnBlcm1pc3Npb25zLmZpbHRlcihwZXIgPT4gcGVyLmlzR3JhbnRlZCk7XG4gICAgY29uc3QgY2hlY2tib3hFbGVtZW50ID0gZG9jdW1lbnQucXVlcnlTZWxlY3RvcignI3NlbGVjdC1hbGwtaW4tYWxsLXRhYnMnKSBhcyBhbnk7XG5cbiAgICBpZiAoc2VsZWN0ZWRBbGxQZXJtaXNzaW9ucy5sZW5ndGggPT09IHRoaXMucGVybWlzc2lvbnMubGVuZ3RoKSB7XG4gICAgICBjaGVja2JveEVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IGZhbHNlO1xuICAgICAgdGhpcy5zZWxlY3RBbGxUYWIgPSB0cnVlO1xuICAgIH0gZWxzZSBpZiAoc2VsZWN0ZWRBbGxQZXJtaXNzaW9ucy5sZW5ndGggPT09IDApIHtcbiAgICAgIGNoZWNrYm94RWxlbWVudC5pbmRldGVybWluYXRlID0gZmFsc2U7XG4gICAgICB0aGlzLnNlbGVjdEFsbFRhYiA9IGZhbHNlO1xuICAgIH0gZWxzZSB7XG4gICAgICBjaGVja2JveEVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IHRydWU7XG4gICAgfVxuICB9XG5cbiAgb25DbGlja1NlbGVjdFRoaXNUYWIoKSB7XG4gICAgdGhpcy5zZWxlY3RlZEdyb3VwUGVybWlzc2lvbnMkLnBpcGUodGFrZSgxKSkuc3Vic2NyaWJlKHBlcm1pc3Npb25zID0+IHtcbiAgICAgIHBlcm1pc3Npb25zLmZvckVhY2gocGVybWlzc2lvbiA9PiB7XG4gICAgICAgIGlmIChwZXJtaXNzaW9uLmlzR3JhbnRlZCAmJiBwZXJtaXNzaW9uLmdyYW50ZWRQcm92aWRlcnMubGVuZ3RoID4gMCkgcmV0dXJuO1xuXG4gICAgICAgIGNvbnN0IGluZGV4ID0gdGhpcy5wZXJtaXNzaW9ucy5maW5kSW5kZXgocGVyID0+IHBlci5uYW1lID09PSBwZXJtaXNzaW9uLm5hbWUpO1xuXG4gICAgICAgIHRoaXMucGVybWlzc2lvbnMgPSBbXG4gICAgICAgICAgLi4udGhpcy5wZXJtaXNzaW9ucy5zbGljZSgwLCBpbmRleCksXG4gICAgICAgICAgeyAuLi50aGlzLnBlcm1pc3Npb25zW2luZGV4XSwgaXNHcmFudGVkOiAhdGhpcy5zZWxlY3RUaGlzVGFiIH0sXG4gICAgICAgICAgLi4udGhpcy5wZXJtaXNzaW9ucy5zbGljZShpbmRleCArIDEpLFxuICAgICAgICBdO1xuICAgICAgfSk7XG4gICAgfSk7XG5cbiAgICB0aGlzLnNldEdyYW50Q2hlY2tib3hTdGF0ZSgpO1xuICB9XG5cbiAgb25DbGlja1NlbGVjdEFsbCgpIHtcbiAgICB0aGlzLnBlcm1pc3Npb25zID0gdGhpcy5wZXJtaXNzaW9ucy5tYXAocGVybWlzc2lvbiA9PiAoeyAuLi5wZXJtaXNzaW9uLCBpc0dyYW50ZWQ6ICF0aGlzLnNlbGVjdEFsbFRhYiB9KSk7XG5cbiAgICB0aGlzLnNlbGVjdFRoaXNUYWIgPSAhdGhpcy5zZWxlY3RBbGxUYWI7XG4gIH1cblxuICBvbkNoYW5nZUdyb3VwKGdyb3VwOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5Hcm91cCkge1xuICAgIHRoaXMuc2VsZWN0ZWRHcm91cCA9IGdyb3VwO1xuICAgIHRoaXMuc2V0VGFiQ2hlY2tib3hTdGF0ZSgpO1xuICB9XG5cbiAgb25TdWJtaXQoKSB7XG4gICAgY29uc3QgdW5jaGFuZ2VkUGVybWlzc2lvbnMgPSBnZXRQZXJtaXNzaW9ucyhcbiAgICAgIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRQZXJtaXNzaW9uR3JvdXBzKSxcbiAgICApO1xuXG4gICAgY29uc3QgY2hhbmdlZFBlcm1pc3Npb25zOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5NaW5pbXVtUGVybWlzc2lvbltdID0gdGhpcy5wZXJtaXNzaW9uc1xuICAgICAgLmZpbHRlcihwZXIgPT5cbiAgICAgICAgdW5jaGFuZ2VkUGVybWlzc2lvbnMuZmluZCh1bmNoYW5nZWQgPT4gdW5jaGFuZ2VkLm5hbWUgPT09IHBlci5uYW1lKS5pc0dyYW50ZWQgPT09IHBlci5pc0dyYW50ZWQgPyBmYWxzZSA6IHRydWUsXG4gICAgICApXG4gICAgICAubWFwKCh7IG5hbWUsIGlzR3JhbnRlZCB9KSA9PiAoeyBuYW1lLCBpc0dyYW50ZWQgfSkpO1xuXG4gICAgaWYgKGNoYW5nZWRQZXJtaXNzaW9ucy5sZW5ndGgpIHtcbiAgICAgIHRoaXMuc3RvcmVcbiAgICAgICAgLmRpc3BhdGNoKFxuICAgICAgICAgIG5ldyBVcGRhdGVQZXJtaXNzaW9ucyh7XG4gICAgICAgICAgICBwcm92aWRlcktleTogdGhpcy5wcm92aWRlcktleSxcbiAgICAgICAgICAgIHByb3ZpZGVyTmFtZTogdGhpcy5wcm92aWRlck5hbWUsXG4gICAgICAgICAgICBwZXJtaXNzaW9uczogY2hhbmdlZFBlcm1pc3Npb25zLFxuICAgICAgICAgIH0pLFxuICAgICAgICApXG4gICAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xuICAgICAgICB9KTtcbiAgICB9IGVsc2Uge1xuICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XG4gICAgfVxuICB9XG5cbiAgb3Blbk1vZGFsKCkge1xuICAgIGlmICghdGhpcy5wcm92aWRlcktleSB8fCAhdGhpcy5wcm92aWRlck5hbWUpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcignUHJvdmlkZXIgS2V5IGFuZCBQcm92aWRlciBOYW1lIGFyZSByZXF1aXJlZC4nKTtcbiAgICB9XG5cbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2gobmV3IEdldFBlcm1pc3Npb25zKHsgcHJvdmlkZXJLZXk6IHRoaXMucHJvdmlkZXJLZXksIHByb3ZpZGVyTmFtZTogdGhpcy5wcm92aWRlck5hbWUgfSkpXG4gICAgICAucGlwZShwbHVjaygnUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZScsICdwZXJtaXNzaW9uUmVzJykpXG4gICAgICAuc3Vic2NyaWJlKChwZXJtaXNzaW9uUmVzOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5SZXNwb25zZSkgPT4ge1xuICAgICAgICB0aGlzLnNlbGVjdGVkR3JvdXAgPSBwZXJtaXNzaW9uUmVzLmdyb3Vwc1swXTtcbiAgICAgICAgdGhpcy5wZXJtaXNzaW9ucyA9IGdldFBlcm1pc3Npb25zKHBlcm1pc3Npb25SZXMuZ3JvdXBzKTtcblxuICAgICAgICB0aGlzLnZpc2libGUgPSB0cnVlO1xuICAgICAgfSk7XG4gIH1cblxuICBpbml0TW9kYWwoKSB7XG4gICAgdGhpcy5zZXRUYWJDaGVja2JveFN0YXRlKCk7XG4gICAgdGhpcy5zZXRHcmFudENoZWNrYm94U3RhdGUoKTtcbiAgfVxuXG4gIG5nT25DaGFuZ2VzKHsgdmlzaWJsZSB9OiBTaW1wbGVDaGFuZ2VzKTogdm9pZCB7XG4gICAgaWYgKCF2aXNpYmxlKSByZXR1cm47XG5cbiAgICBpZiAodmlzaWJsZS5jdXJyZW50VmFsdWUpIHtcbiAgICAgIHRoaXMub3Blbk1vZGFsKCk7XG4gICAgfSBlbHNlIGlmICh2aXNpYmxlLmN1cnJlbnRWYWx1ZSA9PT0gZmFsc2UgJiYgdGhpcy52aXNpYmxlKSB7XG4gICAgICB0aGlzLnZpc2libGUgPSBmYWxzZTtcbiAgICB9XG4gIH1cbn1cblxuZnVuY3Rpb24gZmluZE1hcmdpbihwZXJtaXNzaW9uczogUGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbltdLCBwZXJtaXNzaW9uOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uKSB7XG4gIGNvbnN0IHBhcmVudFBlcm1pc3Npb24gPSBwZXJtaXNzaW9ucy5maW5kKHBlciA9PiBwZXIubmFtZSA9PT0gcGVybWlzc2lvbi5wYXJlbnROYW1lKTtcblxuICBpZiAocGFyZW50UGVybWlzc2lvbiAmJiBwYXJlbnRQZXJtaXNzaW9uLnBhcmVudE5hbWUpIHtcbiAgICBsZXQgbWFyZ2luID0gMjA7XG4gICAgcmV0dXJuIChtYXJnaW4gKz0gZmluZE1hcmdpbihwZXJtaXNzaW9ucywgcGFyZW50UGVybWlzc2lvbikpO1xuICB9XG5cbiAgcmV0dXJuIHBhcmVudFBlcm1pc3Npb24gPyAyMCA6IDA7XG59XG5cbmZ1bmN0aW9uIGdldFBlcm1pc3Npb25zKGdyb3VwczogUGVybWlzc2lvbk1hbmFnZW1lbnQuR3JvdXBbXSk6IFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb25bXSB7XG4gIHJldHVybiBncm91cHMucmVkdWNlKChhY2MsIHZhbCkgPT4gWy4uLmFjYywgLi4udmFsLnBlcm1pc3Npb25zXSwgW10pO1xufVxuIl19