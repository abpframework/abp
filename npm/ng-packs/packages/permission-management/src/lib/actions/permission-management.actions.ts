import { PermissionManagement } from '../models/permission-management';

export class GetPermissions {
  static readonly type = '[PermissionManagement] Get Permissions';
  constructor(public payload: PermissionManagement.GrantedProvider) {}
}

export class UpdatePermissions {
  static readonly type = '[PermissionManagement] Update Permissions';
  constructor(public payload: PermissionManagement.GrantedProvider & PermissionManagement.UpdateRequest) {}
}
