import { registerLocaleData } from '@angular/common';
import { Injector, isDevMode } from '@angular/core';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { tap } from 'rxjs/operators';
import { GetAppConfiguration } from '../actions/config.actions';
import { ABP } from '../models/common';
import { AuthService } from '../services/auth.service';
import { ConfigState } from '../states/config.state';
import { clearOAuthStorage } from '../strategies/auth-flow.strategy';
import { LocaleErrorHandlerData, LOCALE_ERROR_HANDLER } from '../tokens/locale-error-handler.token';
import { CORE_OPTIONS } from '../tokens/options.token';
import { getRemoteEnv } from './environment-utils';
import { parseTenantFromUrl } from './multi-tenancy-utils';

export function getInitialData(injector: Injector) {
  const fn = async () => {
    const store: Store = injector.get(Store);
    const options = injector.get(CORE_OPTIONS) as ABP.Root;

    await getRemoteEnv(injector, options.environment);
    await parseTenantFromUrl(injector);
    await injector.get(AuthService).init();

    if (options.skipGetAppConfiguration) return;

    return store
      .dispatch(new GetAppConfiguration())
      .pipe(tap(res => checkAccessToken(store, injector)))
      .toPromise();
  };

  return fn;
}

export function checkAccessToken(store: Store, injector: Injector) {
  const oAuth = injector.get(OAuthService);
  if (oAuth.hasValidAccessToken() && !store.selectSnapshot(ConfigState.getDeep('currentUser.id'))) {
    clearOAuthStorage();
  }
}

export function localeInitializer(injector: Injector) {
  const fn = () => {
    const store: Store = injector.get(Store);

    const lang = store.selectSnapshot(state => state.SessionState.language) || 'en';

    return new Promise((resolve, reject) => {
      registerLocale(lang, injector).then(() => resolve('resolved'), reject);
    });
  };

  return fn;
}

export function registerLocale(locale: string, injector: Injector): Promise<any> {
  const { cultureNameLocaleFileMap } = injector.get(CORE_OPTIONS, {} as ABP.Root);

  const errorHandlerFn = injector.get(LOCALE_ERROR_HANDLER, defaultLocalErrorHandlerFn);

  return new Promise((resolve, reject) => {
    return import(
      /* webpackChunkName: "_locale-[request]"*/
      /* webpackInclude: /\/(ar|cs|en|fr|pt|tr|ru|sl|zh-Hans|zh-Hant).js/ */
      /* webpackExclude: /\/global|\/extra/ */
      `@angular/common/locales/${cultureNameLocaleFileMap[locale] || locale}.js`
    )
      .then(module => {
        registerLocaleData(module.default, locale);
        resolve(module.default);
      })
      .catch(error => {
        errorHandlerFn({
          resolve,
          reject,
          error,
          injector,
          locale,
          storedLocales: { ...extraLocales },
        });
      });
  });
}

const extraLocales = {};
export function storeLocaleData(data: any, localeId: string) {
  extraLocales[localeId] = data;
}

async function defaultLocalErrorHandlerFn({
  locale,
  storedLocales,
  resolve,
  injector,
}: LocaleErrorHandlerData) {
  if (storedLocales[locale]) {
    registerLocaleData(storedLocales[locale], locale);
    resolve();
    return;
  }

  if (isDevMode) {
    console.error(`Cannot find the ${locale} locale file. You can check how can add new culture at https://docs.abp.io/en/abp/latest/UI/Angular/Localization#adding-new-culture`);
  }

  resolve();
}
