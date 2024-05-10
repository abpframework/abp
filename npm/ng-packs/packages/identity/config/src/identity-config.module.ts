import { ModuleWithProviders, NgModule } from '@angular/core';
import { provideIdentityConfig } from './providers';

/**
 * @deprecated IdentityConfigModule is deprecated use `provideIdentityConfig` *function* instead.
 */
@NgModule()
export class IdentityConfigModule {
  static forRoot(): ModuleWithProviders<IdentityConfigModule> {
    return {
      ngModule: IdentityConfigModule,
      providers: [provideIdentityConfig()],
    };
  }
}
