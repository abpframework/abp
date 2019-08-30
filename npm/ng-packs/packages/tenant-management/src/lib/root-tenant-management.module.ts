import { ModuleWithProviders, NgModule } from '@angular/core';

@NgModule({})
export class RootTenantManagementModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: RootTenantManagementModule,
      providers: [],
    };
  }
}
