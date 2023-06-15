import { AuthService, HttpErrorReporterService } from '@abp/ng.core';
import { HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable, Injector } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, filter, switchMap } from 'rxjs/operators';
import { HttpErrorWrapperComponent } from '../components/http-error-wrapper/http-error-wrapper.component';
import { CustomHttpErrorHandlerService, HttpErrorConfig } from '../models/common';
import { Confirmation } from '../models/confirmation';
import { ConfirmationService } from '../services/confirmation.service';
import { CUSTOM_ERROR_HANDLERS, HTTP_ERROR_HANDLER } from '../tokens/http-error.token';
import { CreateErrorComponentService } from '../services/create-error-component.service';
import { DEFAULT_ERROR_LOCALIZATIONS, DEFAULT_ERROR_MESSAGES } from '../constants/error';
import { RouterErrorHandlerService } from '../services/router-error-handler.service';

@Injectable({ providedIn: 'root' })
export class ErrorHandler {
  protected httpErrorHandler = this.injector.get(HTTP_ERROR_HANDLER, (_, err: HttpErrorResponse) =>
    throwError(err),
  );
  protected httpErrorReporter: HttpErrorReporterService;
  protected confirmationService: ConfirmationService;
  protected httpErrorConfig: HttpErrorConfig;

  private createErrorComponentService = inject(CreateErrorComponentService);
  private customErrorHandlers = inject(CUSTOM_ERROR_HANDLERS);
  private routerErrorHandlerService = inject(RouterErrorHandlerService);

  constructor(protected injector: Injector) {
    this.httpErrorReporter = injector.get(HttpErrorReporterService);
    this.confirmationService = injector.get(ConfirmationService);
    this.httpErrorConfig = injector.get('HTTP_ERROR_CONFIG');
    this.listenToRestError();
    this.listenToRouterError();
  }

  protected listenToRouterError() {
    this.routerErrorHandlerService.listen();
  }

  protected listenToRestError() {
    this.httpErrorReporter.reporter$
      .pipe(filter(this.filterRestErrors), switchMap(this.executeErrorHandler))
      .subscribe();
  }

  private executeErrorHandler = (error: any) => {
    const errHandler = this.httpErrorHandler(this.injector, error);
    const isObservable = errHandler instanceof Observable;
    const response = isObservable ? errHandler : of(null);

    return response.pipe(
      catchError(err => {
        this.handleError(err);
        return of(null);
      }),
    );
  };

  protected sortHttpErrorHandlers(
    a: CustomHttpErrorHandlerService,
    b: CustomHttpErrorHandlerService,
  ) {
    return (b.priority || 0) - (a.priority || 0);
  }

  private handleError(err: unknown) {
    if (this.customErrorHandlers && this.customErrorHandlers.length) {
      const canHandleService = this.customErrorHandlers
        .sort(this.sortHttpErrorHandlers)
        .find(service => service.canHandle(err));
      if (canHandleService) {
        canHandleService.execute();
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

  createErrorComponent(instance: Partial<HttpErrorWrapperComponent>) {
    this.createErrorComponentService.execute(instance);
  }

  protected filterRestErrors = ({ status }: HttpErrorResponse): boolean => {
    if (typeof status !== 'number') return false;

    return (
      !!this.httpErrorConfig.skipHandledErrorCodes &&
      this.httpErrorConfig.skipHandledErrorCodes.findIndex(code => code === status) < 0
    );
  };
}
