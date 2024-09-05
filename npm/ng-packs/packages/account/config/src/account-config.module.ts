import { ModuleWithProviders, NgModule } from '@angular/core';
import { provideAccountConfig } from './providers';

/**
 * @deprecated AccountConfigModule is deprecated use `provideAccountConfig` *function* instead.
 */
@NgModule()
export class AccountConfigModule {
  static forRoot(): ModuleWithProviders<AccountConfigModule> {
    return {
      ngModule: AccountConfigModule,
      providers: [provideAccountConfig()],
    };
  }
}
