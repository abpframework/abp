import { Injectable } from '@angular/core';
import { RestService } from '../../../../services/rest.service';
import type { FindTenantResultDto } from '../../../volo/abp/asp-net-core/mvc/multi-tenancy/models';

@Injectable({
  providedIn: 'root',
})
export class AbpTenantService {
  apiName = 'abp';

  findTenantById = (id: string) =>
    this.restService.request<any, FindTenantResultDto>(
      {
        method: 'GET',
        url: `/api/abp/multi-tenancy/tenants/by-id/${id}`,
      },
      { apiName: this.apiName },
    );

  findTenantByName = (name: string, headers: Record<string, string>) =>
    this.restService.request<any, FindTenantResultDto>(
      {
        method: 'GET',
        url: `/api/abp/multi-tenancy/tenants/by-name/${name}`,
        headers,
      },
      { apiName: this.apiName },
    );

  constructor(private restService: RestService) {}
}
