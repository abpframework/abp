import { RestService, DynamicLayoutComponent, AuthGuard, PermissionGuard, CoreModule } from '@abp/ng.core';
import { ConfirmationService, ThemeSharedModule } from '@abp/ng.theme.shared';
import { Injectable, ɵɵdefineInjectable, ɵɵinject, Component, ViewChild, NgModule } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { Action, Selector, State, Store, Select, NgxsModule } from '@ngxs/store';
import { TableModule } from 'primeng/table';
import { __assign, __decorate, __metadata } from 'tslib';
import { Validators, FormBuilder } from '@angular/forms';
import { Observable } from 'rxjs';
import { tap, switchMap, pluck, take, finalize } from 'rxjs/operators';
import { RouterModule } from '@angular/router';
import { FeatureManagementModule } from '@abp/ng.feature-management';
import { NgxValidateCoreModule } from '@ngx-validate/core';

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
if (false) {
    /** @type {?} */
    GetTenants.type;
    /** @type {?} */
    GetTenants.prototype.payload;
}
var GetTenantById = /** @class */ (function () {
    function GetTenantById(payload) {
        this.payload = payload;
    }
    GetTenantById.type = '[TenantManagement] Get Tenant By Id';
    return GetTenantById;
}());
if (false) {
    /** @type {?} */
    GetTenantById.type;
    /** @type {?} */
    GetTenantById.prototype.payload;
}
var CreateTenant = /** @class */ (function () {
    function CreateTenant(payload) {
        this.payload = payload;
    }
    CreateTenant.type = '[TenantManagement] Create Tenant';
    return CreateTenant;
}());
if (false) {
    /** @type {?} */
    CreateTenant.type;
    /** @type {?} */
    CreateTenant.prototype.payload;
}
var UpdateTenant = /** @class */ (function () {
    function UpdateTenant(payload) {
        this.payload = payload;
    }
    UpdateTenant.type = '[TenantManagement] Update Tenant';
    return UpdateTenant;
}());
if (false) {
    /** @type {?} */
    UpdateTenant.type;
    /** @type {?} */
    UpdateTenant.prototype.payload;
}
var DeleteTenant = /** @class */ (function () {
    function DeleteTenant(payload) {
        this.payload = payload;
    }
    DeleteTenant.type = '[TenantManagement] Delete Tenant';
    return DeleteTenant;
}());
if (false) {
    /** @type {?} */
    DeleteTenant.type;
    /** @type {?} */
    DeleteTenant.prototype.payload;
}

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
        var url = "/api/multi-tenancy/tenants/" + id + "/default-connection-string";
        /** @type {?} */
        var request = {
            method: 'DELETE',
            url: url,
        };
        return this.rest.request(request);
    };
    TenantManagementService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    TenantManagementService.ctorParameters = function () { return [
        { type: RestService }
    ]; };
    /** @nocollapse */ TenantManagementService.ngInjectableDef = ɵɵdefineInjectable({ factory: function TenantManagementService_Factory() { return new TenantManagementService(ɵɵinject(RestService)); }, token: TenantManagementService, providedIn: "root" });
    return TenantManagementService;
}());
if (false) {
    /**
     * @type {?}
     * @private
     */
    TenantManagementService.prototype.rest;
}

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
        return this.tenantManagementService.getTenant(payload).pipe(tap((/**
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
        return this.tenantManagementService.getTenantById(payload).pipe(tap((/**
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
        return this.tenantManagementService.deleteTenant(payload).pipe(switchMap((/**
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
        return this.tenantManagementService.createTenant(payload).pipe(switchMap((/**
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
        return dispatch(new GetTenantById(payload.id)).pipe(switchMap((/**
         * @return {?}
         */
        function () { return _this.tenantManagementService.updateTenant(__assign({}, getState().selectedItem, payload)); })), switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new GetTenants()); })));
    };
    __decorate([
        Action(GetTenants),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, GetTenants]),
        __metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "get", null);
    __decorate([
        Action(GetTenantById),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, GetTenantById]),
        __metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "getById", null);
    __decorate([
        Action(DeleteTenant),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, DeleteTenant]),
        __metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "delete", null);
    __decorate([
        Action(CreateTenant),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, CreateTenant]),
        __metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "add", null);
    __decorate([
        Action(UpdateTenant),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, UpdateTenant]),
        __metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "update", null);
    __decorate([
        Selector(),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object]),
        __metadata("design:returntype", Array)
    ], TenantManagementState, "get", null);
    __decorate([
        Selector(),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object]),
        __metadata("design:returntype", Number)
    ], TenantManagementState, "getTenantsTotalCount", null);
    TenantManagementState = __decorate([
        State({
            name: 'TenantManagementState',
            defaults: (/** @type {?} */ ({ result: {}, selectedItem: {} })),
        }),
        __metadata("design:paramtypes", [TenantManagementService])
    ], TenantManagementState);
    return TenantManagementState;
}());
if (false) {
    /**
     * @type {?}
     * @private
     */
    TenantManagementState.prototype.tenantManagementService;
}

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
        this.visibleFeatures = false;
        this.pageQuery = {
            sorting: 'name',
        };
        this.loading = false;
        this.modalBusy = false;
        this.sortOrder = 'asc';
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
            name: [this.selected.name || '', [Validators.required, Validators.maxLength(256)]],
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
            defaultConnectionString: [this.defaultConnectionString || ''],
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
            .pipe(pluck('TenantManagementState', 'selectedItem'), switchMap((/**
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
            .pipe(pluck('TenantManagementState', 'selectedItem'))
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
        this.modalBusy = true;
        if (this.useSharedDatabase || (!this.useSharedDatabase && !this.connectionString)) {
            this.tenantService
                .deleteDefaultConnectionString(this.selected.id)
                .pipe(take(1), finalize((/**
             * @return {?}
             */
            function () { return (_this.modalBusy = false); })))
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
                .pipe(take(1), finalize((/**
             * @return {?}
             */
            function () { return (_this.modalBusy = false); })))
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
        this.modalBusy = true;
        this.store
            .dispatch(this.selected.id
            ? new UpdateTenant(__assign({}, this.tenantForm.value, { id: this.selected.id }))
            : new CreateTenant(this.tenantForm.value))
            .pipe(finalize((/**
         * @return {?}
         */
        function () { return (_this.modalBusy = false); })))
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
            .pipe(finalize((/**
         * @return {?}
         */
        function () { return (_this.loading = false); })))
            .subscribe();
    };
    /**
     * @return {?}
     */
    TenantsComponent.prototype.changeSortOrder = /**
     * @return {?}
     */
    function () {
        this.sortOrder = this.sortOrder.toLowerCase() === "asc" ? "desc" : "asc";
    };
    TenantsComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-tenants',
                    template: "<div class=\"row entry-row\">\n  <div class=\"col-auto\">\n    <h1 class=\"content-header-title\">{{ 'AbpTenantManagement::Tenants' | abpLocalization }}</h1>\n  </div>\n  <div class=\"col\">\n    <div class=\"text-lg-right pt-2\" id=\"AbpContentToolbar\">\n      <button\n        [abpPermission]=\"'AbpTenantManagement.Tenants.Create'\"\n        id=\"create-tenants\"\n        class=\"btn btn-primary\"\n        type=\"button\"\n        (click)=\"onAddTenant()\"\n      >\n        <i class=\"fa fa-plus mr-1\"></i>\n        <span>{{ 'AbpTenantManagement::NewTenant' | abpLocalization }}</span>\n      </button>\n    </div>\n  </div>\n</div>\n\n<div id=\"wrapper\" class=\"card\">\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          [placeholder]=\"'AbpUi::PagerSearch' | abpLocalization\"\n          (input.debounce)=\"onSearch($event.target.value)\"\n      /></label>\n    </div>\n    <p-table\n      *ngIf=\"[130, 200] as columnWidths\"\n      [value]=\"data$ | async | abpSort: sortOrder\"\n      [lazy]=\"true\"\n      [lazyLoadOnInit]=\"false\"\n      [paginator]=\"true\"\n      [rows]=\"10\"\n      [totalRecords]=\"totalCount$ | async\"\n      [loading]=\"loading\"\n      [resizableColumns]=\"true\"\n      [scrollable]=\"true\"\n      (onLazyLoad)=\"onPageChange($event)\"\n    >\n      <ng-template pTemplate=\"colgroup\">\n        <colgroup>\n          <col *ngFor=\"let width of columnWidths\" [ngStyle]=\"{ 'width.px': width }\" />\n        </colgroup>\n      </ng-template>\n      <ng-template pTemplate=\"emptymessage\" let-columns>\n        <tr\n          abp-table-empty-message\n          [attr.colspan]=\"columnWidths.length\"\n          localizationResource=\"AbpTenantManagement\"\n          localizationProp=\"NoDataAvailableInDatatable\"\n        ></tr>\n      </ng-template>\n      <ng-template pTemplate=\"header\" let-columns>\n        <tr>\n          <th>{{ 'AbpTenantManagement::Actions' | abpLocalization }}</th>\n          <th pResizableColumn (click)=\"changeSortOrder()\">\n            {{ 'AbpTenantManagement::TenantName' | abpLocalization }}\n            <span class=\"float-right\"\n              ><i [ngClass]=\"['fa', sortOrder === 'desc' ? 'fa-sort-desc' : 'fa-sort-asc']\"></i\n            ></span>\n          </th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown container=\"body\" class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpTenantManagement::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Update'\"\n                  ngbDropdownItem\n                  (click)=\"onEditTenant(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::Edit' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageConnectionStrings'\"\n                  ngbDropdownItem\n                  (click)=\"onEditConnectionString(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::Permission:ManageConnectionStrings' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageFeatures'\"\n                  ngbDropdownItem\n                  (click)=\"providerKey = data.id; visibleFeatures = true\"\n                >\n                  {{ 'AbpTenantManagement::Permission:ManageFeatures' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Delete'\"\n                  ngbDropdownItem\n                  (click)=\"delete(data.id, data.name)\"\n                >\n                  {{ 'AbpTenantManagement::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal size=\"md\" [(visible)]=\"isModalVisible\" [busy]=\"modalBusy\">\n  <ng-template #abpHeader>\n    <h3>{{ selectedModalContent.title | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <ng-container *ngTemplateOutlet=\"selectedModalContent?.template\"></ng-container>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n    </button>\n    <abp-button iconClass=\"fa fa-check\" (click)=\"save()\">{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button>\n  </ng-template>\n</abp-modal>\n\n<ng-template #tenantModalTemplate>\n  <form [formGroup]=\"tenantForm\" (ngSubmit)=\"save()\">\n    <div class=\"mt-2\">\n      <div class=\"form-group\">\n        <label for=\"name\">{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</label>\n        <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" autofocus />\n      </div>\n    </div>\n  </form>\n</ng-template>\n\n<ng-template #connectionStringModalTemplate>\n  <form [formGroup]=\"defaultConnectionStringForm\" (ngSubmit)=\"save()\">\n    <label class=\"mt-2\">\n      <div class=\"form-group\">\n        <div class=\"custom-checkbox custom-control mb-2\">\n          <input\n            id=\"useSharedDatabase\"\n            type=\"checkbox\"\n            class=\"custom-control-input\"\n            formControlName=\"useSharedDatabase\"\n            autofocus\n          />\n          <label for=\"useSharedDatabase\" class=\"custom-control-label\">{{\n            'AbpTenantManagement::DisplayName:UseSharedDatabase' | abpLocalization\n          }}</label>\n        </div>\n      </div>\n      <label class=\"form-group\" *ngIf=\"!useSharedDatabase\">\n        <label for=\"defaultConnectionString\">{{\n          'AbpTenantManagement::DisplayName:DefaultConnectionString' | abpLocalization\n        }}</label>\n        <input\n          type=\"text\"\n          id=\"defaultConnectionString\"\n          class=\"form-control\"\n          formControlName=\"defaultConnectionString\"\n        />\n      </label>\n    </label>\n  </form>\n</ng-template>\n\n<abp-feature-management\n  [(visible)]=\"visibleFeatures\"\n  providerName=\"Tenant\"\n  [providerKey]=\"providerKey\"\n></abp-feature-management>\n"
                }] }
    ];
    /** @nocollapse */
    TenantsComponent.ctorParameters = function () { return [
        { type: ConfirmationService },
        { type: TenantManagementService },
        { type: FormBuilder },
        { type: Store }
    ]; };
    TenantsComponent.propDecorators = {
        tenantModalTemplate: [{ type: ViewChild, args: ['tenantModalTemplate', { static: false },] }],
        connectionStringModalTemplate: [{ type: ViewChild, args: ['connectionStringModalTemplate', { static: false },] }]
    };
    __decorate([
        Select(TenantManagementState.get),
        __metadata("design:type", Observable)
    ], TenantsComponent.prototype, "data$", void 0);
    __decorate([
        Select(TenantManagementState.getTenantsTotalCount),
        __metadata("design:type", Observable)
    ], TenantsComponent.prototype, "totalCount$", void 0);
    return TenantsComponent;
}());
if (false) {
    /** @type {?} */
    TenantsComponent.prototype.data$;
    /** @type {?} */
    TenantsComponent.prototype.totalCount$;
    /** @type {?} */
    TenantsComponent.prototype.selected;
    /** @type {?} */
    TenantsComponent.prototype.tenantForm;
    /** @type {?} */
    TenantsComponent.prototype.defaultConnectionStringForm;
    /** @type {?} */
    TenantsComponent.prototype.defaultConnectionString;
    /** @type {?} */
    TenantsComponent.prototype.isModalVisible;
    /** @type {?} */
    TenantsComponent.prototype.selectedModalContent;
    /** @type {?} */
    TenantsComponent.prototype.visibleFeatures;
    /** @type {?} */
    TenantsComponent.prototype.providerKey;
    /** @type {?} */
    TenantsComponent.prototype._useSharedDatabase;
    /** @type {?} */
    TenantsComponent.prototype.pageQuery;
    /** @type {?} */
    TenantsComponent.prototype.loading;
    /** @type {?} */
    TenantsComponent.prototype.modalBusy;
    /** @type {?} */
    TenantsComponent.prototype.sortOrder;
    /** @type {?} */
    TenantsComponent.prototype.tenantModalTemplate;
    /** @type {?} */
    TenantsComponent.prototype.connectionStringModalTemplate;
    /**
     * @type {?}
     * @private
     */
    TenantsComponent.prototype.confirmationService;
    /**
     * @type {?}
     * @private
     */
    TenantsComponent.prototype.tenantService;
    /**
     * @type {?}
     * @private
     */
    TenantsComponent.prototype.fb;
    /**
     * @type {?}
     * @private
     */
    TenantsComponent.prototype.store;
}

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
        { type: Injectable }
    ];
    /** @nocollapse */
    TenantsResolver.ctorParameters = function () { return [
        { type: Store }
    ]; };
    return TenantsResolver;
}());
if (false) {
    /**
     * @type {?}
     * @private
     */
    TenantsResolver.prototype.store;
}

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
        component: DynamicLayoutComponent,
        canActivate: [AuthGuard, PermissionGuard],
        data: ɵ0,
        children: [{ path: '', component: TenantsComponent, resolve: [TenantsResolver] }],
    },
];
var TenantManagementRoutingModule = /** @class */ (function () {
    function TenantManagementRoutingModule() {
    }
    TenantManagementRoutingModule.decorators = [
        { type: NgModule, args: [{
                    imports: [RouterModule.forChild(routes)],
                    exports: [RouterModule],
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
        { type: NgModule, args: [{
                    declarations: [TenantsComponent],
                    imports: [
                        TenantManagementRoutingModule,
                        NgxsModule.forFeature([TenantManagementState]),
                        NgxValidateCoreModule,
                        CoreModule,
                        TableModule,
                        ThemeSharedModule,
                        NgbDropdownModule,
                        FeatureManagementModule,
                    ],
                },] }
    ];
    return TenantManagementModule;
}());
/**
 * @return {?}
 */
function TenantManagementProviders() {
    return [];
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
var TENANT_MANAGEMENT_ROUTES = {
    routes: (/** @type {?} */ ([
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
    ])),
};

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

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
    if (false) {
        /** @type {?} */
        State.prototype.result;
        /** @type {?} */
        State.prototype.selectedItem;
    }
    /**
     * @record
     */
    function Item() { }
    TenantManagement.Item = Item;
    if (false) {
        /** @type {?} */
        Item.prototype.id;
        /** @type {?} */
        Item.prototype.name;
    }
    /**
     * @record
     */
    function AddRequest() { }
    TenantManagement.AddRequest = AddRequest;
    if (false) {
        /** @type {?} */
        AddRequest.prototype.name;
    }
    /**
     * @record
     */
    function UpdateRequest() { }
    TenantManagement.UpdateRequest = UpdateRequest;
    if (false) {
        /** @type {?} */
        UpdateRequest.prototype.id;
    }
    /**
     * @record
     */
    function DefaultConnectionStringRequest() { }
    TenantManagement.DefaultConnectionStringRequest = DefaultConnectionStringRequest;
    if (false) {
        /** @type {?} */
        DefaultConnectionStringRequest.prototype.id;
        /** @type {?} */
        DefaultConnectionStringRequest.prototype.defaultConnectionString;
    }
})(TenantManagement || (TenantManagement = {}));

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

export { CreateTenant, DeleteTenant, GetTenantById, GetTenants, TENANT_MANAGEMENT_ROUTES, TenantManagementModule, TenantManagementProviders, TenantManagementService, TenantManagementState, TenantsComponent, TenantsResolver, UpdateTenant, TenantsComponent as ɵa, TenantManagementState as ɵb, TenantManagementService as ɵc, GetTenants as ɵd, GetTenantById as ɵe, CreateTenant as ɵf, UpdateTenant as ɵg, DeleteTenant as ɵh, TenantManagementRoutingModule as ɵj, TenantsResolver as ɵk };
//# sourceMappingURL=abp-ng.tenant-management.js.map
