import { ABP } from '@abp/ng.core';
import { ConfirmationService, Toaster } from '@abp/ng.theme.shared';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { pluck, switchMap, take } from 'rxjs/operators';
import {
  TenantManagementAdd,
  TenantManagementDelete,
  TenantManagementGetById,
  TenantManagementUpdate,
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
  datas$: Observable<ABP.BasicItem[]>;

  selected: ABP.BasicItem;

  tenantForm: FormGroup;

  defaultConnectionStringForm: FormGroup;

  defaultConnectionString: string;

  isModalVisible: boolean;

  selectedModalContent = {} as SelectedModalContent;

  _useSharedDatabase: boolean;

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

  @ViewChild('featuresModalTemplate', { static: false })
  featuresModalTemplate: TemplateRef<any>;

  constructor(
    private confirmationService: ConfirmationService,
    private tenantService: TenantManagementService,
    private fb: FormBuilder,
    private store: Store,
  ) {}

  private createTenantForm() {
    this.tenantForm = this.fb.group({
      name: [this.selected.name || '', [Validators.required, Validators.maxLength(256)]],
    });
  }

  private createDefaultConnectionStringForm() {
    this.defaultConnectionStringForm = this.fb.group({
      useSharedDatabase: this._useSharedDatabase,
      defaultConnectionString: this.defaultConnectionString || '',
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
      .dispatch(new TenantManagementGetById(id))
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

  onManageFeatures(id: string) {
    this.openModal('AbpTenantManagement::Features', this.featuresModalTemplate, 'saveFeatures');
  }

  onAddTenant() {
    this.selected = {} as ABP.BasicItem;
    this.createTenantForm();
    this.openModal('AbpTenantManagement::NewTenant', this.tenantModalTemplate, 'saveTenant');
  }

  onEditTenant(id: string) {
    this.store
      .dispatch(new TenantManagementGetById(id))
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
    if (this.useSharedDatabase) {
      this.tenantService
        .deleteDefaultConnectionString(this.selected.id)
        .pipe(take(1))
        .subscribe(() => {
          this.isModalVisible = false;
        });
    } else {
      this.tenantService
        .updateDefaultConnectionString({ id: this.selected.id, defaultConnectionString: this.connectionString })
        .pipe(take(1))
        .subscribe(() => {
          this.isModalVisible = false;
        });
    }
  }

  saveTenant() {
    if (!this.tenantForm.valid) return;

    this.store
      .dispatch(
        this.selected.id
          ? new TenantManagementUpdate({ ...this.tenantForm.value, id: this.selected.id })
          : new TenantManagementAdd(this.tenantForm.value),
      )
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
          this.store.dispatch(new TenantManagementDelete(id));
        }
      });
  }
}
