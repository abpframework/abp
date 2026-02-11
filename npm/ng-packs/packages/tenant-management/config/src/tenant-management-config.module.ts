import { ModuleWithProviders, NgModule } from '@angular/core';
import { provideTenantManagementConfig } from './providers';

/**
 * @deprecated TenantManagementConfigModule is deprecated use `provideTenantManagementConfig` *function* instead.
 */
@NgModule()
export class TenantManagementConfigModule {
  static forRoot(): ModuleWithProviders<TenantManagementConfigModule> {
    return {
      ngModule: TenantManagementConfigModule,
      providers: [provideTenantManagementConfig()],
    };
  }
}
