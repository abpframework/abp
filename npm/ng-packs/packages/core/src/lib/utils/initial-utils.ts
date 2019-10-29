import { registerLocaleData } from '@angular/common';
import { Injector } from '@angular/core';
import { Store } from '@ngxs/store';
import { GetAppConfiguration } from '../actions/config.actions';
import differentLocales from '../constants/different-locales';

export function getInitialData(injector: Injector) {
  const fn = () => {
    const store: Store = injector.get(Store);

    return store.dispatch(new GetAppConfiguration()).toPromise();
  };

  return fn;
}

export function localeInitializer(injector: Injector) {
  const fn = () => {
    const store: Store = injector.get(Store);

    const lang = store.selectSnapshot(state => state.SessionState.language) || 'en';

    return new Promise((resolve, reject) => {
      registerLocale(lang).then(() => resolve('resolved'), reject);
    });
  };

  return fn;
}

export function registerLocale(locale: string) {
  return import(
    /* webpackInclude: /(af|am|ar-SA|as|az-Latn|be|bg|bn-BD|bn-IN|bs|ca|ca-ES-VALENCIA|cs|cy|da|de|de|el|en-GB|en|es|en|es-US|es-MX|et|eu|fa|fi|en|fr|fr|fr-CA|ga|gd|gl|gu|ha|he|hi|hr|hu|hy|id|ig|is|it|it|ja|ka|kk|km|kn|ko|kok|en|en|lb|lt|lv|en|mk|ml|mn|mr|ms|mt|nb|ne|nl|nl-BE|nn|en|or|pa|pa-Arab|pl|en|pt|pt-PT|en|en|ro|ru|rw|pa-Arab|si|sk|sl|sq|sr-Cyrl-BA|sr-Cyrl|sr-Latn|sv|sw|ta|te|tg|th|ti|tk|tn|tr|tt|ug|uk|ur|uz-Latn|vi|wo|xh|yo|zh-Hans|zh-Hant|zu)\.js$/ */
    `@angular/common/locales/${differentLocales[locale] || locale}.js`
  ).then(module => {
    registerLocaleData(module.default);
  });
}
