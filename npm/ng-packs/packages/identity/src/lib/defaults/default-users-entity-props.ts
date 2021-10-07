import { LocalizationService } from '@abp/ng.core';
import { IdentityUserDto } from '@abp/ng.identity/proxy';
import { EntityProp, ePropType } from '@abp/ng.theme.shared/extensions';
import { of } from 'rxjs';

export const DEFAULT_USERS_ENTITY_PROPS = EntityProp.createMany<IdentityUserDto>([
  {
    type: ePropType.String,
    name: 'userName',
    displayName: 'AbpIdentity::UserName',
    sortable: true,
    columnWidth: 250,
    valueResolver: data => {
      const l10n = data.getInjected(LocalizationService);
      const t = l10n.instant.bind(l10n);

      const inactiveIcon = `<i title="${t(
        'AbpIdentity::ThisUserIsNotActiveMessage',
      )}" class="fas fa-ban text-danger mr-1"></i>`;

      return of(
        `
        ${!data.record.isActive ? inactiveIcon : ''}
        <span class="${!data.record.isActive ? 'text-muted' : ''}">${data.record.userName}</span>`,
      );
    },
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
