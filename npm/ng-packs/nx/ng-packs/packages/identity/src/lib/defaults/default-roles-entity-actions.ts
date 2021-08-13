import { EntityAction } from '@abp/ng.theme.shared/extensions';
import { RolesComponent } from '../components/roles/roles.component';
import { IdentityRoleDto } from '../proxy/identity/models';

export const DEFAULT_ROLES_ENTITY_ACTIONS = EntityAction.createMany<IdentityRoleDto>([
  {
    text: 'AbpIdentity::Edit',
    action: data => {
      const component = data.getInjected(RolesComponent);
      component.edit(data.record.id);
    },
    permission: 'AbpIdentity.Roles.Update',
  },
  {
    text: 'AbpIdentity::Permissions',
    action: data => {
      const component = data.getInjected(RolesComponent);
      component.openPermissionsModal(data.record.name);
    },
    permission: 'AbpIdentity.Roles.ManagePermissions',
  },
  {
    text: 'AbpIdentity::Delete',
    action: data => {
      const component = data.getInjected(RolesComponent);
      component.delete(data.record.id, data.record.name);
    },
    permission: 'AbpIdentity.Roles.Delete',
    visible: data => !data.record.isStatic,
  },
]);
