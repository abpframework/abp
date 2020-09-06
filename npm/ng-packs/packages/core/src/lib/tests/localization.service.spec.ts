import { CORE_OPTIONS } from '../tokens/options.token';
import { Router } from '@angular/router';
import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { Actions, Store } from '@ngxs/store';
import { of, Subject } from 'rxjs';
import { LocalizationService } from '../services/localization.service';

describe('LocalizationService', () => {
  let spectator: SpectatorService<LocalizationService>;
  let store: SpyObject<Store>;
  let service: LocalizationService;

  const createService = createServiceFactory({
    service: LocalizationService,
    entryComponents: [],
    mocks: [Store, Router],
    providers: [
      { provide: Actions, useValue: new Subject() },
      { provide: CORE_OPTIONS, useValue: { cultureNameLocaleFileMap: {} } },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    store = spectator.inject(Store);
    service = spectator.service;
  });

  describe('#currentLang', () => {
    it('should be tr', () => {
      store.selectSnapshot.andCallFake((selector: (state: any, ...states: any[]) => string) => {
        return selector({ SessionState: { language: 'tr' } });
      });

      expect(service.currentLang).toBe('tr');
    });
  });

  describe('#get', () => {
    it('should be return an observable localization', async () => {
      store.select.andReturn(of('AbpTest'));

      const localization = await service.get('AbpTest').toPromise();

      expect(localization).toBe('AbpTest');
    });
  });

  describe('#instant', () => {
    it('should be return a localization', () => {
      store.selectSnapshot.andReturn('AbpTest');

      expect(service.instant('AbpTest')).toBe('AbpTest');
    });
  });

  describe('#registerLocale', () => {
    it('should return registerLocale and then call setRouteReuse', () => {
      const router = spectator.inject(Router);

      const shouldReuseRoute = () => true;
      router.routeReuseStrategy = { shouldReuseRoute } as any;

      router.navigateByUrl.andCallFake(url => {
        return new Promise(resolve => resolve({ catch: () => null }));
      });

      service.registerLocale('tr');

      expect(router.navigated).toBe(false);
      expect(router.routeReuseStrategy.shouldReuseRoute).not.toEqual(shouldReuseRoute);
    });

    it('should throw an error message when service have an otherInstance', async () => {
      try {
        const instance = new LocalizationService(new Subject(), null, null, null, {} as any);
      } catch (error) {
        expect((error as Error).message).toBe('LocalizationService should have only one instance.');
      }
    });
  });

  describe('#localize', () => {
    test.each`
      resource     | key          | defaultValue | expected
      ${'_'}       | ${'TEST'}    | ${'DEFAULT'} | ${'TEST'}
      ${'foo'}     | ${'bar'}     | ${'DEFAULT'} | ${'baz'}
      ${'x'}       | ${'bar'}     | ${'DEFAULT'} | ${'DEFAULT'}
      ${'a'}       | ${'bar'}     | ${'DEFAULT'} | ${'DEFAULT'}
      ${''}        | ${'bar'}     | ${'DEFAULT'} | ${'DEFAULT'}
      ${undefined} | ${'bar'}     | ${'DEFAULT'} | ${'DEFAULT'}
      ${'foo'}     | ${'y'}       | ${'DEFAULT'} | ${'DEFAULT'}
      ${'x'}       | ${'y'}       | ${'DEFAULT'} | ${'z'}
      ${'a'}       | ${'y'}       | ${'DEFAULT'} | ${'DEFAULT'}
      ${''}        | ${'y'}       | ${'DEFAULT'} | ${'DEFAULT'}
      ${undefined} | ${'y'}       | ${'DEFAULT'} | ${'DEFAULT'}
      ${'foo'}     | ${''}        | ${'DEFAULT'} | ${'DEFAULT'}
      ${'x'}       | ${''}        | ${'DEFAULT'} | ${'DEFAULT'}
      ${'a'}       | ${''}        | ${'DEFAULT'} | ${'DEFAULT'}
      ${''}        | ${''}        | ${'DEFAULT'} | ${'DEFAULT'}
      ${undefined} | ${''}        | ${'DEFAULT'} | ${'DEFAULT'}
      ${'foo'}     | ${undefined} | ${'DEFAULT'} | ${'DEFAULT'}
      ${'x'}       | ${undefined} | ${'DEFAULT'} | ${'DEFAULT'}
      ${'a'}       | ${undefined} | ${'DEFAULT'} | ${'DEFAULT'}
      ${''}        | ${undefined} | ${'DEFAULT'} | ${'DEFAULT'}
      ${undefined} | ${undefined} | ${'DEFAULT'} | ${'DEFAULT'}
    `(
      'should return observable $expected when resource name is $resource and key is $key',
      async ({ resource, key, defaultValue, expected }) => {
        store.select.andReturn(
          of({
            values: { foo: { bar: 'baz' }, x: { y: 'z' } },
            defaultResourceName: 'x',
          }),
        );

        const result = await service.localize(resource, key, defaultValue).toPromise();

        expect(result).toBe(expected);
      },
    );
  });

  describe('#localizeSync', () => {
    test.each`
      resource     | key          | defaultValue | expected
      ${'_'}       | ${'TEST'}    | ${'DEFAULT'} | ${'TEST'}
      ${'foo'}     | ${'bar'}     | ${'DEFAULT'} | ${'baz'}
      ${'x'}       | ${'bar'}     | ${'DEFAULT'} | ${'DEFAULT'}
      ${'a'}       | ${'bar'}     | ${'DEFAULT'} | ${'DEFAULT'}
      ${''}        | ${'bar'}     | ${'DEFAULT'} | ${'DEFAULT'}
      ${undefined} | ${'bar'}     | ${'DEFAULT'} | ${'DEFAULT'}
      ${'foo'}     | ${'y'}       | ${'DEFAULT'} | ${'DEFAULT'}
      ${'x'}       | ${'y'}       | ${'DEFAULT'} | ${'z'}
      ${'a'}       | ${'y'}       | ${'DEFAULT'} | ${'DEFAULT'}
      ${''}        | ${'y'}       | ${'DEFAULT'} | ${'DEFAULT'}
      ${undefined} | ${'y'}       | ${'DEFAULT'} | ${'DEFAULT'}
      ${'foo'}     | ${''}        | ${'DEFAULT'} | ${'DEFAULT'}
      ${'x'}       | ${''}        | ${'DEFAULT'} | ${'DEFAULT'}
      ${'a'}       | ${''}        | ${'DEFAULT'} | ${'DEFAULT'}
      ${''}        | ${''}        | ${'DEFAULT'} | ${'DEFAULT'}
      ${undefined} | ${''}        | ${'DEFAULT'} | ${'DEFAULT'}
      ${'foo'}     | ${undefined} | ${'DEFAULT'} | ${'DEFAULT'}
      ${'x'}       | ${undefined} | ${'DEFAULT'} | ${'DEFAULT'}
      ${'a'}       | ${undefined} | ${'DEFAULT'} | ${'DEFAULT'}
      ${''}        | ${undefined} | ${'DEFAULT'} | ${'DEFAULT'}
      ${undefined} | ${undefined} | ${'DEFAULT'} | ${'DEFAULT'}
    `(
      'should return $expected when resource name is $resource and key is $key',
      ({ resource, key, defaultValue, expected }) => {
        store.selectSnapshot.andReturn({
          values: { foo: { bar: 'baz' }, x: { y: 'z' } },
          defaultResourceName: 'x',
        });

        const result = service.localizeSync(resource, key, defaultValue);

        expect(result).toBe(expected);
      },
    );
  });

  describe('#localizeWithFallback', () => {
    test.each`
      resources          | keys                 | defaultValue | expected
      ${['', '_']}       | ${['TEST', 'OTHER']} | ${'DEFAULT'} | ${'TEST'}
      ${['foo']}         | ${['bar']}           | ${'DEFAULT'} | ${'baz'}
      ${['x']}           | ${['bar']}           | ${'DEFAULT'} | ${'DEFAULT'}
      ${['a', 'b', 'c']} | ${['bar']}           | ${'DEFAULT'} | ${'DEFAULT'}
      ${['']}            | ${['bar']}           | ${'DEFAULT'} | ${'DEFAULT'}
      ${[]}              | ${['bar']}           | ${'DEFAULT'} | ${'DEFAULT'}
      ${['foo']}         | ${['y']}             | ${'DEFAULT'} | ${'z'}
      ${['x']}           | ${['y']}             | ${'DEFAULT'} | ${'z'}
      ${['a', 'b', 'c']} | ${['y']}             | ${'DEFAULT'} | ${'z'}
      ${['']}            | ${['y']}             | ${'DEFAULT'} | ${'z'}
      ${[]}              | ${['y']}             | ${'DEFAULT'} | ${'z'}
      ${['foo']}         | ${['bar', 'y']}      | ${'DEFAULT'} | ${'baz'}
      ${['x']}           | ${['bar', 'y']}      | ${'DEFAULT'} | ${'z'}
      ${['a', 'b', 'c']} | ${['bar', 'y']}      | ${'DEFAULT'} | ${'z'}
      ${['']}            | ${['bar', 'y']}      | ${'DEFAULT'} | ${'z'}
      ${[]}              | ${['bar', 'y']}      | ${'DEFAULT'} | ${'z'}
      ${['foo']}         | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${['x']}           | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${['a', 'b', 'c']} | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${['']}            | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${[]}              | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${['foo']}         | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
      ${['x']}           | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
      ${['a', 'b', 'c']} | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
      ${['']}            | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
      ${[]}              | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
    `(
      'should return observable $expected when resource names are $resources and keys are $keys',
      async ({ resources, keys, defaultValue, expected }) => {
        store.select.andReturn(
          of({
            values: { foo: { bar: 'baz' }, x: { y: 'z' } },
            defaultResourceName: 'x',
          }),
        );

        const result = await service
          .localizeWithFallback(resources, keys, defaultValue)
          .toPromise();

        expect(result).toBe(expected);
      },
    );
  });

  describe('#localizeWithFallbackSync', () => {
    test.each`
      resources          | keys                 | defaultValue | expected
      ${['', '_']}       | ${['TEST', 'OTHER']} | ${'DEFAULT'} | ${'TEST'}
      ${['foo']}         | ${['bar']}           | ${'DEFAULT'} | ${'baz'}
      ${['x']}           | ${['bar']}           | ${'DEFAULT'} | ${'DEFAULT'}
      ${['a', 'b', 'c']} | ${['bar']}           | ${'DEFAULT'} | ${'DEFAULT'}
      ${['']}            | ${['bar']}           | ${'DEFAULT'} | ${'DEFAULT'}
      ${[]}              | ${['bar']}           | ${'DEFAULT'} | ${'DEFAULT'}
      ${['foo']}         | ${['y']}             | ${'DEFAULT'} | ${'z'}
      ${['x']}           | ${['y']}             | ${'DEFAULT'} | ${'z'}
      ${['a', 'b', 'c']} | ${['y']}             | ${'DEFAULT'} | ${'z'}
      ${['']}            | ${['y']}             | ${'DEFAULT'} | ${'z'}
      ${[]}              | ${['y']}             | ${'DEFAULT'} | ${'z'}
      ${['foo']}         | ${['bar', 'y']}      | ${'DEFAULT'} | ${'baz'}
      ${['x']}           | ${['bar', 'y']}      | ${'DEFAULT'} | ${'z'}
      ${['a', 'b', 'c']} | ${['bar', 'y']}      | ${'DEFAULT'} | ${'z'}
      ${['']}            | ${['bar', 'y']}      | ${'DEFAULT'} | ${'z'}
      ${[]}              | ${['bar', 'y']}      | ${'DEFAULT'} | ${'z'}
      ${['foo']}         | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${['x']}           | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${['a', 'b', 'c']} | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${['']}            | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${[]}              | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${['foo']}         | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
      ${['x']}           | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
      ${['a', 'b', 'c']} | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
      ${['']}            | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
      ${[]}              | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
    `(
      'should return $expected when resource names are $resources and keys are $keys',
      ({ resources, keys, defaultValue, expected }) => {
        store.selectSnapshot.andReturn({
          values: { foo: { bar: 'baz' }, x: { y: 'z' } },
          defaultResourceName: 'x',
        });

        const result = service.localizeWithFallbackSync(resources, keys, defaultValue);

        expect(result).toBe(expected);
      },
    );
  });
});
