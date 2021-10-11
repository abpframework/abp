import { TenantDto } from '@abp/ng.tenant-management/proxy';
import { ToolbarAction } from '@abp/ng.theme.shared/extensions';
import { TenantsComponent } from '../components/tenants/tenants.component';

export const DEFAULT_TENANTS_TOOLBAR_ACTIONS = ToolbarAction.createMany<TenantDto[]>([
  {
    text: 'AbpTenantManagement::ManageHostFeatures',
    action: data => {
      const component = data.getInjected(TenantsComponent);
      component.openFeaturesModal(null);
    },
    permission: 'FeatureManagement.ManageHostFeatures',
    icon: 'fa fa-cog',
  },
  {
    text: 'AbpTenantManagement::NewTenant',
    action: data => {
      const component = data.getInjected(TenantsComponent);
      component.addTenant();
    },
    permission: 'AbpTenantManagement.Tenants.Create',
    icon: 'fa fa-plus',
  },
]);
