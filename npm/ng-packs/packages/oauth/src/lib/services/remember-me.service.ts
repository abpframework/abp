import { AbpLocalStorageService } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class RememberMeService {
  readonly #rememberMe = 'remember_me';
  protected readonly localStorageService = inject(AbpLocalStorageService);

  set(remember: boolean) {
    this.localStorageService.setItem(this.#rememberMe, JSON.stringify(remember));
  }

  remove() {
    this.localStorageService.removeItem(this.#rememberMe);
  }

  get() {
    return Boolean(JSON.parse(this.localStorageService.getItem(this.#rememberMe) || 'false'));
  }

  getFromToken(accessToken: string) {
    const parsedToken = JSON.parse(atob(accessToken.split('.')[1]));
    return Boolean(parsedToken[this.#rememberMe]);
  }
}
