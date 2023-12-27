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
import { eIdentityComponents } from '../enums';
import {
  IDENTITY_ENTITY_ACTION_CONTRIBUTORS,
  IDENTITY_TOOLBAR_ACTION_CONTRIBUTORS,
  IDENTITY_ENTITY_PROP_CONTRIBUTORS,
  IDENTITY_CREATE_FORM_PROP_CONTRIBUTORS,
  IDENTITY_EDIT_FORM_PROP_CONTRIBUTORS,
  DEFAULT_IDENTITY_ENTITY_ACTIONS,
  DEFAULT_IDENTITY_TOOLBAR_ACTIONS,
  DEFAULT_IDENTITY_ENTITY_PROPS,
  DEFAULT_IDENTITY_CREATE_FORM_PROPS,
  DEFAULT_IDENTITY_EDIT_FORM_PROPS,
} from '../tokens';
import { ResolveFn } from '@angular/router';

export const identityExtensionsResolver: ResolveFn<any> = () => {
  const configState = inject(ConfigStateService);
  const extensions = inject(ExtensionsService);

  const config = { optional: true };

  const actionContributors = inject(IDENTITY_ENTITY_ACTION_CONTRIBUTORS, config) || {};
  const toolbarContributors = inject(IDENTITY_TOOLBAR_ACTION_CONTRIBUTORS, config) || {};
  const propContributors = inject(IDENTITY_ENTITY_PROP_CONTRIBUTORS, config) || {};
  const createFormContributors = inject(IDENTITY_CREATE_FORM_PROP_CONTRIBUTORS, config) || {};
  const editFormContributors = inject(IDENTITY_EDIT_FORM_PROP_CONTRIBUTORS, config) || {};

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
  );
};
