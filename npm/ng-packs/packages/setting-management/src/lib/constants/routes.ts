import { ABP, eLayoutType } from '@abp/ng.core';

export const SETTING_MANAGEMENT_ROUTES = {
  routes: [
    {
      name: 'Settings',
      path: 'setting-management',
      parentName: 'AbpUiNavigation::Menu:Administration',
      layout: eLayoutType.application,
      order: 6,
      iconClass: 'fa fa-cog',
    },
  ] as ABP.FullRoute[],
};
