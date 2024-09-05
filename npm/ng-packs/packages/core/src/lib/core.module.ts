import { CommonModule } from '@angular/common';
import {
  provideHttpClient,
  withInterceptorsFromDi,
  withXsrfConfiguration,
} from '@angular/common/http';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
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
import { LocalizationModule } from './localization.module';
import { ABP } from './models/common';
import { LocalizationPipe } from './pipes/localization.pipe';
import { SortPipe } from './pipes/sort.pipe';
import { ToInjectorPipe } from './pipes/to-injector.pipe';
import './utils/date-extensions';
import { ShortDateTimePipe } from './pipes/short-date-time.pipe';
import { ShortTimePipe } from './pipes/short-time.pipe';
import { ShortDatePipe } from './pipes/short-date.pipe';
import { SafeHtmlPipe } from './pipes/safe-html.pipe';
import { provideAbpCoreChild, provideAbpCore, withOptions } from './providers';

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
  providers: [LocalizationPipe, provideHttpClient(withInterceptorsFromDi())],
})
export class BaseCoreModule {}

/**
 * RootCoreModule is the module that will be used at root level
 * and it introduces imports useful at root level (e.g. NGXS)
 */
@NgModule({
  exports: [BaseCoreModule, LocalizationModule],
  imports: [BaseCoreModule, LocalizationModule],
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
