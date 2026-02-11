import { LazyModuleFactory } from '@abp/ng.core';
import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { SettingManagementRoutingModule } from './setting-management-routing.module';
import { SettingManagementComponent } from './components/setting-management.component';

export const SETTING_MANAGEMENT_MODULE_EXPORTS = [SettingManagementComponent];

@NgModule({
  declarations: [],
  exports: [],
  imports: [SettingManagementRoutingModule, ...SETTING_MANAGEMENT_MODULE_EXPORTS],
})
export class SettingManagementModule {
  static forChild(): ModuleWithProviders<SettingManagementModule> {
    return {
      ngModule: SettingManagementModule,
      providers: [],
    };
  }
  /**
   * @deprecated `SettingManagementModule.forLazy()` is deprecated. You can use `createRoutes` **function** instead.
   */
  static forLazy(): NgModuleFactory<SettingManagementModule> {
    return new LazyModuleFactory(SettingManagementModule.forChild());
  }
}
