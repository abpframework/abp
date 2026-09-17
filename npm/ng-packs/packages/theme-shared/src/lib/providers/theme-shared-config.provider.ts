import { Provider, makeEnvironmentProviders, inject, provideAppInitializer } from '@angular/core';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import {
  VALIDATION_BLUEPRINTS,
  VALIDATION_MAP_ERRORS_FN,
  defaultMapErrorsFn,
  VALIDATION_VALIDATE_ON_SUBMIT,
  Validation,
} from '@ngx-validate/core';
import { DEFAULT_VALIDATION_BLUEPRINTS } from '../constants';
import { DocumentDirHandlerService, ErrorHandler } from '../handlers';
import { HttpErrorConfig } from '../models';
import {
  THEME_SHARED_APPEND_CONTENT,
  HTTP_ERROR_CONFIG,
  CONFIRMATION_ICONS,
  ConfirmationIcons,
  DEFAULT_CONFIRMATION_ICONS,
} from '../tokens';
import { DateParserFormatter } from '../utils';
import {
  DEFAULT_HANDLERS_PROVIDERS,
  NG_BOOTSTRAP_CONFIG_PROVIDERS,
  THEME_SHARED_ROUTE_PROVIDERS,
  tenantNotFoundProvider,
} from './';

export enum ThemeSharedFeatureKind {
  HttpErrorConfig,
  ValidationBluePrint,
  ValidationErrorsFn,
  ValidateOnSubmit,
  Validation,
  ConfirmationIcons,
}

export interface ThemeSharedFeature<KindT extends ThemeSharedFeatureKind> {
  ɵkind: KindT;
  ɵproviders: Provider[];
}

function makeThemeSharedFeature<KindT extends ThemeSharedFeatureKind>(
  kind: KindT,
  providers: Provider[],
): ThemeSharedFeature<KindT> {
  return {
    ɵkind: kind,
    ɵproviders: providers,
  };
}

export function withHttpErrorConfig(
  httpErrorConfig: HttpErrorConfig,
): ThemeSharedFeature<ThemeSharedFeatureKind.HttpErrorConfig> {
  return makeThemeSharedFeature(ThemeSharedFeatureKind.HttpErrorConfig, [
    {
      provide: HTTP_ERROR_CONFIG,
      useValue: httpErrorConfig,
    },
  ]);
}

export function withValidationBluePrint(
  bluePrints: Validation.Blueprints,
): ThemeSharedFeature<ThemeSharedFeatureKind.ValidationBluePrint> {
  return makeThemeSharedFeature(ThemeSharedFeatureKind.ValidationBluePrint, [
    {
      provide: VALIDATION_BLUEPRINTS,
      useValue: {
        ...DEFAULT_VALIDATION_BLUEPRINTS,
        ...(bluePrints || {}),
      },
    },
  ]);
}

export function withValidationMapErrorsFn(
  mapErrorsFn: Validation.MapErrorsFn,
): ThemeSharedFeature<ThemeSharedFeatureKind.ValidationErrorsFn> {
  return makeThemeSharedFeature(ThemeSharedFeatureKind.ValidationErrorsFn, [
    {
      provide: VALIDATION_MAP_ERRORS_FN,
      useValue: mapErrorsFn || defaultMapErrorsFn,
    },
  ]);
}

export function withValidateOnSubmit(
  validateOnSubmit: boolean,
): ThemeSharedFeature<ThemeSharedFeatureKind.ValidateOnSubmit> {
  return makeThemeSharedFeature(ThemeSharedFeatureKind.ValidateOnSubmit, [
    {
      provide: VALIDATION_VALIDATE_ON_SUBMIT,
      useValue: validateOnSubmit,
    },
  ]);
}

export function withConfirmationIcon(
  confirmationIcons: Partial<ConfirmationIcons>,
): ThemeSharedFeature<ThemeSharedFeatureKind.HttpErrorConfig> {
  return makeThemeSharedFeature(ThemeSharedFeatureKind.HttpErrorConfig, [
    {
      provide: CONFIRMATION_ICONS,
      useValue: { ...DEFAULT_CONFIRMATION_ICONS, ...(confirmationIcons || {}) },
    },
  ]);
}

export function provideAbpThemeShared(...features: ThemeSharedFeature<ThemeSharedFeatureKind>[]) {
  const providers = [
    provideAppInitializer(() => {
      inject(ErrorHandler);
      inject(THEME_SHARED_APPEND_CONTENT);
      inject(DocumentDirHandlerService);
    }),
    THEME_SHARED_ROUTE_PROVIDERS,
    { provide: HTTP_ERROR_CONFIG, useValue: undefined },
    { provide: NgbDateParserFormatter, useClass: DateParserFormatter },
    NG_BOOTSTRAP_CONFIG_PROVIDERS,
    {
      provide: VALIDATION_BLUEPRINTS,
      useValue: { ...DEFAULT_VALIDATION_BLUEPRINTS },
    },
    {
      provide: VALIDATION_MAP_ERRORS_FN,
      useValue: defaultMapErrorsFn,
    },
    {
      provide: VALIDATION_VALIDATE_ON_SUBMIT,
      useValue: undefined,
    },
    DocumentDirHandlerService,
    {
      provide: CONFIRMATION_ICONS,
      useValue: { ...DEFAULT_CONFIRMATION_ICONS },
    },
    tenantNotFoundProvider,
    DEFAULT_HANDLERS_PROVIDERS,
  ];

  for (const feature of features) {
    providers.push(...feature.ɵproviders);
  }

  return makeEnvironmentProviders(providers);
}
