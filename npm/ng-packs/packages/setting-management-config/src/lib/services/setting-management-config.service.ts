import { Injectable } from '@angular/core';
import { addAbpRoutes, eLayoutType, PatchRouteByName, ABP } from '@abp/ng.core';
import { getSettingTabs } from '@abp/ng.theme.shared';
import { Store } from '@ngxs/store';

@Injectable({
  providedIn: 'root',
})
export class SettingManagementConfigService {
  constructor(private store: Store) {
    const route = {
      name: 'AbpSettingManagement::Settings',
      path: 'setting-management',
      parentName: 'AbpUiNavigation::Menu:Administration',
      requiredPolicy: 'AbpAccount.SettingManagement',
      layout: eLayoutType.application,
      order: 6,
      iconClass: 'fa fa-cog',
    } as ABP.FullRoute;

    addAbpRoutes(route);

    setTimeout(() => {
      const tabs = getSettingTabs();
      if (!tabs || !tabs.length) {
        this.store.dispatch(new PatchRouteByName('AbpSettingManagement::Settings', { ...route, invisible: true }));
      }
    });
  }
}
