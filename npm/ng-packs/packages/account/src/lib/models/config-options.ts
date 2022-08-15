import {eAccountComponents} from "../enums";
import { EditFormPropContributorCallback } from '@abp/ng.theme.shared/extensions';
import {UpdateProfileDto} from "@abp/ng.account.core/proxy";

export interface AccountConfigOptions {
  redirectUrl?: string;
}
export type AccountEditFormPropContributors = Partial<{
  [eAccountComponents.PersonalSettings]:  EditFormPropContributorCallback<UpdateProfileDto>[];
}>;

export interface AccountConfigOptions {
  editFormPropContributors?: AccountEditFormPropContributors;
}
