import { Injector } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { pipe } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import {
  AuthService,
  ConfigStateService,
  LoginParams,
  PipeToLoginFn,
} from '@abp/ng.core';
import { RememberMeService } from '../services/remember-me.service';

export const pipeToLogin: PipeToLoginFn = function (
  params: Pick<LoginParams, 'redirectUrl' | 'rememberMe'>,
  injector: Injector,
) {
  const configState = injector.get(ConfigStateService);
  const router = injector.get(Router);
  const rememberMeService = injector.get(RememberMeService);
  const authService = injector.get(AuthService);
  return pipe(
    switchMap(() => configState.refreshAppState()),
    tap(() => {
      rememberMeService.set(
        params.rememberMe ||
        rememberMeService.get() ||
        rememberMeService.getFromToken(authService.getAccessToken())
      );
      if (params.redirectUrl) router.navigate([params.redirectUrl]);
    }),
  );
};

/**
 * True when a token request failed on the way (no connection or a server error), so the result
 * doesn't tell whether the tokens are still valid.
 */
export function isTransientTokenError(error: unknown): boolean {
  return error instanceof HttpErrorResponse && (error.status === 0 || error.status >= 500);
}

//Ref: https://github.com/manfredsteyer/angular-oauth2-oidc/issues/1214
export function isTokenExpired(expireDate: number): boolean {
  const currentDate = new Date().getTime();
  return expireDate < currentDate;
}
