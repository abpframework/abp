import { CoreModule, noop } from '@abp/ng.core';
import { NgModule, ModuleWithProviders, APP_INITIALIZER, Self } from '@angular/core';
import { SettingComponent } from './components/setting/setting.component';
import { SettingManagementRoutingModule } from './setting-management-routing.module';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { InitialService } from './components/services/initial.service';

@NgModule({
  declarations: [SettingComponent],
  imports: [SettingManagementRoutingModule, CoreModule, ThemeSharedModule],
  providers: [InitialService],
})
export class SettingManagementModule {
  constructor(@Self() initialService: InitialService) {}
}
