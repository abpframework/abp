import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { CoreModule } from '@abp/ng.core';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { EmailSettingGroupComponent } from './components/email-setting-group/email-setting-group.component';
import { provideSettingManagementConfig } from './providers';

@NgModule({
  imports: [CoreModule, ThemeSharedModule, NgxValidateCoreModule],
  declarations: [EmailSettingGroupComponent],
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
