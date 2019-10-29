import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SettingManagementComponent } from './components/setting-management.component';
import { DynamicLayoutComponent } from '@abp/ng.core';

const routes: Routes = [
  {
    path: '',
    component: DynamicLayoutComponent,
    children: [
      { path: '', component: SettingManagementComponent, data: { requiredPolicy: 'AbpAccount.SettingManagement' } },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SettingManagementRoutingModule {}
