import {
  CreateFormPropContributorCallback,
  EditFormPropContributorCallback,
  EntityActionContributorCallback,
  EntityPropContributorCallback,
  ToolbarActionContributorCallback,
} from '@abp/ng.theme.shared/extensions';
import { InjectionToken } from '@angular/core';
import { DEFAULT_ROLES_ENTITY_ACTIONS } from '../defaults/default-roles-entity-actions';
import { DEFAULT_ROLES_ENTITY_PROPS } from '../defaults/default-roles-entity-props';
import {
  DEFAULT_ROLES_CREATE_FORM_PROPS,
  DEFAULT_ROLES_EDIT_FORM_PROPS,
} from '../defaults/default-roles-form-props';
import { DEFAULT_ROLES_TOOLBAR_ACTIONS } from '../defaults/default-roles-toolbar-actions';
import { DEFAULT_USERS_ENTITY_ACTIONS } from '../defaults/default-users-entity-actions';
import { DEFAULT_USERS_ENTITY_PROPS } from '../defaults/default-users-entity-props';
import {
  DEFAULT_USERS_CREATE_FORM_PROPS,
  DEFAULT_USERS_EDIT_FORM_PROPS,
} from '../defaults/default-users-form-props';
import { DEFAULT_USERS_TOOLBAR_ACTIONS } from '../defaults/default-users-toolbar-actions';
import { eIdentityComponents } from '../enums/components';
import { IdentityRoleDto, IdentityUserDto } from '../proxy/identity/models';

export const DEFAULT_IDENTITY_ENTITY_ACTIONS = {
  [eIdentityComponents.Roles]: DEFAULT_ROLES_ENTITY_ACTIONS,
  [eIdentityComponents.Users]: DEFAULT_USERS_ENTITY_ACTIONS,
};

export const DEFAULT_IDENTITY_TOOLBAR_ACTIONS = {
  [eIdentityComponents.Roles]: DEFAULT_ROLES_TOOLBAR_ACTIONS,
  [eIdentityComponents.Users]: DEFAULT_USERS_TOOLBAR_ACTIONS,
};

export const DEFAULT_IDENTITY_ENTITY_PROPS = {
  [eIdentityComponents.Roles]: DEFAULT_ROLES_ENTITY_PROPS,
  [eIdentityComponents.Users]: DEFAULT_USERS_ENTITY_PROPS,
};

export const DEFAULT_IDENTITY_CREATE_FORM_PROPS = {
  [eIdentityComponents.Roles]: DEFAULT_ROLES_CREATE_FORM_PROPS,
  [eIdentityComponents.Users]: DEFAULT_USERS_CREATE_FORM_PROPS,
};

export const DEFAULT_IDENTITY_EDIT_FORM_PROPS = {
  [eIdentityComponents.Roles]: DEFAULT_ROLES_EDIT_FORM_PROPS,
  [eIdentityComponents.Users]: DEFAULT_USERS_EDIT_FORM_PROPS,
};

export const IDENTITY_ENTITY_ACTION_CONTRIBUTORS = new InjectionToken<EntityActionContributors>(
  'IDENTITY_ENTITY_ACTION_CONTRIBUTORS',
);

export const IDENTITY_TOOLBAR_ACTION_CONTRIBUTORS = new InjectionToken<ToolbarActionContributors>(
  'IDENTITY_TOOLBAR_ACTION_CONTRIBUTORS',
);

export const IDENTITY_ENTITY_PROP_CONTRIBUTORS = new InjectionToken<EntityPropContributors>(
  'IDENTITY_ENTITY_PROP_CONTRIBUTORS',
);

export const IDENTITY_CREATE_FORM_PROP_CONTRIBUTORS =
  new InjectionToken<CreateFormPropContributors>('IDENTITY_CREATE_FORM_PROP_CONTRIBUTORS');

export const IDENTITY_EDIT_FORM_PROP_CONTRIBUTORS = new InjectionToken<EditFormPropContributors>(
  'IDENTITY_EDIT_FORM_PROP_CONTRIBUTORS',
);

// Fix for TS4023 -> https://github.com/microsoft/TypeScript/issues/9944#issuecomment-254693497
type EntityActionContributors = Partial<{
  [eIdentityComponents.Roles]: EntityActionContributorCallback<IdentityRoleDto>[];
  [eIdentityComponents.Users]: EntityActionContributorCallback<IdentityUserDto>[];
}>;
type ToolbarActionContributors = Partial<{
  [eIdentityComponents.Roles]: ToolbarActionContributorCallback<IdentityRoleDto[]>[];
  [eIdentityComponents.Users]: ToolbarActionContributorCallback<IdentityUserDto[]>[];
}>;
type EntityPropContributors = Partial<{
  [eIdentityComponents.Roles]: EntityPropContributorCallback<IdentityRoleDto>[];
  [eIdentityComponents.Users]: EntityPropContributorCallback<IdentityUserDto>[];
}>;
type CreateFormPropContributors = Partial<{
  [eIdentityComponents.Roles]: CreateFormPropContributorCallback<IdentityRoleDto>[];
  [eIdentityComponents.Users]: CreateFormPropContributorCallback<IdentityUserDto>[];
}>;
type EditFormPropContributors = Partial<{
  [eIdentityComponents.Roles]: EditFormPropContributorCallback<IdentityRoleDto>[];
  [eIdentityComponents.Users]: EditFormPropContributorCallback<IdentityUserDto>[];
}>;
