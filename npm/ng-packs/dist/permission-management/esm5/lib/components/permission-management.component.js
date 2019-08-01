/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Component, EventEmitter, Input, Output, Renderer2, } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { map, pluck, take } from 'rxjs/operators';
import { PermissionManagementGetPermissions, PermissionManagementUpdatePermissions, } from '../actions/permission-management.actions';
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
                .dispatch(new PermissionManagementUpdatePermissions({
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
            .dispatch(new PermissionManagementGetPermissions({ providerKey: this.providerKey, providerName: this.providerName }))
            .pipe(pluck('PermissionManagementState', 'permissionRes'))
            .subscribe((/**
         * @param {?} permissionRes
         * @return {?}
         */
        function (permissionRes) {
            _this.selectedGroup = permissionRes.groups[0];
            _this.permissions = getPermissions(permissionRes.groups);
            _this.visible = true;
            setTimeout((/**
             * @return {?}
             */
            function () {
                _this.setTabCheckboxState();
                _this.setGrantCheckboxState();
            }), 0);
        }));
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
                    template: "<abp-modal [(visible)]=\"visible\" *ngIf=\"visible\" size=\"lg\">\n  <ng-container *ngIf=\"{ entityName: entityName$ | async } as data\">\n    <ng-template #abpHeader>\n      <h4>{{ 'AbpPermissionManagement::Permissions' | abpLocalization }} - {{ data.entityName }}</h4>\n    </ng-template>\n    <ng-template #abpBody>\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input\n          type=\"checkbox\"\n          id=\"select-all-in-all-tabs\"\n          name=\"select-all-in-all-tabs\"\n          class=\"custom-control-input\"\n          [(ngModel)]=\"selectAllTab\"\n          (click)=\"onClickSelectAll()\"\n        />\n        <label class=\"custom-control-label\" for=\"select-all-in-all-tabs\">{{\n          'AbpPermissionManagement::SelectAllInAllTabs' | abpLocalization\n        }}</label>\n      </div>\n\n      <hr class=\"mt-2 mb-2\" />\n      <div class=\"row\">\n        <div class=\"col-4\">\n          <ul class=\"nav nav-pills flex-column\">\n            <perfect-scrollbar class=\"ps-show-always\" style=\"max-height: 70vh;\">\n              <li *ngFor=\"let group of groups$ | async; trackBy: trackByFn\" class=\"nav-item\">\n                <a class=\"nav-link\" [class.active]=\"selectedGroup.name === group.name\" (click)=\"onChangeGroup(group)\">{{\n                  group?.displayName\n                }}</a>\n              </li>\n            </perfect-scrollbar>\n          </ul>\n        </div>\n        <div class=\"col-8\">\n          <h4>{{ selectedGroup.displayName }}</h4>\n          <hr class=\"mt-2 mb-3\" />\n          <div class=\"pl-1 pt-1\">\n            <div class=\"custom-checkbox custom-control mb-2\">\n              <input\n                type=\"checkbox\"\n                id=\"select-all-in-this-tabs\"\n                name=\"select-all-in-this-tabs\"\n                class=\"custom-control-input\"\n                [(ngModel)]=\"selectThisTab\"\n                (click)=\"onClickSelectThisTab()\"\n              />\n              <label class=\"custom-control-label\" for=\"select-all-in-this-tabs\">{{\n                'AbpPermissionManagement::SelectAllInThisTab' | abpLocalization\n              }}</label>\n            </div>\n            <hr class=\"mb-3\" />\n            <perfect-scrollbar class=\"ps-show-always\" style=\"max-height: 60vh;\">\n              <div\n                *ngFor=\"let permission of selectedGroupPermissions$ | async; let i = index; trackBy: trackByFn\"\n                [style.margin-left]=\"permission.margin + 'px'\"\n                class=\"custom-checkbox custom-control mb-2\"\n              >\n                <input\n                  #permissionCheckbox\n                  type=\"checkbox\"\n                  [checked]=\"getChecked(permission.name)\"\n                  [value]=\"getChecked(permission.name)\"\n                  [attr.id]=\"permission.name\"\n                  class=\"custom-control-input\"\n                />\n                <label\n                  class=\"custom-control-label\"\n                  [attr.for]=\"permission.name\"\n                  (click)=\"onClickCheckbox(permission, permissionCheckbox.value)\"\n                  >{{ permission.displayName }}</label\n                >\n              </div>\n            </perfect-scrollbar>\n          </div>\n        </div>\n      </div>\n    </ng-template>\n    <ng-template #abpFooter>\n      <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\" (click)=\"onSubmit()\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n      </button>\n    </ng-template>\n  </ng-container>\n</abp-modal>\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcucGVybWlzc2lvbi1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFDTCxTQUFTLEVBQ1QsWUFBWSxFQUNaLEtBQUssRUFHTCxNQUFNLEVBQ04sU0FBUyxHQUdWLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBUSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDeEMsT0FBTyxFQUFFLEdBQUcsRUFBRSxLQUFLLEVBQUUsSUFBSSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDbEQsT0FBTyxFQUNMLGtDQUFrQyxFQUNsQyxxQ0FBcUMsR0FDdEMsTUFBTSwwQ0FBMEMsQ0FBQztBQUVsRCxPQUFPLEVBQUUseUJBQXlCLEVBQUUsTUFBTSx1Q0FBdUMsQ0FBQztBQUlsRjtJQWtFRSx1Q0FBb0IsS0FBWSxFQUFVLFFBQW1CO1FBQXpDLFVBQUssR0FBTCxLQUFLLENBQU87UUFBVSxhQUFRLEdBQVIsUUFBUSxDQUFXO1FBcEM3RCxrQkFBYSxHQUFHLElBQUksWUFBWSxFQUFXLENBQUM7UUFVNUMsZ0JBQVcsR0FBc0MsRUFBRSxDQUFDO1FBRXBELGtCQUFhLEdBQVksS0FBSyxDQUFDO1FBRS9CLGlCQUFZLEdBQVksS0FBSyxDQUFDO1FBRTlCLGNBQVM7Ozs7O1FBQWdELFVBQUMsQ0FBQyxFQUFFLElBQUksSUFBSyxPQUFBLElBQUksQ0FBQyxJQUFJLEVBQVQsQ0FBUyxFQUFDO0lBb0JoQixDQUFDO0lBckRqRSxzQkFDSSxrREFBTzs7OztRQURYO1lBRUUsT0FBTyxJQUFJLENBQUMsUUFBUSxDQUFDO1FBQ3ZC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Z0IsSUFBSSxnQkFBZ0IsQ0FBQyxVQUFVLEVBQUU7O1lBQy9DLE1BQU0sR0FBRyxFQUFFO1FBQ2YsT0FBTyxDQUFDLE1BQU0sSUFBSSxVQUFVLENBQUMsV0FBVyxFQUFFLGdCQUFnQixDQUFDLENBQUMsQ0FBQztLQUM5RDtJQUVELE9BQU8sZ0JBQWdCLENBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDO0FBQ25DLENBQUM7Ozs7O0FBRUQsU0FBUyxjQUFjLENBQUMsTUFBb0M7SUFDMUQsT0FBTyxNQUFNLENBQUMsTUFBTTs7Ozs7SUFBQyxVQUFDLEdBQUcsRUFBRSxHQUFHLElBQUssd0JBQUksR0FBRyxFQUFLLEdBQUcsQ0FBQyxXQUFXLEdBQTNCLENBQTRCLEdBQUUsRUFBRSxDQUFDLENBQUM7QUFDdkUsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7XG4gIENvbXBvbmVudCxcbiAgRXZlbnRFbWl0dGVyLFxuICBJbnB1dCxcbiAgT25DaGFuZ2VzLFxuICBPbkluaXQsXG4gIE91dHB1dCxcbiAgUmVuZGVyZXIyLFxuICBTaW1wbGVDaGFuZ2VzLFxuICBUcmFja0J5RnVuY3Rpb24sXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IGZyb20sIE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IG1hcCwgcGx1Y2ssIHRha2UgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQge1xuICBQZXJtaXNzaW9uTWFuYWdlbWVudEdldFBlcm1pc3Npb25zLFxuICBQZXJtaXNzaW9uTWFuYWdlbWVudFVwZGF0ZVBlcm1pc3Npb25zLFxufSBmcm9tICcuLi9hY3Rpb25zL3Blcm1pc3Npb24tbWFuYWdlbWVudC5hY3Rpb25zJztcbmltcG9ydCB7IFBlcm1pc3Npb25NYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3Blcm1pc3Npb24tbWFuYWdlbWVudCc7XG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL3Blcm1pc3Npb24tbWFuYWdlbWVudC5zdGF0ZSc7XG5cbnR5cGUgUGVybWlzc2lvbldpdGhNYXJnaW4gPSBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uICYgeyBtYXJnaW46IG51bWJlciB9O1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtcGVybWlzc2lvbi1tYW5hZ2VtZW50JyxcbiAgdGVtcGxhdGVVcmw6ICcuL3Blcm1pc3Npb24tbWFuYWdlbWVudC5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFBlcm1pc3Npb25NYW5hZ2VtZW50Q29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0LCBPbkNoYW5nZXMge1xuICBASW5wdXQoKVxuICBwcm92aWRlck5hbWU6IHN0cmluZztcblxuICBASW5wdXQoKVxuICBwcm92aWRlcktleTogc3RyaW5nO1xuXG4gIHByb3RlY3RlZCBfdmlzaWJsZTtcblxuICBASW5wdXQoKVxuICBnZXQgdmlzaWJsZSgpOiBib29sZWFuIHtcbiAgICByZXR1cm4gdGhpcy5fdmlzaWJsZTtcbiAgfVxuXG4gIHNldCB2aXNpYmxlKHZhbHVlOiBib29sZWFuKSB7XG4gICAgaWYgKCF0aGlzLnNlbGVjdGVkR3JvdXApIHJldHVybjtcblxuICAgIHRoaXMuX3Zpc2libGUgPSB2YWx1ZTtcbiAgICB0aGlzLnZpc2libGVDaGFuZ2UuZW1pdCh2YWx1ZSk7XG5cbiAgICBpZiAoIXZhbHVlKSB7XG4gICAgICB0aGlzLnNlbGVjdGVkR3JvdXAgPSBudWxsO1xuICAgIH1cbiAgfVxuXG4gIEBPdXRwdXQoKVxuICB2aXNpYmxlQ2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxib29sZWFuPigpO1xuXG4gIEBTZWxlY3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRQZXJtaXNzaW9uR3JvdXBzKVxuICBncm91cHMkOiBPYnNlcnZhYmxlPFBlcm1pc3Npb25NYW5hZ2VtZW50Lkdyb3VwW10+O1xuXG4gIEBTZWxlY3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRFbnRpdGl5RGlzcGxheU5hbWUpXG4gIGVudGl0eU5hbWUkOiBPYnNlcnZhYmxlPHN0cmluZz47XG5cbiAgc2VsZWN0ZWRHcm91cDogUGVybWlzc2lvbk1hbmFnZW1lbnQuR3JvdXA7XG5cbiAgcGVybWlzc2lvbnM6IFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb25bXSA9IFtdO1xuXG4gIHNlbGVjdFRoaXNUYWI6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBzZWxlY3RBbGxUYWI6IGJvb2xlYW4gPSBmYWxzZTtcblxuICB0cmFja0J5Rm46IFRyYWNrQnlGdW5jdGlvbjxQZXJtaXNzaW9uTWFuYWdlbWVudC5Hcm91cD4gPSAoXywgaXRlbSkgPT4gaXRlbS5uYW1lO1xuXG4gIGdldCBzZWxlY3RlZEdyb3VwUGVybWlzc2lvbnMkKCk6IE9ic2VydmFibGU8UGVybWlzc2lvbldpdGhNYXJnaW5bXT4ge1xuICAgIHJldHVybiB0aGlzLmdyb3VwcyQucGlwZShcbiAgICAgIG1hcChncm91cHMgPT5cbiAgICAgICAgdGhpcy5zZWxlY3RlZEdyb3VwID8gZ3JvdXBzLmZpbmQoZ3JvdXAgPT4gZ3JvdXAubmFtZSA9PT0gdGhpcy5zZWxlY3RlZEdyb3VwLm5hbWUpLnBlcm1pc3Npb25zIDogW10sXG4gICAgICApLFxuICAgICAgbWFwPFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb25bXSwgUGVybWlzc2lvbldpdGhNYXJnaW5bXT4ocGVybWlzc2lvbnMgPT5cbiAgICAgICAgcGVybWlzc2lvbnMubWFwKFxuICAgICAgICAgIHBlcm1pc3Npb24gPT5cbiAgICAgICAgICAgICgoe1xuICAgICAgICAgICAgICAuLi5wZXJtaXNzaW9uLFxuICAgICAgICAgICAgICBtYXJnaW46IGZpbmRNYXJnaW4ocGVybWlzc2lvbnMsIHBlcm1pc3Npb24pLFxuICAgICAgICAgICAgICBpc0dyYW50ZWQ6IHRoaXMucGVybWlzc2lvbnMuZmluZChwZXIgPT4gcGVyLm5hbWUgPT09IHBlcm1pc3Npb24ubmFtZSkuaXNHcmFudGVkLFxuICAgICAgICAgICAgfSBhcyBhbnkpIGFzIFBlcm1pc3Npb25XaXRoTWFyZ2luKSxcbiAgICAgICAgKSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlLCBwcml2YXRlIHJlbmRlcmVyOiBSZW5kZXJlcjIpIHt9XG5cbiAgbmdPbkluaXQoKTogdm9pZCB7fVxuXG4gIGdldENoZWNrZWQobmFtZTogc3RyaW5nKSB7XG4gICAgcmV0dXJuICh0aGlzLnBlcm1pc3Npb25zLmZpbmQocGVyID0+IHBlci5uYW1lID09PSBuYW1lKSB8fCB7IGlzR3JhbnRlZDogZmFsc2UgfSkuaXNHcmFudGVkO1xuICB9XG5cbiAgb25DbGlja0NoZWNrYm94KGNsaWNrZWRQZXJtaXNzaW9uOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uLCB2YWx1ZSkge1xuICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgdGhpcy5wZXJtaXNzaW9ucyA9IHRoaXMucGVybWlzc2lvbnMubWFwKHBlciA9PiB7XG4gICAgICAgIGlmIChjbGlja2VkUGVybWlzc2lvbi5uYW1lID09PSBwZXIubmFtZSkge1xuICAgICAgICAgIHJldHVybiB7IC4uLnBlciwgaXNHcmFudGVkOiAhcGVyLmlzR3JhbnRlZCB9O1xuICAgICAgICB9IGVsc2UgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLm5hbWUgPT09IHBlci5wYXJlbnROYW1lICYmIGNsaWNrZWRQZXJtaXNzaW9uLmlzR3JhbnRlZCkge1xuICAgICAgICAgIHJldHVybiB7IC4uLnBlciwgaXNHcmFudGVkOiBmYWxzZSB9O1xuICAgICAgICB9IGVsc2UgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLnBhcmVudE5hbWUgPT09IHBlci5uYW1lICYmICFjbGlja2VkUGVybWlzc2lvbi5pc0dyYW50ZWQpIHtcbiAgICAgICAgICByZXR1cm4geyAuLi5wZXIsIGlzR3JhbnRlZDogdHJ1ZSB9O1xuICAgICAgICB9XG5cbiAgICAgICAgcmV0dXJuIHBlcjtcbiAgICAgIH0pO1xuXG4gICAgICB0aGlzLnNldFRhYkNoZWNrYm94U3RhdGUoKTtcbiAgICAgIHRoaXMuc2V0R3JhbnRDaGVja2JveFN0YXRlKCk7XG4gICAgfSwgMCk7XG4gIH1cblxuICBzZXRUYWJDaGVja2JveFN0YXRlKCkge1xuICAgIHRoaXMuc2VsZWN0ZWRHcm91cFBlcm1pc3Npb25zJC5waXBlKHRha2UoMSkpLnN1YnNjcmliZShwZXJtaXNzaW9ucyA9PiB7XG4gICAgICBjb25zdCBzZWxlY3RlZFBlcm1pc3Npb25zID0gcGVybWlzc2lvbnMuZmlsdGVyKHBlciA9PiBwZXIuaXNHcmFudGVkKTtcbiAgICAgIGNvbnN0IGVsZW1lbnQgPSBkb2N1bWVudC5xdWVyeVNlbGVjdG9yKCcjc2VsZWN0LWFsbC1pbi10aGlzLXRhYnMnKSBhcyBhbnk7XG5cbiAgICAgIGlmIChzZWxlY3RlZFBlcm1pc3Npb25zLmxlbmd0aCA9PT0gcGVybWlzc2lvbnMubGVuZ3RoKSB7XG4gICAgICAgIGVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IGZhbHNlO1xuICAgICAgICB0aGlzLnNlbGVjdFRoaXNUYWIgPSB0cnVlO1xuICAgICAgfSBlbHNlIGlmIChzZWxlY3RlZFBlcm1pc3Npb25zLmxlbmd0aCA9PT0gMCkge1xuICAgICAgICBlbGVtZW50LmluZGV0ZXJtaW5hdGUgPSBmYWxzZTtcbiAgICAgICAgdGhpcy5zZWxlY3RUaGlzVGFiID0gZmFsc2U7XG4gICAgICB9IGVsc2Uge1xuICAgICAgICBlbGVtZW50LmluZGV0ZXJtaW5hdGUgPSB0cnVlO1xuICAgICAgfVxuICAgIH0pO1xuICB9XG5cbiAgc2V0R3JhbnRDaGVja2JveFN0YXRlKCkge1xuICAgIGNvbnN0IHNlbGVjdGVkQWxsUGVybWlzc2lvbnMgPSB0aGlzLnBlcm1pc3Npb25zLmZpbHRlcihwZXIgPT4gcGVyLmlzR3JhbnRlZCk7XG4gICAgY29uc3QgY2hlY2tib3hFbGVtZW50ID0gZG9jdW1lbnQucXVlcnlTZWxlY3RvcignI3NlbGVjdC1hbGwtaW4tYWxsLXRhYnMnKSBhcyBhbnk7XG5cbiAgICBpZiAoc2VsZWN0ZWRBbGxQZXJtaXNzaW9ucy5sZW5ndGggPT09IHRoaXMucGVybWlzc2lvbnMubGVuZ3RoKSB7XG4gICAgICBjaGVja2JveEVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IGZhbHNlO1xuICAgICAgdGhpcy5zZWxlY3RBbGxUYWIgPSB0cnVlO1xuICAgIH0gZWxzZSBpZiAoc2VsZWN0ZWRBbGxQZXJtaXNzaW9ucy5sZW5ndGggPT09IDApIHtcbiAgICAgIGNoZWNrYm94RWxlbWVudC5pbmRldGVybWluYXRlID0gZmFsc2U7XG4gICAgICB0aGlzLnNlbGVjdEFsbFRhYiA9IGZhbHNlO1xuICAgIH0gZWxzZSB7XG4gICAgICBjaGVja2JveEVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IHRydWU7XG4gICAgfVxuICB9XG5cbiAgb25DbGlja1NlbGVjdFRoaXNUYWIoKSB7XG4gICAgdGhpcy5zZWxlY3RlZEdyb3VwUGVybWlzc2lvbnMkLnBpcGUodGFrZSgxKSkuc3Vic2NyaWJlKHBlcm1pc3Npb25zID0+IHtcbiAgICAgIHBlcm1pc3Npb25zLmZvckVhY2gocGVybWlzc2lvbiA9PiB7XG4gICAgICAgIGNvbnN0IGluZGV4ID0gdGhpcy5wZXJtaXNzaW9ucy5maW5kSW5kZXgocGVyID0+IHBlci5uYW1lID09PSBwZXJtaXNzaW9uLm5hbWUpO1xuXG4gICAgICAgIHRoaXMucGVybWlzc2lvbnMgPSBbXG4gICAgICAgICAgLi4udGhpcy5wZXJtaXNzaW9ucy5zbGljZSgwLCBpbmRleCksXG4gICAgICAgICAgeyAuLi50aGlzLnBlcm1pc3Npb25zW2luZGV4XSwgaXNHcmFudGVkOiAhdGhpcy5zZWxlY3RUaGlzVGFiIH0sXG4gICAgICAgICAgLi4udGhpcy5wZXJtaXNzaW9ucy5zbGljZShpbmRleCArIDEpLFxuICAgICAgICBdO1xuICAgICAgfSk7XG4gICAgfSk7XG5cbiAgICB0aGlzLnNldEdyYW50Q2hlY2tib3hTdGF0ZSgpO1xuICB9XG5cbiAgb25DbGlja1NlbGVjdEFsbCgpIHtcbiAgICB0aGlzLnBlcm1pc3Npb25zID0gdGhpcy5wZXJtaXNzaW9ucy5tYXAocGVybWlzc2lvbiA9PiAoeyAuLi5wZXJtaXNzaW9uLCBpc0dyYW50ZWQ6ICF0aGlzLnNlbGVjdEFsbFRhYiB9KSk7XG5cbiAgICB0aGlzLnNlbGVjdFRoaXNUYWIgPSAhdGhpcy5zZWxlY3RBbGxUYWI7XG4gIH1cblxuICBvbkNoYW5nZUdyb3VwKGdyb3VwOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5Hcm91cCkge1xuICAgIHRoaXMuc2VsZWN0ZWRHcm91cCA9IGdyb3VwO1xuICAgIHRoaXMuc2V0VGFiQ2hlY2tib3hTdGF0ZSgpO1xuICB9XG5cbiAgb25TdWJtaXQoKSB7XG4gICAgY29uc3QgdW5jaGFuZ2VkUGVybWlzc2lvbnMgPSBnZXRQZXJtaXNzaW9ucyhcbiAgICAgIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRQZXJtaXNzaW9uR3JvdXBzKSxcbiAgICApO1xuXG4gICAgY29uc3QgY2hhbmdlZFBlcm1pc3Npb25zOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5NaW5pbXVtUGVybWlzc2lvbltdID0gdGhpcy5wZXJtaXNzaW9uc1xuICAgICAgLmZpbHRlcihwZXIgPT5cbiAgICAgICAgdW5jaGFuZ2VkUGVybWlzc2lvbnMuZmluZCh1bmNoYW5nZWQgPT4gdW5jaGFuZ2VkLm5hbWUgPT09IHBlci5uYW1lKS5pc0dyYW50ZWQgPT09IHBlci5pc0dyYW50ZWQgPyBmYWxzZSA6IHRydWUsXG4gICAgICApXG4gICAgICAubWFwKCh7IG5hbWUsIGlzR3JhbnRlZCB9KSA9PiAoeyBuYW1lLCBpc0dyYW50ZWQgfSkpO1xuXG4gICAgaWYgKGNoYW5nZWRQZXJtaXNzaW9ucy5sZW5ndGgpIHtcbiAgICAgIHRoaXMuc3RvcmVcbiAgICAgICAgLmRpc3BhdGNoKFxuICAgICAgICAgIG5ldyBQZXJtaXNzaW9uTWFuYWdlbWVudFVwZGF0ZVBlcm1pc3Npb25zKHtcbiAgICAgICAgICAgIHByb3ZpZGVyS2V5OiB0aGlzLnByb3ZpZGVyS2V5LFxuICAgICAgICAgICAgcHJvdmlkZXJOYW1lOiB0aGlzLnByb3ZpZGVyTmFtZSxcbiAgICAgICAgICAgIHBlcm1pc3Npb25zOiBjaGFuZ2VkUGVybWlzc2lvbnMsXG4gICAgICAgICAgfSksXG4gICAgICAgIClcbiAgICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XG4gICAgICAgIH0pO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aGlzLnZpc2libGUgPSBmYWxzZTtcbiAgICB9XG4gIH1cblxuICBvcGVuTW9kYWwoKSB7XG4gICAgaWYgKCF0aGlzLnByb3ZpZGVyS2V5IHx8ICF0aGlzLnByb3ZpZGVyTmFtZSkge1xuICAgICAgdGhyb3cgbmV3IEVycm9yKCdQcm92aWRlciBLZXkgYW5kIFByb3ZpZGVyIE5hbWUgYXJlIHJlcXVpcmVkLicpO1xuICAgIH1cblxuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChcbiAgICAgICAgbmV3IFBlcm1pc3Npb25NYW5hZ2VtZW50R2V0UGVybWlzc2lvbnMoeyBwcm92aWRlcktleTogdGhpcy5wcm92aWRlcktleSwgcHJvdmlkZXJOYW1lOiB0aGlzLnByb3ZpZGVyTmFtZSB9KSxcbiAgICAgIClcbiAgICAgIC5waXBlKHBsdWNrKCdQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlJywgJ3Blcm1pc3Npb25SZXMnKSlcbiAgICAgIC5zdWJzY3JpYmUoKHBlcm1pc3Npb25SZXM6IFBlcm1pc3Npb25NYW5hZ2VtZW50LlJlc3BvbnNlKSA9PiB7XG4gICAgICAgIHRoaXMuc2VsZWN0ZWRHcm91cCA9IHBlcm1pc3Npb25SZXMuZ3JvdXBzWzBdO1xuICAgICAgICB0aGlzLnBlcm1pc3Npb25zID0gZ2V0UGVybWlzc2lvbnMocGVybWlzc2lvblJlcy5ncm91cHMpO1xuXG4gICAgICAgIHRoaXMudmlzaWJsZSA9IHRydWU7XG5cbiAgICAgICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICAgICAgdGhpcy5zZXRUYWJDaGVja2JveFN0YXRlKCk7XG4gICAgICAgICAgdGhpcy5zZXRHcmFudENoZWNrYm94U3RhdGUoKTtcbiAgICAgICAgfSwgMCk7XG4gICAgICB9KTtcbiAgfVxuXG4gIG5nT25DaGFuZ2VzKHsgdmlzaWJsZSB9OiBTaW1wbGVDaGFuZ2VzKTogdm9pZCB7XG4gICAgaWYgKCF2aXNpYmxlKSByZXR1cm47XG5cbiAgICBpZiAodmlzaWJsZS5jdXJyZW50VmFsdWUpIHtcbiAgICAgIHRoaXMub3Blbk1vZGFsKCk7XG4gICAgfSBlbHNlIGlmICh2aXNpYmxlLmN1cnJlbnRWYWx1ZSA9PT0gZmFsc2UgJiYgdGhpcy52aXNpYmxlKSB7XG4gICAgICB0aGlzLnZpc2libGUgPSBmYWxzZTtcbiAgICB9XG4gIH1cbn1cblxuZnVuY3Rpb24gZmluZE1hcmdpbihwZXJtaXNzaW9uczogUGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbltdLCBwZXJtaXNzaW9uOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uKSB7XG4gIGNvbnN0IHBhcmVudFBlcm1pc3Npb24gPSBwZXJtaXNzaW9ucy5maW5kKHBlciA9PiBwZXIubmFtZSA9PT0gcGVybWlzc2lvbi5wYXJlbnROYW1lKTtcblxuICBpZiAocGFyZW50UGVybWlzc2lvbiAmJiBwYXJlbnRQZXJtaXNzaW9uLnBhcmVudE5hbWUpIHtcbiAgICBsZXQgbWFyZ2luID0gMjA7XG4gICAgcmV0dXJuIChtYXJnaW4gKz0gZmluZE1hcmdpbihwZXJtaXNzaW9ucywgcGFyZW50UGVybWlzc2lvbikpO1xuICB9XG5cbiAgcmV0dXJuIHBhcmVudFBlcm1pc3Npb24gPyAyMCA6IDA7XG59XG5cbmZ1bmN0aW9uIGdldFBlcm1pc3Npb25zKGdyb3VwczogUGVybWlzc2lvbk1hbmFnZW1lbnQuR3JvdXBbXSk6IFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb25bXSB7XG4gIHJldHVybiBncm91cHMucmVkdWNlKChhY2MsIHZhbCkgPT4gWy4uLmFjYywgLi4udmFsLnBlcm1pc3Npb25zXSwgW10pO1xufVxuIl19