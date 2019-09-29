import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { SettingLayoutComponent } from './components/setting-layout.component';
import { SettingManagementRoutingModule } from './setting-management-routing.module';

export const SETTING_LAYOUT = SettingLayoutComponent;

@NgModule({
  declarations: [SETTING_LAYOUT],
  imports: [SettingManagementRoutingModule, CoreModule, ThemeSharedModule],
  entryComponents: [SETTING_LAYOUT],
})
export class SettingManagementModule {}
