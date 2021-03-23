import { Injector, ModuleWithProviders, NgModule } from '@angular/core';
import { NAVIGATE_TO_MANAGE_PROFILE } from '@abp/ng.core';
import { ACCOUNT_ROUTE_PROVIDERS } from './providers/route.provider';
import { navigateToManageProfileFactory } from './utils/factories';

@NgModule()
export class AccountConfigModule {
  static forRoot(): ModuleWithProviders<AccountConfigModule> {
    return {
      ngModule: AccountConfigModule,
      providers: [
        ACCOUNT_ROUTE_PROVIDERS,
        {
          provide: NAVIGATE_TO_MANAGE_PROFILE,
          useFactory: navigateToManageProfileFactory,
          deps: [Injector],
        },
      ],
    };
  }
}
