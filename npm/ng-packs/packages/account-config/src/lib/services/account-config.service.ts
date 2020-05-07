import { addAbpRoutes, eLayoutType } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { eAccountRouteNames } from '@abp/ng.account';

@Injectable({
  providedIn: 'root',
})
export class AccountConfigService {
  constructor() {
    addAbpRoutes({
      name: eAccountRouteNames.Account,
      path: 'account',
      invisible: true,
      layout: eLayoutType.application,
      children: [
        { path: 'login', name: eAccountRouteNames.Login, order: 1 },
        { path: 'register', name: eAccountRouteNames.Register, order: 2 },
        { path: 'manage-profile', name: eAccountRouteNames.ManageProfile, order: 3 },
      ],
    });
  }
}
