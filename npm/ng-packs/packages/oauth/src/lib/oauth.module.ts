import { ModuleWithProviders, NgModule } from '@angular/core';
import { provideAbpOAuthConfig } from './providers';

/**
 * @deprecated AbpOAuthModule is deprecated use `provideAbpOAuthConfig` *function* instead.
 */
@NgModule()
export class AbpOAuthModule {
  static forRoot(): ModuleWithProviders<AbpOAuthModule> {
    return {
      ngModule: AbpOAuthModule,
      providers: [provideAbpOAuthConfig()],
    };
  }
}
