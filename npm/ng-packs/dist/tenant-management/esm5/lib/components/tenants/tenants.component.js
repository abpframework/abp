/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/tenants/tenants.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { ConfirmationService } from '@abp/ng.theme.shared';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize, pluck, switchMap, take } from 'rxjs/operators';
import { CreateTenant, DeleteTenant, GetTenantById, GetTenants, UpdateTenant, } from '../../actions/tenant-management.actions';
import { TenantManagementService } from '../../services/tenant-management.service';
import { TenantManagementState } from '../../states/tenant-management.state';
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
var TenantsComponent = /** @class */ (function () {
    function TenantsComponent(confirmationService, tenantService, fb, store) {
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
    Object.defineProperty(TenantsComponent.prototype, "isDisabledSaveButton", {
        get: /**
         * @return {?}
         */
        function () {
            if (!this.selectedModalContent)
                return false;
            if (this.selectedModalContent.type === 'saveConnStr' &&
                this.defaultConnectionStringForm &&
                this.defaultConnectionStringForm.invalid) {
                return true;
            }
            else if (this.selectedModalContent.type === 'saveTenant' &&
                this.tenantForm &&
                this.tenantForm.invalid) {
                return true;
            }
            else {
                return false;
            }
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @return {?}
     */
    TenantsComponent.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        this.get();
    };
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
    TenantsComponent.prototype.addTenant = /**
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
    TenantsComponent.prototype.editTenant = /**
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
        if (this.modalBusy)
            return;
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
                .updateDefaultConnectionString({
                id: this.selected.id,
                defaultConnectionString: this.connectionString,
            })
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
        if (!this.tenantForm.valid || this.modalBusy)
            return;
        this.modalBusy = true;
        this.store
            .dispatch(this.selected.id
            ? new UpdateTenant(tslib_1.__assign({}, this.selected, this.tenantForm.value, { id: this.selected.id }))
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
            _this.get();
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
                _this.store.dispatch(new DeleteTenant(id)).subscribe((/**
                 * @return {?}
                 */
                function () { return _this.get(); }));
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
     * @param {?} value
     * @return {?}
     */
    TenantsComponent.prototype.onSharedDatabaseChange = /**
     * @param {?} value
     * @return {?}
     */
    function (value) {
        if (!value) {
            setTimeout((/**
             * @return {?}
             */
            function () {
                /** @type {?} */
                var defaultConnectionString = (/** @type {?} */ (document.getElementById('defaultConnectionString')));
                if (defaultConnectionString) {
                    defaultConnectionString.focus();
                }
            }), 0);
        }
    };
    TenantsComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-tenants',
                    template: "<div id=\"wrapper\" class=\"card\">\n  <div class=\"card-header\">\n    <div class=\"row\">\n      <div class=\"col col-md-6\">\n        <h5 class=\"card-title\">{{ 'AbpTenantManagement::Tenants' | abpLocalization }}</h5>\n      </div>\n      <div class=\"text-right col col-md-6\">\n        <button\n          *abpPermission=\"'AbpTenantManagement.Tenants.Create'\"\n          id=\"create-tenants\"\n          class=\"btn btn-primary\"\n          type=\"button\"\n          (click)=\"addTenant()\"\n        >\n          <i class=\"fa fa-plus mr-1\"></i>\n          <span>{{ 'AbpTenantManagement::NewTenant' | abpLocalization }}</span>\n        </button>\n      </div>\n    </div>\n  </div>\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          [placeholder]=\"'AbpUi::PagerSearch' | abpLocalization\"\n          (input.debounce)=\"onSearch($event.target.value)\"\n      /></label>\n    </div>\n    <p-table\n      *ngIf=\"[150, 0] as columnWidths\"\n      [value]=\"data$ | async\"\n      [abpTableSort]=\"{ key: sortKey, order: sortOrder }\"\n      [lazy]=\"true\"\n      [lazyLoadOnInit]=\"false\"\n      [paginator]=\"true\"\n      [rows]=\"10\"\n      [totalRecords]=\"totalCount$ | async\"\n      [loading]=\"loading\"\n      [resizableColumns]=\"true\"\n      [scrollable]=\"true\"\n      (onLazyLoad)=\"onPageChange($event)\"\n    >\n      <ng-template pTemplate=\"colgroup\">\n        <colgroup>\n          <col *ngFor=\"let width of columnWidths\" [ngStyle]=\"{ 'width.px': width || undefined }\" />\n        </colgroup>\n      </ng-template>\n      <ng-template pTemplate=\"emptymessage\" let-columns>\n        <tr\n          abp-table-empty-message\n          [attr.colspan]=\"columnWidths.length\"\n          localizationResource=\"AbpTenantManagement\"\n          localizationProp=\"NoDataAvailableInDatatable\"\n        ></tr>\n      </ng-template>\n      <ng-template pTemplate=\"header\" let-columns>\n        <tr>\n          <th>{{ 'AbpTenantManagement::Actions' | abpLocalization }}</th>\n          <th pResizableColumn (click)=\"sortOrderIcon.sort('name')\">\n            {{ 'AbpTenantManagement::TenantName' | abpLocalization }}\n            <abp-sort-order-icon\n              #sortOrderIcon\n              key=\"name\"\n              [(selectedKey)]=\"sortKey\"\n              [(order)]=\"sortOrder\"\n            >\n            </abp-sort-order-icon>\n          </th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td class=\"text-center\">\n            <div ngbDropdown container=\"body\" class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpTenantManagement::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button\n                  *abpPermission=\"'AbpTenantManagement.Tenants.Update'\"\n                  ngbDropdownItem\n                  (click)=\"editTenant(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::Edit' | abpLocalization }}\n                </button>\n                <button\n                  *abpPermission=\"'AbpTenantManagement.Tenants.ManageConnectionStrings'\"\n                  ngbDropdownItem\n                  (click)=\"onEditConnectionString(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::Permission:ManageConnectionStrings' | abpLocalization }}\n                </button>\n                <button\n                  *abpPermission=\"'AbpTenantManagement.Tenants.ManageFeatures'\"\n                  ngbDropdownItem\n                  (click)=\"providerKey = data.id; visibleFeatures = true\"\n                >\n                  {{ 'AbpTenantManagement::Permission:ManageFeatures' | abpLocalization }}\n                </button>\n                <button\n                  *abpPermission=\"'AbpTenantManagement.Tenants.Delete'\"\n                  ngbDropdownItem\n                  (click)=\"delete(data.id, data.name)\"\n                >\n                  {{ 'AbpTenantManagement::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal size=\"md\" [(visible)]=\"isModalVisible\" [busy]=\"modalBusy\">\n  <ng-template #abpHeader>\n    <h3>{{ selectedModalContent.title | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <ng-container *ngTemplateOutlet=\"selectedModalContent?.template\"></ng-container>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n    </button>\n    <abp-button iconClass=\"fa fa-check\" (click)=\"save()\" [disabled]=\"isDisabledSaveButton\">{{\n      'AbpIdentity::Save' | abpLocalization\n    }}</abp-button>\n  </ng-template>\n</abp-modal>\n\n<ng-template #tenantModalTemplate>\n  <form [formGroup]=\"tenantForm\" (ngSubmit)=\"save()\" validateOnSubmit>\n    <div class=\"mt-2\">\n      <div class=\"form-group\">\n        <label for=\"name\">{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</label>\n        <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" autofocus />\n      </div>\n    </div>\n  </form>\n</ng-template>\n\n<ng-template #connectionStringModalTemplate>\n  <form [formGroup]=\"defaultConnectionStringForm\" (ngSubmit)=\"save()\" validateOnSubmit>\n    <div class=\"form-group\">\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input\n          id=\"useSharedDatabase\"\n          type=\"checkbox\"\n          class=\"custom-control-input\"\n          formControlName=\"useSharedDatabase\"\n          autofocus\n          (ngModelChange)=\"onSharedDatabaseChange($event)\"\n        />\n        <label for=\"useSharedDatabase\" class=\"custom-control-label\">{{\n          'AbpTenantManagement::DisplayName:UseSharedDatabase' | abpLocalization\n        }}</label>\n      </div>\n    </div>\n    <div class=\"form-group\" *ngIf=\"!useSharedDatabase\">\n      <label for=\"defaultConnectionString\">{{\n        'AbpTenantManagement::DisplayName:DefaultConnectionString' | abpLocalization\n      }}</label>\n      <input\n        type=\"text\"\n        id=\"defaultConnectionString\"\n        class=\"form-control\"\n        formControlName=\"defaultConnectionString\"\n      />\n    </div>\n  </form>\n</ng-template>\n\n<abp-feature-management [(visible)]=\"visibleFeatures\" providerName=\"T\" [providerKey]=\"providerKey\">\n</abp-feature-management>\n"
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
    tslib_1.__decorate([
        Select(TenantManagementState.get),
        tslib_1.__metadata("design:type", Observable)
    ], TenantsComponent.prototype, "data$", void 0);
    tslib_1.__decorate([
        Select(TenantManagementState.getTenantsTotalCount),
        tslib_1.__metadata("design:type", Observable)
    ], TenantsComponent.prototype, "totalCount$", void 0);
    return TenantsComponent;
}());
export { TenantsComponent };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50cy5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvdGVuYW50cy90ZW5hbnRzLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFDQSxPQUFPLEVBQUUsbUJBQW1CLEVBQVcsTUFBTSxzQkFBc0IsQ0FBQztBQUNwRSxPQUFPLEVBQUUsU0FBUyxFQUFVLFdBQVcsRUFBRSxTQUFTLEVBQXFCLE1BQU0sZUFBZSxDQUFDO0FBQzdGLE9BQU8sRUFBRSxXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDcEUsT0FBTyxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDNUMsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNsQyxPQUFPLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBRSxTQUFTLEVBQUUsSUFBSSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDbEUsT0FBTyxFQUNMLFlBQVksRUFDWixZQUFZLEVBQ1osYUFBYSxFQUNiLFVBQVUsRUFDVixZQUFZLEdBQ2IsTUFBTSx5Q0FBeUMsQ0FBQztBQUNqRCxPQUFPLEVBQUUsdUJBQXVCLEVBQUUsTUFBTSwwQ0FBMEMsQ0FBQztBQUNuRixPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxzQ0FBc0MsQ0FBQzs7OztBQUU3RSxtQ0FJQzs7O0lBSEMsb0NBQW1DOztJQUNuQyxxQ0FBYzs7SUFDZCx3Q0FBMkI7O0FBRzdCO0lBeUVFLDBCQUNVLG1CQUF3QyxFQUN4QyxhQUFzQyxFQUN0QyxFQUFlLEVBQ2YsS0FBWTtRQUhaLHdCQUFtQixHQUFuQixtQkFBbUIsQ0FBcUI7UUFDeEMsa0JBQWEsR0FBYixhQUFhLENBQXlCO1FBQ3RDLE9BQUUsR0FBRixFQUFFLENBQWE7UUFDZixVQUFLLEdBQUwsS0FBSyxDQUFPO1FB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ZSc7XG5pbXBvcnQgeyBDb25maXJtYXRpb25TZXJ2aWNlLCBUb2FzdGVyIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgQ29tcG9uZW50LCBPbkluaXQsIFRlbXBsYXRlUmVmLCBWaWV3Q2hpbGQsIENoYW5nZURldGVjdG9yUmVmIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBGb3JtQnVpbGRlciwgRm9ybUdyb3VwLCBWYWxpZGF0b3JzIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGZpbmFsaXplLCBwbHVjaywgc3dpdGNoTWFwLCB0YWtlIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHtcbiAgQ3JlYXRlVGVuYW50LFxuICBEZWxldGVUZW5hbnQsXG4gIEdldFRlbmFudEJ5SWQsXG4gIEdldFRlbmFudHMsXG4gIFVwZGF0ZVRlbmFudCxcbn0gZnJvbSAnLi4vLi4vYWN0aW9ucy90ZW5hbnQtbWFuYWdlbWVudC5hY3Rpb25zJztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQuc2VydmljZSc7XG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuLi8uLi9zdGF0ZXMvdGVuYW50LW1hbmFnZW1lbnQuc3RhdGUnO1xuXG5pbnRlcmZhY2UgU2VsZWN0ZWRNb2RhbENvbnRlbnQge1xuICB0eXBlOiAnc2F2ZUNvbm5TdHInIHwgJ3NhdmVUZW5hbnQnO1xuICB0aXRsZTogc3RyaW5nO1xuICB0ZW1wbGF0ZTogVGVtcGxhdGVSZWY8YW55Pjtcbn1cblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXRlbmFudHMnLFxuICB0ZW1wbGF0ZVVybDogJy4vdGVuYW50cy5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFRlbmFudHNDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBAU2VsZWN0KFRlbmFudE1hbmFnZW1lbnRTdGF0ZS5nZXQpXG4gIGRhdGEkOiBPYnNlcnZhYmxlPEFCUC5CYXNpY0l0ZW1bXT47XG5cbiAgQFNlbGVjdChUZW5hbnRNYW5hZ2VtZW50U3RhdGUuZ2V0VGVuYW50c1RvdGFsQ291bnQpXG4gIHRvdGFsQ291bnQkOiBPYnNlcnZhYmxlPG51bWJlcj47XG5cbiAgc2VsZWN0ZWQ6IEFCUC5CYXNpY0l0ZW07XG5cbiAgdGVuYW50Rm9ybTogRm9ybUdyb3VwO1xuXG4gIGRlZmF1bHRDb25uZWN0aW9uU3RyaW5nRm9ybTogRm9ybUdyb3VwO1xuXG4gIGRlZmF1bHRDb25uZWN0aW9uU3RyaW5nOiBzdHJpbmc7XG5cbiAgaXNNb2RhbFZpc2libGU6IGJvb2xlYW47XG5cbiAgc2VsZWN0ZWRNb2RhbENvbnRlbnQgPSB7fSBhcyBTZWxlY3RlZE1vZGFsQ29udGVudDtcblxuICB2aXNpYmxlRmVhdHVyZXMgPSBmYWxzZTtcblxuICBwcm92aWRlcktleTogc3RyaW5nO1xuXG4gIF91c2VTaGFyZWREYXRhYmFzZTogYm9vbGVhbjtcblxuICBwYWdlUXVlcnk6IEFCUC5QYWdlUXVlcnlQYXJhbXMgPSB7fTtcblxuICBsb2FkaW5nID0gZmFsc2U7XG5cbiAgbW9kYWxCdXN5ID0gZmFsc2U7XG5cbiAgc29ydE9yZGVyID0gJyc7XG5cbiAgc29ydEtleSA9ICcnO1xuXG4gIGdldCB1c2VTaGFyZWREYXRhYmFzZSgpOiBib29sZWFuIHtcbiAgICByZXR1cm4gdGhpcy5kZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0uZ2V0KCd1c2VTaGFyZWREYXRhYmFzZScpLnZhbHVlO1xuICB9XG5cbiAgZ2V0IGNvbm5lY3Rpb25TdHJpbmcoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy5kZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0uZ2V0KCdkZWZhdWx0Q29ubmVjdGlvblN0cmluZycpLnZhbHVlO1xuICB9XG5cbiAgQFZpZXdDaGlsZCgndGVuYW50TW9kYWxUZW1wbGF0ZScsIHsgc3RhdGljOiBmYWxzZSB9KVxuICB0ZW5hbnRNb2RhbFRlbXBsYXRlOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBWaWV3Q2hpbGQoJ2Nvbm5lY3Rpb25TdHJpbmdNb2RhbFRlbXBsYXRlJywgeyBzdGF0aWM6IGZhbHNlIH0pXG4gIGNvbm5lY3Rpb25TdHJpbmdNb2RhbFRlbXBsYXRlOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIGdldCBpc0Rpc2FibGVkU2F2ZUJ1dHRvbigpOiBib29sZWFuIHtcbiAgICBpZiAoIXRoaXMuc2VsZWN0ZWRNb2RhbENvbnRlbnQpIHJldHVybiBmYWxzZTtcblxuICAgIGlmIChcbiAgICAgIHRoaXMuc2VsZWN0ZWRNb2RhbENvbnRlbnQudHlwZSA9PT0gJ3NhdmVDb25uU3RyJyAmJlxuICAgICAgdGhpcy5kZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0gJiZcbiAgICAgIHRoaXMuZGVmYXVsdENvbm5lY3Rpb25TdHJpbmdGb3JtLmludmFsaWRcbiAgICApIHtcbiAgICAgIHJldHVybiB0cnVlO1xuICAgIH0gZWxzZSBpZiAoXG4gICAgICB0aGlzLnNlbGVjdGVkTW9kYWxDb250ZW50LnR5cGUgPT09ICdzYXZlVGVuYW50JyAmJlxuICAgICAgdGhpcy50ZW5hbnRGb3JtICYmXG4gICAgICB0aGlzLnRlbmFudEZvcm0uaW52YWxpZFxuICAgICkge1xuICAgICAgcmV0dXJuIHRydWU7XG4gICAgfSBlbHNlIHtcbiAgICAgIHJldHVybiBmYWxzZTtcbiAgICB9XG4gIH1cblxuICBjb25zdHJ1Y3RvcihcbiAgICBwcml2YXRlIGNvbmZpcm1hdGlvblNlcnZpY2U6IENvbmZpcm1hdGlvblNlcnZpY2UsXG4gICAgcHJpdmF0ZSB0ZW5hbnRTZXJ2aWNlOiBUZW5hbnRNYW5hZ2VtZW50U2VydmljZSxcbiAgICBwcml2YXRlIGZiOiBGb3JtQnVpbGRlcixcbiAgICBwcml2YXRlIHN0b3JlOiBTdG9yZSxcbiAgKSB7fVxuXG4gIG5nT25Jbml0KCkge1xuICAgIHRoaXMuZ2V0KCk7XG4gIH1cblxuICBvblNlYXJjaCh2YWx1ZSkge1xuICAgIHRoaXMucGFnZVF1ZXJ5LmZpbHRlciA9IHZhbHVlO1xuICAgIHRoaXMuZ2V0KCk7XG4gIH1cblxuICBwcml2YXRlIGNyZWF0ZVRlbmFudEZvcm0oKSB7XG4gICAgdGhpcy50ZW5hbnRGb3JtID0gdGhpcy5mYi5ncm91cCh7XG4gICAgICBuYW1lOiBbdGhpcy5zZWxlY3RlZC5uYW1lIHx8ICcnLCBbVmFsaWRhdG9ycy5yZXF1aXJlZCwgVmFsaWRhdG9ycy5tYXhMZW5ndGgoMjU2KV1dLFxuICAgIH0pO1xuICB9XG5cbiAgcHJpdmF0ZSBjcmVhdGVEZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0oKSB7XG4gICAgdGhpcy5kZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0gPSB0aGlzLmZiLmdyb3VwKHtcbiAgICAgIHVzZVNoYXJlZERhdGFiYXNlOiB0aGlzLl91c2VTaGFyZWREYXRhYmFzZSxcbiAgICAgIGRlZmF1bHRDb25uZWN0aW9uU3RyaW5nOiBbdGhpcy5kZWZhdWx0Q29ubmVjdGlvblN0cmluZyB8fCAnJ10sXG4gICAgfSk7XG4gIH1cblxuICBvcGVuTW9kYWwodGl0bGU6IHN0cmluZywgdGVtcGxhdGU6IFRlbXBsYXRlUmVmPGFueT4sIHR5cGU6ICdzYXZlQ29ublN0cicgfCAnc2F2ZVRlbmFudCcpIHtcbiAgICB0aGlzLnNlbGVjdGVkTW9kYWxDb250ZW50ID0ge1xuICAgICAgdGl0bGUsXG4gICAgICB0ZW1wbGF0ZSxcbiAgICAgIHR5cGUsXG4gICAgfTtcblxuICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSB0cnVlO1xuICB9XG5cbiAgb25FZGl0Q29ubmVjdGlvblN0cmluZyhpZDogc3RyaW5nKSB7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBHZXRUZW5hbnRCeUlkKGlkKSlcbiAgICAgIC5waXBlKFxuICAgICAgICBwbHVjaygnVGVuYW50TWFuYWdlbWVudFN0YXRlJywgJ3NlbGVjdGVkSXRlbScpLFxuICAgICAgICBzd2l0Y2hNYXAoc2VsZWN0ZWQgPT4ge1xuICAgICAgICAgIHRoaXMuc2VsZWN0ZWQgPSBzZWxlY3RlZDtcbiAgICAgICAgICByZXR1cm4gdGhpcy50ZW5hbnRTZXJ2aWNlLmdldERlZmF1bHRDb25uZWN0aW9uU3RyaW5nKGlkKTtcbiAgICAgICAgfSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKGZldGNoZWRDb25uZWN0aW9uU3RyaW5nID0+IHtcbiAgICAgICAgdGhpcy5fdXNlU2hhcmVkRGF0YWJhc2UgPSBmZXRjaGVkQ29ubmVjdGlvblN0cmluZyA/IGZhbHNlIDogdHJ1ZTtcbiAgICAgICAgdGhpcy5kZWZhdWx0Q29ubmVjdGlvblN0cmluZyA9IGZldGNoZWRDb25uZWN0aW9uU3RyaW5nID8gZmV0Y2hlZENvbm5lY3Rpb25TdHJpbmcgOiAnJztcbiAgICAgICAgdGhpcy5jcmVhdGVEZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0oKTtcbiAgICAgICAgdGhpcy5vcGVuTW9kYWwoXG4gICAgICAgICAgJ0FicFRlbmFudE1hbmFnZW1lbnQ6OkNvbm5lY3Rpb25TdHJpbmdzJyxcbiAgICAgICAgICB0aGlzLmNvbm5lY3Rpb25TdHJpbmdNb2RhbFRlbXBsYXRlLFxuICAgICAgICAgICdzYXZlQ29ublN0cicsXG4gICAgICAgICk7XG4gICAgICB9KTtcbiAgfVxuXG4gIGFkZFRlbmFudCgpIHtcbiAgICB0aGlzLnNlbGVjdGVkID0ge30gYXMgQUJQLkJhc2ljSXRlbTtcbiAgICB0aGlzLmNyZWF0ZVRlbmFudEZvcm0oKTtcbiAgICB0aGlzLm9wZW5Nb2RhbCgnQWJwVGVuYW50TWFuYWdlbWVudDo6TmV3VGVuYW50JywgdGhpcy50ZW5hbnRNb2RhbFRlbXBsYXRlLCAnc2F2ZVRlbmFudCcpO1xuICB9XG5cbiAgZWRpdFRlbmFudChpZDogc3RyaW5nKSB7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBHZXRUZW5hbnRCeUlkKGlkKSlcbiAgICAgIC5waXBlKHBsdWNrKCdUZW5hbnRNYW5hZ2VtZW50U3RhdGUnLCAnc2VsZWN0ZWRJdGVtJykpXG4gICAgICAuc3Vic2NyaWJlKHNlbGVjdGVkID0+IHtcbiAgICAgICAgdGhpcy5zZWxlY3RlZCA9IHNlbGVjdGVkO1xuICAgICAgICB0aGlzLmNyZWF0ZVRlbmFudEZvcm0oKTtcbiAgICAgICAgdGhpcy5vcGVuTW9kYWwoJ0FicFRlbmFudE1hbmFnZW1lbnQ6OkVkaXQnLCB0aGlzLnRlbmFudE1vZGFsVGVtcGxhdGUsICdzYXZlVGVuYW50Jyk7XG4gICAgICB9KTtcbiAgfVxuXG4gIHNhdmUoKSB7XG4gICAgY29uc3QgeyB0eXBlIH0gPSB0aGlzLnNlbGVjdGVkTW9kYWxDb250ZW50O1xuICAgIGlmICghdHlwZSkgcmV0dXJuO1xuICAgIGlmICh0eXBlID09PSAnc2F2ZVRlbmFudCcpIHRoaXMuc2F2ZVRlbmFudCgpO1xuICAgIGVsc2UgaWYgKHR5cGUgPT09ICdzYXZlQ29ublN0cicpIHRoaXMuc2F2ZUNvbm5lY3Rpb25TdHJpbmcoKTtcbiAgfVxuXG4gIHNhdmVDb25uZWN0aW9uU3RyaW5nKCkge1xuICAgIGlmICh0aGlzLm1vZGFsQnVzeSkgcmV0dXJuO1xuXG4gICAgdGhpcy5tb2RhbEJ1c3kgPSB0cnVlO1xuICAgIGlmICh0aGlzLnVzZVNoYXJlZERhdGFiYXNlIHx8ICghdGhpcy51c2VTaGFyZWREYXRhYmFzZSAmJiAhdGhpcy5jb25uZWN0aW9uU3RyaW5nKSkge1xuICAgICAgdGhpcy50ZW5hbnRTZXJ2aWNlXG4gICAgICAgIC5kZWxldGVEZWZhdWx0Q29ubmVjdGlvblN0cmluZyh0aGlzLnNlbGVjdGVkLmlkKVxuICAgICAgICAucGlwZShcbiAgICAgICAgICB0YWtlKDEpLFxuICAgICAgICAgIGZpbmFsaXplKCgpID0+ICh0aGlzLm1vZGFsQnVzeSA9IGZhbHNlKSksXG4gICAgICAgIClcbiAgICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IGZhbHNlO1xuICAgICAgICB9KTtcbiAgICB9IGVsc2Uge1xuICAgICAgdGhpcy50ZW5hbnRTZXJ2aWNlXG4gICAgICAgIC51cGRhdGVEZWZhdWx0Q29ubmVjdGlvblN0cmluZyh7XG4gICAgICAgICAgaWQ6IHRoaXMuc2VsZWN0ZWQuaWQsXG4gICAgICAgICAgZGVmYXVsdENvbm5lY3Rpb25TdHJpbmc6IHRoaXMuY29ubmVjdGlvblN0cmluZyxcbiAgICAgICAgfSlcbiAgICAgICAgLnBpcGUoXG4gICAgICAgICAgdGFrZSgxKSxcbiAgICAgICAgICBmaW5hbGl6ZSgoKSA9PiAodGhpcy5tb2RhbEJ1c3kgPSBmYWxzZSkpLFxuICAgICAgICApXG4gICAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSBmYWxzZTtcbiAgICAgICAgfSk7XG4gICAgfVxuICB9XG5cbiAgc2F2ZVRlbmFudCgpIHtcbiAgICBpZiAoIXRoaXMudGVuYW50Rm9ybS52YWxpZCB8fCB0aGlzLm1vZGFsQnVzeSkgcmV0dXJuO1xuICAgIHRoaXMubW9kYWxCdXN5ID0gdHJ1ZTtcblxuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChcbiAgICAgICAgdGhpcy5zZWxlY3RlZC5pZFxuICAgICAgICAgID8gbmV3IFVwZGF0ZVRlbmFudCh7IC4uLnRoaXMuc2VsZWN0ZWQsIC4uLnRoaXMudGVuYW50Rm9ybS52YWx1ZSwgaWQ6IHRoaXMuc2VsZWN0ZWQuaWQgfSlcbiAgICAgICAgICA6IG5ldyBDcmVhdGVUZW5hbnQodGhpcy50ZW5hbnRGb3JtLnZhbHVlKSxcbiAgICAgIClcbiAgICAgIC5waXBlKGZpbmFsaXplKCgpID0+ICh0aGlzLm1vZGFsQnVzeSA9IGZhbHNlKSkpXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IGZhbHNlO1xuICAgICAgICB0aGlzLmdldCgpO1xuICAgICAgfSk7XG4gIH1cblxuICBkZWxldGUoaWQ6IHN0cmluZywgbmFtZTogc3RyaW5nKSB7XG4gICAgdGhpcy5jb25maXJtYXRpb25TZXJ2aWNlXG4gICAgICAud2FybihcbiAgICAgICAgJ0FicFRlbmFudE1hbmFnZW1lbnQ6OlRlbmFudERlbGV0aW9uQ29uZmlybWF0aW9uTWVzc2FnZScsXG4gICAgICAgICdBYnBUZW5hbnRNYW5hZ2VtZW50OjpBcmVZb3VTdXJlJyxcbiAgICAgICAge1xuICAgICAgICAgIG1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXM6IFtuYW1lXSxcbiAgICAgICAgfSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoKHN0YXR1czogVG9hc3Rlci5TdGF0dXMpID0+IHtcbiAgICAgICAgaWYgKHN0YXR1cyA9PT0gVG9hc3Rlci5TdGF0dXMuY29uZmlybSkge1xuICAgICAgICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IERlbGV0ZVRlbmFudChpZCkpLnN1YnNjcmliZSgoKSA9PiB0aGlzLmdldCgpKTtcbiAgICAgICAgfVxuICAgICAgfSk7XG4gIH1cblxuICBvblBhZ2VDaGFuZ2UoZGF0YSkge1xuICAgIHRoaXMucGFnZVF1ZXJ5LnNraXBDb3VudCA9IGRhdGEuZmlyc3Q7XG4gICAgdGhpcy5wYWdlUXVlcnkubWF4UmVzdWx0Q291bnQgPSBkYXRhLnJvd3M7XG5cbiAgICB0aGlzLmdldCgpO1xuICB9XG5cbiAgZ2V0KCkge1xuICAgIHRoaXMubG9hZGluZyA9IHRydWU7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBHZXRUZW5hbnRzKHRoaXMucGFnZVF1ZXJ5KSlcbiAgICAgIC5waXBlKGZpbmFsaXplKCgpID0+ICh0aGlzLmxvYWRpbmcgPSBmYWxzZSkpKVxuICAgICAgLnN1YnNjcmliZSgpO1xuICB9XG5cbiAgb25TaGFyZWREYXRhYmFzZUNoYW5nZSh2YWx1ZTogYm9vbGVhbikge1xuICAgIGlmICghdmFsdWUpIHtcbiAgICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgICBjb25zdCBkZWZhdWx0Q29ubmVjdGlvblN0cmluZyA9IGRvY3VtZW50LmdldEVsZW1lbnRCeUlkKFxuICAgICAgICAgICdkZWZhdWx0Q29ubmVjdGlvblN0cmluZycsXG4gICAgICAgICkgYXMgSFRNTElucHV0RWxlbWVudDtcbiAgICAgICAgaWYgKGRlZmF1bHRDb25uZWN0aW9uU3RyaW5nKSB7XG4gICAgICAgICAgZGVmYXVsdENvbm5lY3Rpb25TdHJpbmcuZm9jdXMoKTtcbiAgICAgICAgfVxuICAgICAgfSwgMCk7XG4gICAgfVxuICB9XG59XG4iXX0=