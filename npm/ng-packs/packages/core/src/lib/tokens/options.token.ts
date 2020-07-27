import { InjectionToken } from '@angular/core';
import { ABP } from '../models/common';
import differentLocales from '../constants/different-locales';

export const CORE_OPTIONS = new InjectionToken<ABP.Root>('CORE_OPTIONS');

export function coreOptionsFactory({
  cultureNameToLocaleFileNameMapping: localeNameMap = {},
  ...options
}: ABP.Root) {
  return {
    ...options,
    cultureNameToLocaleFileNameMapping: { ...differentLocales, ...localeNameMap },
  } as ABP.Root;
}
