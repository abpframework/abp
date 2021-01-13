import { AuthService, LocalizationParam, RestOccurError } from '@abp/ng.core';
import { HttpErrorResponse } from '@angular/common/http';
import {
  ApplicationRef,
  ComponentFactoryResolver,
  ComponentRef,
  EmbeddedViewRef,
  Inject,
  Injectable,
  Injector,
  RendererFactory2,
} from '@angular/core';
import { NavigationError, ResolveEnd, Router } from '@angular/router';
import { Actions, ofActionSuccessful } from '@ngxs/store';
import { Observable, Subject } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import snq from 'snq';
import { HttpErrorWrapperComponent } from '../components/http-error-wrapper/http-error-wrapper.component';
import { ErrorScreenErrorCodes, HttpErrorConfig } from '../models/common';
import { Confirmation } from '../models/confirmation';
import { ConfirmationService } from '../services/confirmation.service';

export const DEFAULT_ERROR_MESSAGES = {
  defaultError: {
    title: 'An error has occurred!',
    details: 'Error detail not sent by server.',
  },
  defaultError401: {
    title: 'You are not authenticated!',
    details: 'You should be authenticated (sign in) in order to perform this operation.',
  },
  defaultError403: {
    title: 'You are not authorized!',
    details: 'You are not allowed to perform this operation.',
  },
  defaultError404: {
    title: 'Resource not found!',
    details: 'The resource requested could not found on the server.',
  },
  defaultError500: {
    title: 'Internal server error',
    details: 'Error detail not sent by server.',
  },
};

export const DEFAULT_ERROR_LOCALIZATIONS = {
  defaultError: {
    title: 'AbpUi::DefaultErrorMessage',
    details: 'AbpUi::DefaultErrorMessageDetail',
  },
  defaultError401: {
    title: 'AbpUi::DefaultErrorMessage401',
    details: 'AbpUi::DefaultErrorMessage401Detail',
  },
  defaultError403: {
    title: 'AbpUi::DefaultErrorMessage403',
    details: 'AbpUi::DefaultErrorMessage403Detail',
  },
  defaultError404: {
    title: 'AbpUi::DefaultErrorMessage404',
    details: 'AbpUi::DefaultErrorMessage404Detail',
  },
  defaultError500: {
    title: 'AbpUi::500Message',
    details: 'AbpUi::DefaultErrorMessage',
  },
};

@Injectable({ providedIn: 'root' })
export class ErrorHandler {
  componentRef: ComponentRef<HttpErrorWrapperComponent>;

  constructor(
    private actions: Actions,
    private router: Router,
    private confirmationService: ConfirmationService,
    private cfRes: ComponentFactoryResolver,
    private rendererFactory: RendererFactory2,
    private injector: Injector,
    @Inject('HTTP_ERROR_CONFIG') private httpErrorConfig: HttpErrorConfig,
  ) {
    this.listenToRestError();
    this.listenToRouterError();
    this.listenToRouterDataResolved();
  }

  private listenToRouterError() {
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationError),
        filter(this.filterRouteErrors),
      )
      .subscribe(() => this.show404Page());
  }

  private listenToRouterDataResolved() {
    this.router.events
      .pipe(
        filter(event => event instanceof ResolveEnd),
        filter(() => !!this.componentRef),
      )
      .subscribe(() => {
        this.componentRef.destroy();
        this.componentRef = null;
      });
  }

  private listenToRestError() {
    this.actions
      .pipe(
        ofActionSuccessful(RestOccurError),
        map(action => action.payload),
        filter(this.filterRestErrors),
      )
      .subscribe(err => {
        const body = snq(() => err.error.error, {
          key: DEFAULT_ERROR_LOCALIZATIONS.defaultError.title,
          defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
        });

        if (err instanceof HttpErrorResponse && err.headers.get('_AbpErrorFormat')) {
          const confirmation$ = this.showError(null, null, body);

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
      });
  }

  private show401Page() {
    this.createErrorComponent({
      title: {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError401.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.title,
      },
      status: 401,
    });
  }

  private show404Page() {
    this.createErrorComponent({
      title: {
        key: DEFAULT_ERROR_LOCALIZATIONS.defaultError404.title,
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.title,
      },
      status: 404,
    });
  }

  private showError(
    message?: LocalizationParam,
    title?: LocalizationParam,
    body?: any,
  ): Observable<Confirmation.Status> {
    if (body) {
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
      }
    }

    return this.confirmationService.error(message, title, {
      hideCancelBtn: true,
      yesText: 'AbpAccount::Close',
    });
  }

  private navigateToLogin() {
    this.injector.get(AuthService).initLogin();
  }

  createErrorComponent(instance: Partial<HttpErrorWrapperComponent>) {
    const renderer = this.rendererFactory.createRenderer(null, null);
    const host = renderer.selectRootElement(document.body, true);

    this.componentRef = this.cfRes
      .resolveComponentFactory(HttpErrorWrapperComponent)
      .create(this.injector);

    for (const key in instance) {
      /* istanbul ignore else */
      if (this.componentRef.instance.hasOwnProperty(key)) {
        this.componentRef.instance[key] = instance[key];
      }
    }

    this.componentRef.instance.hideCloseIcon = this.httpErrorConfig.errorScreen.hideCloseIcon;
    const appRef = this.injector.get(ApplicationRef);

    if (this.canCreateCustomError(instance.status as ErrorScreenErrorCodes)) {
      this.componentRef.instance.cfRes = this.cfRes;
      this.componentRef.instance.appRef = appRef;
      this.componentRef.instance.injector = this.injector;
      this.componentRef.instance.customComponent = this.httpErrorConfig.errorScreen.component;
    }

    appRef.attachView(this.componentRef.hostView);
    renderer.appendChild(host, (this.componentRef.hostView as EmbeddedViewRef<any>).rootNodes[0]);

    const destroy$ = new Subject<void>();
    this.componentRef.instance.destroy$ = destroy$;
    destroy$.subscribe(() => {
      this.componentRef.destroy();
      this.componentRef = null;
    });
  }

  canCreateCustomError(status: ErrorScreenErrorCodes): boolean {
    return snq(
      () =>
        this.httpErrorConfig.errorScreen.component &&
        this.httpErrorConfig.errorScreen.forWhichErrors.indexOf(status) > -1,
    );
  }

  private filterRestErrors = ({ status }: HttpErrorResponse): boolean => {
    if (typeof status !== 'number') return false;

    return this.httpErrorConfig.skipHandledErrorCodes.findIndex(code => code === status) < 0;
  };

  private filterRouteErrors = (navigationError: NavigationError): boolean => {
    return (
      snq(() => navigationError.error.message.indexOf('Cannot match') > -1) &&
      this.httpErrorConfig.skipHandledErrorCodes.findIndex(code => code === 404) < 0
    );
  };
}
