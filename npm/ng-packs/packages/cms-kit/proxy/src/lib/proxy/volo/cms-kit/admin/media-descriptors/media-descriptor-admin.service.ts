import type { CreateMediaInputWithStream, MediaDescriptorDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MediaDescriptorAdminService {
  private restService = inject(RestService);
  apiName = 'CmsKitAdmin';
  

  create = (entityType: string, inputStream: CreateMediaInputWithStream, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MediaDescriptorDto>({
      method: 'POST',
      url: `/api/cms-kit-admin/media/${entityType}`,
      params: { name: inputStream.name },
      body: inputStream.file,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/cms-kit-admin/media/${id}`,
    },
    { apiName: this.apiName,...config });
}