import type { GetTenantsInput, TenantCreateDto, TenantDto, TenantUpdateDto } from './models';
import { RestService } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class TenantService {
  apiName = 'AbpTenantManagement';

  create = (input: TenantCreateDto) =>
    this.restService.request<any, TenantDto>(
      {
        method: 'POST',
        url: '/api/multi-tenancy/tenants',
        body: input,
      },
      { apiName: this.apiName },
    );

  delete = (id: string) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/multi-tenancy/tenants/${id}`,
      },
      { apiName: this.apiName },
    );

  deleteDefaultConnectionString = (id: string) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/multi-tenancy/tenants/${id}/default-connection-string`,
      },
      { apiName: this.apiName },
    );

  get = (id: string) =>
    this.restService.request<any, TenantDto>(
      {
        method: 'GET',
        url: `/api/multi-tenancy/tenants/${id}`,
      },
      { apiName: this.apiName },
    );

  getDefaultConnectionString = (id: string) =>
    this.restService.request<any, string>(
      {
        method: 'GET',
        responseType: 'text',
        url: `/api/multi-tenancy/tenants/${id}/default-connection-string`,
      },
      { apiName: this.apiName },
    );

  getList = (input: GetTenantsInput) =>
    this.restService.request<any, PagedResultDto<TenantDto>>(
      {
        method: 'GET',
        url: '/api/multi-tenancy/tenants',
        params: {
          filter: input.filter,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName },
    );

  update = (id: string, input: TenantUpdateDto) =>
    this.restService.request<any, TenantDto>(
      {
        method: 'PUT',
        url: `/api/multi-tenancy/tenants/${id}`,
        body: input,
      },
      { apiName: this.apiName },
    );

  updateDefaultConnectionString = (id: string, defaultConnectionString: string) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/multi-tenancy/tenants/${id}/default-connection-string`,
        params: { defaultConnectionString },
      },
      { apiName: this.apiName },
    );

  constructor(private restService: RestService) {}
}
