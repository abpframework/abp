import { waitForAsync } from '@angular/core/testing';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { Environment } from '../models';
import { EnvironmentService } from '../services';

export const ENVIRONMENT_DATA = {
  production: false,
  application: {
    name: 'MyProjectName',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44305',
  },
  apis: {
    default: {
      url: 'https://localhost:44305',
    },
    other: {
      url: 'https://localhost:44306',
    },
    yetAnother: {},
  },
  localization: {
    defaultResourceName: 'MyProjectName',
  },
} as any as Environment;

describe('ConfigState', () => {
  let spectator: SpectatorService<EnvironmentService>;
  let environment: EnvironmentService;

  const createService = createServiceFactory({
    service: EnvironmentService,
  });

  beforeEach(() => {
    spectator = createService();
    environment = spectator.service;

    environment.setState(ENVIRONMENT_DATA);
  });

  describe('#getEnvironment', () => {
    it(
      'should return ENVIRONMENT_DATA',
      waitForAsync(() => {
        expect(environment.getEnvironment()).toEqual(ENVIRONMENT_DATA);
        environment.getEnvironment$().subscribe(data => expect(data).toEqual(ENVIRONMENT_DATA));
      }),
    );
  });

  describe('#getApiUrl', () => {
    it(
      'should return api url',
      waitForAsync(() => {
        expect(environment.getApiUrl()).toEqual(ENVIRONMENT_DATA.apis.default.url);
        environment
          .getApiUrl$('other')
          .subscribe(data => expect(data).toEqual(ENVIRONMENT_DATA.apis.other.url));
        environment
          .getApiUrl$('yetAnother')
          .subscribe(data => expect(data).toEqual(ENVIRONMENT_DATA.apis.default.url));
      }),
    );
  });

  // TODO: create permission.service.spec.ts
  // describe('#getGrantedPolicy', () => {
  //   it('should return a granted policy', () => {
  //     expect(ConfigState.getGrantedPolicy('Abp.Identity')(CONFIG_STATE_DATA)).toBe(false);
  //     expect(ConfigState.getGrantedPolicy('Abp.Identity || Abp.Account')(CONFIG_STATE_DATA)).toBe(
  //       true,
  //     );
  //     expect(ConfigState.getGrantedPolicy('Abp.Account && Abp.Identity')(CONFIG_STATE_DATA)).toBe(
  //       false,
  //     );
  //     expect(ConfigState.getGrantedPolicy('Abp.Account &&')(CONFIG_STATE_DATA)).toBe(false);
  //     expect(ConfigState.getGrantedPolicy('|| Abp.Account')(CONFIG_STATE_DATA)).toBe(false);
  //     expect(ConfigState.getGrantedPolicy('')(CONFIG_STATE_DATA)).toBe(true);
  //   });
  // });
});
