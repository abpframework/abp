import { InjectionToken } from '@angular/core';
import { CheckAuthenticationStateFn } from '@abp/ng.core';

export const CHECK_AUTHENTICATION_STATE_FN_KEY = new InjectionToken<CheckAuthenticationStateFn>(
  'CHECK_AUTHENTICATION_STATE_FN_KEY',
);
