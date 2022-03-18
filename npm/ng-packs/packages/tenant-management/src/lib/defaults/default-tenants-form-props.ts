import { TenantCreateDto, TenantUpdateDto } from '@abp/ng.tenant-management/proxy';
import { getPasswordValidators } from '@abp/ng.theme.shared';
import { ePropType, FormProp } from '@abp/ng.theme.shared/extensions';
import { Validators } from '@angular/forms';

export const DEFAULT_TENANTS_CREATE_FORM_PROPS = FormProp.createMany<
  TenantCreateDto | TenantUpdateDto
>([
  {
    type: ePropType.String,
    name: 'name',
    id: 'name',
    displayName: 'AbpTenantManagement::TenantName',
    validators: () => [Validators.required, Validators.maxLength(256)],
  },
  {
    type: ePropType.Email,
    name: 'adminEmailAddress',
    displayName: 'AbpTenantManagement::DisplayName:AdminEmailAddress',
    id: 'admin-email-address',
    validators: () => [Validators.required, Validators.maxLength(256), Validators.email],
  },
  {
    type: ePropType.Password,
    name: 'adminPassword',
    displayName: 'AbpTenantManagement::DisplayName:AdminPassword',
    id: 'admin-password',
    autocomplete: 'new-password',
    validators: data => [Validators.required, ...getPasswordValidators({ get: data.getInjected })],
  },
]);

export const DEFAULT_TENANTS_EDIT_FORM_PROPS = DEFAULT_TENANTS_CREATE_FORM_PROPS.slice(0, 1);
