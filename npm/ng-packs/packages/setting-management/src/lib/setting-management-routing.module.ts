import {
  DynamicLayoutComponent,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
} from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SettingManagementComponent } from './components/setting-management.component';

const routes: Routes = [
  {
    path: '',
    component: DynamicLayoutComponent,
    children: [
      {
        path: '',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'AbpAccount.SettingManagement',
          replaceableComponent: {
            key: 'SettingManagement.SettingManagementComponent',
            defaultComponent: SettingManagementComponent,
          } as ReplaceableComponents.RouteData,
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SettingManagementRoutingModule {}
