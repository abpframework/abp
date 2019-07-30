import { IDENTITY_ROUTES } from '@abp/ng.identity';
import { ACCOUNT_ROUTES } from '@abp/ng.account';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ABP } from '@abp/ng.core';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  {
    path: 'home',
    loadChildren: () => import('./home/home.module').then(m => m.HomeModule),
    data: {
      routes: {
        name: 'Home',
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
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
