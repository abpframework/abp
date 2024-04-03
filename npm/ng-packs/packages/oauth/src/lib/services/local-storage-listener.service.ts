import { DOCUMENT } from '@angular/common';
import { Injectable, inject } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

@Injectable({
  providedIn: 'root',
})
export class LocalStorageListenerService {
  protected readonly window = inject(DOCUMENT).defaultView;
  protected readonly authService = inject(OAuthService);

  constructor() {
    this.window.addEventListener('storage', event => {
      if (event.key === 'access_token' && event.newValue === null) {
        this.authService.logOut();
      }
    });
  }
}
