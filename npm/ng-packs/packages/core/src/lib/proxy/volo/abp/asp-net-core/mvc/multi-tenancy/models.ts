
export interface FindTenantResultDto {
  success: boolean;
  tenantId?: string;
  name?: string;
}

export interface CurrentTenantDto {
  id?: string;
  name?: string;
  isAvailable: boolean;
}

export interface MultiTenancyInfoDto {
  isEnabled: boolean;
}
