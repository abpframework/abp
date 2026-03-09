
export interface FindTenantResultDto {
  success: boolean;
  tenantId?: string;
  name?: string;
  normalizedName?: string;
  isActive: boolean;
}

export interface CurrentTenantDto {
  id?: string;
  name?: string;
  isAvailable: boolean;
}

export enum TenantUserSharingStrategy {
  Isolated = 0,
  Shared = 1,
}

export interface MultiTenancyInfoDto {
  isEnabled: boolean;
  userSharingStrategy?: TenantUserSharingStrategy;
}
