import { Inject, Injectable, Optional } from '@angular/core';
import { OAuthStorage } from 'angular-oauth2-oidc';
import { REQUEST } from '@angular/core';

@Injectable({ providedIn: null })
export class ServerTokenStorageService implements OAuthStorage {
  private cookies = new Map<string, string>();

  constructor(@Optional() @Inject(REQUEST) private req: Request | null) {
    const cookieHeader = this.req?.headers.get('cookie') ?? '';
    for (const part of cookieHeader.split(';')) {
      const i = part.indexOf('=');
      if (i > -1) {
        const k = part.slice(0, i).trim();
        const v = decodeURIComponent(part.slice(i + 1).trim());
        this.cookies.set(k, v);
      }
    }
  }

  getItem(key: string): string {
    const fromCookie = this.cookies.get(key);
    if (fromCookie) {
      return fromCookie;
    }

    return '';
  }

  setItem(_k: string, _v: string): void {}
  removeItem(_k: string): void {}
}
