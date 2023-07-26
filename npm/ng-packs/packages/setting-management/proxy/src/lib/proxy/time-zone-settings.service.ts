import type { NameValue } from './volo/abp/models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TimeZoneSettingsService {
  apiName = 'SettingManagement';
  

  get = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'GET',
      responseType: 'text',
      url: '/api/setting-management/timezone',
    },
    { apiName: this.apiName,...config });
  

  getTimezones = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, NameValue[]>({
      method: 'GET',
      url: '/api/setting-management/timezone/timezones',
    },
    { apiName: this.apiName,...config });
  

  update = (timezone: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/setting-management/timezone',
      params: { timezone },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
