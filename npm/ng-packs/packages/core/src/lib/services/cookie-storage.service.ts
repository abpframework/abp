import { Injectable, PLATFORM_ID, inject, REQUEST } from '@angular/core';
import { DOCUMENT, isPlatformBrowser } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class AbpCookieStorageService implements Storage {
  private platformId = inject(PLATFORM_ID);
  private document = inject(DOCUMENT);
  private request = inject(REQUEST);

  get length(): number {
    return isPlatformBrowser(this.platformId) ? this.keys().length : this.getCookiesFromRequest()?.size ?? 0;
  }

  clear(): void {
    if (!isPlatformBrowser(this.platformId)) return;
    this.keys().forEach(k => this.removeItem(k));
  }

  getItem(key: string): string | null {
    if (!isPlatformBrowser(this.platformId)) {
      return this.getCookiesFromRequest()?.get(key) ?? null;
    }
    const name = key + '=';
    const parts = (this.document.cookie || '').split('; ');
    for (const p of parts) {
      if (p.startsWith(name)) {
        return decodeURIComponent(p.slice(name.length));
      }
    }
    return null;
  }

  key(index: number): string | null {
    if (!isPlatformBrowser(this.platformId)) return null;
    return this.keys()[index] ?? null;
  }

  removeItem(key: string): void {
    if (!isPlatformBrowser(this.platformId)) return;
    this.setCookie(key, '', { 'max-age': -1, path: '/' });
  }

  setItem(key: string, value: string): void {
    if (!isPlatformBrowser(this.platformId)) return;
    this.setCookie(key, encodeURIComponent(value), {
      path: '/',
      sameSite: 'Lax',
      secure: true,
    });
  }

  setItemWithExpiry(key: string, value: string, seconds: number): void {
    if (!isPlatformBrowser(this.platformId)) return;
    this.setCookie(key, encodeURIComponent(value), {
      path: '/',
      sameSite: 'Lax',
      secure: true,
      'max-age': Math.max(0, Math.floor(seconds)),
    });
  }

  private keys(): string[] {
    const raw = (this.document.cookie || '').split('; ').filter(Boolean);
    return raw
      .map(c => decodeURIComponent(c.split('=')[0]));
  }

  private setCookie(name: string, value: string, opts: {
    path?: string;
    domain?: string;
    secure?: boolean;
    sameSite?: 'Lax' | 'Strict' | 'None';
    expires?: Date;
    'max-age'?: number;
  }) {
    let s = `${name}=${value}`;
    if (opts.path) s += `; Path=${opts.path}`;
    if (opts.domain) s += `; Domain=${opts.domain}`;
    if (opts.sameSite) s += `; SameSite=${opts.sameSite}`;
    if (opts.secure) s += `; Secure`;
    if (opts.expires) s += `; Expires=${opts.expires.toUTCString()}`;
    if (typeof opts['max-age'] === 'number') s += `; Max-Age=${opts['max-age']}`;
    this.document.cookie = s;
  }

  private getCookiesFromRequest(): Map<string, string> {
    const  cookies = new Map<string, string>();
    const cookieHeader = this.request?.headers.get('cookie') ?? '';
    for (const part of cookieHeader.split(';')) {
      const i = part.indexOf('=');
      if (i > -1) {
        const k = part.slice(0, i).trim();
        const v = decodeURIComponent(part.slice(i + 1).trim());
        cookies.set(k, v);
      }
    }
    return cookies;
  }
}
