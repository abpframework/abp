import { makeEnvironmentProviders, APP_INITIALIZER, Injector, Provider } from '@angular/core';
import { TitleStrategy } from '@angular/router';
import {
  CORE_OPTIONS,
  LOCALIZATIONS,
  DYNAMIC_LAYOUTS_TOKEN,
  OTHERS_GROUP,
  QUEUE_MANAGER,
  SORT_COMPARE_FUNC,
  TENANT_KEY,
  compareFuncFactory,
  coreOptionsFactory,
  localizationContributor,
} from '../tokens';
import { RoutesHandler } from '../handlers';
import { ABP, SortableItem } from '../models';
import { AuthErrorFilterService } from '../abstracts';
import { DEFAULT_DYNAMIC_LAYOUTS } from '../constants';
import { LocalizationService, LocalStorageListenerService, AbpTitleStrategy } from '../services';
import { DefaultQueueManager, getInitialData, localeInitializer, noop } from '../utils';
import { CookieLanguageProvider, IncludeLocalizationResourcesProvider, LocaleProvider } from './';

export enum CoreFeatureKind {
  Options,
  CompareFunctionFactory,
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

export function withTitleStrategy(strategy: unknown): CoreFeature<CoreFeatureKind.TitleStrategy> {
  return makeCoreFeature(CoreFeatureKind.TitleStrategy, [
    {
      provide: TitleStrategy,
      useExisting: strategy,
    },
  ]);
}

export function withCompareFuncFactory(
  factory: (a: SortableItem, b: SortableItem) => 1 | -1 | 0,
): CoreFeature<CoreFeatureKind.CompareFunctionFactory> {
  return makeCoreFeature(CoreFeatureKind.CompareFunctionFactory, [
    {
      provide: SORT_COMPARE_FUNC,
      useFactory: factory,
    },
  ]);
}

export function provideAbpCore(...features: CoreFeature<CoreFeatureKind>[]) {
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

export function provideAbpCoreChild(options = {} as ABP.Child) {
  return makeEnvironmentProviders([
    {
      provide: LOCALIZATIONS,
      multi: true,
      useValue: localizationContributor(options.localizations),
      deps: [LocalizationService],
    },
  ]);
}
