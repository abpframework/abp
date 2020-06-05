import { addAbpRoutes, eLayoutType } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { eIdentityRouteNames } from '@abp/ng.identity';

@Injectable({
  providedIn: 'root',
})
export class IdentityConfigService {
  constructor() {
    addAbpRoutes([
      {
        name: eIdentityRouteNames.Administration,
        path: '',
        order: 1,
        wrapper: true,
        iconClass: 'fa fa-wrench',
      },
      {
        name: eIdentityRouteNames.IdentityManagement,
        path: 'identity',
        order: 1,
        parentName: eIdentityRouteNames.Administration,
        layout: eLayoutType.application,
        iconClass: 'fa fa-id-card-o',
        children: [
          {
            path: 'roles',
            name: eIdentityRouteNames.Roles,
            order: 1,
            requiredPolicy: 'AbpIdentity.Roles',
          },
          {
            path: 'users',
            name: eIdentityRouteNames.Users,
            order: 2,
            requiredPolicy: 'AbpIdentity.Users',
          },
        ],
      },
    ]);
  }
}
