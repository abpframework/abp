import { InjectionToken } from '@angular/core';
import differentLocales from '../constants/different-locales';
import { ABP } from '../models/common';

export const CORE_OPTIONS = new InjectionToken<ABP.Root>('CORE_OPTIONS');

export function coreOptionsFactory({ cultureNameLocaleFileMap = {}, ...options }: ABP.Root) {
  return {
    ...options,
    cultureNameLocaleFileMap: { ...differentLocales, ...cultureNameLocaleFileMap },
  } as ABP.Root;
}
