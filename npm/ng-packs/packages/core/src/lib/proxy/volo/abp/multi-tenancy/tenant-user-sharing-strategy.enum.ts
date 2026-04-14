import { mapEnumToOptions } from '../../../../utils/form-utils';

export enum TenantUserSharingStrategy {
  Isolated = 0,
  Shared = 1,
}

export const tenantUserSharingStrategyOptions = mapEnumToOptions(TenantUserSharingStrategy);
