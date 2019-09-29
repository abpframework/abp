import { DynamicLayoutComponent } from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SETTING_MANAGEMENT_ROUTES } from './constants/routes';
import { SettingLayoutComponent } from './components/setting-layout.component';

const routes: Routes = [
  {
    path: 'setting-management',
    component: DynamicLayoutComponent,
    children: [{ path: '', component: SettingLayoutComponent }],
    data: { routes: SETTING_MANAGEMENT_ROUTES, settings: [] },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SettingManagementRoutingModule {}
