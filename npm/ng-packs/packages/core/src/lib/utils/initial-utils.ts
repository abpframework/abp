import { registerLocaleData } from '@angular/common';
import { inject, Injector } from '@angular/core';
import { tap, catchError } from 'rxjs/operators';
import { lastValueFrom, throwError } from 'rxjs';
import { ABP } from '../models/common';
import { Environment } from '../models/environment';
import { CurrentTenantDto } from '../proxy/volo/abp/asp-net-core/mvc/multi-tenancy/models';
import { ConfigStateService } from '../services/config-state.service';
import { EnvironmentService } from '../services/environment.service';
import { SessionStateService } from '../services/session-state.service';
import { CORE_OPTIONS } from '../tokens/options.token';
import { APP_INIT_ERROR_HANDLERS } from '../tokens/app-config.token';
import { getRemoteEnv } from './environment-utils';
import { parseTenantFromUrl } from './multi-tenancy-utils';
import { AuthService } from '../abstracts';
import { CHECK_AUTHENTICATION_STATE_FN_KEY } from '../tokens/check-authentication-state';
import { noop } from './common-utils';

export async function getInitialData() {
  const injector = inject(Injector);
  const environmentService = injector.get(EnvironmentService);
  const configState = injector.get(ConfigStateService);
  const options = injector.get(CORE_OPTIONS) as ABP.Root;

  environmentService.setState(options.environment as Environment);
  await getRemoteEnv(injector, options.environment);
  await parseTenantFromUrl(injector);
  const authService = injector.get(AuthService, undefined, { optional: true });
  const checkAuthenticationState = injector.get(CHECK_AUTHENTICATION_STATE_FN_KEY, noop, {
    optional: true,
  });
  if (!options.skipInitAuthService && authService) {
    await authService.init();
  }
  if (options.skipGetAppConfiguration) return;

  const result$ = configState.refreshAppState().pipe(
    tap(() => checkAuthenticationState(injector)),
    tap(() => {
      const currentTenant = configState.getOne('currentTenant') as CurrentTenantDto;
      injector.get(SessionStateService).setTenant(currentTenant);
    }),
    catchError(error => {
      const appInitErrorHandlers = injector.get(APP_INIT_ERROR_HANDLERS, null);
      if (appInitErrorHandlers && appInitErrorHandlers.length) {
        appInitErrorHandlers.forEach(func => func(error));
      }

      return throwError(() => error);
    }),
  );
  // TODO: Not working with SSR
  // await lastValueFrom(result$);
  await localeInitializer(injector);
}

export function localeInitializer(injector?: Injector) {
  const currentInjector = injector || inject(Injector);
  const sessionState = currentInjector.get(SessionStateService);
  const { registerLocaleFn }: ABP.Root = currentInjector.get(CORE_OPTIONS);

  const lang = sessionState.getLanguage() || 'en';

  return new Promise((resolve, reject) => {
    registerLocaleFn(lang).then(module => {
      if (module?.default) registerLocaleData(module.default);

      return resolve('resolved');
    }, reject);
  });
}
