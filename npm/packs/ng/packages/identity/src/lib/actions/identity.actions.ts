import { Identity } from '../models/identity';
import { ABP } from '@abp/ng.core';

export class IdentityGetRoles {
  static readonly type = '[Identity] Get Roles';
}

export class IdentityGetRoleById {
  static readonly type = '[Identity] Get Role By Id';
  constructor(public payload: string) {}
}

export class IdentityDeleteRole {
  static readonly type = '[Identity] Delete Role';
  constructor(public payload: string) {}
}

export class IdentityAddRole {
  static readonly type = '[Identity] Add Role';
  constructor(public payload: Identity.RoleSaveRequest) {}
}

export class IdentityUpdateRole {
  static readonly type = '[Identity] Update Role';
  constructor(public payload: Identity.RoleItem) {}
}

export class IdentityGetUsers {
  static readonly type = '[Identity] Get Users';
  constructor(public payload?: ABP.PageQueryParams) {}
}

export class IdentityGetUserById {
  static readonly type = '[Identity] Get User By Id';
  constructor(public payload: string) {}
}

export class IdentityDeleteUser {
  static readonly type = '[Identity] Delete User';
  constructor(public payload: string) {}
}

export class IdentityAddUser {
  static readonly type = '[Identity] Add User';
  constructor(public payload: Identity.UserSaveRequest) {}
}

export class IdentityUpdateUser {
  static readonly type = '[Identity] Update User';
  constructor(public payload: Identity.UserSaveRequest & { id: string }) {}
}

export class IdentityGetUserRoles {
  static readonly type = '[Identity] Get User Roles';
  constructor(public payload: string) {}
}
