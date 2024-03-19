import { Injectable, inject } from '@angular/core';

import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';

import { ConfigStateService, IAbpGuard } from '@abp/ng.core';
import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultProps,
} from '@abp/ng.components/extensible';

import {
  ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS,
  DEFAULT_ACCOUNT_FORM_PROPS,
} from '../tokens/extensions.token';
import { eAccountComponents } from '../enums/components';

/**
 * @deprecated Use `accountExtensionsResolver` *function* instead.
 */
@Injectable()
export class AccountExtensionsGuard implements IAbpGuard {
  protected readonly configState = inject(ConfigStateService);
  protected readonly extensions = inject(ExtensionsService);

  canActivate(): Observable<boolean> {
    const config = { optional: true };

    const editFormContributors = inject(ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS, config) || {};

    return getObjectExtensionEntitiesFromStore(this.configState, 'Identity').pipe(
      map(entities => ({
        [eAccountComponents.PersonalSettings]: entities.User,
      })),
      mapEntitiesToContributors(this.configState, 'AbpIdentity'),
      tap(objectExtensionContributors => {
        mergeWithDefaultProps(
          this.extensions.editFormProps,
          DEFAULT_ACCOUNT_FORM_PROPS,
          objectExtensionContributors.editForm,
          editFormContributors,
        );
      }),
      map(() => true),
    );
  }
}
