import { Injectable, inject } from '@angular/core';
import { Observable, map, tap } from 'rxjs';
import { ConfigStateService, IAbpGuard } from '@abp/ng.core';
import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultProps,
} from '@abp/ng.theme.shared/extensions';
import {
  BOOK_STORE_RENT_FORM_PROP_CONTRIBUTORS,
  DEFAULT_BOOK_STORE_CREATE_FORM_PROPS,
} from '../tokens';

@Injectable()
export class BooksExtensionsGuard implements IAbpGuard {
  protected readonly configState = inject(ConfigStateService);
  protected readonly extensions = inject(ExtensionsService);

  canActivate(): Observable<boolean> {
    const createFormContributors =
      inject(BOOK_STORE_RENT_FORM_PROP_CONTRIBUTORS, { optional: true }) || {};

    return getObjectExtensionEntitiesFromStore(this.configState, 'BookStore').pipe(
      mapEntitiesToContributors(this.configState, 'BookStore'),
      tap(objectExtensionContributors => {
        mergeWithDefaultProps(
          this.extensions.createFormProps,
          DEFAULT_BOOK_STORE_CREATE_FORM_PROPS,
          objectExtensionContributors.createForm,
          createFormContributors
        );
      }),
      map(() => true)
    );
  }
}
