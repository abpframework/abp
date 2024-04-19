import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClientXsrfModule } from '@angular/common/http';
import { APP_INITIALIZER, Injector, ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, TitleStrategy } from '@angular/router';
import { AbstractNgModelComponent } from './abstracts/ng-model.component';
import { DynamicLayoutComponent } from './components/dynamic-layout.component';
import { ReplaceableRouteContainerComponent } from './components/replaceable-route-container.component';
import { RouterOutletComponent } from './components/router-outlet.component';
import { AutofocusDirective } from './directives/autofocus.directive';
import { InputEventDebounceDirective } from './directives/debounce.directive';
import { ForDirective } from './directives/for.directive';
import { FormSubmitDirective } from './directives/form-submit.directive';
import { InitDirective } from './directives/init.directive';
import { PermissionDirective } from './directives/permission.directive';
import { ReplaceableTemplateDirective } from './directives/replaceable-template.directive';
import { StopPropagationDirective } from './directives/stop-propagation.directive';
import { RoutesHandler } from './handlers/routes.handler';
import { LocalizationModule } from './localization.module';
import { ABP } from './models/common';
import { LocalizationPipe } from './pipes/localization.pipe';
import { SortPipe } from './pipes/sort.pipe';
import { ToInjectorPipe } from './pipes/to-injector.pipe';
import { CookieLanguageProvider } from './providers/cookie-language.provider';
import { LocaleProvider } from './providers/locale.provider';
import { LocalizationService } from './services/localization.service';
import { OTHERS_GROUP } from './tokens';
import { localizationContributor, LOCALIZATIONS } from './tokens/localization.token';
import { CORE_OPTIONS, coreOptionsFactory } from './tokens/options.token';
import { TENANT_KEY } from './tokens/tenant-key.token';
import { noop } from './utils/common-utils';
import './utils/date-extensions';
import { getInitialData, localeInitializer } from './utils/initial-utils';
import { ShortDateTimePipe } from './pipes/short-date-time.pipe';
import { ShortTimePipe } from './pipes/short-time.pipe';
import { ShortDatePipe } from './pipes/short-date.pipe';
import { SafeHtmlPipe } from './pipes/safe-html.pipe';
import { QUEUE_MANAGER } from './tokens/queue.token';
import { DefaultQueueManager } from './utils/queue';
import { IncludeLocalizationResourcesProvider } from './providers/include-localization-resources.provider';
import { SORT_COMPARE_FUNC, compareFuncFactory } from './tokens/compare-func.token';
import { AuthErrorFilterService } from './abstracts';
import { DYNAMIC_LAYOUTS_TOKEN } from "./tokens/dynamic-layout.token";
import { DEFAULT_DYNAMIC_LAYOUTS } from "./constants";
import { AbpTitleStrategy } from './services/title-strategy.service';
import { LocalStorageListenerService } from './services/local-storage-listener.service';


const standaloneDirectives = [
  AutofocusDirective,
  InputEventDebounceDirective,
  ForDirective,
  FormSubmitDirective,
  InitDirective,
  PermissionDirective,
  ReplaceableTemplateDirective,
  StopPropagationDirective,
];
/**
 * BaseCoreModule is the module that holds
 * all imports, declarations, exports, and entryComponents
 * but not the providers.
 * This module will be imported and exported by all others.
 */
@NgModule({
  exports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    LocalizationModule,
    AbstractNgModelComponent,
    DynamicLayoutComponent,
    ReplaceableRouteContainerComponent,
    RouterOutletComponent,
    SortPipe,
    SafeHtmlPipe,
    ToInjectorPipe,
    ShortDateTimePipe,
    ShortTimePipe,
    ShortDatePipe,
    ...standaloneDirectives,
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    LocalizationModule,
    ...standaloneDirectives,
  ],
  declarations: [
    AbstractNgModelComponent,
    DynamicLayoutComponent,
    ReplaceableRouteContainerComponent,
    RouterOutletComponent,
    SortPipe,
    SafeHtmlPipe,
    ToInjectorPipe,
    ShortDateTimePipe,
    ShortTimePipe,
    ShortDatePipe,
  ],
  providers: [LocalizationPipe],
})
export class BaseCoreModule {}

/**
 * RootCoreModule is the module that will be used at root level
 * and it introduces imports useful at root level (e.g. NGXS)
 */
@NgModule({
  exports: [BaseCoreModule, LocalizationModule],
  imports: [
    BaseCoreModule,
    LocalizationModule,
    HttpClientXsrfModule.withOptions({
      cookieName: 'XSRF-TOKEN',
      headerName: 'RequestVerificationToken',
    }),
  ],
})
export class RootCoreModule {}

/**
 * CoreModule is the module that is publicly available
 */
@NgModule({
  exports: [BaseCoreModule],
  imports: [BaseCoreModule],
})
export class CoreModule {
  static forRoot(options = {} as ABP.Root): ModuleWithProviders<RootCoreModule> {
    return {
      ngModule: RootCoreModule,
      providers: [
        LocaleProvider,
        CookieLanguageProvider,
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

        { provide: TENANT_KEY, useValue: options.tenantKey || '__tenant' },
        {
          provide: LOCALIZATIONS,
          multi: true,
          useValue: localizationContributor(options.localizations),
          deps: [LocalizationService],
        },
        {
          provide: SORT_COMPARE_FUNC,
          useFactory: compareFuncFactory,
        },
        {
          provide: QUEUE_MANAGER,
          useClass: DefaultQueueManager,
        },
        {
          provide: OTHERS_GROUP,
          useValue: options.othersGroup || 'AbpUi::OthersGroup',
        },
        AuthErrorFilterService,
        IncludeLocalizationResourcesProvider,
        {
          provide: DYNAMIC_LAYOUTS_TOKEN,
          useValue: options.dynamicLayouts || DEFAULT_DYNAMIC_LAYOUTS
        },
        {
          provide: TitleStrategy,
          useExisting: AbpTitleStrategy
        }
      ],
    };
  }

  static forChild(options = {} as ABP.Child): ModuleWithProviders<RootCoreModule> {
    return {
      ngModule: RootCoreModule,
      providers: [
        {
          provide: LOCALIZATIONS,
          multi: true,
          useValue: localizationContributor(options.localizations),
          deps: [LocalizationService],
        },
      ],
    };
  }
}
