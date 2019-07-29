import { ABP } from '@abp/ng.core';

export namespace Identity {
  export interface State {
    roles: RoleResponse;
    users: UserResponse;
    selectedRole: RoleItem;
    selectedUser: UserItem;
    selectedUserRoles: RoleItem[];
  }

  export type RoleResponse = ABP.PagedResponse<RoleItem>;

  export interface RoleSaveRequest {
    name: string;
    isDefault: boolean;
    isPublic: boolean;
  }

  export interface RoleItem extends RoleSaveRequest {
    isStatic: boolean;
    concurrencyStamp: string;
    id: string;
  }

  export type UserResponse = ABP.PagedResponse<UserItem>;

  export interface UserItem extends User {
    tenantId: string;
    emailConfirmed: boolean;
    phoneNumberConfirmed: boolean;
    isLockedOut: boolean;
    concurrencyStamp: string;
    id: string;
  }

  export interface User {
    userName: string;
    name: string;
    surname: string;
    email: string;
    phoneNumber: string;
    twoFactorEnabled: true;
    lockoutEnabled: true;
  }

  export interface UserSaveRequest extends User {
    password: string;
    roleNames: string[];
  }
}
