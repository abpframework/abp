import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

@Injectable()
export class TimeoutLimitedOAuthService extends OAuthService {
  protected override calcTimeout(storedAt: number, expiration: number): number {
    const now = this.dateTimeService.now();
    const delta = (expiration - storedAt) * this.timeoutFactor - (now - storedAt);
    const result = Math.max(0, delta);
    const MAX_TIMEOUT_DURATION = 2147483647;
    return result < MAX_TIMEOUT_DURATION ? result : MAX_TIMEOUT_DURATION - 1;
  }
}
