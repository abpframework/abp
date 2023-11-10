import { inject } from '@angular/core';
import { ConfigStateService } from '@abp/ng.core';
import { map, tap } from 'rxjs';
import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultProps,
} from '@abp/ng.components/extensible';
import { eAccountComponents } from '../enums';
import { ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS, DEFAULT_ACCOUNT_FORM_PROPS } from '../tokens';
import { ResolveFn } from '@angular/router';

export const accountExtensionsResolver: ResolveFn<any> = () => {
  const configState = inject(ConfigStateService);
  const extensions = inject(ExtensionsService);

  const config = { optional: true };

  const editFormContributors = inject(ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS, config) || {};

  return getObjectExtensionEntitiesFromStore(configState, 'Identity').pipe(
    map(entities => ({
      [eAccountComponents.PersonalSettings]: entities.User,
    })),
    mapEntitiesToContributors(configState, 'AbpIdentity'),
    tap(objectExtensionContributors => {
      mergeWithDefaultProps(
        extensions.editFormProps,
        DEFAULT_ACCOUNT_FORM_PROPS,
        objectExtensionContributors.editForm,
        editFormContributors,
      );
    }),
  );
};
