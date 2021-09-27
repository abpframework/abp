import { IdentityUserDto } from '@abp/ng.identity/proxy';
import { EntityAction } from '@abp/ng.theme.shared/extensions';
import { UsersComponent } from '../components/users/users.component';

export const DEFAULT_USERS_ENTITY_ACTIONS = EntityAction.createMany<IdentityUserDto>([
  {
    text: 'AbpIdentity::Edit',
    action: data => {
      const component = data.getInjected(UsersComponent);
      component.edit(data.record.id);
    },
    permission: 'AbpIdentity.Users.Update',
  },
  {
    text: 'AbpIdentity::Permissions',
    action: data => {
      const component = data.getInjected(UsersComponent);
      component.openPermissionsModal(data.record.id);
    },
    permission: 'AbpIdentity.Users.ManagePermissions',
  },
  {
    text: 'AbpIdentity::Delete',
    action: data => {
      const component = data.getInjected(UsersComponent);
      component.delete(data.record.id, data.record.name || data.record.userName);
    },
    permission: 'AbpIdentity.Users.Delete',
  },
]);
