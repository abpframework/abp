import { ePropType, FormProp } from '@abp/ng.theme.shared/extensions';
import { UpdateProfileDto } from '@abp/ng.account.core/proxy';
import { Validators } from '@angular/forms';
import { PersonalSettingsHalfRowComponent } from '../components/personal-settings/personal-settings-half-row.component';

const { maxLength, required, email } = Validators;
export const DEFAULT_PERSONAL_SETTINGS_UPDATE_FORM_PROPS = FormProp.createMany<UpdateProfileDto>([
  {
    type: ePropType.String,
    name: 'userName',
    displayName: 'AbpIdentity::DisplayName:UserName',
    id: 'username',
    validators: () => [required, maxLength(256)],
  },
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'AbpIdentity::DisplayName:Name',
    id: 'name',
    validators: () => [maxLength(64)],
    template: PersonalSettingsHalfRowComponent,
    className: 'd-inline-block w-50',
  },
  {
    type: ePropType.String,
    name: 'surname',
    displayName: 'AbpIdentity::DisplayName:Surname',
    id: 'surname',
    validators: () => [maxLength(64)],
    className: 'd-inline-block w-50 ps-4',
    template: PersonalSettingsHalfRowComponent,
  },
  {
    type: ePropType.String,
    name: 'email',
    displayName: 'AbpIdentity::DisplayName:Email',
    id: 'email-address',
    validators: () => [required, email, maxLength(256)],
  },
  {
    type: ePropType.String,
    name: 'phoneNumber',
    displayName: 'AbpIdentity::DisplayName:PhoneNumber',
    id: 'phone-number',
    validators: () => [maxLength(16)],
  },
]);
