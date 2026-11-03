import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { effect, signal } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { createHttpFactory, HttpMethod, SpectatorHttp, SpyObject } from '@ngneat/spectator/vitest';
import { OAuthService } from 'angular-oauth2-oidc';
import { of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Rest } from '../models/rest';
import { EnvironmentService } from '../services/environment.service';
import { HttpErrorReporterService } from '../services/http-error-reporter.service';
import { RestService } from '../services/rest.service';
import { CORE_OPTIONS } from '../tokens/options.token';

describe('HttpClient testing', () => {
  let spectator: SpectatorHttp<RestService>;
  let environmentService: SpyObject<EnvironmentService>;
  let httpErrorReporter: SpyObject<HttpErrorReporterService>;
  const api = 'https://abp.io';

  const createHttp = createHttpFactory({
    service: RestService,
    providers: [
      EnvironmentService,
      HttpErrorReporterService,
      { provide: CORE_OPTIONS, useValue: { environment: {} } },
    ],
    mocks: [OAuthService],
  });

  beforeEach(() => {
    spectator = createHttp();
    environmentService = spectator.inject(EnvironmentService);
    httpErrorReporter = spectator.inject(HttpErrorReporterService);
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

  test('should complete upon successful request', async () => {
    const request$ = spectator.service.request({ method: HttpMethod.GET, url: '/test' });

    // Create a promise that resolves when the observable completes
    const completionPromise = new Promise<void>((resolve, reject) => {
      request$.subscribe({
        complete: () => resolve(),
        error: err => reject(err),
      });
    });

    const req = spectator.expectOne(api + '/test', HttpMethod.GET);
    spectator.flushAll([req], [{}]);

    await completionPromise;
  });

  test('should create a resource-based request that still uses the ABP request pipeline', () => {
    const resource = TestBed.runInInjectionContext(() => {
      const resource = spectator.service.requestResource(signal({ method: HttpMethod.GET, url: '/test' }));
      effect(() => {
        resource.value();
      });
      return resource;
    });

    expect(typeof resource.reload).toBe('function');
    expect(typeof resource.value).toBe('function');
    expect(typeof resource.hasValue).toBe('function');
  });

  test('should handle the error', () => {
    const spy = vi.spyOn(httpErrorReporter, 'reportError');

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
    const spy = vi.spyOn(httpErrorReporter, 'reportError');

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

  test('should set Accept: application/octet-stream when config.responseType is blob', () => {
    spectator.service
      .request({ method: HttpMethod.GET, url: '/file' }, { responseType: Rest.ResponseType.Blob })
      .subscribe();
    const req = spectator.expectOne(api + '/file', HttpMethod.GET);
    expect(req.request.headers.get('Accept')).toEqual('application/octet-stream');
    expect(req.request.responseType).toEqual('blob');
  });

  test('should set Accept based on request-level responseType (generator path)', () => {
    spectator.service
      .request({ method: HttpMethod.GET, url: '/file', responseType: 'blob' })
      .subscribe();
    const req = spectator.expectOne(api + '/file', HttpMethod.GET);
    expect(req.request.headers.get('Accept')).toEqual('application/octet-stream');
    expect(req.request.responseType).toEqual('blob');
  });

  test('should set Accept: application/octet-stream when responseType is arraybuffer', () => {
    spectator.service
      .request(
        { method: HttpMethod.GET, url: '/binary' },
        { responseType: Rest.ResponseType.ArrayBuffer },
      )
      .subscribe();
    const req = spectator.expectOne(api + '/binary', HttpMethod.GET);
    expect(req.request.headers.get('Accept')).toEqual('application/octet-stream');
  });

  test('should NOT add Accept for text responseType (left to schematic / caller)', () => {
    spectator.service
      .request({ method: HttpMethod.GET, url: '/text' }, { responseType: Rest.ResponseType.Text })
      .subscribe();
    const req = spectator.expectOne(api + '/text', HttpMethod.GET);
    expect(req.request.headers.has('Accept')).toBe(false);
  });

  test('should not override caller-supplied Accept header (plain object)', () => {
    spectator.service
      .request(
        { method: HttpMethod.GET, url: '/file', headers: { Accept: 'image/png' } },
        { responseType: Rest.ResponseType.Blob },
      )
      .subscribe();
    const req = spectator.expectOne(api + '/file', HttpMethod.GET);
    expect(req.request.headers.get('Accept')).toEqual('image/png');
  });

  test('should not override caller-supplied Accept header (HttpHeaders)', () => {
    spectator.service
      .request(
        {
          method: HttpMethod.GET,
          url: '/file',
          headers: new HttpHeaders({ Accept: 'image/jpeg' }),
        },
        { responseType: Rest.ResponseType.Blob },
      )
      .subscribe();
    const req = spectator.expectOne(api + '/file', HttpMethod.GET);
    expect(req.request.headers.get('Accept')).toEqual('image/jpeg');
  });

  test('should preserve caller-supplied non-Accept headers and add Accept', () => {
    spectator.service
      .request(
        { method: HttpMethod.GET, url: '/file', headers: { 'X-Custom': '1' } },
        { responseType: Rest.ResponseType.Blob },
      )
      .subscribe();
    const req = spectator.expectOne(api + '/file', HttpMethod.GET);
    expect(req.request.headers.get('Accept')).toEqual('application/octet-stream');
    expect(req.request.headers.get('X-Custom')).toEqual('1');
  });

  test('should not add Accept header for default JSON responseType', () => {
    spectator.service.request({ method: HttpMethod.GET, url: '/json' }).subscribe();
    const req = spectator.expectOne(api + '/json', HttpMethod.GET);
    expect(req.request.headers.has('Accept')).toBe(false);
  });

  test('should NOT unwrap error body when skipHandleError is true', async () => {
    const spy = vi.spyOn(httpErrorReporter, 'reportError');

    const completion = new Promise<void>((resolve, reject) => {
      spectator.service
        .request(
          { method: HttpMethod.GET, url: '/text' },
          { responseType: Rest.ResponseType.Text, skipHandleError: true },
        )
        .pipe(
          catchError(err => {
            try {
              expect(spy).toHaveBeenCalledTimes(0);
              expect(typeof err.error).toBe('string');
              expect(err.error).toBe('{"error":{"code":"X","message":"y"}}');
              resolve();
            } catch (e) {
              reject(e);
            }
            return of(null);
          }),
        )
        .subscribe();
    });

    const req = spectator.expectOne(api + '/text', HttpMethod.GET);
    req.flush('{"error":{"code":"X","message":"y"}}', {
      status: 500,
      statusText: 'Internal Server Error',
    });

    await completion;
  });

  test('should unwrap ABP validationErrors envelope in text mode', async () => {
    const spy = vi.spyOn(httpErrorReporter, 'reportError');

    const completion = new Promise<void>((resolve, reject) => {
      spectator.service
        .request({ method: HttpMethod.GET, url: '/text' }, { responseType: Rest.ResponseType.Text })
        .pipe(
          catchError(() => {
            try {
              const errArg: any = spy.mock.calls[0][0];
              expect(typeof errArg.error).toBe('object');
              expect(errArg.error.error.message).toBe('Validation failed');
              expect(errArg.error.error.validationErrors).toHaveLength(1);
              resolve();
            } catch (e) {
              reject(e);
            }
            return of(null);
          }),
        )
        .subscribe();
    });

    const req = spectator.expectOne(api + '/text', HttpMethod.GET);
    req.flush(
      '{"error":{"message":"Validation failed","validationErrors":[{"message":"Required","members":["Name"]}]}}',
      { status: 400, statusText: 'Bad Request' },
    );

    await completion;
  });

  test('should unwrap ABP envelope that carries only validationErrors (no message / code)', async () => {
    const completion = new Promise<void>((resolve, reject) => {
      spectator.service
        .request({ method: HttpMethod.GET, url: '/text' }, { responseType: Rest.ResponseType.Text })
        .pipe(
          catchError(err => {
            try {
              expect(typeof err.error).toBe('object');
              expect(err.error.error.validationErrors).toHaveLength(1);
              resolve();
            } catch (e) {
              reject(e);
            }
            return of(null);
          }),
        )
        .subscribe();
    });

    const req = spectator.expectOne(api + '/text', HttpMethod.GET);
    req.flush(
      '{"error":{"validationErrors":[{"message":"Required","members":["Email"]}]}}',
      { status: 400, statusText: 'Bad Request' },
    );

    await completion;
  });

  test('should leave non-ABP-envelope JSON body alone in text mode', async () => {
    const completion = new Promise<void>((resolve, reject) => {
      spectator.service
        .request({ method: HttpMethod.GET, url: '/text' }, { responseType: Rest.ResponseType.Text })
        .pipe(
          catchError(err => {
            try {
              expect(typeof err.error).toBe('string');
              expect(err.error).toBe('{"foo":"bar"}');
              resolve();
            } catch (e) {
              reject(e);
            }
            return of(null);
          }),
        )
        .subscribe();
    });

    const req = spectator.expectOne(api + '/text', HttpMethod.GET);
    req.flush('{"foo":"bar"}', { status: 500, statusText: 'err' });

    await completion;
  });

  test('should unwrap JSON-encoded error body in text mode for HttpErrorReporter', async () => {
    const spy = vi.spyOn(httpErrorReporter, 'reportError');

    const completion = new Promise<void>((resolve, reject) => {
      spectator.service
        .request(
          { method: HttpMethod.GET, url: '/text' },
          { responseType: Rest.ResponseType.Text },
        )
        .pipe(
          catchError(() => {
            try {
              expect(spy).toHaveBeenCalledTimes(1);
              const errArg: any = spy.mock.calls[0][0];
              expect(errArg.error).toEqual({
                error: { code: 'AbpAuthorization.001', message: 'forbidden' },
              });
              resolve();
            } catch (e) {
              reject(e);
            }
            return of(null);
          }),
        )
        .subscribe();
    });

    const req = spectator.expectOne(api + '/text', HttpMethod.GET);
    req.flush('{"error":{"code":"AbpAuthorization.001","message":"forbidden"}}', {
      status: 403,
      statusText: 'Forbidden',
    });

    await completion;
  });

  test('should unwrap ABP error envelope from a Blob body in blob mode', async () => {
    // HttpTestingController doesn't deliver Blob in HttpErrorResponse; call the helper directly.
    const err: any = {
      error: new Blob(
        ['{"error":{"code":"AbpAuthorization.002","message":"forbidden-blob"}}'],
        { type: 'application/json' },
      ),
    };

    const normalized: any = await (spectator.service as any)
      .normalizeErrorBody(err, Rest.ResponseType.Blob)
      .toPromise();

    expect(normalized.error).toEqual({
      error: { code: 'AbpAuthorization.002', message: 'forbidden-blob' },
    });
  });

  test('should leave non-JSON Blob body alone in blob mode', async () => {
    const blob = new Blob([new Uint8Array([0xff, 0xd8, 0xff])], { type: 'image/jpeg' });
    const err: any = { error: blob };

    const normalized: any = await (spectator.service as any)
      .normalizeErrorBody(err, Rest.ResponseType.Blob)
      .toPromise();

    expect(normalized.error).toBe(blob);
  });

  test('should swallow Blob.text() rejection and keep the original error in blob mode', async () => {
    const fakeBlob = {
      text: () => Promise.reject(new Error('boom')),
    } as unknown as Blob;
    const err: any = { error: fakeBlob, message: 'original' };

    Object.setPrototypeOf(fakeBlob, Blob.prototype);

    const normalized: any = await (spectator.service as any)
      .normalizeErrorBody(err, Rest.ResponseType.Blob)
      .toPromise();

    expect(normalized).toBe(err);
    expect(normalized.error).toBe(fakeBlob);
  });

  test('should leave non-JSON error body alone in text mode', async () => {
    const spy = vi.spyOn(httpErrorReporter, 'reportError');

    const completion = new Promise<void>((resolve, reject) => {
      spectator.service
        .request(
          { method: HttpMethod.GET, url: '/text' },
          { responseType: Rest.ResponseType.Text },
        )
        .pipe(
          catchError(() => {
            try {
              const errArg: any = spy.mock.calls[0][0];
              expect(errArg.error).toBe('plain error text');
              resolve();
            } catch (e) {
              reject(e);
            }
            return of(null);
          }),
        )
        .subscribe();
    });

    const req = spectator.expectOne(api + '/text', HttpMethod.GET);
    req.flush('plain error text', { status: 500, statusText: 'Internal Server Error' });

    await completion;
  });

  test('should send Accept header emitted by schematic verbatim (json scenario)', () => {
    spectator.service
      .request({
        method: HttpMethod.GET,
        url: '/status',
        headers: { Accept: 'application/json' },
      })
      .subscribe();
    const req = spectator.expectOne(api + '/status', HttpMethod.GET);
    expect(req.request.headers.get('Accept')).toEqual('application/json');
  });

  test('should send Accept header emitted by schematic verbatim (text scenario)', () => {
    spectator.service
      .request({ method: HttpMethod.GET, url: '/csv', headers: { Accept: 'text/plain' } })
      .subscribe();
    const req = spectator.expectOne(api + '/csv', HttpMethod.GET);
    expect(req.request.headers.get('Accept')).toEqual('text/plain');
  });

  test('should leave JSON error body alone in json mode (no double-parse)', async () => {
    const spy = vi.spyOn(httpErrorReporter, 'reportError');

    const completion = new Promise<void>((resolve, reject) => {
      spectator.service
        .request({ method: HttpMethod.GET, url: '/json' })
        .pipe(
          catchError(() => {
            try {
              const errArg: any = spy.mock.calls[0][0];
              expect(errArg.error).toEqual({ error: { code: 'X', message: 'y' } });
              resolve();
            } catch (e) {
              reject(e);
            }
            return of(null);
          }),
        )
        .subscribe();
    });

    const req = spectator.expectOne(api + '/json', HttpMethod.GET);
    req.flush(
      { error: { code: 'X', message: 'y' } },
      { status: 403, statusText: 'Forbidden' },
    );

    await completion;
  });

  test('should remove the duplicate slashes', () => {
    spectator.service
      .request({ method: HttpMethod.GET, url: '//test', params: { id: 1 } })
      .subscribe();
    spectator.expectOne(api + '/test?id=1', HttpMethod.GET);
  });
  test('should remove the duplicate slashes multiple', () => {
    spectator.service
      .request({ method: HttpMethod.GET, url: '//test//my//endpoint', params: { id: 1 } })
      .subscribe();
    spectator.expectOne(api + '/test/my/endpoint?id=1', HttpMethod.GET);
  });
});
