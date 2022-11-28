import { Inject, Injectable, NgZone, Optional } from '@angular/core';
import {
  AuthConfig,
  DateTimeProvider,
  HashHandler,
  OAuthLogger,
  OAuthService,
  OAuthStorage,
  UrlHelperService,
  ValidationHandler,
} from 'angular-oauth2-oidc';
import { HttpClient } from '@angular/common/http';
import { DOCUMENT } from '@angular/common';

@Injectable()
export class TimeoutLimitedOAuthService extends OAuthService {
  constructor(
    ngZone: NgZone,
    http: HttpClient,
    @Optional() storage: OAuthStorage,
    @Optional() tokenValidationHandler: ValidationHandler,
    @Optional() config: AuthConfig,
    urlHelper: UrlHelperService,
    logger: OAuthLogger,
    @Optional() crypto: HashHandler,
    @Inject(DOCUMENT) document: any,
    dateTimeService: DateTimeProvider,
  ) {
    super(
      ngZone,
      http,
      storage,
      tokenValidationHandler,
      config,
      urlHelper,
      logger,
      crypto,
      document,
      dateTimeService,
    );
  }

  protected override calcTimeout(storedAt: number, expiration: number): number {
    const result = super.calcTimeout(storedAt, expiration);
    const MAX_TIMEOUT_DURATION = 2147483647;
    return result < MAX_TIMEOUT_DURATION ? result : MAX_TIMEOUT_DURATION - 1;
  }
}
