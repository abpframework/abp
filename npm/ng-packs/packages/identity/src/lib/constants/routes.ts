import { eLayoutType, ABP } from '@abp/ng.core';

/**
 *
 * @deprecated
 */
export const IDENTITY_ROUTES = {
  routes: [
    {
      name: 'AbpUiNavigation::Menu:Administration',
      path: '',
      order: 1,
      wrapper: true,
    },
    {
      name: 'AbpIdentity::Menu:IdentityManagement',
      path: 'identity',
      order: 1,
      parentName: 'AbpUiNavigation::Menu:Administration',
      layout: eLayoutType.application,
      iconClass: 'fa fa-id-card-o',
      children: [
        { path: 'roles', name: 'AbpIdentity::Roles', order: 2, requiredPolicy: 'AbpIdentity.Roles' },
        { path: 'users', name: 'AbpIdentity::Users', order: 1, requiredPolicy: 'AbpIdentity.Users' },
      ],
    },
  ] as ABP.FullRoute[],
};
