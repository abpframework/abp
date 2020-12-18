import {
  ABP,
  BaseCoreModule,
  coreOptionsFactory,
  CORE_OPTIONS,
  LocalizationPipe,
} from '@abp/ng.core';
import { APP_BASE_HREF } from '@angular/common';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { provideRoutes } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { MockLocalizationPipe } from './pipes/mock-localization.pipe';

/**
 * CoreTestingModule is the module that will be used in tests
 * and it provides mock alternatives
 */
@NgModule({
  exports: [RouterTestingModule, BaseCoreModule, MockLocalizationPipe],
  imports: [RouterTestingModule, BaseCoreModule],
  declarations: [MockLocalizationPipe],
})
export class CoreTestingModule {
  static forTest(
    { baseHref = '/', routes = [], ...options } = {} as ABP.Test,
  ): ModuleWithProviders<CoreTestingModule> {
    return {
      ngModule: CoreTestingModule,
      providers: [
        { provide: APP_BASE_HREF, useValue: baseHref },
        {
          provide: 'CORE_OPTIONS',
          useValue: options,
        },
        {
          provide: CORE_OPTIONS,
          useFactory: coreOptionsFactory,
          deps: ['CORE_OPTIONS'],
        },
        {
          provide: LocalizationPipe,
          useClass: MockLocalizationPipe,
        },
        provideRoutes(routes),
      ],
    };
  }
}
