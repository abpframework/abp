import { Injector } from '@angular/core';
import { Router } from '@angular/router';
import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { of } from 'rxjs';
import { AbpApplicationConfigurationService } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/abp-application-configuration.service';
import { ConfigStateService, SessionStateService } from '../services';
import { LocalizationService } from '../services/localization.service';
import { CORE_OPTIONS } from '../tokens/options.token';
import { CONFIG_STATE_DATA } from './config-state.service.spec';

describe('LocalizationService', () => {
  let spectator: SpectatorService<LocalizationService>;
  let sessionState: SpyObject<SessionStateService>;
  let configState: SpyObject<ConfigStateService>;
  let service: LocalizationService;

  const createService = createServiceFactory({
    service: LocalizationService,
    entryComponents: [],
    mocks: [Router],
    providers: [
      {
        provide: CORE_OPTIONS,
        useValue: { registerLocaleFn: () => Promise.resolve(), cultureNameLocaleFileMap: {} },
      },
      {
        provide: AbpApplicationConfigurationService,
        useValue: { get: () => of(CONFIG_STATE_DATA) },
      },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    sessionState = spectator.inject(SessionStateService);
    configState = spectator.inject(ConfigStateService);
    service = spectator.service;

    configState.setState(CONFIG_STATE_DATA);
    sessionState.setLanguage('tr');
  });

  describe('#currentLang', () => {
    it('should be tr', done => {
      setTimeout(() => {
        expect(service.currentLang).toBe('tr');
        done();
      }, 0);
    });
  });

  describe('#get', () => {
    it('should be return an observable localization', done => {
      service.get('AbpIdentity::Identity').subscribe(localization => {
        expect(localization).toBe(CONFIG_STATE_DATA.localization.values.AbpIdentity.Identity);
        done();
      });
    });
  });

  describe('#instant', () => {
    it('should be return a localization', () => {
      const localization = service.instant('AbpIdentity::Identity');

      expect(localization).toBe(CONFIG_STATE_DATA.localization.values.AbpIdentity.Identity);
    });
  });

  describe('#registerLocale', () => {
    it('should throw an error message when service have an otherInstance', async () => {
      try {
        const instance = new LocalizationService(
          sessionState,
          spectator.inject(Injector),
          null,
          null,
        );
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
        configState.setState({
          localization: {
            values: { foo: { bar: 'baz' }, x: { y: 'z' } },
            defaultResourceName: 'x',
          },
        });

        service.localize(resource, key, defaultValue).subscribe(result => {
          expect(result).toBe(expected);
        });
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
        configState.setState({
          localization: {
            values: { foo: { bar: 'baz' }, x: { y: 'z' } },
            defaultResourceName: 'x',
          },
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
        configState.setState({
          localization: {
            values: { foo: { bar: 'baz' }, x: { y: 'z' } },
            defaultResourceName: 'x',
          },
        });

        service.localizeWithFallback(resources, keys, defaultValue).subscribe(result => {
          expect(result).toBe(expected);
        });
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
        configState.setState({
          localization: {
            values: { foo: { bar: 'baz' }, x: { y: 'z' } },
            defaultResourceName: 'x',
          },
        });

        const result = service.localizeWithFallbackSync(resources, keys, defaultValue);

        expect(result).toBe(expected);
      },
    );
  });

  describe('#getLocalization', () => {
    it('should return a localization', () => {
      expect(
        service.instant("MyProjectName::'{0}' and '{1}' do not match.", 'first', 'second'),
      ).toBe('first and second do not match.');
    });
  });
});
