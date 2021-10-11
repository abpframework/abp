import { PageModule } from '@abp/ng.components/page';
import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { SettingManagementComponent } from './components/setting-management.component';
import { SettingManagementRoutingModule } from './setting-management-routing.module';

@NgModule({
  declarations: [SettingManagementComponent],
  exports: [SettingManagementComponent],
  imports: [SettingManagementRoutingModule, CoreModule, ThemeSharedModule, PageModule],
})
export class SettingManagementModule {
  static forChild(): ModuleWithProviders<SettingManagementModule> {
    return {
      ngModule: SettingManagementModule,
      providers: [],
    };
  }

  static forLazy(): NgModuleFactory<SettingManagementModule> {
    return new LazyModuleFactory(SettingManagementModule.forChild());
  }
}
