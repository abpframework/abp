import { TenantManagement } from '../models/tenant-management';
export declare class TenantManagementGet {
    static readonly type = "[TenantManagement] Get";
}
export declare class TenantManagementGetById {
    payload: string;
    static readonly type = "[TenantManagement] Get By Id";
    constructor(payload: string);
}
export declare class TenantManagementAdd {
    payload: TenantManagement.AddRequest;
    static readonly type = "[TenantManagement] Add";
    constructor(payload: TenantManagement.AddRequest);
}
export declare class TenantManagementUpdate {
    payload: TenantManagement.UpdateRequest;
    static readonly type = "[TenantManagement] Update";
    constructor(payload: TenantManagement.UpdateRequest);
}
export declare class TenantManagementDelete {
    payload: string;
    static readonly type = "[TenantManagement] Delete";
    constructor(payload: string);
}
