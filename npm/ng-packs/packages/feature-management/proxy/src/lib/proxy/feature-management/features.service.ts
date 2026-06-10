import type { GetFeatureListResultDto, UpdateFeaturesDto } from './models';
import { RestService } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class FeaturesService {
  private restService = inject(RestService);

  apiName = 'AbpFeatureManagement';

  delete = (providerName: string, providerKey: string) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: '/api/feature-management/features',
        params: { providerName, providerKey },
      },
      { apiName: this.apiName },
    );

  get = (providerName: string, providerKey: string) =>
    this.restService.request<any, GetFeatureListResultDto>(
      {
        method: 'GET',
        url: '/api/feature-management/features',
        params: { providerName, providerKey },
      },
      { apiName: this.apiName },
    );

  update = (providerName: string, providerKey: string, input: UpdateFeaturesDto) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: '/api/feature-management/features',
        params: { providerName, providerKey },
        body: input,
      },
      { apiName: this.apiName },
    );
}
