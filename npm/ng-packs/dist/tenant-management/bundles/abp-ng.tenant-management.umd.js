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
    var GetTenants = /** @class */ (function () {
        function GetTenants(payload) {
            this.payload = payload;
        }
        GetTenants.type = '[TenantManagement] Get Tenant';
        return GetTenants;
    }());
    var GetTenantById = /** @class */ (function () {
        function GetTenantById(payload) {
            this.payload = payload;
        }
        GetTenantById.type = '[TenantManagement] Get Tenant By Id';
        return GetTenantById;
    }());
    var CreateTenant = /** @class */ (function () {
        function CreateTenant(payload) {
            this.payload = payload;
        }
        CreateTenant.type = '[TenantManagement] Create Tenant';
        return CreateTenant;
    }());
    var UpdateTenant = /** @class */ (function () {
        function UpdateTenant(payload) {
            this.payload = payload;
        }
        UpdateTenant.type = '[TenantManagement] Update Tenant';
        return UpdateTenant;
    }());
    var DeleteTenant = /** @class */ (function () {
        function DeleteTenant(payload) {
            this.payload = payload;
        }
        DeleteTenant.type = '[TenantManagement] Delete Tenant';
        return DeleteTenant;
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
         * @param {?=} params
         * @return {?}
         */
        TenantManagementService.prototype.getTenant = /**
         * @param {?=} params
         * @return {?}
         */
        function (params) {
            if (params === void 0) { params = (/** @type {?} */ ({})); }
            /** @type {?} */
            var request = {
                method: 'GET',
                url: '/api/multi-tenancy/tenants',
                params: params,
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} id
         * @return {?}
         */
        TenantManagementService.prototype.getTenantById = /**
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
        TenantManagementService.prototype.deleteTenant = /**
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
        TenantManagementService.prototype.createTenant = /**
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
        TenantManagementService.prototype.updateTenant = /**
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
            var url = "/api/multi-tenancy/tenants/" + id + "/default-connection-string";
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
            var url = "/api/multi-tenancy/tenants/" + payload.id + "/default-connection-string";
            /** @type {?} */
            var request = {
                method: 'PUT',
                url: url,
                params: { defaultConnectionString: payload.defaultConnectionString },
            };
            return this.rest.request(request);
        };
        /**
         * @param {?} id
         * @return {?}
         */
        TenantManagementService.prototype.deleteDefaultConnectionString = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            /** @type {?} */
            var url = "/api/multi-tenancy/tenant/" + id + "/default-connection-string";
            /** @type {?} */
            var request = {
                method: 'DELETE',
                url: url,
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
        TenantManagementState.getTenantsTotalCount = /**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var result = _a.result;
            return result.totalCount;
        };
        /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        TenantManagementState.prototype.get = /**
         * @param {?} __0
         * @param {?} __1
         * @return {?}
         */
        function (_a, _b) {
            var patchState = _a.patchState;
            var payload = _b.payload;
            return this.tenantManagementService.getTenant(payload).pipe(operators.tap((/**
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
            return this.tenantManagementService.getTenantById(payload).pipe(operators.tap((/**
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
            return this.tenantManagementService.deleteTenant(payload).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new GetTenants()); })));
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
            return this.tenantManagementService.createTenant(payload).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new GetTenants()); })));
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
            return dispatch(new GetTenantById(payload.id)).pipe(operators.switchMap((/**
             * @return {?}
             */
            function () { return _this.tenantManagementService.updateTenant(__assign({}, getState().selectedItem, payload)); })), operators.switchMap((/**
             * @return {?}
             */
            function () { return dispatch(new GetTenants()); })));
        };
        __decorate([
            store.Action(GetTenants),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, GetTenants]),
            __metadata("design:returntype", void 0)
        ], TenantManagementState.prototype, "get", null);
        __decorate([
            store.Action(GetTenantById),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, GetTenantById]),
            __metadata("design:returntype", void 0)
        ], TenantManagementState.prototype, "getById", null);
        __decorate([
            store.Action(DeleteTenant),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, DeleteTenant]),
            __metadata("design:returntype", void 0)
        ], TenantManagementState.prototype, "delete", null);
        __decorate([
            store.Action(CreateTenant),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, CreateTenant]),
            __metadata("design:returntype", void 0)
        ], TenantManagementState.prototype, "add", null);
        __decorate([
            store.Action(UpdateTenant),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object, UpdateTenant]),
            __metadata("design:returntype", void 0)
        ], TenantManagementState.prototype, "update", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", Array)
        ], TenantManagementState, "get", null);
        __decorate([
            store.Selector(),
            __metadata("design:type", Function),
            __metadata("design:paramtypes", [Object]),
            __metadata("design:returntype", Number)
        ], TenantManagementState, "getTenantsTotalCount", null);
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
        function TenantsComponent(confirmationService, tenantService, fb, store) {
            this.confirmationService = confirmationService;
            this.tenantService = tenantService;
            this.fb = fb;
            this.store = store;
            this.selectedModalContent = (/** @type {?} */ ({}));
            this.pageQuery = {
                sorting: 'name',
            };
            this.loading = false;
        }
        Object.defineProperty(TenantsComponent.prototype, "useSharedDatabase", {
            get: /**
             * @return {?}
             */
            function () {
                return this.defaultConnectionStringForm.get('useSharedDatabase').value;
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
         * @param {?} value
         * @return {?}
         */
        TenantsComponent.prototype.onSearch = /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this.pageQuery.filter = value;
            this.get();
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
                useSharedDatabase: this._useSharedDatabase,
                defaultConnectionString: this.defaultConnectionString || '',
            });
        };
        /**
         * @param {?} title
         * @param {?} template
         * @param {?} type
         * @return {?}
         */
        TenantsComponent.prototype.openModal = /**
         * @param {?} title
         * @param {?} template
         * @param {?} type
         * @return {?}
         */
        function (title, template, type) {
            this.selectedModalContent = {
                title: title,
                template: template,
                type: type,
            };
            this.isModalVisible = true;
        };
        /**
         * @param {?} id
         * @return {?}
         */
        TenantsComponent.prototype.onEditConnectionString = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            var _this = this;
            this.store
                .dispatch(new GetTenantById(id))
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
                _this._useSharedDatabase = fetchedConnectionString ? false : true;
                _this.defaultConnectionString = fetchedConnectionString ? fetchedConnectionString : '';
                _this.createDefaultConnectionStringForm();
                _this.openModal('AbpTenantManagement::ConnectionStrings', _this.connectionStringModalTemplate, 'saveConnStr');
            }));
        };
        /**
         * @return {?}
         */
        TenantsComponent.prototype.onAddTenant = /**
         * @return {?}
         */
        function () {
            this.selected = (/** @type {?} */ ({}));
            this.createTenantForm();
            this.openModal('AbpTenantManagement::NewTenant', this.tenantModalTemplate, 'saveTenant');
        };
        /**
         * @param {?} id
         * @return {?}
         */
        TenantsComponent.prototype.onEditTenant = /**
         * @param {?} id
         * @return {?}
         */
        function (id) {
            var _this = this;
            this.store
                .dispatch(new GetTenantById(id))
                .pipe(operators.pluck('TenantManagementState', 'selectedItem'))
                .subscribe((/**
             * @param {?} selected
             * @return {?}
             */
            function (selected) {
                _this.selected = selected;
                _this.createTenantForm();
                _this.openModal('AbpTenantManagement::Edit', _this.tenantModalTemplate, 'saveTenant');
            }));
        };
        /**
         * @return {?}
         */
        TenantsComponent.prototype.save = /**
         * @return {?}
         */
        function () {
            var type = this.selectedModalContent.type;
            if (!type)
                return;
            if (type === 'saveTenant')
                this.saveTenant();
            else if (type === 'saveConnStr')
                this.saveConnectionString();
        };
        /**
         * @return {?}
         */
        TenantsComponent.prototype.saveConnectionString = /**
         * @return {?}
         */
        function () {
            var _this = this;
            if (this.useSharedDatabase) {
                this.tenantService
                    .deleteDefaultConnectionString(this.selected.id)
                    .pipe(operators.take(1))
                    .subscribe((/**
                 * @return {?}
                 */
                function () {
                    _this.isModalVisible = false;
                }));
            }
            else {
                this.tenantService
                    .updateDefaultConnectionString({ id: this.selected.id, defaultConnectionString: this.connectionString })
                    .pipe(operators.take(1))
                    .subscribe((/**
                 * @return {?}
                 */
                function () {
                    _this.isModalVisible = false;
                }));
            }
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
                ? new UpdateTenant(__assign({}, this.tenantForm.value, { id: this.selected.id }))
                : new CreateTenant(this.tenantForm.value))
                .subscribe((/**
             * @return {?}
             */
            function () {
                _this.isModalVisible = false;
            }));
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
                    _this.store.dispatch(new DeleteTenant(id));
                }
            }));
        };
        /**
         * @param {?} data
         * @return {?}
         */
        TenantsComponent.prototype.onPageChange = /**
         * @param {?} data
         * @return {?}
         */
        function (data) {
            this.pageQuery.skipCount = data.first;
            this.pageQuery.maxResultCount = data.rows;
            this.get();
        };
        /**
         * @return {?}
         */
        TenantsComponent.prototype.get = /**
         * @return {?}
         */
        function () {
            var _this = this;
            this.loading = true;
            this.store
                .dispatch(new GetTenants(this.pageQuery))
                .pipe(operators.finalize((/**
             * @return {?}
             */
            function () { return (_this.loading = false); })))
                .subscribe();
        };
        TenantsComponent.decorators = [
            { type: core.Component, args: [{
                        selector: 'abp-tenants',
                        template: "<div id=\"wrapper\" class=\"card\">\n  <div class=\"card-header\">\n    <div class=\"row\">\n      <div class=\"col col-md-6\">\n        <h5 class=\"card-title\">\n          {{ 'AbpTenantManagement::Tenants' | abpLocalization }}\n        </h5>\n      </div>\n      <div class=\"text-right col col-md-6\">\n        <button\n          [abpPermission]=\"'AbpTenantManagement.Tenants.Create'\"\n          id=\"create-tenants\"\n          class=\"btn btn-primary\"\n          type=\"button\"\n          (click)=\"onAddTenant()\"\n        >\n          <i class=\"fa fa-plus mr-1\"></i>\n          <span>{{ 'AbpTenantManagement::NewTenant' | abpLocalization }}</span>\n        </button>\n      </div>\n    </div>\n  </div>\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          [placeholder]=\"'AbpUi::PagerSearch' | abpLocalization\"\n          (input.debounce)=\"onSearch($event.target.value)\"\n      /></label>\n    </div>\n    <p-table\n      [value]=\"data$ | async\"\n      [lazy]=\"true\"\n      [lazyLoadOnInit]=\"false\"\n      [paginator]=\"true\"\n      [rows]=\"10\"\n      [totalRecords]=\"totalCount$ | async\"\n      [loading]=\"loading\"\n      (onLazyLoad)=\"onPageChange($event)\"\n    >\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpTenantManagement::Actions' | abpLocalization }}</th>\n          <th>{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpTenantManagement::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Update'\"\n                  ngbDropdownItem\n                  (click)=\"onEditTenant(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::Edit' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageConnectionStrings'\"\n                  ngbDropdownItem\n                  (click)=\"onEditConnectionString(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::ConnectionStrings' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Delete'\"\n                  ngbDropdownItem\n                  (click)=\"delete(data.id, data.name)\"\n                >\n                  {{ 'AbpTenantManagement::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal [(visible)]=\"isModalVisible\">\n  <ng-template #abpHeader>\n    <h3>{{ selectedModalContent.title | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <ng-container *ngTemplateOutlet=\"selectedModalContent?.template\"></ng-container>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n    </button>\n    <abp-button\n      [requestType]=\"['POST', 'PUT', 'DELETE']\"\n      requestURLContainSearchValue=\"multi-tenancy\"\n      iconClass=\"fa fa-check\"\n      (click)=\"save()\"\n      >{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button\n    >\n  </ng-template>\n</abp-modal>\n\n<ng-template #tenantModalTemplate>\n  <form [formGroup]=\"tenantForm\" (ngSubmit)=\"save()\">\n    <div class=\"mt-2\">\n      <div class=\"form-group\">\n        <label for=\"name\">{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</label>\n        <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" autofocus />\n      </div>\n    </div>\n  </form>\n</ng-template>\n\n<ng-template #connectionStringModalTemplate>\n  <form [formGroup]=\"defaultConnectionStringForm\" (ngSubmit)=\"save()\">\n    <div class=\"mt-2\">\n      <div class=\"form-group\">\n        <div class=\"form-check\">\n          <input\n            id=\"useSharedDatabase\"\n            type=\"checkbox\"\n            class=\"form-check-input\"\n            formControlName=\"useSharedDatabase\"\n            autofocus\n          />\n          <label for=\"useSharedDatabase\" class=\"font-check-label\">{{\n            'AbpTenantManagement::DisplayName:UseSharedDatabase' | abpLocalization\n          }}</label>\n        </div>\n      </div>\n      <div class=\"form-group\" *ngIf=\"!useSharedDatabase\">\n        <label for=\"defaultConnectionString\">{{\n          'AbpTenantManagement::DisplayName:DefaultConnectionString' | abpLocalization\n        }}</label>\n        <input\n          type=\"text\"\n          id=\"defaultConnectionString\"\n          class=\"form-control\"\n          formControlName=\"defaultConnectionString\"\n        />\n      </div>\n    </div>\n  </form>\n</ng-template>\n"
                    }] }
        ];
        /** @nocollapse */
        TenantsComponent.ctorParameters = function () { return [
            { type: ng_theme_shared.ConfirmationService },
            { type: TenantManagementService },
            { type: forms.FormBuilder },
            { type: store.Store }
        ]; };
        TenantsComponent.propDecorators = {
            tenantModalTemplate: [{ type: core.ViewChild, args: ['tenantModalTemplate', { static: false },] }],
            connectionStringModalTemplate: [{ type: core.ViewChild, args: ['connectionStringModalTemplate', { static: false },] }]
        };
        __decorate([
            store.Select(TenantManagementState.get),
            __metadata("design:type", rxjs.Observable)
        ], TenantsComponent.prototype, "data$", void 0);
        __decorate([
            store.Select(TenantManagementState.getTenantsTotalCount),
            __metadata("design:type", rxjs.Observable)
        ], TenantsComponent.prototype, "totalCount$", void 0);
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
            return data && data.length ? null : this.store.dispatch(new GetTenants());
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
            name: 'AbpTenantManagement::Menu:TenantManagement',
            path: 'tenant-management',
            parentName: 'AbpUiNavigation::Menu:Administration',
            layout: "application" /* application */,
            iconClass: 'fa fa-users',
            children: [
                {
                    path: 'tenants',
                    name: 'AbpTenantManagement::Tenants',
                    order: 1,
                    requiredPolicy: 'AbpTenantManagement.Tenants',
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

    exports.CreateTenant = CreateTenant;
    exports.DeleteTenant = DeleteTenant;
    exports.GetTenantById = GetTenantById;
    exports.GetTenants = GetTenants;
    exports.TENANT_MANAGEMENT_ROUTES = TENANT_MANAGEMENT_ROUTES;
    exports.TenantManagementModule = TenantManagementModule;
    exports.TenantManagementService = TenantManagementService;
    exports.TenantManagementState = TenantManagementState;
    exports.TenantsComponent = TenantsComponent;
    exports.TenantsResolver = TenantsResolver;
    exports.UpdateTenant = UpdateTenant;
    exports.ɵa = TenantsComponent;
    exports.ɵb = TenantManagementState;
    exports.ɵc = TenantManagementService;
    exports.ɵd = GetTenants;
    exports.ɵe = GetTenantById;
    exports.ɵf = CreateTenant;
    exports.ɵg = UpdateTenant;
    exports.ɵh = DeleteTenant;
    exports.ɵj = TenantManagementRoutingModule;
    exports.ɵk = TenantsResolver;

    Object.defineProperty(exports, '__esModule', { value: true });

}));
//# sourceMappingURL=abp-ng.tenant-management.umd.js.map
