import { IdentityUserDto } from '@abp/ng.identity/proxy';
import { getPasswordValidators } from '@abp/ng.theme.shared';
import { ePropType, FormProp } from '@abp/ng.theme.shared/extensions';
import { Validators } from '@angular/forms';

export const DEFAULT_USERS_CREATE_FORM_PROPS = FormProp.createMany<IdentityUserDto>([
  {
    type: ePropType.String,
    name: 'userName',
    displayName: 'AbpIdentity::UserName',
    id: 'user-name',
    validators: () => [Validators.required, Validators.maxLength(256)],
  },
  {
    type: ePropType.Password,
    name: 'password',
    displayName: 'AbpIdentity::Password',
    id: 'password',
    autocomplete: 'new-password',
    validators: data => [Validators.required, ...getPasswordValidators({ get: data.getInjected })],
  },
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'AbpIdentity::DisplayName:Name',
    id: 'name',
    validators: () => [Validators.maxLength(64)],
  },
  {
    type: ePropType.String,
    name: 'surname',
    displayName: 'AbpIdentity::DisplayName:Surname',
    id: 'surname',
    validators: () => [Validators.maxLength(64)],
  },
  {
    type: ePropType.Email,
    name: 'email',
    displayName: 'AbpIdentity::EmailAddress',
    id: 'email',
    validators: () => [Validators.required, Validators.maxLength(256), Validators.email],
  },
  {
    type: ePropType.String,
    name: 'phoneNumber',
    displayName: 'AbpIdentity::PhoneNumber',
    id: 'phone-number',
    validators: () => [Validators.maxLength(16)],
  },
  {
    type: ePropType.Boolean,
    name: 'lockoutEnabled',
    displayName: 'AbpIdentity::DisplayName:LockoutEnabled',
    id: 'lockout-checkbox',
    defaultValue: true,
  },
]);

export const DEFAULT_USERS_EDIT_FORM_PROPS = DEFAULT_USERS_CREATE_FORM_PROPS.filter(
  prop => prop.name !== 'password',
);
