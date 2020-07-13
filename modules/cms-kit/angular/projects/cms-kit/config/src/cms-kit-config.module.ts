import { ModuleWithProviders, NgModule } from '@angular/core';
import { MY_PROJECT_NAME_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class CmsKitConfigModule {
  static forRoot(): ModuleWithProviders<CmsKitConfigModule> {
    return {
      ngModule: CmsKitConfigModule,
      providers: [MY_PROJECT_NAME_ROUTE_PROVIDERS],
    };
  }
}
