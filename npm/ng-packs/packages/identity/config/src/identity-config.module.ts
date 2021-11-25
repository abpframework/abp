import { ModuleWithProviders, NgModule } from '@angular/core';
import { IDENTITY_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class IdentityConfigModule {
  static forRoot(): ModuleWithProviders<IdentityConfigModule> {
    return {
      ngModule: IdentityConfigModule,
      providers: [IDENTITY_ROUTE_PROVIDERS],
    };
  }
}
