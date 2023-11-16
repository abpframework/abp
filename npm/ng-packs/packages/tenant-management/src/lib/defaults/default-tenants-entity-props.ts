import { TenantDto } from '@abp/ng.tenant-management/proxy';
import { EntityProp, ePropType } from '@abp/ng.components/extensible';

export const DEFAULT_TENANTS_ENTITY_PROPS = EntityProp.createMany<TenantDto>([
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'AbpTenantManagement::TenantName',
    sortable: true,
  },
]);
