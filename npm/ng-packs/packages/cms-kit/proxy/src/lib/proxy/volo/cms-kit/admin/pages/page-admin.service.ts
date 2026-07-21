import type { CreatePageInputDto, GetPagesInputDto, PageDto, UpdatePageInputDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PageAdminService {
  private restService = inject(RestService);
  apiName = 'CmsKitAdmin';
  

  create = (input: CreatePageInputDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageDto>({
      method: 'POST',
      url: '/api/cms-kit-admin/pages',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/cms-kit-admin/pages/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageDto>({
      method: 'GET',
      url: `/api/cms-kit-admin/pages/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetPagesInputDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PageDto>>({
      method: 'GET',
      url: '/api/cms-kit-admin/pages',
      params: { filter: input.filter, status: input.status, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  setAsHomePage = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/cms-kit-admin/pages/setashomepage/${id}`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdatePageInputDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PageDto>({
      method: 'PUT',
      url: `/api/cms-kit-admin/pages/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
}