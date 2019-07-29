(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@abp/ng.core'), require('@abp/ng.theme.shared'), require('@angular/core'), require('@ngxs/store'), require('@ng-bootstrap/ng-bootstrap'), require('rxjs'), require('rxjs/operators'), require('ngx-perfect-scrollbar')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.permission-management', ['exports', '@abp/ng.core', '@abp/ng.theme.shared', '@angular/core', '@ngxs/store', '@ng-bootstrap/ng-bootstrap', 'rxjs', 'rxjs/operators', 'ngx-perfect-scrollbar'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng['permission-management'] = {}), global.ng_core, global.ng_theme_shared, global.ng.core, global.store, global.ngBootstrap, global.rxjs, global.rxjs.operators, global.ngxPerfectScrollbar));
}(this, function (exports, ng_core, ng_theme_shared, core, store, ngBootstrap, rxjs, operators, ngxPerfectScrollbar) { 'use strict';

    /*! *****************************************************************************
    Copyright (c) Microsoft Corporation. All rights reserved.
    Licensed under the Apache License, Version 2.0 (the "License"); you may not use
    this file except in compliance with the License. You may obtain a copy of the
    License at http://www.apache.org/licenses/LICENSE-2.0

    THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
    KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED
    WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
    MERCHANTABLITY OR NON-INFRINGEMENT.

    See the Apache Version 2.0 License for specific language governing permissions
    and limitations under the License.
    ***************************************************************************** */

    var __assign = function() {
        __assign = Object.assign || function __assign(t) {
            for (var s, i = 1, n = arguments.length; i < n; i++) {
                s = arguments[i];
                for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p)) t[p] = s[p];
            }
            return t;
        };
        return __assign.apply(this, arguments);
    };

    function __decorate(decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    }

    function __metadata(metadataKey, metadataValue) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(metadataKey, metadataValue);
    }

    function __read(o, n) {
        var m = typeof Symbol === "function" && o[Symbol.iterator];
        if (!m) return o;
        var i = m.call(o), r, ar = [], e;
        try {
            while ((n === void 0 || n-- > 0) && !(r = i.next()).done) ar.push(r.value);
        }
        catch (error) { e = { error: error }; }
        finally {
            try {
                if (r && !r.done && (m = i["return"])) m.call(i);
            }
            finally { if (e) throw e.error; }
        }
        return ar;
    }

    function __spread() {
        for (var ar = [], i = 0; i < arguments.length; i++)
            ar = ar.concat(__read(arguments[i]));
        return ar;
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var PermissionManagementGetPermissions = /** @class */ (function () {
        function PermissionManagementGetPermissions(payload) {
            this.payload = payload;
        }
        PermissionManagementGetPermissions.type = '[PermissionManagement] Get Permissions';
        return PermissionManagementGetPermissions;
    }());
    var PermissionManagementUpdatePermissions = /** @class */ (function () {
        function PermissionManagementUpdatePermissions(payload) {
            this.payload = payload;
        }
        PermissionManagementUpdatePermissions.type = '[PermissionManagement] Update Permissions';
        return PermissionManagementUpdatePermissions;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var PermissionManagementService = /** @class */ (function () {
        function PermissionManagementService(rest) {
            this.rest = rest;
        }
        /**
         * @param {?} params
         * @return {?}
         */
        PermissionManagementService.prototype.getPermissions = /**
         * @param {?} params
         * @return {?}
         */
        function (params) {
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
        PermissionManagementService.prototype.updatePermissions = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var permissions = _a.permissions, providerKey = _a.providerKey, providerName = _a.providerName;
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
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        PermissionManagementService.ctorParameters = function () { return [
            { type: ng_core.RestService }
        ]; };
        /** @nocollapse */ PermissionManagementService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function PermissionManagementService_Factory() { return new PermissionManagementService(core.ɵɵinject(ng_core.RestService)); }, token: PermissionManagementService, providedIn: "root" });
        return PermissionManagementService;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
        PermissionManagementState.getEntitiyDisplayName = /**
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
            return this.permissionManagementService.getPermissions(payload).pipe(operators.tap((/**
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
        __decorate([
            store.Action(PermissionManagementGetPermissions),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, PermissionManagementGetPermissions]),
            __metadata("design:returntype", void 0)
        ], PermissionManagementState.prototype, "permissionManagementGet", null);
        __decorate([
            store.Action(PermissionManagementUpdatePermissions),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, PermissionManagementUpdatePermissions]),
            __metadata("design:returntype", void 0)
        ], PermissionManagementState.prototype, "permissionManagementUpdate", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", void 0)
        ], PermissionManagementState, "getPermissionGroups", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", String)
        ], PermissionManagementState, "getEntitiyDisplayName", null);
        PermissionManagementState = __decorate([
            store.State({
                name: 'PermissionManagementState',
                defaults: (/** @type {?} */ ({ permissionRes: {} })),
            }),
            __metadata("design:paramtypes", [PermissionManagementService])
        ], PermissionManagementState);
        return PermissionManagementState;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var PermissionManagementComponent = /** @class */ (function () {
        function PermissionManagementComponent(modalService, store, renderer) {
            this.modalService = modalService;
            this.store = store;
            this.renderer = renderer;
            this.visibleChange = new core.EventEmitter();
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
        Object.defineProperty(PermissionManagementComponent.prototype, "selectedGroupPermissions$", {
            get: /**
             * @return {?}
             */
            function () {
                var _this = this;
                return this.groups$.pipe(operators.map((/**
                 * @param {?} groups
                 * @return {?}
                 */
                function (groups) {
                    return _this.selectedGroup ? groups.find((/**
                     * @param {?} group
                     * @return {?}
                     */
                    function (group) { return group.name === _this.selectedGroup.name; })).permissions : [];
                })), operators.map((/**
                 * @param {?} permissions
                 * @return {?}
                 */
                function (permissions) {
                    return permissions.map((/**
                     * @param {?} permission
                     * @return {?}
                     */
                    function (permission) {
                        return ((/** @type {?} */ (((/** @type {?} */ (__assign({}, permission, { margin: findMargin(permissions, permission), isGranted: _this.permissions.find((/**
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
        /**
         * @return {?}
         */
        PermissionManagementComponent.prototype.setTabCheckboxState = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.selectedGroupPermissions$.pipe(operators.take(1)).subscribe((/**
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
            this.selectedGroupPermissions$.pipe(operators.take(1)).subscribe((/**
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
            function (permission) { return (__assign({}, permission, { isGranted: !_this.selectAllTab })); }));
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
                function () { return _this.modalRef.close(); }));
            }
            else {
                this.modalRef.close();
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
                .pipe(operators.pluck('PermissionManagementState', 'permissionRes'))
                .subscribe((/**
             * @param {?} permissionRes
             * @return {?}
             */
            function (permissionRes) {
                _this.selectedGroup = permissionRes.groups[0];
                _this.permissions = getPermissions(permissionRes.groups);
                _this.modalRef = _this.modalService.open(_this.modalContent, { size: 'lg' });
                _this.visibleChange.emit(true);
                setTimeout((/**
                 * @return {?}
                 */
                function () {
                    _this.setTabCheckboxState();
                    _this.setGrantCheckboxState();
                }), 0);
                rxjs.from(_this.modalRef.result)
                    .pipe(operators.take(1))
                    .subscribe((/**
                 * @param {?} data
                 * @return {?}
                 */
                function (data) {
                    _this.setVisible(false);
                }), (/**
                 * @param {?} reason
                 * @return {?}
                 */
                function (reason) {
                    _this.setVisible(false);
                }));
            }));
        };
        /**
         * @param {?} value
         * @return {?}
         */
        PermissionManagementComponent.prototype.setVisible = /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this.visible = value;
            this.visibleChange.emit(value);
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
            else if (visible.currentValue === false && this.modalService.hasOpenModals()) {
                this.modalRef.close();
            }
        };
        PermissionManagementComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-permission-management',
                        template: "<ng-template #modalContent let-modal>\n  <ng-container *ngIf=\"{ entityName: entityName$ | async } as data\">\n    <div class=\"modal-header\">\n      <h4 class=\"modal-title\" id=\"modal-basic-title\">\n        {{ 'AbpPermissionManagement::Permissions' | abpLocalization }} -\n        {{ data.entityName }}\n      </h4>\n      <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n        <span aria-hidden=\"true\">&times;</span>\n      </button>\n    </div>\n    <div class=\"modal-body\">\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input\n          type=\"checkbox\"\n          id=\"select-all-in-all-tabs\"\n          name=\"select-all-in-all-tabs\"\n          class=\"custom-control-input\"\n          [(ngModel)]=\"selectAllTab\"\n          (click)=\"onClickSelectAll()\"\n        />\n        <label class=\"custom-control-label\" for=\"select-all-in-all-tabs\">{{\n          'AbpPermissionManagement::SelectAllInAllTabs' | abpLocalization\n        }}</label>\n      </div>\n\n      <hr class=\"mt-2 mb-2\" />\n      <div class=\"row\">\n        <div class=\"col-4\">\n          <ul class=\"nav nav-pills flex-column\">\n            <perfect-scrollbar class=\"ps-show-always\" style=\"max-height: 70vh;\">\n              <li *ngFor=\"let group of groups$ | async; trackBy: trackByFn\" class=\"nav-item\">\n                <a class=\"nav-link\" [class.active]=\"selectedGroup.name === group.name\" (click)=\"onChangeGroup(group)\">{{\n                  group.displayName\n                }}</a>\n              </li>\n            </perfect-scrollbar>\n          </ul>\n        </div>\n        <div class=\"col-8\">\n          <h4>{{ selectedGroup.displayName }}</h4>\n          <hr class=\"mt-2 mb-3\" />\n          <div class=\"pl-1 pt-1\">\n            <div class=\"custom-checkbox custom-control mb-2\">\n              <input\n                type=\"checkbox\"\n                id=\"select-all-in-this-tabs\"\n                name=\"select-all-in-this-tabs\"\n                class=\"custom-control-input\"\n                [(ngModel)]=\"selectThisTab\"\n                (click)=\"onClickSelectThisTab()\"\n              />\n              <label class=\"custom-control-label\" for=\"select-all-in-this-tabs\">{{\n                'AbpPermissionManagement::SelectAllInThisTab' | abpLocalization\n              }}</label>\n            </div>\n            <hr class=\"mb-3\" />\n            <perfect-scrollbar class=\"ps-show-always\" style=\"max-height: 60vh;\">\n              <div\n                *ngFor=\"let permission of selectedGroupPermissions$ | async; let i = index; trackBy: trackByFn\"\n                [style.margin-left]=\"permission.margin + 'px'\"\n                class=\"custom-checkbox custom-control mb-2\"\n              >\n                <input\n                  #permissionCheckbox\n                  type=\"checkbox\"\n                  [checked]=\"getChecked(permission.name)\"\n                  [value]=\"getChecked(permission.name)\"\n                  [attr.id]=\"permission.name\"\n                  class=\"custom-control-input\"\n                />\n                <label\n                  class=\"custom-control-label\"\n                  [attr.for]=\"permission.name\"\n                  (click)=\"onClickCheckbox(permission, permissionCheckbox.value)\"\n                  >{{ permission.displayName }}</label\n                >\n              </div>\n            </perfect-scrollbar>\n          </div>\n        </div>\n      </div>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpIdentity::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\" (click)=\"onSubmit()\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </ng-container>\n</ng-template>\n"
                    }] }
        ];
        /** @nocollapse */
        PermissionManagementComponent.ctorParameters = function () { return [
            { type: ngBootstrap.NgbModal },
            { type: store.Store },
            { type: core.Renderer2 }
        ]; };
        PermissionManagementComponent.propDecorators = {
            providerName: [{ type: core.Input }],
            providerKey: [{ type: core.Input }],
            visible: [{ type: core.Input }],
            visibleChange: [{ type: core.Output }],
            modalContent: [{ type: core.ViewChild, args: ['modalContent', { static: false },] }]
        };
        __decorate([
            store.Select(PermissionManagementState.getPermissionGroups),
            __metadata("design:type", rxjs.Observable)
        ], PermissionManagementComponent.prototype, "groups$", void 0);
        __decorate([
            store.Select(PermissionManagementState.getEntitiyDisplayName),
            __metadata("design:type", rxjs.Observable)
        ], PermissionManagementComponent.prototype, "entityName$", void 0);
        return PermissionManagementComponent;
    }());
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
        function (acc, val) { return __spread(acc, val.permissions); }), []);
    }

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var PermissionManagementModule = /** @class */ (function () {
        function PermissionManagementModule() {
        }
        PermissionManagementModule.decorators = [
            { type: core.NgModule, args: [{
                        declarations: [PermissionManagementComponent],
                        imports: [ng_core.CoreModule, ng_theme_shared.ThemeSharedModule, store.NgxsModule.forFeature([PermissionManagementState]), ngxPerfectScrollbar.PerfectScrollbarModule],
                        exports: [PermissionManagementComponent],
                    },] }
        ];
        return PermissionManagementModule;
    }());

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

    exports.PermissionManagementComponent = PermissionManagementComponent;
    exports.PermissionManagementGetPermissions = PermissionManagementGetPermissions;
    exports.PermissionManagementModule = PermissionManagementModule;
    exports.PermissionManagementService = PermissionManagementService;
    exports.PermissionManagementState = PermissionManagementState;
    exports.PermissionManagementUpdatePermissions = PermissionManagementUpdatePermissions;
    exports.ɵa = PermissionManagementComponent;
    exports.ɵb = PermissionManagementState;
    exports.ɵc = PermissionManagementService;
    exports.ɵd = PermissionManagementGetPermissions;
    exports.ɵe = PermissionManagementUpdatePermissions;

    Object.defineProperty(exports, '__esModule', { value: true });

}));
//# sourceMappingURL=abp-ng.permission-management.umd.js.map
