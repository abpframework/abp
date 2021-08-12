import { InjectionToken } from '@angular/core';

export type AppInitErrorFn = (error: any) => void;

export const APP_INIT_ERROR_HANDLERS = new InjectionToken<AppInitErrorFn[]>(
  'APP_INIT_ERROR_HANDLERS',
);
