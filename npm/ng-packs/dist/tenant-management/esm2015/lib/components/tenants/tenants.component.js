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
import { pluck, switchMap, take } from 'rxjs/operators';
import { TenantManagementAdd, TenantManagementDelete, TenantManagementGetById, TenantManagementUpdate, } from '../../actions/tenant-management.actions';
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
            this._useSharedDatabase = fetchedConnectionString ? false : true;
            this.defaultConnectionString = fetchedConnectionString ? fetchedConnectionString : '';
            this.createDefaultConnectionStringForm();
            this.openModal('AbpTenantManagement::ConnectionStrings', this.connectionStringModalTemplate, 'saveConnStr');
        }));
    }
    /**
     * @param {?} id
     * @return {?}
     */
    onManageFeatures(id) {
        this.openModal('AbpTenantManagement::Features', this.featuresModalTemplate, 'saveFeatures');
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
            .dispatch(new TenantManagementGetById(id))
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
            ? new TenantManagementUpdate(Object.assign({}, this.tenantForm.value, { id: this.selected.id }))
            : new TenantManagementAdd(this.tenantForm.value))
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
                this.store.dispatch(new TenantManagementDelete(id));
            }
        }));
    }
}
TenantsComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-tenants',
                template: "<div id=\"wrapper\" class=\"card\">\n  <div class=\"card-header\">\n    <div class=\"row\">\n      <div class=\"col col-md-6\">\n        <h5 class=\"card-title\">\n          {{ 'AbpTenantManagement::Tenants' | abpLocalization }}\n        </h5>\n      </div>\n      <div class=\"text-right col col-md-6\">\n        <button\n          [abpPermission]=\"'AbpTenantManagement.Tenants.Create'\"\n          id=\"create-tenants\"\n          class=\"btn btn-primary\"\n          type=\"button\"\n          (click)=\"onAddTenant()\"\n        >\n          <i class=\"fa fa-plus mr-1\"></i>\n          <span>{{ 'AbpTenantManagement::NewTenant' | abpLocalization }}</span>\n        </button>\n      </div>\n    </div>\n  </div>\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          placeholder=\"Search\"\n          (input)=\"dt.filterGlobal($event.target.value, 'contains')\"\n      /></label>\n    </div>\n    <p-table #dt [value]=\"datas$ | async\" [globalFilterFields]=\"['name']\" [paginator]=\"true\" [rows]=\"10\">\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpTenantManagement::Actions' | abpLocalization }}</th>\n          <th>{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpTenantManagement::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Update'\"\n                  ngbDropdownItem\n                  (click)=\"onEditTenant(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::Edit' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageConnectionStrings'\"\n                  ngbDropdownItem\n                  (click)=\"onEditConnectionString(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::ConnectionStrings' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageFeatures'\"\n                  ngbDropdownItem\n                  (click)=\"onManageFeatures(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::Features' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Delete'\"\n                  ngbDropdownItem\n                  (click)=\"delete(data.id, data.name)\"\n                >\n                  {{ 'AbpTenantManagement::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal [(visible)]=\"isModalVisible\" *ngIf=\"isModalVisible\">\n  <ng-template #abpHeader>\n    <h3>{{ selectedModalContent.title | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <ng-container *ngTemplateOutlet=\"selectedModalContent?.template\"></ng-container>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n    </button>\n    <button class=\"btn btn-primary\" type=\"button\" (click)=\"save()\">\n      {{ 'AbpTenantManagement::Save' | abpLocalization }}\n    </button>\n  </ng-template>\n</abp-modal>\n\n<ng-template #tenantModalTemplate>\n  <form [formGroup]=\"tenantForm\">\n    <div class=\"mt-2\">\n      <div class=\"form-group\">\n        <label for=\"name\">{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</label>\n        <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n      </div>\n    </div>\n  </form>\n</ng-template>\n\n<ng-template #connectionStringModalTemplate>\n  <form [formGroup]=\"defaultConnectionStringForm\">\n    <div class=\"mt-2\">\n      <div class=\"form-group\">\n        <div class=\"form-check\">\n          <input id=\"useSharedDatabase\" type=\"checkbox\" class=\"form-check-input\" formControlName=\"useSharedDatabase\" />\n          <label for=\"useSharedDatabase\" class=\"font-check-label\">{{\n            'AbpTenantManagement::DisplayName:UseSharedDatabase' | abpLocalization\n          }}</label>\n        </div>\n      </div>\n      <div class=\"form-group\" *ngIf=\"!useSharedDatabase\">\n        <label for=\"defaultConnectionString\">{{\n          'AbpTenantManagement::DisplayName:DefaultConnectionString' | abpLocalization\n        }}</label>\n        <input\n          type=\"text\"\n          id=\"defaultConnectionString\"\n          class=\"form-control\"\n          formControlName=\"defaultConnectionString\"\n        />\n      </div>\n    </div>\n  </form>\n</ng-template>\n\n<ng-template #featuresModalTemplate>\n  Manage Features\n</ng-template>\n"
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
    connectionStringModalTemplate: [{ type: ViewChild, args: ['connectionStringModalTemplate', { static: false },] }],
    featuresModalTemplate: [{ type: ViewChild, args: ['featuresModalTemplate', { static: false },] }]
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
    TenantsComponent.prototype.isModalVisible;
    /** @type {?} */
    TenantsComponent.prototype.selectedModalContent;
    /** @type {?} */
    TenantsComponent.prototype._useSharedDatabase;
    /** @type {?} */
    TenantsComponent.prototype.tenantModalTemplate;
    /** @type {?} */
    TenantsComponent.prototype.connectionStringModalTemplate;
    /** @type {?} */
    TenantsComponent.prototype.featuresModalTemplate;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50cy5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvdGVuYW50cy90ZW5hbnRzLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUNBLE9BQU8sRUFBRSxtQkFBbUIsRUFBVyxNQUFNLHNCQUFzQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxTQUFTLEVBQUUsV0FBVyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUNsRSxPQUFPLEVBQUUsV0FBVyxFQUFhLFVBQVUsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDbEMsT0FBTyxFQUFFLEtBQUssRUFBRSxTQUFTLEVBQUUsSUFBSSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDeEQsT0FBTyxFQUNMLG1CQUFtQixFQUNuQixzQkFBc0IsRUFDdEIsdUJBQXVCLEVBQ3ZCLHNCQUFzQixHQUN2QixNQUFNLHlDQUF5QyxDQUFDO0FBQ2pELE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxNQUFNLDBDQUEwQyxDQUFDO0FBQ25GLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLHNDQUFzQyxDQUFDO0FBWTdFLE1BQU0sT0FBTyxnQkFBZ0I7Ozs7Ozs7SUFtQzNCLFlBQ1UsbUJBQXdDLEVBQ3hDLGFBQXNDLEVBQ3RDLEVBQWUsRUFDZixLQUFZO1FBSFosd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQUN4QyxrQkFBYSxHQUFiLGFBQWEsQ0FBeUI7UUFDdEMsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUNmLFVBQUssR0FBTCxLQUFLLENBQU87UUF6QnRCLHlCQUFvQixHQUFHLG1CQUFBLEVBQUUsRUFBd0IsQ0FBQztJQTBC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Q0FnRG5CLFNBQVMsU0FBQyxxQkFBcUIsRUFBRSxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUU7NENBR2xELFNBQVMsU0FBQywrQkFBK0IsRUFBRSxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUU7b0NBRzVELFNBQVMsU0FBQyx1QkFBdUIsRUFBRSxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUU7O0FBOUJyRDtJQURDLE1BQU0sQ0FBQyxxQkFBcUIsQ0FBQyxHQUFHLENBQUM7c0NBQzFCLFVBQVU7Z0RBQWtCOzs7SUFEcEMsa0NBQ29DOztJQUVwQyxvQ0FBd0I7O0lBRXhCLHNDQUFzQjs7SUFFdEIsdURBQXVDOztJQUV2QyxtREFBZ0M7O0lBRWhDLDBDQUF3Qjs7SUFFeEIsZ0RBQWtEOztJQUVsRCw4Q0FBNEI7O0lBVTVCLCtDQUNzQzs7SUFFdEMseURBQ2dEOztJQUVoRCxpREFDd0M7Ozs7O0lBR3RDLCtDQUFnRDs7Ozs7SUFDaEQseUNBQThDOzs7OztJQUM5Qyw4QkFBdUI7Ozs7O0lBQ3ZCLGlDQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBDb25maXJtYXRpb25TZXJ2aWNlLCBUb2FzdGVyIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgQ29tcG9uZW50LCBUZW1wbGF0ZVJlZiwgVmlld0NoaWxkIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBGb3JtQnVpbGRlciwgRm9ybUdyb3VwLCBWYWxpZGF0b3JzIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IHBsdWNrLCBzd2l0Y2hNYXAsIHRha2UgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQge1xuICBUZW5hbnRNYW5hZ2VtZW50QWRkLFxuICBUZW5hbnRNYW5hZ2VtZW50RGVsZXRlLFxuICBUZW5hbnRNYW5hZ2VtZW50R2V0QnlJZCxcbiAgVGVuYW50TWFuYWdlbWVudFVwZGF0ZSxcbn0gZnJvbSAnLi4vLi4vYWN0aW9ucy90ZW5hbnQtbWFuYWdlbWVudC5hY3Rpb25zJztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQuc2VydmljZSc7XG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuLi8uLi9zdGF0ZXMvdGVuYW50LW1hbmFnZW1lbnQuc3RhdGUnO1xuXG50eXBlIFNlbGVjdGVkTW9kYWxDb250ZW50ID0ge1xuICB0eXBlOiBzdHJpbmc7XG4gIHRpdGxlOiBzdHJpbmc7XG4gIHRlbXBsYXRlOiBUZW1wbGF0ZVJlZjxhbnk+O1xufTtcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXRlbmFudHMnLFxuICB0ZW1wbGF0ZVVybDogJy4vdGVuYW50cy5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFRlbmFudHNDb21wb25lbnQge1xuICBAU2VsZWN0KFRlbmFudE1hbmFnZW1lbnRTdGF0ZS5nZXQpXG4gIGRhdGFzJDogT2JzZXJ2YWJsZTxBQlAuQmFzaWNJdGVtW10+O1xuXG4gIHNlbGVjdGVkOiBBQlAuQmFzaWNJdGVtO1xuXG4gIHRlbmFudEZvcm06IEZvcm1Hcm91cDtcblxuICBkZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm06IEZvcm1Hcm91cDtcblxuICBkZWZhdWx0Q29ubmVjdGlvblN0cmluZzogc3RyaW5nO1xuXG4gIGlzTW9kYWxWaXNpYmxlOiBib29sZWFuO1xuXG4gIHNlbGVjdGVkTW9kYWxDb250ZW50ID0ge30gYXMgU2VsZWN0ZWRNb2RhbENvbnRlbnQ7XG5cbiAgX3VzZVNoYXJlZERhdGFiYXNlOiBib29sZWFuO1xuXG4gIGdldCB1c2VTaGFyZWREYXRhYmFzZSgpOiBib29sZWFuIHtcbiAgICByZXR1cm4gdGhpcy5kZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0uZ2V0KCd1c2VTaGFyZWREYXRhYmFzZScpLnZhbHVlO1xuICB9XG5cbiAgZ2V0IGNvbm5lY3Rpb25TdHJpbmcoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy5kZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0uZ2V0KCdkZWZhdWx0Q29ubmVjdGlvblN0cmluZycpLnZhbHVlO1xuICB9XG5cbiAgQFZpZXdDaGlsZCgndGVuYW50TW9kYWxUZW1wbGF0ZScsIHsgc3RhdGljOiBmYWxzZSB9KVxuICB0ZW5hbnRNb2RhbFRlbXBsYXRlOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBWaWV3Q2hpbGQoJ2Nvbm5lY3Rpb25TdHJpbmdNb2RhbFRlbXBsYXRlJywgeyBzdGF0aWM6IGZhbHNlIH0pXG4gIGNvbm5lY3Rpb25TdHJpbmdNb2RhbFRlbXBsYXRlOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBWaWV3Q2hpbGQoJ2ZlYXR1cmVzTW9kYWxUZW1wbGF0ZScsIHsgc3RhdGljOiBmYWxzZSB9KVxuICBmZWF0dXJlc01vZGFsVGVtcGxhdGU6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgY29uc3RydWN0b3IoXG4gICAgcHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlLFxuICAgIHByaXZhdGUgdGVuYW50U2VydmljZTogVGVuYW50TWFuYWdlbWVudFNlcnZpY2UsXG4gICAgcHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIsXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXG4gICkge31cblxuICBwcml2YXRlIGNyZWF0ZVRlbmFudEZvcm0oKSB7XG4gICAgdGhpcy50ZW5hbnRGb3JtID0gdGhpcy5mYi5ncm91cCh7XG4gICAgICBuYW1lOiBbdGhpcy5zZWxlY3RlZC5uYW1lIHx8ICcnLCBbVmFsaWRhdG9ycy5yZXF1aXJlZCwgVmFsaWRhdG9ycy5tYXhMZW5ndGgoMjU2KV1dLFxuICAgIH0pO1xuICB9XG5cbiAgcHJpdmF0ZSBjcmVhdGVEZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0oKSB7XG4gICAgdGhpcy5kZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0gPSB0aGlzLmZiLmdyb3VwKHtcbiAgICAgIHVzZVNoYXJlZERhdGFiYXNlOiB0aGlzLl91c2VTaGFyZWREYXRhYmFzZSxcbiAgICAgIGRlZmF1bHRDb25uZWN0aW9uU3RyaW5nOiB0aGlzLmRlZmF1bHRDb25uZWN0aW9uU3RyaW5nIHx8ICcnLFxuICAgIH0pO1xuICB9XG5cbiAgb3Blbk1vZGFsKHRpdGxlOiBzdHJpbmcsIHRlbXBsYXRlOiBUZW1wbGF0ZVJlZjxhbnk+LCB0eXBlOiBzdHJpbmcpIHtcbiAgICB0aGlzLnNlbGVjdGVkTW9kYWxDb250ZW50ID0ge1xuICAgICAgdGl0bGUsXG4gICAgICB0ZW1wbGF0ZSxcbiAgICAgIHR5cGUsXG4gICAgfTtcblxuICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSB0cnVlO1xuICB9XG5cbiAgb25FZGl0Q29ubmVjdGlvblN0cmluZyhpZDogc3RyaW5nKSB7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBUZW5hbnRNYW5hZ2VtZW50R2V0QnlJZChpZCkpXG4gICAgICAucGlwZShcbiAgICAgICAgcGx1Y2soJ1RlbmFudE1hbmFnZW1lbnRTdGF0ZScsICdzZWxlY3RlZEl0ZW0nKSxcbiAgICAgICAgc3dpdGNoTWFwKHNlbGVjdGVkID0+IHtcbiAgICAgICAgICB0aGlzLnNlbGVjdGVkID0gc2VsZWN0ZWQ7XG4gICAgICAgICAgcmV0dXJuIHRoaXMudGVuYW50U2VydmljZS5nZXREZWZhdWx0Q29ubmVjdGlvblN0cmluZyhpZCk7XG4gICAgICAgIH0pLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZShmZXRjaGVkQ29ubmVjdGlvblN0cmluZyA9PiB7XG4gICAgICAgIHRoaXMuX3VzZVNoYXJlZERhdGFiYXNlID0gZmV0Y2hlZENvbm5lY3Rpb25TdHJpbmcgPyBmYWxzZSA6IHRydWU7XG4gICAgICAgIHRoaXMuZGVmYXVsdENvbm5lY3Rpb25TdHJpbmcgPSBmZXRjaGVkQ29ubmVjdGlvblN0cmluZyA/IGZldGNoZWRDb25uZWN0aW9uU3RyaW5nIDogJyc7XG4gICAgICAgIHRoaXMuY3JlYXRlRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdGb3JtKCk7XG4gICAgICAgIHRoaXMub3Blbk1vZGFsKCdBYnBUZW5hbnRNYW5hZ2VtZW50OjpDb25uZWN0aW9uU3RyaW5ncycsIHRoaXMuY29ubmVjdGlvblN0cmluZ01vZGFsVGVtcGxhdGUsICdzYXZlQ29ublN0cicpO1xuICAgICAgfSk7XG4gIH1cblxuICBvbk1hbmFnZUZlYXR1cmVzKGlkOiBzdHJpbmcpIHtcbiAgICB0aGlzLm9wZW5Nb2RhbCgnQWJwVGVuYW50TWFuYWdlbWVudDo6RmVhdHVyZXMnLCB0aGlzLmZlYXR1cmVzTW9kYWxUZW1wbGF0ZSwgJ3NhdmVGZWF0dXJlcycpO1xuICB9XG5cbiAgb25BZGRUZW5hbnQoKSB7XG4gICAgdGhpcy5zZWxlY3RlZCA9IHt9IGFzIEFCUC5CYXNpY0l0ZW07XG4gICAgdGhpcy5jcmVhdGVUZW5hbnRGb3JtKCk7XG4gICAgdGhpcy5vcGVuTW9kYWwoJ0FicFRlbmFudE1hbmFnZW1lbnQ6Ok5ld1RlbmFudCcsIHRoaXMudGVuYW50TW9kYWxUZW1wbGF0ZSwgJ3NhdmVUZW5hbnQnKTtcbiAgfVxuXG4gIG9uRWRpdFRlbmFudChpZDogc3RyaW5nKSB7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBUZW5hbnRNYW5hZ2VtZW50R2V0QnlJZChpZCkpXG4gICAgICAucGlwZShwbHVjaygnVGVuYW50TWFuYWdlbWVudFN0YXRlJywgJ3NlbGVjdGVkSXRlbScpKVxuICAgICAgLnN1YnNjcmliZShzZWxlY3RlZCA9PiB7XG4gICAgICAgIHRoaXMuc2VsZWN0ZWQgPSBzZWxlY3RlZDtcbiAgICAgICAgdGhpcy5jcmVhdGVUZW5hbnRGb3JtKCk7XG4gICAgICAgIHRoaXMub3Blbk1vZGFsKCdBYnBUZW5hbnRNYW5hZ2VtZW50OjpFZGl0JywgdGhpcy50ZW5hbnRNb2RhbFRlbXBsYXRlLCAnc2F2ZVRlbmFudCcpO1xuICAgICAgfSk7XG4gIH1cblxuICBzYXZlKCkge1xuICAgIGNvbnN0IHsgdHlwZSB9ID0gdGhpcy5zZWxlY3RlZE1vZGFsQ29udGVudDtcbiAgICBpZiAoIXR5cGUpIHJldHVybjtcbiAgICBpZiAodHlwZSA9PT0gJ3NhdmVUZW5hbnQnKSB0aGlzLnNhdmVUZW5hbnQoKTtcbiAgICBlbHNlIGlmICh0eXBlID09PSAnc2F2ZUNvbm5TdHInKSB0aGlzLnNhdmVDb25uZWN0aW9uU3RyaW5nKCk7XG4gIH1cblxuICBzYXZlQ29ubmVjdGlvblN0cmluZygpIHtcbiAgICBpZiAodGhpcy51c2VTaGFyZWREYXRhYmFzZSkge1xuICAgICAgdGhpcy50ZW5hbnRTZXJ2aWNlXG4gICAgICAgIC5kZWxldGVEZWZhdWx0Q29ubmVjdGlvblN0cmluZyh0aGlzLnNlbGVjdGVkLmlkKVxuICAgICAgICAucGlwZSh0YWtlKDEpKVxuICAgICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gZmFsc2U7XG4gICAgICAgIH0pO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aGlzLnRlbmFudFNlcnZpY2VcbiAgICAgICAgLnVwZGF0ZURlZmF1bHRDb25uZWN0aW9uU3RyaW5nKHsgaWQ6IHRoaXMuc2VsZWN0ZWQuaWQsIGRlZmF1bHRDb25uZWN0aW9uU3RyaW5nOiB0aGlzLmNvbm5lY3Rpb25TdHJpbmcgfSlcbiAgICAgICAgLnBpcGUodGFrZSgxKSlcbiAgICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IGZhbHNlO1xuICAgICAgICB9KTtcbiAgICB9XG4gIH1cblxuICBzYXZlVGVuYW50KCkge1xuICAgIGlmICghdGhpcy50ZW5hbnRGb3JtLnZhbGlkKSByZXR1cm47XG5cbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2goXG4gICAgICAgIHRoaXMuc2VsZWN0ZWQuaWRcbiAgICAgICAgICA/IG5ldyBUZW5hbnRNYW5hZ2VtZW50VXBkYXRlKHsgLi4udGhpcy50ZW5hbnRGb3JtLnZhbHVlLCBpZDogdGhpcy5zZWxlY3RlZC5pZCB9KVxuICAgICAgICAgIDogbmV3IFRlbmFudE1hbmFnZW1lbnRBZGQodGhpcy50ZW5hbnRGb3JtLnZhbHVlKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gZmFsc2U7XG4gICAgICB9KTtcbiAgfVxuXG4gIGRlbGV0ZShpZDogc3RyaW5nLCBuYW1lOiBzdHJpbmcpIHtcbiAgICB0aGlzLmNvbmZpcm1hdGlvblNlcnZpY2VcbiAgICAgIC53YXJuKCdBYnBUZW5hbnRNYW5hZ2VtZW50OjpUZW5hbnREZWxldGlvbkNvbmZpcm1hdGlvbk1lc3NhZ2UnLCAnQWJwVGVuYW50TWFuYWdlbWVudDo6QXJlWW91U3VyZScsIHtcbiAgICAgICAgbWVzc2FnZUxvY2FsaXphdGlvblBhcmFtczogW25hbWVdLFxuICAgICAgfSlcbiAgICAgIC5zdWJzY3JpYmUoKHN0YXR1czogVG9hc3Rlci5TdGF0dXMpID0+IHtcbiAgICAgICAgaWYgKHN0YXR1cyA9PT0gVG9hc3Rlci5TdGF0dXMuY29uZmlybSkge1xuICAgICAgICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFRlbmFudE1hbmFnZW1lbnREZWxldGUoaWQpKTtcbiAgICAgICAgfVxuICAgICAgfSk7XG4gIH1cbn1cbiJdfQ==