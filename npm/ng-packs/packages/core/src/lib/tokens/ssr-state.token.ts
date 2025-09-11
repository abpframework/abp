import { inject, InjectionToken, makeStateKey, PLATFORM_ID, TransferState } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

export const SSR_FLAG = makeStateKey<boolean>('SSR_FLAG');

export const APP_STARTED_WITH_SSR = new InjectionToken<boolean>('APP_STARTED_WITH_SSR', {
  providedIn: 'root',
  factory: () => {
    const platformId = inject(PLATFORM_ID);
    if (!isPlatformBrowser(platformId)) return true;
    const ts = inject(TransferState);
    return ts.get(SSR_FLAG, false);
  },
});
