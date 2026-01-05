
export interface GetPermissionListResultDto {
  entityDisplayName?: string;
  groups: PermissionGroupDto[];
}

export interface GetResourcePermissionDefinitionListResultDto {
  permissions?: ResourcePermissionDefinitionDto[];
}

export interface GetResourcePermissionListResultDto {
  permissions?: ResourcePermissionGrantInfoDto[];
}

export interface GetResourcePermissionWithProviderListResultDto {
  permissions?: ResourcePermissionWithProdiverGrantInfoDto[];
}

export interface GetResourceProviderListResultDto {
  providers?: ResourceProviderDto[];
}

export interface GrantedResourcePermissionDto {
  name?: string;
  displayName?: string;
}

export interface PermissionGrantInfoDto {
  name?: string;
  displayName?: string;
  parentName?: string;
  isGranted: boolean;
  allowedProviders: string[];
  grantedProviders: ProviderInfoDto[];
}

export interface PermissionGroupDto {
  name?: string;
  displayName?: string;
  permissions: PermissionGrantInfoDto[];
  displayNameKey?: string;
  displayNameResource?: string;
}

export interface ProviderInfoDto {
  providerName?: string;
  providerKey?: string;
}

export interface ResourcePermissionDefinitionDto {
  name?: string;
  displayName?: string;
}

export interface ResourcePermissionGrantInfoDto {
  providerName?: string;
  providerKey?: string;
  providerDisplayName?: string;
  providerNameDisplayName?: string;
  permissions?: GrantedResourcePermissionDto[];
}

export interface ResourcePermissionWithProdiverGrantInfoDto {
  name?: string;
  displayName?: string;
  providers?: string[];
  isGranted?: boolean;
}

export interface ResourceProviderDto {
  name?: string;
  displayName?: string;
}

export interface SearchProviderKeyInfo {
  providerKey?: string;
  providerDisplayName?: string;
}

export interface SearchProviderKeyListResultDto {
  keys?: SearchProviderKeyInfo[];
}

export interface UpdatePermissionDto {
  name?: string;
  isGranted: boolean;
}

export interface UpdatePermissionsDto {
  permissions: UpdatePermissionDto[];
}

export interface UpdateResourcePermissionsDto {
  providerName?: string;
  providerKey?: string;
  permissions?: string[];
}
