import { Observable } from 'rxjs';
import { RestService, ABP } from '@abp/ng.core';
import { TenantManagement } from '../models/tenant-management';
export declare class TenantManagementService {
    private rest;
    constructor(rest: RestService);
    getTenant(params?: ABP.PageQueryParams): Observable<TenantManagement.Response>;
    getTenantById(id: string): Observable<ABP.BasicItem>;
    deleteTenant(id: string): Observable<null>;
    createTenant(body: TenantManagement.AddRequest): Observable<ABP.BasicItem>;
    updateTenant(body: TenantManagement.UpdateRequest): Observable<ABP.BasicItem>;
    getDefaultConnectionString(id: string): Observable<string>;
    updateDefaultConnectionString(payload: TenantManagement.DefaultConnectionStringRequest): Observable<any>;
    deleteDefaultConnectionString(id: string): Observable<string>;
}
