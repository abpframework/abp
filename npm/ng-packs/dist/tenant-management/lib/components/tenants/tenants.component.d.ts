import { ABP } from '@abp/ng.core';
import { ConfirmationService } from '@abp/ng.theme.shared';
import { TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { TenantManagementService } from '../../services/tenant-management.service';
interface SelectedModalContent {
  type: string;
  title: string;
  template: TemplateRef<any>;
}
export declare class TenantsComponent {
  constructor(
    confirmationService: ConfirmationService,
    tenantService: TenantManagementService,
    fb: FormBuilder,
    store: Store,
  );
  private confirmationService;
  private tenantService;
  private fb;
  private store;
  data$: Observable<ABP.BasicItem[]>;
  totalCount$: Observable<number>;
  selected: ABP.BasicItem;
  tenantForm: FormGroup;
  defaultConnectionStringForm: FormGroup;
  defaultConnectionString: string;
  isModalVisible: boolean;
  selectedModalContent: SelectedModalContent;
  visibleFeatures: boolean;
  providerKey: string;
  _useSharedDatabase: boolean;
  pageQuery: ABP.PageQueryParams;
  loading: boolean;
  modalBusy: boolean;
  sortOrder: string;
  sortKey: string;
  readonly useSharedDatabase: boolean;
  readonly connectionString: string;
  tenantModalTemplate: TemplateRef<any>;
  connectionStringModalTemplate: TemplateRef<any>;
  private createTenantForm;
  private createDefaultConnectionStringForm;
  onSearch(value: any): void;
  openModal(title: string, template: TemplateRef<any>, type: string): void;
  onEditConnectionString(id: string): void;
  onAddTenant(): void;
  onEditTenant(id: string): void;
  save(): void;
  saveConnectionString(): void;
  saveTenant(): void;
  delete(id: string, name: string): void;
  onPageChange(data: any): void;
  get(): void;
}
export {};
