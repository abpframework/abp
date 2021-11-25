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
import { eIdentityComponents } from '../enums/components';
import {
  IdentityCreateFormPropContributors,
  IdentityEditFormPropContributors,
  IdentityEntityActionContributors,
  IdentityEntityPropContributors,
  IdentityToolbarActionContributors,
} from '../models/config-options';
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

@Injectable()
export class IdentityExtensionsGuard implements CanActivate {
  constructor(private injector: Injector) {}

  canActivate(): Observable<boolean> {
    const extensions: ExtensionsService = this.injector.get(ExtensionsService);
    const actionContributors: IdentityEntityActionContributors =
      this.injector.get(IDENTITY_ENTITY_ACTION_CONTRIBUTORS, null) || {};
    const toolbarContributors: IdentityToolbarActionContributors =
      this.injector.get(IDENTITY_TOOLBAR_ACTION_CONTRIBUTORS, null) || {};
    const propContributors: IdentityEntityPropContributors =
      this.injector.get(IDENTITY_ENTITY_PROP_CONTRIBUTORS, null) || {};
    const createFormContributors: IdentityCreateFormPropContributors =
      this.injector.get(IDENTITY_CREATE_FORM_PROP_CONTRIBUTORS, null) || {};
    const editFormContributors: IdentityEditFormPropContributors =
      this.injector.get(IDENTITY_EDIT_FORM_PROP_CONTRIBUTORS, null) || {};

    const configState = this.injector.get(ConfigStateService);
    return getObjectExtensionEntitiesFromStore(configState, 'Identity').pipe(
      map(entities => ({
        [eIdentityComponents.Roles]: entities.Role,
        [eIdentityComponents.Users]: entities.User,
      })),
      mapEntitiesToContributors(configState, 'AbpIdentity'),
      tap(objectExtensionContributors => {
        mergeWithDefaultActions(
          extensions.entityActions,
          DEFAULT_IDENTITY_ENTITY_ACTIONS,
          actionContributors,
        );
        mergeWithDefaultActions(
          extensions.toolbarActions,
          DEFAULT_IDENTITY_TOOLBAR_ACTIONS,
          toolbarContributors,
        );
        mergeWithDefaultProps(
          extensions.entityProps,
          DEFAULT_IDENTITY_ENTITY_PROPS,
          objectExtensionContributors.prop,
          propContributors,
        );
        mergeWithDefaultProps(
          extensions.createFormProps,
          DEFAULT_IDENTITY_CREATE_FORM_PROPS,
          objectExtensionContributors.createForm,
          createFormContributors,
        );
        mergeWithDefaultProps(
          extensions.editFormProps,
          DEFAULT_IDENTITY_EDIT_FORM_PROPS,
          objectExtensionContributors.editForm,
          editFormContributors,
        );
      }),
      mapTo(true),
    );
  }
}
