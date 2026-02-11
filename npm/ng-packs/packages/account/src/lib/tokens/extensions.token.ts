import { eAccountComponents } from '../enums';
import { DEFAULT_PERSONAL_SETTINGS_UPDATE_FORM_PROPS } from '../defaults/default-personal-settings-form-props';
import { InjectionToken } from '@angular/core';
import { EditFormPropContributorCallback } from '@abp/ng.components/extensible';
import { UpdateProfileDto } from '@abp/ng.account.core/proxy';

export const DEFAULT_ACCOUNT_FORM_PROPS = {
  [eAccountComponents.PersonalSettings]: DEFAULT_PERSONAL_SETTINGS_UPDATE_FORM_PROPS,
};

export const ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS = new InjectionToken<EditFormPropContributors>(
  'ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS',
);

type EditFormPropContributors = Partial<{
  [eAccountComponents.PersonalSettings]: EditFormPropContributorCallback<UpdateProfileDto>[];
}>;
