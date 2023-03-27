import { inject, Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

import { AbpStorageService } from './storage.service';

@Injectable({ providedIn: 'root' })
export class AbpCookieStorageService implements AbpStorageService {
  private cookieService = inject(CookieService);

  get length(): number {
    const cookies = this.cookieService.getAll() || {};
    return Object.keys(cookies).length;
  }

  clear(): void {
    this.cookieService.deleteAll();
  }

  getItem(key: string): string {
    return this.cookieService.get(key);
  }

  key(index: number): string {
    const cookies = this.cookieService.getAll() || {};
    const keys = Object.keys(cookies);
    const key = keys[index];
    return cookies[key];
  }

  removeItem(key: string): void {
    this.removeItem(key);
  }

  setItem(key: string, value: string): void {
    this.cookieService.set(key, value);
  }
}