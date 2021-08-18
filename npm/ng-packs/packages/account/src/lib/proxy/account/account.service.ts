import type { RegisterDto, ResetPasswordDto, SendPasswordResetCodeDto } from './models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { IdentityUserDto } from '../identity/models';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  apiName = 'AbpAccount';

  register = (input: RegisterDto) =>
    this.restService.request<any, IdentityUserDto>(
      {
        method: 'POST',
        url: '/api/account/register',
        body: input,
      },
      { apiName: this.apiName },
    );

  resetPassword = (input: ResetPasswordDto) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/account/reset-password',
        body: input,
      },
      { apiName: this.apiName },
    );

  sendPasswordResetCode = (input: SendPasswordResetCodeDto) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/account/send-password-reset-code',
        body: input,
      },
      { apiName: this.apiName },
    );

  constructor(private restService: RestService) {}
}
