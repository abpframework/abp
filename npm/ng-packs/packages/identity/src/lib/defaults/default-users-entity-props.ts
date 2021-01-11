import { EntityProp, ePropType } from '@abp/ng.theme.shared/extensions';
import { IdentityUserDto } from '../proxy/identity/models';

export const DEFAULT_USERS_ENTITY_PROPS = EntityProp.createMany<IdentityUserDto>([
  {
    type: ePropType.String,
    name: 'userName',
    displayName: 'AbpIdentity::UserName',
    sortable: true,
    columnWidth: 250,
  },
  {
    type: ePropType.String,
    name: 'email',
    displayName: 'AbpIdentity::EmailAddress',
    sortable: true,
    columnWidth: 250,
  },
  {
    type: ePropType.String,
    name: 'phoneNumber',
    displayName: 'AbpIdentity::PhoneNumber',
    sortable: true,
    columnWidth: 250,
  },
]);
