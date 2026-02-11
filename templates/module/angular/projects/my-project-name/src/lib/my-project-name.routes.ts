import { RouterOutletComponent } from '@abp/ng.core';
import { Routes } from '@angular/router';
import { MyProjectNameComponent } from './components/my-project-name.component';

export const myProjectNameRoutes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: RouterOutletComponent,
    children: [
      {
        path: '',
        component: MyProjectNameComponent,
      },
    ],
  },
];
