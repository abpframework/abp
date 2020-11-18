import { registerLocaleData } from '@angular/common';
import { Injector } from '@angular/core';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { tap } from 'rxjs/operators';
import { ApplicationConfiguration } from '../models/application-configuration';
import { ABP } from '../models/common';
import { Environment } from '../models/environment';
import { ApplicationConfigurationService } from '../services/application-configuration.service';
import { AuthService } from '../services/auth.service';
import { ConfigStateService } from '../services/config-state.service';
import { EnvironmentService } from '../services/environment.service';
import { SessionStateService } from '../services/session-state.service';
import { clearOAuthStorage } from '../strategies/auth-flow.strategy';
import { CORE_OPTIONS } from '../tokens/options.token';
import { getRemoteEnv } from './environment-utils';
import { parseTenantFromUrl } from './multi-tenancy-utils';

export function getInitialData(injector: Injector) {
  const fn = async () => {
    const environmentService = injector.get(EnvironmentService);
    const configState = injector.get(ConfigStateService);
    const appConfigService = injector.get(ApplicationConfigurationService);
    const options = injector.get(CORE_OPTIONS) as ABP.Root;

    environmentService.setState(options.environment as Environment);
    await getRemoteEnv(injector, options.environment);
    await parseTenantFromUrl(injector);
    await injector.get(AuthService).init();

    if (options.skipGetAppConfiguration) return;

    return appConfigService
      .getConfiguration()
      .pipe(
        tap(res => configState.setState(res)),
        tap(() => checkAccessToken(injector)),
        tap(() => {
          const currentTenant = configState.getOne(
            'currentTenant',
          ) as ApplicationConfiguration.CurrentTenant;
          if (!currentTenant?.id) return;

          injector.get(SessionStateService).setTenant(currentTenant);
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
