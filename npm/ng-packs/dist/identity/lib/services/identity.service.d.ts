import { Observable } from 'rxjs';
import { RestService, ABP } from '@abp/ng.core';
import { Identity } from '../models/identity';
export declare class IdentityService {
    private rest;
    constructor(rest: RestService);
    getRoles(params?: ABP.PageQueryParams): Observable<Identity.RoleResponse>;
    getRoleById(id: string): Observable<Identity.RoleItem>;
    deleteRole(id: string): Observable<Identity.RoleItem>;
    createRole(body: Identity.RoleSaveRequest): Observable<Identity.RoleItem>;
    updateRole(body: Identity.RoleItem): Observable<Identity.RoleItem>;
    getUsers(params?: ABP.PageQueryParams): Observable<Identity.UserResponse>;
    getUserById(id: string): Observable<Identity.UserItem>;
    getUserRoles(id: string): Observable<Identity.RoleResponse>;
    deleteUser(id: string): Observable<null>;
    createUser(body: Identity.UserSaveRequest): Observable<Identity.UserItem>;
    updateUser(body: Identity.UserItem): Observable<Identity.UserItem>;
}
