import { inject, Injectable } from '@angular/core';
import { NavigationError } from '@angular/router';
import { filter } from 'rxjs/operators';
import { RouterEvents } from '@abp/ng.core';
import { HTTP_ERROR_CONFIG } from '../tokens/';
import { CreateErrorComponentService } from '../services';
import { DEFAULT_ERROR_LOCALIZATIONS, DEFAULT_ERROR_MESSAGES } from '../constants/default-errors';
import { ErrorScreenErrorCodes } from '../models';

@Injectable({ providedIn: 'root' })
export class RouterErrorHandlerService {
  protected readonly routerEvents = inject(RouterEvents);
  protected readonly httpErrorConfig = inject(HTTP_ERROR_CONFIG);
  protected readonly createErrorComponentService = inject(CreateErrorComponentService);

  protected filterRouteErrors = (navigationError: NavigationError): boolean => {
    if (!this.httpErrorConfig?.skipHandledErrorCodes) {
      return true;
    }

    return (
      navigationError.error?.message?.indexOf('Cannot match') > -1 &&
      this.httpErrorConfig.skipHandledErrorCodes.findIndex(code => code === 404) < 0
    );
  };

  listen() {
    this.routerEvents
      .getNavigationEvents('Error')
      .pipe(filter(this.filterRouteErrors))
      .subscribe(() => this.show404Page());
  }

  show404Page() {
    const instance = {
      title: {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError404.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.title,
      },
      status: <ErrorScreenErrorCodes>404,
    };

    this.createErrorComponentService.execute(instance);
  }
}
