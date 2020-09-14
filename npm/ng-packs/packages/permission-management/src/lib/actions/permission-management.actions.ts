import { PermissionManagement } from '../models/permission-management';
import { UpdatePermissionsDto } from '../proxy/permission-management/models';

export class GetPermissions {
  static readonly type = '[PermissionManagement] Get Permissions';
  constructor(public payload: PermissionManagement.GrantedProvider) {}
}

export class UpdatePermissions {
  static readonly type = '[PermissionManagement] Update Permissions';
  constructor(public payload: PermissionManagement.GrantedProvider & UpdatePermissionsDto) {}
}
