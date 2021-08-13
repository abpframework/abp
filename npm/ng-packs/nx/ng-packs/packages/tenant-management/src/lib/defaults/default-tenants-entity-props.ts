import { EntityProp, ePropType } from '@abp/ng.theme.shared/extensions';
import { TenantDto } from '../proxy/models';

export const DEFAULT_TENANTS_ENTITY_PROPS = EntityProp.createMany<TenantDto>([
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'AbpTenantManagement::TenantName',
    sortable: true,
  },
]);
