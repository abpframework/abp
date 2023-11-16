import { Injectable, inject } from '@angular/core';

import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { ConfigStateService, IAbpGuard } from '@abp/ng.core';
import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultActions,
  mergeWithDefaultProps,
} from '@abp/ng.components/extensible';

import { eIdentityComponents } from '../enums/components';
import {
  DEFAULT_IDENTITY_CREATE_FORM_PROPS,
  DEFAULT_IDENTITY_EDIT_FORM_PROPS,
  DEFAULT_IDENTITY_ENTITY_ACTIONS,
  DEFAULT_IDENTITY_ENTITY_PROPS,
  DEFAULT_IDENTITY_TOOLBAR_ACTIONS,
  IDENTITY_CREATE_FORM_PROP_CONTRIBUTORS,
  IDENTITY_EDIT_FORM_PROP_CONTRIBUTORS,
  IDENTITY_ENTITY_ACTION_CONTRIBUTORS,
  IDENTITY_ENTITY_PROP_CONTRIBUTORS,
  IDENTITY_TOOLBAR_ACTION_CONTRIBUTORS,
} from '../tokens/extensions.token';

/**
 * @deprecated Use `identityExtensionsResolver` *function* instead.
 */
@Injectable()
export class IdentityExtensionsGuard implements IAbpGuard {
  protected readonly configState = inject(ConfigStateService);
  protected readonly extensions = inject(ExtensionsService);

  canActivate(): Observable<boolean> {
    const config = { optional: true };

    const actionContributors = inject(IDENTITY_ENTITY_ACTION_CONTRIBUTORS, config) || {};
    const toolbarContributors = inject(IDENTITY_TOOLBAR_ACTION_CONTRIBUTORS, config) || {};
    const propContributors = inject(IDENTITY_ENTITY_PROP_CONTRIBUTORS, config) || {};
    const createFormContributors = inject(IDENTITY_CREATE_FORM_PROP_CONTRIBUTORS, config) || {};
    const editFormContributors = inject(IDENTITY_EDIT_FORM_PROP_CONTRIBUTORS, config) || {};

    return getObjectExtensionEntitiesFromStore(this.configState, 'Identity').pipe(
      map(entities => ({
        [eIdentityComponents.Roles]: entities.Role,
        [eIdentityComponents.Users]: entities.User,
      })),
      mapEntitiesToContributors(this.configState, 'AbpIdentity'),
      tap(objectExtensionContributors => {
        mergeWithDefaultActions(
          this.extensions.entityActions,
          DEFAULT_IDENTITY_ENTITY_ACTIONS,
          actionContributors,
        );
        mergeWithDefaultActions(
          this.extensions.toolbarActions,
          DEFAULT_IDENTITY_TOOLBAR_ACTIONS,
          toolbarContributors,
        );
        mergeWithDefaultProps(
          this.extensions.entityProps,
          DEFAULT_IDENTITY_ENTITY_PROPS,
          objectExtensionContributors.prop,
          propContributors,
        );
        mergeWithDefaultProps(
          this.extensions.createFormProps,
          DEFAULT_IDENTITY_CREATE_FORM_PROPS,
          objectExtensionContributors.createForm,
          createFormContributors,
        );
        mergeWithDefaultProps(
          this.extensions.editFormProps,
          DEFAULT_IDENTITY_EDIT_FORM_PROPS,
          objectExtensionContributors.editForm,
          editFormContributors,
        );
      }),
      map(() => true),
    );
  }
}
