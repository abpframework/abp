import { APP_INITIALIZER, ErrorHandler, makeEnvironmentProviders } from '@angular/core';
import { noop } from '@abp/ng.core';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import {
  VALIDATION_BLUEPRINTS,
  VALIDATION_MAP_ERRORS_FN,
  defaultMapErrorsFn,
  VALIDATION_VALIDATE_ON_SUBMIT,
} from '@ngx-validate/core';
import { DEFAULT_VALIDATION_BLUEPRINTS } from '../constants';
import { DocumentDirHandlerService } from '../handlers';
import { RootParams } from '../models';
import { THEME_SHARED_APPEND_CONTENT, HTTP_ERROR_CONFIG } from '../tokens';
import { CONFIRMATION_ICONS, DEFAULT_CONFIRMATION_ICONS } from '../tokens/confirmation-icons.token';
import { DateParserFormatter } from '../utils';
import { DEFAULT_HANDLERS_PROVIDERS } from './error-handlers.provider';
import { NG_BOOTSTRAP_CONFIG_PROVIDERS } from './ng-bootstrap-config.provider';
import { THEME_SHARED_ROUTE_PROVIDERS } from './route.provider';
import { tenantNotFoundProvider } from './tenant-not-found.provider';

export function provideThemeSharedConfig(
  { httpErrorConfig, validation = {}, confirmationIcons = {} } = {} as RootParams,
) {
  const providers = [
    {
      provide: APP_INITIALIZER,
      multi: true,
      deps: [ErrorHandler],
      useFactory: noop,
    },
    THEME_SHARED_ROUTE_PROVIDERS,
    {
      provide: APP_INITIALIZER,
      multi: true,
      deps: [THEME_SHARED_APPEND_CONTENT],
      useFactory: noop,
    },
    { provide: HTTP_ERROR_CONFIG, useValue: httpErrorConfig },
    { provide: NgbDateParserFormatter, useClass: DateParserFormatter },
    NG_BOOTSTRAP_CONFIG_PROVIDERS,
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
    DocumentDirHandlerService,
    {
      provide: APP_INITIALIZER,
      useFactory: noop,
      multi: true,
      deps: [DocumentDirHandlerService],
    },
    {
      provide: CONFIRMATION_ICONS,
      useValue: {
        ...DEFAULT_CONFIRMATION_ICONS,
        ...(confirmationIcons || {}),
      },
    },
    tenantNotFoundProvider,
    DEFAULT_HANDLERS_PROVIDERS,
  ];

  return makeEnvironmentProviders(providers);
}
