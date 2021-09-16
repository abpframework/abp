import {
  CreateFormPropContributorCallback,
  EditFormPropContributorCallback,
  EntityActionContributorCallback,
  EntityPropContributorCallback,
  ToolbarActionContributorCallback,
} from '@abp/ng.theme.shared/extensions';
import { InjectionToken } from '@angular/core';
import { DEFAULT_TENANTS_ENTITY_ACTIONS } from '../defaults/default-tenants-entity-actions';
import { DEFAULT_TENANTS_ENTITY_PROPS } from '../defaults/default-tenants-entity-props';
import {
  DEFAULT_TENANTS_CREATE_FORM_PROPS,
  DEFAULT_TENANTS_EDIT_FORM_PROPS,
} from '../defaults/default-tenants-form-props';
import { DEFAULT_TENANTS_TOOLBAR_ACTIONS } from '../defaults/default-tenants-toolbar-actions';
import { eTenantManagementComponents } from '../enums/components';
import { TenantCreateDto, TenantDto, TenantUpdateDto } from '../proxy/models';

export const DEFAULT_TENANT_MANAGEMENT_ENTITY_ACTIONS = {
  [eTenantManagementComponents.Tenants]: DEFAULT_TENANTS_ENTITY_ACTIONS,
};

export const DEFAULT_TENANT_MANAGEMENT_TOOLBAR_ACTIONS = {
  [eTenantManagementComponents.Tenants]: DEFAULT_TENANTS_TOOLBAR_ACTIONS,
};

export const DEFAULT_TENANT_MANAGEMENT_ENTITY_PROPS = {
  [eTenantManagementComponents.Tenants]: DEFAULT_TENANTS_ENTITY_PROPS,
};

export const DEFAULT_TENANT_MANAGEMENT_CREATE_FORM_PROPS = {
  [eTenantManagementComponents.Tenants]: DEFAULT_TENANTS_CREATE_FORM_PROPS,
};

export const DEFAULT_TENANT_MANAGEMENT_EDIT_FORM_PROPS = {
  [eTenantManagementComponents.Tenants]: DEFAULT_TENANTS_EDIT_FORM_PROPS,
};

export const TENANT_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS =
  new InjectionToken<EntityActionContributors>('TENANT_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS');

export const TENANT_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS =
  new InjectionToken<ToolbarActionContributors>('TENANT_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS');

export const TENANT_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS =
  new InjectionToken<EntityPropContributors>('TENANT_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS');

export const TENANT_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS =
  new InjectionToken<CreateFormPropContributors>('TENANT_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS');

export const TENANT_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS =
  new InjectionToken<EditFormPropContributors>('TENANT_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS');

// Fix for TS4023 -> https://github.com/microsoft/TypeScript/issues/9944#issuecomment-254693497
type EntityActionContributors = Partial<{
  [eTenantManagementComponents.Tenants]: EntityActionContributorCallback<TenantDto>[];
}>;
type ToolbarActionContributors = Partial<{
  [eTenantManagementComponents.Tenants]: ToolbarActionContributorCallback<TenantDto[]>[];
}>;
type EntityPropContributors = Partial<{
  [eTenantManagementComponents.Tenants]: EntityPropContributorCallback<TenantDto>[];
}>;
type CreateFormPropContributors = Partial<{
  [eTenantManagementComponents.Tenants]: CreateFormPropContributorCallback<TenantCreateDto>[];
}>;
type EditFormPropContributors = Partial<{
  [eTenantManagementComponents.Tenants]: EditFormPropContributorCallback<TenantUpdateDto>[];
}>;
