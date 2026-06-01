import { Provider, makeEnvironmentProviders } from '@angular/core';
import { CMS_KIT_PUBLIC_ROUTE_PROVIDERS } from './route.provider';

export function provideCmsKitPublicConfig() {
  return makeEnvironmentProviders([CMS_KIT_PUBLIC_ROUTE_PROVIDERS]);
}
