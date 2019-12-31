import {
  AuthGuard,
  DynamicLayoutComponent,
  PermissionGuard,
  ReplaceableComponents,
  ReplaceableRouteContainerComponent,
} from '@abp/ng.core';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MyProjectNameComponent } from './components/my-project-name.component';

const routes: Routes = [
  {
    path: '',
    component: DynamicLayoutComponent,
    canActivate: [AuthGuard, PermissionGuard],
    children: [
      {
        path: '',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: '',
          replaceableComponent: {
            defaultComponent: MyProjectNameComponent,
            key: 'MyProjectName.MyProjectNameComponent',
          } as ReplaceableComponents.RouteData<MyProjectNameComponent>,
        },
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MyProjectNameRoutingModule {}
