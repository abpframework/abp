import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { APP_INITIALIZER, Injector, ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgxsRouterPluginModule } from '@ngxs/router-plugin';
import { NgxsStoragePluginModule } from '@ngxs/storage-plugin';
import { NgxsModule, NGXS_PLUGINS } from '@ngxs/store';
import { OAuthModule } from 'angular-oauth2-oidc';
import { AbstractNgModelComponent } from './abstracts/ng-model.component';
import { DynamicLayoutComponent } from './components/dynamic-layout.component';
import { RouterOutletComponent } from './components/router-outlet.component';
import { AutofocusDirective } from './directives/autofocus.directive';
import { InputEventDebounceDirective } from './directives/debounce.directive';
import { EllipsisDirective } from './directives/ellipsis.directive';
import { ForDirective } from './directives/for.directive';
import { FormSubmitDirective } from './directives/form-submit.directive';
import { PermissionDirective } from './directives/permission.directive';
import { ClickEventStopPropagationDirective } from './directives/stop-propagation.directive';
import { VisibilityDirective } from './directives/visibility.directive';
import { ApiInterceptor } from './interceptors/api.interceptor';
import { ABP } from './models/common';
import { LocalizationPipe } from './pipes/localization.pipe';
import { SortPipe } from './pipes/sort.pipe';
import { ConfigPlugin, NGXS_CONFIG_PLUGIN_OPTIONS } from './plugins/config.plugin';
import { LocaleProvider } from './providers/locale.provider';
import { ConfigState } from './states/config.state';
import { ProfileState } from './states/profile.state';
import { SessionState } from './states/session.state';
import { getInitialData, localeInitializer } from './utils/initial-utils';

@NgModule({
  imports: [
    NgxsModule.forFeature([ProfileState, SessionState, ConfigState]),
    NgxsRouterPluginModule.forRoot(),
    NgxsStoragePluginModule.forRoot({ key: ['SessionState'] }),
    OAuthModule.forRoot(),
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
  ],
  declarations: [
    RouterOutletComponent,
    DynamicLayoutComponent,
    AutofocusDirective,
    EllipsisDirective,
    ForDirective,
    FormSubmitDirective,
    LocalizationPipe,
    SortPipe,
    PermissionDirective,
    VisibilityDirective,
    InputEventDebounceDirective,
    ClickEventStopPropagationDirective,
    AbstractNgModelComponent,
  ],
  exports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    RouterOutletComponent,
    DynamicLayoutComponent,
    AutofocusDirective,
    EllipsisDirective,
    ForDirective,
    FormSubmitDirective,
    LocalizationPipe,
    SortPipe,
    PermissionDirective,
    VisibilityDirective,
    InputEventDebounceDirective,
    LocalizationPipe,
    ClickEventStopPropagationDirective,
    AbstractNgModelComponent,
  ],
  providers: [LocalizationPipe],
  entryComponents: [RouterOutletComponent, DynamicLayoutComponent],
})
export class CoreModule {
  static forRoot(options = {} as ABP.Root): ModuleWithProviders {
    return {
      ngModule: CoreModule,
      providers: [
        LocaleProvider,
        {
          provide: NGXS_PLUGINS,
          useClass: ConfigPlugin,
          multi: true,
        },
        {
          provide: NGXS_CONFIG_PLUGIN_OPTIONS,
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
      ],
    };
  }
}
