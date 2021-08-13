import { GetTenantsInput, TenantCreateDto, TenantUpdateDto } from '../proxy/models';

export class GetTenants {
  static readonly type = '[TenantManagement] Get Tenant';
  constructor(public payload?: GetTenantsInput) {}
}

export class GetTenantById {
  static readonly type = '[TenantManagement] Get Tenant By Id';
  constructor(public payload: string) {}
}

export class CreateTenant {
  static readonly type = '[TenantManagement] Create Tenant';
  constructor(public payload: TenantCreateDto) {}
}

export class UpdateTenant {
  static readonly type = '[TenantManagement] Update Tenant';
  constructor(public payload: TenantUpdateDto & { id: string }) {}
}

export class DeleteTenant {
  static readonly type = '[TenantManagement] Delete Tenant';
  constructor(public payload: string) {}
}
