import { makeEnvironmentProviders } from '@angular/core';
import { IDENTITY_ROUTE_PROVIDERS } from './route.provider';

export function provideIdentityConfig() {
  return makeEnvironmentProviders([IDENTITY_ROUTE_PROVIDERS]);
}
