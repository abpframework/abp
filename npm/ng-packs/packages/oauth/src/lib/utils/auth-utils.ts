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
import { OAuthService } from 'angular-oauth2-oidc';

const rememberMe = 'remember_me';

export class RememberMeService {
  constructor(private injector: Injector) { }
  localStorageService = this.injector.get(AbpLocalStorageService);

  setRememberMe(remember: boolean) {
    this.localStorageService.setItem(rememberMe, JSON.stringify(remember));
  }

  removeRememberMe() {
    this.localStorageService.removeItem(rememberMe);
  }

  getRememberMe() {
    return this.localStorageService.getItem(rememberMe);
  }
}

export const pipeToLogin: PipeToLoginFn = function (
  params: Pick<LoginParams, 'redirectUrl' | 'rememberMe'>,
  injector: Injector,
) {
  const configState = injector.get(ConfigStateService);
  const router = injector.get(Router);
  const rememberMeService = new RememberMeService(injector);
  return pipe(
    switchMap(() => configState.refreshAppState()),
    tap(() => {
      rememberMeService.setRememberMe(params.rememberMe);
      if (params.redirectUrl) router.navigate([params.redirectUrl]);
    }),
  );
};

export function isTokenExpired(oAuthService: OAuthService): boolean {
  const expireDate = oAuthService.getAccessTokenExpiration();
  const currentDate = new Date().getTime();
  return expireDate < currentDate;
}
