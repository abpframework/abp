import { eLayoutType, ABP } from '@abp/ng.core';

export const IDENTITY_ROUTES = [
  {
    name: 'Administration',
    path: '',
    order: 1,
    wrapper: true,
  },
  {
    name: 'Identity Management',
    path: 'identity',
    order: 1,
    parentName: 'Administration',
    layout: eLayoutType.application,
    iconClass: 'fa fa-id-card-o',
    children: [
      { path: 'roles', name: 'Roles', order: 2, requiredPolicy: 'AbpIdentity.Roles' },
      { path: 'users', name: 'Users', order: 1, requiredPolicy: 'AbpIdentity.Users' },
    ],
  },
] as ABP.FullRoute[];
