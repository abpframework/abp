import { Component } from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import clone from 'just-clone';
import { BehaviorSubject } from 'rxjs';
import { AbpTenantService } from '../proxy/pages/abp/multi-tenancy/abp-tenant.service';
import {
  CurrentTenantDto,
  FindTenantResultDto,
} from '../proxy/volo/abp/asp-net-core/mvc/multi-tenancy/models';
import { EnvironmentService, MultiTenancyService } from '../services';
import { parseTenantFromUrl } from '../utils';

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
    providers: [{ provide: AbpTenantService, useValue: { findTenantByName: () => {} } }],
  });

  beforeEach(() => (spectator = createComponent()));

  describe('#parseTenantFromUrl', () => {
    test('should get the tenancyName, set replaced environment and call the findTenantByName method of AbpTenantService', async () => {
      const environmentService = spectator.inject(EnvironmentService);
      const multiTenancyService = spectator.inject(MultiTenancyService);
      const abpTenantService = spectator.inject(AbpTenantService);
      const findTenantByNameSpy = jest.spyOn(abpTenantService, 'findTenantByName');
      const getEnvironmentSpy = jest.spyOn(environmentService, 'getEnvironment');
      const setStateSpy = jest.spyOn(environmentService, 'setState');

      getEnvironmentSpy.mockReturnValue(clone(environment));

      setHref('https://abp.volosoft.com/');

      findTenantByNameSpy.mockReturnValue(
        new BehaviorSubject({ name: 'abp', tenantId: '1', success: true } as FindTenantResultDto),
      );

      const mockInjector = {
        get: arg => {
          if (arg === EnvironmentService) return environmentService;
          if (arg === AbpTenantService) return abpTenantService;
          if (arg === MultiTenancyService) return multiTenancyService;
        },
      };
      parseTenantFromUrl(mockInjector);

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
      expect(findTenantByNameSpy).toHaveBeenCalledWith('abp', { __tenant: '' });
      expect(multiTenancyService.domainTenant).toEqual({
        id: '1',
        name: 'abp',
      } as CurrentTenantDto);
    });
  });
});
