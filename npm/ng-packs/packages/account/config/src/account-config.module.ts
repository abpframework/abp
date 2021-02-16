import { noop } from '@abp/ng.core';
import { APP_INITIALIZER, ModuleWithProviders, NgModule } from '@angular/core';
import { SessionHandler } from './handlers/session.handler';
import { ACCOUNT_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class AccountConfigModule {
  static forRoot(): ModuleWithProviders<AccountConfigModule> {
    return {
      ngModule: AccountConfigModule,
      providers: [
        ACCOUNT_ROUTE_PROVIDERS,
        {
          provide: APP_INITIALIZER,
          multi: true,
          deps: [SessionHandler],
          useFactory: noop,
        },
      ],
    };
  }
}
