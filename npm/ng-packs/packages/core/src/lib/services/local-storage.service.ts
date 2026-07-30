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

  private get storage(): Storage | null {
    if (!isPlatformBrowser(this.platformId) || typeof window === 'undefined') {
      return null;
    }

    try {
      return window.localStorage;
    } catch {
      return null;
    }
  }

  get length(): number {
    return this.storage?.length || 0;
  }

  clear(): void {
    this.storage?.clear();
  }
  getItem(key: string): string | null {
    return this.storage?.getItem(key) || null;
  }
  key(index: number): string | null {
    return this.storage?.key(index) || null;
  }
  removeItem(key: string): void {
    this.storage?.removeItem(key);
  }
  setItem(key: string, value: string): void {
    this.storage?.setItem(key, value);
  }
}
