import { addAbpRoutes, eLayoutType } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class IdentityConfigService {
  constructor() {
    addAbpRoutes([
      {
        name: 'AbpUiNavigation::Menu:Administration',
        path: '',
        order: 1,
        wrapper: true,
        iconClass: 'fa fa-wrench',
      },
      {
        name: 'AbpIdentity::Menu:IdentityManagement',
        path: 'identity',
        order: 1,
        parentName: 'AbpUiNavigation::Menu:Administration',
        layout: eLayoutType.application,
        iconClass: 'fa fa-id-card-o',
        children: [
          {
            path: 'roles',
            name: 'AbpIdentity::Roles',
            order: 1,
            requiredPolicy: 'AbpIdentity.Roles',
          },
          {
            path: 'users',
            name: 'AbpIdentity::Users',
            order: 2,
            requiredPolicy: 'AbpIdentity.Users',
          },
        ],
      },
    ]);
  }
}
