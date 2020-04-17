import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RestService, Rest, ABP } from '@abp/ng.core';
import { TenantManagement } from '../models/tenant-management';

@Injectable({
  providedIn: 'root',
})
export class TenantManagementService {
  apiName = 'AbpTenantManagement';

  constructor(private rest: RestService) {}

  getTenant(params = {} as ABP.PageQueryParams): Observable<TenantManagement.Response> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: '/api/multi-tenancy/tenants',
      params,
    };

    return this.rest.request<null, TenantManagement.Response>(request, { apiName: this.apiName });
  }

  getTenantById(id: string): Observable<ABP.BasicItem> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: `/api/multi-tenancy/tenants/${id}`,
    };

    return this.rest.request<null, ABP.BasicItem>(request, { apiName: this.apiName });
  }

  deleteTenant(id: string): Observable<null> {
    const request: Rest.Request<null> = {
      method: 'DELETE',
      url: `/api/multi-tenancy/tenants/${id}`,
    };

    return this.rest.request<null, null>(request, { apiName: this.apiName });
  }

  createTenant(body: TenantManagement.AddRequest): Observable<ABP.BasicItem> {
    const request: Rest.Request<TenantManagement.AddRequest> = {
      method: 'POST',
      url: '/api/multi-tenancy/tenants',
      body,
    };

    return this.rest.request<TenantManagement.AddRequest, ABP.BasicItem>(request, {
      apiName: this.apiName,
    });
  }

  updateTenant(body: TenantManagement.UpdateRequest): Observable<ABP.BasicItem> {
    const url = `/api/multi-tenancy/tenants/${body.id}`;
    delete body.id;

    const request: Rest.Request<TenantManagement.UpdateRequest> = {
      method: 'PUT',
      url,
      body,
    };

    return this.rest.request<TenantManagement.UpdateRequest, ABP.BasicItem>(request, {
      apiName: this.apiName,
    });
  }

  getDefaultConnectionString(id: string): Observable<string> {
    const url = `/api/multi-tenancy/tenants/${id}/default-connection-string`;

    const request: Rest.Request<TenantManagement.DefaultConnectionStringRequest> = {
      method: 'GET',
      responseType: Rest.ResponseType.Text,
      url,
    };
    return this.rest.request<TenantManagement.DefaultConnectionStringRequest, string>(request, {
      apiName: this.apiName,
    });
  }

  updateDefaultConnectionString(
    payload: TenantManagement.DefaultConnectionStringRequest,
  ): Observable<any> {
    const url = `/api/multi-tenancy/tenants/${payload.id}/default-connection-string`;

    const request: Rest.Request<TenantManagement.DefaultConnectionStringRequest> = {
      method: 'PUT',
      url,
      params: { defaultConnectionString: payload.defaultConnectionString },
    };
    return this.rest.request<TenantManagement.DefaultConnectionStringRequest, any>(request, {
      apiName: this.apiName,
    });
  }

  deleteDefaultConnectionString(id: string): Observable<string> {
    const url = `/api/multi-tenancy/tenants/${id}/default-connection-string`;

    const request: Rest.Request<TenantManagement.DefaultConnectionStringRequest> = {
      method: 'DELETE',
      url,
    };
    return this.rest.request<TenantManagement.DefaultConnectionStringRequest, any>(request, {
      apiName: this.apiName,
    });
  }
}
