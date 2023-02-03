import { InjectionToken } from '@angular/core';
import { CheckAuthenticationStateFn } from '../models/auth';

export const CHECK_AUTHENTICATION_STATE_FN_KEY = new InjectionToken<CheckAuthenticationStateFn>(
  'CHECK_AUTHENTICATION_STATE_FN_KEY',
);
