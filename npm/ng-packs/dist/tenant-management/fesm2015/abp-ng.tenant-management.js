import { RestService, DynamicLayoutComponent, AuthGuard, PermissionGuard, CoreModule } from '@abp/ng.core';
import { ConfirmationService, ThemeSharedModule } from '@abp/ng.theme.shared';
import { Injectable, ɵɵdefineInjectable, ɵɵinject, Component, ViewChild, NgModule } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { Action, Selector, State, Store, Select, NgxsModule } from '@ngxs/store';
import { TableModule } from 'primeng/table';
import { __decorate, __metadata } from 'tslib';
import { Validators, FormBuilder } from '@angular/forms';
import { Observable } from 'rxjs';
import { tap, switchMap, pluck, take, finalize } from 'rxjs/operators';
import { RouterModule } from '@angular/router';
import { FeatureManagementModule } from '@abp/ng.feature-management';
import { NgxValidateCoreModule } from '@ngx-validate/core';

/**
 * @fileoverview added by tsickle
 * Generated from: lib/actions/tenant-management.actions.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class GetTenants {
    /**
     * @param {?=} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
GetTenants.type = '[TenantManagement] Get Tenant';
if (false) {
    /** @type {?} */
    GetTenants.type;
    /** @type {?} */
    GetTenants.prototype.payload;
}
class GetTenantById {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
GetTenantById.type = '[TenantManagement] Get Tenant By Id';
if (false) {
    /** @type {?} */
    GetTenantById.type;
    /** @type {?} */
    GetTenantById.prototype.payload;
}
class CreateTenant {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
CreateTenant.type = '[TenantManagement] Create Tenant';
if (false) {
    /** @type {?} */
    CreateTenant.type;
    /** @type {?} */
    CreateTenant.prototype.payload;
}
class UpdateTenant {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
UpdateTenant.type = '[TenantManagement] Update Tenant';
if (false) {
    /** @type {?} */
    UpdateTenant.type;
    /** @type {?} */
    UpdateTenant.prototype.payload;
}
class DeleteTenant {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
DeleteTenant.type = '[TenantManagement] Delete Tenant';
if (false) {
    /** @type {?} */
    DeleteTenant.type;
    /** @type {?} */
    DeleteTenant.prototype.payload;
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/tenant-management.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class TenantManagementService {
    /**
     * @param {?} rest
     */
    constructor(rest) {
        this.rest = rest;
    }
    /**
     * @param {?=} params
     * @return {?}
     */
    getTenant(params = (/** @type {?} */ ({}))) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: '/api/multi-tenancy/tenants',
            params
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    getTenantById(id) {
        /** @type {?} */
        const request = {
            method: 'GET',
            url: `/api/multi-tenancy/tenants/${id}`
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    deleteTenant(id) {
        /** @type {?} */
        const request = {
            method: 'DELETE',
            url: `/api/multi-tenancy/tenants/${id}`
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} body
     * @return {?}
     */
    createTenant(body) {
        /** @type {?} */
        const request = {
            method: 'POST',
            url: '/api/multi-tenancy/tenants',
            body
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} body
     * @return {?}
     */
    updateTenant(body) {
        /** @type {?} */
        const url = `/api/multi-tenancy/tenants/${body.id}`;
        delete body.id;
        /** @type {?} */
        const request = {
            method: 'PUT',
            url,
            body
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    getDefaultConnectionString(id) {
        /** @type {?} */
        const url = `/api/multi-tenancy/tenants/${id}/default-connection-string`;
        /** @type {?} */
        const request = {
            method: 'GET',
            responseType: "text" /* Text */,
            url
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} payload
     * @return {?}
     */
    updateDefaultConnectionString(payload) {
        /** @type {?} */
        const url = `/api/multi-tenancy/tenants/${payload.id}/default-connection-string`;
        /** @type {?} */
        const request = {
            method: 'PUT',
            url,
            params: { defaultConnectionString: payload.defaultConnectionString }
        };
        return this.rest.request(request);
    }
    /**
     * @param {?} id
     * @return {?}
     */
    deleteDefaultConnectionString(id) {
        /** @type {?} */
        const url = `/api/multi-tenancy/tenants/${id}/default-connection-string`;
        /** @type {?} */
        const request = {
            method: 'DELETE',
            url
        };
        return this.rest.request(request);
    }
}
TenantManagementService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root'
            },] }
];
/** @nocollapse */
TenantManagementService.ctorParameters = () => [
    { type: RestService }
];
/** @nocollapse */ TenantManagementService.ngInjectableDef = ɵɵdefineInjectable({ factory: function TenantManagementService_Factory() { return new TenantManagementService(ɵɵinject(RestService)); }, token: TenantManagementService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    TenantManagementService.prototype.rest;
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/tenant-management.state.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
let TenantManagementState = class TenantManagementState {
    /**
     * @param {?} tenantManagementService
     */
    constructor(tenantManagementService) {
        this.tenantManagementService = tenantManagementService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static get({ result }) {
        return result.items || [];
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getTenantsTotalCount({ result }) {
        return result.totalCount;
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    get({ patchState }, { payload }) {
        return this.tenantManagementService.getTenant(payload).pipe(tap((/**
         * @param {?} result
         * @return {?}
         */
        result => patchState({
            result,
        }))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    getById({ patchState }, { payload }) {
        return this.tenantManagementService.getTenantById(payload).pipe(tap((/**
         * @param {?} selectedItem
         * @return {?}
         */
        selectedItem => patchState({
            selectedItem,
        }))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    delete({ dispatch }, { payload }) {
        return this.tenantManagementService.deleteTenant(payload).pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new GetTenants()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    add({ dispatch }, { payload }) {
        return this.tenantManagementService.createTenant(payload).pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new GetTenants()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    update({ dispatch, getState }, { payload }) {
        return this.tenantManagementService
            .updateTenant(Object.assign({}, getState().selectedItem, payload))
            .pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new GetTenants()))));
    }
};
TenantManagementState.ctorParameters = () => [
    { type: TenantManagementService }
];
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
if (false) {
    /**
     * @type {?}
     * @private
     */
    TenantManagementState.prototype.tenantManagementService;
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/tenants/tenants.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/**
 * @record
 */
function SelectedModalContent() { }
if (false) {
    /** @type {?} */
    SelectedModalContent.prototype.type;
    /** @type {?} */
    SelectedModalContent.prototype.title;
    /** @type {?} */
    SelectedModalContent.prototype.template;
}
class TenantsComponent {
    /**
     * @param {?} confirmationService
     * @param {?} tenantService
     * @param {?} fb
     * @param {?} store
     */
    constructor(confirmationService, tenantService, fb, store) {
        this.confirmationService = confirmationService;
        this.tenantService = tenantService;
        this.fb = fb;
        this.store = store;
        this.selectedModalContent = (/** @type {?} */ ({}));
        this.visibleFeatures = false;
        this.pageQuery = {};
        this.loading = false;
        this.modalBusy = false;
        this.sortOrder = '';
        this.sortKey = '';
    }
    /**
     * @return {?}
     */
    get useSharedDatabase() {
        return this.defaultConnectionStringForm.get('useSharedDatabase').value;
    }
    /**
     * @return {?}
     */
    get connectionString() {
        return this.defaultConnectionStringForm.get('defaultConnectionString').value;
    }
    /**
     * @return {?}
     */
    get isDisabledSaveButton() {
        if (!this.selectedModalContent)
            return false;
        if (this.selectedModalContent.type === 'saveConnStr' && this.defaultConnectionStringForm.invalid) {
            return true;
        }
        else if (this.selectedModalContent.type === 'saveTenant' && this.tenantForm.invalid) {
            return true;
        }
        else {
            return false;
        }
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        this.get();
    }
    /**
     * @param {?} value
     * @return {?}
     */
    onSearch(value) {
        this.pageQuery.filter = value;
        this.get();
    }
    /**
     * @private
     * @return {?}
     */
    createTenantForm() {
        this.tenantForm = this.fb.group({
            name: [this.selected.name || '', [Validators.required, Validators.maxLength(256)]],
        });
    }
    /**
     * @private
     * @return {?}
     */
    createDefaultConnectionStringForm() {
        this.defaultConnectionStringForm = this.fb.group({
            useSharedDatabase: this._useSharedDatabase,
            defaultConnectionString: [this.defaultConnectionString || ''],
        });
    }
    /**
     * @param {?} title
     * @param {?} template
     * @param {?} type
     * @return {?}
     */
    openModal(title, template, type) {
        this.selectedModalContent = {
            title,
            template,
            type,
        };
        this.isModalVisible = true;
    }
    /**
     * @param {?} id
     * @return {?}
     */
    onEditConnectionString(id) {
        this.store
            .dispatch(new GetTenantById(id))
            .pipe(pluck('TenantManagementState', 'selectedItem'), switchMap((/**
         * @param {?} selected
         * @return {?}
         */
        selected => {
            this.selected = selected;
            return this.tenantService.getDefaultConnectionString(id);
        })))
            .subscribe((/**
         * @param {?} fetchedConnectionString
         * @return {?}
         */
        fetchedConnectionString => {
            this._useSharedDatabase = fetchedConnectionString ? false : true;
            this.defaultConnectionString = fetchedConnectionString ? fetchedConnectionString : '';
            this.createDefaultConnectionStringForm();
            this.openModal('AbpTenantManagement::ConnectionStrings', this.connectionStringModalTemplate, 'saveConnStr');
        }));
    }
    /**
     * @return {?}
     */
    addTenant() {
        this.selected = (/** @type {?} */ ({}));
        this.createTenantForm();
        this.openModal('AbpTenantManagement::NewTenant', this.tenantModalTemplate, 'saveTenant');
    }
    /**
     * @param {?} id
     * @return {?}
     */
    editTenant(id) {
        this.store
            .dispatch(new GetTenantById(id))
            .pipe(pluck('TenantManagementState', 'selectedItem'))
            .subscribe((/**
         * @param {?} selected
         * @return {?}
         */
        selected => {
            this.selected = selected;
            this.createTenantForm();
            this.openModal('AbpTenantManagement::Edit', this.tenantModalTemplate, 'saveTenant');
        }));
    }
    /**
     * @return {?}
     */
    save() {
        const { type } = this.selectedModalContent;
        if (!type)
            return;
        if (type === 'saveTenant')
            this.saveTenant();
        else if (type === 'saveConnStr')
            this.saveConnectionString();
    }
    /**
     * @return {?}
     */
    saveConnectionString() {
        if (this.modalBusy)
            return;
        this.modalBusy = true;
        if (this.useSharedDatabase || (!this.useSharedDatabase && !this.connectionString)) {
            this.tenantService
                .deleteDefaultConnectionString(this.selected.id)
                .pipe(take(1), finalize((/**
             * @return {?}
             */
            () => (this.modalBusy = false))))
                .subscribe((/**
             * @return {?}
             */
            () => {
                this.isModalVisible = false;
            }));
        }
        else {
            this.tenantService
                .updateDefaultConnectionString({ id: this.selected.id, defaultConnectionString: this.connectionString })
                .pipe(take(1), finalize((/**
             * @return {?}
             */
            () => (this.modalBusy = false))))
                .subscribe((/**
             * @return {?}
             */
            () => {
                this.isModalVisible = false;
            }));
        }
    }
    /**
     * @return {?}
     */
    saveTenant() {
        if (!this.tenantForm.valid || this.modalBusy)
            return;
        this.modalBusy = true;
        this.store
            .dispatch(this.selected.id
            ? new UpdateTenant(Object.assign({}, this.selected, this.tenantForm.value, { id: this.selected.id }))
            : new CreateTenant(this.tenantForm.value))
            .pipe(finalize((/**
         * @return {?}
         */
        () => (this.modalBusy = false))))
            .subscribe((/**
         * @return {?}
         */
        () => {
            this.isModalVisible = false;
        }));
    }
    /**
     * @param {?} id
     * @param {?} name
     * @return {?}
     */
    delete(id, name) {
        this.confirmationService
            .warn('AbpTenantManagement::TenantDeletionConfirmationMessage', 'AbpTenantManagement::AreYouSure', {
            messageLocalizationParams: [name],
        })
            .subscribe((/**
         * @param {?} status
         * @return {?}
         */
        (status) => {
            if (status === "confirm" /* confirm */) {
                this.store.dispatch(new DeleteTenant(id));
            }
        }));
    }
    /**
     * @param {?} data
     * @return {?}
     */
    onPageChange(data) {
        this.pageQuery.skipCount = data.first;
        this.pageQuery.maxResultCount = data.rows;
        this.get();
    }
    /**
     * @return {?}
     */
    get() {
        this.loading = true;
        this.store
            .dispatch(new GetTenants(this.pageQuery))
            .pipe(finalize((/**
         * @return {?}
         */
        () => (this.loading = false))))
            .subscribe();
    }
    /**
     * @param {?} value
     * @return {?}
     */
    onSharedDatabaseChange(value) {
        if (!value) {
            setTimeout((/**
             * @return {?}
             */
            () => {
                /** @type {?} */
                const defaultConnectionString = (/** @type {?} */ (document.getElementById('defaultConnectionString')));
                if (defaultConnectionString) {
                    defaultConnectionString.focus();
                }
            }), 0);
        }
    }
}
TenantsComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-tenants',
                template: "<div id=\"wrapper\" class=\"card\">\r\n  <div class=\"card-header\">\r\n    <div class=\"row\">\r\n      <div class=\"col col-md-6\">\r\n        <h5 class=\"card-title\">{{ 'AbpTenantManagement::Tenants' | abpLocalization }}</h5>\r\n      </div>\r\n      <div class=\"text-right col col-md-6\">\r\n        <button\r\n          [abpPermission]=\"'AbpTenantManagement.Tenants.Create'\"\r\n          id=\"create-tenants\"\r\n          class=\"btn btn-primary\"\r\n          type=\"button\"\r\n          (click)=\"addTenant()\"\r\n        >\r\n          <i class=\"fa fa-plus mr-1\"></i>\r\n          <span>{{ 'AbpTenantManagement::NewTenant' | abpLocalization }}</span>\r\n        </button>\r\n      </div>\r\n    </div>\r\n  </div>\r\n  <div class=\"card-body\">\r\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\r\n      <label\r\n        ><input\r\n          type=\"search\"\r\n          class=\"form-control form-control-sm\"\r\n          [placeholder]=\"'AbpUi::PagerSearch' | abpLocalization\"\r\n          (input.debounce)=\"onSearch($event.target.value)\"\r\n      /></label>\r\n    </div>\r\n    <p-table\r\n      *ngIf=\"[150, 0] as columnWidths\"\r\n      [value]=\"data$ | async\"\r\n      [abpTableSort]=\"{ key: sortKey, order: sortOrder }\"\r\n      [lazy]=\"true\"\r\n      [lazyLoadOnInit]=\"false\"\r\n      [paginator]=\"true\"\r\n      [rows]=\"10\"\r\n      [totalRecords]=\"totalCount$ | async\"\r\n      [loading]=\"loading\"\r\n      [resizableColumns]=\"true\"\r\n      [scrollable]=\"true\"\r\n      (onLazyLoad)=\"onPageChange($event)\"\r\n    >\r\n      <ng-template pTemplate=\"colgroup\">\r\n        <colgroup>\r\n          <col *ngFor=\"let width of columnWidths\" [ngStyle]=\"{ 'width.px': width || undefined }\" />\r\n        </colgroup>\r\n      </ng-template>\r\n      <ng-template pTemplate=\"emptymessage\" let-columns>\r\n        <tr\r\n          abp-table-empty-message\r\n          [attr.colspan]=\"columnWidths.length\"\r\n          localizationResource=\"AbpTenantManagement\"\r\n          localizationProp=\"NoDataAvailableInDatatable\"\r\n        ></tr>\r\n      </ng-template>\r\n      <ng-template pTemplate=\"header\" let-columns>\r\n        <tr>\r\n          <th>{{ 'AbpTenantManagement::Actions' | abpLocalization }}</th>\r\n          <th pResizableColumn (click)=\"sortOrderIcon.sort('name')\">\r\n            {{ 'AbpTenantManagement::TenantName' | abpLocalization }}\r\n            <abp-sort-order-icon #sortOrderIcon key=\"name\" [(selectedKey)]=\"sortKey\" [(order)]=\"sortOrder\">\r\n            </abp-sort-order-icon>\r\n          </th>\r\n        </tr>\r\n      </ng-template>\r\n      <ng-template pTemplate=\"body\" let-data>\r\n        <tr>\r\n          <td class=\"text-center\">\r\n            <div ngbDropdown container=\"body\" class=\"d-inline-block\">\r\n              <button\r\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\r\n                data-toggle=\"dropdown\"\r\n                aria-haspopup=\"true\"\r\n                ngbDropdownToggle\r\n              >\r\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpTenantManagement::Actions' | abpLocalization }}\r\n              </button>\r\n              <div ngbDropdownMenu>\r\n                <button\r\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Update'\"\r\n                  ngbDropdownItem\r\n                  (click)=\"editTenant(data.id)\"\r\n                >\r\n                  {{ 'AbpTenantManagement::Edit' | abpLocalization }}\r\n                </button>\r\n                <button\r\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageConnectionStrings'\"\r\n                  ngbDropdownItem\r\n                  (click)=\"onEditConnectionString(data.id)\"\r\n                >\r\n                  {{ 'AbpTenantManagement::Permission:ManageConnectionStrings' | abpLocalization }}\r\n                </button>\r\n                <button\r\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageFeatures'\"\r\n                  ngbDropdownItem\r\n                  (click)=\"providerKey = data.id; visibleFeatures = true\"\r\n                >\r\n                  {{ 'AbpTenantManagement::Permission:ManageFeatures' | abpLocalization }}\r\n                </button>\r\n                <button\r\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Delete'\"\r\n                  ngbDropdownItem\r\n                  (click)=\"delete(data.id, data.name)\"\r\n                >\r\n                  {{ 'AbpTenantManagement::Delete' | abpLocalization }}\r\n                </button>\r\n              </div>\r\n            </div>\r\n          </td>\r\n          <td>{{ data.name }}</td>\r\n        </tr>\r\n      </ng-template>\r\n    </p-table>\r\n  </div>\r\n</div>\r\n\r\n<abp-modal size=\"md\" [(visible)]=\"isModalVisible\" [busy]=\"modalBusy\">\r\n  <ng-template #abpHeader>\r\n    <h3>{{ selectedModalContent.title | abpLocalization }}</h3>\r\n  </ng-template>\r\n\r\n  <ng-template #abpBody>\r\n    <ng-container *ngTemplateOutlet=\"selectedModalContent?.template\"></ng-container>\r\n  </ng-template>\r\n\r\n  <ng-template #abpFooter>\r\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\r\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\r\n    </button>\r\n    <abp-button iconClass=\"fa fa-check\" (click)=\"save()\" [disabled]=\"isDisabledSaveButton\">{{\r\n      'AbpIdentity::Save' | abpLocalization\r\n    }}</abp-button>\r\n  </ng-template>\r\n</abp-modal>\r\n\r\n<ng-template #tenantModalTemplate>\r\n  <form [formGroup]=\"tenantForm\" (ngSubmit)=\"save()\" validateOnSubmit>\r\n    <div class=\"mt-2\">\r\n      <div class=\"form-group\">\r\n        <label for=\"name\">{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</label>\r\n        <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" autofocus />\r\n      </div>\r\n    </div>\r\n  </form>\r\n</ng-template>\r\n\r\n<ng-template #connectionStringModalTemplate>\r\n  <form [formGroup]=\"defaultConnectionStringForm\" (ngSubmit)=\"save()\" validateOnSubmit>\r\n    <label class=\"mt-2\">\r\n      <div class=\"form-group\">\r\n        <div class=\"custom-checkbox custom-control mb-2\">\r\n          <input\r\n            id=\"useSharedDatabase\"\r\n            type=\"checkbox\"\r\n            class=\"custom-control-input\"\r\n            formControlName=\"useSharedDatabase\"\r\n            autofocus\r\n            (ngModelChange)=\"onSharedDatabaseChange($event)\"\r\n          />\r\n          <label for=\"useSharedDatabase\" class=\"custom-control-label\">{{\r\n            'AbpTenantManagement::DisplayName:UseSharedDatabase' | abpLocalization\r\n          }}</label>\r\n        </div>\r\n      </div>\r\n      <label class=\"form-group\" *ngIf=\"!useSharedDatabase\">\r\n        <label for=\"defaultConnectionString\">{{\r\n          'AbpTenantManagement::DisplayName:DefaultConnectionString' | abpLocalization\r\n        }}</label>\r\n        <input\r\n          type=\"text\"\r\n          id=\"defaultConnectionString\"\r\n          class=\"form-control\"\r\n          formControlName=\"defaultConnectionString\"\r\n        />\r\n      </label>\r\n    </label>\r\n  </form>\r\n</ng-template>\r\n\r\n<abp-feature-management [(visible)]=\"visibleFeatures\" providerName=\"T\" [providerKey]=\"providerKey\">\r\n</abp-feature-management>\r\n"
            }] }
];
/** @nocollapse */
TenantsComponent.ctorParameters = () => [
    { type: ConfirmationService },
    { type: TenantManagementService },
    { type: FormBuilder },
    { type: Store }
];
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
    TenantsComponent.prototype.sortKey;
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
 * Generated from: lib/tenant-management-routing.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
const ɵ0 = { requiredPolicy: 'AbpTenantManagement.Tenants' };
/** @type {?} */
const routes = [
    { path: '', redirectTo: 'tenants', pathMatch: 'full' },
    {
        path: 'tenants',
        component: DynamicLayoutComponent,
        canActivate: [AuthGuard, PermissionGuard],
        data: ɵ0,
        children: [{ path: '', component: TenantsComponent }],
    },
];
class TenantManagementRoutingModule {
}
TenantManagementRoutingModule.decorators = [
    { type: NgModule, args: [{
                imports: [RouterModule.forChild(routes)],
                exports: [RouterModule],
            },] }
];

/**
 * @fileoverview added by tsickle
 * Generated from: lib/tenant-management.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class TenantManagementModule {
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
/**
 *
 * @deprecated since version 0.9.0
 * @return {?}
 */
function TenantManagementProviders() {
    return [];
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/actions/index.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/index.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: lib/constants/routes.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/**
 *
 * @deprecated since version 0.9.0
 * @type {?}
 */
const TENANT_MANAGEMENT_ROUTES = {
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
 * Generated from: lib/constants/index.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: lib/models/tenant-management.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
 * Generated from: lib/models/index.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/tenant-management-state.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class TenantManagementStateService {
    /**
     * @param {?} store
     */
    constructor(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    get() {
        return this.store.selectSnapshot(TenantManagementState.get);
    }
    /**
     * @return {?}
     */
    getTenantsTotalCount() {
        return this.store.selectSnapshot(TenantManagementState.getTenantsTotalCount);
    }
}
TenantManagementStateService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
TenantManagementStateService.ctorParameters = () => [
    { type: Store }
];
/** @nocollapse */ TenantManagementStateService.ngInjectableDef = ɵɵdefineInjectable({ factory: function TenantManagementStateService_Factory() { return new TenantManagementStateService(ɵɵinject(Store)); }, token: TenantManagementStateService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    TenantManagementStateService.prototype.store;
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/index.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/index.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: public-api.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: abp-ng.tenant-management.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

export { CreateTenant, DeleteTenant, GetTenantById, GetTenants, TENANT_MANAGEMENT_ROUTES, TenantManagementModule, TenantManagementProviders, TenantManagementService, TenantManagementState, TenantManagementStateService, TenantsComponent, UpdateTenant, TenantsComponent as ɵa, TenantManagementState as ɵb, TenantManagementService as ɵc, GetTenants as ɵd, GetTenantById as ɵe, CreateTenant as ɵf, UpdateTenant as ɵg, DeleteTenant as ɵh, TenantManagementRoutingModule as ɵj };
//# sourceMappingURL=abp-ng.tenant-management.js.map
