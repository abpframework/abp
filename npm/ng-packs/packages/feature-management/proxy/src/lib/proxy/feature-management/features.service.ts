import type { GetFeatureListResultDto, UpdateFeaturesDto } from './models';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class FeaturesService {
  apiName = 'AbpFeatureManagement';

  get = (providerName: string, providerKey: string) =>
    this.restService.request<any, GetFeatureListResultDto>({
      method: 'GET',
      url: '/api/feature-management/features',
      params: { providerName, providerKey },
    },
    { apiName: this.apiName });

  update = (providerName: string, providerKey: string, input: UpdateFeaturesDto) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: '/api/feature-management/features',
      params: { providerName, providerKey },
      body: input,
    },
    { apiName: this.apiName });

  constructor(private restService: RestService) {}
}
