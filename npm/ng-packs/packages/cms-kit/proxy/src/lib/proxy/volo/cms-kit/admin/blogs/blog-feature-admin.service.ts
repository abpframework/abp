import type { BlogFeatureInputDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { BlogFeatureDto } from '../../blogs/models';

@Injectable({
  providedIn: 'root',
})
export class BlogFeatureAdminService {
  private restService = inject(RestService);
  apiName = 'CmsKitAdmin';
  

  getList = (blogId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BlogFeatureDto[]>({
      method: 'GET',
      url: `/api/cms-kit-admin/blogs/${blogId}/features`,
    },
    { apiName: this.apiName,...config });
  

  set = (blogId: string, dto: BlogFeatureInputDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/cms-kit-admin/blogs/${blogId}/features`,
      body: dto,
    },
    { apiName: this.apiName,...config });
}