import { ABP } from '@abp/ng.core';
export declare namespace Identity {
    interface State {
        roles: RoleResponse;
        users: UserResponse;
        selectedRole: RoleItem;
        selectedUser: UserItem;
        selectedUserRoles: RoleItem[];
    }
    type RoleResponse = ABP.PagedResponse<RoleItem>;
    interface RoleSaveRequest {
        name: string;
        isDefault: boolean;
        isPublic: boolean;
    }
    interface RoleItem extends RoleSaveRequest {
        isStatic: boolean;
        concurrencyStamp: string;
        id: string;
    }
    type UserResponse = ABP.PagedResponse<UserItem>;
    interface UserItem extends User {
        tenantId: string;
        emailConfirmed: boolean;
        phoneNumberConfirmed: boolean;
        isLockedOut: boolean;
        concurrencyStamp: string;
        id: string;
    }
    interface User {
        userName: string;
        name: string;
        surname: string;
        email: string;
        phoneNumber: string;
        twoFactorEnabled: true;
        lockoutEnabled: true;
    }
    interface UserSaveRequest extends User {
        password: string;
        roleNames: string[];
    }
}
