import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {
  AuthGuardFn,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from '@abp/ng.core';
import { SettingManagementComponent } from './components/setting-management.component';
import { eSettingManagementComponents } from './enums/components';

const routes: Routes = [
  {
    path: '',
    component: RouterOutletComponent,
    canActivate: [AuthGuardFn],
    children: [
      {
        path: '',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'AbpAccount.SettingManagement',
          replaceableComponent: {
            key: eSettingManagementComponents.SettingManagement,
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
