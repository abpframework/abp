import { ABP, eLayoutType } from '@abp/ng.core';

export const ACCOUNT_ROUTES = [
  {
    name: 'Account',
    path: 'account',
    invisible: true,
    layout: eLayoutType.application,
    children: [{ path: 'login', name: 'Login', order: 1 }, { path: 'register', name: 'Register', order: 2 }],
  },
] as ABP.FullRoute[];
