import { inject, Injectable } from '@angular/core';
import { filter } from 'rxjs/operators';
import { RouterEvents } from '@abp/ng.core';
import { NavigationError } from '@angular/router';
import { HTTP_ERROR_CONFIG } from '../tokens/';
import { CreateErrorComponentService } from '../services';
import { DEFAULT_ERROR_LOCALIZATIONS, DEFAULT_ERROR_MESSAGES } from '../constants/default-errors';

@Injectable({ providedIn: 'root' })
export class RouterErrorHandlerService {
  private readonly routerEvents = inject(RouterEvents);
  private httpErrorConfig = inject(HTTP_ERROR_CONFIG);
  private createErrorComponentService = inject(CreateErrorComponentService);

  listen() {
    this.routerEvents
      .getNavigationEvents('Error')
      .pipe(filter(this.filterRouteErrors))
      .subscribe(() => this.show404Page());
  }

  protected filterRouteErrors = (navigationError: NavigationError): boolean => {
    return (
      navigationError.error?.message?.indexOf('Cannot match') > -1 &&
      !!this.httpErrorConfig.skipHandledErrorCodes &&
      this.httpErrorConfig.skipHandledErrorCodes.findIndex(code => code === 404) < 0
    );
  };

  show404Page() {
    const instance = {
      title: {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError404.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.title,
      },
      status: 404,
    };

    this.createErrorComponentService.execute(instance);
  }
}
