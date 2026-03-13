import { mapEnumToOptions } from '@abp/ng.core';

export enum TenantUserSharingStrategy {
  Isolated = 0,
  Shared = 1,
}

export const tenantUserSharingStrategyOptions = mapEnumToOptions(TenantUserSharingStrategy);
