import type { ApplicationConfigurationDto, ApplicationConfigurationRequestOptions } from './models';
import { RestService } from '../../../../../../services';
import { Rest } from '../../../../../../models';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AbpApplicationConfigurationService {
  private restService = inject(RestService);

  apiName = 'abp';


  get = (options: ApplicationConfigurationRequestOptions, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApplicationConfigurationDto>({
      method: 'GET',
      url: '/api/abp/application-configuration',
      params: { includeLocalizationResources: options.includeLocalizationResources },
    },
      { apiName: this.apiName, ...config });
}
