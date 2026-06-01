import { makeEnvironmentProviders, Provider, provideAppInitializer, inject } from '@angular/core';
import { TitleStrategy } from '@angular/router';
import {
  provideHttpClient,
  withFetch,
  withInterceptors,
  withInterceptorsFromDi,
  withXsrfConfiguration,
} from '@angular/common/http';
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
import {
  LocalizationService,
  LocalStorageListenerService,
  AbpTitleStrategy,
  UILocalizationService,
} from '../services';
import { DefaultQueueManager, getInitialData } from '../utils';
import { CookieLanguageProvider, IncludeLocalizationResourcesProvider, LocaleProvider } from './';
import { timezoneInterceptor, transferStateInterceptor } from '../interceptors';

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
    provideHttpClient(
      withInterceptorsFromDi(),
      withXsrfConfiguration({
        cookieName: 'XSRF-TOKEN',
        headerName: 'RequestVerificationToken',
      }),
      withFetch(),
      withInterceptors([transferStateInterceptor, timezoneInterceptor]),
    ),
    provideAppInitializer(async () => {
      inject(LocalizationService);
      inject(LocalStorageListenerService);
      inject(RoutesHandler);
      // Initialize UILocalizationService if UI-only mode is enabled
      const options = inject(CORE_OPTIONS);
      if (options?.uiLocalization?.enabled) {
        inject(UILocalizationService);
      }
      await getInitialData();
    }),
    LocaleProvider,
    CookieLanguageProvider,
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
