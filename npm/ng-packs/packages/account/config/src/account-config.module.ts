import { ModuleWithProviders, NgModule } from '@angular/core';
import { ACCOUNT_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class AccountConfigModule {
  static forRoot(): ModuleWithProviders<AccountConfigModule> {
    return {
      ngModule: AccountConfigModule,
      providers: [ACCOUNT_ROUTE_PROVIDERS],
    };
  }
}
