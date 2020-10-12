import { InjectionToken, Injector } from '@angular/core';

export interface LocaleErrorHandlerData {
  resolve: any;
  reject: any;
  error: any;
  locale: string;
  storedLocales: Record<string, any>;
  injector: Injector;
}

export const LOCALE_ERROR_HANDLER = new InjectionToken<(data: LocaleErrorHandlerData) => any>(
  'LOCALE_ERROR_HANDLER',
);
