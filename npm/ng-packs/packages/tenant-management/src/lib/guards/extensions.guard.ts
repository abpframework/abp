import { ConfigStateService } from '@abp/ng.core';
import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultActions,
  mergeWithDefaultProps,
} from '@abp/ng.theme.shared/extensions';
import { Injectable, Injector } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Observable } from 'rxjs';
import { map, mapTo, tap } from 'rxjs/operators';
import { eTenantManagementComponents } from '../enums/components';
import {
  TenantManagementCreateFormPropContributors,
  TenantManagementEditFormPropContributors,
  TenantManagementEntityActionContributors,
  TenantManagementEntityPropContributors,
  TenantManagementToolbarActionContributors,
} from '../models/config-options';
import {
  DEFAULT_TENANT_MANAGEMENT_CREATE_FORM_PROPS,
  DEFAULT_TENANT_MANAGEMENT_EDIT_FORM_PROPS,
  DEFAULT_TENANT_MANAGEMENT_ENTITY_ACTIONS,
  DEFAULT_TENANT_MANAGEMENT_ENTITY_PROPS,
  DEFAULT_TENANT_MANAGEMENT_TOOLBAR_ACTIONS,
  TENANT_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS,
  TENANT_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS,
  TENANT_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS,
} from '../tokens/extensions.token';

@Injectable()
export class TenantManagementExtensionsGuard implements CanActivate {
  constructor(private injector: Injector) {}

  canActivate(): Observable<boolean> {
    const extensions: ExtensionsService = this.injector.get(ExtensionsService);
    const actionContributors: TenantManagementEntityActionContributors =
      this.injector.get(TENANT_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS, null) || {};
    const toolbarContributors: TenantManagementToolbarActionContributors =
      this.injector.get(TENANT_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS, null) || {};
    const propContributors: TenantManagementEntityPropContributors =
      this.injector.get(TENANT_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS, null) || {};
    const createFormContributors: TenantManagementCreateFormPropContributors =
      this.injector.get(TENANT_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS, null) || {};
    const editFormContributors: TenantManagementEditFormPropContributors =
      this.injector.get(TENANT_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS, null) || {};

    const configState = this.injector.get(ConfigStateService);
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
      mapTo(true),
    );
  }
}
