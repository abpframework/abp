import { registerLocaleData } from '@angular/common';
import { Injector } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { tap, catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { ABP } from '../models/common';
import { Environment } from '../models/environment';
import { AbpApplicationConfigurationService } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/abp-application-configuration.service';
import { CurrentTenantDto } from '../proxy/volo/abp/asp-net-core/mvc/multi-tenancy/models';
import { AuthService } from '../services/auth.service';
import { ConfigStateService } from '../services/config-state.service';
import { EnvironmentService } from '../services/environment.service';
import { SessionStateService } from '../services/session-state.service';
import { clearOAuthStorage } from '../strategies/auth-flow.strategy';
import { CORE_OPTIONS } from '../tokens/options.token';
import { APP_INIT_ERROR_HANDLERS } from '../tokens/app-config.token';
import { getRemoteEnv } from './environment-utils';
import { parseTenantFromUrl } from './multi-tenancy-utils';

export function getInitialData(injector: Injector) {
  const fn = async () => {
    const environmentService = injector.get(EnvironmentService);
    const configState = injector.get(ConfigStateService);
    const appConfigService = injector.get(AbpApplicationConfigurationService);
    const options = injector.get(CORE_OPTIONS) as ABP.Root;

    environmentService.setState(options.environment as Environment);
    await getRemoteEnv(injector, options.environment);
    await parseTenantFromUrl(injector);
    await injector.get(AuthService).init();

    if (options.skipGetAppConfiguration) return;

    return appConfigService
      .get()
      .pipe(
        tap(res => configState.setState(res)),
        tap(() => checkAccessToken(injector)),
        tap(() => {
          const currentTenant = configState.getOne('currentTenant') as CurrentTenantDto;
          injector.get(SessionStateService).setTenant(currentTenant);
        }),
        catchError(error => {
          const appInitErrorHandlers = injector.get(APP_INIT_ERROR_HANDLERS, null);
          if (appInitErrorHandlers && appInitErrorHandlers.length) {
            appInitErrorHandlers.forEach(func => func(error));
          }

          return throwError(error);
        }),
      )
      .toPromise();
  };

  return fn;
}

export function checkAccessToken(injector: Injector) {
  const configState = injector.get(ConfigStateService);
  const oAuth = injector.get(OAuthService);
  if (oAuth.hasValidAccessToken() && !configState.getDeep('currentUser.id')) {
    clearOAuthStorage();
  }
}

export function localeInitializer(injector: Injector) {
  const fn = () => {
    const sessionState = injector.get(SessionStateService);
    const { registerLocaleFn }: ABP.Root = injector.get(CORE_OPTIONS);

    const lang = sessionState.getLanguage() || 'en';

    return new Promise((resolve, reject) => {
      registerLocaleFn(lang).then(module => {
        if (module?.default) registerLocaleData(module.default);

        return resolve('resolved');
      }, reject);
    });
  };

  return fn;
}
