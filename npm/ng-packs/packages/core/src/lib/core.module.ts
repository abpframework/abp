import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClientModule, HttpClientXsrfModule } from '@angular/common/http';
import { APP_INITIALIZER, Injector, ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { OAuthModule, OAuthStorage } from 'angular-oauth2-oidc';
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
import { OAuthConfigurationHandler } from './handlers/oauth-configuration.handler';
import { RoutesHandler } from './handlers/routes.handler';
import { ApiInterceptor } from './interceptors/api.interceptor';
import { LocalizationModule } from './localization.module';
import { ABP } from './models/common';
import { LocalizationPipe } from './pipes/localization.pipe';
import { SortPipe } from './pipes/sort.pipe';
import { ToInjectorPipe } from './pipes/to-injector.pipe';
import { CookieLanguageProvider } from './providers/cookie-language.provider';
import { LocaleProvider } from './providers/locale.provider';
import { LocalizationService } from './services/localization.service';
import { oAuthStorage } from './strategies/auth-flow.strategy';
import { localizationContributor, LOCALIZATIONS } from './tokens/localization.token';
import { CORE_OPTIONS, coreOptionsFactory } from './tokens/options.token';
import { TENANT_KEY } from './tokens/tenant-key.token';
import { noop } from './utils/common-utils';
import './utils/date-extensions';
import { getInitialData, localeInitializer } from './utils/initial-utils';
import { ShortDateTimePipe } from './pipes/short-date-time.pipe';
import { ShortTimePipe } from './pipes/short-time.pipe';
import { ShortDatePipe } from './pipes/short-date.pipe';

export function storageFactory(): OAuthStorage {
  return oAuthStorage;
}

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
    AutofocusDirective,
    DynamicLayoutComponent,
    ForDirective,
    FormSubmitDirective,
    InitDirective,
    InputEventDebounceDirective,
    PermissionDirective,
    ReplaceableRouteContainerComponent,
    ReplaceableTemplateDirective,
    RouterOutletComponent,
    SortPipe,
    StopPropagationDirective,
    ToInjectorPipe,
    ShortDateTimePipe,
    ShortTimePipe,
    ShortDatePipe,
  ],
  imports: [
    OAuthModule,
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    LocalizationModule,
  ],
  declarations: [
    AbstractNgModelComponent,
    AutofocusDirective,
    DynamicLayoutComponent,
    ForDirective,
    FormSubmitDirective,
    InitDirective,
    InputEventDebounceDirective,
    PermissionDirective,
    ReplaceableRouteContainerComponent,
    ReplaceableTemplateDirective,
    RouterOutletComponent,
    SortPipe,
    StopPropagationDirective,
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
    OAuthModule,
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
        OAuthModule.forRoot().providers,
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
          provide: HTTP_INTERCEPTORS,
          useExisting: ApiInterceptor,
          multi: true,
        },
        {
          provide: APP_INITIALIZER,
          multi: true,
          deps: [OAuthConfigurationHandler],
          useFactory: noop,
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
          deps: [RoutesHandler],
          useFactory: noop,
        },
        { provide: OAuthStorage, useFactory: storageFactory },
        { provide: TENANT_KEY, useValue: options.tenantKey || '__tenant' },
        {
          provide: LOCALIZATIONS,
          multi: true,
          useValue: localizationContributor(options.localizations),
          deps: [LocalizationService],
        },
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
