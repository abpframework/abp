import type { GlobalResourcesDto, GlobalResourcesUpdateDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class GlobalResourceAdminService {
  private restService = inject(RestService);
  apiName = 'CmsKitAdmin';
  

  get = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, GlobalResourcesDto>({
      method: 'GET',
      url: '/api/cms-kit-admin/global-resources',
    },
    { apiName: this.apiName,...config });
  

  setGlobalResources = (input: GlobalResourcesUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/cms-kit-admin/global-resources',
      body: input,
    },
    { apiName: this.apiName,...config });
}