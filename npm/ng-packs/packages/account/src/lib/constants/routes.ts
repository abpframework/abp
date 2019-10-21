import { ABP, eLayoutType } from '@abp/ng.core';

/**
 *
 * @deprecated since version 0.9
 */
export const ACCOUNT_ROUTES = {
  routes: [
    {
      name: 'Account',
      path: 'account',
      invisible: true,
      layout: eLayoutType.application,
      children: [{ path: 'login', name: 'Login', order: 1 }, { path: 'register', name: 'Register', order: 2 }],
    },
  ] as ABP.FullRoute[],
};
