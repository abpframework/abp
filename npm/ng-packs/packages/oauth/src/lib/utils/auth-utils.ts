import { Injector } from '@angular/core';
import { Router } from '@angular/router';
import { pipe } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import {
  ConfigStateService,
  LoginParams,
  PipeToLoginFn,
  AbpLocalStorageService,
} from '@abp/ng.core';

const cookieKey = 'rememberMe';
const storageKey = 'passwordFlow';
const remember_me = 'remember_me';

export const pipeToLogin: PipeToLoginFn = function (
  params: Pick<LoginParams, 'redirectUrl' | 'rememberMe'>,
  injector: Injector,
) {
  const configState = injector.get(ConfigStateService);
  const router = injector.get(Router);
  const localStorage = injector.get(AbpLocalStorageService);
  return pipe(
    switchMap(() => configState.refreshAppState()),
    tap(() => {
      setRememberMe(params.rememberMe, localStorage);
      if (params.redirectUrl) router.navigate([params.redirectUrl]);
    }),
  );
};

export function setRememberMe(
  remember: boolean | undefined,
  localStorageService: AbpLocalStorageService,
) {
  console.log('set Remember Me');
  localStorageService.setItem(remember_me, JSON.stringify(remember));
}

export function removeRememberMe(localStorageService: AbpLocalStorageService) {
  console.log('remove remember Me');
  localStorageService.removeItem(remember_me);
}

export function getRememberMe(localStorageService: AbpLocalStorageService){
  return localStorageService.getItem(remember_me);
}
