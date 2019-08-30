import { ModuleWithProviders, NgModule } from '@angular/core';

@NgModule({})
export class RootPermissionManagementModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: RootPermissionManagementModule,
      providers: [],
    };
  }
}
