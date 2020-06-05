import { Injectable } from '@angular/core';
import { eLayoutType, addAbpRoutes, ABP } from '@abp/ng.core';
import { addSettingTab } from '@abp/ng.theme.shared';
import { MyProjectNameSettingsComponent } from '../components/my-project-name-settings.component';

@Injectable({
  providedIn: 'root',
})
export class MyProjectNameConfigService {
  constructor() {
    addAbpRoutes({
      name: 'MyProjectName',
      path: 'my-project-name',
      layout: eLayoutType.application,
      order: 2,
    } as ABP.FullRoute);

    const route = addSettingTab({
      component: MyProjectNameSettingsComponent,
      name: 'MyProjectName Settings',
      order: 1,
      requiredPolicy: '',
    });
  }
}
