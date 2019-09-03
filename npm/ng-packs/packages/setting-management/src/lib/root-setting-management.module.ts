import { ModuleWithProviders, NgModule } from '@angular/core';

@NgModule({})
export class RootSettingManagementModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: RootSettingManagementModule,
      providers: [],
    };
  }
}
