import { Identity } from '../models/identity';
import { ABP } from '@abp/ng.core';
export declare class GetRoles {
    payload?: ABP.PageQueryParams;
    static readonly type = "[Identity] Get Roles";
    constructor(payload?: ABP.PageQueryParams);
}
export declare class GetRoleById {
    payload: string;
    static readonly type = "[Identity] Get Role By Id";
    constructor(payload: string);
}
export declare class DeleteRole {
    payload: string;
    static readonly type = "[Identity] Delete Role";
    constructor(payload: string);
}
export declare class CreateRole {
    payload: Identity.RoleSaveRequest;
    static readonly type = "[Identity] Create Role";
    constructor(payload: Identity.RoleSaveRequest);
}
export declare class UpdateRole {
    payload: Identity.RoleItem;
    static readonly type = "[Identity] Update Role";
    constructor(payload: Identity.RoleItem);
}
export declare class GetUsers {
    payload?: ABP.PageQueryParams;
    static readonly type = "[Identity] Get Users";
    constructor(payload?: ABP.PageQueryParams);
}
export declare class GetUserById {
    payload: string;
    static readonly type = "[Identity] Get User By Id";
    constructor(payload: string);
}
export declare class DeleteUser {
    payload: string;
    static readonly type = "[Identity] Delete User";
    constructor(payload: string);
}
export declare class CreateUser {
    payload: Identity.UserSaveRequest;
    static readonly type = "[Identity] Create User";
    constructor(payload: Identity.UserSaveRequest);
}
export declare class UpdateUser {
    payload: Identity.UserSaveRequest & {
        id: string;
    };
    static readonly type = "[Identity] Update User";
    constructor(payload: Identity.UserSaveRequest & {
        id: string;
    });
}
export declare class GetUserRoles {
    payload: string;
    static readonly type = "[Identity] Get User Roles";
    constructor(payload: string);
}
