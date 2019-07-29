import { Observable } from 'rxjs';
import { RestService, ABP } from '@abp/ng.core';
import { TenantManagement } from '../models/tenant-management';
export declare class TenantManagementService {
    private rest;
    constructor(rest: RestService);
    get(): Observable<TenantManagement.Response>;
    getById(id: string): Observable<ABP.BasicItem>;
    delete(id: string): Observable<null>;
    add(body: TenantManagement.AddRequest): Observable<ABP.BasicItem>;
    update(body: TenantManagement.UpdateRequest): Observable<ABP.BasicItem>;
    getDefaultConnectionString(id: string): Observable<string>;
    updateDefaultConnectionString(payload: TenantManagement.DefaultConnectionStringRequest): Observable<any>;
}
