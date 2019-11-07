/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { ConfirmationService } from '@abp/ng.theme.shared';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize, pluck, switchMap, take } from 'rxjs/operators';
import {
  CreateTenant,
  DeleteTenant,
  GetTenantById,
  GetTenants,
  UpdateTenant,
} from '../../actions/tenant-management.actions';
import { TenantManagementService } from '../../services/tenant-management.service';
import { TenantManagementState } from '../../states/tenant-management.state';
/**
 * @record
 */
function SelectedModalContent() {}
if (false) {
  /** @type {?} */
  SelectedModalContent.prototype.type;
  /** @type {?} */
  SelectedModalContent.prototype.title;
  /** @type {?} */
  SelectedModalContent.prototype.template;
}
var TenantsComponent = /** @class */ (function() {
  function TenantsComponent(confirmationService, tenantService, fb, store) {
    this.confirmationService = confirmationService;
    this.tenantService = tenantService;
    this.fb = fb;
    this.store = store;
    this.selectedModalContent = /** @type {?} */ ({});
    this.visibleFeatures = false;
    this.pageQuery = {};
    this.loading = false;
    this.modalBusy = false;
    this.sortOrder = '';
    this.sortKey = '';
  }
  Object.defineProperty(TenantsComponent.prototype, 'useSharedDatabase', {
    /**
     * @return {?}
     */
    get: function() {
      return this.defaultConnectionStringForm.get('useSharedDatabase').value;
    },
    enumerable: true,
    configurable: true,
  });
  Object.defineProperty(TenantsComponent.prototype, 'connectionString', {
    /**
     * @return {?}
     */
    get: function() {
      return this.defaultConnectionStringForm.get('defaultConnectionString').value;
    },
    enumerable: true,
    configurable: true,
  });
  /**
   * @return {?}
   */
  TenantsComponent.prototype.ngOnInit
  /**
   * @return {?}
   */ = function() {
    this.get();
  };
  /**
   * @param {?} value
   * @return {?}
   */
  TenantsComponent.prototype.onSearch
  /**
   * @param {?} value
   * @return {?}
   */ = function(value) {
    this.pageQuery.filter = value;
    this.get();
  };
  /**
   * @private
   * @return {?}
   */
  TenantsComponent.prototype.createTenantForm
  /**
   * @private
   * @return {?}
   */ = function() {
    this.tenantForm = this.fb.group({
      name: [this.selected.name || '', [Validators.required, Validators.maxLength(256)]],
    });
  };
  /**
   * @private
   * @return {?}
   */
  TenantsComponent.prototype.createDefaultConnectionStringForm
  /**
   * @private
   * @return {?}
   */ = function() {
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
  TenantsComponent.prototype.openModal
  /**
   * @param {?} title
   * @param {?} template
   * @param {?} type
   * @return {?}
   */ = function(title, template, type) {
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
  TenantsComponent.prototype.onEditConnectionString
  /**
   * @param {?} id
   * @return {?}
   */ = function(id) {
    var _this = this;
    this.store
      .dispatch(new GetTenantById(id))
      .pipe(
        pluck('TenantManagementState', 'selectedItem'),
        switchMap(
          /**
           * @param {?} selected
           * @return {?}
           */
          function(selected) {
            _this.selected = selected;
            return _this.tenantService.getDefaultConnectionString(id);
          },
        ),
      )
      .subscribe(
        /**
         * @param {?} fetchedConnectionString
         * @return {?}
         */
        function(fetchedConnectionString) {
          _this._useSharedDatabase = fetchedConnectionString ? false : true;
          _this.defaultConnectionString = fetchedConnectionString ? fetchedConnectionString : '';
          _this.createDefaultConnectionStringForm();
          _this.openModal('AbpTenantManagement::ConnectionStrings', _this.connectionStringModalTemplate, 'saveConnStr');
        },
      );
  };
  /**
   * @return {?}
   */
  TenantsComponent.prototype.onAddTenant
  /**
   * @return {?}
   */ = function() {
    this.selected = /** @type {?} */ ({});
    this.createTenantForm();
    this.openModal('AbpTenantManagement::NewTenant', this.tenantModalTemplate, 'saveTenant');
  };
  /**
   * @param {?} id
   * @return {?}
   */
  TenantsComponent.prototype.onEditTenant
  /**
   * @param {?} id
   * @return {?}
   */ = function(id) {
    var _this = this;
    this.store
      .dispatch(new GetTenantById(id))
      .pipe(pluck('TenantManagementState', 'selectedItem'))
      .subscribe(
        /**
         * @param {?} selected
         * @return {?}
         */
        function(selected) {
          _this.selected = selected;
          _this.createTenantForm();
          _this.openModal('AbpTenantManagement::Edit', _this.tenantModalTemplate, 'saveTenant');
        },
      );
  };
  /**
   * @return {?}
   */
  TenantsComponent.prototype.save
  /**
   * @return {?}
   */ = function() {
    var type = this.selectedModalContent.type;
    if (!type) return;
    if (type === 'saveTenant') this.saveTenant();
    else if (type === 'saveConnStr') this.saveConnectionString();
  };
  /**
   * @return {?}
   */
  TenantsComponent.prototype.saveConnectionString
  /**
   * @return {?}
   */ = function() {
    var _this = this;
    this.modalBusy = true;
    if (this.useSharedDatabase || (!this.useSharedDatabase && !this.connectionString)) {
      this.tenantService
        .deleteDefaultConnectionString(this.selected.id)
        .pipe(
          take(1),
          finalize(
            /**
             * @return {?}
             */
            function() {
              return (_this.modalBusy = false);
            },
          ),
        )
        .subscribe(
          /**
           * @return {?}
           */
          function() {
            _this.isModalVisible = false;
          },
        );
    } else {
      this.tenantService
        .updateDefaultConnectionString({ id: this.selected.id, defaultConnectionString: this.connectionString })
        .pipe(
          take(1),
          finalize(
            /**
             * @return {?}
             */
            function() {
              return (_this.modalBusy = false);
            },
          ),
        )
        .subscribe(
          /**
           * @return {?}
           */
          function() {
            _this.isModalVisible = false;
          },
        );
    }
  };
  /**
   * @return {?}
   */
  TenantsComponent.prototype.saveTenant
  /**
   * @return {?}
   */ = function() {
    var _this = this;
    if (!this.tenantForm.valid) return;
    this.modalBusy = true;
    this.store
      .dispatch(
        this.selected.id
          ? new UpdateTenant(tslib_1.__assign({}, this.tenantForm.value, { id: this.selected.id }))
          : new CreateTenant(this.tenantForm.value),
      )
      .pipe(
        finalize(
          /**
           * @return {?}
           */
          function() {
            return (_this.modalBusy = false);
          },
        ),
      )
      .subscribe(
        /**
         * @return {?}
         */
        function() {
          _this.isModalVisible = false;
        },
      );
  };
  /**
   * @param {?} id
   * @param {?} name
   * @return {?}
   */
  TenantsComponent.prototype.delete
  /**
   * @param {?} id
   * @param {?} name
   * @return {?}
   */ = function(id, name) {
    var _this = this;
    this.confirmationService
      .warn('AbpTenantManagement::TenantDeletionConfirmationMessage', 'AbpTenantManagement::AreYouSure', {
        messageLocalizationParams: [name],
      })
      .subscribe(
        /**
         * @param {?} status
         * @return {?}
         */
        function(status) {
          if (status === 'confirm' /* confirm */) {
            _this.store.dispatch(new DeleteTenant(id));
          }
        },
      );
  };
  /**
   * @param {?} data
   * @return {?}
   */
  TenantsComponent.prototype.onPageChange
  /**
   * @param {?} data
   * @return {?}
   */ = function(data) {
    this.pageQuery.skipCount = data.first;
    this.pageQuery.maxResultCount = data.rows;
    this.get();
  };
  /**
   * @return {?}
   */
  TenantsComponent.prototype.get
  /**
   * @return {?}
   */ = function() {
    var _this = this;
    this.loading = true;
    this.store
      .dispatch(new GetTenants(this.pageQuery))
      .pipe(
        finalize(
          /**
           * @return {?}
           */
          function() {
            return (_this.loading = false);
          },
        ),
      )
      .subscribe();
  };
  TenantsComponent.decorators = [
    {
      type: Component,
      args: [
        {
          selector: 'abp-tenants',
          template:
            '<div class="row entry-row">\n  <div class="col-auto">\n    <h1 class="content-header-title">{{ \'AbpTenantManagement::Tenants\' | abpLocalization }}</h1>\n  </div>\n  <div class="col">\n    <div class="text-lg-right pt-2" id="AbpContentToolbar">\n      <button\n        [abpPermission]="\'AbpTenantManagement.Tenants.Create\'"\n        id="create-tenants"\n        class="btn btn-primary"\n        type="button"\n        (click)="onAddTenant()"\n      >\n        <i class="fa fa-plus mr-1"></i>\n        <span>{{ \'AbpTenantManagement::NewTenant\' | abpLocalization }}</span>\n      </button>\n    </div>\n  </div>\n</div>\n\n<div id="wrapper" class="card">\n  <div class="card-body">\n    <div id="data-tables-table-filter" class="data-tables-filter">\n      <label\n        ><input\n          type="search"\n          class="form-control form-control-sm"\n          [placeholder]="\'AbpUi::PagerSearch\' | abpLocalization"\n          (input.debounce)="onSearch($event.target.value)"\n      /></label>\n    </div>\n    <p-table\n      *ngIf="[150, 0] as columnWidths"\n      [value]="data$ | async"\n      [abpTableSort]="{ key: sortKey, order: sortOrder }"\n      [lazy]="true"\n      [lazyLoadOnInit]="false"\n      [paginator]="true"\n      [rows]="10"\n      [totalRecords]="totalCount$ | async"\n      [loading]="loading"\n      [resizableColumns]="true"\n      [scrollable]="true"\n      (onLazyLoad)="onPageChange($event)"\n    >\n      <ng-template pTemplate="colgroup">\n        <colgroup>\n          <col *ngFor="let width of columnWidths" [ngStyle]="{ \'width.px\': width || undefined }" />\n        </colgroup>\n      </ng-template>\n      <ng-template pTemplate="emptymessage" let-columns>\n        <tr\n          abp-table-empty-message\n          [attr.colspan]="columnWidths.length"\n          localizationResource="AbpTenantManagement"\n          localizationProp="NoDataAvailableInDatatable"\n        ></tr>\n      </ng-template>\n      <ng-template pTemplate="header" let-columns>\n        <tr>\n          <th>{{ \'AbpTenantManagement::Actions\' | abpLocalization }}</th>\n          <th pResizableColumn (click)="sortOrderIcon.sort(\'name\')">\n            {{ \'AbpTenantManagement::TenantName\' | abpLocalization }}\n            <abp-sort-order-icon #sortOrderIcon key="name" [(selectedKey)]="sortKey" [(order)]="sortOrder">\n            </abp-sort-order-icon>\n          </th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate="body" let-data>\n        <tr>\n          <td class="text-center">\n            <div ngbDropdown container="body" class="d-inline-block">\n              <button\n                class="btn btn-primary btn-sm dropdown-toggle"\n                data-toggle="dropdown"\n                aria-haspopup="true"\n                ngbDropdownToggle\n              >\n                <i class="fa fa-cog mr-1"></i>{{ \'AbpTenantManagement::Actions\' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button\n                  [abpPermission]="\'AbpTenantManagement.Tenants.Update\'"\n                  ngbDropdownItem\n                  (click)="onEditTenant(data.id)"\n                >\n                  {{ \'AbpTenantManagement::Edit\' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]="\'AbpTenantManagement.Tenants.ManageConnectionStrings\'"\n                  ngbDropdownItem\n                  (click)="onEditConnectionString(data.id)"\n                >\n                  {{ \'AbpTenantManagement::Permission:ManageConnectionStrings\' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]="\'AbpTenantManagement.Tenants.ManageFeatures\'"\n                  ngbDropdownItem\n                  (click)="providerKey = data.id; visibleFeatures = true"\n                >\n                  {{ \'AbpTenantManagement::Permission:ManageFeatures\' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]="\'AbpTenantManagement.Tenants.Delete\'"\n                  ngbDropdownItem\n                  (click)="delete(data.id, data.name)"\n                >\n                  {{ \'AbpTenantManagement::Delete\' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal size="md" [(visible)]="isModalVisible" [busy]="modalBusy">\n  <ng-template #abpHeader>\n    <h3>{{ selectedModalContent.title | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <ng-container *ngTemplateOutlet="selectedModalContent?.template"></ng-container>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button #abpClose type="button" class="btn btn-secondary">\n      {{ \'AbpTenantManagement::Cancel\' | abpLocalization }}\n    </button>\n    <abp-button iconClass="fa fa-check" (click)="save()">{{ \'AbpIdentity::Save\' | abpLocalization }}</abp-button>\n  </ng-template>\n</abp-modal>\n\n<ng-template #tenantModalTemplate>\n  <form [formGroup]="tenantForm" (ngSubmit)="save()">\n    <div class="mt-2">\n      <div class="form-group">\n        <label for="name">{{ \'AbpTenantManagement::TenantName\' | abpLocalization }}</label>\n        <input type="text" id="name" class="form-control" formControlName="name" autofocus />\n      </div>\n    </div>\n  </form>\n</ng-template>\n\n<ng-template #connectionStringModalTemplate>\n  <form [formGroup]="defaultConnectionStringForm" (ngSubmit)="save()">\n    <label class="mt-2">\n      <div class="form-group">\n        <div class="custom-checkbox custom-control mb-2">\n          <input\n            id="useSharedDatabase"\n            type="checkbox"\n            class="custom-control-input"\n            formControlName="useSharedDatabase"\n            autofocus\n          />\n          <label for="useSharedDatabase" class="custom-control-label">{{\n            \'AbpTenantManagement::DisplayName:UseSharedDatabase\' | abpLocalization\n          }}</label>\n        </div>\n      </div>\n      <label class="form-group" *ngIf="!useSharedDatabase">\n        <label for="defaultConnectionString">{{\n          \'AbpTenantManagement::DisplayName:DefaultConnectionString\' | abpLocalization\n        }}</label>\n        <input\n          type="text"\n          id="defaultConnectionString"\n          class="form-control"\n          formControlName="defaultConnectionString"\n        />\n      </label>\n    </label>\n  </form>\n</ng-template>\n\n<abp-feature-management [(visible)]="visibleFeatures" providerName="T" [providerKey]="providerKey">\n</abp-feature-management>\n',
        },
      ],
    },
  ];
  /** @nocollapse */
  TenantsComponent.ctorParameters = function() {
    return [{ type: ConfirmationService }, { type: TenantManagementService }, { type: FormBuilder }, { type: Store }];
  };
  TenantsComponent.propDecorators = {
    tenantModalTemplate: [{ type: ViewChild, args: ['tenantModalTemplate', { static: false }] }],
    connectionStringModalTemplate: [{ type: ViewChild, args: ['connectionStringModalTemplate', { static: false }] }],
  };
  tslib_1.__decorate(
    [Select(TenantManagementState.get), tslib_1.__metadata('design:type', Observable)],
    TenantsComponent.prototype,
    'data$',
    void 0,
  );
  tslib_1.__decorate(
    [Select(TenantManagementState.getTenantsTotalCount), tslib_1.__metadata('design:type', Observable)],
    TenantsComponent.prototype,
    'totalCount$',
    void 0,
  );
  return TenantsComponent;
})();
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50cy5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvdGVuYW50cy90ZW5hbnRzLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUNBLE9BQU8sRUFBRSxtQkFBbUIsRUFBVyxNQUFNLHNCQUFzQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxTQUFTLEVBQVUsV0FBVyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMxRSxPQUFPLEVBQUUsV0FBVyxFQUFhLFVBQVUsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDbEMsT0FBTyxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQUUsU0FBUyxFQUFFLElBQUksRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ2xFLE9BQU8sRUFDTCxZQUFZLEVBQ1osWUFBWSxFQUNaLGFBQWEsRUFDYixVQUFVLEVBQ1YsWUFBWSxHQUNiLE1BQU0seUNBQXlDLENBQUM7QUFDakQsT0FBTyxFQUFFLHVCQUF1QixFQUFFLE1BQU0sMENBQTBDLENBQUM7QUFDbkYsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sc0NBQXNDLENBQUM7Ozs7QUFFN0UsbUNBSUM7OztJQUhDLG9DQUFhOztJQUNiLHFDQUFjOztJQUNkLHdDQUEyQjs7QUFHN0I7SUFxREUsMEJBQ1UsbUJBQXdDLEVBQ3hDLGFBQXNDLEVBQ3RDLEVBQWUsRUFDZixLQUFZO1FBSFosd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQUN4QyxrQkFBYSxHQUFiLGFBQWEsQ0FBeUI7UUFDdEMsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUNmLFVBQUssR0FBTCxLQUFLLENBQU87UUFwQ3RC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QUN2QixpQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgQ29uZmlybWF0aW9uU2VydmljZSwgVG9hc3RlciB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IENvbXBvbmVudCwgT25Jbml0LCBUZW1wbGF0ZVJlZiwgVmlld0NoaWxkIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBGb3JtQnVpbGRlciwgRm9ybUdyb3VwLCBWYWxpZGF0b3JzIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGZpbmFsaXplLCBwbHVjaywgc3dpdGNoTWFwLCB0YWtlIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHtcbiAgQ3JlYXRlVGVuYW50LFxuICBEZWxldGVUZW5hbnQsXG4gIEdldFRlbmFudEJ5SWQsXG4gIEdldFRlbmFudHMsXG4gIFVwZGF0ZVRlbmFudCxcbn0gZnJvbSAnLi4vLi4vYWN0aW9ucy90ZW5hbnQtbWFuYWdlbWVudC5hY3Rpb25zJztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQuc2VydmljZSc7XG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuLi8uLi9zdGF0ZXMvdGVuYW50LW1hbmFnZW1lbnQuc3RhdGUnO1xuXG5pbnRlcmZhY2UgU2VsZWN0ZWRNb2RhbENvbnRlbnQge1xuICB0eXBlOiBzdHJpbmc7XG4gIHRpdGxlOiBzdHJpbmc7XG4gIHRlbXBsYXRlOiBUZW1wbGF0ZVJlZjxhbnk+O1xufVxuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtdGVuYW50cycsXG4gIHRlbXBsYXRlVXJsOiAnLi90ZW5hbnRzLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgVGVuYW50c0NvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XG4gIEBTZWxlY3QoVGVuYW50TWFuYWdlbWVudFN0YXRlLmdldClcbiAgZGF0YSQ6IE9ic2VydmFibGU8QUJQLkJhc2ljSXRlbVtdPjtcblxuICBAU2VsZWN0KFRlbmFudE1hbmFnZW1lbnRTdGF0ZS5nZXRUZW5hbnRzVG90YWxDb3VudClcbiAgdG90YWxDb3VudCQ6IE9ic2VydmFibGU8bnVtYmVyPjtcblxuICBzZWxlY3RlZDogQUJQLkJhc2ljSXRlbTtcblxuICB0ZW5hbnRGb3JtOiBGb3JtR3JvdXA7XG5cbiAgZGVmYXVsdENvbm5lY3Rpb25TdHJpbmdGb3JtOiBGb3JtR3JvdXA7XG5cbiAgZGVmYXVsdENvbm5lY3Rpb25TdHJpbmc6IHN0cmluZztcblxuICBpc01vZGFsVmlzaWJsZTogYm9vbGVhbjtcblxuICBzZWxlY3RlZE1vZGFsQ29udGVudCA9IHt9IGFzIFNlbGVjdGVkTW9kYWxDb250ZW50O1xuXG4gIHZpc2libGVGZWF0dXJlcyA9IGZhbHNlO1xuXG4gIHByb3ZpZGVyS2V5OiBzdHJpbmc7XG5cbiAgX3VzZVNoYXJlZERhdGFiYXNlOiBib29sZWFuO1xuXG4gIHBhZ2VRdWVyeTogQUJQLlBhZ2VRdWVyeVBhcmFtcyA9IHt9O1xuXG4gIGxvYWRpbmcgPSBmYWxzZTtcblxuICBtb2RhbEJ1c3kgPSBmYWxzZTtcblxuICBzb3J0T3JkZXIgPSAnJztcblxuICBzb3J0S2V5ID0gJyc7XG5cbiAgZ2V0IHVzZVNoYXJlZERhdGFiYXNlKCk6IGJvb2xlYW4ge1xuICAgIHJldHVybiB0aGlzLmRlZmF1bHRDb25uZWN0aW9uU3RyaW5nRm9ybS5nZXQoJ3VzZVNoYXJlZERhdGFiYXNlJykudmFsdWU7XG4gIH1cblxuICBnZXQgY29ubmVjdGlvblN0cmluZygpOiBzdHJpbmcge1xuICAgIHJldHVybiB0aGlzLmRlZmF1bHRDb25uZWN0aW9uU3RyaW5nRm9ybS5nZXQoJ2RlZmF1bHRDb25uZWN0aW9uU3RyaW5nJykudmFsdWU7XG4gIH1cblxuICBAVmlld0NoaWxkKCd0ZW5hbnRNb2RhbFRlbXBsYXRlJywgeyBzdGF0aWM6IGZhbHNlIH0pXG4gIHRlbmFudE1vZGFsVGVtcGxhdGU6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQFZpZXdDaGlsZCgnY29ubmVjdGlvblN0cmluZ01vZGFsVGVtcGxhdGUnLCB7IHN0YXRpYzogZmFsc2UgfSlcbiAgY29ubmVjdGlvblN0cmluZ01vZGFsVGVtcGxhdGU6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgY29uc3RydWN0b3IoXG4gICAgcHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlLFxuICAgIHByaXZhdGUgdGVuYW50U2VydmljZTogVGVuYW50TWFuYWdlbWVudFNlcnZpY2UsXG4gICAgcHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIsXG4gICAgcHJpdmF0ZSBzdG9yZTogU3RvcmUsXG4gICkge31cblxuICBuZ09uSW5pdCgpIHtcbiAgICB0aGlzLmdldCgpO1xuICB9XG5cbiAgb25TZWFyY2godmFsdWUpIHtcbiAgICB0aGlzLnBhZ2VRdWVyeS5maWx0ZXIgPSB2YWx1ZTtcbiAgICB0aGlzLmdldCgpO1xuICB9XG5cbiAgcHJpdmF0ZSBjcmVhdGVUZW5hbnRGb3JtKCkge1xuICAgIHRoaXMudGVuYW50Rm9ybSA9IHRoaXMuZmIuZ3JvdXAoe1xuICAgICAgbmFtZTogW3RoaXMuc2VsZWN0ZWQubmFtZSB8fCAnJywgW1ZhbGlkYXRvcnMucmVxdWlyZWQsIFZhbGlkYXRvcnMubWF4TGVuZ3RoKDI1NildXSxcbiAgICB9KTtcbiAgfVxuXG4gIHByaXZhdGUgY3JlYXRlRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdGb3JtKCkge1xuICAgIHRoaXMuZGVmYXVsdENvbm5lY3Rpb25TdHJpbmdGb3JtID0gdGhpcy5mYi5ncm91cCh7XG4gICAgICB1c2VTaGFyZWREYXRhYmFzZTogdGhpcy5fdXNlU2hhcmVkRGF0YWJhc2UsXG4gICAgICBkZWZhdWx0Q29ubmVjdGlvblN0cmluZzogW3RoaXMuZGVmYXVsdENvbm5lY3Rpb25TdHJpbmcgfHwgJyddLFxuICAgIH0pO1xuICB9XG5cbiAgb3Blbk1vZGFsKHRpdGxlOiBzdHJpbmcsIHRlbXBsYXRlOiBUZW1wbGF0ZVJlZjxhbnk+LCB0eXBlOiBzdHJpbmcpIHtcbiAgICB0aGlzLnNlbGVjdGVkTW9kYWxDb250ZW50ID0ge1xuICAgICAgdGl0bGUsXG4gICAgICB0ZW1wbGF0ZSxcbiAgICAgIHR5cGUsXG4gICAgfTtcblxuICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSB0cnVlO1xuICB9XG5cbiAgb25FZGl0Q29ubmVjdGlvblN0cmluZyhpZDogc3RyaW5nKSB7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBHZXRUZW5hbnRCeUlkKGlkKSlcbiAgICAgIC5waXBlKFxuICAgICAgICBwbHVjaygnVGVuYW50TWFuYWdlbWVudFN0YXRlJywgJ3NlbGVjdGVkSXRlbScpLFxuICAgICAgICBzd2l0Y2hNYXAoc2VsZWN0ZWQgPT4ge1xuICAgICAgICAgIHRoaXMuc2VsZWN0ZWQgPSBzZWxlY3RlZDtcbiAgICAgICAgICByZXR1cm4gdGhpcy50ZW5hbnRTZXJ2aWNlLmdldERlZmF1bHRDb25uZWN0aW9uU3RyaW5nKGlkKTtcbiAgICAgICAgfSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKGZldGNoZWRDb25uZWN0aW9uU3RyaW5nID0+IHtcbiAgICAgICAgdGhpcy5fdXNlU2hhcmVkRGF0YWJhc2UgPSBmZXRjaGVkQ29ubmVjdGlvblN0cmluZyA/IGZhbHNlIDogdHJ1ZTtcbiAgICAgICAgdGhpcy5kZWZhdWx0Q29ubmVjdGlvblN0cmluZyA9IGZldGNoZWRDb25uZWN0aW9uU3RyaW5nID8gZmV0Y2hlZENvbm5lY3Rpb25TdHJpbmcgOiAnJztcbiAgICAgICAgdGhpcy5jcmVhdGVEZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0oKTtcbiAgICAgICAgdGhpcy5vcGVuTW9kYWwoJ0FicFRlbmFudE1hbmFnZW1lbnQ6OkNvbm5lY3Rpb25TdHJpbmdzJywgdGhpcy5jb25uZWN0aW9uU3RyaW5nTW9kYWxUZW1wbGF0ZSwgJ3NhdmVDb25uU3RyJyk7XG4gICAgICB9KTtcbiAgfVxuXG4gIG9uQWRkVGVuYW50KCkge1xuICAgIHRoaXMuc2VsZWN0ZWQgPSB7fSBhcyBBQlAuQmFzaWNJdGVtO1xuICAgIHRoaXMuY3JlYXRlVGVuYW50Rm9ybSgpO1xuICAgIHRoaXMub3Blbk1vZGFsKCdBYnBUZW5hbnRNYW5hZ2VtZW50OjpOZXdUZW5hbnQnLCB0aGlzLnRlbmFudE1vZGFsVGVtcGxhdGUsICdzYXZlVGVuYW50Jyk7XG4gIH1cblxuICBvbkVkaXRUZW5hbnQoaWQ6IHN0cmluZykge1xuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChuZXcgR2V0VGVuYW50QnlJZChpZCkpXG4gICAgICAucGlwZShwbHVjaygnVGVuYW50TWFuYWdlbWVudFN0YXRlJywgJ3NlbGVjdGVkSXRlbScpKVxuICAgICAgLnN1YnNjcmliZShzZWxlY3RlZCA9PiB7XG4gICAgICAgIHRoaXMuc2VsZWN0ZWQgPSBzZWxlY3RlZDtcbiAgICAgICAgdGhpcy5jcmVhdGVUZW5hbnRGb3JtKCk7XG4gICAgICAgIHRoaXMub3Blbk1vZGFsKCdBYnBUZW5hbnRNYW5hZ2VtZW50OjpFZGl0JywgdGhpcy50ZW5hbnRNb2RhbFRlbXBsYXRlLCAnc2F2ZVRlbmFudCcpO1xuICAgICAgfSk7XG4gIH1cblxuICBzYXZlKCkge1xuICAgIGNvbnN0IHsgdHlwZSB9ID0gdGhpcy5zZWxlY3RlZE1vZGFsQ29udGVudDtcbiAgICBpZiAoIXR5cGUpIHJldHVybjtcbiAgICBpZiAodHlwZSA9PT0gJ3NhdmVUZW5hbnQnKSB0aGlzLnNhdmVUZW5hbnQoKTtcbiAgICBlbHNlIGlmICh0eXBlID09PSAnc2F2ZUNvbm5TdHInKSB0aGlzLnNhdmVDb25uZWN0aW9uU3RyaW5nKCk7XG4gIH1cblxuICBzYXZlQ29ubmVjdGlvblN0cmluZygpIHtcbiAgICB0aGlzLm1vZGFsQnVzeSA9IHRydWU7XG4gICAgaWYgKHRoaXMudXNlU2hhcmVkRGF0YWJhc2UgfHwgKCF0aGlzLnVzZVNoYXJlZERhdGFiYXNlICYmICF0aGlzLmNvbm5lY3Rpb25TdHJpbmcpKSB7XG4gICAgICB0aGlzLnRlbmFudFNlcnZpY2VcbiAgICAgICAgLmRlbGV0ZURlZmF1bHRDb25uZWN0aW9uU3RyaW5nKHRoaXMuc2VsZWN0ZWQuaWQpXG4gICAgICAgIC5waXBlKFxuICAgICAgICAgIHRha2UoMSksXG4gICAgICAgICAgZmluYWxpemUoKCkgPT4gKHRoaXMubW9kYWxCdXN5ID0gZmFsc2UpKSxcbiAgICAgICAgKVxuICAgICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gZmFsc2U7XG4gICAgICAgIH0pO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aGlzLnRlbmFudFNlcnZpY2VcbiAgICAgICAgLnVwZGF0ZURlZmF1bHRDb25uZWN0aW9uU3RyaW5nKHsgaWQ6IHRoaXMuc2VsZWN0ZWQuaWQsIGRlZmF1bHRDb25uZWN0aW9uU3RyaW5nOiB0aGlzLmNvbm5lY3Rpb25TdHJpbmcgfSlcbiAgICAgICAgLnBpcGUoXG4gICAgICAgICAgdGFrZSgxKSxcbiAgICAgICAgICBmaW5hbGl6ZSgoKSA9PiAodGhpcy5tb2RhbEJ1c3kgPSBmYWxzZSkpLFxuICAgICAgICApXG4gICAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSBmYWxzZTtcbiAgICAgICAgfSk7XG4gICAgfVxuICB9XG5cbiAgc2F2ZVRlbmFudCgpIHtcbiAgICBpZiAoIXRoaXMudGVuYW50Rm9ybS52YWxpZCkgcmV0dXJuO1xuICAgIHRoaXMubW9kYWxCdXN5ID0gdHJ1ZTtcblxuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChcbiAgICAgICAgdGhpcy5zZWxlY3RlZC5pZFxuICAgICAgICAgID8gbmV3IFVwZGF0ZVRlbmFudCh7IC4uLnRoaXMudGVuYW50Rm9ybS52YWx1ZSwgaWQ6IHRoaXMuc2VsZWN0ZWQuaWQgfSlcbiAgICAgICAgICA6IG5ldyBDcmVhdGVUZW5hbnQodGhpcy50ZW5hbnRGb3JtLnZhbHVlKSxcbiAgICAgIClcbiAgICAgIC5waXBlKGZpbmFsaXplKCgpID0+ICh0aGlzLm1vZGFsQnVzeSA9IGZhbHNlKSkpXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IGZhbHNlO1xuICAgICAgfSk7XG4gIH1cblxuICBkZWxldGUoaWQ6IHN0cmluZywgbmFtZTogc3RyaW5nKSB7XG4gICAgdGhpcy5jb25maXJtYXRpb25TZXJ2aWNlXG4gICAgICAud2FybignQWJwVGVuYW50TWFuYWdlbWVudDo6VGVuYW50RGVsZXRpb25Db25maXJtYXRpb25NZXNzYWdlJywgJ0FicFRlbmFudE1hbmFnZW1lbnQ6OkFyZVlvdVN1cmUnLCB7XG4gICAgICAgIG1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXM6IFtuYW1lXSxcbiAgICAgIH0pXG4gICAgICAuc3Vic2NyaWJlKChzdGF0dXM6IFRvYXN0ZXIuU3RhdHVzKSA9PiB7XG4gICAgICAgIGlmIChzdGF0dXMgPT09IFRvYXN0ZXIuU3RhdHVzLmNvbmZpcm0pIHtcbiAgICAgICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBEZWxldGVUZW5hbnQoaWQpKTtcbiAgICAgICAgfVxuICAgICAgfSk7XG4gIH1cblxuICBvblBhZ2VDaGFuZ2UoZGF0YSkge1xuICAgIHRoaXMucGFnZVF1ZXJ5LnNraXBDb3VudCA9IGRhdGEuZmlyc3Q7XG4gICAgdGhpcy5wYWdlUXVlcnkubWF4UmVzdWx0Q291bnQgPSBkYXRhLnJvd3M7XG5cbiAgICB0aGlzLmdldCgpO1xuICB9XG5cbiAgZ2V0KCkge1xuICAgIHRoaXMubG9hZGluZyA9IHRydWU7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBHZXRUZW5hbnRzKHRoaXMucGFnZVF1ZXJ5KSlcbiAgICAgIC5waXBlKGZpbmFsaXplKCgpID0+ICh0aGlzLmxvYWRpbmcgPSBmYWxzZSkpKVxuICAgICAgLnN1YnNjcmliZSgpO1xuICB9XG59XG4iXX0=
