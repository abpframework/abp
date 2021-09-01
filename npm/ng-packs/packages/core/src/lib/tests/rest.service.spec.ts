import { createHttpFactory, HttpMethod, SpectatorHttp, SpyObject } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Rest } from '../models';
import { EnvironmentService } from '../services';
import { RestService } from '../services/rest.service';
import { CORE_OPTIONS } from '../tokens';

describe('HttpClient testing', () => {
  let spectator: SpectatorHttp<RestService>;
  let environmentService: SpyObject<EnvironmentService>;
  let store: SpyObject<Store>;
  const api = 'https://abp.io';

  const createHttp = createHttpFactory({
    service: RestService,
    providers: [EnvironmentService, { provide: CORE_OPTIONS, useValue: { environment: {} } }],
    mocks: [OAuthService, Store],
  });

  beforeEach(() => {
    spectator = createHttp();
    environmentService = spectator.inject(EnvironmentService);
    store = spectator.inject(Store);
    environmentService.setState({
      apis: {
        default: {
          url: api,
        },
        foo: {
          url: 'bar',
        },
      },
    });
  });

  afterEach(() => {
    spectator.controller.verify();
  });

  test('should send a GET request with params', () => {
    spectator.service
      .request({ method: HttpMethod.GET, url: '/test', params: { id: 1 } })
      .subscribe();
    spectator.expectOne(api + '/test?id=1', HttpMethod.GET);
  });

  test('should send a POST request with body', () => {
    spectator.service
      .request({ method: HttpMethod.POST, url: '/test', body: { id: 1 } })
      .subscribe();
    const req = spectator.expectOne(api + '/test', HttpMethod.POST);
    expect(req.request.body['id']).toEqual(1);
  });

  test('should use the specific api', () => {
    spectator.service
      .request({ method: HttpMethod.GET, url: '/test' }, null, 'http://test.api')
      .subscribe();
    spectator.expectOne('http://test.api' + '/test', HttpMethod.GET);
  });

  test('should use the url of a specific API when apiName is given', () => {
    spectator.service
      .request({ method: HttpMethod.GET, url: '/test' }, { apiName: 'foo' })
      .subscribe();

    spectator.expectOne('bar' + '/test', HttpMethod.GET);
  });

  test('should complete upon successful request', done => {
    const complete = jest.fn(done);

    spectator.service.request({ method: HttpMethod.GET, url: '/test' }).subscribe({ complete });

    const req = spectator.expectOne(api + '/test', HttpMethod.GET);
    spectator.flushAll([req], [{}]);
  });

  test('should handle the error', () => {
    const spy = jest.spyOn(store, 'dispatch');

    spectator.service
      .request({ method: HttpMethod.GET, url: '/test' }, { observe: Rest.Observe.Events })
      .pipe(
        catchError(err => {
          expect(err).toBeTruthy();
          expect(spy).toHaveBeenCalled();
          return of(null);
        }),
      )
      .subscribe();

    const req = spectator.expectOne(api + '/test', HttpMethod.GET);
    spectator.flushAll([req], [throwError('Testing error')]);
  });

  test('should not handle the error when skipHandleError is true', () => {
    const spy = jest.spyOn(store, 'dispatch');

    spectator.service
      .request(
        { method: HttpMethod.GET, url: '/test' },
        { observe: Rest.Observe.Events, skipHandleError: true },
      )
      .pipe(
        catchError(err => {
          expect(err).toBeTruthy();
          expect(spy).toHaveBeenCalledTimes(0);
          return of(null);
        }),
      )
      .subscribe();

    const req = spectator.expectOne(api + '/test', HttpMethod.GET);
    spectator.flushAll([req], [throwError('Testing error')]);
  });
});
