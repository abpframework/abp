import { Routes } from '@angular/router';

export const appRoutes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () => import('./home/home.component').then(m => m.HomeComponent),
  },
  {
    path: 'dynamic-form',
    loadComponent: () => import('./dynamic-form-page/dynamic-form-page.component').then(m => m.DynamicFormPageComponent),
  },
  {
    path: 'localization-test',
    loadComponent: () => import('./localization-test/localization-test.component').then(m => m.LocalizationTestComponent),
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(m => m.createRoutes()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(m => m.createRoutes()),
  },
  {
    path: 'tenant-management',
    loadChildren: () => import('@abp/ng.tenant-management').then(m => m.createRoutes()),
  },
  {
    path: 'setting-management',
    loadChildren: () => import('@abp/ng.setting-management').then(m => m.createRoutes()),
  },
];
