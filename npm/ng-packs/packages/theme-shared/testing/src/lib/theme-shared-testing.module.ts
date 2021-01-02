import {
  BaseThemeSharedModule,
  DateParserFormatter,
  DEFAULT_VALIDATION_BLUEPRINTS,
  THEME_SHARED_ROUTE_PROVIDERS,
} from '@abp/ng.theme.shared';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { RouterTestingModule } from '@angular/router/testing';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import {
  defaultMapErrorsFn,
  VALIDATION_BLUEPRINTS,
  VALIDATION_MAP_ERRORS_FN,
  VALIDATION_VALIDATE_ON_SUBMIT,
} from '@ngx-validate/core';
import { Config } from './models/config';

/**
 * ThemeSharedTestingModule is the module that will be used in tests
 */
@NgModule({
  exports: [RouterTestingModule, BaseThemeSharedModule],
  imports: [RouterTestingModule, BaseThemeSharedModule],
})
export class ThemeSharedTestingModule {
  static withConfig(
    { validation = {} } = {} as Config,
  ): ModuleWithProviders<ThemeSharedTestingModule> {
    return {
      ngModule: ThemeSharedTestingModule,
      providers: [
        THEME_SHARED_ROUTE_PROVIDERS,
        { provide: NgbDateParserFormatter, useClass: DateParserFormatter },
        {
          provide: VALIDATION_BLUEPRINTS,
          useValue: {
            ...DEFAULT_VALIDATION_BLUEPRINTS,
            ...(validation.blueprints || {}),
          },
        },
        {
          provide: VALIDATION_MAP_ERRORS_FN,
          useValue: validation.mapErrorsFn || defaultMapErrorsFn,
        },
        {
          provide: VALIDATION_VALIDATE_ON_SUBMIT,
          useValue: validation.validateOnSubmit,
        },
      ],
    };
  }
}
