import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { FeatureManagementModule } from '@abp/ng.feature-management';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { UiExtensionsModule } from '@abp/ng.theme.shared/extensions';
import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { NgxsModule } from '@ngxs/store';
import { TenantsComponent } from './components/tenants/tenants.component';
import { TenantManagementExtensionsGuard } from './guards/extensions.guard';
import { TenantManagementConfigOptions } from './models/config-options';
import { TenantManagementState } from './states/tenant-management.state';
import { TenantManagementRoutingModule } from './tenant-management-routing.module';
import {
  TENANT_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS,
  TENANT_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS,
} from './tokens/extensions.token';

@NgModule({
  declarations: [TenantsComponent],
  exports: [TenantsComponent],
  imports: [
    TenantManagementRoutingModule,
    NgxsModule.forFeature([TenantManagementState]),
    NgxValidateCoreModule,
    CoreModule,
    ThemeSharedModule,
    NgbDropdownModule,
    FeatureManagementModule,
    UiExtensionsModule,
  ],
})
export class TenantManagementModule {
  static forChild(
    options: TenantManagementConfigOptions = {},
  ): ModuleWithProviders<TenantManagementModule> {
    return {
      ngModule: TenantManagementModule,
      providers: [
        {
          provide: TENANT_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS,
          useValue: options.entityActionContributors,
        },
        {
          provide: TENANT_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS,
          useValue: options.toolbarActionContributors,
        },
        {
          provide: TENANT_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS,
          useValue: options.entityPropContributors,
        },
        {
          provide: TENANT_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS,
          useValue: options.createFormPropContributors,
        },
        {
          provide: TENANT_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS,
          useValue: options.editFormPropContributors,
        },
        TenantManagementExtensionsGuard,
      ],
    };
  }

  static forLazy(
    options: TenantManagementConfigOptions = {},
  ): NgModuleFactory<TenantManagementModule> {
    return new LazyModuleFactory(TenantManagementModule.forChild(options));
  }
}
