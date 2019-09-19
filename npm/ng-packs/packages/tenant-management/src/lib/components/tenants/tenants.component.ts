import { ABP } from '@abp/ng.core';
import { ConfirmationService, Toaster } from '@abp/ng.theme.shared';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Select, Store } from '@ngxs/store';
import { Observable, Subject } from 'rxjs';
import { debounceTime, finalize, pluck, switchMap, take } from 'rxjs/operators';
import {
  CreateTenant,
  DeleteTenant,
  GetTenants,
  GetTenantById,
  UpdateTenant,
} from '../../actions/tenant-management.actions';
import { TenantManagementService } from '../../services/tenant-management.service';
import { TenantManagementState } from '../../states/tenant-management.state';

type SelectedModalContent = {
  type: string;
  title: string;
  template: TemplateRef<any>;
};

@Component({
  selector: 'abp-tenants',
  templateUrl: './tenants.component.html',
})
export class TenantsComponent {
  @Select(TenantManagementState.get)
  data$: Observable<ABP.BasicItem[]>;

  @Select(TenantManagementState.getTenantsTotalCount)
  totalCount$: Observable<number>;

  selected: ABP.BasicItem;

  tenantForm: FormGroup;

  defaultConnectionStringForm: FormGroup;

  defaultConnectionString: string;

  isModalVisible: boolean;

  selectedModalContent = {} as SelectedModalContent;

  visibleFeatures: boolean = false;

  providerKey: string;

  _useSharedDatabase: boolean;

  pageQuery: ABP.PageQueryParams = {
    sorting: 'name',
  };

  loading: boolean = false;

  modalBusy: boolean = false;

  sortOrder: string = 'asc';

  get useSharedDatabase(): boolean {
    return this.defaultConnectionStringForm.get('useSharedDatabase').value;
  }

  get connectionString(): string {
    return this.defaultConnectionStringForm.get('defaultConnectionString').value;
  }

  @ViewChild('tenantModalTemplate', { static: false })
  tenantModalTemplate: TemplateRef<any>;

  @ViewChild('connectionStringModalTemplate', { static: false })
  connectionStringModalTemplate: TemplateRef<any>;

  constructor(
    private confirmationService: ConfirmationService,
    private tenantService: TenantManagementService,
    private fb: FormBuilder,
    private store: Store,
  ) {}

  onSearch(value) {
    this.pageQuery.filter = value;
    this.get();
  }

  private createTenantForm() {
    this.tenantForm = this.fb.group({
      name: [this.selected.name || '', [Validators.required, Validators.maxLength(256)]],
    });
  }

  private createDefaultConnectionStringForm() {
    this.defaultConnectionStringForm = this.fb.group({
      useSharedDatabase: this._useSharedDatabase,
      defaultConnectionString: [this.defaultConnectionString || ''],
    });
  }

  openModal(title: string, template: TemplateRef<any>, type: string) {
    this.selectedModalContent = {
      title,
      template,
      type,
    };

    this.isModalVisible = true;
  }

  onEditConnectionString(id: string) {
    this.store
      .dispatch(new GetTenantById(id))
      .pipe(
        pluck('TenantManagementState', 'selectedItem'),
        switchMap(selected => {
          this.selected = selected;
          return this.tenantService.getDefaultConnectionString(id);
        }),
      )
      .subscribe(fetchedConnectionString => {
        this._useSharedDatabase = fetchedConnectionString ? false : true;
        this.defaultConnectionString = fetchedConnectionString ? fetchedConnectionString : '';
        this.createDefaultConnectionStringForm();
        this.openModal('AbpTenantManagement::ConnectionStrings', this.connectionStringModalTemplate, 'saveConnStr');
      });
  }

  onAddTenant() {
    this.selected = {} as ABP.BasicItem;
    this.createTenantForm();
    this.openModal('AbpTenantManagement::NewTenant', this.tenantModalTemplate, 'saveTenant');
  }

  onEditTenant(id: string) {
    this.store
      .dispatch(new GetTenantById(id))
      .pipe(pluck('TenantManagementState', 'selectedItem'))
      .subscribe(selected => {
        this.selected = selected;
        this.createTenantForm();
        this.openModal('AbpTenantManagement::Edit', this.tenantModalTemplate, 'saveTenant');
      });
  }

  save() {
    const { type } = this.selectedModalContent;
    if (!type) return;
    if (type === 'saveTenant') this.saveTenant();
    else if (type === 'saveConnStr') this.saveConnectionString();
  }

  saveConnectionString() {
    this.modalBusy = true;
    if (this.useSharedDatabase || (!this.useSharedDatabase && !this.connectionString)) {
      this.tenantService
        .deleteDefaultConnectionString(this.selected.id)
        .pipe(
          take(1),
          finalize(() => (this.modalBusy = false)),
        )
        .subscribe(() => {
          this.isModalVisible = false;
        });
    } else {
      this.tenantService
        .updateDefaultConnectionString({ id: this.selected.id, defaultConnectionString: this.connectionString })
        .pipe(
          take(1),
          finalize(() => (this.modalBusy = false)),
        )
        .subscribe(() => {
          this.isModalVisible = false;
        });
    }
  }

  saveTenant() {
    if (!this.tenantForm.valid) return;
    this.modalBusy = true;

    this.store
      .dispatch(
        this.selected.id
          ? new UpdateTenant({ ...this.tenantForm.value, id: this.selected.id })
          : new CreateTenant(this.tenantForm.value),
      )
      .pipe(finalize(()=> (this.modalBusy = false)))
      .subscribe(() => {
        this.isModalVisible = false;
      });
  }

  delete(id: string, name: string) {
    this.confirmationService
      .warn('AbpTenantManagement::TenantDeletionConfirmationMessage', 'AbpTenantManagement::AreYouSure', {
        messageLocalizationParams: [name],
      })
      .subscribe((status: Toaster.Status) => {
        if (status === Toaster.Status.confirm) {
          this.store.dispatch(new DeleteTenant(id));
        }
      });
  }

  onPageChange(data) {
    this.pageQuery.skipCount = data.first;
    this.pageQuery.maxResultCount = data.rows;

    this.get();
  }

  get() {
    this.loading = true;
    this.store
      .dispatch(new GetTenants(this.pageQuery))
      .pipe(finalize(() => (this.loading = false)))
      .subscribe();
  }

  changeSortOrder() {
    this.sortOrder = this.sortOrder.toLowerCase() === "asc" ? "desc" : "asc";
  }
}
