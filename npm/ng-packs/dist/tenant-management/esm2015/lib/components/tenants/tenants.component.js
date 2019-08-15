/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { ConfirmationService } from '@abp/ng.theme.shared';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize, pluck, switchMap, take } from 'rxjs/operators';
import { CreateTenant, DeleteTenant, GetTenants, GetTenantById, UpdateTenant, } from '../../actions/tenant-management.actions';
import { TenantManagementService } from '../../services/tenant-management.service';
import { TenantManagementState } from '../../states/tenant-management.state';
export class TenantsComponent {
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
        this.pageQuery = {
            sorting: 'name',
        };
        this.loading = false;
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
            defaultConnectionString: this.defaultConnectionString || '',
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
    onAddTenant() {
        this.selected = (/** @type {?} */ ({}));
        this.createTenantForm();
        this.openModal('AbpTenantManagement::NewTenant', this.tenantModalTemplate, 'saveTenant');
    }
    /**
     * @param {?} id
     * @return {?}
     */
    onEditTenant(id) {
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
        if (this.useSharedDatabase) {
            this.tenantService
                .deleteDefaultConnectionString(this.selected.id)
                .pipe(take(1))
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
                .pipe(take(1))
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
        if (!this.tenantForm.valid)
            return;
        this.store
            .dispatch(this.selected.id
            ? new UpdateTenant(Object.assign({}, this.tenantForm.value, { id: this.selected.id }))
            : new CreateTenant(this.tenantForm.value))
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
}
TenantsComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-tenants',
                template: "<div id=\"wrapper\" class=\"card\">\n  <div class=\"card-header\">\n    <div class=\"row\">\n      <div class=\"col col-md-6\">\n        <h5 class=\"card-title\">\n          {{ 'AbpTenantManagement::Tenants' | abpLocalization }}\n        </h5>\n      </div>\n      <div class=\"text-right col col-md-6\">\n        <button\n          [abpPermission]=\"'AbpTenantManagement.Tenants.Create'\"\n          id=\"create-tenants\"\n          class=\"btn btn-primary\"\n          type=\"button\"\n          (click)=\"onAddTenant()\"\n        >\n          <i class=\"fa fa-plus mr-1\"></i>\n          <span>{{ 'AbpTenantManagement::NewTenant' | abpLocalization }}</span>\n        </button>\n      </div>\n    </div>\n  </div>\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          [placeholder]=\"'AbpUi::PagerSearch' | abpLocalization\"\n          (input.debounce)=\"onSearch($event.target.value)\"\n      /></label>\n    </div>\n    <p-table\n      [value]=\"data$ | async\"\n      [lazy]=\"true\"\n      [lazyLoadOnInit]=\"false\"\n      [paginator]=\"true\"\n      [rows]=\"10\"\n      [totalRecords]=\"totalCount$ | async\"\n      [loading]=\"loading\"\n      (onLazyLoad)=\"onPageChange($event)\"\n    >\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpTenantManagement::Actions' | abpLocalization }}</th>\n          <th>{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpTenantManagement::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Update'\"\n                  ngbDropdownItem\n                  (click)=\"onEditTenant(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::Edit' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageConnectionStrings'\"\n                  ngbDropdownItem\n                  (click)=\"onEditConnectionString(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::ConnectionStrings' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Delete'\"\n                  ngbDropdownItem\n                  (click)=\"delete(data.id, data.name)\"\n                >\n                  {{ 'AbpTenantManagement::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal [(visible)]=\"isModalVisible\">\n  <ng-template #abpHeader>\n    <h3>{{ selectedModalContent.title | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <ng-container *ngTemplateOutlet=\"selectedModalContent?.template\"></ng-container>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n    </button>\n    <abp-button\n      [requestType]=\"['POST', 'PUT', 'DELETE']\"\n      requestURLContainSearchValue=\"multi-tenancy\"\n      iconClass=\"fa fa-check\"\n      (click)=\"save()\"\n      >{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button\n    >\n  </ng-template>\n</abp-modal>\n\n<ng-template #tenantModalTemplate>\n  <form [formGroup]=\"tenantForm\" (ngSubmit)=\"save()\">\n    <div class=\"mt-2\">\n      <div class=\"form-group\">\n        <label for=\"name\">{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</label>\n        <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" autofocus />\n      </div>\n    </div>\n  </form>\n</ng-template>\n\n<ng-template #connectionStringModalTemplate>\n  <form [formGroup]=\"defaultConnectionStringForm\" (ngSubmit)=\"save()\">\n    <div class=\"mt-2\">\n      <div class=\"form-group\">\n        <div class=\"form-check\">\n          <input\n            id=\"useSharedDatabase\"\n            type=\"checkbox\"\n            class=\"form-check-input\"\n            formControlName=\"useSharedDatabase\"\n            autofocus\n          />\n          <label for=\"useSharedDatabase\" class=\"font-check-label\">{{\n            'AbpTenantManagement::DisplayName:UseSharedDatabase' | abpLocalization\n          }}</label>\n        </div>\n      </div>\n      <div class=\"form-group\" *ngIf=\"!useSharedDatabase\">\n        <label for=\"defaultConnectionString\">{{\n          'AbpTenantManagement::DisplayName:DefaultConnectionString' | abpLocalization\n        }}</label>\n        <input\n          type=\"text\"\n          id=\"defaultConnectionString\"\n          class=\"form-control\"\n          formControlName=\"defaultConnectionString\"\n        />\n      </div>\n    </div>\n  </form>\n</ng-template>\n"
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
tslib_1.__decorate([
    Select(TenantManagementState.get),
    tslib_1.__metadata("design:type", Observable)
], TenantsComponent.prototype, "data$", void 0);
tslib_1.__decorate([
    Select(TenantManagementState.getTenantsTotalCount),
    tslib_1.__metadata("design:type", Observable)
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
    TenantsComponent.prototype._useSharedDatabase;
    /** @type {?} */
    TenantsComponent.prototype.pageQuery;
    /** @type {?} */
    TenantsComponent.prototype.loading;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50cy5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvdGVuYW50cy90ZW5hbnRzLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUNBLE9BQU8sRUFBRSxtQkFBbUIsRUFBVyxNQUFNLHNCQUFzQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxTQUFTLEVBQUUsV0FBVyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUNsRSxPQUFPLEVBQUUsV0FBVyxFQUFhLFVBQVUsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxVQUFVLEVBQVcsTUFBTSxNQUFNLENBQUM7QUFDM0MsT0FBTyxFQUFnQixRQUFRLEVBQUUsS0FBSyxFQUFFLFNBQVMsRUFBRSxJQUFJLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNoRixPQUFPLEVBQ0wsWUFBWSxFQUNaLFlBQVksRUFDWixVQUFVLEVBQ1YsYUFBYSxFQUNiLFlBQVksR0FDYixNQUFNLHlDQUF5QyxDQUFDO0FBQ2pELE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxNQUFNLDBDQUEwQyxDQUFDO0FBQ25GLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLHNDQUFzQyxDQUFDO0FBWTdFLE1BQU0sT0FBTyxnQkFBZ0I7Ozs7Ozs7SUF5QzNCLFlBQ1UsbUJBQXdDLEVBQ3hDLGFBQXNDLEVBQ3RDLEVBQWUsRUFDZixLQUFZO1FBSFosd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQUN4QyxrQkFBYSxHQUFiLGFBQWEsQ0FBeUI7UUFDdEMsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUNmLFVBQUssR0FBTCxLQUFLLENBQU87UUE1QnRCLHlCQUFvQixHQUFHLG1CQUFBLEVBQUUsRUFBd0Is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REFDZ0Q7Ozs7O0lBRzlDLCtDQUFnRDs7Ozs7SUFDaEQseUNBQThDOzs7OztJQUM5Qyw4QkFBdUI7Ozs7O0lBQ3ZCLGlDQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBDb25maXJtYXRpb25TZXJ2aWNlLCBUb2FzdGVyIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgQ29tcG9uZW50LCBUZW1wbGF0ZVJlZiwgVmlld0NoaWxkIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBGb3JtQnVpbGRlciwgRm9ybUdyb3VwLCBWYWxpZGF0b3JzIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUsIFN1YmplY3QgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGRlYm91bmNlVGltZSwgZmluYWxpemUsIHBsdWNrLCBzd2l0Y2hNYXAsIHRha2UgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQge1xuICBDcmVhdGVUZW5hbnQsXG4gIERlbGV0ZVRlbmFudCxcbiAgR2V0VGVuYW50cyxcbiAgR2V0VGVuYW50QnlJZCxcbiAgVXBkYXRlVGVuYW50LFxufSBmcm9tICcuLi8uLi9hY3Rpb25zL3RlbmFudC1tYW5hZ2VtZW50LmFjdGlvbnMnO1xuaW1wb3J0IHsgVGVuYW50TWFuYWdlbWVudFNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcy90ZW5hbnQtbWFuYWdlbWVudC5zZXJ2aWNlJztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnRTdGF0ZSB9IGZyb20gJy4uLy4uL3N0YXRlcy90ZW5hbnQtbWFuYWdlbWVudC5zdGF0ZSc7XG5cbnR5cGUgU2VsZWN0ZWRNb2RhbENvbnRlbnQgPSB7XG4gIHR5cGU6IHN0cmluZztcbiAgdGl0bGU6IHN0cmluZztcbiAgdGVtcGxhdGU6IFRlbXBsYXRlUmVmPGFueT47XG59O1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtdGVuYW50cycsXG4gIHRlbXBsYXRlVXJsOiAnLi90ZW5hbnRzLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgVGVuYW50c0NvbXBvbmVudCB7XG4gIEBTZWxlY3QoVGVuYW50TWFuYWdlbWVudFN0YXRlLmdldClcbiAgZGF0YSQ6IE9ic2VydmFibGU8QUJQLkJhc2ljSXRlbVtdPjtcblxuICBAU2VsZWN0KFRlbmFudE1hbmFnZW1lbnRTdGF0ZS5nZXRUZW5hbnRzVG90YWxDb3VudClcbiAgdG90YWxDb3VudCQ6IE9ic2VydmFibGU8bnVtYmVyPjtcblxuICBzZWxlY3RlZDogQUJQLkJhc2ljSXRlbTtcblxuICB0ZW5hbnRGb3JtOiBGb3JtR3JvdXA7XG5cbiAgZGVmYXVsdENvbm5lY3Rpb25TdHJpbmdGb3JtOiBGb3JtR3JvdXA7XG5cbiAgZGVmYXVsdENvbm5lY3Rpb25TdHJpbmc6IHN0cmluZztcblxuICBpc01vZGFsVmlzaWJsZTogYm9vbGVhbjtcblxuICBzZWxlY3RlZE1vZGFsQ29udGVudCA9IHt9IGFzIFNlbGVjdGVkTW9kYWxDb250ZW50O1xuXG4gIF91c2VTaGFyZWREYXRhYmFzZTogYm9vbGVhbjtcblxuICBwYWdlUXVlcnk6IEFCUC5QYWdlUXVlcnlQYXJhbXMgPSB7XG4gICAgc29ydGluZzogJ25hbWUnLFxuICB9O1xuXG4gIGxvYWRpbmc6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBnZXQgdXNlU2hhcmVkRGF0YWJhc2UoKTogYm9vbGVhbiB7XG4gICAgcmV0dXJuIHRoaXMuZGVmYXVsdENvbm5lY3Rpb25TdHJpbmdGb3JtLmdldCgndXNlU2hhcmVkRGF0YWJhc2UnKS52YWx1ZTtcbiAgfVxuXG4gIGdldCBjb25uZWN0aW9uU3RyaW5nKCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHRoaXMuZGVmYXVsdENvbm5lY3Rpb25TdHJpbmdGb3JtLmdldCgnZGVmYXVsdENvbm5lY3Rpb25TdHJpbmcnKS52YWx1ZTtcbiAgfVxuXG4gIEBWaWV3Q2hpbGQoJ3RlbmFudE1vZGFsVGVtcGxhdGUnLCB7IHN0YXRpYzogZmFsc2UgfSlcbiAgdGVuYW50TW9kYWxUZW1wbGF0ZTogVGVtcGxhdGVSZWY8YW55PjtcblxuICBAVmlld0NoaWxkKCdjb25uZWN0aW9uU3RyaW5nTW9kYWxUZW1wbGF0ZScsIHsgc3RhdGljOiBmYWxzZSB9KVxuICBjb25uZWN0aW9uU3RyaW5nTW9kYWxUZW1wbGF0ZTogVGVtcGxhdGVSZWY8YW55PjtcblxuICBjb25zdHJ1Y3RvcihcbiAgICBwcml2YXRlIGNvbmZpcm1hdGlvblNlcnZpY2U6IENvbmZpcm1hdGlvblNlcnZpY2UsXG4gICAgcHJpdmF0ZSB0ZW5hbnRTZXJ2aWNlOiBUZW5hbnRNYW5hZ2VtZW50U2VydmljZSxcbiAgICBwcml2YXRlIGZiOiBGb3JtQnVpbGRlcixcbiAgICBwcml2YXRlIHN0b3JlOiBTdG9yZSxcbiAgKSB7fVxuXG4gIG9uU2VhcmNoKHZhbHVlKSB7XG4gICAgdGhpcy5wYWdlUXVlcnkuZmlsdGVyID0gdmFsdWU7XG4gICAgdGhpcy5nZXQoKTtcbiAgfVxuXG4gIHByaXZhdGUgY3JlYXRlVGVuYW50Rm9ybSgpIHtcbiAgICB0aGlzLnRlbmFudEZvcm0gPSB0aGlzLmZiLmdyb3VwKHtcbiAgICAgIG5hbWU6IFt0aGlzLnNlbGVjdGVkLm5hbWUgfHwgJycsIFtWYWxpZGF0b3JzLnJlcXVpcmVkLCBWYWxpZGF0b3JzLm1heExlbmd0aCgyNTYpXV0sXG4gICAgfSk7XG4gIH1cblxuICBwcml2YXRlIGNyZWF0ZURlZmF1bHRDb25uZWN0aW9uU3RyaW5nRm9ybSgpIHtcbiAgICB0aGlzLmRlZmF1bHRDb25uZWN0aW9uU3RyaW5nRm9ybSA9IHRoaXMuZmIuZ3JvdXAoe1xuICAgICAgdXNlU2hhcmVkRGF0YWJhc2U6IHRoaXMuX3VzZVNoYXJlZERhdGFiYXNlLFxuICAgICAgZGVmYXVsdENvbm5lY3Rpb25TdHJpbmc6IHRoaXMuZGVmYXVsdENvbm5lY3Rpb25TdHJpbmcgfHwgJycsXG4gICAgfSk7XG4gIH1cblxuICBvcGVuTW9kYWwodGl0bGU6IHN0cmluZywgdGVtcGxhdGU6IFRlbXBsYXRlUmVmPGFueT4sIHR5cGU6IHN0cmluZykge1xuICAgIHRoaXMuc2VsZWN0ZWRNb2RhbENvbnRlbnQgPSB7XG4gICAgICB0aXRsZSxcbiAgICAgIHRlbXBsYXRlLFxuICAgICAgdHlwZSxcbiAgICB9O1xuXG4gICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IHRydWU7XG4gIH1cblxuICBvbkVkaXRDb25uZWN0aW9uU3RyaW5nKGlkOiBzdHJpbmcpIHtcbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2gobmV3IEdldFRlbmFudEJ5SWQoaWQpKVxuICAgICAgLnBpcGUoXG4gICAgICAgIHBsdWNrKCdUZW5hbnRNYW5hZ2VtZW50U3RhdGUnLCAnc2VsZWN0ZWRJdGVtJyksXG4gICAgICAgIHN3aXRjaE1hcChzZWxlY3RlZCA9PiB7XG4gICAgICAgICAgdGhpcy5zZWxlY3RlZCA9IHNlbGVjdGVkO1xuICAgICAgICAgIHJldHVybiB0aGlzLnRlbmFudFNlcnZpY2UuZ2V0RGVmYXVsdENvbm5lY3Rpb25TdHJpbmcoaWQpO1xuICAgICAgICB9KSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoZmV0Y2hlZENvbm5lY3Rpb25TdHJpbmcgPT4ge1xuICAgICAgICB0aGlzLl91c2VTaGFyZWREYXRhYmFzZSA9IGZldGNoZWRDb25uZWN0aW9uU3RyaW5nID8gZmFsc2UgOiB0cnVlO1xuICAgICAgICB0aGlzLmRlZmF1bHRDb25uZWN0aW9uU3RyaW5nID0gZmV0Y2hlZENvbm5lY3Rpb25TdHJpbmcgPyBmZXRjaGVkQ29ubmVjdGlvblN0cmluZyA6ICcnO1xuICAgICAgICB0aGlzLmNyZWF0ZURlZmF1bHRDb25uZWN0aW9uU3RyaW5nRm9ybSgpO1xuICAgICAgICB0aGlzLm9wZW5Nb2RhbCgnQWJwVGVuYW50TWFuYWdlbWVudDo6Q29ubmVjdGlvblN0cmluZ3MnLCB0aGlzLmNvbm5lY3Rpb25TdHJpbmdNb2RhbFRlbXBsYXRlLCAnc2F2ZUNvbm5TdHInKTtcbiAgICAgIH0pO1xuICB9XG5cbiAgb25BZGRUZW5hbnQoKSB7XG4gICAgdGhpcy5zZWxlY3RlZCA9IHt9IGFzIEFCUC5CYXNpY0l0ZW07XG4gICAgdGhpcy5jcmVhdGVUZW5hbnRGb3JtKCk7XG4gICAgdGhpcy5vcGVuTW9kYWwoJ0FicFRlbmFudE1hbmFnZW1lbnQ6Ok5ld1RlbmFudCcsIHRoaXMudGVuYW50TW9kYWxUZW1wbGF0ZSwgJ3NhdmVUZW5hbnQnKTtcbiAgfVxuXG4gIG9uRWRpdFRlbmFudChpZDogc3RyaW5nKSB7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBHZXRUZW5hbnRCeUlkKGlkKSlcbiAgICAgIC5waXBlKHBsdWNrKCdUZW5hbnRNYW5hZ2VtZW50U3RhdGUnLCAnc2VsZWN0ZWRJdGVtJykpXG4gICAgICAuc3Vic2NyaWJlKHNlbGVjdGVkID0+IHtcbiAgICAgICAgdGhpcy5zZWxlY3RlZCA9IHNlbGVjdGVkO1xuICAgICAgICB0aGlzLmNyZWF0ZVRlbmFudEZvcm0oKTtcbiAgICAgICAgdGhpcy5vcGVuTW9kYWwoJ0FicFRlbmFudE1hbmFnZW1lbnQ6OkVkaXQnLCB0aGlzLnRlbmFudE1vZGFsVGVtcGxhdGUsICdzYXZlVGVuYW50Jyk7XG4gICAgICB9KTtcbiAgfVxuXG4gIHNhdmUoKSB7XG4gICAgY29uc3QgeyB0eXBlIH0gPSB0aGlzLnNlbGVjdGVkTW9kYWxDb250ZW50O1xuICAgIGlmICghdHlwZSkgcmV0dXJuO1xuICAgIGlmICh0eXBlID09PSAnc2F2ZVRlbmFudCcpIHRoaXMuc2F2ZVRlbmFudCgpO1xuICAgIGVsc2UgaWYgKHR5cGUgPT09ICdzYXZlQ29ublN0cicpIHRoaXMuc2F2ZUNvbm5lY3Rpb25TdHJpbmcoKTtcbiAgfVxuXG4gIHNhdmVDb25uZWN0aW9uU3RyaW5nKCkge1xuICAgIGlmICh0aGlzLnVzZVNoYXJlZERhdGFiYXNlKSB7XG4gICAgICB0aGlzLnRlbmFudFNlcnZpY2VcbiAgICAgICAgLmRlbGV0ZURlZmF1bHRDb25uZWN0aW9uU3RyaW5nKHRoaXMuc2VsZWN0ZWQuaWQpXG4gICAgICAgIC5waXBlKHRha2UoMSkpXG4gICAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSBmYWxzZTtcbiAgICAgICAgfSk7XG4gICAgfSBlbHNlIHtcbiAgICAgIHRoaXMudGVuYW50U2VydmljZVxuICAgICAgICAudXBkYXRlRGVmYXVsdENvbm5lY3Rpb25TdHJpbmcoeyBpZDogdGhpcy5zZWxlY3RlZC5pZCwgZGVmYXVsdENvbm5lY3Rpb25TdHJpbmc6IHRoaXMuY29ubmVjdGlvblN0cmluZyB9KVxuICAgICAgICAucGlwZSh0YWtlKDEpKVxuICAgICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gZmFsc2U7XG4gICAgICAgIH0pO1xuICAgIH1cbiAgfVxuXG4gIHNhdmVUZW5hbnQoKSB7XG4gICAgaWYgKCF0aGlzLnRlbmFudEZvcm0udmFsaWQpIHJldHVybjtcblxuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChcbiAgICAgICAgdGhpcy5zZWxlY3RlZC5pZFxuICAgICAgICAgID8gbmV3IFVwZGF0ZVRlbmFudCh7IC4uLnRoaXMudGVuYW50Rm9ybS52YWx1ZSwgaWQ6IHRoaXMuc2VsZWN0ZWQuaWQgfSlcbiAgICAgICAgICA6IG5ldyBDcmVhdGVUZW5hbnQodGhpcy50ZW5hbnRGb3JtLnZhbHVlKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gZmFsc2U7XG4gICAgICB9KTtcbiAgfVxuXG4gIGRlbGV0ZShpZDogc3RyaW5nLCBuYW1lOiBzdHJpbmcpIHtcbiAgICB0aGlzLmNvbmZpcm1hdGlvblNlcnZpY2VcbiAgICAgIC53YXJuKCdBYnBUZW5hbnRNYW5hZ2VtZW50OjpUZW5hbnREZWxldGlvbkNvbmZpcm1hdGlvbk1lc3NhZ2UnLCAnQWJwVGVuYW50TWFuYWdlbWVudDo6QXJlWW91U3VyZScsIHtcbiAgICAgICAgbWVzc2FnZUxvY2FsaXphdGlvblBhcmFtczogW25hbWVdLFxuICAgICAgfSlcbiAgICAgIC5zdWJzY3JpYmUoKHN0YXR1czogVG9hc3Rlci5TdGF0dXMpID0+IHtcbiAgICAgICAgaWYgKHN0YXR1cyA9PT0gVG9hc3Rlci5TdGF0dXMuY29uZmlybSkge1xuICAgICAgICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IERlbGV0ZVRlbmFudChpZCkpO1xuICAgICAgICB9XG4gICAgICB9KTtcbiAgfVxuXG4gIG9uUGFnZUNoYW5nZShkYXRhKSB7XG4gICAgdGhpcy5wYWdlUXVlcnkuc2tpcENvdW50ID0gZGF0YS5maXJzdDtcbiAgICB0aGlzLnBhZ2VRdWVyeS5tYXhSZXN1bHRDb3VudCA9IGRhdGEucm93cztcblxuICAgIHRoaXMuZ2V0KCk7XG4gIH1cblxuICBnZXQoKSB7XG4gICAgdGhpcy5sb2FkaW5nID0gdHJ1ZTtcbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2gobmV3IEdldFRlbmFudHModGhpcy5wYWdlUXVlcnkpKVxuICAgICAgLnBpcGUoZmluYWxpemUoKCkgPT4gKHRoaXMubG9hZGluZyA9IGZhbHNlKSkpXG4gICAgICAuc3Vic2NyaWJlKCk7XG4gIH1cbn1cbiJdfQ==