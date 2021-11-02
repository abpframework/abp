import { ListService, PagedResultDto } from '@abp/ng.core';
import { eFeatureManagementComponents } from '@abp/ng.feature-management';
import { GetTenantsInput, TenantDto, TenantService } from '@abp/ng.tenant-management/proxy';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import {
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.theme.shared/extensions';
import { Component, Injector, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { eTenantManagementComponents } from '../../enums/components';

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
  data: PagedResultDto<TenantDto> = { items: [], totalCount: 0 };

  selected!: TenantDto;

  tenantForm!: FormGroup;

  isModalVisible!: boolean;

  visibleFeatures = false;

  providerKey: string;

  modalBusy = false;

  featureManagementKey = eFeatureManagementComponents.FeatureManagement;

  get hasSelectedTenant(): boolean {
    return Boolean(this.selected.id);
  }

  onVisibleFeaturesChange = (value: boolean) => {
    this.visibleFeatures = value;
  };

  constructor(
    public readonly list: ListService<GetTenantsInput>,
    private injector: Injector,
    private confirmationService: ConfirmationService,
    private service: TenantService,
    private fb: FormBuilder,
  ) {}

  ngOnInit() {
    this.hookToQuery();
  }

  private createTenantForm() {
    const data = new FormPropData(this.injector, this.selected);
    this.tenantForm = generateFormFromProps(data);
  }

  addTenant() {
    this.selected = {} as TenantDto;
    this.createTenantForm();
    this.isModalVisible = true;
  }

  editTenant(id: string) {
    this.service.get(id).subscribe(res => {
      this.selected = res;
      this.createTenantForm();
      this.isModalVisible = true;
    });
  }

  save() {
    if (!this.tenantForm.valid || this.modalBusy) return;
    this.modalBusy = true;

    const { id } = this.selected;

    (id
      ? this.service.update(id, { ...this.selected, ...this.tenantForm.value })
      : this.service.create(this.tenantForm.value)
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
          this.service.delete(id).subscribe(() => this.list.get());
        }
      });
  }

  hookToQuery() {
    this.list
      .hookToQuery(query => this.service.getList(query))
      .subscribe(res => {
        this.data = res;
      });
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

  sort(data: any) {
    const { prop, dir } = data.sorts[0];
    this.list.sortKey = prop;
    this.list.sortOrder = dir;
  }
}
