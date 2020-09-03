import { InjectionToken } from '@angular/core';
import { RootParams } from '../models/common';

export const THEME_SHARED_OPTIONS = new InjectionToken<RootParams>('THEME_SHARED_OPTIONS');

export function themeSharedOptionsFactory({ ngbDatepickerOptions = {}, ...options }: RootParams) {
  return {
    ...options,
    ngbDatepickerOptions: {
      minDate: { year: 1900, month: 1, day: 1 },
      maxDate: { year: new Date().getFullYear() + 100, month: 1, day: 1 },
      ...ngbDatepickerOptions,
    },
  } as RootParams;
}
