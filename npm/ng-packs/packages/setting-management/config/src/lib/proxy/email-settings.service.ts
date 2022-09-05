import type { EmailSettingsDto, SendTestEmailInput, UpdateEmailSettingsDto } from './models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EmailSettingsService {
  apiName = 'SettingManagement';

  get = () =>
    this.restService.request<any, EmailSettingsDto>({
      method: 'GET',
      url: '/api/setting-management/emailing',
    },
    { apiName: this.apiName });

  sendTestEmail = (input: SendTestEmailInput) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/setting-management/emailing/send-test-email',
      body: input,
    },
    { apiName: this.apiName });

  update = (input: UpdateEmailSettingsDto) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/setting-management/emailing',
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
