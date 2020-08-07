import { HttpClient } from '@angular/common/http';
import { Component, Injector } from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { BehaviorSubject, of } from 'rxjs';
import { getRemoteEnv } from '../utils/environment-utils';
import { SetEnvironment } from '../actions/config.actions';

const environment = {
  production: false,
  hmr: false,
  application: {
    baseUrl: 'https://volosoft.com',
    name: 'MyProjectName',
    logoUrl: '',
  },
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

@Component({
  selector: 'abp-dummy',
  template: '',
})
export class DummyComponent {}

describe('EnvironmentUtils', () => {
  let spectator: Spectator<DummyComponent>;
  const createComponent = createComponentFactory({
    component: DummyComponent,
    mocks: [Store, HttpClient],
  });

  beforeEach(() => (spectator = createComponent()));

  describe('#getRemoteEnv', async () => {
    test('should call the remoteEnv URL and dispatch the SetEnvironment action ', async () => {
      const injector = spectator.inject(Injector);
      const injectorSpy = jest.spyOn(injector, 'get');
      const store = spectator.inject(Store);
      const dispatchSpy = jest.spyOn(store, 'dispatch');
      const http = spectator.inject(HttpClient);
      const requestSpy = jest.spyOn(http, 'request');

      injectorSpy.mockReturnValueOnce(http);
      injectorSpy.mockReturnValueOnce(store);

      requestSpy.mockReturnValue(new BehaviorSubject(environment));
      dispatchSpy.mockReturnValue(of(true));

      const partialEnv = { remoteEnv: { url: '/assets/appsettings.json' } };
      getRemoteEnv(injector, partialEnv);

      expect(requestSpy).toHaveBeenCalledWith('GET', '/assets/appsettings.json', { headers: {} });
      expect(dispatchSpy).toHaveBeenCalledWith(
        new SetEnvironment({ ...environment, ...partialEnv }),
      );
    });
  });
});
