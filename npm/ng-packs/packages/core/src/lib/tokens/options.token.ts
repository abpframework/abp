import { InjectionToken } from '@angular/core';
import { ABP } from '../models/common';

export const CORE_OPTIONS = new InjectionToken<ABP.Root>('CORE_OPTIONS');

export function coreOptionsFactory({ ...options }: ABP.Root) {
  return {
    ...options,
  } as ABP.Root;
}
