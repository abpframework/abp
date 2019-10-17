import { RestOccurError } from '@abp/ng.core';
import { HttpErrorResponse } from '@angular/common/http';
import {
  ApplicationRef,
  ComponentFactoryResolver,
  EmbeddedViewRef,
  Injectable,
  Injector,
  NgZone,
  RendererFactory2,
} from '@angular/core';
import { Router } from '@angular/router';
import { Actions, ofActionSuccessful, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import snq from 'snq';
import { ErrorComponent } from '../components/error/error.component';
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
    title: '500',
    details: { key: 'AbpAccount::InternalServerErrorMessage', defaultValue: 'Error detail not sent by server.' },
  },
  defaultErrorUnknown: {
    title: 'Unknown Error',
    details: { key: 'AbpAccount::InternalServerErrorMessage', defaultValue: 'Error detail not sent by server.' },
  },
};

@Injectable({ providedIn: 'root' })
export class ErrorHandler {
  constructor(
    private actions: Actions,
    private router: Router,
    private ngZone: NgZone,
    private store: Store,
    private confirmationService: ConfirmationService,
    private appRef: ApplicationRef,
    private cfRes: ComponentFactoryResolver,
    private rendererFactory: RendererFactory2,
    private injector: Injector,
  ) {
    actions.pipe(ofActionSuccessful(RestOccurError)).subscribe(res => {
      const { payload: err = {} as HttpErrorResponse | any } = res;
      const body = snq(() => (err as HttpErrorResponse).error.error, DEFAULT_ERROR_MESSAGES.defaultError.title);

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
            this.showError(
              DEFAULT_ERROR_MESSAGES.defaultError401.details,
              DEFAULT_ERROR_MESSAGES.defaultError401.title,
            ).subscribe(() => this.navigateToLogin());
            break;
          case 403:
            this.createErrorComponent({
              title: DEFAULT_ERROR_MESSAGES.defaultError403.title,
              details: DEFAULT_ERROR_MESSAGES.defaultError403.details,
            });
            break;
          case 404:
            this.showError(
              DEFAULT_ERROR_MESSAGES.defaultError404.details,
              DEFAULT_ERROR_MESSAGES.defaultError404.title,
            );
            break;
          case 500:
            this.createErrorComponent({
              title: DEFAULT_ERROR_MESSAGES.defaultError500.title,
              details: DEFAULT_ERROR_MESSAGES.defaultError500.details,
            });
            break;
          case 0:
            if ((err as HttpErrorResponse).statusText === 'Unknown Error') {
              this.createErrorComponent({
                title: DEFAULT_ERROR_MESSAGES.defaultErrorUnknown.title,
                details: DEFAULT_ERROR_MESSAGES.defaultErrorUnknown.details,
              });
            }
            break;
          default:
            this.showError(DEFAULT_ERROR_MESSAGES.defaultError.details, DEFAULT_ERROR_MESSAGES.defaultError.title);
            break;
        }
      }
    });
  }

  private showError(message?: string, title?: string, body?: any): Observable<Toaster.Status> {
    if (body) {
      if (body.details) {
        message = body.details;
        title = body.message;
      } else {
        message = body.message || DEFAULT_ERROR_MESSAGES.defaultError.title;
      }
    }

    return this.confirmationService.error(message, title, {
      hideCancelBtn: true,
      yesCopy: 'OK',
    });
  }

  private navigateToLogin() {
    this.ngZone.run(() => {
      this.router.navigate(['/account/login'], {
        state: { redirectUrl: this.router.url },
      });
    });
  }

  createErrorComponent(instance: Partial<ErrorComponent>) {
    const renderer = this.rendererFactory.createRenderer(null, null);
    const host = renderer.selectRootElement(document.body, true);

    const componentRef = this.cfRes.resolveComponentFactory(ErrorComponent).create(this.injector);

    for (const key in componentRef.instance) {
      if (componentRef.instance.hasOwnProperty(key)) {
        componentRef.instance[key] = instance[key];
      }
    }

    this.appRef.attachView(componentRef.hostView);
    renderer.appendChild(host, (componentRef.hostView as EmbeddedViewRef<any>).rootNodes[0]);

    componentRef.instance.renderer = renderer;
    componentRef.instance.elementRef = componentRef.location;
    componentRef.instance.host = host;
  }
}
