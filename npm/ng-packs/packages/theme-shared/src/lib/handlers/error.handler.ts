import { RestOccurError } from '@abp/ng.core';
import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Navigate, RouterState } from '@ngxs/router-plugin';
import { Actions, ofActionSuccessful, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { Toaster } from '../models/toaster';
import { ConfirmationService } from '../services/confirmation.service';

const DEFAULTS = {
  defaultError: {
    message: 'An error has occurred!',
    details: 'Error detail not sent by server.',
  },

  defaultError401: {
    message: 'You are not authenticated!',
    details: 'You should be authenticated (sign in) in order to perform this operation.',
  },

  defaultError403: {
    message: 'You are not authorized!',
    details: 'You are not allowed to perform this operation.',
  },

  defaultError404: {
    message: 'Resource not found!',
    details: 'The resource requested could not found on the server.',
  },
};

@Injectable({ providedIn: 'root' })
export class ErrorHandler {
  constructor(private actions: Actions, private store: Store, private confirmationService: ConfirmationService) {
    actions.pipe(ofActionSuccessful(RestOccurError)).subscribe(res => {
      const { payload: err = {} as HttpErrorResponse | any } = res;
      const body = (err as HttpErrorResponse).error.error;

      if (err.headers.get('_AbpErrorFormat')) {
        const confirmation$ = this.showError(null, null, body);

        if (err.status === 401) {
          confirmation$.subscribe(() => {
            this.navigateToLogin();
          });
        }
      } else {
        switch ((err as HttpErrorResponse).status) {
          case 401:
            this.showError(DEFAULTS.defaultError401.details, DEFAULTS.defaultError401.message).subscribe(() =>
              this.navigateToLogin(),
            );
            break;
          case 403:
            this.showError(DEFAULTS.defaultError403.details, DEFAULTS.defaultError403.message);
            break;
          case 404:
            this.showError(DEFAULTS.defaultError404.details, DEFAULTS.defaultError404.message);
            break;
          default:
            this.showError(DEFAULTS.defaultError.details, DEFAULTS.defaultError.message);
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
        message = body.message || DEFAULTS.defaultError.message;
      }
    }

    return this.confirmationService.error(message, title, {
      hideCancelBtn: true,
      yesCopy: 'OK',
    });
  }

  private navigateToLogin() {
    this.store.dispatch(
      new Navigate(['/account/login'], null, {
        state: { redirectUrl: this.store.selectSnapshot(RouterState).state.url },
      }),
    );
  }
}
