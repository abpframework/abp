import { RestService, DynamicLayoutComponent, AuthGuard, PermissionGuard, CoreModule } from '@abp/ng.core';
import { ConfirmationService, ThemeSharedModule } from '@abp/ng.theme.shared';
import { Injectable, ɵɵdefineInjectable, ɵɵinject, Component, ViewChild, NgModule } from '@angular/core';
import { NgbModal, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { Action, Selector, State, Store, Select, NgxsModule } from '@ngxs/store';
import { TableModule } from 'primeng/table';
import { __assign, __decorate, __metadata } from 'tslib';
import { Validators, FormBuilder } from '@angular/forms';
import { Observable } from 'rxjs';
import { tap, switchMap, pluck, take } from 'rxjs/operators';
import { RouterModule } from '@angular/router';

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
            url: '/api/multi-tenancy/tenant',
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
            url: "/api/multi-tenancy/tenant/" + id,
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
            url: "/api/multi-tenancy/tenant/" + id,
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
            url: "/api/multi-tenancy/tenant",
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
        var url = "/api/multi-tenancy/tenant/" + body.id;
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
        var url = "/api/multi-tenancy/tenant/" + id + "/defaultConnectionString";
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
        var url = "/api/multi-tenancy/tenant/" + payload.id + "/defaultConnectionString";
        /** @type {?} */
        var request = {
            method: 'PUT',
            url: url,
            params: { defaultConnectionString: payload.defaultConnectionString },
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
        return this.tenantManagementService.get().pipe(tap((/**
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
        return this.tenantManagementService.getById(payload).pipe(tap((/**
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
        return this.tenantManagementService.delete(payload).pipe(switchMap((/**
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
        return this.tenantManagementService.add(payload).pipe(switchMap((/**
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
        return dispatch(new TenantManagementGetById(payload.id)).pipe(switchMap((/**
         * @return {?}
         */
        function () { return _this.tenantManagementService.update(__assign({}, getState().selectedItem, payload)); })), switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new TenantManagementGet()); })));
    };
    __decorate([
        Action(TenantManagementGet),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object]),
        __metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "get", null);
    __decorate([
        Action(TenantManagementGetById),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, TenantManagementGetById]),
        __metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "getById", null);
    __decorate([
        Action(TenantManagementDelete),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, TenantManagementDelete]),
        __metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "delete", null);
    __decorate([
        Action(TenantManagementAdd),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, TenantManagementAdd]),
        __metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "add", null);
    __decorate([
        Action(TenantManagementUpdate),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, TenantManagementUpdate]),
        __metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "update", null);
    __decorate([
        Selector(),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object]),
        __metadata("design:returntype", Array)
    ], TenantManagementState, "get", null);
    TenantManagementState = __decorate([
        State({
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
            .pipe(take(1))
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
            .pipe(pluck('TenantManagementState', 'selectedItem'))
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
        { type: Component, args: [{
                    selector: 'abp-tenants',
                    template: "<div id=\"wrapper\" class=\"card\">\n  <div class=\"card-header\">\n    <div class=\"row\">\n      <div class=\"col col-md-6\">\n        <h5 class=\"card-title\">\n          {{ 'AbpTenantManagement::Tenants' | abpLocalization }}\n        </h5>\n      </div>\n      <div class=\"text-right col col-md-6\">\n        <button\n          [abpPermission]=\"'AbpTenantManagement.Tenants.Create'\"\n          id=\"create-tenants\"\n          class=\"btn btn-primary\"\n          type=\"button\"\n          (click)=\"onAdd()\"\n        >\n          <i class=\"fa fa-plus mr-1\"></i>\n          <span>{{ 'AbpTenantManagement::NewTenant' | abpLocalization }}</span>\n        </button>\n      </div>\n    </div>\n  </div>\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          placeholder=\"Search\"\n          (input)=\"dt.filterGlobal($event.target.value, 'contains')\"\n      /></label>\n    </div>\n    <p-table #dt [value]=\"datas$ | async\" [globalFilterFields]=\"['name']\" [paginator]=\"true\" [rows]=\"10\">\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpTenantManagement::Actions' | abpLocalization }}</th>\n          <th>{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpTenantManagement::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Update'\"\n                  ngbDropdownItem\n                  (click)=\"onEdit(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::Edit' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageConnectionStrings'\"\n                  ngbDropdownItem\n                  (click)=\"onEditConnStr(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::ConnectionStrings' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageFeatures'\"\n                  ngbDropdownItem\n                  (click)=\"onManageFeatures(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::Features' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Delete'\"\n                  ngbDropdownItem\n                  (click)=\"delete(data.id, data.name)\"\n                >\n                  {{ 'AbpTenantManagement::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<ng-template #modalWrapper let-modal>\n  <div class=\"modal-header\">\n    <h5 class=\"modal-title\" id=\"modal-basic-title\">\n      {{ selectedModalContent.title | abpLocalization }}\n    </h5>\n    <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n      <span aria-hidden=\"true\">&times;</span>\n    </button>\n  </div>\n\n  <form [formGroup]=\"tenantForm\" (ngSubmit)=\"selectedModalContent.onSave()\">\n    <div class=\"modal-body\">\n      <ng-container *ngTemplateOutlet=\"selectedModalContent.template; context: { $implicit: modal }\"></ng-container>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpTenantManagement::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </form>\n</ng-template>\n\n<ng-template #mTemplateTenant let-modal>\n  <div class=\"mt-2\">\n    <div class=\"form-group\">\n      <label for=\"name\">{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</label>\n      <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n    </div>\n  </div>\n</ng-template>\n\n<ng-template #mTemplateConnStr let-modal>\n  <div class=\"mt-2\">\n    <div class=\"form-group\">\n      <div class=\"form-check\">\n        <input id=\"useSharedDatabase\" type=\"checkbox\" class=\"form-check-input\" formControlName=\"useSharedDatabase\" />\n        <label for=\"useSharedDatabase\" class=\"font-check-label\">{{\n          'AbpTenantManagement::DisplayName:UseSharedDatabase' | abpLocalization\n        }}</label>\n      </div>\n    </div>\n    <div class=\"form-group\" *ngIf=\"showInput\">\n      <label for=\"defaultConnectionString\">{{\n        'AbpTenantManagement::DisplayName:DefaultConnectionString' | abpLocalization\n      }}</label>\n      <input type=\"text\" id=\"defaultConnectionString\" class=\"form-control\" formControlName=\"defaultConnectionString\" />\n    </div>\n  </div>\n</ng-template>\n\n<ng-template #mTemplateFeatures let-modal>\n  Manage Features\n</ng-template>\n"
                }] }
    ];
    /** @nocollapse */
    TenantsComponent.ctorParameters = function () { return [
        { type: ConfirmationService },
        { type: TenantManagementService },
        { type: NgbModal },
        { type: FormBuilder },
        { type: Store }
    ]; };
    TenantsComponent.propDecorators = {
        modalWrapper: [{ type: ViewChild, args: ['modalWrapper', { static: false },] }],
        mTemplateConnStr: [{ type: ViewChild, args: ['mTemplateConnStr', { static: false },] }],
        mTemplateFeatures: [{ type: ViewChild, args: ['mTemplateFeatures', { static: false },] }],
        mTemplateTenant: [{ type: ViewChild, args: ['mTemplateTenant', { static: false },] }]
    };
    __decorate([
        Select(TenantManagementState.get),
        __metadata("design:type", Observable)
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
        { type: Injectable }
    ];
    /** @nocollapse */
    TenantsResolver.ctorParameters = function () { return [
        { type: Store }
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
                        CoreModule,
                        TableModule,
                        ThemeSharedModule,
                        NgbDropdownModule,
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

export { TENANT_MANAGEMENT_ROUTES, TenantManagementAdd, TenantManagementDelete, TenantManagementGet, TenantManagementGetById, TenantManagementModule, TenantManagementService, TenantManagementState, TenantManagementUpdate, TenantsComponent, TenantsResolver, TenantsComponent as ɵa, TenantManagementState as ɵb, TenantManagementService as ɵc, TenantManagementGet as ɵd, TenantManagementGetById as ɵe, TenantManagementAdd as ɵf, TenantManagementUpdate as ɵg, TenantManagementDelete as ɵh, TenantManagementService as ɵj, TenantManagementRoutingModule as ɵk, TenantsResolver as ɵl };
//# sourceMappingURL=abp-ng.tenant-management.js.map
