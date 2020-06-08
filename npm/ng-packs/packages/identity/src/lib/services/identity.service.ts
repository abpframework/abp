import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RestService, Rest, ABP } from '@abp/ng.core';
import { Identity } from '../models/identity';

@Injectable({
  providedIn: 'root',
})
export class IdentityService {
  apiName = 'AbpIdentity';

  constructor(private rest: RestService) {}

  getRoles(params = {} as ABP.PageQueryParams): Observable<Identity.RoleResponse> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: '/api/identity/roles',
      params,
    };

    return this.rest.request<null, Identity.RoleResponse>(request, { apiName: this.apiName });
  }

  getAllRoles(): Observable<Identity.RoleResponse> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: '/api/identity/roles/all',
    };

    return this.rest.request<null, Identity.RoleResponse>(request, { apiName: this.apiName });
  }

  getRoleById(id: string): Observable<Identity.RoleItem> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: `/api/identity/roles/${id}`,
    };

    return this.rest.request<null, Identity.RoleItem>(request, { apiName: this.apiName });
  }

  deleteRole(id: string): Observable<Identity.RoleItem> {
    const request: Rest.Request<null> = {
      method: 'DELETE',
      url: `/api/identity/roles/${id}`,
    };

    return this.rest.request<null, Identity.RoleItem>(request, { apiName: this.apiName });
  }

  createRole(body: Identity.RoleSaveRequest): Observable<Identity.RoleItem> {
    const request: Rest.Request<Identity.RoleSaveRequest> = {
      method: 'POST',
      url: '/api/identity/roles',
      body,
    };

    return this.rest.request<Identity.RoleSaveRequest, Identity.RoleItem>(request, {
      apiName: this.apiName,
    });
  }

  updateRole(body: Identity.RoleItem): Observable<Identity.RoleItem> {
    const url = `/api/identity/roles/${body.id}`;
    delete body.id;

    const request: Rest.Request<Identity.RoleItem> = {
      method: 'PUT',
      url,
      body,
    };

    return this.rest.request<Identity.RoleItem, Identity.RoleItem>(request, {
      apiName: this.apiName,
    });
  }

  getUsers(params = {} as ABP.PageQueryParams): Observable<Identity.UserResponse> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: '/api/identity/users',
      params,
    };

    return this.rest.request<null, Identity.UserResponse>(request, { apiName: this.apiName });
  }

  getUserById(id: string): Observable<Identity.UserItem> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: `/api/identity/users/${id}`,
    };

    return this.rest.request<null, Identity.UserItem>(request, { apiName: this.apiName });
  }

  getUserRoles(id: string): Observable<Identity.RoleResponse> {
    const request: Rest.Request<null> = {
      method: 'GET',
      url: `/api/identity/users/${id}/roles`,
    };

    return this.rest.request<null, Identity.RoleResponse>(request, { apiName: this.apiName });
  }

  deleteUser(id: string): Observable<null> {
    const request: Rest.Request<null> = {
      method: 'DELETE',
      url: `/api/identity/users/${id}`,
    };

    return this.rest.request<null, null>(request, { apiName: this.apiName });
  }

  createUser(body: Identity.UserSaveRequest): Observable<Identity.UserItem> {
    const request: Rest.Request<Identity.UserSaveRequest> = {
      method: 'POST',
      url: '/api/identity/users',
      body,
    };

    return this.rest.request<Identity.UserSaveRequest, Identity.UserItem>(request, {
      apiName: this.apiName,
    });
  }

  updateUser(body: Identity.UserItem): Observable<Identity.UserItem> {
    const url = `/api/identity/users/${body.id}`;
    delete body.id;

    const request: Rest.Request<Identity.UserItem> = {
      method: 'PUT',
      url,
      body,
    };

    return this.rest.request<Identity.UserItem, Identity.UserItem>(request, {
      apiName: this.apiName,
    });
  }
}
