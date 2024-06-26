import { NAVIGATE_TO_MANAGE_PROFILE } from '@abp/ng.core';
import { makeEnvironmentProviders, Injector } from '@angular/core';
import { navigateToManageProfileFactory } from '../utils';
import { ACCOUNT_ROUTE_PROVIDERS } from './';

export function provideAccountConfig() {
  return makeEnvironmentProviders([
    ACCOUNT_ROUTE_PROVIDERS,
    {
      provide: NAVIGATE_TO_MANAGE_PROFILE,
      useFactory: navigateToManageProfileFactory,
      deps: [Injector],
    },
  ]);
}
