
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

export interface MultiTenancyInfoDto {
  isEnabled: boolean;
}
