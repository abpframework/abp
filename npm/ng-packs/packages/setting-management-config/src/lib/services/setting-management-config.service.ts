import { Injectable } from '@angular/core';
import { ABP_ROUTES, eLayoutType } from '@abp/ng.core';

@Injectable({
  providedIn: 'root',
})
export class SettingManagementConfigService {
  constructor() {
    ABP_ROUTES.push({
      name: 'Settings',
      path: 'setting-management',
      parentName: 'AbpUiNavigation::Menu:Administration',
      layout: eLayoutType.application,
      order: 6,
      iconClass: 'fa fa-cog',
    });
  }
}
