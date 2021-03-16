import { InjectionToken } from '@angular/core';
import { AccountConfigOptions } from '../models/config-options';

export const ACCOUNT_CONFIG_OPTIONS = new InjectionToken<AccountConfigOptions>(
  'ACCOUNT_CONFIG_OPTIONS',
);
