import { InjectionToken } from '@angular/core';

export const NAVIGATE_TO_MANAGE_PROFILE = new InjectionToken<() => void>(
  'NAVIGATE_TO_MANAGE_PROFILE',
);
