import { ABP } from '@abp/ng.core';
import { ConfirmationService, Toaster } from '@abp/ng.theme.shared';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { pluck, switchMap, take } from 'rxjs/operators';
import {
  TenantManagementAdd,
  TenantManagementDelete,
  TenantManagementGetById,
  TenantManagementUpdate,
} from '../../actions/tenant-management.actions';
import { TenantManagementService } from '../../services';
import { TenantManagementState } from '../../states/tenant-management.state';

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

  useSharedDatabase: boolean;

  selectedModalContent: {
    title: string;
    template: TemplateRef<any>;
    onSave: () => void;
  };

  get showInput(): boolean {
    return !this.defaultConnectionStringForm.get('useSharedDatabase').value;
  }

  get connectionString(): string {
    return this.defaultConnectionStringForm.get('defaultConnectionString').value;
  }

  @ViewChild('modalWrapper', { static: false })
  modalWrapper: TemplateRef<any>;

  @ViewChild('mTemplateConnStr', { static: false })
  mTemplateConnStr: TemplateRef<any>;

  @ViewChild('mTemplateFeatures', { static: false })
  mTemplateFeatures: TemplateRef<any>;

  @ViewChild('mTemplateTenant', { static: false })
  mTemplateTenant: TemplateRef<any>;

  constructor(
    private confirmationService: ConfirmationService,
    private tenantService: TenantManagementService,
    private modalService: NgbModal,
    private fb: FormBuilder,
    private store: Store,
  ) {}

  openModal() {
    this.modalService.open(this.modalWrapper);
  }

  private createTenantForm() {
    this.tenantForm = this.fb.group({
      name: [this.selected.name || '', [Validators.required, Validators.maxLength(256)]],
    });
  }

  private createDefaultConnectionStringForm() {
    this.defaultConnectionStringForm = this.fb.group({
      useSharedDatabase: this.useSharedDatabase,
      defaultConnectionString: this.defaultConnectionString || '',
    });
  }

  onEditConnStr(id: string) {
    this.selectedModalContent = {
      title: this.selected && this.selected.id ? 'AbpTenantManagement::Edit' : 'AbpTenantManagement::NewTenant',
      template: this.mTemplateConnStr,
      onSave: () => this.saveConnStr,
    };
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
        this.useSharedDatabase = fetchedConnectionString ? false : true;
        this.defaultConnectionString = fetchedConnectionString ? fetchedConnectionString : '';
        this.createDefaultConnectionStringForm();
        this.openModal();
      });
  }

  saveConnStr() {
    this.tenantService
      .updateDefaultConnectionString({ id: this.selected.id, defaultConnectionString: this.connectionString })
      .pipe(take(1))
      .subscribe(() => this.modalService.dismissAll());
  }

  onManageFeatures(id: string) {
    this.selectedModalContent = {
      title: this.selected && this.selected.id ? 'AbpTenantManagement::Edit' : 'AbpTenantManagement::NewTenant',
      template: this.mTemplateFeatures,
      onSave: () => {},
    };
    this.openModal();
  }

  onAdd() {
    this.selected = {} as ABP.BasicItem;
    this.createTenantForm();
    this.openModal();
    this.selectedModalContent = {
      title: 'AbpTenantManagement::NewTenant',
      template: this.mTemplateTenant,
      onSave: () => this.saveTenant,
    };
  }

  onEdit(id: string) {
    this.store
      .dispatch(new TenantManagementGetById(id))
      .pipe(pluck('TenantManagementState', 'selectedItem'))
      .subscribe(selected => {
        this.selected = selected;
        this.selectedModalContent = {
          title: 'AbpTenantManagement::Edit',
          template: this.mTemplateTenant,
          onSave: () => this.saveTenant,
        };
        this.createTenantForm();
        this.openModal();
      });
  }

  saveTenant() {
    if (!this.tenantForm.valid) return;

    this.store
      .dispatch(
        this.selected.id
          ? new TenantManagementUpdate({ ...this.tenantForm.value, id: this.selected.id })
          : new TenantManagementAdd(this.tenantForm.value),
      )
      .subscribe(() => this.modalService.dismissAll());
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
