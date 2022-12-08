import type { ApplicationConfigurationDto, ApplicationConfigurationRequestOptions } from './models';
import { RestService } from '../../../../../../services/rest.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AbpApplicationConfigurationService {
  apiName = 'abp';
  
  get = (options: ApplicationConfigurationRequestOptions) =>
    this.restService.request<any, ApplicationConfigurationDto>({
      method: 'GET',
      url: '/api/abp/application-configuration',
      params: { includeLocalizationResources: options.includeLocalizationResources },
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
