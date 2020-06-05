import { Injectable, Injector } from '@angular/core';
import { addAbpRoutes, eLayoutType, PatchRouteByName, ABP } from '@abp/ng.core';
import { getSettingTabs } from '@abp/ng.theme.shared';
import { Store } from '@ngxs/store';
import { eSettingManagementRouteNames } from '@abp/ng.setting-management';

@Injectable({
  providedIn: 'root',
})
export class SettingManagementConfigService {
  get store(): Store {
    return this.injector.get(Store);
  }

  constructor(private injector: Injector) {
    const route = {
      name: eSettingManagementRouteNames.Settings,
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
        this.store.dispatch(
          new PatchRouteByName('AbpSettingManagement::Settings', { ...route, invisible: true }),
        );
      }
    });
  }
}
