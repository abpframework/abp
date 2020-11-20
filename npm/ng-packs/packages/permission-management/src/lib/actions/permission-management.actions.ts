import { ProviderInfoDto, UpdatePermissionsDto } from '../proxy/models';

export class GetPermissions {
  static readonly type = '[PermissionManagement] Get Permissions';
  constructor(public payload: ProviderInfoDto) {}
}

export class UpdatePermissions {
  static readonly type = '[PermissionManagement] Update Permissions';
  constructor(public payload: ProviderInfoDto & UpdatePermissionsDto) {}
}
