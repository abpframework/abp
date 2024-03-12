import { inject, Injectable, Injector } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { filter, switchMap } from 'rxjs/operators';

import { HttpErrorReporterService } from '@abp/ng.core';

import { CustomHttpErrorHandlerService } from '../models/common';
import { Confirmation } from '../models/confirmation';

import { CUSTOM_ERROR_HANDLERS, HTTP_ERROR_HANDLER } from '../tokens/http-error.token';
import { HTTP_ERROR_CONFIG } from '../tokens/http-error.token';
import { DEFAULT_ERROR_LOCALIZATIONS, DEFAULT_ERROR_MESSAGES } from '../constants/default-errors';

import { ConfirmationService } from '../services/confirmation.service';
import { RouterErrorHandlerService } from '../services/router-error-handler.service';

@Injectable({ providedIn: 'root' })
export class ErrorHandler {
  protected readonly httpErrorReporter = inject(HttpErrorReporterService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly routerErrorHandlerService = inject(RouterErrorHandlerService);
  protected readonly httpErrorConfig = inject(HTTP_ERROR_CONFIG);
  protected readonly customErrorHandlers = inject(CUSTOM_ERROR_HANDLERS);
  protected readonly httpErrorHandler = inject(HTTP_ERROR_HANDLER, { optional: true });

  constructor(protected injector: Injector) {
    this.listenToRestError();
    this.listenToRouterError();
  }

  protected listenToRouterError() {
    this.routerErrorHandlerService.listen();
  }

  protected listenToRestError() {
    this.httpErrorReporter.reporter$
      .pipe(filter(this.filterRestErrors), switchMap(this.executeErrorHandler))
      .subscribe(err => this.handleError(err));
  }

  protected executeErrorHandler = (error: HttpErrorResponse) => {
    if (this.httpErrorHandler) {
      return this.httpErrorHandler(this.injector, error);
    }

    return of(error);
  };

  protected sortHttpErrorHandlers(
    a: CustomHttpErrorHandlerService,
    b: CustomHttpErrorHandlerService,
  ) {
    return (b.priority || 0) - (a.priority || 0);
  }

  protected handleError(err: unknown) {
    if (this.customErrorHandlers && this.customErrorHandlers.length) {
      const errorHandlerService = this.customErrorHandlers
        .sort(this.sortHttpErrorHandlers)
        .find(service => service.canHandle(err));

      if (errorHandlerService) {
        errorHandlerService.execute();
        return;
      }
    }

    this.showError().subscribe();
  }

  protected showError(): Observable<Confirmation.Status> {
    const title = {
      key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
      defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
    };
    const message = {
      key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.details,
      defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.details,
    };
    return this.confirmationService.error(message, title, {
      hideCancelBtn: true,
      yesText: 'AbpAccount::Close',
    });
  }

  protected filterRestErrors = ({ status }: HttpErrorResponse): boolean => {
    if (typeof status !== 'number') return false;

    if (!this.httpErrorConfig?.skipHandledErrorCodes) {
      return true;
    }

    return this.httpErrorConfig.skipHandledErrorCodes?.findIndex(code => code === status) < 0;
  };
}
