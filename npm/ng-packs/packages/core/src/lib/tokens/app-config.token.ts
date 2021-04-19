import { InjectionToken } from '@angular/core';

export type AppConfigInitErrorFn = (error: any) => void;

export const APP_CONFIG_INITIALIZATION_ERROR = new InjectionToken<AppConfigInitErrorFn[]>(
  'APP_CONFIG_INITIALIZATION_ERROR',
);
