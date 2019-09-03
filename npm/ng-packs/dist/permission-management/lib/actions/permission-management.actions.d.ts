import { PermissionManagement } from '../models/permission-management';
export declare class GetPermissions {
    payload: PermissionManagement.GrantedProvider;
    static readonly type = "[PermissionManagement] Get Permissions";
    constructor(payload: PermissionManagement.GrantedProvider);
}
export declare class UpdatePermissions {
    payload: PermissionManagement.GrantedProvider & PermissionManagement.UpdateRequest;
    static readonly type = "[PermissionManagement] Update Permissions";
    constructor(payload: PermissionManagement.GrantedProvider & PermissionManagement.UpdateRequest);
}
