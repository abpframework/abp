import type { AbpLoginResult, UserLoginInfo } from './models/models';
import { RestService } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private restService = inject(RestService);

  apiName = 'AbpAccount';

  checkPasswordByLogin = (login: UserLoginInfo) =>
    this.restService.request<any, AbpLoginResult>({
      method: 'POST',
      url: '/api/account/check-password',
      body: login,
    },
    { apiName: this.apiName });

  loginByLogin = (login: UserLoginInfo) =>
    this.restService.request<any, AbpLoginResult>({
      method: 'POST',
      url: '/api/account/login',
      body: login,
    },
    { apiName: this.apiName });

  logout = () =>
    this.restService.request<any, void>({
      method: 'GET',
      url: '/api/account/logout',
    },
    { apiName: this.apiName });
}
