import { RestService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import {
  Injectable,
  ɵɵdefineInjectable,
  ɵɵinject,
  EventEmitter,
  Component,
  Renderer2,
  Input,
  Output,
  NgModule,
} from '@angular/core';
import { Action, Selector, State, Store, Select, NgxsModule } from '@ngxs/store';
import { __decorate, __metadata, __assign, __spread } from 'tslib';
import { Observable } from 'rxjs';
import { tap, map, take, finalize, pluck } from 'rxjs/operators';

/**
 * @fileoverview added by tsickle
 * Generated from: lib/actions/permission-management.actions.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var GetPermissions = /** @class */ (function() {
  function GetPermissions(payload) {
    this.payload = payload;
  }
  GetPermissions.type = '[PermissionManagement] Get Permissions';
  return GetPermissions;
})();
if (false) {
  /** @type {?} */
  GetPermissions.type;
  /** @type {?} */
  GetPermissions.prototype.payload;
}
var UpdatePermissions = /** @class */ (function() {
  function UpdatePermissions(payload) {
    this.payload = payload;
  }
  UpdatePermissions.type = '[PermissionManagement] Update Permissions';
  return UpdatePermissions;
})();
if (false) {
  /** @type {?} */
  UpdatePermissions.type;
  /** @type {?} */
  UpdatePermissions.prototype.payload;
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/permission-management.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var PermissionManagementService = /** @class */ (function() {
  function PermissionManagementService(rest) {
    this.rest = rest;
  }
  /**
   * @param {?} params
   * @return {?}
   */
  PermissionManagementService.prototype.getPermissions
  /**
   * @param {?} params
   * @return {?}
   */ = function(params) {
    /** @type {?} */
    var request = {
      method: 'GET',
      url: '/api/abp/permissions',
      params: params,
    };
    return this.rest.request(request);
  };
  /**
   * @param {?} __0
   * @return {?}
   */
  PermissionManagementService.prototype.updatePermissions
  /**
   * @param {?} __0
   * @return {?}
   */ = function(_a) {
    var permissions = _a.permissions,
      providerKey = _a.providerKey,
      providerName = _a.providerName;
    /** @type {?} */
    var request = {
      method: 'PUT',
      url: '/api/abp/permissions',
      body: { permissions: permissions },
      params: { providerKey: providerKey, providerName: providerName },
    };
    return this.rest.request(request);
  };
  PermissionManagementService.decorators = [
    {
      type: Injectable,
      args: [
        {
          providedIn: 'root',
        },
      ],
    },
  ];
  /** @nocollapse */
  PermissionManagementService.ctorParameters = function() {
    return [{ type: RestService }];
  };
  /** @nocollapse */ PermissionManagementService.ngInjectableDef = ɵɵdefineInjectable({
    factory: function PermissionManagementService_Factory() {
      return new PermissionManagementService(ɵɵinject(RestService));
    },
    token: PermissionManagementService,
    providedIn: 'root',
  });
  return PermissionManagementService;
})();
if (false) {
  /**
   * @type {?}
   * @private
   */
  PermissionManagementService.prototype.rest;
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/permission-management.state.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var PermissionManagementState = /** @class */ (function () {
    function PermissionManagementState(permissionManagementService) {
        this.permissionManagementService = permissionManagementService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    PermissionManagementState.getPermissionGroups = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var permissionRes = _a.permissionRes;
        return permissionRes.groups || [];
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    PermissionManagementState.getEntityDisplayName = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var permissionRes = _a.permissionRes;
        return permissionRes.entityDisplayName;
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    PermissionManagementState.prototype.permissionManagementGet = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState;
        var payload = _b.payload;
        return this.permissionManagementService.getPermissions(payload).pipe(tap((/**
         * @param {?} permissionResponse
         * @return {?}
         */
        function (permissionResponse) {
            return patchState({
                permissionRes: permissionResponse,
            });
        })));
    };
    /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    PermissionManagementState.prototype.permissionManagementUpdate = /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    function (_, _a) {
        var payload = _a.payload;
        return this.permissionManagementService.updatePermissions(payload);
    };
    PermissionManagementState.ctorParameters = function () { return [
        { type: PermissionManagementService }
    ]; };
    __decorate([
        Action(GetPermissions),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, GetPermissions]),
        __metadata("design:returntype", void 0)
    ], PermissionManagementState.prototype, "permissionManagementGet", null);
    __decorate([
        Action(UpdatePermissions),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, UpdatePermissions]),
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
    ], PermissionManagementState, "getEntityDisplayName", null);
    PermissionManagementState = __decorate([
        State({
            name: 'PermissionManagementState',
            defaults: (/** @type {?} */ ({ permissionRes: {} })),
        }),
        __metadata("design:paramtypes", [PermissionManagementService])
    ], PermissionManagementState);
    return PermissionManagementState;
}());
                    function(per) {
                      return per.name === permission.name;
                    },
                  ).isGranted,
                }));
              },
            );
          },
        ),
      );
    },
    enumerable: true,
    configurable: true,
  });
  /**
   * @return {?}
   */
  PermissionManagementComponent.prototype.ngOnInit
  /**
   * @return {?}
   */ = function() {};
  /**
   * @param {?} name
   * @return {?}
   */
  PermissionManagementComponent.prototype.getChecked
  /**
   * @param {?} name
   * @return {?}
   */ = function(name) {
    return (
      this.permissions.find(
        /**
         * @param {?} per
         * @return {?}
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
           * @return {?}
           */
          function(p) {
            return p.providerName === 'Role';
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
                    return __assign({}, per, { isGranted: !per.isGranted });
                }
                else if (clickedPermission.name === per.parentName && clickedPermission.isGranted) {
                    return __assign({}, per, { isGranted: false });
                }
                else if (clickedPermission.parentName === per.name && !clickedPermission.isGranted) {
                    return __assign({}, per, { isGranted: true });
                }
                return per;
            }));
            _this.setTabCheckboxState();
            _this.setGrantCheckboxState();
        }), 0);
    };
   */
  PermissionManagementComponent.prototype.setTabCheckboxState
  /**
   * @return {?}
   */ = function() {
    var _this = this;
    this.selectedGroupPermissions$.pipe(take(1)).subscribe(
      /**
       * @param {?} permissions
       * @return {?}
       */
      function(permissions) {
        /** @type {?} */
        var selectedPermissions = permissions.filter(
          /**
           * @param {?} per
           * @return {?}
           */
          (function(per) {
            return per.isGranted;
          }),
        );
        /** @type {?} */
        var element = /** @type {?} */ (document.querySelector('#select-all-in-this-tabs'));
        if (selectedPermissions.length === permissions.length) {
          element.indeterminate = false;
          _this.selectThisTab = true;
        } else if (selectedPermissions.length === 0) {
          element.indeterminate = false;
          _this.selectThisTab = false;
        } else {
          element.indeterminate = true;
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
                _this.permissions = __spread(_this.permissions.slice(0, index), [
                    __assign({}, _this.permissions[index], { isGranted: !_this.selectThisTab })
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
        function (permission) { return (__assign({}, permission, { isGranted: _this.isGrantedByOtherProviderName(permission.grantedProviders) || !_this.selectAllTab })); }));
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
        (function(per) {
          return unchangedPermissions.find(
            /**
             * @param {?} unchanged
             * @return {?}
             */
            function(unchanged) {
              return unchanged.name === per.name;
            },
          ).isGranted === per.isGranted
            ? false
            : true;
        }),
      )
      .map(
        /**
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
                _this.permissions = __spread(_this.permissions.slice(0, index), [
                    __assign({}, _this.permissions[index], { isGranted: !_this.selectThisTab })
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
        function (permission) { return (__assign({}, permission, { isGranted: _this.isGrantedByOtherProviderName(permission.grantedProviders) || !_this.selectAllTab })); }));
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
   */
  function Group() {}
  PermissionManagement.Group = Group;
  if (false) {
    /** @type {?} */
    Group.prototype.name;
    /** @type {?} */
    Group.prototype.displayName;
    /** @type {?} */
    Group.prototype.permissions;
  }
  /**
   * @record
   */
  function MinimumPermission() {}
  PermissionManagement.MinimumPermission = MinimumPermission;
  if (false) {
    /** @type {?} */
    MinimumPermission.prototype.name;
    /** @type {?} */
    MinimumPermission.prototype.isGranted;
  }
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
    __decorate([
        Select(PermissionManagementState.getPermissionGroups),
        __metadata("design:type", Observable)
    ], PermissionManagementComponent.prototype, "groups$", void 0);
    __decorate([
        Select(PermissionManagementState.getEntityDisplayName),
        __metadata("design:type", Observable)
    ], PermissionManagementComponent.prototype, "entityName$", void 0);
    return PermissionManagementComponent;
}());
var PermissionManagementStateService = /** @class */ (function () {
    function PermissionManagementStateService(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    PermissionManagementStateService.prototype.getPermissionGroups = /**
     * @return {?}
     */
    function () {
        return this.store.selectSnapshot(PermissionManagementState.getPermissionGroups);
    };
    /**
     * @return {?}
     */
    PermissionManagementStateService.prototype.getEntityDisplayName = /**
     * @return {?}
     */
    function () {
        return this.store.selectSnapshot(PermissionManagementState.getEntityDisplayName);
    };
    PermissionManagementStateService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    PermissionManagementStateService.ctorParameters = function () { return [
        { type: Store }
    ]; };
    /** @nocollapse */ PermissionManagementStateService.ngInjectableDef = ɵɵdefineInjectable({ factory: function PermissionManagementStateService_Factory() { return new PermissionManagementStateService(ɵɵinject(Store)); }, token: PermissionManagementStateService, providedIn: "root" });
    return PermissionManagementStateService;
}());
