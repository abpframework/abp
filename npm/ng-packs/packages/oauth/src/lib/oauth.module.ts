import { ModuleWithProviders, NgModule } from '@angular/core';
import { provideAbpOAuth } from './providers';

/**
 * @deprecated AbpOAuthModule is deprecated use `provideAbpOAuth` *function* instead.
 */
@NgModule()
export class AbpOAuthModule {
  static forRoot(): ModuleWithProviders<AbpOAuthModule> {
    return {
      ngModule: AbpOAuthModule,
      providers: [provideAbpOAuth()],
    };
  }
}
