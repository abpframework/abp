import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RestService, Rest, ABP } from '@abp/ng.core';
import { TenantManagement } from '../models/tenant-management';

@Injectable({
  providedIn: 'root',
})
export class TenantManagementService {
  constructor(private rest: RestService) {}

  get(): Observable<TenantManagement.Response> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: '/api/multi-tenancy/tenant',
    };

    return this.rest.request<null, TenantManagement.Response>(request);
  }

  getById(id: string): Observable<ABP.BasicItem> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: `/api/multi-tenancy/tenant/${id}`,
    };

    return this.rest.request<null, ABP.BasicItem>(request);
  }

  delete(id: string): Observable<null> {
    const request: Rest.Request<null> = {
      method: 'DELETE',
      url: `/api/multi-tenancy/tenant/${id}`,
    };

    return this.rest.request<null, null>(request);
  }

  add(body: TenantManagement.AddRequest): Observable<ABP.BasicItem> {
    const request: Rest.Request<TenantManagement.AddRequest> = {
      method: 'POST',
      url: `/api/multi-tenancy/tenant`,
      body,
    };

    return this.rest.request<TenantManagement.AddRequest, ABP.BasicItem>(request);
  }

  update(body: TenantManagement.UpdateRequest): Observable<ABP.BasicItem> {
    const url = `/api/multi-tenancy/tenant/${body.id}`;
    delete body.id;

    const request: Rest.Request<TenantManagement.UpdateRequest> = {
      method: 'PUT',
      url,
      body,
    };

    return this.rest.request<TenantManagement.AddRequest, ABP.BasicItem>(request);
  }

  getDefaultConnectionString(id: string): Observable<string> {
    const url = `/api/multi-tenancy/tenant/${id}/defaultConnectionString`;

    const request: Rest.Request<TenantManagement.DefaultConnectionStringRequest> = {
      method: 'GET',
      responseType: Rest.ResponseType.Text,
      url,
    };
    return this.rest.request<TenantManagement.DefaultConnectionStringRequest, string>(request);
  }

  updateDefaultConnectionString(payload: TenantManagement.DefaultConnectionStringRequest): Observable<any> {
    const url = `/api/multi-tenancy/tenant/${payload.id}/defaultConnectionString`;

    const request: Rest.Request<TenantManagement.DefaultConnectionStringRequest> = {
      method: 'PUT',
      url,
      params: { defaultConnectionString: payload.defaultConnectionString },
    };
    return this.rest.request<TenantManagement.DefaultConnectionStringRequest, any>(request);
  }
}
