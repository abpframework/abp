import { InjectionToken } from '@angular/core';
import { ABP } from '../models/common';
import differentLocales from '../constants/different-locales';

export const CORE_OPTIONS = new InjectionToken<ABP.Root>('CORE_OPTIONS');

export function coreOptionsFactory({
  cultureNameLocaleFileMap: localeNameMap = {},
  ...options
}: ABP.Root) {
  return {
    ...options,
    cultureNameLocaleFileMap: { ...differentLocales, ...localeNameMap },
  } as ABP.Root;
}
