import { inject, InjectionToken, makeStateKey, PLATFORM_ID, TransferState } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { AbpCookieStorageService } from '../services';

export const SSR_FLAG = makeStateKey<boolean>('SSR_FLAG');

export const APP_STARTED_WITH_SSR = new InjectionToken<boolean>('APP_STARTED_WITH_SSR', {
  providedIn: 'root',
  factory: () => {
    const platformId = inject(PLATFORM_ID);
    const cookieService = inject(AbpCookieStorageService);
    if (!isPlatformBrowser(platformId)) return true;
    const ts = inject(TransferState);
    const ssrEnabled = cookieService.getItem('ssr-init');
    // Remove the cookie after reading its value because it's only needed once
    cookieService.removeItem('ssr-init');
    return ts.get(SSR_FLAG, false) || ssrEnabled === 'true';
  },
});
