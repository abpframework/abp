import { Routes } from '@angular/router';

export const homeRoutes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () => import('./home.component').then(m => m.HomeComponent),
  },
];
