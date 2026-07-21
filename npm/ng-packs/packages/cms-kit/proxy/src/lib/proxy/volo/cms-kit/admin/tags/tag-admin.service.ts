import type { TagCreateDto, TagDefinitionDto, TagGetListInput, TagUpdateDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';
import type { TagDto } from '../../tags/models';

@Injectable({
  providedIn: 'root',
})
export class TagAdminService {
  private restService = inject(RestService);
  apiName = 'CmsKitAdmin';
  

  create = (input: TagCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TagDto>({
      method: 'POST',
      url: '/api/cms-kit-admin/tags',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/cms-kit-admin/tags/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TagDto>({
      method: 'GET',
      url: `/api/cms-kit-admin/tags/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: TagGetListInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<TagDto>>({
      method: 'GET',
      url: '/api/cms-kit-admin/tags',
      params: { filter: input.filter, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getTagDefinitions = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, TagDefinitionDto[]>({
      method: 'GET',
      url: '/api/cms-kit-admin/tags/tag-definitions',
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: TagUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, TagDto>({
      method: 'PUT',
      url: `/api/cms-kit-admin/tags/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
}