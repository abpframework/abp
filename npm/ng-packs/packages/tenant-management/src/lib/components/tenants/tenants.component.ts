import { ListService, PagedResultDto } from '@abp/ng.core';
import { eFeatureManagementComponents } from '@abp/ng.feature-management';
import { Confirmation, ConfirmationService, getPasswordValidators } from '@abp/ng.theme.shared';
import { Component, Injector, OnInit, TemplateRef, ViewChild } from '@angular/core';
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
import { GetTenantsInput, TenantDto } from '../../proxy/models';
import { TenantService } from '../../proxy/tenant.service';
import { TenantManagementState } from '../../states/tenant-management.state';
import {
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.theme.shared/extensions';
import { eTenantManagementComponents } from '../../enums/components';

interface SelectedModalContent {
  type: 'saveConnStr' | 'saveTenant';
  title: string;
  template: TemplateRef<any>;
}

@Component({
  selector: 'abp-tenants',
  templateUrl: './tenants.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eTenantManagementComponents.Tenants,
    },
  ],
})
export class TenantsComponent implements OnInit {
  @Select(TenantManagementState.get)
  data$: Observable<PagedResultDto<TenantDto>>;

  @Select(TenantManagementState.getTenantsTotalCount)
  totalCount$: Observable<number>;

  selected: TenantDto;

  tenantForm: FormGroup;

  defaultConnectionStringForm: FormGroup;

  defaultConnectionString: string;

  isModalVisible: boolean;

  selectedModalContent = {} as SelectedModalContent;

  visibleFeatures = false;

  providerKey: string;

  _useSharedDatabase: boolean;

  modalBusy = false;

  featureManagementKey = eFeatureManagementComponents.FeatureManagement;

  get hasSelectedTenant(): boolean {
    return Boolean(this.selected.id);
  }

  get useSharedDatabase(): boolean {
    return this.defaultConnectionStringForm.get('useSharedDatabase').value;
  }

  get connectionString(): string {
    return this.defaultConnectionStringForm.get('defaultConnectionString').value;
  }

  @ViewChild('tenantModalTemplate')
  tenantModalTemplate: TemplateRef<any>;

  @ViewChild('connectionStringModalTemplate')
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
    public readonly list: ListService<GetTenantsInput>,
    private injector: Injector,
    private confirmationService: ConfirmationService,
    private tenantService: TenantService,
    private fb: FormBuilder,
    private store: Store,
  ) {}

  ngOnInit() {
    this.hookToQuery();
  }

  private createTenantForm() {
    const data = new FormPropData(this.injector, this.selected);
    this.tenantForm = generateFormFromProps(data);
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
    this.selected = {} as TenantDto;
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
        .updateDefaultConnectionString(this.selected.id, this.connectionString)
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
        this.list.get();
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
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.store.dispatch(new DeleteTenant(id)).subscribe(() => this.list.get());
        }
      });
  }

  hookToQuery() {
    this.list.hookToQuery(query => this.store.dispatch(new GetTenants(query))).subscribe();
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

  openFeaturesModal(providerKey: string) {
    this.providerKey = providerKey;
    setTimeout(() => {
      this.visibleFeatures = true;
    }, 0);
  }

  sort(data) {
    const { prop, dir } = data.sorts[0];
    this.list.sortKey = prop;
    this.list.sortOrder = dir;
  }
}
