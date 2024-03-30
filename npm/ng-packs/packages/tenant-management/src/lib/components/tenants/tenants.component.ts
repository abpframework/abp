import { ListService, PagedResultDto } from '@abp/ng.core';
import { eFeatureManagementComponents } from '@abp/ng.feature-management';
import { GetTenantsInput, TenantDto, TenantService } from '@abp/ng.tenant-management/proxy';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import {
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.components/extensible';
import { Component, inject, Injector, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
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
  protected readonly list = inject(ListService<GetTenantsInput>);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly service = inject(TenantService);
  protected readonly toasterService = inject(ToasterService);
  private readonly fb = inject(UntypedFormBuilder);
  private readonly injector = inject(Injector);

  data: PagedResultDto<TenantDto> = { items: [], totalCount: 0 };

  selected!: TenantDto;

  tenantForm!: UntypedFormGroup;

  isModalVisible!: boolean;

  visibleFeatures = false;

  providerKey!: string;

  modalBusy = false;

  featureManagementKey = eFeatureManagementComponents.FeatureManagement;

  get hasSelectedTenant(): boolean {
    return Boolean(this.selected.id);
  }

  onVisibleFeaturesChange = (value: boolean) => {
    this.visibleFeatures = value;
  };

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
        this.toasterService.success('AbpUi::SavedSuccessfully');
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
          this.toasterService.success('AbpUi::DeletedSuccessfully');
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
