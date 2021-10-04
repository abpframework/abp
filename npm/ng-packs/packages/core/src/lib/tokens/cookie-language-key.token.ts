import { InjectionToken } from '@angular/core';

export const COOKIE_LANGUAGE_KEY = new InjectionToken<string>('COOKIE_LANGUAGE_KEY', {
  factory: () => '.AspNetCore.Culture',
});
