import { ConfigState } from '@abp/ng.core';
import { createHttpFactory, HttpMethod, SpectatorHttp, SpyObject } from '@ngneat/spectator/jest';
import { NgxsModule, Store } from '@ngxs/store';
import { of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Rest } from '../models';
import { RestService } from '../services/rest.service';

describe('HttpClient testing', () => {
  let spectator: SpectatorHttp<RestService>;
  let store: SpyObject<Store>;
  const api = 'https://abp.io';

  const createHttp = createHttpFactory({
    dataService: RestService,
    imports: [NgxsModule.forRoot([ConfigState])],
  });

  beforeEach(() => {
    spectator = createHttp();
    store = spectator.get(Store);
    store.reset({
      ConfigState: {
        environment: {
          apis: {
            default: {
              url: api,
            },
            foo: {
              url: 'bar',
            },
          },
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
