import { InjectionToken } from '@angular/core';

export const NAVIGATE_TO_MANAGE_PROFILE = new InjectionToken<() => void>(
  'NAVIGATE_TO_MANAGE_PROFILE',
);

export const NAVIGATE_TO_MY_SECURITY_LOGS = new InjectionToken<() => void>(
  'NAVIGATE_TO_MY_SECURITY_LOGS',
);
