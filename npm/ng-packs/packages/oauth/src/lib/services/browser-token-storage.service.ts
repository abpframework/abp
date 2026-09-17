import { Injectable } from '@angular/core';
import { OAuthStorage } from 'angular-oauth2-oidc';

@Injectable({
  providedIn: 'root',
})
export class BrowserTokenStorageService implements OAuthStorage {
  getItem(key: string): string {
    return this.readCookie(key);
  }

  removeItem(key: string): void {
    this.removeCookie(key);
  }

  setItem(key: string, data: string): void {
    this.writeCookie(key, data);
  }

  readCookie(name: string): string | null {
    if (typeof document === 'undefined') return null;
    const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? decodeURIComponent(match[2]) : null;
  }

  writeCookie(name: string, value: string, days = 7): void {
    if (typeof document === 'undefined') return;
    const expires = new Date(Date.now() + days * 86400000).toUTCString();
    document.cookie = `${name}=${encodeURIComponent(value)}; expires=${expires}; path=/; Secure; SameSite=Lax`;
  }

  removeCookie(name: string): void {
    if (typeof document === 'undefined') return;
    document.cookie = `${name}=; Max-Age=0; path=/;`;
  }
}
