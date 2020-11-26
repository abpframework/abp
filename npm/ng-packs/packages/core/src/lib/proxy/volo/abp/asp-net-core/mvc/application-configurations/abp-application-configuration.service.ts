import { Injectable } from '@angular/core';
import { RestService } from '../../../../../../services/rest.service';
import type { ApplicationConfigurationDto } from './models';

@Injectable({
  providedIn: 'root',
})
export class AbpApplicationConfigurationService {
  apiName = 'abp';

  get() {
    return this.restService.request<any, ApplicationConfigurationDto>(
      {
        method: 'GET',
        url: '/api/abp/application-configuration',
      },
      { apiName: this.apiName },
    );
  }

  constructor(private restService: RestService) {}
}
