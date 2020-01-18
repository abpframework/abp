import { eLayoutType, RestService, addAbpRoutes } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AccountConfigService {
  constructor(private router: Router, private restService: RestService) {
    addAbpRoutes({
      name: 'AbpAccount::Menu:Account',
      path: 'account',
      invisible: true,
      layout: eLayoutType.application,
      children: [
        { path: 'login', name: 'AbpAccount::Login', order: 1 },
        { path: 'register', name: 'AbpAccount::Register', order: 2 },
        { path: 'manage-profile', name: 'AbpAccount::ManageYourProfile', order: 3 },
      ],
    });
  }
}
