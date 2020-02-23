import { Config, RestOccurError } from '@abp/ng.core';
import { HttpErrorResponse } from '@angular/common/http';
import {
  ApplicationRef,
  ComponentFactoryResolver,
  EmbeddedViewRef,
  Inject,
  Injectable,
  Injector,
  RendererFactory2,
  Type,
  ComponentRef,
} from '@angular/core';
import { Navigate, RouterError, RouterState, RouterDataResolved } from '@ngxs/router-plugin';
import { Actions, ofActionSuccessful, Store } from '@ngxs/store';
import { Observable, Subject } from 'rxjs';
import snq from 'snq';
import { HttpErrorWrapperComponent } from '../components/http-error-wrapper/http-error-wrapper.component';
import { HttpErrorConfig, ErrorScreenErrorCodes } from '../models/common';
import { Toaster } from '../models/toaster';
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

@Injectable({ providedIn: 'root' })
export class ErrorHandler {
  componentRef: ComponentRef<HttpErrorWrapperComponent>;

  constructor(
    private actions: Actions,
    private store: Store,
    private confirmationService: ConfirmationService,
    private appRef: ApplicationRef,
    private cfRes: ComponentFactoryResolver,
    private rendererFactory: RendererFactory2,
    private injector: Injector,
    @Inject('HTTP_ERROR_CONFIG') private httpErrorConfig: HttpErrorConfig,
  ) {
    this.actions
      .pipe(ofActionSuccessful(RestOccurError, RouterError, RouterDataResolved))
      .subscribe(res => {
        if (res instanceof RestOccurError) {
          const { payload: err = {} as HttpErrorResponse | any } = res;
          const body = snq(
            () => (err as HttpErrorResponse).error.error,
            DEFAULT_ERROR_MESSAGES.defaultError.title,
          );

          if (err instanceof HttpErrorResponse && err.headers.get('_AbpErrorFormat')) {
            const confirmation$ = this.showError(null, null, body);

            if (err.status === 401) {
              confirmation$.subscribe(() => {
                this.navigateToLogin();
              });
            }
          } else {
            switch ((err as HttpErrorResponse).status) {
              case 401:
                this.canCreateCustomError(401)
                  ? this.show401Page()
                  : this.showError(
                      {
                        key: 'AbpAccount::DefaultErrorMessage401',
                        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.title,
                      },
                      {
                        key: 'AbpAccount::DefaultErrorMessage401Detail',
                        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.details,
                      },
                    ).subscribe(() => this.navigateToLogin());
                break;
              case 403:
                this.createErrorComponent({
                  title: {
                    key: 'AbpAccount::DefaultErrorMessage403',
                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError403.title,
                  },
                  details: {
                    key: 'AbpAccount::DefaultErrorMessage403Detail',
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
                        key: 'AbpAccount::DefaultErrorMessage404',
                        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.details,
                      },
                      {
                        key: 'AbpAccount::DefaultErrorMessage404Detail',
                        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.title,
                      },
                    );
                break;
              case 500:
                this.createErrorComponent({
                  title: {
                    key: 'AbpAccount::500Message',
                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError500.title,
                  },
                  details: {
                    key: 'AbpAccount::InternalServerErrorMessage',
                    defaultValue: DEFAULT_ERROR_MESSAGES.defaultError500.details,
                  },
                  status: 500,
                });
                break;
              case 0:
                if ((err as HttpErrorResponse).statusText === 'Unknown Error') {
                  this.createErrorComponent({
                    title: {
                      key: 'AbpAccount::DefaultErrorMessage',
                      defaultValue: DEFAULT_ERROR_MESSAGES.defaultError.title,
                    },
                  });
                }
                break;
              default:
                this.showError(
                  DEFAULT_ERROR_MESSAGES.defaultError.details,
                  DEFAULT_ERROR_MESSAGES.defaultError.title,
                );
                break;
            }
          }
        } else if (
          res instanceof RouterError &&
          snq(() => res.event.error.indexOf('Cannot match') > -1, false)
        ) {
          this.show404Page();
        } else if (res instanceof RouterDataResolved && this.componentRef) {
          this.componentRef.destroy();
          this.componentRef = null;
        }
      });
  }

  private show401Page() {
    this.createErrorComponent({
      title: {
        key: 'AbpAccount::401Message',
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError401.title,
      },
      status: 401,
    });
  }

  private show404Page() {
    this.createErrorComponent({
      title: {
        key: 'AbpAccount::404Message',
        defaultValue: DEFAULT_ERROR_MESSAGES.defaultError404.title,
      },
      status: 404,
    });
  }

  private showError(
    message?: Config.LocalizationParam,
    title?: Config.LocalizationParam,
    body?: any,
  ): Observable<Toaster.Status> {
    if (body) {
      if (body.details) {
        message = body.details;
        title = body.message;
      } else if (body.message) {
        title = DEFAULT_ERROR_MESSAGES.defaultError.title;
        message = body.message;
      } else {
        message = body.message || DEFAULT_ERROR_MESSAGES.defaultError.title;
      }
    }

    return this.confirmationService.error(message, title, {
      hideCancelBtn: true,
      yesText: 'AbpAccount::Close',
    });
  }

  private navigateToLogin() {
    this.store.dispatch(
      new Navigate(['/account/login'], null, {
        state: { redirectUrl: this.store.selectSnapshot(RouterState.url) },
      }),
    );
  }

  createErrorComponent(instance: Partial<HttpErrorWrapperComponent>) {
    const renderer = this.rendererFactory.createRenderer(null, null);
    const host = renderer.selectRootElement(document.body, true);

    this.componentRef = this.cfRes
      .resolveComponentFactory(HttpErrorWrapperComponent)
      .create(this.injector);

    for (const key in this.componentRef.instance) {
      if (this.componentRef.instance.hasOwnProperty(key)) {
        this.componentRef.instance[key] = instance[key];
      }
    }
    this.componentRef.instance.hideCloseIcon = this.httpErrorConfig.errorScreen.hideCloseIcon;
    if (this.canCreateCustomError(instance.status as ErrorScreenErrorCodes)) {
      this.componentRef.instance.cfRes = this.cfRes;
      this.componentRef.instance.appRef = this.appRef;
      this.componentRef.instance.injector = this.injector;
      this.componentRef.instance.customComponent = this.httpErrorConfig.errorScreen.component;
    }

    this.appRef.attachView(this.componentRef.hostView);
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
}
