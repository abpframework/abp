import { ModuleWithProviders, NgModule } from '@angular/core';
import { CoreModule } from '@abp/ng.core';
import { EmailSettingGroupComponent } from './components/email-setting-group/email-setting-group.component';
import { SETTING_MANAGEMENT_ROUTE_PROVIDERS } from './providers/route.provider';
import { SETTING_MANAGEMENT_SETTING_TAB_PROVIDERS } from './providers/setting-tab.provider';
import { NgxValidateCoreModule } from '@ngx-validate/core';

@NgModule({
  imports: [CoreModule, NgxValidateCoreModule],
  declarations: [EmailSettingGroupComponent],
  exports: [EmailSettingGroupComponent],
})
export class SettingManagementConfigModule {
  static forRoot(): ModuleWithProviders<SettingManagementConfigModule> {
    return {
      ngModule: SettingManagementConfigModule,
      providers: [SETTING_MANAGEMENT_ROUTE_PROVIDERS, SETTING_MANAGEMENT_SETTING_TAB_PROVIDERS],
    };
  }
}
