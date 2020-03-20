import { ConfigState } from '@abp/ng.core';
import { createHttpFactory, HttpMethod, SpectatorHttp, SpyObject } from '@ngneat/spectator/jest';
import { NgxsModule, Store } from '@ngxs/store';
import { interval, of, Subscription, throwError, timer } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { RestOccurError } from '../actions';
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

  test('should close the subscriber when observe equal to body', done => {
    jest.spyOn(spectator.httpClient, 'request').mockReturnValue(interval(50));

    const subscriber: Subscription = spectator.service
      .request({ method: HttpMethod.GET, url: '/test' }, { observe: Rest.Observe.Body })
      .subscribe();

    timer(51).subscribe(() => {
      expect(subscriber.closed).toBe(true);
      done();
    });
  });

  test('should open the subscriber when observe not equal to body', done => {
    jest.spyOn(spectator.httpClient, 'request').mockReturnValue(interval(50));

    const subscriber: Subscription = spectator.service
      .request({ method: HttpMethod.GET, url: '/test' }, { observe: Rest.Observe.Events })
      .subscribe();

    timer(51).subscribe(() => {
      expect(subscriber.closed).toBe(false);
      done();
    });
  });

  test('should handle the error', () => {
    jest.spyOn(spectator.httpClient, 'request').mockReturnValue(throwError('Testing error'));
    const spy = jest.spyOn(store, 'dispatch');

    spectator.service
      .request({ method: HttpMethod.GET, url: '/test' }, { observe: Rest.Observe.Events })
      .pipe(
        catchError(err => {
          expect(err).toBeTruthy();
          expect(spy.mock.calls[0][0] instanceof RestOccurError).toBe(true);
          return of(null);
        }),
      )
      .subscribe();
  });

  test('should not handle the error when skipHandleError is true', () => {
    jest.spyOn(spectator.httpClient, 'request').mockReturnValue(throwError('Testing error'));
    const spy = jest.spyOn(store, 'dispatch');

    spectator.service
      .request(
        { method: HttpMethod.GET, url: '/test' },
        { observe: Rest.Observe.Events, skipHandleError: true },
      )
      .pipe(
        catchError(err => {
          expect(err).toBeTruthy();
          expect(spy.mock.calls).toHaveLength(0);
          return of(null);
        }),
      )
      .subscribe();
  });
});
