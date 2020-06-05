import { InjectionToken } from '@angular/core';
import { Options } from '../models/options';

export function optionsFactory(options: Options) {
  return {
    redirectUrl: '/',
    ...options,
  };
}

export const ACCOUNT_OPTIONS = new InjectionToken('ACCOUNT_OPTIONS');
