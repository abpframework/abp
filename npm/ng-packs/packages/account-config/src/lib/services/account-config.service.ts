import { ABP_ROUTES, eLayoutType, RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AccountConfigService {
  constructor(private router: Router, private restService: RestService) {
    ABP_ROUTES.push({
      name: 'AbpAccount::Menu:Account',
      path: 'account',
      invisible: true,
      layout: eLayoutType.application,
      children: [
        { path: 'login', name: 'AbpAccount::Login', order: 1 },
        { path: 'register', name: 'AbpAccount::Register', order: 2 },
      ],
    });
  }
}
