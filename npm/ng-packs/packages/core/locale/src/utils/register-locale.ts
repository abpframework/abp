import { differentLocales } from '@abp/ng.core';
import { isDevMode } from '@angular/core';

export interface LocaleErrorHandlerData {
  resolve: any;
  reject: any;
  error: any;
  locale: string;
}

let localeMap = {} as { [key: string]: string };

export interface RegisterLocaleData {
  cultureNameLocaleFileMap?: Record<string, string>;
  errorHandlerFn?: (data: LocaleErrorHandlerData) => any;
}


function loadLocale(locale: string) {
  // hard coded list works with esbuild. Source https://github.com/angular/angular-cli/issues/26904#issuecomment-1903596563

  const list = {
      'ar': () => import('@angular/common/locales/ar'),
      'cs': () => import('@angular/common/locales/cs'),
      'en': () => import('@angular/common/locales/en'),
      'en-GB': () => import('@angular/common/locales/en-GB'),
      'es': () => import('@angular/common/locales/es'),
      'de': () => import('@angular/common/locales/de'),
      'fi': () => import('@angular/common/locales/fi'),
      'fr': () => import('@angular/common/locales/fr'),
      'hi': () => import('@angular/common/locales/hi'),
      'hu': () => import('@angular/common/locales/hu'),
      'is': () => import('@angular/common/locales/is'),
      'it': () => import('@angular/common/locales/it'),
      'pt': () => import('@angular/common/locales/pt'),
      'tr': () => import('@angular/common/locales/tr'),
      'ru': () => import('@angular/common/locales/ru'),
      'ro': () => import('@angular/common/locales/ro'),
      'sk': () => import('@angular/common/locales/sk'),
      'sl': () => import('@angular/common/locales/sl'),
      'zh-Hans': () => import('@angular/common/locales/zh-Hans'),
      'zh-Hant': () => import('@angular/common/locales/zh-Hant')
  }
  return list[locale]();
}

export function registerLocaleForEsBuild(
  {
      cultureNameLocaleFileMap = {},
      errorHandlerFn = defaultLocalErrorHandlerFn,
  } = {} as RegisterLocaleData,
) {
  return (locale: string): Promise<any> => {
      localeMap = { ...differentLocales, ...cultureNameLocaleFileMap };
      const l = localeMap[locale] || locale;
      const localeSupportList = "ar|cs|en|en-GB|es|de|fi|fr|hi|hu|is|it|pt|tr|ru|ro|sk|sl|zh-Hans|zh-Hant".split("|");

      if (localeSupportList.indexOf(locale) == -1) {
          return;
      }
      return new Promise((resolve, reject) => {
          return loadLocale(l)
              .then(val => {
                  let module = val;
                  while (module.default) {
                      module = module.default;
                  }
                  resolve({ default: module });
              })
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



export function registerLocale(
  {
    cultureNameLocaleFileMap = {},
    errorHandlerFn = defaultLocalErrorHandlerFn,
  } = {} as RegisterLocaleData,
) {
  return (locale: string): Promise<any> => {
    localeMap = { ...differentLocales, ...cultureNameLocaleFileMap };
    const localePath = `/locales/${localeMap[locale] || locale}`;
    return new Promise((resolve, reject) => {
      return import(
        /* webpackMode: "lazy-once" */
        /* webpackChunkName: "locales"*/
        /* webpackInclude: /[/\\](ar|cs|en|en-GB|es|de|fi|fr|hi|hu|is|it|pt|tr|ru|ro|sk|sl|zh-Hans|zh-Hant)\.(mjs|js)$/ */
        /* webpackExclude: /[/\\]global|extra/ */
        `@angular/common${localePath}`
      )
        .then(val => {
          let module = val;
          while (module.default) {
            module = module.default;
          }
          resolve({ default: module });
        })
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

const extraLocales = {} as { [key: string]: any };
export function storeLocaleData(data: any, localeId: string) {
  extraLocales[localeId] = data;
}

export async function defaultLocalErrorHandlerFn({ locale, resolve }: LocaleErrorHandlerData) {
  if (extraLocales[locale]) {
    resolve({ default: extraLocales[localeMap[locale] || locale] });
    return;
  }

  if (isDevMode()) {
    console.error(
      `Cannot find the ${locale} locale file. You can check how can add new culture at https://abp.io/docs/latest/framework/ui/angular/localization#adding-a-new-culture`,
    );
  }

  resolve();
}
