import { ModuleWithProviders, NgModule } from '@angular/core';
import { EmailSettingGroupComponent } from './components/email-setting-group/email-setting-group.component';
import { provideSettingManagementConfig } from './providers';

@NgModule({
  imports: [EmailSettingGroupComponent],
  declarations: [],
  exports: [EmailSettingGroupComponent],
})
export class SettingManagementConfigModule {
  /**
   * @deprecated forRoot method is deprecated, use `provideSettingManagementConfig` *function* for config settings.
   */
  static forRoot(): ModuleWithProviders<SettingManagementConfigModule> {
    return {
      ngModule: SettingManagementConfigModule,
      providers: [provideSettingManagementConfig()],
    };
  }
}
