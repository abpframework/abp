import { Injectable, Injector } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Observable } from 'rxjs';
import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultProps,
} from '@abp/ng.theme.shared/extensions';
import { ConfigStateService } from '@abp/ng.core';
import { tap, map, mapTo } from 'rxjs/operators';
import {
  ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS,
  DEFAULT_ACCOUNT_FORM_PROPS,
} from '../tokens/extensions.token';
import { AccountEditFormPropContributors } from '../models/config-options';
import { eAccountComponents } from '../enums/components';

@Injectable()
export class AccountExtensionsGuard implements CanActivate {
  constructor(private injector: Injector) {}

  canActivate(): Observable<boolean> {
    const extensions: ExtensionsService = this.injector.get(ExtensionsService);

    const editFormContributors: AccountEditFormPropContributors =
      this.injector.get(ACCOUNT_EDIT_FORM_PROP_CONTRIBUTORS, null) || {};

    const configState = this.injector.get(ConfigStateService);
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
      mapTo(true),
    );
  }
}
