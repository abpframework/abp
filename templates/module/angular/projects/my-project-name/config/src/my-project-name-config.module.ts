import { ModuleWithProviders, NgModule } from '@angular/core';
import { MY_PROJECT_NAME_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class MyProjectNameConfigModule {
  static forRoot(): ModuleWithProviders<MyProjectNameConfigModule> {
    return {
      ngModule: MyProjectNameConfigModule,
      providers: [MY_PROJECT_NAME_ROUTE_PROVIDERS],
    };
  }
}
