import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultActions,
  mergeWithDefaultProps,
} from '@abp/ng.components/extensible';
import { IAbpGuard } from '@abp/ng.core';
import { Injectable, Injector, inject } from '@angular/core';

import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { eTenantManagementComponents } from '../enums/components';
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

/**
 * @deprecated Use `tenantManagementExtensionsResolver` *function* instead.
 */
@Injectable()
export class TenantManagementExtensionsGuard implements IAbpGuard {
  protected readonly injector = inject(Injector);
  protected readonly extensions = inject(ExtensionsService);

  canActivate(): Observable<boolean> {
    const config = { optional: true };

    const actionContributors = inject(TENANT_MANAGEMENT_ENTITY_ACTION_CONTRIBUTORS, config) || {};
    const toolbarContributors = inject(TENANT_MANAGEMENT_TOOLBAR_ACTION_CONTRIBUTORS, config) || {};
    const propContributors = inject(TENANT_MANAGEMENT_ENTITY_PROP_CONTRIBUTORS, config) || {};
    const createFormContributors =
      inject(TENANT_MANAGEMENT_CREATE_FORM_PROP_CONTRIBUTORS, config) || {};
    const editFormContributors =
      inject(TENANT_MANAGEMENT_EDIT_FORM_PROP_CONTRIBUTORS, config) || {};

    return getObjectExtensionEntitiesFromStore(this.injector, 'TenantManagement').pipe(
      map(entities => ({
        [eTenantManagementComponents.Tenants]: entities.Tenant,
      })),
      mapEntitiesToContributors(this.injector, 'TenantManagement'),
      tap(objectExtensionContributors => {
        mergeWithDefaultActions(
          this.extensions.entityActions,
          DEFAULT_TENANT_MANAGEMENT_ENTITY_ACTIONS,
          actionContributors,
        );
        mergeWithDefaultActions(
          this.extensions.toolbarActions,
          DEFAULT_TENANT_MANAGEMENT_TOOLBAR_ACTIONS,
          toolbarContributors,
        );
        mergeWithDefaultProps(
          this.extensions.entityProps,
          DEFAULT_TENANT_MANAGEMENT_ENTITY_PROPS,
          objectExtensionContributors.prop,
          propContributors,
        );
        mergeWithDefaultProps(
          this.extensions.createFormProps,
          DEFAULT_TENANT_MANAGEMENT_CREATE_FORM_PROPS,
          objectExtensionContributors.createForm,
          createFormContributors,
        );
        mergeWithDefaultProps(
          this.extensions.editFormProps,
          DEFAULT_TENANT_MANAGEMENT_EDIT_FORM_PROPS,
          objectExtensionContributors.editForm,
          editFormContributors,
        );
      }),
      map(() => true),
    );
  }
}
