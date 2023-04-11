import { inject, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthStorage, TokenResponse } from 'angular-oauth2-oidc';
import { pipe } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import {
  ConfigStateService,
  LoginParams,
  PipeToLoginFn,
  SetTokenResponseToStorageFn,
} from '@abp/ng.core';

const cookieKey = 'rememberMe';
const storageKey = 'passwordFlow';

export const pipeToLogin: PipeToLoginFn = function (
  params: Pick<LoginParams, 'redirectUrl' | 'rememberMe'>,
  injector: Injector,
) {
  const configState = injector.get(ConfigStateService);
  const router = injector.get(Router);

  return pipe(
    switchMap(() => configState.refreshAppState()),
    tap(() => {
      setRememberMe(params.rememberMe);
      if (params.redirectUrl) router.navigate([params.redirectUrl]);
    }),
  );
};

export function setRememberMe(remember: boolean | undefined) {
  removeRememberMe();
  localStorage.setItem(storageKey, 'true');
  document.cookie = `${cookieKey}=true; path=/${
    remember ? ' ;expires=Fri, 31 Dec 9999 23:59:59 GMT' : ''
  }`;
}

export function removeRememberMe() {
  localStorage.removeItem(storageKey);
  document.cookie = cookieKey + '= ; path=/; expires = Thu, 01 Jan 1970 00:00:00 GMT';
}
