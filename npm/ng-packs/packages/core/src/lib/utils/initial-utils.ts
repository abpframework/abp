import { registerLocaleData } from '@angular/common';
import { Injector } from '@angular/core';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { tap } from 'rxjs/operators';
import { GetAppConfiguration } from '../actions/config.actions';
import { ABP } from '../models/common';
import { ConfigState } from '../states/config.state';
import { CORE_OPTIONS } from '../tokens/options.token';

export function configureOAuth(injector: Injector, options: ABP.Root) {
  const fn = () => {
    const oAuth = injector.get(OAuthService);
    oAuth.configure(options.environment.oAuthConfig);
    return Promise.resolve();
  };

  return fn;
}

export function getInitialData(injector: Injector) {
  const fn = () => {
    const store: Store = injector.get(Store);
    const { skipGetAppConfiguration } = injector.get(CORE_OPTIONS) as ABP.Root;

    if (skipGetAppConfiguration) return;

    return store
      .dispatch(new GetAppConfiguration())
      .pipe(tap(res => checkAccessToken(store, injector)))
      .toPromise();
  };

  return fn;
}

function checkAccessToken(store: Store, injector: Injector) {
  const oAuth = injector.get(OAuthService);
  if (oAuth.hasValidAccessToken() && !store.selectSnapshot(ConfigState.getDeep('currentUser.id'))) {
    oAuth.logOut();
  }
}

export function localeInitializer(injector: Injector) {
  const fn = () => {
    const store: Store = injector.get(Store);
    const options = injector.get(CORE_OPTIONS);

    const lang = store.selectSnapshot(state => state.SessionState.language) || 'en';

    return new Promise((resolve, reject) => {
      registerLocale(lang, options.cultureNameToLocaleFileNameMapping).then(
        () => resolve('resolved'),
        reject,
      );
    });
  };

  return fn;
}

export function registerLocale(locale: string, localeNameMap: ABP.Dictionary<string>) {
  return import(
    /* webpackChunkName: "_locale-[request]"*/
    `@angular/common/locales/${localeNameMap[locale] || locale}.js`
  ).then(module => {
    registerLocaleData(module.default);
  });
}
