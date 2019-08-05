import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RestService, Rest } from '@abp/ng.core';
import { RegisterResponse, RegisterRequest, Tenant } from '../models';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(private rest: RestService) {}

  findTenant(tenantName: string): Observable<Tenant> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: `/api/abp/multi-tenancy/find-tenant/${tenantName}`,
    };

    return this.rest.request<null, Tenant>(request);
  }

  register(body: RegisterRequest): Observable<RegisterResponse> {
    const request: Rest.Request<RegisterRequest> = {
      method: 'POST',
      url: `/api/account/register`,
      body,
    };

    return this.rest.request<RegisterRequest, RegisterResponse>(request, { throwErr: true });
  }
}
