import { Injectable } from '@angular/core';
import { RestService, Rest } from '@abp/ng.core';
import { Observable } from 'rxjs';
import { PermissionManagement } from '../models/permission-management';

@Injectable({
  providedIn: 'root',
})
export class PermissionManagementService {
  constructor(private rest: RestService) {}

  getPermissions(params: PermissionManagement.GrantedProvider): Observable<PermissionManagement.Response> {
    const request: Rest.Request<PermissionManagement.GrantedProvider> = {
      method: 'GET',
      url: '/api/abp/permissions',
      params,
    };

    return this.rest.request<PermissionManagement.GrantedProvider, PermissionManagement.Response>(request);
  }

  updatePermissions({
    permissions,
    providerKey,
    providerName,
  }: PermissionManagement.GrantedProvider & PermissionManagement.UpdateRequest): Observable<null> {
    const request: Rest.Request<PermissionManagement.UpdateRequest> = {
      method: 'PUT',
      url: '/api/abp/permissions',
      body: { permissions },
      params: { providerKey, providerName },
    };

    return this.rest.request<PermissionManagement.UpdateRequest, null>(request);
  }
}
