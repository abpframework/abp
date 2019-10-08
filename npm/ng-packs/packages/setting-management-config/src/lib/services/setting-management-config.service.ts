import { Injectable } from '@angular/core';
import { addAbpRoutes, eLayoutType } from '@abp/ng.core';

@Injectable({
  providedIn: 'root',
})
export class SettingManagementConfigService {
  constructor() {
    addAbpRoutes({
      name: 'Settings',
      path: 'setting-management',
      parentName: 'AbpUiNavigation::Menu:Administration',
      layout: eLayoutType.application,
      order: 6,
      iconClass: 'fa fa-cog',
    });
  }
}
