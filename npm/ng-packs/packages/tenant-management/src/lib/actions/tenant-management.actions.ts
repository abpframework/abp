import { TenantManagement } from '../models/tenant-management';
import { ABP } from '@abp/ng.core';

export class TenantManagementGet {
  static readonly type = '[TenantManagement] Get';
  constructor(public payload?: ABP.PageQueryParams) {}
}

export class TenantManagementGetById {
  static readonly type = '[TenantManagement] Get By Id';
  constructor(public payload: string) {}
}

export class TenantManagementAdd {
  static readonly type = '[TenantManagement] Add';
  constructor(public payload: TenantManagement.AddRequest) {}
}

export class TenantManagementUpdate {
  static readonly type = '[TenantManagement] Update';
  constructor(public payload: TenantManagement.UpdateRequest) {}
}

export class TenantManagementDelete {
  static readonly type = '[TenantManagement] Delete';
  constructor(public payload: string) {}
}
