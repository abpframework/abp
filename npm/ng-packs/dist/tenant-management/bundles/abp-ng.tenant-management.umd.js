(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports, require('@abp/ng.core'), require('@abp/ng.theme.shared'), require('@angular/core'), require('@ng-bootstrap/ng-bootstrap'), require('@ngxs/store'), require('primeng/table'), require('@angular/forms'), require('rxjs'), require('rxjs/operators'), require('@angular/router')) :
    typeof define === 'function' && define.amd ? define('@abp/ng.tenant-management', ['exports', '@abp/ng.core', '@abp/ng.theme.shared', '@angular/core', '@ng-bootstrap/ng-bootstrap', '@ngxs/store', 'primeng/table', '@angular/forms', 'rxjs', 'rxjs/operators', '@angular/router'], factory) :
    (global = global || self, factory((global.abp = global.abp || {}, global.abp.ng = global.abp.ng || {}, global.abp.ng['tenant-management'] = {}), global.ng_core, global.ng_theme_shared, global.ng.core, global.ngBootstrap, global.store, global.table, global.ng.forms, global.rxjs, global.rxjs.operators, global.ng.router));
}(this, function (exports, ng_core, ng_theme_shared, core, ngBootstrap, store, table, forms, rxjs, operators, router) { 'use strict';

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

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var TenantManagementGet = /** @class */ (function () {
        function TenantManagementGet() {
        }
        TenantManagementGet.type = '[TenantManagement] Get';
        return TenantManagementGet;
    }());
    var TenantManagementGetById = /** @class */ (function () {
        function TenantManagementGetById(payload) {
            this.payload = payload;
        }
        TenantManagementGetById.type = '[TenantManagement] Get By Id';
        return TenantManagementGetById;
    }());
    var TenantManagementAdd = /** @class */ (function () {
        function TenantManagementAdd(payload) {
            this.payload = payload;
        }
        TenantManagementAdd.type = '[TenantManagement] Add';
        return TenantManagementAdd;
    }());
    var TenantManagementUpdate = /** @class */ (function () {
        function TenantManagementUpdate(payload) {
            this.payload = payload;
        }
        TenantManagementUpdate.type = '[TenantManagement] Update';
        return TenantManagementUpdate;
    }());
    var TenantManagementDelete = /** @class */ (function () {
        function TenantManagementDelete(payload) {
            this.payload = payload;
        }
        TenantManagementDelete.type = '[TenantManagement] Delete';
        return TenantManagementDelete;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var TenantManagementService = /** @class */ (function () {
        function TenantManagementService(rest) {
            this.rest = rest;
        }
        /**
         * @return {?}
         */
        TenantManagementService.prototype.get = /**
         * @return {?}
         */
        function () {
            /** @type {?} */
            var request = {
                method: 'GET',
                url: '/api/multi-tenancy/tenants',
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} id
         * @return {?}
         */
        TenantManagementService.prototype.getById = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            /** @type {?} */
            var request = {
                method: 'GET',
                url: "/api/multi-tenancy/tenants/" + id,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} id
         * @return {?}
         */
        TenantManagementService.prototype.delete = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            /** @type {?} */
            var request = {
                method: 'DELETE',
                url: "/api/multi-tenancy/tenants/" + id,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} body
         * @return {?}
         */
        TenantManagementService.prototype.add = /**
         * @param {?} body
         * @return {?}
         */
        function (body) {
            /** @type {?} */
            var request = {
                method: 'POST',
                url: "/api/multi-tenancy/tenants",
                body: body,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} body
         * @return {?}
         */
        TenantManagementService.prototype.update = /**
         * @param {?} body
         * @return {?}
         */
        function (body) {
            /** @type {?} */
            var url = "/api/multi-tenancy/tenants/" + body.id;
            delete body.id;
            /** @type {?} */
            var request = {
                method: 'PUT',
                url: url,
                body: body,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} id
         * @return {?}
         */
        TenantManagementService.prototype.getDefaultConnectionString = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            /** @type {?} */
            var url = "/api/multi-tenancy/tenants/" + id + "/defaultConnectionString";
            /** @type {?} */
            var request = {
                method: 'GET',
                responseType: "text" /* Text */,
                url: url,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} payload
         * @return {?}
         */
        TenantManagementService.prototype.updateDefaultConnectionString = /**
         * @param {?} payload
         * @return {?}
         */
        function (payload) {
            /** @type {?} */
            var url = "/api/multi-tenancy/tenants/" + payload.id + "/defaultConnectionString";
            /** @type {?} */
            var request = {
                method: 'PUT',
                url: url,
                params: { defaultConnectionString: payload.defaultConnectionString },
            };
            return this.rest.request(request);
        };
        TenantManagementService.decorators = [
            { type: core.Injectable, args: [{
                        providedIn: 'root',
                    },] }
        ];
        /** @nocollapse */
        TenantManagementService.ctorParameters = function () { return [
            { type: ng_core.RestService }
        ]; };
        /** @nocollapse */ TenantManagementService.ngInjectableDef = core.ɵɵdefineInjectable({ factory: function TenantManagementService_Factory() { return new TenantManagementService(core.ɵɵinject(ng_core.RestService)); }, token: TenantManagementService, providedIn: "root" });
        return TenantManagementService;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var TenantManagementState = /** @class */ (function () {
        function TenantManagementState(tenantManagementService) {
            this.tenantManagementService = tenantManagementService;
        }
        /**
         * @param {?} __0
         * @return {?}
         */
        TenantManagementState.get = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var result = _a.result;
            return result.items || [];
        };
        /**
         * @param {?} __0
         * @return {?}
         */
        TenantManagementState.prototype.get = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var patchState = _a.patchState;
            return this.tenantManagementService.get().pipe(operators.tap((/**
             * @param {?} result
             * @return {?}
             */
            function (result) {
                return patchState({
                    result: result,
                });
            })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        TenantManagementState.prototype.getById = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var patchState = _a.patchState;
            var payload = _b.payload;
            return this.tenantManagementService.getById(payload).pipe(operators.tap((/**
             * @param {?} selectedItem
             * @return {?}
             */
            function (selectedItem) {
                return patchState({
                    selectedItem: selectedItem,
                });
            })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        TenantManagementState.prototype.delete = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var dispatch = _a.dispatch;
            var payload = _b.payload;
            return this.tenantManagementService.delete(payload).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new TenantManagementGet()); })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        TenantManagementState.prototype.add = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var dispatch = _a.dispatch;
            var payload = _b.payload;
            return this.tenantManagementService.add(payload).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new TenantManagementGet()); })));
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        TenantManagementState.prototype.update = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var _this = this;
            var dispatch = _a.dispatch, getState = _a.getState;
            var payload = _b.payload;
            return dispatch(new TenantManagementGetById(payload.id)).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return _this.tenantManagementService.update(__assign({}, getState().selectedItem, payload)); })), operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new TenantManagementGet()); })));
        };
        __decorate([
            store.Action(TenantManagementGet),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", void 0)
        ], TenantManagementState.prototype, "get", null);
        __decorate([
            store.Action(TenantManagementGetById),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, TenantManagementGetById]),
            __metadata("design:returntype", void 0)
        ], TenantManagementState.prototype, "getById", null);
        __decorate([
            store.Action(TenantManagementDelete),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, TenantManagementDelete]),
            __metadata("design:returntype", void 0)
        ], TenantManagementState.prototype, "delete", null);
        __decorate([
            store.Action(TenantManagementAdd),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, TenantManagementAdd]),
            __metadata("design:returntype", void 0)
        ], TenantManagementState.prototype, "add", null);
        __decorate([
            store.Action(TenantManagementUpdate),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, TenantManagementUpdate]),
            __metadata("design:returntype", void 0)
        ], TenantManagementState.prototype, "update", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", Array)
        ], TenantManagementState, "get", null);
        TenantManagementState = __decorate([
            store.State({
                name: 'TenantManagementState',
                defaults: (/** @type {?} */ ({ result: {}, selectedItem: {} })),
            }),
            __metadata("design:paramtypes", [TenantManagementService])
        ], TenantManagementState);
        return TenantManagementState;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var TenantsComponent = /** @class */ (function () {
        function TenantsComponent(confirmationService, tenantService, modalService, fb, store) {
            this.confirmationService = confirmationService;
            this.tenantService = tenantService;
            this.modalService = modalService;
            this.fb = fb;
            this.store = store;
        }
        Object.defineProperty(TenantsComponent.prototype, "showInput", {
            get: /**
             * @return {?}
             */
            function () {
                return !this.defaultConnectionStringForm.get('useSharedDatabase').value;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(TenantsComponent.prototype, "connectionString", {
            get: /**
             * @return {?}
             */
            function () {
                return this.defaultConnectionStringForm.get('defaultConnectionString').value;
            },
            enumerable: true,
            configurable: true
        });
        /**
         * @return {?}
         */
        TenantsComponent.prototype.openModal = /**
         * @return {?}
         */
        function () {
            this.modalService.open(this.modalWrapper);
        };
        /**
         * @private
         * @return {?}
         */
        TenantsComponent.prototype.createTenantForm = /**
         * @private
         * @return {?}
         */
        function () {
            this.tenantForm = this.fb.group({
                name: [this.selected.name || '', [forms.Validators.required, forms.Validators.maxLength(256)]],
            });
        };
        /**
         * @private
         * @return {?}
         */
        TenantsComponent.prototype.createDefaultConnectionStringForm = /**
         * @private
         * @return {?}
         */
        function () {
            this.defaultConnectionStringForm = this.fb.group({
                useSharedDatabase: this.useSharedDatabase,
                defaultConnectionString: this.defaultConnectionString || '',
            });
        };
        /**
         * @param {?} id
         * @return {?}
         */
        TenantsComponent.prototype.onEditConnStr = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            var _this = this;
            this.selectedModalContent = {
                title: this.selected && this.selected.id ? 'AbpTenantManagement::Edit' : 'AbpTenantManagement::NewTenant',
                template: this.mTemplateConnStr,
                onSave: (/**
                 * @return {?}
                 */
                function () { return _this.saveConnStr; }),
            };
            this.store
                .dispatch(new TenantManagementGetById(id))
                .pipe(operators.pluck('TenantManagementState', 'selectedItem'), operators.switchMap((/**
             * @param {?} selected
             * @return {?}
             */
            function (selected) {
                _this.selected = selected;
                return _this.tenantService.getDefaultConnectionString(id);
            })))
                .subscribe((/**
             * @param {?} fetchedConnectionString
             * @return {?}
             */
            function (fetchedConnectionString) {
                _this.useSharedDatabase = fetchedConnectionString ? false : true;
                _this.defaultConnectionString = fetchedConnectionString ? fetchedConnectionString : '';
                _this.createDefaultConnectionStringForm();
                _this.openModal();
            }));
        };
        /**
         * @return {?}
         */
        TenantsComponent.prototype.saveConnStr = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.tenantService
                .updateDefaultConnectionString({ id: this.selected.id, defaultConnectionString: this.connectionString })
                .pipe(operators.take(1))
                .subscribe((/**
             * @return {?}
             */
            function () { return _this.modalService.dismissAll(); }));
        };
        /**
         * @param {?} id
         * @return {?}
         */
        TenantsComponent.prototype.onManageFeatures = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            this.selectedModalContent = {
                title: this.selected && this.selected.id ? 'AbpTenantManagement::Edit' : 'AbpTenantManagement::NewTenant',
                template: this.mTemplateFeatures,
                onSave: (/**
                 * @return {?}
                 */
                function () { }),
            };
            this.openModal();
        };
        /**
         * @return {?}
         */
        TenantsComponent.prototype.onAdd = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.selected = (/** @type {?} */ ({}));
            this.createTenantForm();
            this.openModal();
            this.selectedModalContent = {
                title: 'AbpTenantManagement::NewTenant',
                template: this.mTemplateTenant,
                onSave: (/**
                 * @return {?}
                 */
                function () { return _this.saveTenant; }),
            };
        };
        /**
         * @param {?} id
         * @return {?}
         */
        TenantsComponent.prototype.onEdit = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            var _this = this;
            this.store
                .dispatch(new TenantManagementGetById(id))
                .pipe(operators.pluck('TenantManagementState', 'selectedItem'))
                .subscribe((/**
             * @param {?} selected
             * @return {?}
             */
            function (selected) {
                _this.selected = selected;
                _this.selectedModalContent = {
                    title: 'AbpTenantManagement::Edit',
                    template: _this.mTemplateTenant,
                    onSave: (/**
                     * @return {?}
                     */
                    function () { return _this.saveTenant; }),
                };
                _this.createTenantForm();
                _this.openModal();
            }));
        };
        /**
         * @return {?}
         */
        TenantsComponent.prototype.saveTenant = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (!this.tenantForm.valid)
                return;
            this.store
                .dispatch(this.selected.id
                ? new TenantManagementUpdate(__assign({}, this.tenantForm.value, { id: this.selected.id }))
                : new TenantManagementAdd(this.tenantForm.value))
                .subscribe((/**
             * @return {?}
             */
            function () { return _this.modalService.dismissAll(); }));
        };
        /**
         * @param {?} id
         * @param {?} name
         * @return {?}
         */
        TenantsComponent.prototype.delete = /**
         * @param {?} id
         * @param {?} name
         * @return {?}
         */
        function (id, name) {
            var _this = this;
            this.confirmationService
                .warn('AbpTenantManagement::TenantDeletionConfirmationMessage', 'AbpTenantManagement::AreYouSure', {
                messageLocalizationParams: [name],
            })
                .subscribe((/**
             * @param {?} status
             * @return {?}
             */
            function (status) {
                if (status === "confirm" /* confirm */) {
                    _this.store.dispatch(new TenantManagementDelete(id));
                }
            }));
        };
        TenantsComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-tenants',
                        template: "<div id=\"wrapper\" class=\"card\">\n  <div class=\"card-header\">\n    <div class=\"row\">\n      <div class=\"col col-md-6\">\n        <h5 class=\"card-title\">\n          {{ 'AbpTenantManagement::Tenants' | abpLocalization }}\n        </h5>\n      </div>\n      <div class=\"text-right col col-md-6\">\n        <button\n          [abpPermission]=\"'AbpTenantManagement.Tenants.Create'\"\n          id=\"create-tenants\"\n          class=\"btn btn-primary\"\n          type=\"button\"\n          (click)=\"onAdd()\"\n        >\n          <i class=\"fa fa-plus mr-1\"></i>\n          <span>{{ 'AbpTenantManagement::NewTenant' | abpLocalization }}</span>\n        </button>\n      </div>\n    </div>\n  </div>\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          placeholder=\"Search\"\n          (input)=\"dt.filterGlobal($event.target.value, 'contains')\"\n      /></label>\n    </div>\n    <p-table #dt [value]=\"datas$ | async\" [globalFilterFields]=\"['name']\" [paginator]=\"true\" [rows]=\"10\">\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpTenantManagement::Actions' | abpLocalization }}</th>\n          <th>{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpTenantManagement::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Update'\"\n                  ngbDropdownItem\n                  (click)=\"onEdit(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::Edit' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageConnectionStrings'\"\n                  ngbDropdownItem\n                  (click)=\"onEditConnStr(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::ConnectionStrings' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageFeatures'\"\n                  ngbDropdownItem\n                  (click)=\"onManageFeatures(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::Features' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Delete'\"\n                  ngbDropdownItem\n                  (click)=\"delete(data.id, data.name)\"\n                >\n                  {{ 'AbpTenantManagement::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<ng-template #modalWrapper let-modal>\n  <div class=\"modal-header\">\n    <h5 class=\"modal-title\" id=\"modal-basic-title\">\n      {{ selectedModalContent.title | abpLocalization }}\n    </h5>\n    <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n      <span aria-hidden=\"true\">&times;</span>\n    </button>\n  </div>\n\n  <form [formGroup]=\"tenantForm\" (ngSubmit)=\"selectedModalContent.onSave()\">\n    <div class=\"modal-body\">\n      <ng-container *ngTemplateOutlet=\"selectedModalContent.template; context: { $implicit: modal }\"></ng-container>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpTenantManagement::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </form>\n</ng-template>\n\n<ng-template #mTemplateTenant let-modal>\n  <div class=\"mt-2\">\n    <div class=\"form-group\">\n      <label for=\"name\">{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</label>\n      <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n    </div>\n  </div>\n</ng-template>\n\n<ng-template #mTemplateConnStr let-modal>\n  <div class=\"mt-2\">\n    <div class=\"form-group\">\n      <div class=\"form-check\">\n        <input id=\"useSharedDatabase\" type=\"checkbox\" class=\"form-check-input\" formControlName=\"useSharedDatabase\" />\n        <label for=\"useSharedDatabase\" class=\"font-check-label\">{{\n          'AbpTenantManagement::DisplayName:UseSharedDatabase' | abpLocalization\n        }}</label>\n      </div>\n    </div>\n    <div class=\"form-group\" *ngIf=\"showInput\">\n      <label for=\"defaultConnectionString\">{{\n        'AbpTenantManagement::DisplayName:DefaultConnectionString' | abpLocalization\n      }}</label>\n      <input type=\"text\" id=\"defaultConnectionString\" class=\"form-control\" formControlName=\"defaultConnectionString\" />\n    </div>\n  </div>\n</ng-template>\n\n<ng-template #mTemplateFeatures let-modal>\n  Manage Features\n</ng-template>\n"
                    }] }
        ];
        /** @nocollapse */
        TenantsComponent.ctorParameters = function () { return [
            { type: ng_theme_shared.ConfirmationService },
            { type: TenantManagementService },
            { type: ngBootstrap.NgbModal },
            { type: forms.FormBuilder },
            { type: store.Store }
        ]; };
        TenantsComponent.propDecorators = {
            modalWrapper: [{ type: core.ViewChild, args: ['modalWrapper', { static: false },] }],
            mTemplateConnStr: [{ type: core.ViewChild, args: ['mTemplateConnStr', { static: false },] }],
            mTemplateFeatures: [{ type: core.ViewChild, args: ['mTemplateFeatures', { static: false },] }],
            mTemplateTenant: [{ type: core.ViewChild, args: ['mTemplateTenant', { static: false },] }]
        };
        __decorate([
            store.Select(TenantManagementState.get),
            __metadata("design:type", rxjs.Observable)
        ], TenantsComponent.prototype, "datas$", void 0);
        return TenantsComponent;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var TenantsResolver = /** @class */ (function () {
        function TenantsResolver(store) {
            this.store = store;
        }
        /**
         * @return {?}
         */
        TenantsResolver.prototype.resolve = /**
         * @return {?}
         */
        function () {
            /** @type {?} */
            var data = this.store.selectSnapshot(TenantManagementState.get);
            return data && data.length
                ? null
                : this.store.dispatch(new TenantManagementGet());
        };
        TenantsResolver.decorators = [
            { type: core.Injectable }
        ];
        /** @nocollapse */
        TenantsResolver.ctorParameters = function () { return [
            { type: store.Store }
        ]; };
        return TenantsResolver;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var ɵ0 = { requiredPolicy: 'AbpTenantManagement.Tenants' };
    /** @type {?} */
    var routes = [
        { path: '', redirectTo: 'tenants', pathMatch: 'full' },
        {
            path: 'tenants',
            component: ng_core.DynamicLayoutComponent,
            canActivate: [ng_core.AuthGuard, ng_core.PermissionGuard],
            data: ɵ0,
            children: [{ path: '', component: TenantsComponent, resolve: [TenantsResolver] }],
        },
    ];
    var TenantManagementRoutingModule = /** @class */ (function () {
        function TenantManagementRoutingModule() {
        }
        TenantManagementRoutingModule.decorators = [
            { type: core.NgModule, args: [{
                        imports: [router.RouterModule.forChild(routes)],
                        exports: [router.RouterModule],
                        providers: [TenantsResolver],
                    },] }
        ];
        return TenantManagementRoutingModule;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var TenantManagementModule = /** @class */ (function () {
        function TenantManagementModule() {
        }
        TenantManagementModule.decorators = [
            { type: core.NgModule, args: [{
                        declarations: [TenantsComponent],
                        imports: [
                            TenantManagementRoutingModule,
                            store.NgxsModule.forFeature([TenantManagementState]),
                            ng_core.CoreModule,
                            table.TableModule,
                            ng_theme_shared.ThemeSharedModule,
                            ngBootstrap.NgbDropdownModule,
                        ],
                    },] }
        ];
        return TenantManagementModule;
    }());

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    /** @type {?} */
    var TENANT_MANAGEMENT_ROUTES = (/** @type {?} */ ([
        {
            name: 'TenantManagement',
            path: 'tenant-management',
            parentName: 'Administration',
            layout: "application" /* application */,
            children: [
                {
                    path: 'tenants',
                    name: 'Tenants',
                    order: 1,
                    requiredPolicy: 'AbpTenantManagement.Tenants',
                    parentName: 'TenantManagement',
                },
            ],
        },
    ]));

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
     */
    var TenantManagement;
    (function (TenantManagement) {
        /**
         * @record
         */
        function State() { }
        TenantManagement.State = State;
        /**
         * @record
         */
        function Item() { }
        TenantManagement.Item = Item;
        /**
         * @record
         */
        function AddRequest() { }
        TenantManagement.AddRequest = AddRequest;
        /**
         * @record
         */
        function UpdateRequest() { }
        TenantManagement.UpdateRequest = UpdateRequest;
        /**
         * @record
         */
        function DefaultConnectionStringRequest() { }
        TenantManagement.DefaultConnectionStringRequest = DefaultConnectionStringRequest;
    })(TenantManagement || (TenantManagement = {}));

    exports.TENANT_MANAGEMENT_ROUTES = TENANT_MANAGEMENT_ROUTES;
    exports.TenantManagementAdd = TenantManagementAdd;
    exports.TenantManagementDelete = TenantManagementDelete;
    exports.TenantManagementGet = TenantManagementGet;
    exports.TenantManagementGetById = TenantManagementGetById;
    exports.TenantManagementModule = TenantManagementModule;
    exports.TenantManagementService = TenantManagementService;
    exports.TenantManagementState = TenantManagementState;
    exports.TenantManagementUpdate = TenantManagementUpdate;
    exports.TenantsComponent = TenantsComponent;
    exports.TenantsResolver = TenantsResolver;
    exports.ɵa = TenantsComponent;
    exports.ɵb = TenantManagementState;
    exports.ɵc = TenantManagementService;
    exports.ɵd = TenantManagementGet;
    exports.ɵe = TenantManagementGetById;
    exports.ɵf = TenantManagementAdd;
    exports.ɵg = TenantManagementUpdate;
    exports.ɵh = TenantManagementDelete;
    exports.ɵj = TenantManagementRoutingModule;
    exports.ɵk = TenantsResolver;

    Object.defineProperty(exports, '__esModule', { value: true });

}));
//# sourceMappingURL=abp-ng.tenant-management.umd.js.map
