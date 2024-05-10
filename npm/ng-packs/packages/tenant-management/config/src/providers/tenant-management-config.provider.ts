import { makeEnvironmentProviders } from '@angular/core';
import { TENANT_MANAGEMENT_ROUTE_PROVIDERS } from './route.provider';

export function provideTenantManagementConfig() {
  return makeEnvironmentProviders([TENANT_MANAGEMENT_ROUTE_PROVIDERS]);
}
