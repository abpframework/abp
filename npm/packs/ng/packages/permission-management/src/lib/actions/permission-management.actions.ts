import { PermissionManagement } from '../models/permission-management';

export class PermissionManagementGetPermissions {
  static readonly type = '[PermissionManagement] Get Permissions';
  constructor(public payload: PermissionManagement.GrantedProvider) {}
}

export class PermissionManagementUpdatePermissions {
  static readonly type = '[PermissionManagement] Update Permissions';
  constructor(public payload: PermissionManagement.GrantedProvider & PermissionManagement.UpdateRequest) {}
}
