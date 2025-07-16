import { CommonModule } from '@angular/common';
import {
  provideHttpClient,
  withInterceptorsFromDi,
  withXsrfConfiguration,
} from '@angular/common/http';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AbstractNgModelComponent } from './abstracts';
import {
  DynamicLayoutComponent,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
} from './components';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import {
  StopPropagationDirective,
  ReplaceableTemplateDirective,
  PermissionDirective,
  InitDirective,
  ForDirective,
  InputEventDebounceDirective,
  AutofocusDirective,
  FormSubmitDirective,
} from './directives';
import { ABP } from './models/common';
import './utils/date-extensions';
import { provideAbpCoreChild, provideAbpCore, withOptions } from './providers';
import {
  LazyLocalizationPipe,
  UtcToLocalPipe,
  SafeHtmlPipe,
  ShortDatePipe,
  ShortTimePipe,
  ShortDateTimePipe,
  ToInjectorPipe,
  SortPipe,
  LocalizationPipe,
} from './pipes';

const CORE_DIRECTIVES = [
  AutofocusDirective,
  InputEventDebounceDirective,
  ForDirective,
  FormSubmitDirective,
  InitDirective,
  PermissionDirective,
  ReplaceableTemplateDirective,
  StopPropagationDirective,
];

const CORE_PIPES = [
  LocalizationPipe,
  SortPipe,
  SafeHtmlPipe,
  ShortDateTimePipe,
  ShortTimePipe,
  ShortDatePipe,
  ToInjectorPipe,
  UtcToLocalPipe,
  LazyLocalizationPipe,
];

const CORE_COMPONENTS = [
  DynamicLayoutComponent,
  ReplaceableRouteContainerComponent,
  RouterOutletComponent,
  AbstractNgModelComponent,
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
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    NgxValidateCoreModule,
    ...CORE_DIRECTIVES,
    ...CORE_PIPES,
    ...CORE_COMPONENTS,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    NgxValidateCoreModule,
    ...CORE_DIRECTIVES,
    ...CORE_PIPES,
    ...CORE_COMPONENTS,
  ],
  declarations: [],
  providers: [LocalizationPipe, provideHttpClient(withInterceptorsFromDi())],
})
export class BaseCoreModule {}

/**
 * RootCoreModule is the module that will be used at root level
 * and it introduces imports useful at root level (e.g. NGXS)
 */
@NgModule({
  exports: [BaseCoreModule],
  imports: [BaseCoreModule],
  providers: [
    provideHttpClient(
      withXsrfConfiguration({
        cookieName: 'XSRF-TOKEN',
        headerName: 'RequestVerificationToken',
      }),
    ),
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
  /**
   * @deprecated forRoot method is deprecated, use `provideAbpCore` *function* for config settings.
   */
  static forRoot(options = {} as ABP.Root): ModuleWithProviders<RootCoreModule> {
    return {
      ngModule: RootCoreModule,
      providers: [provideAbpCore(withOptions(options))],
    };
  }

  /**
   * @deprecated forChild method is deprecated, use `provideAbpCoreChild` *function* for config settings.
   */
  static forChild(options = {} as ABP.Child): ModuleWithProviders<RootCoreModule> {
    return {
      ngModule: RootCoreModule,
      providers: [provideAbpCoreChild(options)],
    };
  }
}
