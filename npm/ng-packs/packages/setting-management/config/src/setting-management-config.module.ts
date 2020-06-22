import { ModuleWithProviders, NgModule } from '@angular/core';
import { SETTING_MANAGEMENT_ROUTE_PROVIDERS } from './providers/route.provider';

@NgModule()
export class SettingManagementConfigModule {
  static forRoot(): ModuleWithProviders<SettingManagementConfigModule> {
    return {
      ngModule: SettingManagementConfigModule,
      providers: [SETTING_MANAGEMENT_ROUTE_PROVIDERS],
    };
  }
}
