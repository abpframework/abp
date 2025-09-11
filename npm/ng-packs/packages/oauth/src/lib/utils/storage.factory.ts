import { inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { ServerTokenStorageService } from '../services/server-token-storage.service';
import { BrowserTokenStorageService } from '../services';
import { OAuthStorage } from 'angular-oauth2-oidc';
import { AbpLocalStorageService, APP_STARTED_WITH_SSR } from '@abp/ng.core';

export class MockStorage implements Storage {
  private data = new Map<string, string>();
  get length() {
    return this.data.size;
  }
  clear() {
    this.data.clear();
  }
  getItem(key: string) {
    return this.data.get(key) || null;
  }
  key(index: number) {
    return Array.from(this.data.keys())[index] || null;
  }
  removeItem(key: string) {
    this.data.delete(key);
  }
  setItem(key: string, value: string) {
    this.data.set(key, value);
  }
}

export function oAuthStorageFactory(): OAuthStorage {
  const platformId = inject(PLATFORM_ID);
  const appStartedWithSSR = inject(APP_STARTED_WITH_SSR);
  if (appStartedWithSSR) {
    return isPlatformBrowser(platformId)
      ? inject(BrowserTokenStorageService)
      : inject(ServerTokenStorageService);
  }
  return inject(AbpLocalStorageService);
}
