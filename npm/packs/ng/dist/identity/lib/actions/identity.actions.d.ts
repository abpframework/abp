import { Identity } from '../models/identity';
import { ABP } from '@abp/ng.core';
export declare class IdentityGetRoles {
    static readonly type = "[Identity] Get Roles";
}
export declare class IdentityGetRoleById {
    payload: string;
    static readonly type = "[Identity] Get Role By Id";
    constructor(payload: string);
}
export declare class IdentityDeleteRole {
    payload: string;
    static readonly type = "[Identity] Delete Role";
    constructor(payload: string);
}
export declare class IdentityAddRole {
    payload: Identity.RoleSaveRequest;
    static readonly type = "[Identity] Add Role";
    constructor(payload: Identity.RoleSaveRequest);
}
export declare class IdentityUpdateRole {
    payload: Identity.RoleItem;
    static readonly type = "[Identity] Update Role";
    constructor(payload: Identity.RoleItem);
}
export declare class IdentityGetUsers {
    payload?: ABP.PageQueryParams;
    static readonly type = "[Identity] Get Users";
    constructor(payload?: ABP.PageQueryParams);
}
export declare class IdentityGetUserById {
    payload: string;
    static readonly type = "[Identity] Get User By Id";
    constructor(payload: string);
}
export declare class IdentityDeleteUser {
    payload: string;
    static readonly type = "[Identity] Delete User";
    constructor(payload: string);
}
export declare class IdentityAddUser {
    payload: Identity.UserSaveRequest;
    static readonly type = "[Identity] Add User";
    constructor(payload: Identity.UserSaveRequest);
}
export declare class IdentityUpdateUser {
    payload: Identity.UserSaveRequest & {
        id: string;
    };
    static readonly type = "[Identity] Update User";
    constructor(payload: Identity.UserSaveRequest & {
        id: string;
    });
}
export declare class IdentityGetUserRoles {
    payload: string;
    static readonly type = "[Identity] Get User Roles";
    constructor(payload: string);
}
