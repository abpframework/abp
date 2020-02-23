import { ABP } from '@abp/ng.core';
import { ConfirmationService, Toaster } from '@abp/ng.theme.shared';
import { Component, OnInit, TemplateRef, ViewChild, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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

interface SelectedModalContent {
  type: 'saveConnStr' | 'saveTenant';
  title: string;
  template: TemplateRef<any>;
}

@Component({
  selector: 'abp-tenants',
  templateUrl: './tenants.component.html',
})
export class TenantsComponent implements OnInit {
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

  visibleFeatures = false;

  providerKey: string;

  _useSharedDatabase: boolean;

  pageQuery: ABP.PageQueryParams = { maxResultCount: 10 };

  loading = false;

  modalBusy = false;

  sortOrder = '';

  sortKey = '';

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

  get isDisabledSaveButton(): boolean {
    if (!this.selectedModalContent) return false;

    if (
      this.selectedModalContent.type === 'saveConnStr' &&
      this.defaultConnectionStringForm &&
      this.defaultConnectionStringForm.invalid
    ) {
      return true;
    } else if (
      this.selectedModalContent.type === 'saveTenant' &&
      this.tenantForm &&
      this.tenantForm.invalid
    ) {
      return true;
    } else {
      return false;
    }
  }

  onVisibleFeaturesChange = (value: boolean) => {
    this.visibleFeatures = value;
  };

  constructor(
    private confirmationService: ConfirmationService,
    private tenantService: TenantManagementService,
    private fb: FormBuilder,
    private store: Store,
  ) {}

  ngOnInit() {
    this.get();
  }

  onSearch(value: string) {
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

  openModal(title: string, template: TemplateRef<any>, type: 'saveConnStr' | 'saveTenant') {
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
        this.openModal(
          'AbpTenantManagement::ConnectionStrings',
          this.connectionStringModalTemplate,
          'saveConnStr',
        );
      });
  }

  addTenant() {
    this.selected = {} as ABP.BasicItem;
    this.createTenantForm();
    this.openModal('AbpTenantManagement::NewTenant', this.tenantModalTemplate, 'saveTenant');
  }

  editTenant(id: string) {
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
    if (this.modalBusy) return;

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
        .updateDefaultConnectionString({
          id: this.selected.id,
          defaultConnectionString: this.connectionString,
        })
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
    if (!this.tenantForm.valid || this.modalBusy) return;
    this.modalBusy = true;

    this.store
      .dispatch(
        this.selected.id
          ? new UpdateTenant({ ...this.selected, ...this.tenantForm.value, id: this.selected.id })
          : new CreateTenant(this.tenantForm.value),
      )
      .pipe(finalize(() => (this.modalBusy = false)))
      .subscribe(() => {
        this.isModalVisible = false;
        this.get();
      });
  }

  delete(id: string, name: string) {
    this.confirmationService
      .warn(
        'AbpTenantManagement::TenantDeletionConfirmationMessage',
        'AbpTenantManagement::AreYouSure',
        {
          messageLocalizationParams: [name],
        },
      )
      .subscribe((status: Toaster.Status) => {
        if (status === Toaster.Status.confirm) {
          this.store.dispatch(new DeleteTenant(id)).subscribe(() => this.get());
        }
      });
  }

  onPageChange(page: number) {
    this.pageQuery.skipCount = (page - 1) * this.pageQuery.maxResultCount;

    this.get();
  }

  get() {
    this.loading = true;
    this.store
      .dispatch(new GetTenants(this.pageQuery))
      .pipe(finalize(() => (this.loading = false)))
      .subscribe();
  }

  onSharedDatabaseChange(value: boolean) {
    if (!value) {
      setTimeout(() => {
        const defaultConnectionString = document.getElementById(
          'defaultConnectionString',
        ) as HTMLInputElement;
        if (defaultConnectionString) {
          defaultConnectionString.focus();
        }
      }, 0);
    }
  }
}
