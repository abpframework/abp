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
        if (this.useSharedDatabase) {
            this.tenantService
                .deleteDefaultConnectionString(this.selected.id)
                .pipe(take(1))
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
                .pipe(take(1))
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
            ? new UpdateTenant(tslib_1.__assign({}, this.tenantForm.value, { id: this.selected.id }))
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
            .pipe(finalize((/**
         * @return {?}
         */
        function () { return (_this.loading = false); })))
            .subscribe();
    };
    TenantsComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-tenants',
                    template: "<div id=\"wrapper\" class=\"card\">\n  <div class=\"card-header\">\n    <div class=\"row\">\n      <div class=\"col col-md-6\">\n        <h5 class=\"card-title\">\n          {{ 'AbpTenantManagement::Tenants' | abpLocalization }}\n        </h5>\n      </div>\n      <div class=\"text-right col col-md-6\">\n        <button\n          [abpPermission]=\"'AbpTenantManagement.Tenants.Create'\"\n          id=\"create-tenants\"\n          class=\"btn btn-primary\"\n          type=\"button\"\n          (click)=\"onAddTenant()\"\n        >\n          <i class=\"fa fa-plus mr-1\"></i>\n          <span>{{ 'AbpTenantManagement::NewTenant' | abpLocalization }}</span>\n        </button>\n      </div>\n    </div>\n  </div>\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          [placeholder]=\"'AbpUi::PagerSearch' | abpLocalization\"\n          (input.debounce)=\"onSearch($event.target.value)\"\n      /></label>\n    </div>\n    <p-table\n      [value]=\"data$ | async\"\n      [lazy]=\"true\"\n      [lazyLoadOnInit]=\"false\"\n      [paginator]=\"true\"\n      [rows]=\"10\"\n      [totalRecords]=\"totalCount$ | async\"\n      [loading]=\"loading\"\n      (onLazyLoad)=\"onPageChange($event)\"\n    >\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpTenantManagement::Actions' | abpLocalization }}</th>\n          <th>{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpTenantManagement::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Update'\"\n                  ngbDropdownItem\n                  (click)=\"onEditTenant(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::Edit' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.ManageConnectionStrings'\"\n                  ngbDropdownItem\n                  (click)=\"onEditConnectionString(data.id)\"\n                >\n                  {{ 'AbpTenantManagement::ConnectionStrings' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpTenantManagement.Tenants.Delete'\"\n                  ngbDropdownItem\n                  (click)=\"delete(data.id, data.name)\"\n                >\n                  {{ 'AbpTenantManagement::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal [(visible)]=\"isModalVisible\">\n  <ng-template #abpHeader>\n    <h3>{{ selectedModalContent.title | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <ng-container *ngTemplateOutlet=\"selectedModalContent?.template\"></ng-container>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button #abpClose type=\"button\" class=\"btn btn-secondary\">\n      {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n    </button>\n    <abp-button\n      [requestType]=\"['POST', 'PUT', 'DELETE']\"\n      requestURLContainSearchValue=\"multi-tenancy\"\n      iconClass=\"fa fa-check\"\n      (click)=\"save()\"\n      >{{ 'AbpIdentity::Save' | abpLocalization }}</abp-button\n    >\n  </ng-template>\n</abp-modal>\n\n<ng-template #tenantModalTemplate>\n  <form [formGroup]=\"tenantForm\" (ngSubmit)=\"save()\">\n    <div class=\"mt-2\">\n      <div class=\"form-group\">\n        <label for=\"name\">{{ 'AbpTenantManagement::TenantName' | abpLocalization }}</label>\n        <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" autofocus />\n      </div>\n    </div>\n  </form>\n</ng-template>\n\n<ng-template #connectionStringModalTemplate>\n  <form [formGroup]=\"defaultConnectionStringForm\" (ngSubmit)=\"save()\">\n    <div class=\"mt-2\">\n      <div class=\"form-group\">\n        <div class=\"form-check\">\n          <input\n            id=\"useSharedDatabase\"\n            type=\"checkbox\"\n            class=\"form-check-input\"\n            formControlName=\"useSharedDatabase\"\n            autofocus\n          />\n          <label for=\"useSharedDatabase\" class=\"font-check-label\">{{\n            'AbpTenantManagement::DisplayName:UseSharedDatabase' | abpLocalization\n          }}</label>\n        </div>\n      </div>\n      <div class=\"form-group\" *ngIf=\"!useSharedDatabase\">\n        <label for=\"defaultConnectionString\">{{\n          'AbpTenantManagement::DisplayName:DefaultConnectionString' | abpLocalization\n        }}</label>\n        <input\n          type=\"text\"\n          id=\"defaultConnectionString\"\n          class=\"form-control\"\n          formControlName=\"defaultConnectionString\"\n        />\n      </div>\n    </div>\n  </form>\n</ng-template>\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50cy5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvdGVuYW50cy90ZW5hbnRzLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUNBLE9BQU8sRUFBRSxtQkFBbUIsRUFBVyxNQUFNLHNCQUFzQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxTQUFTLEVBQUUsV0FBVyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUNsRSxPQUFPLEVBQUUsV0FBVyxFQUFhLFVBQVUsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxVQUFVLEVBQVcsTUFBTSxNQUFNLENBQUM7QUFDM0MsT0FBTyxFQUFnQixRQUFRLEVBQUUsS0FBSyxFQUFFLFNBQVMsRUFBRSxJQUFJLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNoRixPQUFPLEVBQ0wsWUFBWSxFQUNaLFlBQVksRUFDWixVQUFVLEVBQ1YsYUFBYSxFQUNiLFlBQVksR0FDYixNQUFNLHlDQUF5QyxDQUFDO0FBQ2pELE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxNQUFNLDBDQUEwQyxDQUFDO0FBQ25GLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLHNDQUFzQyxDQUFDO0FBUTdFO0lBNkNFLDBCQUNVLG1CQUF3QyxFQUN4QyxhQUFzQyxFQUN0QyxFQUFlLEVBQ2YsS0FBWTtRQUhaLHdCQUFtQixHQUFuQixtQkFBbUIsQ0FBcUI7UUFDeEMsa0JBQWEsR0FBYixhQUFhLENBQXlCO1FBQ3RDLE9BQUUsR0FBRixFQUFFLENBQWE7UUFDZixVQUFLLEdBQUwsS0FBSyxDQUFPO1FBNUJ0Qix5QkFBb0IsR0FBRyxtQkFBQSxFQUFFLEVBQXdCLENBQUM7UUFJbEQsY0FBUyxHQUF3QjtZ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QUMzQixpQ0FDbUM7O0lBRW5DLHVDQUNnQzs7SUFFaEMsb0NBQXdCOztJQUV4QixzQ0FBc0I7O0lBRXRCLHVEQUF1Qzs7SUFFdkMsbURBQWdDOztJQUVoQywwQ0FBd0I7O0lBRXhCLGdEQUFrRDs7SUFFbEQsOENBQTRCOztJQUU1QixxQ0FFRTs7SUFFRixtQ0FBeUI7O0lBVXpCLCtDQUNzQzs7SUFFdEMseURBQ2dEOzs7OztJQUc5QywrQ0FBZ0Q7Ozs7O0lBQ2hELHlDQUE4Qzs7Ozs7SUFDOUMsOEJBQXVCOzs7OztJQUN2QixpQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgQ29uZmlybWF0aW9uU2VydmljZSwgVG9hc3RlciB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IENvbXBvbmVudCwgVGVtcGxhdGVSZWYsIFZpZXdDaGlsZCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgRm9ybUJ1aWxkZXIsIEZvcm1Hcm91cCwgVmFsaWRhdG9ycyB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IFNlbGVjdCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBPYnNlcnZhYmxlLCBTdWJqZWN0IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBkZWJvdW5jZVRpbWUsIGZpbmFsaXplLCBwbHVjaywgc3dpdGNoTWFwLCB0YWtlIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHtcbiAgQ3JlYXRlVGVuYW50LFxuICBEZWxldGVUZW5hbnQsXG4gIEdldFRlbmFudHMsXG4gIEdldFRlbmFudEJ5SWQsXG4gIFVwZGF0ZVRlbmFudCxcbn0gZnJvbSAnLi4vLi4vYWN0aW9ucy90ZW5hbnQtbWFuYWdlbWVudC5hY3Rpb25zJztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQuc2VydmljZSc7XG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuLi8uLi9zdGF0ZXMvdGVuYW50LW1hbmFnZW1lbnQuc3RhdGUnO1xuXG50eXBlIFNlbGVjdGVkTW9kYWxDb250ZW50ID0ge1xuICB0eXBlOiBzdHJpbmc7XG4gIHRpdGxlOiBzdHJpbmc7XG4gIHRlbXBsYXRlOiBUZW1wbGF0ZVJlZjxhbnk+O1xufTtcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXRlbmFudHMnLFxuICB0ZW1wbGF0ZVVybDogJy4vdGVuYW50cy5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFRlbmFudHNDb21wb25lbnQge1xuICBAU2VsZWN0KFRlbmFudE1hbmFnZW1lbnRTdGF0ZS5nZXQpXG4gIGRhdGEkOiBPYnNlcnZhYmxlPEFCUC5CYXNpY0l0ZW1bXT47XG5cbiAgQFNlbGVjdChUZW5hbnRNYW5hZ2VtZW50U3RhdGUuZ2V0VGVuYW50c1RvdGFsQ291bnQpXG4gIHRvdGFsQ291bnQkOiBPYnNlcnZhYmxlPG51bWJlcj47XG5cbiAgc2VsZWN0ZWQ6IEFCUC5CYXNpY0l0ZW07XG5cbiAgdGVuYW50Rm9ybTogRm9ybUdyb3VwO1xuXG4gIGRlZmF1bHRDb25uZWN0aW9uU3RyaW5nRm9ybTogRm9ybUdyb3VwO1xuXG4gIGRlZmF1bHRDb25uZWN0aW9uU3RyaW5nOiBzdHJpbmc7XG5cbiAgaXNNb2RhbFZpc2libGU6IGJvb2xlYW47XG5cbiAgc2VsZWN0ZWRNb2RhbENvbnRlbnQgPSB7fSBhcyBTZWxlY3RlZE1vZGFsQ29udGVudDtcblxuICBfdXNlU2hhcmVkRGF0YWJhc2U6IGJvb2xlYW47XG5cbiAgcGFnZVF1ZXJ5OiBBQlAuUGFnZVF1ZXJ5UGFyYW1zID0ge1xuICAgIHNvcnRpbmc6ICduYW1lJyxcbiAgfTtcblxuICBsb2FkaW5nOiBib29sZWFuID0gZmFsc2U7XG5cbiAgZ2V0IHVzZVNoYXJlZERhdGFiYXNlKCk6IGJvb2xlYW4ge1xuICAgIHJldHVybiB0aGlzLmRlZmF1bHRDb25uZWN0aW9uU3RyaW5nRm9ybS5nZXQoJ3VzZVNoYXJlZERhdGFiYXNlJykudmFsdWU7XG4gIH1cblxuICBnZXQgY29ubmVjdGlvblN0cmluZygpOiBzdHJpbmcge1xuICAgIHJldHVybiB0aGlzLmRlZmF1bHRDb25uZWN0aW9uU3RyaW5nRm9ybS5nZXQoJ2RlZmF1bHRDb25uZWN0aW9uU3RyaW5nJykudmFsdWU7XG4gIH1cblxuICBAVmlld0NoaWxkKCd0ZW5hbnRNb2RhbFRlbXBsYXRlJywgeyBzdGF0aWM6IGZhbHNlIH0pXG4gIHRlbmFudE1vZGFsVGVtcGxhdGU6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQFZpZXdDaGlsZCgnY29ubmVjdGlvblN0cmluZ01vZGFsVGVtcGxhdGUnLCB7IHN0YXRpYzogZmFsc2UgfSlcbiAgY29ubmVjdGlvblN0cmluZ01vZGFsVGVtcGxhdGU6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgY29uc3RydWN0b3IoXG4gICAgcHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlLFxuICAgIHByaXZhdGUgdGVuYW50U2VydmljZTogVGVuYW50TWFuYWdlbWVudFNlcnZpY2UsXG4gICAgcHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIsXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXG4gICkge31cblxuICBvblNlYXJjaCh2YWx1ZSkge1xuICAgIHRoaXMucGFnZVF1ZXJ5LmZpbHRlciA9IHZhbHVlO1xuICAgIHRoaXMuZ2V0KCk7XG4gIH1cblxuICBwcml2YXRlIGNyZWF0ZVRlbmFudEZvcm0oKSB7XG4gICAgdGhpcy50ZW5hbnRGb3JtID0gdGhpcy5mYi5ncm91cCh7XG4gICAgICBuYW1lOiBbdGhpcy5zZWxlY3RlZC5uYW1lIHx8ICcnLCBbVmFsaWRhdG9ycy5yZXF1aXJlZCwgVmFsaWRhdG9ycy5tYXhMZW5ndGgoMjU2KV1dLFxuICAgIH0pO1xuICB9XG5cbiAgcHJpdmF0ZSBjcmVhdGVEZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0oKSB7XG4gICAgdGhpcy5kZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0gPSB0aGlzLmZiLmdyb3VwKHtcbiAgICAgIHVzZVNoYXJlZERhdGFiYXNlOiB0aGlzLl91c2VTaGFyZWREYXRhYmFzZSxcbiAgICAgIGRlZmF1bHRDb25uZWN0aW9uU3RyaW5nOiB0aGlzLmRlZmF1bHRDb25uZWN0aW9uU3RyaW5nIHx8ICcnLFxuICAgIH0pO1xuICB9XG5cbiAgb3Blbk1vZGFsKHRpdGxlOiBzdHJpbmcsIHRlbXBsYXRlOiBUZW1wbGF0ZVJlZjxhbnk+LCB0eXBlOiBzdHJpbmcpIHtcbiAgICB0aGlzLnNlbGVjdGVkTW9kYWxDb250ZW50ID0ge1xuICAgICAgdGl0bGUsXG4gICAgICB0ZW1wbGF0ZSxcbiAgICAgIHR5cGUsXG4gICAgfTtcblxuICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSB0cnVlO1xuICB9XG5cbiAgb25FZGl0Q29ubmVjdGlvblN0cmluZyhpZDogc3RyaW5nKSB7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBHZXRUZW5hbnRCeUlkKGlkKSlcbiAgICAgIC5waXBlKFxuICAgICAgICBwbHVjaygnVGVuYW50TWFuYWdlbWVudFN0YXRlJywgJ3NlbGVjdGVkSXRlbScpLFxuICAgICAgICBzd2l0Y2hNYXAoc2VsZWN0ZWQgPT4ge1xuICAgICAgICAgIHRoaXMuc2VsZWN0ZWQgPSBzZWxlY3RlZDtcbiAgICAgICAgICByZXR1cm4gdGhpcy50ZW5hbnRTZXJ2aWNlLmdldERlZmF1bHRDb25uZWN0aW9uU3RyaW5nKGlkKTtcbiAgICAgICAgfSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKGZldGNoZWRDb25uZWN0aW9uU3RyaW5nID0+IHtcbiAgICAgICAgdGhpcy5fdXNlU2hhcmVkRGF0YWJhc2UgPSBmZXRjaGVkQ29ubmVjdGlvblN0cmluZyA/IGZhbHNlIDogdHJ1ZTtcbiAgICAgICAgdGhpcy5kZWZhdWx0Q29ubmVjdGlvblN0cmluZyA9IGZldGNoZWRDb25uZWN0aW9uU3RyaW5nID8gZmV0Y2hlZENvbm5lY3Rpb25TdHJpbmcgOiAnJztcbiAgICAgICAgdGhpcy5jcmVhdGVEZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0oKTtcbiAgICAgICAgdGhpcy5vcGVuTW9kYWwoJ0FicFRlbmFudE1hbmFnZW1lbnQ6OkNvbm5lY3Rpb25TdHJpbmdzJywgdGhpcy5jb25uZWN0aW9uU3RyaW5nTW9kYWxUZW1wbGF0ZSwgJ3NhdmVDb25uU3RyJyk7XG4gICAgICB9KTtcbiAgfVxuXG4gIG9uQWRkVGVuYW50KCkge1xuICAgIHRoaXMuc2VsZWN0ZWQgPSB7fSBhcyBBQlAuQmFzaWNJdGVtO1xuICAgIHRoaXMuY3JlYXRlVGVuYW50Rm9ybSgpO1xuICAgIHRoaXMub3Blbk1vZGFsKCdBYnBUZW5hbnRNYW5hZ2VtZW50OjpOZXdUZW5hbnQnLCB0aGlzLnRlbmFudE1vZGFsVGVtcGxhdGUsICdzYXZlVGVuYW50Jyk7XG4gIH1cblxuICBvbkVkaXRUZW5hbnQoaWQ6IHN0cmluZykge1xuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChuZXcgR2V0VGVuYW50QnlJZChpZCkpXG4gICAgICAucGlwZShwbHVjaygnVGVuYW50TWFuYWdlbWVudFN0YXRlJywgJ3NlbGVjdGVkSXRlbScpKVxuICAgICAgLnN1YnNjcmliZShzZWxlY3RlZCA9PiB7XG4gICAgICAgIHRoaXMuc2VsZWN0ZWQgPSBzZWxlY3RlZDtcbiAgICAgICAgdGhpcy5jcmVhdGVUZW5hbnRGb3JtKCk7XG4gICAgICAgIHRoaXMub3Blbk1vZGFsKCdBYnBUZW5hbnRNYW5hZ2VtZW50OjpFZGl0JywgdGhpcy50ZW5hbnRNb2RhbFRlbXBsYXRlLCAnc2F2ZVRlbmFudCcpO1xuICAgICAgfSk7XG4gIH1cblxuICBzYXZlKCkge1xuICAgIGNvbnN0IHsgdHlwZSB9ID0gdGhpcy5zZWxlY3RlZE1vZGFsQ29udGVudDtcbiAgICBpZiAoIXR5cGUpIHJldHVybjtcbiAgICBpZiAodHlwZSA9PT0gJ3NhdmVUZW5hbnQnKSB0aGlzLnNhdmVUZW5hbnQoKTtcbiAgICBlbHNlIGlmICh0eXBlID09PSAnc2F2ZUNvbm5TdHInKSB0aGlzLnNhdmVDb25uZWN0aW9uU3RyaW5nKCk7XG4gIH1cblxuICBzYXZlQ29ubmVjdGlvblN0cmluZygpIHtcbiAgICBpZiAodGhpcy51c2VTaGFyZWREYXRhYmFzZSkge1xuICAgICAgdGhpcy50ZW5hbnRTZXJ2aWNlXG4gICAgICAgIC5kZWxldGVEZWZhdWx0Q29ubmVjdGlvblN0cmluZyh0aGlzLnNlbGVjdGVkLmlkKVxuICAgICAgICAucGlwZSh0YWtlKDEpKVxuICAgICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gZmFsc2U7XG4gICAgICAgIH0pO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aGlzLnRlbmFudFNlcnZpY2VcbiAgICAgICAgLnVwZGF0ZURlZmF1bHRDb25uZWN0aW9uU3RyaW5nKHsgaWQ6IHRoaXMuc2VsZWN0ZWQuaWQsIGRlZmF1bHRDb25uZWN0aW9uU3RyaW5nOiB0aGlzLmNvbm5lY3Rpb25TdHJpbmcgfSlcbiAgICAgICAgLnBpcGUodGFrZSgxKSlcbiAgICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IGZhbHNlO1xuICAgICAgICB9KTtcbiAgICB9XG4gIH1cblxuICBzYXZlVGVuYW50KCkge1xuICAgIGlmICghdGhpcy50ZW5hbnRGb3JtLnZhbGlkKSByZXR1cm47XG5cbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2goXG4gICAgICAgIHRoaXMuc2VsZWN0ZWQuaWRcbiAgICAgICAgICA/IG5ldyBVcGRhdGVUZW5hbnQoeyAuLi50aGlzLnRlbmFudEZvcm0udmFsdWUsIGlkOiB0aGlzLnNlbGVjdGVkLmlkIH0pXG4gICAgICAgICAgOiBuZXcgQ3JlYXRlVGVuYW50KHRoaXMudGVuYW50Rm9ybS52YWx1ZSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IGZhbHNlO1xuICAgICAgfSk7XG4gIH1cblxuICBkZWxldGUoaWQ6IHN0cmluZywgbmFtZTogc3RyaW5nKSB7XG4gICAgdGhpcy5jb25maXJtYXRpb25TZXJ2aWNlXG4gICAgICAud2FybignQWJwVGVuYW50TWFuYWdlbWVudDo6VGVuYW50RGVsZXRpb25Db25maXJtYXRpb25NZXNzYWdlJywgJ0FicFRlbmFudE1hbmFnZW1lbnQ6OkFyZVlvdVN1cmUnLCB7XG4gICAgICAgIG1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXM6IFtuYW1lXSxcbiAgICAgIH0pXG4gICAgICAuc3Vic2NyaWJlKChzdGF0dXM6IFRvYXN0ZXIuU3RhdHVzKSA9PiB7XG4gICAgICAgIGlmIChzdGF0dXMgPT09IFRvYXN0ZXIuU3RhdHVzLmNvbmZpcm0pIHtcbiAgICAgICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBEZWxldGVUZW5hbnQoaWQpKTtcbiAgICAgICAgfVxuICAgICAgfSk7XG4gIH1cblxuICBvblBhZ2VDaGFuZ2UoZGF0YSkge1xuICAgIHRoaXMucGFnZVF1ZXJ5LnNraXBDb3VudCA9IGRhdGEuZmlyc3Q7XG4gICAgdGhpcy5wYWdlUXVlcnkubWF4UmVzdWx0Q291bnQgPSBkYXRhLnJvd3M7XG5cbiAgICB0aGlzLmdldCgpO1xuICB9XG5cbiAgZ2V0KCkge1xuICAgIHRoaXMubG9hZGluZyA9IHRydWU7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBHZXRUZW5hbnRzKHRoaXMucGFnZVF1ZXJ5KSlcbiAgICAgIC5waXBlKGZpbmFsaXplKCgpID0+ICh0aGlzLmxvYWRpbmcgPSBmYWxzZSkpKVxuICAgICAgLnN1YnNjcmliZSgpO1xuICB9XG59XG4iXX0=