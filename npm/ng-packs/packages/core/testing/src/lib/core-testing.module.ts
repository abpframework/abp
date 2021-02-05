import {
  ABP,
  BaseCoreModule,
  coreOptionsFactory,
  CORE_OPTIONS,
  LIST_QUERY_DEBOUNCE_TIME,
  LOADER_DELAY,
  PermissionService,
  RestService,
} from '@abp/ng.core';
import { APP_BASE_HREF } from '@angular/common';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { provideRoutes } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { MockPermissionService } from './services/mock-permission.service';
import { MockRestService } from './services/mock-rest.service';

/**
 * CoreTestingModule is the module that will be used in tests
 * and it provides mock alternatives
 */
@NgModule({
  exports: [RouterTestingModule, BaseCoreModule],
  imports: [NoopAnimationsModule, RouterTestingModule, BaseCoreModule],
})
export class CoreTestingModule {
  static withConfig(
    { baseHref = '/', listQueryDebounceTime = 0, routes = [], ...options } = {} as ABP.Test,
  ): ModuleWithProviders<CoreTestingModule> {
    return {
      ngModule: CoreTestingModule,
      providers: [
        { provide: APP_BASE_HREF, useValue: baseHref },
        {
          provide: 'CORE_OPTIONS',
          useValue: {
            skipGetAppConfiguration: true,
            ...options,
          },
        },
        {
          provide: CORE_OPTIONS,
          useFactory: coreOptionsFactory,
          deps: ['CORE_OPTIONS'],
        },
        {
          provide: LIST_QUERY_DEBOUNCE_TIME,
          useValue: listQueryDebounceTime,
        },
        {
          provide: PermissionService,
          useClass: MockPermissionService,
        },
        {
          provide: RestService,
          useClass: MockRestService,
        },
        {
          provide: LOADER_DELAY,
          useValue: 0,
        },
        provideRoutes(routes),
      ],
    };
  }
}
