import { inject } from '@angular/core';
import { map, tap } from 'rxjs';
import { ConfigStateService } from '@abp/ng.core';
import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultActions,
  mergeWithDefaultProps,
} from '@abp/ng.components/extensible';
import { eTenantManagementComponents } from '../enums';
import {
  TENANT_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS,
  TENANT_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS,
  TENANT_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS,
  DEFAULT_TENANT_MANAGEMENT_ENTITY_ACTIONS,
  DEFAULT_TENANT_MANAGEMENT_TOOLBAR_ACTIONS,
  DEFAULT_TENANT_MANAGEMENT_ENTITY_PROPS,
  DEFAULT_TENANT_MANAGEMENT_CREATE_FORM_PROPS,
  DEFAULT_TENANT_MANAGEMENT_EDIT_FORM_PROPS,
} from '../tokens';
import { ResolveFn } from '@angular/router';

export const tenantManagementExtensionsResolver: ResolveFn<any> = () => {
  const configState = inject(ConfigStateService);
  const extensions = inject(ExtensionsService);

  const config = { optional: true };

  const actionContributors = inject(TENANT_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS, config) || {};
  const toolbarContributors = inject(TENANT_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS, config) || {};
  const propContributors = inject(TENANT_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS, config) || {};
  const createFormContributors =
    inject(TENANT_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS, config) || {};
  const editFormContributors = inject(TENANT_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS, config) || {};

  return getObjectExtensionEntitiesFromStore(configState, 'TenantManagement').pipe(
    map(entities => ({
      [eTenantManagementComponents.Tenants]: entities.Tenant,
    })),
    mapEntitiesToContributors(configState, 'TenantManagement'),
    tap(objectExtensionContributors => {
      mergeWithDefaultActions(
        extensions.entityActions,
        DEFAULT_TENANT_MANAGEMENT_ENTITY_ACTIONS,
        actionContributors,
      );
      mergeWithDefaultActions(
        extensions.toolbarActions,
        DEFAULT_TENANT_MANAGEMENT_TOOLBAR_ACTIONS,
        toolbarContributors,
      );
      mergeWithDefaultProps(
        extensions.entityProps,
        DEFAULT_TENANT_MANAGEMENT_ENTITY_PROPS,
        objectExtensionContributors.prop,
        propContributors,
      );
      mergeWithDefaultProps(
        extensions.createFormProps,
        DEFAULT_TENANT_MANAGEMENT_CREATE_FORM_PROPS,
        objectExtensionContributors.createForm,
        createFormContributors,
      );
      mergeWithDefaultProps(
        extensions.editFormProps,
        DEFAULT_TENANT_MANAGEMENT_EDIT_FORM_PROPS,
        objectExtensionContributors.editForm,
        editFormContributors,
      );
    }),
  );
};
