import { ModuleWithProviders, NgModule, makeEnvironmentProviders } from '@angular/core';
import { TENANT_MANAGEMENT_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class TenantManagementConfigModule {
  static forRoot(): ModuleWithProviders<TenantManagementConfigModule> {
    return {
      ngModule: TenantManagementConfigModule,
      providers: [TENANT_MANAGEMENT_ROUTE_PROVIDERS],
    };
  }
}

export function provideTenantManagementConfig() {
  return makeEnvironmentProviders([TENANT_MANAGEMENT_ROUTE_PROVIDERS]);
}
