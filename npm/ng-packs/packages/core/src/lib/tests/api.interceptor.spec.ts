import { HttpRequest } from '@angular/common/http';
import { SpyObject } from '@ngneat/spectator';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { OAuthService } from 'angular-oauth2-oidc';
import { Subject, timer } from 'rxjs';
import { ApiInterceptor } from '../interceptors';
import { HttpWaitService, SessionStateService } from '../services';

describe('ApiInterceptor', () => {
  let spectator: SpectatorService<ApiInterceptor>;
  let interceptor: ApiInterceptor;
  let oauthService: SpyObject<OAuthService>;
  let sessionState: SpyObject<SessionStateService>;
  let httpWaitService: SpyObject<HttpWaitService>;

  const createService = createServiceFactory({
    service: ApiInterceptor,
    mocks: [OAuthService, SessionStateService],
  });

  beforeEach(() => {
    spectator = createService();
    interceptor = spectator.service;
    sessionState = spectator.inject(SessionStateService);
    oauthService = spectator.inject(OAuthService);
    httpWaitService = spectator.inject(HttpWaitService);
  });

  it('should add headers to http request', done => {
    oauthService.getAccessToken.andReturn('ey892mkwa8^2jk');
    sessionState.getLanguage.andReturn('tr');
    sessionState.getTenant.andReturn({ id: 'Volosoft', name: 'Volosoft' });

    const request = new HttpRequest('GET', 'https://abp.io');
    const handleRes$ = new Subject();

    const handler = {
      handle: (req: HttpRequest<any>) => {
        expect(req.headers.get('Authorization')).toEqual('Bearer ey892mkwa8^2jk');
        expect(req.headers.get('Accept-Language')).toEqual('tr');
        expect(req.headers.get('__tenant')).toEqual('Volosoft');
        done();
        return handleRes$;
      },
    };

    interceptor.intercept(request, handler as any);

    handleRes$.next();
    handleRes$.complete();
  });

  it('should call http wait services add request and delete request', done => {
    const spyAddRequest = jest.spyOn(httpWaitService, 'addRequest');
    const spyDeleteRequest = jest.spyOn(httpWaitService, 'deleteRequest');

    const request = new HttpRequest('GET', 'https://abp.io');
    const handleRes$ = new Subject();

    const handler = {
      handle: (req: HttpRequest<any>) => {
        return handleRes$;
      },
    };

    interceptor.intercept(request, handler as any).subscribe();

    handleRes$.next();
    handleRes$.complete();

    timer(0).subscribe(() => {
      expect(spyAddRequest).toHaveBeenCalled();
      expect(spyDeleteRequest).toHaveBeenCalled();
      done();
    });
  });
});
