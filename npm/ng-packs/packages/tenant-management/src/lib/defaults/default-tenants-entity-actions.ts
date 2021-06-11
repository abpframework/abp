import { EntityAction } from '@abp/ng.theme.shared/extensions';
import { TenantsComponent } from '../components/tenants/tenants.component';
import { TenantDto } from '../proxy/models';

export const DEFAULT_TENANTS_ENTITY_ACTIONS = EntityAction.createMany<TenantDto>([
  {
    text: 'AbpTenantManagement::Edit',
    action: data => {
      const component = data.getInjected(TenantsComponent);
      component.editTenant(data.record.id);
    },
    permission: 'AbpTenantManagement.Tenants.Update',
  },
  {
    text: 'AbpTenantManagement::Permission:ManageFeatures',
    action: data => {
      const component = data.getInjected(TenantsComponent);
      component.openFeaturesModal(data.record.id);
    },
    permission: 'AbpTenantManagement.Tenants.ManageFeatures',
  },
  {
    text: 'AbpTenantManagement::Delete',
    action: data => {
      const component = data.getInjected(TenantsComponent);
      component.delete(data.record.id, data.record.name);
    },
    permission: 'AbpTenantManagement.Tenants.Delete',
  },
]);
