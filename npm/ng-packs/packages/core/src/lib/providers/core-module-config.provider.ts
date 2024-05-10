import {
  ABP,
  AuthErrorFilterService,
  CORE_OPTIONS,
  DEFAULT_DYNAMIC_LAYOUTS,
  DefaultQueueManager,
  LOCALIZATIONS,
  LocalStorageListenerService,
  LocalizationService,
  OTHERS_GROUP,
  QUEUE_MANAGER,
  SORT_COMPARE_FUNC,
  TENANT_KEY,
  compareFuncFactory,
  coreOptionsFactory,
  getInitialData,
  localeInitializer,
  localizationContributor,
  noop,
} from '@abp/ng.core';
import { makeEnvironmentProviders, APP_INITIALIZER, Injector, Provider } from '@angular/core';
import { TitleStrategy } from '@angular/router';
import { RoutesHandler } from '../handlers';
import { AbpTitleStrategy } from '../services/title-strategy.service';
import { DYNAMIC_LAYOUTS_TOKEN } from '../tokens/dynamic-layout.token';
import { CookieLanguageProvider } from './cookie-language.provider';
import { IncludeLocalizationResourcesProvider } from './include-localization-resources.provider';
import { LocaleProvider } from './locale.provider';

export enum CoreFeatureKind {
  Options,
  compareFuncFactory,
  TitleStrategy,
}

export interface CoreFeature<KindT extends CoreFeatureKind> {
  ɵkind: KindT;
  ɵproviders: Provider[];
}

function makeCoreFeature<KindT extends CoreFeatureKind>(
  kind: KindT,
  providers: Provider[],
): CoreFeature<KindT> {
  return {
    ɵkind: kind,
    ɵproviders: providers,
  };
}

export function withOptions(options = {} as ABP.Root): CoreFeature<CoreFeatureKind.Options> {
  return makeCoreFeature(CoreFeatureKind.Options, [
    {
      provide: 'CORE_OPTIONS',
      useValue: options,
    },
    {
      provide: CORE_OPTIONS,
      useFactory: coreOptionsFactory,
      deps: ['CORE_OPTIONS'],
    },
    { provide: TENANT_KEY, useValue: options.tenantKey || '__tenant' },
    {
      provide: LOCALIZATIONS,
      multi: true,
      useValue: localizationContributor(options.localizations),
      deps: [LocalizationService],
    },
    {
      provide: OTHERS_GROUP,
      useValue: options.othersGroup || 'AbpUi::OthersGroup',
    },
    {
      provide: DYNAMIC_LAYOUTS_TOKEN,
      useValue: options.dynamicLayouts || DEFAULT_DYNAMIC_LAYOUTS,
    },
  ]);
}

export function withTitleStrategy(strategy: any): CoreFeature<CoreFeatureKind.TitleStrategy> {
  return makeCoreFeature(CoreFeatureKind.TitleStrategy, [
    {
      provide: TitleStrategy,
      useExisting: strategy,
    },
  ]);
}

export function withCompareFuncFactory(
  factory: any,
): CoreFeature<CoreFeatureKind.compareFuncFactory> {
  return makeCoreFeature(CoreFeatureKind.compareFuncFactory, [
    {
      provide: SORT_COMPARE_FUNC,
      useFactory: factory,
    },
  ]);
}

export function provideCoreModuleConfig(...features: CoreFeature<CoreFeatureKind>[]) {
  const providers = [
    LocaleProvider,
    CookieLanguageProvider,
    {
      provide: APP_INITIALIZER,
      multi: true,
      deps: [Injector],
      useFactory: getInitialData,
    },
    {
      provide: APP_INITIALIZER,
      multi: true,
      deps: [Injector],
      useFactory: localeInitializer,
    },
    {
      provide: APP_INITIALIZER,
      multi: true,
      deps: [LocalizationService],
      useFactory: noop,
    },
    {
      provide: APP_INITIALIZER,
      multi: true,
      deps: [LocalStorageListenerService],
      useFactory: noop,
    },
    {
      provide: APP_INITIALIZER,
      multi: true,
      deps: [RoutesHandler],
      useFactory: noop,
    },
    {
      provide: SORT_COMPARE_FUNC,
      useFactory: compareFuncFactory,
    },
    {
      provide: QUEUE_MANAGER,
      useClass: DefaultQueueManager,
    },
    AuthErrorFilterService,
    IncludeLocalizationResourcesProvider,
    {
      provide: TitleStrategy,
      useExisting: AbpTitleStrategy,
    },
  ];

  for (const feature of features) {
    providers.push(...feature.ɵproviders);
  }

  return makeEnvironmentProviders(providers);
}

export function provideCoreModuleConfigChild(options = {} as ABP.Child) {
  return makeEnvironmentProviders([
    {
      provide: LOCALIZATIONS,
      multi: true,
      useValue: localizationContributor(options.localizations),
      deps: [LocalizationService],
    },
  ]);
}
