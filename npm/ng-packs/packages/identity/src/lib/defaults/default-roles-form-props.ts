import { IdentityRoleDto } from '@abp/ng.identity/proxy';
import { ePropType, FormProp, PropData, PropPredicate } from "@abp/ng.components/extensible";
import { Validators } from '@angular/forms';

export const DEFAULT_ROLES_CREATE_FORM_PROPS = FormProp.createMany<IdentityRoleDto>([
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'AbpIdentity::RoleName',
    id: 'role-name',
    disabled: ((data: PropData<IdentityRoleDto>) => data.record && data.record.isStatic) as PropPredicate<IdentityRoleDto>,
    validators: () => [Validators.required],
  },
  {
    type: ePropType.Boolean,
    name: 'isDefault',
    displayName: 'AbpIdentity::DisplayName:IsDefault',
    id: 'role-is-default',
    defaultValue: false,
  },
  {
    type: ePropType.Boolean,
    name: 'isPublic',
    displayName: 'AbpIdentity::DisplayName:IsPublic',
    id: 'role-is-public',
    defaultValue: false,
  },
]);

export const DEFAULT_ROLES_EDIT_FORM_PROPS = DEFAULT_ROLES_CREATE_FORM_PROPS;
