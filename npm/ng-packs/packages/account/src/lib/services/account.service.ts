import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RestService, Rest } from '@abp/ng.core';
import { RegisterResponse, RegisterRequest, TenantIdResponse } from '../models';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(private rest: RestService) {}

  findTenant(tenantName: string): Observable<TenantIdResponse> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: `/api/abp/multi-tenancy/tenants/by-name/${tenantName}`,
    };

    return this.rest.request<null, TenantIdResponse>(request);
  }

  register(body: RegisterRequest): Observable<RegisterResponse> {
    const request: Rest.Request<RegisterRequest> = {
      method: 'POST',
      url: '/api/account/register',
      body,
    };

    return this.rest.request<RegisterRequest, RegisterResponse>(request, { skipHandleError: true });
  }
}
