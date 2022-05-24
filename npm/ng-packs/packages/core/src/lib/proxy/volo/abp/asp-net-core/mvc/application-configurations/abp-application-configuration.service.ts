import type { ApplicationConfigurationDto } from './models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AbpApplicationConfigurationService {
  apiName = 'abp';

  get = () =>
    this.restService.request<any, ApplicationConfigurationDto>({
      method: 'GET',
      url: '/api/abp/application-configuration',
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
