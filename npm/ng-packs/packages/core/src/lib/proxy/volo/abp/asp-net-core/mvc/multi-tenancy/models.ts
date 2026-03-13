import type { TenantUserSharingStrategy } from '../../../multi-tenancy/tenant-user-sharing-strategy.enum';

export interface FindTenantResultDto {
  success?: boolean;
  tenantId?: string | null;
  name?: string | null;
  normalizedName?: string | null;
  isActive?: boolean;
}

export interface CurrentTenantDto {
  id?: string | null;
  name?: string | null;
  isAvailable?: boolean;
}

export interface MultiTenancyInfoDto {
  isEnabled?: boolean;
  userSharingStrategy?: TenantUserSharingStrategy;
}
