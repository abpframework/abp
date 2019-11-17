import { IDENTITY_ROUTES } from '@abp/ng.identity';
import { ACCOUNT_ROUTES } from '@abp/ng.account';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ABP } from '@abp/ng.core';
import { TENANT_MANAGEMENT_ROUTES } from '@abp/ng.tenant-management';
import { ApplicationLayoutComponent } from '@abp/ng.theme.basic';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./home/home.module').then(m => m.HomeModule),
    data: {
      routes: {
        name: '::Menu:Home',
      } as ABP.Route,
    },
  },
  {
    path: 'account',
    loadChildren: () => import('./lazy-libs/account-wrapper.module').then(m => m.AccountWrapperModule),
    data: { routes: ACCOUNT_ROUTES },
  },
  {
    path: 'identity',
    loadChildren: () => import('./lazy-libs/identity-wrapper.module').then(m => m.IdentityWrapperModule),
    data: { routes: IDENTITY_ROUTES },
  },
  {
    path: 'tenant-management',
    loadChildren: () =>
      import('./lazy-libs/tenant-management-wrapper.module').then(m => m.TenantManagementWrapperModule),
    data: { routes: TENANT_MANAGEMENT_ROUTES },
  },
  {
    path: 'books',
    component: ApplicationLayoutComponent,
    loadChildren: () => import('./books/books.module').then(m => m.BooksModule),
    data: {
      routes: {
        name: 'Books',
      } as ABP.Route,
    },
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
