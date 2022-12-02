import { Injectable, NgZone, Optional } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

@Injectable()
export class TimeoutLimitedOAuthService extends OAuthService {
  protected override calcTimeout(storedAt: number, expiration: number): number {
    const result = super.calcTimeout(storedAt, expiration);
    const MAX_TIMEOUT_DURATION = 2147483647;
    return result < MAX_TIMEOUT_DURATION ? result : MAX_TIMEOUT_DURATION - 1;
  }
}
