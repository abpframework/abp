 import {
  AuthService,
  HttpErrorReporterService,
  LocalizationParam,
  RouterEvents,
  SessionStateService,
} from '@abp/ng.core';
import { HttpErrorResponse } from '@angular/common/http';
import {
  ComponentFactoryResolver,
  ComponentRef,
  inject,
  Injectable,
  Injector,
  RendererFactory2,
} from '@angular/core';
import { NavigationError, ResolveEnd } from '@angular/router';
import { Observable, of, Subject, throwError } from 'rxjs';
import { catchError, filter, switchMap } from 'rxjs/operators';
import { HttpErrorWrapperComponent } from '../components/http-error-wrapper/http-error-wrapper.component';
import { ErrorScreenErrorCodes, HttpErrorConfig } from '../models/common';
import { Confirmation } from '../models/confirmation';
import { ConfirmationService } from '../services/confirmation.service';
import { HTTP_ERROR_HANDLER } from '../tokens/http-error.token';
import { CreateErrorComponentService } from '../services/create-error-component.service';
import { CanCreateCustomErrorService } from '../services/can-create-custom-error.service';
import { DEFAULT_ERROR_LOCALIZATIONS, DEFAULT_ERROR_MESSAGES } from '../constants/error';

@Injectable({ providedIn: 'root' })
export class ErrorHandler {
  protected httpErrorHandler = this.injector.get(HTTP_ERROR_HANDLER, (_, err: HttpErrorResponse) =>
    throwError(err),
  );

  protected httpErrorReporter: HttpErrorReporterService;
  protected routerEvents: RouterEvents;
  protected confirmationService: ConfirmationService;
  protected cfRes: ComponentFactoryResolver;
  protected rendererFactory: RendererFactory2;
  protected httpErrorConfig: HttpErrorConfig;
  protected sessionStateService: SessionStateService;
  private authService: AuthService;
  

  private createErrorComponentService = inject(CreateErrorComponentService);
  private canCreateCustomErrorService = inject(CanCreateCustomErrorService);
  constructor(protected injector: Injector) {
    this.httpErrorReporter = injector.get(HttpErrorReporterService);
    this.routerEvents = injector.get(RouterEvents);
    this.confirmationService = injector.get(ConfirmationService);
    this.cfRes = injector.get(ComponentFactoryResolver);
    this.rendererFactory = injector.get(RendererFactory2);
    this.httpErrorConfig = injector.get('HTTP_ERROR_CONFIG');
    this.authService = this.injector.get(AuthService);
    this.sessionStateService = this.injector.get(SessionStateService);

    this.listenToRestError();
    this.listenToRouterError();
  }

  protected listenToRouterError() {
    this.routerEvents
      .getNavigationEvents('Error')
      .pipe(filter(this.filterRouteErrors))
      .subscribe(() => this.show404Page());
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

  private handleError(err: any) {
    const body = err?.error?.error || {
      key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
      defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
    };

    if (err instanceof HttpErrorResponse && err.headers.get('Abp-Tenant-Resolve-Error')) {
      this.sessionStateService.setTenant(null)
      this.authService.logout().subscribe();
      return;
    }

    const isAbpErrorFormat = err.headers.get('_AbpErrorFormat');
    if (err instanceof HttpErrorResponse && isAbpErrorFormat) {
      const confirmation$ = this.showErrorWithRequestBody(body);

      if (err.status === 401) {
        confirmation$.subscribe(() => {
          this.navigateToLogin();
        });
      }
    } else {
      switch (err.status) {
        case 401:
          this.canCreateCustomError(401)
            ? this.show401Page()
            : this.showError(
                {
                  key: DEFAULT_ERROR_LOCALIZATIONS.defaultError401.title,
                  defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.title,
                },
                {
                  key: DEFAULT_ERROR_LOCALIZATIONS.defaultError401.details,
                  defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.details,
                },
              ).subscribe(() => this.navigateToLogin());
          break;
        case 403:
          this.createErrorComponent({
            title: {
              key: DEFAULT_ERROR_LOCALIZATIONS.defaultError403.title,
              defaultValue: DEFAULT_ERROR_MESSAGES.defaultError403.title,
            },
            details: {
              key: DEFAULT_ERROR_LOCALIZATIONS.defaultError403.details,
              defaultValue: DEFAULT_ERROR_MESSAGES.defaultError403.details,
            },
            status: 403,
          });
          break;
        case 404:
          this.canCreateCustomError(404)
            ? this.show404Page()
            : this.showError(
                {
                  key: DEFAULT_ERROR_LOCALIZATIONS.defaultError404.details,
                  defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.details,
                },
                {
                  key: DEFAULT_ERROR_LOCALIZATIONS.defaultError404.title,
                  defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.title,
                },
              );
          break;
        case 500:
          this.createErrorComponent({
            title: {
              key: DEFAULT_ERROR_LOCALIZATIONS.defaultError500.title,
              defaultValue: DEFAULT_ERROR_MESSAGES.defaultError500.title,
            },
            details: {
              key: DEFAULT_ERROR_LOCALIZATIONS.defaultError500.details,
              defaultValue: DEFAULT_ERROR_MESSAGES.defaultError500.details,
            },
            status: 500,
          });
          break;
        case 0:
          if (err.statusText === 'Unknown Error') {
            this.createErrorComponent({
              title: {
                key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
                defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
              },
              details: err.message,
              isHomeShow: false,
            });
          }
          break;
        default:
          this.showError(
            {
              key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.details,
              defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.details,
            },
            {
              key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
              defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
            },
          );
          break;
      }
    }
  }

  protected show401Page() {
    this.createErrorComponent({
      title: {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError401.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.title,
      },
      status: 401,
    });
  }

  protected show404Page() {
    this.createErrorComponent({
      title: {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError404.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.title,
      },
      status: 404,
    });
  }

  protected showErrorWithRequestBody(body: any) {
    let message: LocalizationParam;
    let title: LocalizationParam;

    if (body.details) {
      message = body.details;
      title = body.message;
    } else if (body.message) {
      title = {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
      };
      message = body.message;
    } else {
      message = body.message || {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
      };
      title = '';
    }

    return this.showError(message, title);
  }

  protected showError(
    message: LocalizationParam,
    title: LocalizationParam,
  ): Observable<Confirmation.Status> {
    return this.confirmationService.error(message, title, {
      hideCancelBtn: true,
      yesText: 'AbpAccount::Close',
    });
  }

  private navigateToLogin() {
    this.authService.navigateToLogin();
  }

  createErrorComponent(instance: Partial<HttpErrorWrapperComponent>) {
    this.createErrorComponentService.execute(instance);
  }

  canCreateCustomError(status: ErrorScreenErrorCodes): boolean {
    return this.canCreateCustomErrorService.execute(status);
  }

  protected filterRestErrors = ({ status }: HttpErrorResponse): boolean => {
    if (typeof status !== 'number') return false;

    return (
      !!this.httpErrorConfig.skipHandledErrorCodes &&
      this.httpErrorConfig.skipHandledErrorCodes.findIndex(code => code === status) < 0
    );
  };

  protected filterRouteErrors = (navigationError: NavigationError): boolean => {
    return (
      navigationError.error?.message?.indexOf('Cannot match') > -1 &&
      !!this.httpErrorConfig.skipHandledErrorCodes &&
      this.httpErrorConfig.skipHandledErrorCodes.findIndex(code => code === 404) < 0
    );
  };
}
