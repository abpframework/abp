import { Injectable } from '@angular/core';
import { eLayoutType, addAbpRoutes, ABP } from '@abp/ng.core';

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
  }
}
