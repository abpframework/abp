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
  GetTenants,
  GetTenantById,
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
    this.selectedModalContent = /** @type {?} */ ({});
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
      .pipe(
        pluck('TenantManagementState', 'selectedItem'),
        switchMap(
          /**
           * @param {?} selected
           * @return {?}
           */
          selected => {
            this.selected = selected;
            return this.tenantService.getDefaultConnectionString(id);
          },
        ),
      )
      .subscribe(
        /**
         * @param {?} fetchedConnectionString
         * @return {?}
         */
        fetchedConnectionString => {
          this._useSharedDatabase = fetchedConnectionString ? false : true;
          this.defaultConnectionString = fetchedConnectionString ? fetchedConnectionString : '';
          this.createDefaultConnectionStringForm();
          this.openModal('AbpTenantManagement::ConnectionStrings', this.connectionStringModalTemplate, 'saveConnStr');
        },
      );
  }
  /**
   * @return {?}
   */
  onAddTenant() {
    this.selected = /** @type {?} */ ({});
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
      .subscribe(
        /**
         * @param {?} selected
         * @return {?}
         */
        selected => {
          this.selected = selected;
          this.createTenantForm();
          this.openModal('AbpTenantManagement::Edit', this.tenantModalTemplate, 'saveTenant');
        },
      );
  }
  /**
   * @return {?}
   */
  save() {
    const { type } = this.selectedModalContent;
    if (!type) return;
    if (type === 'saveTenant') this.saveTenant();
    else if (type === 'saveConnStr') this.saveConnectionString();
  }
  /**
   * @return {?}
   */
  saveConnectionString() {
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
            () => (this.modalBusy = false),
          ),
        )
        .subscribe(
          /**
           * @return {?}
           */
          () => {
            this.isModalVisible = false;
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
            () => (this.modalBusy = false),
          ),
        )
        .subscribe(
          /**
           * @return {?}
           */
          () => {
            this.isModalVisible = false;
          },
        );
    }
  }
  /**
   * @return {?}
   */
  saveTenant() {
    if (!this.tenantForm.valid) return;
    this.modalBusy = true;
    this.store
      .dispatch(
        this.selected.id
          ? new UpdateTenant(Object.assign({}, this.tenantForm.value, { id: this.selected.id }))
          : new CreateTenant(this.tenantForm.value),
      )
      .pipe(
        finalize(
          /**
           * @return {?}
           */
          () => (this.modalBusy = false),
        ),
      )
      .subscribe(
        /**
         * @return {?}
         */
        () => {
          this.isModalVisible = false;
        },
      );
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
      .subscribe(
        /**
         * @param {?} status
         * @return {?}
         */
        status => {
          if (status === 'confirm' /* confirm */) {
            this.store.dispatch(new DeleteTenant(id));
          }
        },
      );
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
      .pipe(
        finalize(
          /**
           * @return {?}
           */
          () => (this.loading = false),
        ),
      )
      .subscribe();
  }
}
TenantsComponent.decorators = [
  {
    type: Component,
    args: [
      {
        selector: 'abp-tenants',
        template:
          '<div class="row entry-row">\n  <div class="col-auto">\n    <h1 class="content-header-title">{{ \'AbpTenantManagement::Tenants\' | abpLocalization }}</h1>\n  </div>\n  <div class="col">\n    <div class="text-lg-right pt-2" id="AbpContentToolbar">\n      <button\n        [abpPermission]="\'AbpTenantManagement.Tenants.Create\'"\n        id="create-tenants"\n        class="btn btn-primary"\n        type="button"\n        (click)="onAddTenant()"\n      >\n        <i class="fa fa-plus mr-1"></i>\n        <span>{{ \'AbpTenantManagement::NewTenant\' | abpLocalization }}</span>\n      </button>\n    </div>\n  </div>\n</div>\n\n<div id="wrapper" class="card">\n  <div class="card-body">\n    <div id="data-tables-table-filter" class="data-tables-filter">\n      <label\n        ><input\n          type="search"\n          class="form-control form-control-sm"\n          [placeholder]="\'AbpUi::PagerSearch\' | abpLocalization"\n          (input.debounce)="onSearch($event.target.value)"\n      /></label>\n    </div>\n    <p-table\n      *ngIf="[130, 200] as columnWidths"\n      [value]="data$ | async"\n      [abpTableSort]="{ key: sortKey, order: sortOrder }"\n      [lazy]="true"\n      [lazyLoadOnInit]="false"\n      [paginator]="true"\n      [rows]="10"\n      [totalRecords]="totalCount$ | async"\n      [loading]="loading"\n      [resizableColumns]="true"\n      [scrollable]="true"\n      (onLazyLoad)="onPageChange($event)"\n    >\n      <ng-template pTemplate="colgroup">\n        <colgroup>\n          <col *ngFor="let width of columnWidths" [ngStyle]="{ \'width.px\': width }" />\n        </colgroup>\n      </ng-template>\n      <ng-template pTemplate="emptymessage" let-columns>\n        <tr\n          abp-table-empty-message\n          [attr.colspan]="columnWidths.length"\n          localizationResource="AbpTenantManagement"\n          localizationProp="NoDataAvailableInDatatable"\n        ></tr>\n      </ng-template>\n      <ng-template pTemplate="header" let-columns>\n        <tr>\n          <th>{{ \'AbpTenantManagement::Actions\' | abpLocalization }}</th>\n          <th pResizableColumn (click)="sortOrderIcon.sort(\'name\')">\n            {{ \'AbpTenantManagement::TenantName\' | abpLocalization }}\n            <abp-sort-order-icon #sortOrderIcon key="name" [(selectedKey)]="sortKey" [(order)]="sortOrder">\n            </abp-sort-order-icon>\n          </th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate="body" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown container="body" class="d-inline-block">\n              <button\n                class="btn btn-primary btn-sm dropdown-toggle"\n                data-toggle="dropdown"\n                aria-haspopup="true"\n                ngbDropdownToggle\n              >\n                <i class="fa fa-cog mr-1"></i>{{ \'AbpTenantManagement::Actions\' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button\n                  [abpPermission]="\'AbpTenantManagement.Tenants.Update\'"\n                  ngbDropdownItem\n                  (click)="onEditTenant(data.id)"\n                >\n                  {{ \'AbpTenantManagement::Edit\' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]="\'AbpTenantManagement.Tenants.ManageConnectionStrings\'"\n                  ngbDropdownItem\n                  (click)="onEditConnectionString(data.id)"\n                >\n                  {{ \'AbpTenantManagement::Permission:ManageConnectionStrings\' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]="\'AbpTenantManagement.Tenants.ManageFeatures\'"\n                  ngbDropdownItem\n                  (click)="providerKey = data.id; visibleFeatures = true"\n                >\n                  {{ \'AbpTenantManagement::Permission:ManageFeatures\' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]="\'AbpTenantManagement.Tenants.Delete\'"\n                  ngbDropdownItem\n                  (click)="delete(data.id, data.name)"\n                >\n                  {{ \'AbpTenantManagement::Delete\' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal size="md" [(visible)]="isModalVisible" [busy]="modalBusy">\n  <ng-template #abpHeader>\n    <h3>{{ selectedModalContent.title | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <ng-container *ngTemplateOutlet="selectedModalContent?.template"></ng-container>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button #abpClose type="button" class="btn btn-secondary">\n      {{ \'AbpTenantManagement::Cancel\' | abpLocalization }}\n    </button>\n    <abp-button iconClass="fa fa-check" (click)="save()">{{ \'AbpIdentity::Save\' | abpLocalization }}</abp-button>\n  </ng-template>\n</abp-modal>\n\n<ng-template #tenantModalTemplate>\n  <form [formGroup]="tenantForm" (ngSubmit)="save()">\n    <div class="mt-2">\n      <div class="form-group">\n        <label for="name">{{ \'AbpTenantManagement::TenantName\' | abpLocalization }}</label>\n        <input type="text" id="name" class="form-control" formControlName="name" autofocus />\n      </div>\n    </div>\n  </form>\n</ng-template>\n\n<ng-template #connectionStringModalTemplate>\n  <form [formGroup]="defaultConnectionStringForm" (ngSubmit)="save()">\n    <label class="mt-2">\n      <div class="form-group">\n        <div class="custom-checkbox custom-control mb-2">\n          <input\n            id="useSharedDatabase"\n            type="checkbox"\n            class="custom-control-input"\n            formControlName="useSharedDatabase"\n            autofocus\n          />\n          <label for="useSharedDatabase" class="custom-control-label">{{\n            \'AbpTenantManagement::DisplayName:UseSharedDatabase\' | abpLocalization\n          }}</label>\n        </div>\n      </div>\n      <label class="form-group" *ngIf="!useSharedDatabase">\n        <label for="defaultConnectionString">{{\n          \'AbpTenantManagement::DisplayName:DefaultConnectionString\' | abpLocalization\n        }}</label>\n        <input\n          type="text"\n          id="defaultConnectionString"\n          class="form-control"\n          formControlName="defaultConnectionString"\n        />\n      </label>\n    </label>\n  </form>\n</ng-template>\n\n<abp-feature-management [(visible)]="visibleFeatures" providerName="Tenant" [providerKey]="providerKey">\n</abp-feature-management>\n',
      },
    ],
  },
];
/** @nocollapse */
TenantsComponent.ctorParameters = () => [
  { type: ConfirmationService },
  { type: TenantManagementService },
  { type: FormBuilder },
  { type: Store },
];
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50cy5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvdGVuYW50cy90ZW5hbnRzLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUNBLE9BQU8sRUFBRSxtQkFBbUIsRUFBVyxNQUFNLHNCQUFzQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxTQUFTLEVBQUUsV0FBVyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUNsRSxPQUFPLEVBQUUsV0FBVyxFQUFhLFVBQVUsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxVQUFVLEVBQVcsTUFBTSxNQUFNLENBQUM7QUFDM0MsT0FBTyxFQUFnQixRQUFRLEVBQUUsS0FBSyxFQUFFLFNBQVMsRUFBRSxJQUFJLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNoRixPQUFPLEVBQ0wsWUFBWSxFQUNaLFlBQVksRUFDWixVQUFVLEVBQ1YsYUFBYSxFQUNiLFlBQVksR0FDYixNQUFNLHlDQUF5QyxDQUFDO0FBQ2pELE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxNQUFNLDBDQUEwQyxDQUFDO0FBQ25GLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLHNDQUFzQyxDQUFDOzs7O0FBRTdFLG1DQUlDOzs7SUFIQyxvQ0FBYTs7SUFDYixxQ0FBYzs7SUFDZCx3Q0FBMkI7O0FBTzdCLE1BQU0sT0FBTyxnQkFBZ0I7Ozs7Ozs7SUFpRDNCLFlBQ1UsbUJBQXdDLEVBQ3hDLGFBQXNDLEVBQ3RDLEVBQWUsRUFDZixLQUFZO1FBSFosd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQUN4QyxrQkFBYSxHQUFiLGFBQWEsQ0FBeUI7UUFDdEMsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUNmLFVB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LFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsYUFBYTtnQkFDdkIseXZOQUF1QzthQUN4Qzs7OztZQXpCUSxtQkFBbUI7WUFhbkIsdUJBQXVCO1lBWHZCLFdBQVc7WUFDSCxLQUFLOzs7a0NBa0VuQixTQUFTLFNBQUMscUJBQXFCLEVBQUUsRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFOzRDQUdsRCxTQUFTLFNBQUMsK0JBQStCLEVBQUUsRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFOztBQTVDN0Q7SUFEQyxNQUFNLENBQUMscUJBQXFCLENBQUMsR0FBRyxDQUFDO3NDQUMzQixVQUFVOytDQUFrQjtBQUduQztJQURDLE1BQU0sQ0FBQyxxQkFBcUIsQ0FBQyxvQkFBb0IsQ0FBQztzQ0FDdEMsVUFBVTtxREFBUzs7O0lBSmhDLGlDQUNtQzs7SUFFbkMsdUNBQ2dDOztJQUVoQyxvQ0FBd0I7O0lBRXhCLHNDQUFzQjs7SUFFdEIsdURBQXVDOztJQUV2QyxtREFBZ0M7O0lBRWhDLDBDQUF3Qjs7SUFFeEIsZ0RBQWtEOztJQUVsRCwyQ0FBd0I7O0lBRXhCLHVDQUFvQjs7SUFFcEIsOENBQTRCOztJQUU1QixxQ0FBb0M7O0lBRXBDLG1DQUFnQjs7SUFFaEIscUNBQWtCOztJQUVsQixxQ0FBZTs7SUFFZixtQ0FBYTs7SUFVYiwrQ0FDc0M7O0lBRXRDLHlEQUNnRDs7Ozs7SUFHOUMsK0NBQWdEOzs7OztJQUNoRCx5Q0FBOEM7Ozs7O0lBQzlDLDhCQUF1Qjs7Ozs7SUFDdkIsaUNBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IENvbmZpcm1hdGlvblNlcnZpY2UsIFRvYXN0ZXIgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBDb21wb25lbnQsIFRlbXBsYXRlUmVmLCBWaWV3Q2hpbGQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEZvcm1CdWlsZGVyLCBGb3JtR3JvdXAsIFZhbGlkYXRvcnMgfSBmcm9tICdAYW5ndWxhci9mb3Jtcyc7XG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSwgU3ViamVjdCB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgZGVib3VuY2VUaW1lLCBmaW5hbGl6ZSwgcGx1Y2ssIHN3aXRjaE1hcCwgdGFrZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7XG4gIENyZWF0ZVRlbmFudCxcbiAgRGVsZXRlVGVuYW50LFxuICBHZXRUZW5hbnRzLFxuICBHZXRUZW5hbnRCeUlkLFxuICBVcGRhdGVUZW5hbnQsXG59IGZyb20gJy4uLy4uL2FjdGlvbnMvdGVuYW50LW1hbmFnZW1lbnQuYWN0aW9ucyc7XG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50U2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzL3RlbmFudC1tYW5hZ2VtZW50LnNlcnZpY2UnO1xuaW1wb3J0IHsgVGVuYW50TWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi4vLi4vc3RhdGVzL3RlbmFudC1tYW5hZ2VtZW50LnN0YXRlJztcblxuaW50ZXJmYWNlIFNlbGVjdGVkTW9kYWxDb250ZW50IHtcbiAgdHlwZTogc3RyaW5nO1xuICB0aXRsZTogc3RyaW5nO1xuICB0ZW1wbGF0ZTogVGVtcGxhdGVSZWY8YW55Pjtcbn1cblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXRlbmFudHMnLFxuICB0ZW1wbGF0ZVVybDogJy4vdGVuYW50cy5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFRlbmFudHNDb21wb25lbnQge1xuICBAU2VsZWN0KFRlbmFudE1hbmFnZW1lbnRTdGF0ZS5nZXQpXG4gIGRhdGEkOiBPYnNlcnZhYmxlPEFCUC5CYXNpY0l0ZW1bXT47XG5cbiAgQFNlbGVjdChUZW5hbnRNYW5hZ2VtZW50U3RhdGUuZ2V0VGVuYW50c1RvdGFsQ291bnQpXG4gIHRvdGFsQ291bnQkOiBPYnNlcnZhYmxlPG51bWJlcj47XG5cbiAgc2VsZWN0ZWQ6IEFCUC5CYXNpY0l0ZW07XG5cbiAgdGVuYW50Rm9ybTogRm9ybUdyb3VwO1xuXG4gIGRlZmF1bHRDb25uZWN0aW9uU3RyaW5nRm9ybTogRm9ybUdyb3VwO1xuXG4gIGRlZmF1bHRDb25uZWN0aW9uU3RyaW5nOiBzdHJpbmc7XG5cbiAgaXNNb2RhbFZpc2libGU6IGJvb2xlYW47XG5cbiAgc2VsZWN0ZWRNb2RhbENvbnRlbnQgPSB7fSBhcyBTZWxlY3RlZE1vZGFsQ29udGVudDtcblxuICB2aXNpYmxlRmVhdHVyZXMgPSBmYWxzZTtcblxuICBwcm92aWRlcktleTogc3RyaW5nO1xuXG4gIF91c2VTaGFyZWREYXRhYmFzZTogYm9vbGVhbjtcblxuICBwYWdlUXVlcnk6IEFCUC5QYWdlUXVlcnlQYXJhbXMgPSB7fTtcblxuICBsb2FkaW5nID0gZmFsc2U7XG5cbiAgbW9kYWxCdXN5ID0gZmFsc2U7XG5cbiAgc29ydE9yZGVyID0gJyc7XG5cbiAgc29ydEtleSA9ICcnO1xuXG4gIGdldCB1c2VTaGFyZWREYXRhYmFzZSgpOiBib29sZWFuIHtcbiAgICByZXR1cm4gdGhpcy5kZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0uZ2V0KCd1c2VTaGFyZWREYXRhYmFzZScpLnZhbHVlO1xuICB9XG5cbiAgZ2V0IGNvbm5lY3Rpb25TdHJpbmcoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy5kZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0uZ2V0KCdkZWZhdWx0Q29ubmVjdGlvblN0cmluZycpLnZhbHVlO1xuICB9XG5cbiAgQFZpZXdDaGlsZCgndGVuYW50TW9kYWxUZW1wbGF0ZScsIHsgc3RhdGljOiBmYWxzZSB9KVxuICB0ZW5hbnRNb2RhbFRlbXBsYXRlOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBWaWV3Q2hpbGQoJ2Nvbm5lY3Rpb25TdHJpbmdNb2RhbFRlbXBsYXRlJywgeyBzdGF0aWM6IGZhbHNlIH0pXG4gIGNvbm5lY3Rpb25TdHJpbmdNb2RhbFRlbXBsYXRlOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIGNvbnN0cnVjdG9yKFxuICAgIHByaXZhdGUgY29uZmlybWF0aW9uU2VydmljZTogQ29uZmlybWF0aW9uU2VydmljZSxcbiAgICBwcml2YXRlIHRlbmFudFNlcnZpY2U6IFRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlLFxuICAgIHByaXZhdGUgZmI6IEZvcm1CdWlsZGVyLFxuICAgIHByaXZhdGUgc3RvcmU6IFN0b3JlLFxuICApIHt9XG5cbiAgb25TZWFyY2godmFsdWUpIHtcbiAgICB0aGlzLnBhZ2VRdWVyeS5maWx0ZXIgPSB2YWx1ZTtcbiAgICB0aGlzLmdldCgpO1xuICB9XG5cbiAgcHJpdmF0ZSBjcmVhdGVUZW5hbnRGb3JtKCkge1xuICAgIHRoaXMudGVuYW50Rm9ybSA9IHRoaXMuZmIuZ3JvdXAoe1xuICAgICAgbmFtZTogW3RoaXMuc2VsZWN0ZWQubmFtZSB8fCAnJywgW1ZhbGlkYXRvcnMucmVxdWlyZWQsIFZhbGlkYXRvcnMubWF4TGVuZ3RoKDI1NildXSxcbiAgICB9KTtcbiAgfVxuXG4gIHByaXZhdGUgY3JlYXRlRGVmYXVsdENvbm5lY3Rpb25TdHJpbmdGb3JtKCkge1xuICAgIHRoaXMuZGVmYXVsdENvbm5lY3Rpb25TdHJpbmdGb3JtID0gdGhpcy5mYi5ncm91cCh7XG4gICAgICB1c2VTaGFyZWREYXRhYmFzZTogdGhpcy5fdXNlU2hhcmVkRGF0YWJhc2UsXG4gICAgICBkZWZhdWx0Q29ubmVjdGlvblN0cmluZzogW3RoaXMuZGVmYXVsdENvbm5lY3Rpb25TdHJpbmcgfHwgJyddLFxuICAgIH0pO1xuICB9XG5cbiAgb3Blbk1vZGFsKHRpdGxlOiBzdHJpbmcsIHRlbXBsYXRlOiBUZW1wbGF0ZVJlZjxhbnk+LCB0eXBlOiBzdHJpbmcpIHtcbiAgICB0aGlzLnNlbGVjdGVkTW9kYWxDb250ZW50ID0ge1xuICAgICAgdGl0bGUsXG4gICAgICB0ZW1wbGF0ZSxcbiAgICAgIHR5cGUsXG4gICAgfTtcblxuICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSB0cnVlO1xuICB9XG5cbiAgb25FZGl0Q29ubmVjdGlvblN0cmluZyhpZDogc3RyaW5nKSB7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBHZXRUZW5hbnRCeUlkKGlkKSlcbiAgICAgIC5waXBlKFxuICAgICAgICBwbHVjaygnVGVuYW50TWFuYWdlbWVudFN0YXRlJywgJ3NlbGVjdGVkSXRlbScpLFxuICAgICAgICBzd2l0Y2hNYXAoc2VsZWN0ZWQgPT4ge1xuICAgICAgICAgIHRoaXMuc2VsZWN0ZWQgPSBzZWxlY3RlZDtcbiAgICAgICAgICByZXR1cm4gdGhpcy50ZW5hbnRTZXJ2aWNlLmdldERlZmF1bHRDb25uZWN0aW9uU3RyaW5nKGlkKTtcbiAgICAgICAgfSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKGZldGNoZWRDb25uZWN0aW9uU3RyaW5nID0+IHtcbiAgICAgICAgdGhpcy5fdXNlU2hhcmVkRGF0YWJhc2UgPSBmZXRjaGVkQ29ubmVjdGlvblN0cmluZyA/IGZhbHNlIDogdHJ1ZTtcbiAgICAgICAgdGhpcy5kZWZhdWx0Q29ubmVjdGlvblN0cmluZyA9IGZldGNoZWRDb25uZWN0aW9uU3RyaW5nID8gZmV0Y2hlZENvbm5lY3Rpb25TdHJpbmcgOiAnJztcbiAgICAgICAgdGhpcy5jcmVhdGVEZWZhdWx0Q29ubmVjdGlvblN0cmluZ0Zvcm0oKTtcbiAgICAgICAgdGhpcy5vcGVuTW9kYWwoJ0FicFRlbmFudE1hbmFnZW1lbnQ6OkNvbm5lY3Rpb25TdHJpbmdzJywgdGhpcy5jb25uZWN0aW9uU3RyaW5nTW9kYWxUZW1wbGF0ZSwgJ3NhdmVDb25uU3RyJyk7XG4gICAgICB9KTtcbiAgfVxuXG4gIG9uQWRkVGVuYW50KCkge1xuICAgIHRoaXMuc2VsZWN0ZWQgPSB7fSBhcyBBQlAuQmFzaWNJdGVtO1xuICAgIHRoaXMuY3JlYXRlVGVuYW50Rm9ybSgpO1xuICAgIHRoaXMub3Blbk1vZGFsKCdBYnBUZW5hbnRNYW5hZ2VtZW50OjpOZXdUZW5hbnQnLCB0aGlzLnRlbmFudE1vZGFsVGVtcGxhdGUsICdzYXZlVGVuYW50Jyk7XG4gIH1cblxuICBvbkVkaXRUZW5hbnQoaWQ6IHN0cmluZykge1xuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChuZXcgR2V0VGVuYW50QnlJZChpZCkpXG4gICAgICAucGlwZShwbHVjaygnVGVuYW50TWFuYWdlbWVudFN0YXRlJywgJ3NlbGVjdGVkSXRlbScpKVxuICAgICAgLnN1YnNjcmliZShzZWxlY3RlZCA9PiB7XG4gICAgICAgIHRoaXMuc2VsZWN0ZWQgPSBzZWxlY3RlZDtcbiAgICAgICAgdGhpcy5jcmVhdGVUZW5hbnRGb3JtKCk7XG4gICAgICAgIHRoaXMub3Blbk1vZGFsKCdBYnBUZW5hbnRNYW5hZ2VtZW50OjpFZGl0JywgdGhpcy50ZW5hbnRNb2RhbFRlbXBsYXRlLCAnc2F2ZVRlbmFudCcpO1xuICAgICAgfSk7XG4gIH1cblxuICBzYXZlKCkge1xuICAgIGNvbnN0IHsgdHlwZSB9ID0gdGhpcy5zZWxlY3RlZE1vZGFsQ29udGVudDtcbiAgICBpZiAoIXR5cGUpIHJldHVybjtcbiAgICBpZiAodHlwZSA9PT0gJ3NhdmVUZW5hbnQnKSB0aGlzLnNhdmVUZW5hbnQoKTtcbiAgICBlbHNlIGlmICh0eXBlID09PSAnc2F2ZUNvbm5TdHInKSB0aGlzLnNhdmVDb25uZWN0aW9uU3RyaW5nKCk7XG4gIH1cblxuICBzYXZlQ29ubmVjdGlvblN0cmluZygpIHtcbiAgICB0aGlzLm1vZGFsQnVzeSA9IHRydWU7XG4gICAgaWYgKHRoaXMudXNlU2hhcmVkRGF0YWJhc2UgfHwgKCF0aGlzLnVzZVNoYXJlZERhdGFiYXNlICYmICF0aGlzLmNvbm5lY3Rpb25TdHJpbmcpKSB7XG4gICAgICB0aGlzLnRlbmFudFNlcnZpY2VcbiAgICAgICAgLmRlbGV0ZURlZmF1bHRDb25uZWN0aW9uU3RyaW5nKHRoaXMuc2VsZWN0ZWQuaWQpXG4gICAgICAgIC5waXBlKFxuICAgICAgICAgIHRha2UoMSksXG4gICAgICAgICAgZmluYWxpemUoKCkgPT4gKHRoaXMubW9kYWxCdXN5ID0gZmFsc2UpKSxcbiAgICAgICAgKVxuICAgICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gZmFsc2U7XG4gICAgICAgIH0pO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aGlzLnRlbmFudFNlcnZpY2VcbiAgICAgICAgLnVwZGF0ZURlZmF1bHRDb25uZWN0aW9uU3RyaW5nKHsgaWQ6IHRoaXMuc2VsZWN0ZWQuaWQsIGRlZmF1bHRDb25uZWN0aW9uU3RyaW5nOiB0aGlzLmNvbm5lY3Rpb25TdHJpbmcgfSlcbiAgICAgICAgLnBpcGUoXG4gICAgICAgICAgdGFrZSgxKSxcbiAgICAgICAgICBmaW5hbGl6ZSgoKSA9PiAodGhpcy5tb2RhbEJ1c3kgPSBmYWxzZSkpLFxuICAgICAgICApXG4gICAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSBmYWxzZTtcbiAgICAgICAgfSk7XG4gICAgfVxuICB9XG5cbiAgc2F2ZVRlbmFudCgpIHtcbiAgICBpZiAoIXRoaXMudGVuYW50Rm9ybS52YWxpZCkgcmV0dXJuO1xuICAgIHRoaXMubW9kYWxCdXN5ID0gdHJ1ZTtcblxuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChcbiAgICAgICAgdGhpcy5zZWxlY3RlZC5pZFxuICAgICAgICAgID8gbmV3IFVwZGF0ZVRlbmFudCh7IC4uLnRoaXMudGVuYW50Rm9ybS52YWx1ZSwgaWQ6IHRoaXMuc2VsZWN0ZWQuaWQgfSlcbiAgICAgICAgICA6IG5ldyBDcmVhdGVUZW5hbnQodGhpcy50ZW5hbnRGb3JtLnZhbHVlKSxcbiAgICAgIClcbiAgICAgIC5waXBlKGZpbmFsaXplKCgpID0+ICh0aGlzLm1vZGFsQnVzeSA9IGZhbHNlKSkpXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IGZhbHNlO1xuICAgICAgfSk7XG4gIH1cblxuICBkZWxldGUoaWQ6IHN0cmluZywgbmFtZTogc3RyaW5nKSB7XG4gICAgdGhpcy5jb25maXJtYXRpb25TZXJ2aWNlXG4gICAgICAud2FybignQWJwVGVuYW50TWFuYWdlbWVudDo6VGVuYW50RGVsZXRpb25Db25maXJtYXRpb25NZXNzYWdlJywgJ0FicFRlbmFudE1hbmFnZW1lbnQ6OkFyZVlvdVN1cmUnLCB7XG4gICAgICAgIG1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXM6IFtuYW1lXSxcbiAgICAgIH0pXG4gICAgICAuc3Vic2NyaWJlKChzdGF0dXM6IFRvYXN0ZXIuU3RhdHVzKSA9PiB7XG4gICAgICAgIGlmIChzdGF0dXMgPT09IFRvYXN0ZXIuU3RhdHVzLmNvbmZpcm0pIHtcbiAgICAgICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBEZWxldGVUZW5hbnQoaWQpKTtcbiAgICAgICAgfVxuICAgICAgfSk7XG4gIH1cblxuICBvblBhZ2VDaGFuZ2UoZGF0YSkge1xuICAgIHRoaXMucGFnZVF1ZXJ5LnNraXBDb3VudCA9IGRhdGEuZmlyc3Q7XG4gICAgdGhpcy5wYWdlUXVlcnkubWF4UmVzdWx0Q291bnQgPSBkYXRhLnJvd3M7XG5cbiAgICB0aGlzLmdldCgpO1xuICB9XG5cbiAgZ2V0KCkge1xuICAgIHRoaXMubG9hZGluZyA9IHRydWU7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBHZXRUZW5hbnRzKHRoaXMucGFnZVF1ZXJ5KSlcbiAgICAgIC5waXBlKGZpbmFsaXplKCgpID0+ICh0aGlzLmxvYWRpbmcgPSBmYWxzZSkpKVxuICAgICAgLnN1YnNjcmliZSgpO1xuICB9XG59XG4iXX0=
