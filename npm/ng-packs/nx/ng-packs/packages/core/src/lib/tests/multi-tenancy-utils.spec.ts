import { Component } from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import clone from 'just-clone';
import { of } from 'rxjs';

import {
  CurrentTenantDto,
  FindTenantResultDto,
} from '../proxy/volo/abp/asp-net-core/mvc/multi-tenancy/models';
import { EnvironmentService, MultiTenancyService } from '../services';
import { parseTenantFromUrl } from '../utils';
import { TENANT_KEY } from '../tokens';

const environment = {
  production: false,
  hmr: false,
  application: {
    baseUrl: 'https://{0}.volosoft.com',
    name: 'MyProjectName',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://{0}.api.volosoft.com',
    redirectUri: 'https://{0}.volosoft.com',
    clientId: 'MyProjectName_App',
    responseType: 'code',
    scope: 'offline_access MyProjectName',
  },
  apis: {
    default: {
      url: 'https://{0}.api.volosoft.com',
    },
    abp: {
      url: 'https://api.volosoft.com/{0}',
    },
  },
};

const setHref = url => {
  global.window = Object.create(window);
  delete window.location;
  Object.defineProperty(window, 'location', {
    value: {
      href: url,
    },
  });
};

const testTenantKey = 'TEST_TENANT_KEY';

@Component({
  selector: 'abp-dummy',
  template: '',
})
export class DummyComponent {}

describe('MultiTenancyUtils', () => {
  let spectator: Spectator<DummyComponent>;
  const createComponent = createComponentFactory({
    component: DummyComponent,
    mocks: [EnvironmentService, MultiTenancyService],
    providers: [{ provide: TENANT_KEY, useValue: testTenantKey }],
  });

  beforeEach(() => (spectator = createComponent()));

  describe('#parseTenantFromUrl', () => {
    test('should get the tenancyName, set replaced environment and call the findTenantByName method of AbpTenantService', async () => {
      const environmentService = spectator.inject(EnvironmentService);
      const multiTenancyService = spectator.inject(MultiTenancyService);
      const setTenantByName = jest.spyOn(multiTenancyService, 'setTenantByName');
      const getEnvironmentSpy = jest.spyOn(environmentService, 'getEnvironment');
      const setStateSpy = jest.spyOn(environmentService, 'setState');

      getEnvironmentSpy.mockReturnValue(clone(environment));

      const testTenant: FindTenantResultDto = {
        name: 'abp',
        tenantId: '1',
        isActive: true,
        success: true,
      };

      setHref('https://abp.volosoft.com/');

      setTenantByName.mockReturnValue(of(testTenant));

      const mockInjector = {
        get: arg => {
          if (arg === EnvironmentService) return environmentService;
          if (arg === MultiTenancyService) return multiTenancyService;
        },
      };
      await parseTenantFromUrl(mockInjector);

      const replacedEnv = {
        ...environment,
        application: { ...environment.application, baseUrl: 'https://abp.volosoft.com' },
        oAuthConfig: {
          ...environment.oAuthConfig,
          issuer: 'https://abp.api.volosoft.com',
          redirectUri: 'https://abp.volosoft.com',
        },
        apis: {
          default: {
            url: 'https://abp.api.volosoft.com',
          },
          abp: {
            url: 'https://api.volosoft.com/abp',
          },
        },
      };

      expect(setStateSpy).toHaveBeenCalledWith(replacedEnv);
      expect(multiTenancyService.domainTenant).toEqual({
        id: testTenant.tenantId,
        name: testTenant.name,
        isAvailable: true,
      } as CurrentTenantDto);
    });
  });
});
