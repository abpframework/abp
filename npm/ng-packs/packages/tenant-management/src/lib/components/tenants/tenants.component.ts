import {
  ChangeDetectionStrategy,
  Component,
  DOCUMENT,
  inject,
  Injector,
  makeStateKey,
  signal,
} from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import {
  FormsModule,
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
} from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { NgxValidateCoreModule } from '@ngx-validate/core';

import {
  ListService,
  LocalizationPipe,
  PagedResultDto,
  ReplaceableTemplateDirective,
} from '@abp/ng.core';
import {
  eFeatureManagementComponents,
  FeatureManagementComponent,
} from '@abp/ng.feature-management';
import { GetTenantsInput, TenantDto, TenantService } from '@abp/ng.tenant-management/proxy';
import {
  ButtonComponent,
  Confirmation,
  ConfirmationService,
  ModalCloseDirective,
  ModalComponent,
  ToasterService,
} from '@abp/ng.theme.shared';
import {
  ExtensibleFormComponent,
  ExtensibleTableComponent,
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.components/extensible';
import { PageComponent } from '@abp/ng.components/page';
import { eTenantManagementComponents } from '../../enums/components';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-tenants',
  templateUrl: './tenants.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eTenantManagementComponents.Tenants,
    },
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    PageComponent,
    LocalizationPipe,
    ExtensibleTableComponent,
    ModalComponent,
    FeatureManagementComponent,
    ButtonComponent,
    ReplaceableTemplateDirective,
    ExtensibleFormComponent,
    ModalCloseDirective,
    NgxValidateCoreModule,
  ],
})
export class TenantsComponent {
  protected readonly list = inject(ListService<GetTenantsInput>);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly service = inject(TenantService);
  protected readonly toasterService = inject(ToasterService);
  private readonly fb = inject(UntypedFormBuilder);
  private readonly injector = inject(Injector);
  private document = inject(DOCUMENT);

  readonly data = toSignal(
    this.list.hookToQuery(query => this.service.getList(query)),
    {
      initialValue: { items: [], totalCount: 0 } as PagedResultDto<TenantDto>,
    },
  );

  selected!: TenantDto;

  tenantForm!: UntypedFormGroup;

  readonly isModalVisible = signal(false);
  readonly visibleFeatures = signal(false);
  readonly modalBusy = signal(false);

  providerKey!: string;

  featureManagementKey = eFeatureManagementComponents.FeatureManagement;
  TENANTS_KEY = makeStateKey<PagedResultDto<TenantDto>>('tenants');

  get hasSelectedTenant(): boolean {
    return Boolean(this.selected.id);
  }

  onVisibleFeaturesChange = (value: boolean) => {
    this.visibleFeatures.set(value);
  };

  private createTenantForm() {
    const data = new FormPropData(this.injector, this.selected);
    this.tenantForm = generateFormFromProps(data);
  }

  addTenant() {
    this.selected = {} as TenantDto;
    this.createTenantForm();
    this.isModalVisible.set(true);
  }

  editTenant(id: string) {
    this.service.get(id).subscribe(res => {
      this.selected = res;
      this.createTenantForm();
      this.isModalVisible.set(true);
    });
  }

  save() {
    if (!this.tenantForm.valid || this.modalBusy()) return;
    this.modalBusy.set(true);

    const { id } = this.selected;

    (id
      ? this.service.update(id, { ...this.selected, ...this.tenantForm.value })
      : this.service.create(this.tenantForm.value)
    )
      .pipe(finalize(() => this.modalBusy.set(false)))
      .subscribe(() => {
        this.isModalVisible.set(false);
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

  onSharedDatabaseChange(value: boolean) {
    if (!value) {
      setTimeout(() => {
        const defaultConnectionString = this.document.getElementById(
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
      this.visibleFeatures.set(true);
    }, 0);
  }

  sort(data: any) {
    const { prop, dir } = data.sorts[0];
    this.list.sortKey = prop;
    this.list.sortOrder = dir;
  }
}
