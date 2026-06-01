import { HttpRequest } from '@angular/common/http';
import { SpyObject } from '@ngneat/spectator';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/vitest';
import { OAuthService } from 'angular-oauth2-oidc';
import { Subject } from 'rxjs';
import { HttpWaitService, SessionStateService, TENANT_KEY } from '@abp/ng.core';
import { OAuthApiInterceptor } from '../interceptors';

describe('ApiInterceptor', () => {
  let spectator: SpectatorService<OAuthApiInterceptor>;
  let interceptor: OAuthApiInterceptor;
  let oauthService: SpyObject<OAuthService>;
  let sessionState: SpyObject<SessionStateService>;
  let httpWaitService: SpyObject<HttpWaitService>;

  const testTenantKey = 'TEST_TENANT_KEY';

  const createService = createServiceFactory({
    service: OAuthApiInterceptor,
    mocks: [OAuthService, SessionStateService],
    providers: [{ provide: TENANT_KEY, useValue: testTenantKey }],
  });

  beforeEach(() => {
    spectator = createService();
    interceptor = spectator.service;
    sessionState = spectator.inject(SessionStateService);
    oauthService = spectator.inject(OAuthService);
    httpWaitService = spectator.inject(HttpWaitService);
  });

  it('should add headers to http request', () => {
    oauthService.getAccessToken.andReturn('ey892mkwa8^2jk');
    sessionState.getLanguage.andReturn('tr');
    sessionState.getTenant.andReturn({ id: 'Volosoft', name: 'Volosoft' });

    const request = new HttpRequest('GET', 'https://abp.io');
    const handleRes$ = new Subject<void>();

    const handler = {
      handle: (req: HttpRequest<any>) => {
        expect(req.headers.get('Authorization')).toEqual('Bearer ey892mkwa8^2jk');
        expect(req.headers.get('Accept-Language')).toEqual('tr');
        expect(req.headers.get(testTenantKey)).toEqual('Volosoft');
        return handleRes$;
      },
    };

    interceptor.intercept(request, handler as any);

    handleRes$.next();
    handleRes$.complete();
  });

  it('should call http wait services add request and delete request', () => {
    const spyAddRequest = vi.spyOn(httpWaitService, 'addRequest');
    const spyDeleteRequest = vi.spyOn(httpWaitService, 'deleteRequest');

    const request = new HttpRequest('GET', 'https://abp.io');
    const handleRes$ = new Subject<void>();

    const handler = {
      handle: (req: HttpRequest<any>) => {
        return handleRes$;
      },
    };

    interceptor.intercept(request, handler as any).subscribe();

    handleRes$.next();
    handleRes$.complete();

    expect(spyAddRequest).toHaveBeenCalled();
    expect(spyDeleteRequest).toHaveBeenCalled();
  });
});
