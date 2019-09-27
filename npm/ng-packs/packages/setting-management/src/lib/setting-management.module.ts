import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgModule } from '@angular/core';
import { SettingManagementRoutingModule } from './setting-management-routing.module';
import { SettingManagementComponent } from './components/setting-management.component';

@NgModule({
  declarations: [SettingManagementComponent],
  imports: [SettingManagementRoutingModule, CoreModule, ThemeSharedModule],
})
export class SettingManagementModule {}
