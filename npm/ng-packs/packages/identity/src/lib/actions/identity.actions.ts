import { Identity } from '../models/identity';
import { ABP } from '@abp/ng.core';

export class GetRoles {
  static readonly type = '[Identity] Get Roles';
  constructor(public payload?: ABP.PageQueryParams) {}
}

export class GetRoleById {
  static readonly type = '[Identity] Get Role By Id';
  constructor(public payload: string) {}
}

export class DeleteRole {
  static readonly type = '[Identity] Delete Role';
  constructor(public payload: string) {}
}

export class CreateRole {
  static readonly type = '[Identity] Create Role';
  constructor(public payload: Identity.RoleSaveRequest) {}
}

export class UpdateRole {
  static readonly type = '[Identity] Update Role';
  constructor(public payload: Identity.RoleItem) {}
}

export class GetUsers {
  static readonly type = '[Identity] Get Users';
  constructor(public payload?: ABP.PageQueryParams) {}
}

export class GetUserById {
  static readonly type = '[Identity] Get User By Id';
  constructor(public payload: string) {}
}

export class DeleteUser {
  static readonly type = '[Identity] Delete User';
  constructor(public payload: string) {}
}

export class CreateUser {
  static readonly type = '[Identity] Create User';
  constructor(public payload: Identity.UserSaveRequest) {}
}

export class UpdateUser {
  static readonly type = '[Identity] Update User';
  constructor(public payload: Identity.UserSaveRequest & { id: string }) {}
}

export class GetUserRoles {
  static readonly type = '[Identity] Get User Roles';
  constructor(public payload: string) {}
}
