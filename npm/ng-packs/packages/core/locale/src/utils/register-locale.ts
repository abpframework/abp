import { differentLocales } from '@abp/ng.core';
import { registerLocaleData } from '@angular/common';
import { Injector, isDevMode } from '@angular/core';

export interface LocaleErrorHandlerData {
  resolve: any;
  reject: any;
  error: any;
  locale: string;
  injector: Injector;
}

export interface RegisterLocaleData {
  cultureNameLocaleFileMap: Record<string, string>;
  errorHandlerFn: (data: LocaleErrorHandlerData) => any;
}

export function registerLocale(
  {
    cultureNameLocaleFileMap = {},
    errorHandlerFn = defaultLocalErrorHandlerFn,
  } = {} as RegisterLocaleData,
) {
  return (locale: string, injector: Injector): Promise<any> => {
    cultureNameLocaleFileMap = { ...differentLocales, ...cultureNameLocaleFileMap };

    return new Promise((resolve, reject) => {
      return import(
        /* webpackChunkName: "_locale-[request]"*/
        /* webpackInclude: /[/\\](ar|cs|en|fr|pt|tr|ru|hu|sl|zh-Hans|zh-Hant).js/ */
        /* webpackExclude: /[/\\]global|extra/ */
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
          });
        });
    });
  };
}

const extraLocales = {};
export function storeLocaleData(data: any, localeId: string) {
  extraLocales[localeId] = data;
}

export async function defaultLocalErrorHandlerFn({
  locale,
  resolve,
  injector,
}: LocaleErrorHandlerData) {
  if (extraLocales[locale]) {
    registerLocaleData(extraLocales[locale], locale);
    resolve();
    return;
  }

  if (isDevMode) {
    console.error(
      `Cannot find the ${locale} locale file. You can check how can add new culture at https://docs.abp.io/en/abp/latest/UI/Angular/Localization#adding-a-new-culture`,
    );
  }

  resolve();
}
