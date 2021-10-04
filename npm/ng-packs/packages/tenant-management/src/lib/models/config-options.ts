import { TenantCreateDto, TenantDto, TenantUpdateDto } from '@abp/ng.tenant-management/proxy';
import {
  CreateFormPropContributorCallback,
  EditFormPropContributorCallback,
  EntityActionContributorCallback,
  EntityPropContributorCallback,
  ToolbarActionContributorCallback,
} from '@abp/ng.theme.shared/extensions';
import { eTenantManagementComponents } from '../enums/components';

export type TenantManagementEntityActionContributors = Partial<{
  [eTenantManagementComponents.Tenants]: EntityActionContributorCallback<TenantDto>[];
}>;

export type TenantManagementToolbarActionContributors = Partial<{
  [eTenantManagementComponents.Tenants]: ToolbarActionContributorCallback<TenantDto[]>[];
}>;

export type TenantManagementEntityPropContributors = Partial<{
  [eTenantManagementComponents.Tenants]: EntityPropContributorCallback<TenantDto>[];
}>;

export type TenantManagementCreateFormPropContributors = Partial<{
  [eTenantManagementComponents.Tenants]: CreateFormPropContributorCallback<TenantCreateDto>[];
}>;

export type TenantManagementEditFormPropContributors = Partial<{
  [eTenantManagementComponents.Tenants]: EditFormPropContributorCallback<TenantUpdateDto>[];
}>;

export interface TenantManagementConfigOptions {
  entityActionContributors?: TenantManagementEntityActionContributors;
  toolbarActionContributors?: TenantManagementToolbarActionContributors;
  entityPropContributors?: TenantManagementEntityPropContributors;
  createFormPropContributors?: TenantManagementCreateFormPropContributors;
  editFormPropContributors?: TenantManagementEditFormPropContributors;
}
