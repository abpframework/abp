import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class AbpLocalStorageService implements Storage {
  private platformId = inject(PLATFORM_ID);

  constructor() {
  }
  [name: string]: any;
  get length(): number {
    return isPlatformBrowser(this.platformId) ? localStorage.length : 0;
  }

  clear(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.clear();
    }
  }
  getItem(key: string): string | null {
    if (!isPlatformBrowser(this.platformId)) {
      return null;
    }
    return localStorage.getItem(key);
  }
  key(index: number): string | null {
    if (!isPlatformBrowser(this.platformId)) {
      return null;
    }
    return localStorage.key(index);
  }
  removeItem(key: string): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(key);
    }
  }
  setItem(key: string, value: string): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(key, value);
    }
  }
}
