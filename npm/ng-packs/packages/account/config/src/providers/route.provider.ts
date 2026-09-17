import { eLayoutType, RoutesService } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';
import { eAccountRouteNames } from '../enums/route-names';

export const ACCOUNT_ROUTE_PROVIDERS = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

export function configureRoutes() {
  const routes = inject(RoutesService);
  routes.add([
    {
      path: undefined,
      name: eAccountRouteNames.Account,
      invisible: true,
      layout: eLayoutType.account,
      breadcrumbText: eAccountRouteNames.Account,
      iconClass: 'bi bi-person-fill-gear',
      order: 1,
    },
    {
      path: '/account/login',
      name: eAccountRouteNames.Login,
      parentName: eAccountRouteNames.Account,
      layout: eLayoutType.account,
      order: 1,
    },
    {
      path: '/account/register',
      name: eAccountRouteNames.Register,
      parentName: eAccountRouteNames.Account,
      layout: eLayoutType.account,
      order: 2,
    },
    {
      path: '/account/manage',
      name: eAccountRouteNames.ManageProfile,
      parentName: eAccountRouteNames.Account,
      layout: eLayoutType.application,
      breadcrumbText: 'AbpAccount::Manage',
      iconClass: 'bi bi-kanban-fill',
      order: 3,
    },
    {
      path: '/account/forgot-password',
      parentName: eAccountRouteNames.Account,
      name: eAccountRouteNames.ForgotPassword,
      layout: eLayoutType.account,
      invisible: true,
    },
    {
      path: '/account/reset-password',
      parentName: eAccountRouteNames.Account,
      name: eAccountRouteNames.ResetPassword,
      layout: eLayoutType.account,
      invisible: true,
    },
  ]);
}
