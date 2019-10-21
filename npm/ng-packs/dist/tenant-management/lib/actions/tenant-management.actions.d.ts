import { TenantManagement } from '../models/tenant-management';
import { ABP } from '@abp/ng.core';
export declare class GetTenants {
    payload?: ABP.PageQueryParams;
    static readonly type = "[TenantManagement] Get Tenant";
    constructor(payload?: ABP.PageQueryParams);
}
export declare class GetTenantById {
    payload: string;
    static readonly type = "[TenantManagement] Get Tenant By Id";
    constructor(payload: string);
}
export declare class CreateTenant {
    payload: TenantManagement.AddRequest;
    static readonly type = "[TenantManagement] Create Tenant";
    constructor(payload: TenantManagement.AddRequest);
}
export declare class UpdateTenant {
    payload: TenantManagement.UpdateRequest;
    static readonly type = "[TenantManagement] Update Tenant";
    constructor(payload: TenantManagement.UpdateRequest);
}
export declare class DeleteTenant {
    payload: string;
    static readonly type = "[TenantManagement] Delete Tenant";
    constructor(payload: string);
}
