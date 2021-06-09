import { Injector } from '@angular/core';
import { Router } from '@angular/router';
import { pipe } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { LoginParams } from '../models/auth';
import { AbpApplicationConfigurationService } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/abp-application-configuration.service';
import { ConfigStateService } from '../services/config-state.service';

const cookieKey = 'rememberMe';
const storageKey = 'passwordFlow';

export function pipeToLogin(
  params: Pick<LoginParams, 'redirectUrl' | 'rememberMe'>,
  injector: Injector,
) {
  const configState = injector.get(ConfigStateService);
  const appConfigService = injector.get(AbpApplicationConfigurationService);
  const router = injector.get(Router);

  return pipe(
    switchMap(() => appConfigService.get()),
    tap(res => {
      configState.setState(res);
      setRememberMe(params.rememberMe);
      if (params.redirectUrl) router.navigate([params.redirectUrl]);
    }),
  );
}

export function setRememberMe(remember: boolean) {
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
