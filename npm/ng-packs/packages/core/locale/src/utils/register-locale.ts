import { differentLocales } from '@abp/ng.core';
import { isDevMode } from '@angular/core';

export interface LocaleErrorHandlerData {
  resolve: any;
  reject: any;
  error: any;
  locale: string;
}

let localeMap = {};

export interface RegisterLocaleData {
  cultureNameLocaleFileMap?: Record<string, string>;
  errorHandlerFn?: (data: LocaleErrorHandlerData) => any;
}

export function registerLocale(
  {
    cultureNameLocaleFileMap = {},
    errorHandlerFn = defaultLocalErrorHandlerFn,
  } = {} as RegisterLocaleData,
) {
  return (locale: string): Promise<any> => {
    localeMap = { ...differentLocales, ...cultureNameLocaleFileMap };

    return new Promise((resolve, reject) => {
      return import(
        /* webpackMode: "lazy-once" */
        /* webpackChunkName: "locales"*/
        /* webpackInclude: /[/\\](ar|cs|en|en-GB|es|de|fr|pt|tr|ru|hu|sl|zh-Hans|zh-Hant).js/ */
        /* webpackExclude: /[/\\]global|extra/ */
        `@angular/common/locales/${localeMap[locale] || locale}.js`
      )
        .then(resolve)
        .catch(error => {
          errorHandlerFn({
            resolve,
            reject,
            error,
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

export async function defaultLocalErrorHandlerFn({ locale, resolve }: LocaleErrorHandlerData) {
  if (extraLocales[locale]) {
    resolve({ default: extraLocales[localeMap[locale] || locale] });
    return;
  }

  if (isDevMode) {
    console.error(
      `Cannot find the ${locale} locale file. You can check how can add new culture at https://docs.abp.io/en/abp/latest/UI/Angular/Localization#adding-a-new-culture`,
    );
  }

  resolve();
}
