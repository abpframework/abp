import { APP_BASE_HREF, CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { APP_INITIALIZER, Injector, ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgxsRouterPluginModule } from '@ngxs/router-plugin';
import { NgxsStoragePluginModule } from '@ngxs/storage-plugin';
import { NgxsModule, NGXS_PLUGINS } from '@ngxs/store';
import { OAuthModule, OAuthStorage } from 'angular-oauth2-oidc';
import { AbstractNgModelComponent } from './abstracts/ng-model.component';
import { DynamicLayoutComponent } from './components/dynamic-layout.component';
import { ReplaceableRouteContainerComponent } from './components/replaceable-route-container.component';
import { RouterOutletComponent } from './components/router-outlet.component';
import { AutofocusDirective } from './directives/autofocus.directive';
import { InputEventDebounceDirective } from './directives/debounce.directive';
import { EllipsisDirective } from './directives/ellipsis.directive';
import { ForDirective } from './directives/for.directive';
import { FormSubmitDirective } from './directives/form-submit.directive';
import { InitDirective } from './directives/init.directive';
import { PermissionDirective } from './directives/permission.directive';
import { ReplaceableTemplateDirective } from './directives/replaceable-template.directive';
import { StopPropagationDirective } from './directives/stop-propagation.directive';
import { VisibilityDirective } from './directives/visibility.directive';
import { ApiInterceptor } from './interceptors/api.interceptor';
import { LocalizationModule } from './localization.module';
import { ABP } from './models/common';
import { LocalizationPipe, MockLocalizationPipe } from './pipes/localization.pipe';
import { SortPipe } from './pipes/sort.pipe';
import { ConfigPlugin, NGXS_CONFIG_PLUGIN_OPTIONS } from './plugins/config.plugin';
import { LocaleProvider } from './providers/locale.provider';
import { LocalizationService } from './services/localization.service';
import { ConfigState } from './states/config.state';
import { ProfileState } from './states/profile.state';
import { ReplaceableComponentsState } from './states/replaceable-components.state';
import { SessionState } from './states/session.state';
import { CORE_OPTIONS } from './tokens/options.token';
import { noop } from './utils/common-utils';
import './utils/date-extensions';
import { getInitialData, localeInitializer } from './utils/initial-utils';

export function storageFactory(): OAuthStorage {
  return localStorage;
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

    AbstractNgModelComponent,
    AutofocusDirective,
    DynamicLayoutComponent,
    EllipsisDirective,
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
    VisibilityDirective,
  ],
  imports: [
    OAuthModule,
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
  ],
  declarations: [
    AbstractNgModelComponent,
    AutofocusDirective,
    DynamicLayoutComponent,
    EllipsisDirective,
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
    VisibilityDirective,
  ],
  entryComponents: [
    RouterOutletComponent,
    DynamicLayoutComponent,
    ReplaceableRouteContainerComponent,
  ],
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
    NgxsModule.forFeature([ReplaceableComponentsState, ProfileState, SessionState, ConfigState]),
    NgxsRouterPluginModule.forRoot(),
    NgxsStoragePluginModule.forRoot({ key: ['SessionState'] }),
  ],
})
export class RootCoreModule {}

/**
 * TestCoreModule is the module that will be used in tests
 * and it provides mock alternatives
 */
@NgModule({
  exports: [RouterModule, BaseCoreModule, MockLocalizationPipe],
  imports: [RouterModule.forRoot([]), BaseCoreModule],
  declarations: [MockLocalizationPipe],
})
export class TestCoreModule {}

/**
 * CoreModule is the module that is publicly available
 */
@NgModule({
  exports: [BaseCoreModule, LocalizationModule],
  imports: [BaseCoreModule, LocalizationModule],
  providers: [LocalizationPipe],
})
export class CoreModule {
  static forTest({ baseHref = '/' } = {} as ABP.Test): ModuleWithProviders<TestCoreModule> {
    return {
      ngModule: TestCoreModule,
      providers: [
        { provide: APP_BASE_HREF, useValue: baseHref },
        {
          provide: LocalizationPipe,
          useClass: MockLocalizationPipe,
        },
      ],
    };
  }

  static forRoot(options = {} as ABP.Root): ModuleWithProviders<RootCoreModule> {
    return {
      ngModule: RootCoreModule,
      providers: [
        LocaleProvider,
        {
          provide: NGXS_PLUGINS,
          useClass: ConfigPlugin,
          multi: true,
        },
        {
          provide: NGXS_CONFIG_PLUGIN_OPTIONS,
          useValue: { environment: options.environment },
        },
        {
          provide: CORE_OPTIONS,
          useValue: options,
        },
        {
          provide: HTTP_INTERCEPTORS,
          useClass: ApiInterceptor,
          multi: true,
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
        ...OAuthModule.forRoot().providers,
        { provide: OAuthStorage, useFactory: storageFactory },
      ],
    };
  }
}
