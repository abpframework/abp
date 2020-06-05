import { TenantManagement } from '../models/tenant-management';
import { ABP } from '@abp/ng.core';

export class GetTenants {
  static readonly type = '[TenantManagement] Get Tenant';
  constructor(public payload?: ABP.PageQueryParams) {}
}

export class GetTenantById {
  static readonly type = '[TenantManagement] Get Tenant By Id';
  constructor(public payload: string) {}
}

export class CreateTenant {
  static readonly type = '[TenantManagement] Create Tenant';
  constructor(public payload: TenantManagement.AddRequest) {}
}

export class UpdateTenant {
  static readonly type = '[TenantManagement] Update Tenant';
  constructor(public payload: TenantManagement.UpdateRequest) {}
}

export class DeleteTenant {
  static readonly type = '[TenantManagement] Delete Tenant';
  constructor(public payload: string) {}
}
