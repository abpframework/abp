/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { ConfirmationService } from '@abp/ng.theme.shared';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { pluck, switchMap, take } from 'rxjs/operators';
import { TenantManagementAdd, TenantManagementDelete, TenantManagementGetById, TenantManagementUpdate, } from '../../actions/tenant-management.actions';
import { TenantManagementService } from '../../services';
import { TenantManagementState } from '../../states/tenant-management.state';
export class TenantsComponent {
    /**
     * @param {?} confirmationService
     * @param {?} tenantService
     * @param {?} modalService
     * @param {?} fb
     * @param {?} store
     */
    constructor(confirmationService, tenantService, modalService, fb, store) {
        this.confirmationService = confirmationService;
        this.tenantService = tenantService;
        this.modalService = modalService;
        this.fb = fb;
        this.store = store;
    }
    /**
     * @return {?}
     */
    get showInput() {
        return !this.defaultConnectionStringForm.get('useSharedDatabase').value;
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
    openModal() {
        this.modalService.open(this.modalWrapper);
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
            useSharedDatabase: this.useSharedDatabase,
            defaultConnectionString: this.defaultConnectionString || '',
        });
    }
    /**
     * @param {?} id
     * @return {?}
     */
    onEditConnStr(id) {
        this.selectedModalContent = {
            title: this.selected && this.selected.id ? 'AbpTenantManagement::Edit' : 'AbpTenantManagement::NewTenant',
            template: this.mTemplateConnStr,
            onSave: (/**
             * @return {?}
             */
            () => this.saveConnStr),
        };
        this.store
            .dispatch(new TenantManagementGetById(id))
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
            this.useSharedDatabase = fetchedConnectionString ? false : true;
            this.defaultConnectionString = fetchedConnectionString ? fetchedConnectionString : '';
            this.createDefaultConnectionStringForm();
            this.openModal();
        }));
    }
    /**
     * @return {?}
     */
    saveConnStr() {
        this.tenantService
            .updateDefaultConnectionString({ id: this.selected.id, defaultConnectionString: this.connectionString })
            .pipe(take(1))
            .subscribe((/**
         * @return {?}
         */
        () => this.modalService.dismissAll()));
    }
    /**
     * @param {?} id
     * @return {?}
     */
    onManageFeatures(id) {
        this.selectedModalContent = {
            title: this.selected && this.selected.id ? 'AbpTenantManagement::Edit' : 'AbpTenantManagement::NewTenant',
            template: this.mTemplateFeatures,
            onSave: (/**
             * @return {?}
             */
            () => { }),
        };
        this.openModal();
    }
    /**
     * @return {?}
     */
    onAdd() {
        this.selected = (/** @type {?} */ ({}));
        this.createTenantForm();
        this.openModal();
        this.selectedModalContent = {
            title: 'AbpTenantManagement::NewTenant',
            template: this.mTemplateTenant,
            onSave: (/**
             * @return {?}
             */
            () => this.saveTenant),
        };
    }
    /**
     * @param {?} id
     * @return {?}
     */
    onEdit(id) {
        this.store
            .dispatch(new TenantManagementGetById(id))
            .pipe(pluck('TenantManagementState', 'selectedItem'))
            .subscribe((/**
         * @param {?} selected
         * @return {?}
         */
        selected => {
            this.selected = selected;
            this.selectedModalContent = {
                title: 'AbpTenantManagement::Edit',
                template: this.mTemplateTenant,
                onSave: (/**
                 * @return {?}
                 */
                () => this.saveTenant),
            };
            this.createTenantForm();
            this.openModal();
        }));
    }
    /**
     * @return {?}
     */
    saveTenant() {
        if (!this.tenantForm.valid)
            return;
        this.store
            .dispatch(this.selected.id
            ? new TenantManagementUpdate(Object.assign({}, this.tenantForm.value, { id: this.selected.id }))
            : new TenantManagementAdd(this.tenantForm.value))
            .subscribe((/**
         * @return {?}
         */
        () => this.modalService.dismissAll()));
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
                this.store.dispatch(new TenantManagementDelete(id));
            }
        }));
    }
}
TenantsComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-tenants',
                template: "<div id=\"wrapper\" class=\"card\">\n  <div class=\"card-header\">\n    <div class=\"row\">\n      <div class=\"col col-md-6\">\n        <h5 class=\"card-title\">\n          {{ 'AbpTenantManagement::Tenants' | abpLocalization }}\n        </h5>\n      </div>\n      <div class=\"text-right col col-md-6\">\n        <button\n          [abpPermission]=\"'AbpTenantManagement.Tenants.Create'\"\n          id=\"create-tenants\"\n          class=\"btn btn-primary\"\n          type=\"button\"\n          (click)=\"onAdd()\"\n        >\n          <i class=\"fa fa-plus mr-1\"></i>\n          <span>{{ 'AbpTenantManagement::NewTenant' | abpLocalization }}</span>\n        </button>\n      </div>\n    </div>\n  </div>\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          placeholder=\"Search\"\n          (input)=\"dt.filterGlobal($event.target.value, 'contains')\"\n      /></label>\n    </div>\n    <p-table #dt [value]=\"datas$ | async\" [globalFilterFields]=\"['name']\" [paginator]=\"true\" [rows]=\"10\">\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpTenantManagement::Actions' | abpLocalization }}</th>\n          <th>{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpTenantManagement::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Update'\"\n                  ngbDropdownItem\n                  (click)=\"onEdit(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::Edit' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageConnectionStrings'\"\n                  ngbDropdownItem\n                  (click)=\"onEditConnStr(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::ConnectionStrings' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageFeatures'\"\n                  ngbDropdownItem\n                  (click)=\"onManageFeatures(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::Features' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Delete'\"\n                  ngbDropdownItem\n                  (click)=\"delete(data.id, data.name)\"\n                >\n                  {{ 'AbpTenantManagement::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<ng-template #modalWrapper let-modal>\n  <div class=\"modal-header\">\n    <h5 class=\"modal-title\" id=\"modal-basic-title\">\n      {{ selectedModalContent.title | abpLocalization }}\n    </h5>\n    <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n      <span aria-hidden=\"true\">&times;</span>\n    </button>\n  </div>\n\n  <form [formGroup]=\"tenantForm\" (ngSubmit)=\"selectedModalContent.onSave()\">\n    <div class=\"modal-body\">\n      <ng-container *ngTemplateOutlet=\"selectedModalContent.template; context: { $implicit: modal }\"></ng-container>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpTenantManagement::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </form>\n</ng-template>\n\n<ng-template #mTemplateTenant let-modal>\n  <div class=\"mt-2\">\n    <div class=\"form-group\">\n      <label for=\"name\">{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</label>\n      <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n    </div>\n  </div>\n</ng-template>\n\n<ng-template #mTemplateConnStr let-modal>\n  <div class=\"mt-2\">\n    <div class=\"form-group\">\n      <div class=\"form-check\">\n        <input id=\"useSharedDatabase\" type=\"checkbox\" class=\"form-check-input\" formControlName=\"useSharedDatabase\" />\n        <label for=\"useSharedDatabase\" class=\"font-check-label\">{{\n          'AbpTenantManagement::DisplayName:UseSharedDatabase' | abpLocalization\n        }}</label>\n      </div>\n    </div>\n    <div class=\"form-group\" *ngIf=\"showInput\">\n      <label for=\"defaultConnectionString\">{{\n        'AbpTenantManagement::DisplayName:DefaultConnectionString' | abpLocalization\n      }}</label>\n      <input type=\"text\" id=\"defaultConnectionString\" class=\"form-control\" formControlName=\"defaultConnectionString\" />\n    </div>\n  </div>\n</ng-template>\n\n<ng-template #mTemplateFeatures let-modal>\n  Manage Features\n</ng-template>\n"
            }] }
];
/** @nocollapse */
TenantsComponent.ctorParameters = () => [
    { type: ConfirmationService },
    { type: TenantManagementService },
    { type: NgbModal },
    { type: FormBuilder },
    { type: Store }
];
TenantsComponent.propDecorators = {
    modalWrapper: [{ type: ViewChild, args: ['modalWrapper', { static: false },] }],
    mTemplateConnStr: [{ type: ViewChild, args: ['mTemplateConnStr', { static: false },] }],
    mTemplateFeatures: [{ type: ViewChild, args: ['mTemplateFeatures', { static: false },] }],
    mTemplateTenant: [{ type: ViewChild, args: ['mTemplateTenant', { static: false },] }]
};
tslib_1.__decorate([
    Select(TenantManagementState.get),
    tslib_1.__metadata("design:type", Observable)
], TenantsComponent.prototype, "datas$", void 0);
if (false) {
    /** @type {?} */
    TenantsComponent.prototype.datas$;
    /** @type {?} */
    TenantsComponent.prototype.selected;
    /** @type {?} */
    TenantsComponent.prototype.tenantForm;
    /** @type {?} */
    TenantsComponent.prototype.defaultConnectionStringForm;
    /** @type {?} */
    TenantsComponent.prototype.defaultConnectionString;
    /** @type {?} */
    TenantsComponent.prototype.useSharedDatabase;
    /** @type {?} */
    TenantsComponent.prototype.selectedModalContent;
    /** @type {?} */
    TenantsComponent.prototype.modalWrapper;
    /** @type {?} */
    TenantsComponent.prototype.mTemplateConnStr;
    /** @type {?} */
    TenantsComponent.prototype.mTemplateFeatures;
    /** @type {?} */
    TenantsComponent.prototype.mTemplateTenant;
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
    TenantsComponent.prototype.modalService;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50cy5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvdGVuYW50cy90ZW5hbnRzLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUNBLE9BQU8sRUFBRSxtQkFBbUIsRUFBVyxNQUFNLHNCQUFzQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxTQUFTLEVBQUUsV0FBVyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUNsRSxPQUFPLEVBQUUsV0FBVyxFQUFhLFVBQVUsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUN0RCxPQUFPLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1QyxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2xDLE9BQU8sRUFBRSxLQUFLLEVBQUUsU0FBUyxFQUFFLElBQUksRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3hELE9BQU8sRUFDTCxtQkFBbUIsRUFDbkIsc0JBQXNCLEVBQ3RCLHVCQUF1QixFQUN2QixzQkFBc0IsR0FDdkIsTUFBTSx5Q0FBeUMsQ0FBQztBQUNqRCxPQUFPLEVBQUUsdUJBQXVCLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUN6RCxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxzQ0FBc0MsQ0FBQztBQU03RSxNQUFNLE9BQU8sZ0JBQWdCOzs7Ozs7OztJQXdDM0IsWUFDVSxtQkFBd0MsRUFDeEMsYUFBc0MsRUFDdEMsWUFBc0IsRUFDdEIsRUFBZSxFQUNmLEtBQVk7UUFKWix3QkFBbUIsR0FBbkIsbUJBQW1CLENBQXFCO1FBQ3hDLGtCQUFhLEdBQWIsYUFBYSxDQUF5QjtRQUN0QyxpQkFBWSxHQUFaLFlBQVksQ0FBVTtRQUN0QixPQUFFLEdB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QlEsbUJBQW1CO1lBYW5CLHVCQUF1QjtZQVZ2QixRQUFRO1lBRFIsV0FBVztZQUVILEtBQUs7OzsyQkE0Q25CLFNBQVMsU0FBQyxjQUFjLEVBQUUsRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFOytCQUczQyxTQUFTLFNBQUMsa0JBQWtCLEVBQUUsRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFO2dDQUcvQyxTQUFTLFNBQUMsbUJBQW1CLEVBQUUsRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFOzhCQUdoRCxTQUFTLFNBQUMsaUJBQWlCLEVBQUUsRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFOztBQW5DL0M7SUFEQyxNQUFNLENBQUMscUJBQXFCLENBQUMsR0FBRyxDQUFDO3NDQUMxQixVQUFVO2dEQUFrQjs7O0lBRHBDLGtDQUNvQzs7SUFFcEMsb0NBQXdCOztJQUV4QixzQ0FBc0I7O0lBRXRCLHVEQUF1Qzs7SUFFdkMsbURBQWdDOztJQUVoQyw2Q0FBMkI7O0lBRTNCLGdEQUlFOztJQVVGLHdDQUMrQjs7SUFFL0IsNENBQ21DOztJQUVuQyw2Q0FDb0M7O0lBRXBDLDJDQUNrQzs7Ozs7SUFHaEMsK0NBQWdEOzs7OztJQUNoRCx5Q0FBOEM7Ozs7O0lBQzlDLHdDQUE4Qjs7Ozs7SUFDOUIsOEJBQXVCOzs7OztJQUN2QixpQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgQ29uZmlybWF0aW9uU2VydmljZSwgVG9hc3RlciB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IENvbXBvbmVudCwgVGVtcGxhdGVSZWYsIFZpZXdDaGlsZCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgRm9ybUJ1aWxkZXIsIEZvcm1Hcm91cCwgVmFsaWRhdG9ycyB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IE5nYk1vZGFsIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IHBsdWNrLCBzd2l0Y2hNYXAsIHRha2UgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQge1xuICBUZW5hbnRNYW5hZ2VtZW50QWRkLFxuICBUZW5hbnRNYW5hZ2VtZW50RGVsZXRlLFxuICBUZW5hbnRNYW5hZ2VtZW50R2V0QnlJZCxcbiAgVGVuYW50TWFuYWdlbWVudFVwZGF0ZSxcbn0gZnJvbSAnLi4vLi4vYWN0aW9ucy90ZW5hbnQtbWFuYWdlbWVudC5hY3Rpb25zJztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMnO1xuaW1wb3J0IHsgVGVuYW50TWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi4vLi4vc3RhdGVzL3RlbmFudC1tYW5hZ2VtZW50LnN0YXRlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXRlbmFudHMnLFxuICB0ZW1wbGF0ZVVybDogJy4vdGVuYW50cy5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFRlbmFudHNDb21wb25lbnQge1xuICBAU2VsZWN0KFRlbmFudE1hbmFnZW1lbnRTdGF0ZS5nZXQpXG4gIGRhdGFzJDogT2JzZXJ2YWJsZTxBQlAuQmFzaWNJdGVtW10+O1xuXG4gIHNlbGVjdGVkOiBBQlAuQmFzaWNJdGVtO1xuXG4gIHRlbmFudEZvcm06IEZvcm1Hcm91cDtcblxuICBkZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm06IEZvcm1Hcm91cDtcblxuICBkZWZhdWx0Q29ubmVjdGlvblN0cmluZzogc3RyaW5nO1xuXG4gIHVzZVNoYXJlZERhdGFiYXNlOiBib29sZWFuO1xuXG4gIHNlbGVjdGVkTW9kYWxDb250ZW50OiB7XG4gICAgdGl0bGU6IHN0cmluZztcbiAgICB0ZW1wbGF0ZTogVGVtcGxhdGVSZWY8YW55PjtcbiAgICBvblNhdmU6ICgpID0+IHZvaWQ7XG4gIH07XG5cbiAgZ2V0IHNob3dJbnB1dCgpOiBib29sZWFuIHtcbiAgICByZXR1cm4gIXRoaXMuZGVmYXVsdENvbm5lY3Rpb25TdHJpbmdGb3JtLmdldCgndXNlU2hhcmVkRGF0YWJhc2UnKS52YWx1ZTtcbiAgfVxuXG4gIGdldCBjb25uZWN0aW9uU3RyaW5nKCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHRoaXMuZGVmYXVsdENvbm5lY3Rpb25TdHJpbmdGb3JtLmdldCgnZGVmYXVsdENvbm5lY3Rpb25TdHJpbmcnKS52YWx1ZTtcbiAgfVxuXG4gIEBWaWV3Q2hpbGQoJ21vZGFsV3JhcHBlcicsIHsgc3RhdGljOiBmYWxzZSB9KVxuICBtb2RhbFdyYXBwZXI6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQFZpZXdDaGlsZCgnbVRlbXBsYXRlQ29ublN0cicsIHsgc3RhdGljOiBmYWxzZSB9KVxuICBtVGVtcGxhdGVDb25uU3RyOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBWaWV3Q2hpbGQoJ21UZW1wbGF0ZUZlYXR1cmVzJywgeyBzdGF0aWM6IGZhbHNlIH0pXG4gIG1UZW1wbGF0ZUZlYXR1cmVzOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBWaWV3Q2hpbGQoJ21UZW1wbGF0ZVRlbmFudCcsIHsgc3RhdGljOiBmYWxzZSB9KVxuICBtVGVtcGxhdGVUZW5hbnQ6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgY29uc3RydWN0b3IoXG4gICAgcHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlLFxuICAgIHByaXZhdGUgdGVuYW50U2VydmljZTogVGVuYW50TWFuYWdlbWVudFNlcnZpY2UsXG4gICAgcHJpdmF0ZSBtb2RhbFNlcnZpY2U6IE5nYk1vZGFsLFxuICAgIHByaXZhdGUgZmI6IEZvcm1CdWlsZGVyLFxuICAgIHByaXZhdGUgc3RvcmU6IFN0b3JlLFxuICApIHt9XG5cbiAgb3Blbk1vZGFsKCkge1xuICAgIHRoaXMubW9kYWxTZXJ2aWNlLm9wZW4odGhpcy5tb2RhbFdyYXBwZXIpO1xuICB9XG5cbiAgcHJpdmF0ZSBjcmVhdGVUZW5hbnRGb3JtKCkge1xuICAgIHRoaXMudGVuYW50Rm9ybSA9IHRoaXMuZmIuZ3JvdXAoe1xuICAgICAgbmFtZTogW3RoaXMuc2VsZWN0ZWQubmFtZSB8fCAnJywgW1ZhbGlkYXRvcnMucmVxdWlyZWQsIFZhbGlkYXRvcnMubWF4TGVuZ3RoKDI1NildXSxcbiAgICB9KTtcbiAgfVxuXG4gIHByaXZhdGUgY3JlYXRlRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdGb3JtKCkge1xuICAgIHRoaXMuZGVmYXVsdENvbm5lY3Rpb25TdHJpbmdGb3JtID0gdGhpcy5mYi5ncm91cCh7XG4gICAgICB1c2VTaGFyZWREYXRhYmFzZTogdGhpcy51c2VTaGFyZWREYXRhYmFzZSxcbiAgICAgIGRlZmF1bHRDb25uZWN0aW9uU3RyaW5nOiB0aGlzLmRlZmF1bHRDb25uZWN0aW9uU3RyaW5nIHx8ICcnLFxuICAgIH0pO1xuICB9XG5cbiAgb25FZGl0Q29ublN0cihpZDogc3RyaW5nKSB7XG4gICAgdGhpcy5zZWxlY3RlZE1vZGFsQ29udGVudCA9IHtcbiAgICAgIHRpdGxlOiB0aGlzLnNlbGVjdGVkICYmIHRoaXMuc2VsZWN0ZWQuaWQgPyAnQWJwVGVuYW50TWFuYWdlbWVudDo6RWRpdCcgOiAnQWJwVGVuYW50TWFuYWdlbWVudDo6TmV3VGVuYW50JyxcbiAgICAgIHRlbXBsYXRlOiB0aGlzLm1UZW1wbGF0ZUNvbm5TdHIsXG4gICAgICBvblNhdmU6ICgpID0+IHRoaXMuc2F2ZUNvbm5TdHIsXG4gICAgfTtcbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2gobmV3IFRlbmFudE1hbmFnZW1lbnRHZXRCeUlkKGlkKSlcbiAgICAgIC5waXBlKFxuICAgICAgICBwbHVjaygnVGVuYW50TWFuYWdlbWVudFN0YXRlJywgJ3NlbGVjdGVkSXRlbScpLFxuICAgICAgICBzd2l0Y2hNYXAoc2VsZWN0ZWQgPT4ge1xuICAgICAgICAgIHRoaXMuc2VsZWN0ZWQgPSBzZWxlY3RlZDtcbiAgICAgICAgICByZXR1cm4gdGhpcy50ZW5hbnRTZXJ2aWNlLmdldERlZmF1bHRDb25uZWN0aW9uU3RyaW5nKGlkKTtcbiAgICAgICAgfSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKGZldGNoZWRDb25uZWN0aW9uU3RyaW5nID0+IHtcbiAgICAgICAgdGhpcy51c2VTaGFyZWREYXRhYmFzZSA9IGZldGNoZWRDb25uZWN0aW9uU3RyaW5nID8gZmFsc2UgOiB0cnVlO1xuICAgICAgICB0aGlzLmRlZmF1bHRDb25uZWN0aW9uU3RyaW5nID0gZmV0Y2hlZENvbm5lY3Rpb25TdHJpbmcgPyBmZXRjaGVkQ29ubmVjdGlvblN0cmluZyA6ICcnO1xuICAgICAgICB0aGlzLmNyZWF0ZURlZmF1bHRDb25uZWN0aW9uU3RyaW5nRm9ybSgpO1xuICAgICAgICB0aGlzLm9wZW5Nb2RhbCgpO1xuICAgICAgfSk7XG4gIH1cblxuICBzYXZlQ29ublN0cigpIHtcbiAgICB0aGlzLnRlbmFudFNlcnZpY2VcbiAgICAgIC51cGRhdGVEZWZhdWx0Q29ubmVjdGlvblN0cmluZyh7IGlkOiB0aGlzLnNlbGVjdGVkLmlkLCBkZWZhdWx0Q29ubmVjdGlvblN0cmluZzogdGhpcy5jb25uZWN0aW9uU3RyaW5nIH0pXG4gICAgICAucGlwZSh0YWtlKDEpKVxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB0aGlzLm1vZGFsU2VydmljZS5kaXNtaXNzQWxsKCkpO1xuICB9XG5cbiAgb25NYW5hZ2VGZWF0dXJlcyhpZDogc3RyaW5nKSB7XG4gICAgdGhpcy5zZWxlY3RlZE1vZGFsQ29udGVudCA9IHtcbiAgICAgIHRpdGxlOiB0aGlzLnNlbGVjdGVkICYmIHRoaXMuc2VsZWN0ZWQuaWQgPyAnQWJwVGVuYW50TWFuYWdlbWVudDo6RWRpdCcgOiAnQWJwVGVuYW50TWFuYWdlbWVudDo6TmV3VGVuYW50JyxcbiAgICAgIHRlbXBsYXRlOiB0aGlzLm1UZW1wbGF0ZUZlYXR1cmVzLFxuICAgICAgb25TYXZlOiAoKSA9PiB7fSxcbiAgICB9O1xuICAgIHRoaXMub3Blbk1vZGFsKCk7XG4gIH1cblxuICBvbkFkZCgpIHtcbiAgICB0aGlzLnNlbGVjdGVkID0ge30gYXMgQUJQLkJhc2ljSXRlbTtcbiAgICB0aGlzLmNyZWF0ZVRlbmFudEZvcm0oKTtcbiAgICB0aGlzLm9wZW5Nb2RhbCgpO1xuICAgIHRoaXMuc2VsZWN0ZWRNb2RhbENvbnRlbnQgPSB7XG4gICAgICB0aXRsZTogJ0FicFRlbmFudE1hbmFnZW1lbnQ6Ok5ld1RlbmFudCcsXG4gICAgICB0ZW1wbGF0ZTogdGhpcy5tVGVtcGxhdGVUZW5hbnQsXG4gICAgICBvblNhdmU6ICgpID0+IHRoaXMuc2F2ZVRlbmFudCxcbiAgICB9O1xuICB9XG5cbiAgb25FZGl0KGlkOiBzdHJpbmcpIHtcbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2gobmV3IFRlbmFudE1hbmFnZW1lbnRHZXRCeUlkKGlkKSlcbiAgICAgIC5waXBlKHBsdWNrKCdUZW5hbnRNYW5hZ2VtZW50U3RhdGUnLCAnc2VsZWN0ZWRJdGVtJykpXG4gICAgICAuc3Vic2NyaWJlKHNlbGVjdGVkID0+IHtcbiAgICAgICAgdGhpcy5zZWxlY3RlZCA9IHNlbGVjdGVkO1xuICAgICAgICB0aGlzLnNlbGVjdGVkTW9kYWxDb250ZW50ID0ge1xuICAgICAgICAgIHRpdGxlOiAnQWJwVGVuYW50TWFuYWdlbWVudDo6RWRpdCcsXG4gICAgICAgICAgdGVtcGxhdGU6IHRoaXMubVRlbXBsYXRlVGVuYW50LFxuICAgICAgICAgIG9uU2F2ZTogKCkgPT4gdGhpcy5zYXZlVGVuYW50LFxuICAgICAgICB9O1xuICAgICAgICB0aGlzLmNyZWF0ZVRlbmFudEZvcm0oKTtcbiAgICAgICAgdGhpcy5vcGVuTW9kYWwoKTtcbiAgICAgIH0pO1xuICB9XG5cbiAgc2F2ZVRlbmFudCgpIHtcbiAgICBpZiAoIXRoaXMudGVuYW50Rm9ybS52YWxpZCkgcmV0dXJuO1xuXG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKFxuICAgICAgICB0aGlzLnNlbGVjdGVkLmlkXG4gICAgICAgICAgPyBuZXcgVGVuYW50TWFuYWdlbWVudFVwZGF0ZSh7IC4uLnRoaXMudGVuYW50Rm9ybS52YWx1ZSwgaWQ6IHRoaXMuc2VsZWN0ZWQuaWQgfSlcbiAgICAgICAgICA6IG5ldyBUZW5hbnRNYW5hZ2VtZW50QWRkKHRoaXMudGVuYW50Rm9ybS52YWx1ZSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHRoaXMubW9kYWxTZXJ2aWNlLmRpc21pc3NBbGwoKSk7XG4gIH1cblxuICBkZWxldGUoaWQ6IHN0cmluZywgbmFtZTogc3RyaW5nKSB7XG4gICAgdGhpcy5jb25maXJtYXRpb25TZXJ2aWNlXG4gICAgICAud2FybignQWJwVGVuYW50TWFuYWdlbWVudDo6VGVuYW50RGVsZXRpb25Db25maXJtYXRpb25NZXNzYWdlJywgJ0FicFRlbmFudE1hbmFnZW1lbnQ6OkFyZVlvdVN1cmUnLCB7XG4gICAgICAgIG1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXM6IFtuYW1lXSxcbiAgICAgIH0pXG4gICAgICAuc3Vic2NyaWJlKChzdGF0dXM6IFRvYXN0ZXIuU3RhdHVzKSA9PiB7XG4gICAgICAgIGlmIChzdGF0dXMgPT09IFRvYXN0ZXIuU3RhdHVzLmNvbmZpcm0pIHtcbiAgICAgICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBUZW5hbnRNYW5hZ2VtZW50RGVsZXRlKGlkKSk7XG4gICAgICAgIH1cbiAgICAgIH0pO1xuICB9XG59XG4iXX0=