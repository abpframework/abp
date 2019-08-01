import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { APP_INITIALIZER, Injector, ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgxsRouterPluginModule } from '@ngxs/router-plugin';
import { NgxsStoragePluginModule } from '@ngxs/storage-plugin';
import { NgxsModule, NGXS_PLUGINS } from '@ngxs/store';
import { DynamicLayoutComponent } from './components/dynamic-layout.component';
import { RouterOutletComponent } from './components/router-outlet.component';
import { PermissionDirective } from './directives/permission.directive';
import { VisibilityDirective } from './directives/visibility.directive';
import { ApiInterceptor } from './interceptors/api.interceptor';
import { ABP } from './models/common';
import { LocalizationPipe } from './pipes/localization.pipe';
import { ConfigPlugin, NGXS_CONFIG_PLUGIN_OPTIONS } from './plugins/config.plugin';
import { ConfigState } from './states/config.state';
import { ProfileState } from './states/profile.state';
import { SessionState } from './states/session.state';
import { getInitialData } from './utils/initial-utils';
import { EllipsisDirective } from './directives/ellipsis.directive';

@NgModule({
  imports: [
    NgxsModule.forFeature([ProfileState, SessionState, ConfigState]),
    NgxsStoragePluginModule.forRoot({ key: 'SessionState' }),
    NgxsRouterPluginModule.forRoot(),
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
  ],
  declarations: [
    RouterOutletComponent,
    DynamicLayoutComponent,
    PermissionDirective,
    VisibilityDirective,
    LocalizationPipe,
    EllipsisDirective,
  ],
  exports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    RouterOutletComponent,
    DynamicLayoutComponent,
    PermissionDirective,
    VisibilityDirective,
    EllipsisDirective,
    LocalizationPipe,
  ],
  providers: [LocalizationPipe],
  entryComponents: [RouterOutletComponent, DynamicLayoutComponent],
})
export class CoreModule {
  static forRoot(options = {} as ABP.Root): ModuleWithProviders {
    return {
      ngModule: CoreModule,
      providers: [
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
      ],
    };
  }
}
