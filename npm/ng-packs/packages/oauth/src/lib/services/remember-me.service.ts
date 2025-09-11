import {
  AbpCookieStorageService,
  AbpLocalStorageService,
  APP_STARTED_WITH_SSR,
} from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class RememberMeService {
  readonly #rememberMe = 'remember_me';
  protected readonly localStorageService = inject(AbpLocalStorageService);
  protected readonly cookieStorageService = inject(AbpCookieStorageService);
  private appStartedWithSsr = inject(APP_STARTED_WITH_SSR, { optional: true });

  set(remember: boolean) {
    if (this.appStartedWithSsr) {
      this.cookieStorageService.setItem(this.#rememberMe, JSON.stringify(remember));
      return;
    }
    this.localStorageService.setItem(this.#rememberMe, JSON.stringify(remember));
  }

  remove() {
    if (this.appStartedWithSsr) {
      this.cookieStorageService.removeItem(this.#rememberMe);
      return;
    }
    this.localStorageService.removeItem(this.#rememberMe);
  }

  get() {
    if (this.appStartedWithSsr) {
      return Boolean(JSON.parse(this.cookieStorageService.getItem(this.#rememberMe) || 'false'));
    }
    return Boolean(JSON.parse(this.localStorageService.getItem(this.#rememberMe) || 'false'));
  }

  getFromToken(accessToken: string) {
    const tokenBody = accessToken.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
    try {
      const parsedToken = JSON.parse(atob(tokenBody));
      return Boolean(parsedToken[this.#rememberMe]);
    } catch {
      return false;
    }
  }
}
