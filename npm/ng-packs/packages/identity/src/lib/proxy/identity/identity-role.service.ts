import type { ListResultDto, PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { RestService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { IdentityRoleCreateDto, IdentityRoleDto, IdentityRoleUpdateDto } from './models';

@Injectable({
  providedIn: 'root',
})
export class IdentityRoleService {
  apiName = 'AbpIdentity';

  create = (input: IdentityRoleCreateDto) =>
    this.restService.request<any, IdentityRoleDto>(
      {
        method: 'POST',
        url: '/api/identity/roles',
        body: input,
      },
      { apiName: this.apiName },
    );

  delete = (id: string) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/identity/roles/${id}`,
      },
      { apiName: this.apiName },
    );

  get = (id: string) =>
    this.restService.request<any, IdentityRoleDto>(
      {
        method: 'GET',
        url: `/api/identity/roles/${id}`,
      },
      { apiName: this.apiName },
    );

  getAllList = () =>
    this.restService.request<any, ListResultDto<IdentityRoleDto>>(
      {
        method: 'GET',
        url: '/api/identity/roles/all',
      },
      { apiName: this.apiName },
    );

  getList = (input: PagedAndSortedResultRequestDto) =>
    this.restService.request<any, PagedResultDto<IdentityRoleDto>>(
      {
        method: 'GET',
        url: '/api/identity/roles',
        params: {
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName },
    );

  update = (id: string, input: IdentityRoleUpdateDto) =>
    this.restService.request<any, IdentityRoleDto>(
      {
        method: 'PUT',
        url: `/api/identity/roles/${id}`,
        body: input,
      },
      { apiName: this.apiName },
    );

  constructor(private restService: RestService) {}
}
