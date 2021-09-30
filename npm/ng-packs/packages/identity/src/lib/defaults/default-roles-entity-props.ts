import { LocalizationService } from '@abp/ng.core';
import { IdentityRoleDto } from '@abp/ng.identity/proxy';
import { EntityProp, ePropType } from '@abp/ng.theme.shared/extensions';
import { of } from 'rxjs';

export const DEFAULT_ROLES_ENTITY_PROPS = EntityProp.createMany<IdentityRoleDto>([
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'AbpIdentity::RoleName',
    sortable: true,
    valueResolver: data => {
      const l10n = data.getInjected(LocalizationService);
      const t = l10n.instant.bind(l10n);
      const { isDefault, isPublic, name } = data.record;

      return of(
        name +
          (isDefault
            ? `<span class="badge badge-pill badge-success ml-1">${t(
                'AbpIdentity::DisplayName:IsDefault',
              )}</span>`
            : '') +
          (isPublic
            ? `<span class="badge badge-pill badge-info ml-1">${t(
                'AbpIdentity::DisplayName:IsPublic',
              )}</span>`
            : ''),
      );
    },
  },
]);
