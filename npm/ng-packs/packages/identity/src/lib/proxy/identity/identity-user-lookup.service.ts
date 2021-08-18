import type { UserLookupCountInputDto, UserLookupSearchInputDto } from './models';
import { RestService } from '@abp/ng.core';
import type { ListResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { UserData } from '../users/models';

@Injectable({
  providedIn: 'root',
})
export class IdentityUserLookupService {
  apiName = 'AbpIdentity';

  findById = (id: string) =>
    this.restService.request<any, UserData>(
      {
        method: 'GET',
        url: `/api/identity/users/lookup/${id}`,
      },
      { apiName: this.apiName },
    );

  findByUserName = (userName: string) =>
    this.restService.request<any, UserData>(
      {
        method: 'GET',
        url: `/api/identity/users/lookup/by-username/${userName}`,
      },
      { apiName: this.apiName },
    );

  getCount = (input: UserLookupCountInputDto) =>
    this.restService.request<any, number>(
      {
        method: 'GET',
        url: '/api/identity/users/lookup/count',
        params: { filter: input.filter },
      },
      { apiName: this.apiName },
    );

  search = (input: UserLookupSearchInputDto) =>
    this.restService.request<any, ListResultDto<UserData>>(
      {
        method: 'GET',
        url: '/api/identity/users/lookup/search',
        params: {
          filter: input.filter,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName },
    );

  constructor(private restService: RestService) {}
}
