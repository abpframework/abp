import { LazyModuleFactory } from '@abp/ng.core';
import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { TenantManagementExtensionsGuard } from './guards/extensions.guard';
import { TenantManagementConfigOptions } from './models/config-options';
import {
  TENANT_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS,
  TENANT_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS,
} from './tokens/extensions.token';
import { TenantsComponent } from './components';
import { TenantManagementRoutingModule } from './tenant-management-routing.module';

@NgModule({
  declarations: [],
  exports: [],
  imports: [TenantManagementRoutingModule, TenantsComponent],
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
  /**
   * @deprecated `TenantManagementModule.forLazy()` is deprecated. You can use `createRoutes` **function** instead.
   */
  static forLazy(
    options: TenantManagementConfigOptions = {},
  ): NgModuleFactory<TenantManagementModule> {
    return new LazyModuleFactory(TenantManagementModule.forChild(options));
  }
}
