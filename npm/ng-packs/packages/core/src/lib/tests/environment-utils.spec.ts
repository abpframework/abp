import { HttpClient } from '@angular/common/http';
import { Component, Injector } from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { BehaviorSubject } from 'rxjs';
import { Environment, RemoteEnv } from '../models/environment';
import { EnvironmentService } from '../services';
import { getRemoteEnv } from '../utils/environment-utils';
import { deepMerge } from '../utils/object-utils';

@Component({
  selector: 'abp-dummy',
  template: '',
})
export class DummyComponent {}

describe('EnvironmentUtils', () => {
  let spectator: Spectator<DummyComponent>;
  const createComponent = createComponentFactory({
    component: DummyComponent,
    mocks: [EnvironmentService, HttpClient],
  });

  beforeEach(() => (spectator = createComponent()));

  describe('#getRemoteEnv', () => {
    const environment: Environment = {
      production: false,
      hmr: false,
      application: {
        baseUrl: 'https://volosoft.com',
        name: 'MyProjectName',
        logoUrl: '',
      },
      remoteEnv: { url: '/assets/appsettings.json', mergeStrategy: 'deepmerge' },
      oAuthConfig: {
        issuer: 'https://api.volosoft.com',
        clientId: 'MyProjectName_App',
        dummyClientSecret: '1q2w3e*',
        scope: 'MyProjectName',
        oidc: false,
        requireHttps: true,
      },
      apis: {
        default: {
          url: 'https://api.volosoft.com',
        },
      },
    };

    const customEnv = {
      application: {
        baseUrl: 'https://custom-volosoft.com',
        name: 'Custom-MyProjectName',
        logoUrl: 'https://logourl/',
      },
      apis: {
        default: {
          url: 'https://test-api.volosoft.com',
        },
      },
    };

    const someEnv = { apiUrl: 'https://some-api-url' } as any;
    const customFn = (_, __) => someEnv;

    test.each`
      case           | strategy       | expected
      ${'null'}      | ${null}        | ${customEnv}
      ${'undefined'} | ${undefined}   | ${customEnv}
      ${'overwrite'} | ${'overwrite'} | ${customEnv}
      ${'deepmerge'} | ${'deepmerge'} | ${deepMerge(environment, customEnv)}
      ${'customFn'}  | ${customFn}    | ${someEnv}
    `(
      'should call the remoteEnv URL and dispatch the SetEnvironment action for case $case ',
      ({ strategy, expected }) => setupTestAndRun({ mergeStrategy: strategy }, expected),
    );

    function setupTestAndRun(strategy: Pick<RemoteEnv, 'mergeStrategy'>, expectedValue) {
      const injector = spectator.inject(Injector);
      const injectorSpy = jest.spyOn(injector, 'get');
      const http = spectator.inject(HttpClient);
      const requestSpy = jest.spyOn(http, 'request');
      const environmentService = spectator.inject(EnvironmentService);
      const setStateSpy = jest.spyOn(environmentService, 'setState');

      injectorSpy.mockReturnValueOnce(environmentService);
      injectorSpy.mockReturnValueOnce(http);
      injectorSpy.mockReturnValueOnce({});

      requestSpy.mockReturnValue(new BehaviorSubject(customEnv));

      environment.remoteEnv.mergeStrategy = strategy.mergeStrategy;
      getRemoteEnv(injector, environment);

      expect(requestSpy).toHaveBeenCalledWith('GET', '/assets/appsettings.json', { headers: {} });
      expect(setStateSpy).toHaveBeenCalledWith(expectedValue);
    }
  });
});
