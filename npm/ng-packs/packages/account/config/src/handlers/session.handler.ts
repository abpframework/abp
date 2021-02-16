import { EnvironmentService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { filter, take } from 'rxjs/operators';

const cookieKey = 'rememberMe';
const storageKey = 'rememberMe';

@Injectable({ providedIn: 'root' })
export class SessionHandler {
  constructor(private oAuthService: OAuthService, environmentService: EnvironmentService) {
    environmentService
      .getEnvironment$()
      .pipe(
        filter(env => env && JSON.stringify(env) !== '{}'),
        take(1),
      )
      .subscribe(environment => {
        if (environment.oAuthConfig.responseType === 'code') return;

        this.init();
      });
  }

  private init() {
    this.oAuthService.events.subscribe(event => {
      if (event.type === 'logout') {
        this.removeRememberInfo();
      }
    });

    if (!getCookieValueByName('rememberMe') && localStorage.getItem(storageKey)) {
      this.oAuthService.logOut();
    }
  }

  setRememberInfo(remember: boolean) {
    this.removeRememberInfo();
    localStorage.setItem(storageKey, 'true');
    const expiration = new Date(this.oAuthService.getAccessTokenExpiration()).toUTCString();
    document.cookie = `${cookieKey}=true${remember ? `;expires=${expiration}` : ''}`;
  }

  removeRememberInfo() {
    localStorage.removeItem(storageKey);
    document.cookie = cookieKey + '= ; expires = Thu, 01 Jan 1970 00:00:00 GMT';
  }
}

function getCookieValueByName(name: string) {
  var match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
  return match ? match[2] : '';
}
