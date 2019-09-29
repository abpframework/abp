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
    PermissionManagementComponent.prototype.isGrantedByRole = /**
     * @param {?} grantedProviders
     * @return {?}
     */
    function (grantedProviders) {
        if (grantedProviders.length) {
            return grantedProviders.findIndex((/**
             * @param {?} p
             * @return {?}
             */
            function (p) { return p.providerName === 'Role'; })) > -1;
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
        if (clickedPermission.isGranted && this.isGrantedByRole(clickedPermission.grantedProviders))
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
                if (permission.isGranted && _this.isGrantedByRole(permission.grantedProviders))
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
                .subscribe((/**
             * @return {?}
             */
            function () {
                _this.modalBusy = false;
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
                    template: "<abp-modal [(visible)]=\"visible\" (init)=\"initModal()\" [busy]=\"modalBusy\">\n  <ng-container *ngIf=\"{ entityName: entityName$ | async } as data\">\n    <ng-template #abpHeader>\n      <h4>{{ 'AbpPermissionManagement::Permissions' | abpLocalization }} - {{ data.entityName }}</h4>\n    </ng-template>\n    <ng-template #abpBody>\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input\n          type=\"checkbox\"\n          id=\"select-all-in-all-tabs\"\n          name=\"select-all-in-all-tabs\"\n          class=\"custom-control-input\"\n          [(ngModel)]=\"selectAllTab\"\n          (click)=\"onClickSelectAll()\"\n        />\n        <label class=\"custom-control-label\" for=\"select-all-in-all-tabs\">{{\n          'AbpPermissionManagement::SelectAllInAllTabs' | abpLocalization\n        }}</label>\n      </div>\n\n      <hr class=\"mt-2 mb-2\" />\n      <div class=\"row\">\n        <div class=\"col-4\">\n          <ul class=\"nav nav-pills flex-column\">\n            <li *ngFor=\"let group of groups$ | async; trackBy: trackByFn\" class=\"nav-item\">\n              <a\n                class=\"nav-link pointer\"\n                [class.active]=\"selectedGroup?.name === group?.name\"\n                (click)=\"onChangeGroup(group)\"\n                >{{ group?.displayName }}</a\n              >\n            </li>\n          </ul>\n        </div>\n        <div class=\"col-8\">\n          <h4>{{ selectedGroup?.displayName }}</h4>\n          <hr class=\"mt-2 mb-3\" />\n          <div class=\"pl-1 pt-1\">\n            <div class=\"custom-checkbox custom-control mb-2\">\n              <input\n                type=\"checkbox\"\n                id=\"select-all-in-this-tabs\"\n                name=\"select-all-in-this-tabs\"\n                class=\"custom-control-input\"\n                [(ngModel)]=\"selectThisTab\"\n                (click)=\"onClickSelectThisTab()\"\n              />\n              <label class=\"custom-control-label\" for=\"select-all-in-this-tabs\">{{\n                'AbpPermissionManagement::SelectAllInThisTab' | abpLocalization\n              }}</label>\n            </div>\n            <hr class=\"mb-3\" />\n            <div\n              *ngFor=\"let permission of selectedGroupPermissions$ | async; let i = index; trackBy: trackByFn\"\n              [style.margin-left]=\"permission.margin + 'px'\"\n              class=\"custom-checkbox custom-control mb-2\"\n            >\n              <input\n                #permissionCheckbox\n                type=\"checkbox\"\n                [checked]=\"getChecked(permission.name)\"\n                [value]=\"getChecked(permission.name)\"\n                [attr.id]=\"permission.name\"\n                class=\"custom-control-input\"\n                [disabled]=\"isGrantedByRole(permission.grantedProviders)\"\n              />\n              <label\n                class=\"custom-control-label\"\n                [attr.for]=\"permission.name\"\n                (click)=\"onClickCheckbox(permission, permissionCheckbox.value)\"\n                >{{ permission.displayName }}\n                <span *ngFor=\"let provider of permission.grantedProviders\" class=\"badge badge-light\"\n                  >{{ provider.providerName }}: {{ provider.providerKey }}</span\n                ></label\n              >\n            </div>\n          </div>\n        </div>\n      </div>\n    </ng-template>\n    <ng-template #abpFooter>\n      <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\n      </button>\n      <abp-button iconClass=\"fa fa-check\" (click)=\"submit()\">{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button>\n    </ng-template>\n  </ng-container>\n</abp-modal>\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcucGVybWlzc2lvbi1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFDTCxTQUFTLEVBQ1QsWUFBWSxFQUNaLEtBQUssRUFHTCxNQUFNLEVBQ04sU0FBUyxHQUdWLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBUSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDeEMsT0FBTyxFQUFFLEdBQUcsRUFBRSxLQUFLLEVBQUUsSUFBSSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDbEQsT0FBTyxFQUFFLGNBQWMsRUFBRSxpQkFBaUIsRUFBRSxNQUFNLDBDQUEwQyxDQUFDO0FBRTdGLE9BQU8sRUFBRSx5QkFBeUIsRUFBRSxNQUFNLHVDQUF1QyxDQUFDO0FBSWxGO0lBb0VFLHVDQUFvQixLQUFZLEVBQVUsUUFBbUI7UUFBekMsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUFVLGFBQVEsR0FBUixRQUFRLENBQVc7UUF0QzdELGtCQUFhLEdBQUcsSUFBSSxZQUFZLEVBQVcsQ0FBQztRQVU1QyxnQkFBVyxHQUFzQyxFQUFFLENBQUM7UUFFcEQsa0JBQWEsR0FBWSxLQUFLLENBQUM7UUFFL0IsaUJBQVksR0FBWSxLQUFLLENBQUM7UUFFOUIsY0FBUyxHQUFZLEtBQUssQ0FBQztRQUUzQixjQUFTOzs7OztRQUFnRCxVQUFDLENBQUMsRUFBRSxJQUFJLElBQUssT0FBQSxJQUFJLENBQUMsSUFBSSxFQUFULENBQVMsRUFBQztJQW9CaEIsQ0FBQztJQXZEakUsc0JBQ0ksa0RBQU87Ozs7UUFEWDtZQUVFLE9BQU8sSUFB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ImltcG9ydCB7XG4gIENvbXBvbmVudCxcbiAgRXZlbnRFbWl0dGVyLFxuICBJbnB1dCxcbiAgT25DaGFuZ2VzLFxuICBPbkluaXQsXG4gIE91dHB1dCxcbiAgUmVuZGVyZXIyLFxuICBTaW1wbGVDaGFuZ2VzLFxuICBUcmFja0J5RnVuY3Rpb24sXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IGZyb20sIE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IG1hcCwgcGx1Y2ssIHRha2UgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgeyBHZXRQZXJtaXNzaW9ucywgVXBkYXRlUGVybWlzc2lvbnMgfSBmcm9tICcuLi9hY3Rpb25zL3Blcm1pc3Npb24tbWFuYWdlbWVudC5hY3Rpb25zJztcbmltcG9ydCB7IFBlcm1pc3Npb25NYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3Blcm1pc3Npb24tbWFuYWdlbWVudCc7XG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL3Blcm1pc3Npb24tbWFuYWdlbWVudC5zdGF0ZSc7XG5cbnR5cGUgUGVybWlzc2lvbldpdGhNYXJnaW4gPSBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uICYgeyBtYXJnaW46IG51bWJlciB9O1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtcGVybWlzc2lvbi1tYW5hZ2VtZW50JyxcbiAgdGVtcGxhdGVVcmw6ICcuL3Blcm1pc3Npb24tbWFuYWdlbWVudC5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFBlcm1pc3Npb25NYW5hZ2VtZW50Q29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0LCBPbkNoYW5nZXMge1xuICBASW5wdXQoKVxuICBwcm92aWRlck5hbWU6IHN0cmluZztcblxuICBASW5wdXQoKVxuICBwcm92aWRlcktleTogc3RyaW5nO1xuXG4gIHByb3RlY3RlZCBfdmlzaWJsZTtcblxuICBASW5wdXQoKVxuICBnZXQgdmlzaWJsZSgpOiBib29sZWFuIHtcbiAgICByZXR1cm4gdGhpcy5fdmlzaWJsZTtcbiAgfVxuXG4gIHNldCB2aXNpYmxlKHZhbHVlOiBib29sZWFuKSB7XG4gICAgaWYgKCF0aGlzLnNlbGVjdGVkR3JvdXApIHJldHVybjtcblxuICAgIHRoaXMuX3Zpc2libGUgPSB2YWx1ZTtcbiAgICB0aGlzLnZpc2libGVDaGFuZ2UuZW1pdCh2YWx1ZSk7XG5cbiAgICBpZiAoIXZhbHVlKSB7XG4gICAgICB0aGlzLnNlbGVjdGVkR3JvdXAgPSBudWxsO1xuICAgIH1cbiAgfVxuXG4gIEBPdXRwdXQoKVxuICB2aXNpYmxlQ2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxib29sZWFuPigpO1xuXG4gIEBTZWxlY3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRQZXJtaXNzaW9uR3JvdXBzKVxuICBncm91cHMkOiBPYnNlcnZhYmxlPFBlcm1pc3Npb25NYW5hZ2VtZW50Lkdyb3VwW10+O1xuXG4gIEBTZWxlY3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRFbnRpdGl5RGlzcGxheU5hbWUpXG4gIGVudGl0eU5hbWUkOiBPYnNlcnZhYmxlPHN0cmluZz47XG5cbiAgc2VsZWN0ZWRHcm91cDogUGVybWlzc2lvbk1hbmFnZW1lbnQuR3JvdXA7XG5cbiAgcGVybWlzc2lvbnM6IFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb25bXSA9IFtdO1xuXG4gIHNlbGVjdFRoaXNUYWI6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBzZWxlY3RBbGxUYWI6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBtb2RhbEJ1c3k6IGJvb2xlYW4gPSBmYWxzZTtcblxuICB0cmFja0J5Rm46IFRyYWNrQnlGdW5jdGlvbjxQZXJtaXNzaW9uTWFuYWdlbWVudC5Hcm91cD4gPSAoXywgaXRlbSkgPT4gaXRlbS5uYW1lO1xuXG4gIGdldCBzZWxlY3RlZEdyb3VwUGVybWlzc2lvbnMkKCk6IE9ic2VydmFibGU8UGVybWlzc2lvbldpdGhNYXJnaW5bXT4ge1xuICAgIHJldHVybiB0aGlzLmdyb3VwcyQucGlwZShcbiAgICAgIG1hcChncm91cHMgPT5cbiAgICAgICAgdGhpcy5zZWxlY3RlZEdyb3VwID8gZ3JvdXBzLmZpbmQoZ3JvdXAgPT4gZ3JvdXAubmFtZSA9PT0gdGhpcy5zZWxlY3RlZEdyb3VwLm5hbWUpLnBlcm1pc3Npb25zIDogW10sXG4gICAgICApLFxuICAgICAgbWFwPFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb25bXSwgUGVybWlzc2lvbldpdGhNYXJnaW5bXT4ocGVybWlzc2lvbnMgPT5cbiAgICAgICAgcGVybWlzc2lvbnMubWFwKFxuICAgICAgICAgIHBlcm1pc3Npb24gPT5cbiAgICAgICAgICAgICgoe1xuICAgICAgICAgICAgICAuLi5wZXJtaXNzaW9uLFxuICAgICAgICAgICAgICBtYXJnaW46IGZpbmRNYXJnaW4ocGVybWlzc2lvbnMsIHBlcm1pc3Npb24pLFxuICAgICAgICAgICAgICBpc0dyYW50ZWQ6IHRoaXMucGVybWlzc2lvbnMuZmluZChwZXIgPT4gcGVyLm5hbWUgPT09IHBlcm1pc3Npb24ubmFtZSkuaXNHcmFudGVkLFxuICAgICAgICAgICAgfSBhcyBhbnkpIGFzIFBlcm1pc3Npb25XaXRoTWFyZ2luKSxcbiAgICAgICAgKSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlLCBwcml2YXRlIHJlbmRlcmVyOiBSZW5kZXJlcjIpIHt9XG5cbiAgbmdPbkluaXQoKTogdm9pZCB7fVxuXG4gIGdldENoZWNrZWQobmFtZTogc3RyaW5nKSB7XG4gICAgcmV0dXJuICh0aGlzLnBlcm1pc3Npb25zLmZpbmQocGVyID0+IHBlci5uYW1lID09PSBuYW1lKSB8fCB7IGlzR3JhbnRlZDogZmFsc2UgfSkuaXNHcmFudGVkO1xuICB9XG5cbiAgaXNHcmFudGVkQnlSb2xlKGdyYW50ZWRQcm92aWRlcnM6IFBlcm1pc3Npb25NYW5hZ2VtZW50LkdyYW50ZWRQcm92aWRlcltdKTogYm9vbGVhbiB7XG4gICAgaWYgKGdyYW50ZWRQcm92aWRlcnMubGVuZ3RoKSB7XG4gICAgICByZXR1cm4gZ3JhbnRlZFByb3ZpZGVycy5maW5kSW5kZXgocCA9PiBwLnByb3ZpZGVyTmFtZSA9PT0gJ1JvbGUnKSA+IC0xO1xuICAgIH1cbiAgICByZXR1cm4gZmFsc2U7XG4gIH1cblxuICBvbkNsaWNrQ2hlY2tib3goY2xpY2tlZFBlcm1pc3Npb246IFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb24sIHZhbHVlKSB7XG4gICAgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLmlzR3JhbnRlZCAmJiB0aGlzLmlzR3JhbnRlZEJ5Um9sZShjbGlja2VkUGVybWlzc2lvbi5ncmFudGVkUHJvdmlkZXJzKSkgcmV0dXJuO1xuXG4gICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICB0aGlzLnBlcm1pc3Npb25zID0gdGhpcy5wZXJtaXNzaW9ucy5tYXAocGVyID0+IHtcbiAgICAgICAgaWYgKGNsaWNrZWRQZXJtaXNzaW9uLm5hbWUgPT09IHBlci5uYW1lKSB7XG4gICAgICAgICAgcmV0dXJuIHsgLi4ucGVyLCBpc0dyYW50ZWQ6ICFwZXIuaXNHcmFudGVkIH07XG4gICAgICAgIH0gZWxzZSBpZiAoY2xpY2tlZFBlcm1pc3Npb24ubmFtZSA9PT0gcGVyLnBhcmVudE5hbWUgJiYgY2xpY2tlZFBlcm1pc3Npb24uaXNHcmFudGVkKSB7XG4gICAgICAgICAgcmV0dXJuIHsgLi4ucGVyLCBpc0dyYW50ZWQ6IGZhbHNlIH07XG4gICAgICAgIH0gZWxzZSBpZiAoY2xpY2tlZFBlcm1pc3Npb24ucGFyZW50TmFtZSA9PT0gcGVyLm5hbWUgJiYgIWNsaWNrZWRQZXJtaXNzaW9uLmlzR3JhbnRlZCkge1xuICAgICAgICAgIHJldHVybiB7IC4uLnBlciwgaXNHcmFudGVkOiB0cnVlIH07XG4gICAgICAgIH1cblxuICAgICAgICByZXR1cm4gcGVyO1xuICAgICAgfSk7XG5cbiAgICAgIHRoaXMuc2V0VGFiQ2hlY2tib3hTdGF0ZSgpO1xuICAgICAgdGhpcy5zZXRHcmFudENoZWNrYm94U3RhdGUoKTtcbiAgICB9LCAwKTtcbiAgfVxuXG4gIHNldFRhYkNoZWNrYm94U3RhdGUoKSB7XG4gICAgdGhpcy5zZWxlY3RlZEdyb3VwUGVybWlzc2lvbnMkLnBpcGUodGFrZSgxKSkuc3Vic2NyaWJlKHBlcm1pc3Npb25zID0+IHtcbiAgICAgIGNvbnN0IHNlbGVjdGVkUGVybWlzc2lvbnMgPSBwZXJtaXNzaW9ucy5maWx0ZXIocGVyID0+IHBlci5pc0dyYW50ZWQpO1xuICAgICAgY29uc3QgZWxlbWVudCA9IGRvY3VtZW50LnF1ZXJ5U2VsZWN0b3IoJyNzZWxlY3QtYWxsLWluLXRoaXMtdGFicycpIGFzIGFueTtcblxuICAgICAgaWYgKHNlbGVjdGVkUGVybWlzc2lvbnMubGVuZ3RoID09PSBwZXJtaXNzaW9ucy5sZW5ndGgpIHtcbiAgICAgICAgZWxlbWVudC5pbmRldGVybWluYXRlID0gZmFsc2U7XG4gICAgICAgIHRoaXMuc2VsZWN0VGhpc1RhYiA9IHRydWU7XG4gICAgICB9IGVsc2UgaWYgKHNlbGVjdGVkUGVybWlzc2lvbnMubGVuZ3RoID09PSAwKSB7XG4gICAgICAgIGVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IGZhbHNlO1xuICAgICAgICB0aGlzLnNlbGVjdFRoaXNUYWIgPSBmYWxzZTtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIGVsZW1lbnQuaW5kZXRlcm1pbmF0ZSA9IHRydWU7XG4gICAgICB9XG4gICAgfSk7XG4gIH1cblxuICBzZXRHcmFudENoZWNrYm94U3RhdGUoKSB7XG4gICAgY29uc3Qgc2VsZWN0ZWRBbGxQZXJtaXNzaW9ucyA9IHRoaXMucGVybWlzc2lvbnMuZmlsdGVyKHBlciA9PiBwZXIuaXNHcmFudGVkKTtcbiAgICBjb25zdCBjaGVja2JveEVsZW1lbnQgPSBkb2N1bWVudC5xdWVyeVNlbGVjdG9yKCcjc2VsZWN0LWFsbC1pbi1hbGwtdGFicycpIGFzIGFueTtcblxuICAgIGlmIChzZWxlY3RlZEFsbFBlcm1pc3Npb25zLmxlbmd0aCA9PT0gdGhpcy5wZXJtaXNzaW9ucy5sZW5ndGgpIHtcbiAgICAgIGNoZWNrYm94RWxlbWVudC5pbmRldGVybWluYXRlID0gZmFsc2U7XG4gICAgICB0aGlzLnNlbGVjdEFsbFRhYiA9IHRydWU7XG4gICAgfSBlbHNlIGlmIChzZWxlY3RlZEFsbFBlcm1pc3Npb25zLmxlbmd0aCA9PT0gMCkge1xuICAgICAgY2hlY2tib3hFbGVtZW50LmluZGV0ZXJtaW5hdGUgPSBmYWxzZTtcbiAgICAgIHRoaXMuc2VsZWN0QWxsVGFiID0gZmFsc2U7XG4gICAgfSBlbHNlIHtcbiAgICAgIGNoZWNrYm94RWxlbWVudC5pbmRldGVybWluYXRlID0gdHJ1ZTtcbiAgICB9XG4gIH1cblxuICBvbkNsaWNrU2VsZWN0VGhpc1RhYigpIHtcbiAgICB0aGlzLnNlbGVjdGVkR3JvdXBQZXJtaXNzaW9ucyQucGlwZSh0YWtlKDEpKS5zdWJzY3JpYmUocGVybWlzc2lvbnMgPT4ge1xuICAgICAgcGVybWlzc2lvbnMuZm9yRWFjaChwZXJtaXNzaW9uID0+IHtcbiAgICAgICAgaWYgKHBlcm1pc3Npb24uaXNHcmFudGVkICYmIHRoaXMuaXNHcmFudGVkQnlSb2xlKHBlcm1pc3Npb24uZ3JhbnRlZFByb3ZpZGVycykpIHJldHVybjtcblxuICAgICAgICBjb25zdCBpbmRleCA9IHRoaXMucGVybWlzc2lvbnMuZmluZEluZGV4KHBlciA9PiBwZXIubmFtZSA9PT0gcGVybWlzc2lvbi5uYW1lKTtcblxuICAgICAgICB0aGlzLnBlcm1pc3Npb25zID0gW1xuICAgICAgICAgIC4uLnRoaXMucGVybWlzc2lvbnMuc2xpY2UoMCwgaW5kZXgpLFxuICAgICAgICAgIHsgLi4udGhpcy5wZXJtaXNzaW9uc1tpbmRleF0sIGlzR3JhbnRlZDogIXRoaXMuc2VsZWN0VGhpc1RhYiB9LFxuICAgICAgICAgIC4uLnRoaXMucGVybWlzc2lvbnMuc2xpY2UoaW5kZXggKyAxKSxcbiAgICAgICAgXTtcbiAgICAgIH0pO1xuICAgIH0pO1xuXG4gICAgdGhpcy5zZXRHcmFudENoZWNrYm94U3RhdGUoKTtcbiAgfVxuXG4gIG9uQ2xpY2tTZWxlY3RBbGwoKSB7XG4gICAgdGhpcy5wZXJtaXNzaW9ucyA9IHRoaXMucGVybWlzc2lvbnMubWFwKHBlcm1pc3Npb24gPT4gKHsgLi4ucGVybWlzc2lvbiwgaXNHcmFudGVkOiAhdGhpcy5zZWxlY3RBbGxUYWIgfSkpO1xuXG4gICAgdGhpcy5zZWxlY3RUaGlzVGFiID0gIXRoaXMuc2VsZWN0QWxsVGFiO1xuICB9XG5cbiAgb25DaGFuZ2VHcm91cChncm91cDogUGVybWlzc2lvbk1hbmFnZW1lbnQuR3JvdXApIHtcbiAgICB0aGlzLnNlbGVjdGVkR3JvdXAgPSBncm91cDtcbiAgICB0aGlzLnNldFRhYkNoZWNrYm94U3RhdGUoKTtcbiAgfVxuXG4gIHN1Ym1pdCgpIHtcbiAgICB0aGlzLm1vZGFsQnVzeSA9IHRydWU7XG4gICAgY29uc3QgdW5jaGFuZ2VkUGVybWlzc2lvbnMgPSBnZXRQZXJtaXNzaW9ucyhcbiAgICAgIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRQZXJtaXNzaW9uR3JvdXBzKSxcbiAgICApO1xuXG4gICAgY29uc3QgY2hhbmdlZFBlcm1pc3Npb25zOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5NaW5pbXVtUGVybWlzc2lvbltdID0gdGhpcy5wZXJtaXNzaW9uc1xuICAgICAgLmZpbHRlcihwZXIgPT5cbiAgICAgICAgdW5jaGFuZ2VkUGVybWlzc2lvbnMuZmluZCh1bmNoYW5nZWQgPT4gdW5jaGFuZ2VkLm5hbWUgPT09IHBlci5uYW1lKS5pc0dyYW50ZWQgPT09IHBlci5pc0dyYW50ZWQgPyBmYWxzZSA6IHRydWUsXG4gICAgICApXG4gICAgICAubWFwKCh7IG5hbWUsIGlzR3JhbnRlZCB9KSA9PiAoeyBuYW1lLCBpc0dyYW50ZWQgfSkpO1xuXG4gICAgaWYgKGNoYW5nZWRQZXJtaXNzaW9ucy5sZW5ndGgpIHtcbiAgICAgIHRoaXMuc3RvcmVcbiAgICAgICAgLmRpc3BhdGNoKFxuICAgICAgICAgIG5ldyBVcGRhdGVQZXJtaXNzaW9ucyh7XG4gICAgICAgICAgICBwcm92aWRlcktleTogdGhpcy5wcm92aWRlcktleSxcbiAgICAgICAgICAgIHByb3ZpZGVyTmFtZTogdGhpcy5wcm92aWRlck5hbWUsXG4gICAgICAgICAgICBwZXJtaXNzaW9uczogY2hhbmdlZFBlcm1pc3Npb25zLFxuICAgICAgICAgIH0pLFxuICAgICAgICApXG4gICAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICAgIHRoaXMubW9kYWxCdXN5ID0gZmFsc2U7XG4gICAgICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XG4gICAgICAgIH0pO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aGlzLm1vZGFsQnVzeSA9IGZhbHNlO1xuICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XG4gICAgfVxuICB9XG5cbiAgb3Blbk1vZGFsKCkge1xuICAgIGlmICghdGhpcy5wcm92aWRlcktleSB8fCAhdGhpcy5wcm92aWRlck5hbWUpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcignUHJvdmlkZXIgS2V5IGFuZCBQcm92aWRlciBOYW1lIGFyZSByZXF1aXJlZC4nKTtcbiAgICB9XG5cbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2gobmV3IEdldFBlcm1pc3Npb25zKHsgcHJvdmlkZXJLZXk6IHRoaXMucHJvdmlkZXJLZXksIHByb3ZpZGVyTmFtZTogdGhpcy5wcm92aWRlck5hbWUgfSkpXG4gICAgICAucGlwZShwbHVjaygnUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZScsICdwZXJtaXNzaW9uUmVzJykpXG4gICAgICAuc3Vic2NyaWJlKChwZXJtaXNzaW9uUmVzOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5SZXNwb25zZSkgPT4ge1xuICAgICAgICB0aGlzLnNlbGVjdGVkR3JvdXAgPSBwZXJtaXNzaW9uUmVzLmdyb3Vwc1swXTtcbiAgICAgICAgdGhpcy5wZXJtaXNzaW9ucyA9IGdldFBlcm1pc3Npb25zKHBlcm1pc3Npb25SZXMuZ3JvdXBzKTtcblxuICAgICAgICB0aGlzLnZpc2libGUgPSB0cnVlO1xuICAgICAgfSk7XG4gIH1cblxuICBpbml0TW9kYWwoKSB7XG4gICAgdGhpcy5zZXRUYWJDaGVja2JveFN0YXRlKCk7XG4gICAgdGhpcy5zZXRHcmFudENoZWNrYm94U3RhdGUoKTtcbiAgfVxuXG4gIG5nT25DaGFuZ2VzKHsgdmlzaWJsZSB9OiBTaW1wbGVDaGFuZ2VzKTogdm9pZCB7XG4gICAgaWYgKCF2aXNpYmxlKSByZXR1cm47XG5cbiAgICBpZiAodmlzaWJsZS5jdXJyZW50VmFsdWUpIHtcbiAgICAgIHRoaXMub3Blbk1vZGFsKCk7XG4gICAgfSBlbHNlIGlmICh2aXNpYmxlLmN1cnJlbnRWYWx1ZSA9PT0gZmFsc2UgJiYgdGhpcy52aXNpYmxlKSB7XG4gICAgICB0aGlzLnZpc2libGUgPSBmYWxzZTtcbiAgICB9XG4gIH1cbn1cblxuZnVuY3Rpb24gZmluZE1hcmdpbihwZXJtaXNzaW9uczogUGVybWlzc2lvbk1hbmFnZW1lbnQuUGVybWlzc2lvbltdLCBwZXJtaXNzaW9uOiBQZXJtaXNzaW9uTWFuYWdlbWVudC5QZXJtaXNzaW9uKSB7XG4gIGNvbnN0IHBhcmVudFBlcm1pc3Npb24gPSBwZXJtaXNzaW9ucy5maW5kKHBlciA9PiBwZXIubmFtZSA9PT0gcGVybWlzc2lvbi5wYXJlbnROYW1lKTtcblxuICBpZiAocGFyZW50UGVybWlzc2lvbiAmJiBwYXJlbnRQZXJtaXNzaW9uLnBhcmVudE5hbWUpIHtcbiAgICBsZXQgbWFyZ2luID0gMjA7XG4gICAgcmV0dXJuIChtYXJnaW4gKz0gZmluZE1hcmdpbihwZXJtaXNzaW9ucywgcGFyZW50UGVybWlzc2lvbikpO1xuICB9XG5cbiAgcmV0dXJuIHBhcmVudFBlcm1pc3Npb24gPyAyMCA6IDA7XG59XG5cbmZ1bmN0aW9uIGdldFBlcm1pc3Npb25zKGdyb3VwczogUGVybWlzc2lvbk1hbmFnZW1lbnQuR3JvdXBbXSk6IFBlcm1pc3Npb25NYW5hZ2VtZW50LlBlcm1pc3Npb25bXSB7XG4gIHJldHVybiBncm91cHMucmVkdWNlKChhY2MsIHZhbCkgPT4gWy4uLmFjYywgLi4udmFsLnBlcm1pc3Npb25zXSwgW10pO1xufVxuIl19