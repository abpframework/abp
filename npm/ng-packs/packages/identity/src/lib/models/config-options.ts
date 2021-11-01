import { IdentityRoleDto, IdentityUserDto } from '@abp/ng.identity/proxy';
import {
  CreateFormPropContributorCallback,
  EditFormPropContributorCallback,
  EntityActionContributorCallback,
  EntityPropContributorCallback,
  ToolbarActionContributorCallback,
} from '@abp/ng.theme.shared/extensions';
import { eIdentityComponents } from '../enums/components';

export type IdentityEntityActionContributors = Partial<{
  [eIdentityComponents.Roles]: EntityActionContributorCallback<IdentityRoleDto>[];
  [eIdentityComponents.Users]: EntityActionContributorCallback<IdentityUserDto>[];
}>;

export type IdentityToolbarActionContributors = Partial<{
  [eIdentityComponents.Roles]: ToolbarActionContributorCallback<IdentityRoleDto[]>[];
  [eIdentityComponents.Users]: ToolbarActionContributorCallback<IdentityUserDto[]>[];
}>;

export type IdentityEntityPropContributors = Partial<{
  [eIdentityComponents.Roles]: EntityPropContributorCallback<IdentityRoleDto>[];
  [eIdentityComponents.Users]: EntityPropContributorCallback<IdentityUserDto>[];
}>;

export type IdentityCreateFormPropContributors = Partial<{
  [eIdentityComponents.Roles]: CreateFormPropContributorCallback<IdentityRoleDto>[];
  [eIdentityComponents.Users]: CreateFormPropContributorCallback<IdentityUserDto>[];
}>;

export type IdentityEditFormPropContributors = Partial<{
  [eIdentityComponents.Roles]: EditFormPropContributorCallback<IdentityRoleDto>[];
  [eIdentityComponents.Users]: EditFormPropContributorCallback<IdentityUserDto>[];
}>;

export interface IdentityConfigOptions {
  entityActionContributors?: IdentityEntityActionContributors;
  toolbarActionContributors?: IdentityToolbarActionContributors;
  entityPropContributors?: IdentityEntityPropContributors;
  createFormPropContributors?: IdentityCreateFormPropContributors;
  editFormPropContributors?: IdentityEditFormPropContributors;
}
