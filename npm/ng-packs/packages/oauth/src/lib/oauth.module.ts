import { APP_INITIALIZER, Injector, ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OAuthModule, OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { AuthGuard, AuthService, noop } from '@abp/ng.core';
import { storageFactory } from './utils/storage.factory';
import { AbpOAuthService, TimeoutLimitedOAuthService } from './services';
import { OAuthConfigurationHandler } from './handlers/oauth-configuration.handler';
import { initFactory } from './utils/init-factory';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ApiInterceptor } from './interceptors/api.interceptor';
import { AbpOAuthGuard } from './guards/oauth.guard';

@NgModule({
  imports: [CommonModule, OAuthModule],
})
export class AbpOAuthModule {
  static forRoot(): ModuleWithProviders<AbpOAuthModule> {
    return {
      ngModule: AbpOAuthModule,
      providers: [
        {
          provide: AuthService,
          useClass: AbpOAuthService,
        },
        {
          provide: AuthGuard,
          useClass: AbpOAuthGuard,
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
          useFactory: initFactory,
        },

        OAuthModule.forRoot().providers,
        { provide: OAuthStorage, useFactory: storageFactory },
        // {provide: OAuthService, useClass: TimeoutLimitedOAuthService}
      ],
    };
  }
}
