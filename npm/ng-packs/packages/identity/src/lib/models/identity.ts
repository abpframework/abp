import { ABP, PagedResultDto } from '@abp/ng.core';
import { IdentityRoleDto, IdentityUserDto } from '../proxy/identity/models';

export namespace Identity {
  export interface State {
    roles: PagedResultDto<IdentityRoleDto>;
    users: PagedResultDto<IdentityUserDto>;
    selectedRole: IdentityRoleDto;
    selectedUser: IdentityUserDto;
    selectedUserRoles: IdentityRoleDto[];
  }

  /**
   * @deprecated To be deleted in v4.0.
   */
  export type RoleResponse = ABP.PagedResponse<RoleItem>;

  /**
   * @deprecated To be deleted in v4.0.
   */
  export interface RoleSaveRequest {
    name: string;
    isDefault: boolean;
    isPublic: boolean;
  }

  /**
   * @deprecated To be deleted in v4.0.
   */
  export interface RoleItem extends RoleSaveRequest {
    isStatic: boolean;
    concurrencyStamp: string;
    id: string;
  }

  /**
   * @deprecated To be deleted in v4.0.
   */
  export type UserResponse = ABP.PagedResponse<UserItem>;

  /**
   * @deprecated To be deleted in v4.0.
   */
  export interface UserItem extends User {
    tenantId: string;
    emailConfirmed: boolean;
    phoneNumberConfirmed: boolean;
    isLockedOut: boolean;
    concurrencyStamp: string;
    id: string;
  }

  /**
   * @deprecated To be deleted in v4.0.
   */
  export interface User {
    userName: string;
    name: string;
    surname: string;
    email: string;
    phoneNumber: string;
    twoFactorEnabled: true;
    lockoutEnabled: true;
  }

  /**
   * @deprecated To be deleted in v4.0.
   */
  export interface UserSaveRequest extends User {
    password: string;
    roleNames: string[];
  }
}
