import { createServiceFactory, SpectatorService } from '@ngneat/spectator/vitest';
import { firstValueFrom } from 'rxjs';
import { Environment } from '../models/environment';
import { EnvironmentService } from '../services/environment.service';

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

describe('Environment', () => {
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
    it('should return ENVIRONMENT_DATA', async () => {
      expect(environment.getEnvironment()).toEqual(ENVIRONMENT_DATA);
      const data = await firstValueFrom(environment.getEnvironment$());
      expect(data).toEqual(ENVIRONMENT_DATA);
    });
  });

  describe('#getApiUrl', () => {
    it('should return api url', async () => {
      expect(environment.getApiUrl('default')).toEqual(ENVIRONMENT_DATA.apis.default.url);
      const otherData = await firstValueFrom(environment.getApiUrl$('other'));
      expect(otherData).toEqual(ENVIRONMENT_DATA.apis.other.url);
      const yetAnotherData = await firstValueFrom(environment.getApiUrl$('yetAnother'));
      expect(yetAnotherData).toEqual(ENVIRONMENT_DATA.apis.default.url);
    });
  });
});
