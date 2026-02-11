import { RestService } from '../../../../services';
import { Rest } from '../../../../models';
import { Injectable, inject } from '@angular/core';
import type { FindTenantResultDto } from '../../../volo/abp/asp-net-core/mvc/multi-tenancy/models';

@Injectable({
  providedIn: 'root',
})
export class AbpTenantService {
  private restService = inject(RestService);

  apiName = 'abp';
  

  findTenantById = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FindTenantResultDto>({
      method: 'GET',
      url: `/api/abp/multi-tenancy/tenants/by-id/${id}`,
    },
    { apiName: this.apiName,...config });
  

  findTenantByName = (name: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FindTenantResultDto>({
      method: 'GET',
      url: `/api/abp/multi-tenancy/tenants/by-name/${name}`,
    },
    { apiName: this.apiName,...config });
}
