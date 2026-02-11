import { inject, Injector } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { map, tap } from 'rxjs';
import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultProps,
} from '@abp/ng.components/extensible';
import { eAccountComponents } from '../enums';
import { ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS, DEFAULT_ACCOUNT_FORM_PROPS } from '../tokens';

export const accountExtensionsResolver: ResolveFn<any> = () => {
  const injector = inject(Injector);
  const extensions = inject(ExtensionsService);

  const config = { optional: true };

  const editFormContributors = inject(ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS, config) || {};

  return getObjectExtensionEntitiesFromStore(injector, 'Identity').pipe(
    map(entities => ({
      [eAccountComponents.PersonalSettings]: entities.User,
    })),
    mapEntitiesToContributors(injector, 'AbpIdentity'),
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
