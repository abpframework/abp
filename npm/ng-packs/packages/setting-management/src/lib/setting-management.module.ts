import { CoreModule, LazyModuleFactory } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ModuleWithProviders, NgModule, NgModuleFactory } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { SettingManagementComponent } from './components/setting-management.component';
import { SettingManagementRoutingModule } from './setting-management-routing.module';
import { SettingManagementState } from './states/setting-management.state';

@NgModule({
  declarations: [SettingManagementComponent],
  exports: [SettingManagementComponent],
  imports: [
    SettingManagementRoutingModule,
    CoreModule,
    ThemeSharedModule,
    NgxsModule.forFeature([SettingManagementState]),
  ],
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
